#r "nuget: Newtonsoft.Json, 12.0.2"
#load "../utils/Arguments/ArgumentBuilder.csx"
#load "../utils/Utils.csx"

using System.Net.Http;
using static System.Console;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;
using static System.Environment;

var help = $@"
create gitlab snippets

Usage: gitlab-snippets [options]

Options:
    --domain        api domain, default is https://gitlab.com 
    --token         token of api authentication
    --title         snippet's title
    --description   snippet's description
    --visibility    snippet's visibility, avaliable values: public, internal, private

    --directory     find dir
    --recursive     recursive directory or not
    --file-match    file full path filter pattern

    --files         files path, split by comma

Examples:

    # push two files to gitlab snippets
    gitlab-snippets --token your_token_value --files ./file1.txt,./file2.txt    
    
    # push all .sh/.bat files under ./dir to gitlab snippets
    gitlab-snippets --token your_token_value --directory ./dir --recurisive true --file-match ""\.(sh|bat)$"" 
";
if (Args.Count == 0 || Args.IndexOf("--help") != -1)
{
    WriteLine(help);
    return;
}
(bool IsValid, Options Instance, IEnumerable<String> Errors) valid = ArgumentBuilder.Build<Options>(Args).GetValidArgs();

if (!valid.IsValid)
{
    valid.Errors.ToList().ForEach(WriteLine);
    return;
}

var opt = valid.Instance;
IList<String> files = new List<string>();
if (opt.Directory != null)
    files = DirFiles(opt.Directory, opt.Recursive, opt.FileMatch).ToList();

if (opt.Files != null)
{
    opt.Files.Split(",")
    .Where(f => !String.IsNullOrWhiteSpace(f))
    .Select(Path.GetFullPath)
    .Where(File.Exists)
    .ToList()
    .ForEach(f => files.Add(f));
}
if (files.Count == 0)
{
    WriteLine("Not found any file.");
    return;
}
var reqData = new Dictionary<String, String>();
reqData["title"] = opt.Title;
reqData["file_name"] = "files.md";
reqData["description"] = opt.Description;
reqData["visibility"] = opt.Visibility;
reqData["content"] = files.Where(File.Exists)
                            .Select(f => new { Path = f, Content = File.ReadAllText(f) })
                            .Aggregate(new StringBuilder(), (prev, next) =>
                            {
                                var ext = Path.GetExtension(next.Path).TrimStart('.');
                                prev.Append($"{NewLine}# ${next.Path}{NewLine}```{ext}{NewLine}{next.Content} {NewLine}``` {NewLine}");
                                WriteLine($"Append file {next.Path}");
                                return prev;
                            })
                            .ToString();

var client = new HttpClient();
var req = new HttpRequestMessage(HttpMethod.Post, $"{opt.Domain}/api/v4/snippets");
req.Content = new StringContent(JsonConvert.SerializeObject(reqData), Encoding.UTF8, "application/json");
req.Headers.Add("PRIVATE-TOKEN", opt.Token);
var rep = await client.SendAsync(req);
var repData = JsonConvert.DeserializeObject<CreateResponse>(await rep.Content.ReadAsStringAsync());
WriteLine($"{rep.StatusCode}: {repData.web_url ?? repData.message}");

public class Options
{
    [Argument(Default = "https://gitlab.com", Required = true)]
    public String Domain { get; set; }

    [Argument(Required = true)]
    public String Token { get; set; }

    [Argument(Default = "gitlab-snippets")]
    public String Title { get; set; }

    [Argument(Default = "gitlab-snippets")]
    public String Description { get; set; }

    [Argument(Default = "private", Required = true)]
    public String Visibility { get; set; }

    [Argument]
    public bool Recursive { get; set; }

    [Argument]
    public String Directory { get; set; }

    [Argument(Name = "file-match")]
    public String FileMatch { get; set; }

    [Argument]
    public String Files { get; set; }
}
public class CreateResponse
{
    public long? id { get; set; }
    public String title { get; set; }
    public String file_name { get; set; }
    public String description { get; set; }
    public String visibility { get; set; }
    public String updated_at { get; set; }
    public String created_at { get; set; }
    public String project_id { get; set; }
    public String web_url { get; set; }
    public String raw_url { get; set; }
    public String message { get; set; }
}