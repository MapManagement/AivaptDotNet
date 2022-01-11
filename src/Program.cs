using System.Threading.Tasks;


namespace AivaptDotNet
{
    public class Program
    {
	    public static async Task Main()
        {
            await new Bot().StartBot();
        }
    }   
}
