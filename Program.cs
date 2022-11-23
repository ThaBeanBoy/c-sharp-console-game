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
                Game.DsiplayWorld();
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
        private bool Won;
        private const int PitSpawnChance = 15;

        //Constructor & Destructor
        public GameWorld(int int_GameWorldSize, int int_NumOfBatteries)
        {
            Random Rnd = new Random();

            // Setting properties
            Won = false;
            WorldSize = int_GameWorldSize;
            NumOfBatteries = int_NumOfBatteries;
            World = new char[int_GameWorldSize, int_GameWorldSize];

            //Creating the display
            for (int x = 0; x < WorldSize; x++)
                for(int y = 0; y < WorldSize; y++)
                {
                    // setting traps
                    if (Rnd.Next(100) < PitSpawnChance)
                        World[x, y] = 'X';
                    else
                        World[x, y] = ' ';
                }
        }

        // Mehthods
        public void DsiplayWorld()
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
    }
}
