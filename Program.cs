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
                Console.Write("Move : ");

                // Reaon I put an index of 0 is because I want the program to just take the first character in the string
                // I used ToLower() incase the player inputs any capital letters
                char Move = Console.ReadLine().ToLower()[0];

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
            public int NumOfBatteries { get; }

            //Player constructor
            public Player(Coordinates newCoordinates)
            {
                Coordinates = newCoordinates;
                NumOfBatteries = 1;
            }
        }
        public enum PLAYER_MOVE
        {
            W,
            A,
            S,
            D
        }
        private Player Player1 { get; }
        Coordinates[] Batteries;

        private int NumOfTurns;
        private bool Won;
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
            switch (Move)
            {
                case PLAYER_MOVE.W:
                    Console.WriteLine("Moved up");
                    break;
                case PLAYER_MOVE.S:
                    Console.WriteLine("Moved down");
                    break;
                case PLAYER_MOVE.A:
                    Console.WriteLine("Moved left");
                    break;
                case PLAYER_MOVE.D:
                    Console.WriteLine("Moved right");
                    break;
            }
        }

        public void DisplayWorld()
        {
            DsiplayVerticalWall();

            for (int x = 0; x < WorldSize; x++)
            {
                //Displaying left side wall
                Console.Write("|");

                for (int y = 0; y < WorldSize; y++)
                    Console.Write(World[x, y]);

                //Displaying right side wall
                Console.Write("|");

                Console.WriteLine();
            }

            DsiplayVerticalWall();
        }

        //Helper methods
        private void DsiplayVerticalWall()
        {
            Console.Write(" ");
            for (int i = 0; i < WorldSize; i++)
                Console.Write("-");
            Console.Write("\n");
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
