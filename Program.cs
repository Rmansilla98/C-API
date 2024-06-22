

using AuthKalumManagement;

public class Program 
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => //metodo que levantara la configuracion que se establecion en la clase StartUp.cs
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>{
            webBuilder.UseStartup<StartUp>();
        });
}
