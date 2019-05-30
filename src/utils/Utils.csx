#load "./ArgumentAttribute.csx"
using System.Reflection;

public static T GetArgs<T>(String[] args) where T : new()
{
    List<String> t = args.ToList();
    var data = new Dictionary<string, string>();
    while(t.Count > 0){
        var key = t[0];
        if(String.IsNullOrWhiteSpace(key) || key[0] != '-'){
            t.RemoveAt(0);
            continue;
        }
        var value = t[1];
    }

    var instance = new T();
    var properties = instance.GetType()
        .GetProperties()
        .Where(p => p.CanWrite)
        .Select(p => new { Attr = p.GetCustomAttribute<ArgumentAttribute>(), Prop = p })
        .Where(item => item.Attr != null);

    return instance;
}