/*
TIC TAC TOE
Author: Andrew Kind
*/

using System;

namespace Tic_Tac_toe
{
    static public class GameBoard
    {
        public static string squareSymbol; // The value within a square (can be "X", "O" or a number 1-9)
        public static int[] squarePosition; // The position of the square within the array. The array is: [row, column]
        public static bool playAgain = true; // Remains true if the player wants to play another game 
        public static int movesPlayed = 0; // Keeps track of the number of moves played
        public static bool gameActive = true; // The game is active unless we have a winner/tie

        public static string currentPlayer; // Hold current player. Will be either "X" or "O" at all times and initializes as "X".
        public static bool gameTie = false; // Set to true if the game ends in a tie
        public static string[,] board; // Our game board. Array size of 3x3.  
        public static int[] score = { 0, 0, 0 }; // Player X and Player O scores and ties
    }
    public class Program
    {
        // Global Variable Declarations


        static void Main(string[] args)
        {
            // Determines if we play a game
            while (GameBoard.playAgain)
            {

                //GameBoard.playAgain will be set to true at the end of our game if we play again
                GameBoard.playAgain = false;

                // Begin the game
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
                        GameBoard.playAgain = true;


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
                    int[] squarePosition = GetGameBoard(userInput);

                    GameBoard.squareSymbol = GameBoard.board[GameBoard.squarePosition[0], GameBoard.squarePosition[1]];
                    if (GameBoard.squareSymbol == "X" || GameBoard.squareSymbol == "O")
                    {
                        // The Symbol in the square position is already filled with a "X" or "O"
                        // Prompt user to try a different square selection
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkRed;

                        Console.WriteLine();
                        Console.Write("Square in row {0} and column {1} is already taken. press any key.", GameBoard.squarePosition[0], GameBoard.squarePosition[1]);
                        Console.WriteLine();

                        Console.ResetColor();

                        Console.ReadKey();

                        PrintGameBoard();
                        PrintUserPrompt();
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

        static void PrintUserPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Black;

            if (GameBoard.currentPlayer == "X")
            {
                Console.BackgroundColor = ConsoleColor.Red;

            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Blue;

            }
            Console.WriteLine();
            Console.Write(" It is Player's {0} turn! ", GameBoard.currentPlayer);
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("Please press 1-9 to play a square!");

        }

        //Performs a player's turn.
        static void DoTurn()
        {
            PrintGameBoard();
            //
            GetUserInput();
            PrintGameBoard();

            GameBoard.movesPlayed++; // Turn is over so increment moves played.
        }

        /// <summary>
        /// Logic to get user selection, validate the input, and perform the player move</summary>  
        static void StartGame()
        {
            // Initialize variables
            InitializeGame();

            showSplashScreen();

            // Main Game Loop
            while (GameBoard.gameActive == true)
            {

                // Each player performs a turn one after another until the game ends by a winner or tie
                DoTurn();

                // Check for a winner
                if (haveWinner())
                {
                    // We have found a winner
                    GameBoard.gameActive = false;
                    Console.ForegroundColor = ConsoleColor.Black;

                    if (GameBoard.currentPlayer == "X")
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        GameBoard.score[0]++;

                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        GameBoard.score[1]++;
                    }
                    Console.WriteLine();

                    Console.Write(" PLAYER {0} WINS ", GameBoard.currentPlayer);
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.WriteLine();

                    printScore();
                }

                // We have no winner yet.
                // Check if the board is full for a tie
                else if (GameBoard.movesPlayed > 8)
                {
                    // We have a tie

                    GameBoard.score[2]++; // increment tie score

                    Console.ForegroundColor = ConsoleColor.Black;

                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine();

                    Console.Write(" GAME IS A ");
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("TIE ");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.Yellow;

                    printScore();

                    //GameBoard.gameTie = true; // set game as a tie
                    GameBoard.gameActive = false; //end the game
                }


                // Game did not end so switch to next player.
                if (GameBoard.currentPlayer == "X")
                {
                    GameBoard.currentPlayer = "O";
                }
                else
                {
                    GameBoard.currentPlayer = "X";
                }

            }

        }
        /// <summary>
        /// Displays the player's scores to console</summary>
        static void printScore()
        {

            //Score
            Console.ResetColor();
            if (GameBoard.currentPlayer == "X")
            {
                // We are a "X" symbol so return a red color
                Console.BackgroundColor = ConsoleColor.Red;
            }
            else
            {
                // We are a "O" symbol so return a blue color
                Console.BackgroundColor = ConsoleColor.Blue;
            }



            //
            Console.ResetColor();
            Console.Write("   ");

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;

            //X
            Console.Write(" Score ");
            Console.ResetColor();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Write(" X: {0} ", GameBoard.score[0]);
            Console.ResetColor();

            //O
            Console.ResetColor();
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Write(" O: {0} ", GameBoard.score[1]);
            Console.ResetColor();

            Console.WriteLine();
            Console.Write("   ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Ties: {0}", GameBoard.score[2]);
            Console.ResetColor();

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
        static bool haveWinner()
        {

            //row one across
            if (CheckPositionsForWinner(GameBoard.board[0, 0], GameBoard.board[0, 1], GameBoard.board[0, 2]))
            {
                return true;
            }

            //row two across
            if (CheckPositionsForWinner(GameBoard.board[1, 0], GameBoard.board[1, 1], GameBoard.board[1, 2]))
            {
                return true;
            }
            //row three across
            if (CheckPositionsForWinner(GameBoard.board[2, 0], GameBoard.board[2, 1], GameBoard.board[2, 2]))
            {
                return true;
            }
            //column 1 down
            if (CheckPositionsForWinner(GameBoard.board[0, 0], GameBoard.board[1, 0], GameBoard.board[2, 0]))
            {
                return true;
            }
            // column 2 down
            if (CheckPositionsForWinner(GameBoard.board[0, 1], GameBoard.board[1, 1], GameBoard.board[2, 1]))
            {
                return true;
            }
            // column 3 down
            if (CheckPositionsForWinner(GameBoard.board[0, 2], GameBoard.board[1, 2], GameBoard.board[2, 2]))
            {
                return true;
            }
            // top left to bottom right
            if (CheckPositionsForWinner(GameBoard.board[0, 0], GameBoard.board[1, 1], GameBoard.board[2, 2]))
            {
                return true;
            }
            // top right to bottom left
            if (CheckPositionsForWinner(GameBoard.board[2, 0], GameBoard.board[1, 1], GameBoard.board[0, 2]))
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

            Console.ForegroundColor = GetSymbolColor(GameBoard.board[0, 0]);
            Console.Write(GameBoard.board[0, 0]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(GameBoard.board[0, 1]);
            Console.Write(GameBoard.board[0, 1]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(GameBoard.board[0, 2]);
            Console.Write(GameBoard.board[0, 2]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();
            // End of first row

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("-------------");

            Console.Write("| ");

            Console.ForegroundColor = GetSymbolColor(GameBoard.board[1, 0]);
            Console.Write(GameBoard.board[1, 0]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(GameBoard.board[1, 1]);
            Console.Write(GameBoard.board[1, 1]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(GameBoard.board[1, 2]);
            Console.Write(GameBoard.board[1, 2]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();
            //end of second row

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("-------------");

            Console.Write("| ");

            Console.ForegroundColor = GetSymbolColor(GameBoard.board[2, 0]);
            Console.Write(GameBoard.board[2, 0]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(GameBoard.board[2, 1]);
            Console.Write(GameBoard.board[2, 1]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = GetSymbolColor(GameBoard.board[2, 2]);
            Console.Write(GameBoard.board[2, 2]);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("-------------");
            Console.ResetColor();
            // end of third row 


        }

        /// <summary>
        /// Method to initiliaze the game. It resets all variables to start the game properly.</summary>
        static void InitializeGame()
        {
            GameBoard.currentPlayer = "X";
            GameBoard.movesPlayed = 0;
            GameBoard.gameActive = true;

            //reset game board
            GameBoard.board = new string[3, 3]{
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
            GameBoard.board[GameBoard.squarePosition[0], GameBoard.squarePosition[1]] = GameBoard.currentPlayer; //Assign the current turn (either X or O to the square position)

        }

        /// <summary>
        /// Pass in the user's input and return the square position in a 2d int array.</summary>
        /// <param name="userInput"> validated user input</param>
        static int[] GetGameBoard(string userInput)
        {

            switch (userInput)
            {

                case "1":
                    GameBoard.squarePosition = new int[2] { 0, 0 };

                    break;
                case "2":
                    GameBoard.squarePosition = new int[2] { 0, 1 };
                    break;
                case "3":
                    GameBoard.squarePosition = new int[2] { 0, 2 };
                    break;
                case "4":
                    GameBoard.squarePosition = new int[2] { 1, 0 };
                    break;
                case "5":
                    GameBoard.squarePosition = new int[2] { 1, 1 };
                    break;
                case "6":
                    GameBoard.squarePosition = new int[2] { 1, 2 };
                    break;
                case "7":
                    GameBoard.squarePosition = new int[2] { 2, 0 };
                    break;
                case "8":
                    GameBoard.squarePosition = new int[2] { 2, 1 };
                    break;
                case "9":
                    GameBoard.squarePosition = new int[2] { 2, 2 };
                    break;
                default:
                    break;
            }
            string squareSymbol = GameBoard.board[GameBoard.squarePosition[0], GameBoard.squarePosition[1]];

            return GameBoard.squarePosition;
        }
    }
}
