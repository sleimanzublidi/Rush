using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using Rush;

namespace DynamicSandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var task = new Test().RunAsync();
                Task.WaitAll(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }

    public class Test
    {
        public Task RunAsync()
        {
            var tsc = new TaskCompletionSource<bool>();
            Action action = async delegate
            {
                RushObject obj = new RushObject("Test");
                obj["PlayerName"] = "John Doe";
                obj["Score"] = 12345;
                obj["Array"] = new string[] { "A", "B" };
                
                await RushClient.SaveAsync(obj);

                //User user = new User();

                //await user.SaveAsync();

                //Console.WriteLine(user.ObjectId);
                //Console.WriteLine(user.CreatedAt);
                //Console.WriteLine(user.UpdatedAt);

                tsc.TrySetResult(true);
            };
            action();
            
            return tsc.Task;

            //return new Task(action);

            //return Task.Factory.StartNew(Run);
        }
    }
}
