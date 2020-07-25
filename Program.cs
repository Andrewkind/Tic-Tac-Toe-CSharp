/*
TECHCareers Practice Questions
Author: Andrew Kind
*/

using System;

namespace Tic_Tac_toe
{
    class Program
    {

        // Variable Declarations

        static string squareSymbol; // Holds the current symbol
        static int[] squarePosition;
        static bool playAgain = true;
        static int movesPlayed = 0;
        static bool gameActive = true; // The game is active unless we have a winner/tie

        static string currentTurn = "X"; // Starts game with player X
        static bool gameTie = false; // Used to set game winner
        static string[,] gameBoard;

        static void Main(string[] args)
        {


            while (playAgain)
            {
                playAgain = false; // playAgain will be set to true at the end of our game to see if we play again
                StartGame();

                //game is over. see if we replay
                // check with use if he wants to play again?
                bool validInput = false;
                Console.WriteLine();

                while (validInput == false)
                {
                    Console.WriteLine("Would you like to play again? [Y/N]");
                    string userInput = Console.ReadKey().KeyChar.ToString().ToUpper();
                    if (userInput == "Y")
                    {
                        //play again
                        validInput = true;
                        playAgain = true;


                    }
                    else if (userInput == "N")
                    {
                        //exit
                        validInput = true;

                        // User decided not to play again. say goodbye.
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkBlue;

                        Console.Write("Thank you for playing TIC TAC TOE (shareware edition)");
                        Console.WriteLine();

                        Console.ResetColor();
                    }
                    else
                    {
                        // invalid entry
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkRed;

                        Console.Write("Invalid Key. Press [Y] to play again or [N] to Quit.");
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }



            // determine if player plays again
        }


        // Write the Main Menu Lines
        static void showSplashScreen()
        {
            // How to ignore escape character \ in WriteLine
            //https://stackoverflow.com/questions/15142141/how-do-i-write-console-writeline-and-ignore-unrecognized-escape


            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.WriteLine("▒▒██████████████████████████████████████████████████▒▒");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;

            Console.WriteLine(@"██  _____ ___ ___   _____ _   ___   _____ ___  ___  ██");
            Console.WriteLine(@"██ |_   _|_ _/ __| |_   _/_\ / __| |_   _/ _ \| __| ██");
            Console.WriteLine(@"██   | |  | | (__    | |/ _ \ (__    | || (_) | _|  ██");
            Console.WriteLine(@"██   |_| |___\___|   |_/_/ \_\___|   |_| \___/|___| ██");

            Console.WriteLine(@"██                                                  ██");
            Console.BackgroundColor = ConsoleColor.Magenta;

            Console.WriteLine("▒▒██████████████████████████████████████████████████▒▒");
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine("Press any key to begin!");
            Console.ReadKey();
            Console.Clear();


        }

        // Validate user input from main menu
        // It should be a key from numbers 1-4
        static bool validateInput(string userInput)
        {

            //  //https://stackoverflow.com/questions/12644473/c-sharp-check-if-consolekeyinfo-keychar-is-a-number
            // Validate user input.
            // Should be number 1-5
            int number;
            if (Int32.TryParse(userInput, out number))
            {
                // User input is a number
                if (number >= 1 && number <= 9)
                {
                    // valid number
                    //check if it is free
                    int[] squarePosition = GetSquarePosition(userInput);
                    squareSymbol = gameBoard[squarePosition[0], squarePosition[1]];
                    if (squareSymbol == "X" || squareSymbol == "O")
                    {
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
            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("Invalid input: '{0}'. Press any key.", userInput);
            Console.WriteLine();

            Console.ResetColor();

            Console.ReadKey();
            PrintGameBoard();
            return false;

        }



        //Perform tasks to prompt the main menu

        static void DoTurn()
        {
            PrintGameBoard();
            //
            PerformUserSelection();

            movesPlayed++; // Turn is over so increment moves played.
        }

        static void PerformUserSelection()
        {
            string validSelection = GetUserInput();

            //   Console.WriteLine("User proper selection is {0}", validSelection);

            // Fill in the spot
            gameBoard[squarePosition[0], squarePosition[1]] = currentTurn; //Assign the current turn (either X or O to the square position)






        }
        static void StartGame()
        {
            InitializeGame();
            showSplashScreen();

            while (gameActive == true)
            {
                DoTurn();

                // See if we have any winner...
                // if we have a winner
                if (WeHaveWinner())
                {
                    gameActive = false;
                    PrintGameBoard();
                    if (currentTurn == "X")
                    {
                        Console.BackgroundColor = ConsoleColor.Green;

                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;

                    }
                    Console.ForegroundColor = ConsoleColor.Black;


                    Console.WriteLine();
                    Console.Write("PLAYER {0} WINS", currentTurn);
                    Console.WriteLine();

                    Console.ResetColor();
                }

                // we have no winner yet.
                // CHeck if the board is full
                if (movesPlayed > 8)
                {
                    //we are tied.
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
                if (currentTurn == "X")
                {
                    currentTurn = "O";
                }
                else
                {
                    currentTurn = "X";
                }

            }



        }


        static bool CheckPositionsForWinner(string symbol1, string symbol2, string symbol3)
        {

            if (symbol1 == symbol2 && symbol2 == symbol3)
            {
                return true;

            }
            return false;
        }
        static bool WeHaveWinner()
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
        static ConsoleColor GetSymbolColor(string symbol)
        {

            if (symbol == "X")
            {

                //return blue?
                return ConsoleColor.Green;
            }
            else if (symbol == "O")
            {
                return ConsoleColor.Yellow;
            }
            else
            {
                return ConsoleColor.Cyan;
                {

                }
            }
        }

        static void PrintGameBoard()
        {
            // multicolor lines
            // https://stackoverflow.com/questions/38516955/console-color-more-than-1-in-same-line

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("-------------");

            Console.Write("| ");

            Console.ForegroundColor = GetSymbolColor(gameBoard[0, 0]);
            Console.Write(gameBoard[0, 0]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[0, 1]);
            Console.Write(gameBoard[0, 1]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[0, 2]);
            Console.Write(gameBoard[0, 2]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" | ");
            Console.ResetColor();


            // End of first row

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine();
            Console.WriteLine("-------------");

            Console.Write("| ");

            Console.ForegroundColor = GetSymbolColor(gameBoard[1, 0]);
            Console.Write(gameBoard[1, 0]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[1, 1]);
            Console.Write(gameBoard[1, 1]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[1, 2]);
            Console.Write(gameBoard[1, 2]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" | ");
            Console.ResetColor();

            //end of second row


            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine();
            Console.WriteLine("-------------");

            Console.Write("| ");

            Console.ForegroundColor = GetSymbolColor(gameBoard[2, 0]);
            Console.Write(gameBoard[2, 0]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[2, 1]);
            Console.Write(gameBoard[2, 1]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(gameBoard[2, 2]);
            Console.Write(gameBoard[2, 2]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine();
            Console.WriteLine("-------------");
            Console.ResetColor();
            // end of third row 

            Console.ForegroundColor = ConsoleColor.Black;

            if (currentTurn == "X")
            {
                Console.BackgroundColor = ConsoleColor.Green;

            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Yellow;

            }
            Console.WriteLine();
            Console.Write(" It is Player's {0} turn! ", currentTurn);
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("Please press 1-9 to play a square!");



        }

        static void InitializeGame()
        {

            currentTurn = "X";
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
        static string GetUserInput()
        {
            // 1. Get user input as key
            // 2. Make sure key is valid (1-9)
            // 3. Make sure key is associated to an empty square



            bool isValid = false;
            string userInput = "";


            while (!isValid)
            {
                userInput = Console.ReadKey().KeyChar.ToString();

                isValid = validateInput(userInput);
            }

            // newLine();
            //  Console.WriteLine("userInput: {0}", userInput);





            return "1";
        }

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



        //Write a program that will take in a 4 digit integer and calculate the sum of its digits without ever converting the integer to a string.


    }
}
