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
                int int_NumberOfBAatteries = Convert.ToInt32(args[5]);
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
}
