[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ArgumentAttribute : System.Attribute
{
    public String Prefix { get; set; } = "--";
    public String Name { get; set; }
    public String Alias { get; set; }
}