[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ArgumentAttribute : System.Attribute
{
    public String Name { get; set; }
    public String Alias { get; set; }
    public String Required { get; set; }
}