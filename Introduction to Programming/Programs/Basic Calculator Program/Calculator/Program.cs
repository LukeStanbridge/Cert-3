using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            float number1;
            float number2;
            float result = 0;
            string userOperation;
            string statementType;

            Console.WriteLine("Enter first number");
            float.TryParse(Console.ReadLine(), out number1);

            Console.WriteLine("Enter second number");
            float.TryParse(Console.ReadLine(), out number2);

            Console.WriteLine("Enter operation: + - / *");
            userOperation = Console.ReadLine();

            Console.WriteLine("Use a switch statement or a if statement?");
            statementType = Console.ReadLine();

            if (statementType == "switch")
            {
                switchResult(ref result, number1, number2, userOperation);
            }
            else
            {
                ifElseResult(ref result, number1, number2, userOperation);
            }

            Console.WriteLine("Result is " + result);
            Console.ReadKey();         
        }

        static private void switchResult(ref float result, float number1, float number2, string userOperation)
        {
            switch (userOperation)
            {
                case "+":
                    result = number1 + number2;
                    break;
                case "-":
                    result = number1 - number2;
                    break;
                case "/":
                    result = number1 / number2;
                    break;
                case "*":
                    result = number1 * number2;
                    break;
                default:
                    Console.WriteLine("Wrong input idiot.");
                    break;
            }
        }

        static private void ifElseResult(ref float result, float number1, float number2, string userOperation)
        {
            if (userOperation == "+")
            {
                result = number1 + number2;
            }
            else if (userOperation == "-")
            {
                result = number1 - number2;
            }
            else if (userOperation == "/")
            {
                result = number1 / number2;
            }
            else if (userOperation == "*")
            {
                result = number1 * number2;
            }
            else
            {
                Console.WriteLine("You are a dumbass!");
            }
        }
    }
}
