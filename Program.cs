using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_game
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int int_SizeOfEnvironment = Convert.ToInt32(args[0]);
                int int_NumberOfBAatteries = Convert.ToInt32(args[1]);

                GameWorld Game = new GameWorld(int_SizeOfEnvironment, int_NumberOfBAatteries);
                bool bln_continue = true;

                while (bln_continue)
                {
                    Console.Clear();

                    // Reaon I put an index of 0 is because I want the program to just take the first character in the string
                    // I used ToLower() incase the player inputs any capital letters
                    Game.DisplayGame();
                    Console.Write("\nMove : ");
                    string Input = Console.ReadLine().ToLower().Trim();
                    char Move = Input == "" ? '\0' : Input[0];

                    switch (Move)
                    {
                        //Player pressed W
                        case 'w':
                            Game.PlayerMove(GameWorld.PLAYER_MOVE.W);
                            break;

                        //Player pressed S
                        case 's':
                            Game.PlayerMove(GameWorld.PLAYER_MOVE.S);
                            break;

                        // Player pressed A
                        case 'a':
                            Game.PlayerMove(GameWorld.PLAYER_MOVE.A);
                            break;

                        // Player pressed D
                        case 'd':
                            Game.PlayerMove(GameWorld.PLAYER_MOVE.D);
                            break;

                        case 'e':
                            Game.PlayerMove(GameWorld.PLAYER_MOVE.E);
                            break;

                        //Player quit
                        case 'x':
                            bln_continue = false;
                            break;

                        // Player pressed wrong key
                        default:
                            Console.Clear();
                            Console.WriteLine("You need to input a valid key\n\nPress Enter to continue the game ");
                            Console.ReadLine();
                            break;
                    }  
                }

            }
            catch(FormatException e)
            {
                Console.WriteLine("Please input numbers for your command lines arguments");

            }catch(IndexOutOfRangeException e)
            {
                Console.WriteLine("Please input 2 command line arguments");
            }
            catch (Exception e)
            {
                Console.WriteLine("There has been an err : " + e.Message);
            }
            finally
            {
                Console.Write("\nPress Enter To Exit : ");
                Console.ReadLine();
            }
        }
    }

    class GameWorld
    {
        //Properties
        public int WorldSize { get; }
        public int NumOfBatteries { get; }

        private char[,] World;
        public struct Coordinates
        {
            public int X;
            public int Y;

            public Coordinates(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        private class Player
        {
            //Player properties
            public Coordinates Coordinates { get; set; }
            private int Cells;
            public int NumOfBatteries { get {
                    return Cells > 3 
                        ? (Cells-3) / 3
                        : 0;    
            } }
            public string TorchPercantage { get {
                    string output = "";
                    if (Cells == 3)
                        output = "///";
                    else {
                        int CellsInTorch = Cells % 3;
                        for (int i = 0; i < 3; i++)
                            output += i < CellsInTorch ? "/" : " ";
                    }
                    return output;
            } }
            private bool TorchState;
            public bool TorchOn { get {
                    return TorchState;
            } }

            //Player constructor
            public Player(Coordinates newCoordinates)
            {
                Coordinates = newCoordinates;
                Cells = 3;
                TorchState = true;
            }

            public void PlayerMoved()
            {
                Cells-= Cells > 0 ? 1 : 0;

                //Switching off torch if there are no cells left
                TorchState = Cells > 0
                    ? TorchState
                    : false;
            }

            public void FlickTorch()
            {
                TorchState = !TorchState;
            }
        }
        public enum PLAYER_MOVE
        {
            W,
            A,
            S,
            D,
            E, //Flick switch
        }
        private Player Player1 { get; }
        Coordinates[] Batteries;

        private int NumOfTurns;
        public bool Won { get; }
        public bool GameOver { get; }

        private const int PitSpawnChance = 15;
        private enum GAME_CHARACTER
        {
            CHR_EMPTY_SPACE = ' ',
            CHR_PIT = 'O',
            CHR_PLAYER = 'A',
            CHR_BATTERY = 'B'
        }

        private Random GameRandomiser = new Random(); 

        //Constructor & Destructor
        public GameWorld(int int_GameWorldSize, int int_NumOfBatteries)
        {
            // Setting properties
            WorldSize = Math.Abs(int_GameWorldSize);
            NumOfBatteries = Math.Abs(int_NumOfBatteries);
            NumOfTurns = 2 * NumOfBatteries;
            Won = false;
            World = new char[int_GameWorldSize, int_GameWorldSize];

            //Creating the display
            for (int x = 0; x < WorldSize; x++)
                for(int y = 0; y < WorldSize; y++)
                    World[y, x] = (GameRandomiser.Next(100) < PitSpawnChance) 
                        ? (char)GAME_CHARACTER.CHR_PIT 
                        : (char)GAME_CHARACTER.CHR_EMPTY_SPACE;

            //Saving the player coordinates & saving them in the world
            Player1 = new Player(RandomCoordinates());
            World[Player1.Coordinates.Y, Player1.Coordinates.X] = (char)GAME_CHARACTER.CHR_PLAYER;


            //Instantiating Batteries & saving them in the world
            Batteries = new Coordinates[int_NumOfBatteries];
            for (int i = 0; i < Batteries.Length; i++)
            {
                Batteries[i] = RandomCoordinates();
                World[Batteries[i].Y, Batteries[i].X] = (char)GAME_CHARACTER.CHR_BATTERY;
            }
        }

        // Mehthods
        public void PlayerMove(PLAYER_MOVE Move)
        {
            Coordinates FutureCoordinates;

            switch (Move)
            {
                case PLAYER_MOVE.W:
                    //Moving Up
                    FutureCoordinates.X = Player1.Coordinates.X;
                    FutureCoordinates.Y = Player1.Coordinates.Y - 1;
                    
                    break;
                case PLAYER_MOVE.S:
                    //Moving Down
                    FutureCoordinates.X = Player1.Coordinates.X;
                    FutureCoordinates.Y = Player1.Coordinates.Y + 1;

                    break;
                case PLAYER_MOVE.A:
                    //Moving Left
                    FutureCoordinates.X = Player1.Coordinates.X - 1;
                    FutureCoordinates.Y = Player1.Coordinates.Y;

                    break;
                case PLAYER_MOVE.E:
                    //Switching on/off light
                    Player1.FlickTorch();
                    return;
                default:
                    //Moving Right
                    FutureCoordinates.X = Player1.Coordinates.X + 1;
                    FutureCoordinates.Y = Player1.Coordinates.Y;
                    break;
            }

            //Checking if future coords are beyond the world
            if (!InWorld(FutureCoordinates.X, FutureCoordinates.Y))
                return;

            //Pickeing up battery functionality

            // 1. Clearing the player's old position
            // 2. Saving the player's new position & placing them in world
            World[Player1.Coordinates.Y, Player1.Coordinates.X] = (char)GAME_CHARACTER.CHR_EMPTY_SPACE;
            Player1.Coordinates = FutureCoordinates;
            World[Player1.Coordinates.Y, Player1.Coordinates.X] = (char)GAME_CHARACTER.CHR_PLAYER;

            Player1.PlayerMoved();
        }

        public void DisplayGame()
        {
            //Displaying Player details
            Console.WriteLine("Player Details" +
                              "\n---------------" +
                              "\n Spare Batteries : " + Player1.NumOfBatteries +
                              "\nTorch Percantage : [" + Player1.TorchPercantage + "}" +
                              "\n           Torch : " + (Player1.TorchOn ? "ON" : "OFF") + "\n\n");

            //Displaying world
            DisplayHorizontalWall();

            for (int x = 0; x < WorldSize; x++)
            {
                //Displaying left side wall
                Console.Write("|");

                // Include logic for torch on / off
                for (int y = 0; y < WorldSize; y++)
                    Console.Write(InPlayerRadius(y, x) || Player1.TorchOn 
                        ? World[x, y] 
                        : (char)GAME_CHARACTER.CHR_EMPTY_SPACE);

                //Displaying right side wall
                Console.Write("|");

                Console.WriteLine();
            }

            DisplayHorizontalWall();
        }

        //Helper methods
        private void DisplayHorizontalWall()
        {
            Console.Write(" ");
            for (int i = 0; i < WorldSize; i++)
                Console.Write("-");
            Console.Write("\n");
        }
        
        private bool InWorld(int X, int Y)
        {
            bool xInBounds = 0 <= X && X < WorldSize;
            bool yInBounds = 0 <= Y && Y < WorldSize;

            return xInBounds && yInBounds;
        }

        private bool InPlayerRadius(int X, int Y)
        {
            //min <= val <= max
            bool xInRadius = (Player1.Coordinates.X - 1) <= X && X <= (Player1.Coordinates.X + 1);
            bool yInRadius = (Player1.Coordinates.Y - 1) <= Y && Y <= (Player1.Coordinates.Y + 1);

            return xInRadius && yInRadius;
        }

        private Coordinates RandomCoordinates()
        {
            while(true)
            {
                int RandomX = GameRandomiser.Next(WorldSize);
                int RandomY = GameRandomiser.Next(WorldSize);

                if (World[RandomY, RandomX] == (char)GAME_CHARACTER.CHR_EMPTY_SPACE)
                    return new Coordinates(RandomX, RandomY);
            }
        }
    }
}
