using System;

namespace calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write q if you want to quit");
            var input = Console.ReadLine();
            while (input != "q")
            {

                try
                {
                    Console.WriteLine(Calculator.Calculate(input));
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Expression is invalid");
                }
                input = Console.ReadLine();
            }
        }
    }
}
