// // See https://aka.ms/new-console-template for more information
//

public class Program
{
    
    public static async Task Main(string[] args)
    {
        List<Task> tasks = new List<Task>();
        
        for(int i = 0; i < 5; i++)
        {
            tasks.Add(Task.Run(() => Console.WriteLine(i + 1)));
        }

        Task.WaitAll(tasks.ToArray());
        
    }

}







// A self-contained lambda. Not a closure.

