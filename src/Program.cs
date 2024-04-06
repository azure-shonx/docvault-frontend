public class Program
{

    // public static readonly string BACKEND = "https://backend.docvault.shonx.net/";
    public static readonly string BACKEND = "https://docvault-backend.graysky-3fc7894e.eastus.azurecontainerapps.io/";
    public static void Main(string[] args)
    {
        new WebHandler(args);
    }
}