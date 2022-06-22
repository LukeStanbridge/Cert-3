using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_2
{
    class Program
    {
        static void Main(string[] args)
        {
            string firstName = "";
            string lastName = "";
            
            Console.WriteLine("Enter first name? ");
            firstName = Console.ReadLine();

            Console.WriteLine("Enter last name? ");
            lastName = Console.ReadLine();

            string fullName = string.Format("{0} {1}", firstName, lastName);

            Console.WriteLine(fullName);
            Console.ReadKey();

            //Console.WriteLine("Nice to meet you, " + firstName + " " + lastName);
            //Console.WriteLine("I wish you would hurry up and make a game!");

            //Console.WriteLine("Press any key to ciao");
            //Console.ReadKey();
        }
    }
}
