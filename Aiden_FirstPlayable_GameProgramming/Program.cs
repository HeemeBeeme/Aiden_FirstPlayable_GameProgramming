using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aiden_FirstPlayable_GameProgramming
{
    internal class Program
    {
        static bool isDead = false;

        static int FrameTime = 100;

        static ConsoleKeyInfo PlayerInput;

        #region Player

        static Char Player = '0';

        static (int, int) PlayerPos = (6, 12);
        static (int, int) NewPlayerPos = (0, 0);

        static int PlayerHealth = 100;
        static int PlayerAttack = 15;
        static int PlayerSpeed = 15;

        static (int, int) PlayerPosYClamp ;
        static (int, int) PlayerPosXClamp;

        #endregion

        #region Enemy

        static Char Enemy = 'X';

        static (int, int) EnemyPos = (11, 45);
        static (int, int) NewEnemyPos = (0, 0);

        static int EnemyHealth = 75;
        static int EnemyAttack = 10;
        static int EnemySpeed = 2;

        #endregion

        #region Map

        static string path = "Map.txt";
        static string[] MapStringArray;

        static Char[][] MapChar;

        #endregion

        static void PrintMap()
        {
            for (int i = 0; i < MapStringArray.Length; i++)
            {
                for (int j = 0; j < MapChar[i].Length; j++)
                {
                    (int, int) MapTuple = (i, j);

                    if (MapTuple == PlayerPos)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(Player);
                    }
                    else if (MapTuple == EnemyPos)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Enemy);
                    }
                    else if (MapChar[i][j] == '-')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(MapChar[i][j]);
                    }
                    else if (MapChar[i][j] == '~')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write(MapChar[i][j]);
                    }
                    else if (MapChar[i][j] == '^')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(MapChar[i][j]);
                    }

                }

                Console.WriteLine();

            }
        }

        static void InitializeMap()
        {
            MapStringArray = File.ReadAllLines(path);
            MapChar = new char[MapStringArray.Length][];

            for (int i = 0; i < MapStringArray.Length; i++)
            {
                MapChar[i] = new char[MapStringArray[i].Length];

                for (int j = 0; j < MapStringArray[i].Length; j++)
                {
                    MapChar[i][j] = MapStringArray[i][j];
                }
            }

        }

        static void PlayerMovement()
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.W:

                    if (PlayerPos.Item1 > PlayerPosYClamp.Item1 - 1 && !(MapChar[PlayerPos.Item1-1][PlayerPos.Item2] == '^') && !(MapChar[PlayerPos.Item1-1][PlayerPos.Item2] == '~'))
                    {
                        PlayerPos.Item1--;
                        NewPlayerPos = PlayerPos;
                    }

                    break;

                case ConsoleKey.S:

                    if (PlayerPos.Item1 < PlayerPosYClamp.Item2 - 1 && !(MapChar[PlayerPos.Item1+1][PlayerPos.Item2] == '^') && !(MapChar[PlayerPos.Item1+1][PlayerPos.Item2] == '~'))
                    {
                        PlayerPos.Item1++;
                        NewPlayerPos = PlayerPos;
                    }

                    break;

                case ConsoleKey.A:

                    if (PlayerPos.Item2 > PlayerPosXClamp.Item1 - 1 && !(MapChar[PlayerPos.Item1][PlayerPos.Item2-1] == '^') && !(MapChar[PlayerPos.Item1][PlayerPos.Item2-1] == '~'))
                    {
                        PlayerPos.Item2--;
                        NewPlayerPos = PlayerPos;
                    }

                    break;

                case ConsoleKey.D:

                    if (PlayerPos.Item2 < PlayerPosXClamp.Item2 - 1 && !(MapChar[PlayerPos.Item1][PlayerPos.Item2+1] == '^') && !(MapChar[PlayerPos.Item1][PlayerPos.Item2+1] == '~'))
                    {
                        PlayerPos.Item2++;
                        NewPlayerPos = PlayerPos;
                    }

                    break;
            }
                
        }

        static void Update()
        {
            PlayerPosYClamp = (1, MapStringArray.Length);
            PlayerPosXClamp = (1, MapStringArray[1].Length);

            while (!isDead)
            {
                PrintMap();
                PlayerMovement();
                Console.Clear();

            }
        }

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            InitializeMap();
            Update();

            Console.WriteLine("Game Over!");
        }
    }
}
