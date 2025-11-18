using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aiden_FirstPlayable_GameProgramming
{
    internal class Program
    {
        static bool isPlaying = true;

        #region Player

        static int PlayerHealth = 100;
        static int PlayerAttack = 15;
        static int PlayerSpeed = 15;

        static (int, int) PlayerPosition = (0, 0);

        #endregion

        #region Enemy

        static int EnemyHealth = 75;
        static int EnemyAttack = 10;
        static int EnemySpeed = 2;

        static (int, int) EnemyPosition = (0, 0);

        #endregion

        #region Map

        static string path = "Map.txt";
        static string[] MapStringArray;

        static Char[][] MapChar;

        #endregion

        static void PrintMap()
        {

            MapStringArray = File.ReadAllLines(path);

            for (int i = 0; i < 12; i++)
            {
                MapChar = new char[i + 1][];

                for (int j = 0; j < 57; j++)
                {
                    MapChar[i][j] = MapStringArray[i][j];
                }
            }

            for (int i = 0; i < MapStringArray.GetLength(0); i++)
            {
                for (int j = 0; j < MapStringArray.GetLength(1); j++)
                {
                    if (MapChar[i][j] == '-')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    else if(MapChar[i][j] == '~')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    else if(MapChar[i][j] == '^')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    Console.Write(MapChar[i][j]);

                }

                Console.WriteLine();

            }
        }

        static void PlayerInput()
        {
            Console.ReadKey();
            isPlaying = false;
        }

        static void Main(string[] args)
        {
            while(isPlaying)
            {
                PrintMap();
                PlayerInput();
            }
        }
    }
}
