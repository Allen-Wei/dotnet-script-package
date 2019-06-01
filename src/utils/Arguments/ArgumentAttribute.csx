[AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public class ArgumentAttribute : System.Attribute
{
    public String Name { get; set; }
    public String Alias { get; set; }
    public bool Required { get; set; } 
    public String Default { get; set; }
}