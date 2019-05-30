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
    public T GetArgs(String[] args)
    {
        var instance = new T();
        
        return instance;
    }
    public class PropArg
    {
        public PropertyInfo Property { get; set; }
        public ArgumentAttribute Argument { get; set; }
        public bool IsValid()
        {
            return this.Argument != null;
        }
    }
}