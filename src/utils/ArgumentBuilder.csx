#load "./ArgumentAttribute.csx"
using System.Reflection;


public class ArgumentBuilder<T> where T : new()
{
    private readonly List<PropArg> Properties;
    public ArgumentBuilder()
    {
        this.Properties = typeof(T).GetProperties()
        .Where(p => p.CanWrite)
        .Select(p => new PropArg { Argument = p.GetCustomAttribute<ArgumentAttribute>(), Property = p })
        .Where(item => item.IsValid())
        .ToList();
    }
    public T GetArgs(List<String> args)
    {
        var instance = new T();
        foreach (var prop in this.Properties)
        {
            var argName = "-" + prop.Argument.Name;
            var aliasName = "--" + prop.Argument.Alias;
            var index = args.IndexOf(argName);
            if (index == -1 && prop.Argument.Alias != null)
            {
            }
        }

        return instance;
    }
    public static (bool, String) GetArgValue(List<String> args, String argName)
    {
        var index = args.IndexOf(argName);
        if(index == -1) return (false, "NOT_FOUND");
        var nextValue = index > args.Count ? 
    }
    public class PropArg
    {
        public PropertyInfo Property { get; set; }
        public ArgumentAttribute Argument { get; set; }
        public (bool, String) GetArgNameValue(List<String> args)
        {
            if (!String.IsNullOrWhiteSpace(this.Argument.Name))
            {
                var index = args.IndexOf("-" + this.Argument.Name);
                if (index > 0)
                {

                }
            }
            return (false, "");
        }
        public bool IsValid()
        {
            return this.Argument != null;
        }
        public bool SetValue<T>(T instance)
        {
            this.Property.SetValue(instance)
            return true;
        }
    }
}