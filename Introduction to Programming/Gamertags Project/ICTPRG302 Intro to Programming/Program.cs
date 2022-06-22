using System;
using System.IO;

namespace ICTPRG302_Intro_to_Programming
{
	class Program
	{
		/// <summary>
		/// This is the starting location for the program.
		/// </summary>
		/// <param name="args">
		/// A list of strings passed in to the program
		/// from the command-line when it was started
		/// </param>
		
		
		static void Main(string[] args)
		{
			// Initialise this variable to true so the first while-test will pass and update it each time we re-ask the user if the program should continue
			bool isRunning = true;
			int optionNumber = 0; // variable used to match a case label in switch statement

			while (isRunning)
			{
				Gamertags gamertags = new Gamertags();

				gamertags.ShowWelcomeMessage();
				gamertags.UserInput(ref optionNumber);
				gamertags.LoadGamertags();

				switch (optionNumber) // switch statement for user options
                {
					case 1:
						gamertags.PrintAllGamertags();
						break;

					case 2:
						gamertags.PrintGamertagsEndingWithNumber();
						break;

					case 3:
						gamertags.PrintGamertagsNotStartingWithLetterOrNumber();
						break;

					case 4:
						gamertags.PrintGamertagsWithOnlyLettersOrNumbers();
						break;

					case 5:
						gamertags.PrintGamertagsToUppercase();
						break;

					case 6:
						gamertags.PrintFirstFiveGamertags();
						break;

					case 7:
						gamertags.AddMoreGamertags();
						break;

					default:
						Console.WriteLine("You didn't pick a valid option....");
						break;
				}
										
				// Ask the user if we should re-run sequence or exit program
				Console.WriteLine("");
				Console.WriteLine("Would you like to view the gamertags again (y/n)?");

				// Read the user input from the console and compare it to the character 'y'
				// Keep running this loop only if user selects 'y' key
				if (Console.ReadKey().Key == ConsoleKey.Y)
					isRunning = true;
				else
					isRunning = false;
			}			
		}		
	}
}
