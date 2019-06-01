#load "./ArgumentAttribute.csx"
#load "./PropertyArgument.csx"
using System.Reflection;


public class ArgumentBuilder
{
    private ArgumentBuilder() { }
    public static ArgumentBuilder<T> Build<T>(IEnumerable<String> args)
    where T : new()
    {
        return new ArgumentBuilder<T>(args);
    }
}
public class ArgumentBuilder<T> where T : new()
{
    private readonly List<PropertyArgument> Properties;
    private readonly List<String> Args;
    public ArgumentBuilder(IEnumerable<String> args)
    {
        this.Properties = typeof(T).GetProperties()
        .Where(p => p.CanWrite)
        .Select(p => new PropertyArgument { Argument = p.GetCustomAttribute<ArgumentAttribute>(), Property = p })
        .Where(item => item.Argument != null)
        .ToList();

        this.Args = args.ToList();
    }
    public T GetArgs()
    {
        var instance = new T();
        this.Properties.ForEach(pa => pa.SetPropValue(this.Args, instance));
        return instance;
    }
    public (bool, T, IEnumerable<string>) GetValidArgs()
    {
        var instance = new T();
        this.Properties.ForEach(pa => pa.SetPropValue(this.Args, instance));
        List<String> errors = this.Properties.Where(pa => pa.Argument.Required && pa.Property.GetValue(instance) == null).Select(pa => $"{String.Join(",", pa.ArgNames)} can't be null").ToList();
        if (errors.Count > 0)
            return (false, instance, errors);

        return (true, instance, new List<String>());
    }
}