using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guessing_Game_Program
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;

            // Create a new instance of a random number generator
            Random rnd = new Random();
            int number;

            // Keep running this loop while "isRunning == true"
            while (isRunning)
            {
                // Make the computer to store a random number between 1-100
                number = rnd.Next(1, 30);

                // Prompt the user to start guessing
                Console.WriteLine("Guess my number within 5 turns...");

                for (int i = 0; i < 5; i++)
                {
                    int.TryParse(Console.ReadLine(), out int guess);

                    if (guess == number)
                    {
                        Console.WriteLine("Congrats! You guessed correct!");
                        break; // Break out of the for-loop
                    }
                    else if (guess > number) // Player guessed too high
                    {
                        Console.WriteLine("Lower...");
                    }
                    else 
                    // Player guessed too low
                    {
                        Console.WriteLine("Higher...");
                    }
                }

                // Ask the player if they want to play again
                Console.WriteLine("Would you like to play again (y/n)?");

                string userChoice = Console.ReadLine();
                if (userChoice == "y" || userChoice == "Y")
                    isRunning = true;
                else
                    isRunning = false;
            }

            Console.WriteLine("Thanks for playing jackass");
            Console.ReadKey();
        }
    }
}
