using System.Runtime.CompilerServices;
using System.IO;
using System.Text.RegularExpressions;

public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

public static (bool, String) GetArgValue(List<String> args, String argName)
{
    var lowerCaseArgs = args.Select(arg => (arg ?? "").ToLower()).ToList();
    var lowerCaseArgName = argName.ToLower();

    var index = lowerCaseArgs.IndexOf("-" + lowerCaseArgName);
    if (index == -1)
        index = lowerCaseArgs.IndexOf("--" + lowerCaseArgName);

    if (index == -1) return (false, "NOT_FOUND");
    var nextValue = index == args.Count - 1 ? "true" : args[index + 1];
    if (nextValue[0] == '-')
        return (true, "true");
    return (true, nextValue);
}

public static string ReadStdIn()
{
    using (var streamReader = new StreamReader(Console.OpenStandardInput()))
    {
        return streamReader.ReadToEnd();
    }
}
public static IEnumerable<String> DirFiles(String path, bool recurise = false, String regPattern = null)
{
    foreach (var file in Directory.GetFiles(path))
    {
        var fp = Path.GetFullPath(file);
        if (regPattern != null && !Regex.IsMatch(fp, regPattern))
            continue;
        yield return fp;
    }
    if (!recurise)
    {
        yield break;
    }
    foreach (var dir in Directory.GetDirectories(path))
    {
        foreach (var file in DirFiles(dir, recurise, regPattern))
        {
            yield return file;
        }
    }
}
