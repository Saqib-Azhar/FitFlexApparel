using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPracticingApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<int> stackObj = new Stack<int>();
            for(int count = 0; count < 10; count++)
            {
                var input = Console.Read();
                stackObj.Push(Convert.ToInt32(input));
            }

            foreach(var item in stackObj)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
