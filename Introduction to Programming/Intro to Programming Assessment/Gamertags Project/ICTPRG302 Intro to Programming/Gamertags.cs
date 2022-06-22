using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ICTPRG302_Intro_to_Programming
{
    class Gamertags
    {
        // The list of gamer tags loaded from file
        private string[] gamerTagList = { };

        // Load a list of gamertags from file and store the resulting list in our gamerTagList
        public void LoadGamertags()
        {
            gamerTagList = File.ReadAllLines("../Gamertags.txt");
        }

        // Display a welcome message
        public void ShowWelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("Welcome to the Gamertag Database...");
            Console.WriteLine("====================================");
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        //Enable user to select an option
        public void UserInput(ref int optionNum)
        {
            Console.Clear();
            Console.WriteLine("1. Print all gamertags");
            Console.WriteLine("2. Print gamertags ending with a number");
            Console.WriteLine("3. Print gamertags NOT starting with a letter or number");
            Console.WriteLine("4. Print gamertags with ONLY letters or numbers");
            Console.WriteLine("5. Print all gamertags in uppercase");
            Console.WriteLine("6. Print first 5 gamertags");
            Console.WriteLine("7. Add a new gamertag to the list");
            Console.WriteLine("");
            Console.WriteLine("Select a number option from the list");
            optionNum = Convert.ToInt32(Console.ReadLine());
        }

        // Display all gamertags on the console
        public void PrintAllGamertags()
        {
            Console.Clear();
            Console.WriteLine("==============");
            Console.WriteLine("All Gamertags");
            Console.WriteLine("==============");

            // Loop over the list of gamertags and print each out on a new line
            int lineNumber = 1; // this local variable is used as a "bullet list" counter for each line
            foreach (string s in gamerTagList)
            {
                // Format a line for each gamertag with a number in front
                // Note: There are alternative memor-efficient methods to concatenate strings
                Console.WriteLine(lineNumber.ToString() + ") " + s);
                lineNumber = lineNumber + 1; // Increment the lineNumber for the next time around the loop.
            }

            // Display a meeage to the user and wait for a keypress
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Display gamertags ending with a number
        public void PrintGamertagsEndingWithNumber()
        {
            Console.Clear();
            Console.WriteLine("===============================");
            Console.WriteLine("Gamertags ending with a number");
            Console.WriteLine("===============================");

            int lineNumber = 1;
            foreach (string s in gamerTagList)
            {
                if ((s.Length > 0) && Char.IsNumber(s, s.Length - 1))
                {
                    Console.WriteLine(lineNumber.ToString() + ") " + s);
                    lineNumber = lineNumber + 1;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Display gamertags NOT starting with a letter or number
        public void PrintGamertagsNotStartingWithLetterOrNumber()
        {
            Console.Clear();
            Console.WriteLine("===============================================");
            Console.WriteLine("Gamertags NOT starting with a letter or number");
            Console.WriteLine("===============================================");

            int lineNumber = 1;
            foreach (string s in gamerTagList)
            {
                if ((s.Length > 0) && !Char.IsLetterOrDigit(s, 0))
                {
                    Console.WriteLine(lineNumber.ToString() + ") " + s);
                    lineNumber = lineNumber + 1;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Display gamertags that include only letters or numbers
        public void PrintGamertagsWithOnlyLettersOrNumbers()
        {
            Console.Clear();
            Console.WriteLine("=======================================");
            Console.WriteLine("Gamertags with only letters or numbers");
            Console.WriteLine("=======================================");

            int lineNumber = 1;
            foreach (string s in gamerTagList)
            {
                if ((s.Length > 0) && (s.All(Char.IsLetterOrDigit)))
                {
                    Console.WriteLine(lineNumber.ToString() + ") " + s);
                    lineNumber = lineNumber + 1;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Display gamertags in uppercase
        public void PrintGamertagsToUppercase()
        {
            Console.Clear();
            Console.WriteLine("=======================");
            Console.WriteLine("Gamertags in uppercase");
            Console.WriteLine("=======================");

            int lineNumber = 1;
            foreach (string s in gamerTagList)
            {
                Console.WriteLine(lineNumber.ToString() + ") " + s.ToUpper());
                lineNumber = lineNumber + 1;
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Display first 5 gamertags only
        public void PrintFirstFiveGamertags()
        {
            Console.Clear();
            Console.WriteLine("==================");
            Console.WriteLine("First 5 Gamertags");
            Console.WriteLine("==================");

            int lineNumber = 1;
            foreach (string s in gamerTagList)
            {
                if ((s.Length > 0) && (lineNumber < 6))
                {
                    Console.WriteLine(lineNumber.ToString() + ") " + s);
                    lineNumber = lineNumber + 1;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Allow user to add more gamertags to the list
        public void AddMoreGamertags()
        {
            Console.Clear();
            Console.WriteLine("=====================");
            Console.WriteLine("Add another Gamertag");
            Console.WriteLine("=====================");

            Console.WriteLine("Please enter a new gamertag, then press enter to continue...");
            string newGamertag = Console.ReadLine();
            StreamWriter streamWriter = new StreamWriter("../Gamertags.txt", true);
            streamWriter.Write("\n" + newGamertag);
            streamWriter.Close();
        }
    }
}
