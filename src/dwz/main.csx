#!/usr/bin/env dotnet-script

#r "nuget: Newtonsoft.Json, 12.0.2"

using System.Net.Http;
using static System.Console;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;

if (Args.Count == 0)
{
    Console.WriteLine($@"
使用默认token进行短链请求, 参数支持多个域名或者多个包含域名列表的文本文件: 
> dwz http://z.cn http://tim.qq.com /user/some-urls.txt 

使用自己token进行短链请求: 
> dwz token:893hf890ih3 http://z.cn http://tim.qq.com /user/some-urls.txt
    ");
    return;
}
static readonly var client = new HttpClient();

public static async Task<DwzResponse> ShortUrl(String token, String longUrl)
{
    try
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://dwz.cn/admin/v2/create");
        var reqBody = SerializeObject(new { url = longUrl });
        request.Content = new StringContent(reqBody, Encoding.UTF8, "application/json");
        request.Content.Headers.Add("Token", token);
        var response = await client.SendAsync(request);
        var repData = await response.Content.ReadAsStringAsync();
        return DeserializeObject<DwzResponse>(repData);
    }
    catch (Exception ex)
    {
        return new DwzResponse() { Code = -1, ErrMsg = ex.Message };
    }
}

var hasInputToken = Args[0].StartsWith("token:");
var TOKEN = hasInputToken ? Args[0].Split(":")[1] : "e97faefe8ba7a18d2bd4d17d9d8209b1";

Args.Skip(hasInputToken ? 1 : 0)
    .Select(arg => File.Exists(arg) ? File.ReadAllLines(arg).Where(line => !String.IsNullOrWhiteSpace(line)) : new[] { arg })
    .Aggregate(new List<String>(), (prev, next) =>
    {
        prev.AddRange(next);
        return prev;
    })
    .Select(domain => ShortUrl(TOKEN, domain).Result)
    .ToList()
    .ForEach(response => WriteLine($"{(response.Code == 0 ? response.ShortUrl : response.ErrMsg)} > {response.LongUrl}"));

public class DwzResponse
{
    public int Code { get; set; }
    public String ErrMsg { get; set; }
    public String LongUrl { get; set; }
    public String ShortUrl { get; set; }
    public override String ToString()
    {
        return SerializeObject(this, Formatting.Indented);
    }
}