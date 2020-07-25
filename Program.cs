/*
TIC TAC TOE
Author: Andrew Kind
*/

using System;

namespace Tic_Tac_toe
{
    class Program
    {
        // Global Variable Declarations
        static string squareSymbol; // The value within a square (can be "X", "O" or a number 1-9)
        static int[] squarePosition; // The position of the square played. The array is: [row, column]
        static bool playAgain = true; // Remains true if the player wants to play another game 
        static int movesPlayed = 0; // Keeps track of the number of moves played
        static bool gameActive = true; // The game is active unless we have a winner/tie

        static string currentPlayer; // Hold current player. Will be either "X" or "O" at all times.
        static bool gameTie = false; // Set to true if the game ends in a tie
        static string[,] gameBoard; // Our game board. Array size of 3x3.  

        static void Main(string[] args)
        {
            // Determines if we play a game
            while (playAgain)
            {

                // playAgain will be set to true at the end of our game to see if we play again
                playAgain = false;

                // Run main game logic
                StartGame();

                //Game is over. Check with user if they want to play again.
                bool validInput = false;

                while (validInput == false)
                {
                    //Ask player if they want to play again
                    Console.WriteLine();

                    Console.WriteLine("Would you like to play again? [Y/N]");
                    string userInput = Console.ReadKey().KeyChar.ToString().ToUpper();
                    if (userInput == "Y")
                    {
                        //User indicated they want to play again
                        validInput = true;
                        playAgain = true;


                    }
                    else if (userInput == "N")
                    {
                        //User indicated they do not want to play again
                        validInput = true;

                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.Write("Thank you for playing TIC TAC TOE (shareware edition)");
                        Console.ResetColor();
                        Console.WriteLine();

                    }
                    else
                    {
                        // User provided an invalid entry (Did not enter Y or N)
                        // User will be prompt an error and can try again
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkRed;

                        Console.Write("Invalid Key. Press [Y] to play again or [N] to Quit.");
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }

        }

        static void showSplashScreen()
        {
            // How to ignore escape character \ in WriteLine
            //Source: https://stackoverflow.com/questions/15142141/how-do-i-write-console-writeline-and-ignore-unrecognized-escape

            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.WriteLine("▒▒██████████████████████████████████████████████████▒▒");

            Console.WriteLine(@"██  _____ ___ ___   _____ _   ___   _____ ___  ___  ██");
            Console.WriteLine(@"██ |_   _|_ _/ __| |_   _/_\ / __| |_   _/ _ \| __| ██");
            Console.WriteLine(@"██   | |  | | (__    | |/ _ \ (__    | || (_) | _|  ██");
            Console.WriteLine(@"██   |_| |___\___|   |_/_/ \_\___|   |_| \___/|___| ██");

            Console.WriteLine(@"██                                                  ██");

            Console.WriteLine("▒▒██████████████████████████████████████████████████▒▒");
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine("Press any key to begin!");
            Console.ReadKey();
            Console.Clear();

        }

        /*
    The main Math class
    Contains all methods for performing basic math functions
        */
        static bool validateInput(string userInput)
        {
            // Determine if console key is a number
            // Source: https://stackoverflow.com/questions/12644473/c-sharp-check-if-consolekeyinfo-keychar-is-a-number

            // Validate user input.
            // Should be number 1-9
            int number;
            if (Int32.TryParse(userInput, out number))
            {
                // User input is a number
                if (number >= 1 && number <= 9)
                {
                    // valid number
                    //check if the square position associated with the number is filled already
                    int[] squarePosition = GetSquarePosition(userInput);
                    squareSymbol = gameBoard[squarePosition[0], squarePosition[1]];
                    if (squareSymbol == "X" || squareSymbol == "O")
                    {
                        // The Symbol in the square position is already filled with a "X" or "O"
                        // Prompt user to try a different square selection
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkRed;

                        Console.WriteLine();
                        Console.Write("Square in row {0} and column {1} is already taken. press any key.", squarePosition[0], squarePosition[1]);
                        Console.WriteLine();

                        Console.ResetColor();

                        Console.ReadKey();

                        PrintGameBoard();
                        return false;
                    }

                    return true;
                }

            }

            // User input was invalid. It did not parse to number.
            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("Invalid input: '{0}'. Press any key.", userInput);
            Console.WriteLine();

            Console.ResetColor();

            Console.ReadKey();
            PrintGameBoard();
            return false;
        }

        //Performs a player's turn.
        static void DoTurn()
        {
            PrintGameBoard();
            //
            GetUserInput();

            movesPlayed++; // Turn is over so increment moves played.
        }

        /// <summary>
        /// Logic to get user selection, validate the input, and perform the player move</summary>  
        static void StartGame()
        {
            InitializeGame();
            showSplashScreen();

            while (gameActive == true)
            {
                DoTurn();

                // Check for a winner
                if (CheckForWinner())
                {

                    // We have found a winner
                    gameActive = false;
                    PrintGameBoard();
                    if (currentPlayer == "X")
                    {
                        Console.BackgroundColor = ConsoleColor.Red;

                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;

                    }
                    Console.ForegroundColor = ConsoleColor.Black;

                    Console.WriteLine();
                    Console.Write("PLAYER {0} WINS", currentPlayer);
                    Console.WriteLine();

                    Console.ResetColor();
                }

                // We have no winner yet.
                // Check if the board is full for a tie
                else if (movesPlayed > 8)
                {
                    // We have a tie
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine();

                    Console.Write("GAME IS A ");
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("TIE");

                    Console.ResetColor();

                    gameTie = true; // set game as a tie
                    gameActive = false; //end the game
                }


                // Game did not end so switch to next player.
                if (currentPlayer == "X")
                {
                    currentPlayer = "O";
                }
                else
                {
                    currentPlayer = "X";
                }

            }

        }

        /// <summary>
        /// Check the 3 square positions and return true if they match symbols.</summary>
        /// <param name="symbol1"> 1st square to compare with 2nd square</param>
        /// <param name="symbol2"> 2nd square to compare with1st and 3rd squares</param>
        /// <param name="symbol3"> 3rd square to compare with 2nd square</param>

        static bool CheckPositionsForWinner(string symbol1, string symbol2, string symbol3)
        {

            if (symbol1 == symbol2 && symbol2 == symbol3)
            {
                return true;

            }
            return false;
        }

        /// <summary>
        /// Check to see if the current game has a winner.</summary>
        static bool CheckForWinner()
        {

            //row one across
            if (CheckPositionsForWinner(gameBoard[0, 0], gameBoard[0, 1], gameBoard[0, 2]))
            {
                return true;
            }

            //row two across
            if (CheckPositionsForWinner(gameBoard[1, 0], gameBoard[1, 1], gameBoard[1, 2]))
            {
                return true;
            }
            //row three across
            if (CheckPositionsForWinner(gameBoard[2, 0], gameBoard[2, 1], gameBoard[2, 2]))
            {
                return true;
            }
            //column 1 down
            if (CheckPositionsForWinner(gameBoard[0, 0], gameBoard[1, 0], gameBoard[2, 0]))
            {
                return true;
            }
            // column 2 down
            if (CheckPositionsForWinner(gameBoard[0, 1], gameBoard[1, 1], gameBoard[2, 1]))
            {
                return true;
            }
            // column 3 down
            if (CheckPositionsForWinner(gameBoard[0, 2], gameBoard[1, 2], gameBoard[2, 2]))
            {
                return true;
            }
            // top left to bottom right
            if (CheckPositionsForWinner(gameBoard[0, 0], gameBoard[1, 1], gameBoard[2, 2]))
            {
                return true;
            }
            // top right to bottom left
            if (CheckPositionsForWinner(gameBoard[2, 0], gameBoard[1, 1], gameBoard[0, 2]))
            {
                return true;
            }

            return false; //we do not have a winner
        }

        /// <summary>
        /// Description for SomeMethod.</summary>
        /// <param name="s"> Parameter description for s goes here</param>

        /// <summary>
        /// Based on the symbol in question, we return the appropriate color to display</summary>
        /// <param name="symbol"> The symbol that we are checking to see what color to return</param>

        static ConsoleColor GetSymbolColor(string symbol)
        {

            if (symbol == "X")
            {
                // We are a "X" symbol so return a red color
                return ConsoleColor.Red;
            }
            else if (symbol == "O")
            {
                // We are a "O" symbol so return a blue color
                return ConsoleColor.Blue;
            }
            else
            {
                // We are a not filled yet, and must be a number.
                return ConsoleColor.Green;

            }
        }

        /// <summary>
        /// Logic to print out the game board.</summary>
        static void PrintGameBoard()
        {
            // multicolor lines
            // https://stackoverflow.com/questions/38516955/console-color-more-than-1-in-same-line

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-------------");

            Console.Write("| ");

            Console.ForegroundColor = GetSymbolColor(gameBoard[0, 0]);
            Console.Write(gameBoard[0, 0]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[0, 1]);
            Console.Write(gameBoard[0, 1]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[0, 2]);
            Console.Write(gameBoard[0, 2]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();
            // End of first row

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("-------------");

            Console.Write("| ");

            Console.ForegroundColor = GetSymbolColor(gameBoard[1, 0]);
            Console.Write(gameBoard[1, 0]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[1, 1]);
            Console.Write(gameBoard[1, 1]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[1, 2]);
            Console.Write(gameBoard[1, 2]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();
            //end of second row

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("-------------");

            Console.Write("| ");

            Console.ForegroundColor = GetSymbolColor(gameBoard[2, 0]);
            Console.Write(gameBoard[2, 0]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[2, 1]);
            Console.Write(gameBoard[2, 1]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[2, 2]);
            Console.Write(gameBoard[2, 2]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("-------------");
            Console.ResetColor();
            // end of third row 

            Console.ForegroundColor = ConsoleColor.Black;

            if (currentPlayer == "X")
            {
                Console.BackgroundColor = ConsoleColor.Red;

            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Blue;

            }
            Console.WriteLine();
            Console.Write(" It is Player's {0} turn! ", currentPlayer);
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("Please press 1-9 to play a square!");

        }

        /// <summary>
        /// Method to initiliaze the game. It resets all variables to start the game properly.</summary>
        static void InitializeGame()
        {
            currentPlayer = "X";
            movesPlayed = 0;
            gameTie = false;
            gameActive = true;

            //reset game board
            gameBoard = new string[3, 3]{
                                {"1", "2", "3"},
                                {"4", "5", "6"},
                                {"7", "8", "9"},
                            };

        }

        static void GetUserInput()
        {

            bool isValid = false;
            string userInput;


            while (!isValid)
            {
                userInput = Console.ReadKey().KeyChar.ToString();

                isValid = validateInput(userInput);
            }
            // Fill in the spot
            gameBoard[squarePosition[0], squarePosition[1]] = currentPlayer; //Assign the current turn (either X or O to the square position)

        }

        /// <summary>
        /// Pass in the user's input and return the square position in a 2d int array.</summary>
        /// <param name="userInput"> validated user input</param>
        static int[] GetSquarePosition(string userInput)
        {

            switch (userInput)
            {

                case "1":
                    squarePosition = new int[2] { 0, 0 };

                    break;
                case "2":
                    squarePosition = new int[2] { 0, 1 };
                    break;
                case "3":
                    squarePosition = new int[2] { 0, 2 };
                    break;
                case "4":
                    squarePosition = new int[2] { 1, 0 };
                    break;
                case "5":
                    squarePosition = new int[2] { 1, 1 };
                    break;
                case "6":
                    squarePosition = new int[2] { 1, 2 };
                    break;
                case "7":
                    squarePosition = new int[2] { 2, 0 };
                    break;
                case "8":
                    squarePosition = new int[2] { 2, 1 };
                    break;
                case "9":
                    squarePosition = new int[2] { 2, 2 };
                    break;
                default:
                    break;
            }
            string squareSymbol = gameBoard[squarePosition[0], squarePosition[1]];

            return squarePosition;
        }
    }
}
