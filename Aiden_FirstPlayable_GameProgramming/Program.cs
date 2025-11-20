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

        static int FrameTime = 35;

        static ConsoleKeyInfo PlayerInput;

        #region Player

        static Char Player = '0';

        static (int, int) PlayerPos = (6, 12);
        static (int, int) NewPlayerPos;

        static int PlayerHealth = 100;
        static int PlayerAttack = 25;
        static int PlayerSpeed = 1;

        static (int, int) PlayerPosYClamp;
        static (int, int) PlayerPosXClamp;

        #endregion

        #region Enemy

        static Char Enemy = 'X';

        static (int, int) EnemyPos = (11, 45);
        static (int, int) NewEnemyPos;

        static float EnemyPosY = EnemyPos.Item1;
        static float EnemyPosX = EnemyPos.Item2;

        static int EnemyHealth = 75;
        static int EnemyAttack = 10;
        static int EnemyAttackSpeed = 200;
        static float EnemySpeed = 0.25f;

        #endregion

        #region Map

        static string path = "Map.txt";
        static string[] MapStringArray;

        static Char[][] MapChar;

        #endregion

        static void PrintHorizontalBorder()
        {
            Console.Write("*");

            for (int i = 0; i < MapStringArray[1].Length; i++)
            {
                Console.Write("-");
            }

            Console.Write("*");
        }

        static void PrintMap()
        {
            PrintHorizontalBorder();

            Console.WriteLine();

            for (int i = 0; i < MapStringArray.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
                for (int j = 0; j < MapChar[i].Length; j++)
                {
                    if (MapChar[i][j] == '-')
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
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");

                Console.WriteLine();

            }

            PrintHorizontalBorder();
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

            NewPlayerPos = PlayerPos;

            switch (key)
            {
                case ConsoleKey.W:

                    if (PlayerPos.Item1 > PlayerPosYClamp.Item1 - PlayerSpeed && !(MapChar[PlayerPos.Item1 - PlayerSpeed][PlayerPos.Item2] == '^') && !(MapChar[PlayerPos.Item1 - PlayerSpeed][PlayerPos.Item2] == '~'))
                    {
                        NewPlayerPos.Item1 -= PlayerSpeed;
                    }

                    break;

                case ConsoleKey.S:

                    if (PlayerPos.Item1 < PlayerPosYClamp.Item2 - PlayerSpeed && !(MapChar[PlayerPos.Item1 + PlayerSpeed][PlayerPos.Item2] == '^') && !(MapChar[PlayerPos.Item1 + PlayerSpeed][PlayerPos.Item2] == '~'))
                    {
                        NewPlayerPos.Item1 += PlayerSpeed;
                    }

                    break;

                case ConsoleKey.A:

                    if (PlayerPos.Item2 > PlayerPosXClamp.Item1 - PlayerSpeed && !(MapChar[PlayerPos.Item1][PlayerPos.Item2 - PlayerSpeed] == '^') && !(MapChar[PlayerPos.Item1][PlayerPos.Item2 - PlayerSpeed] == '~'))
                    {
                        NewPlayerPos.Item2 -= PlayerSpeed;
                    }

                    break;

                case ConsoleKey.D:

                    if (PlayerPos.Item2 < PlayerPosXClamp.Item2 - PlayerSpeed && !(MapChar[PlayerPos.Item1][PlayerPos.Item2 + PlayerSpeed] == '^') && !(MapChar[PlayerPos.Item1][PlayerPos.Item2 + PlayerSpeed] == '~'))
                    {

                        NewPlayerPos.Item2 += PlayerSpeed;
                    }

                    break;
            }

            Console.SetCursorPosition(PlayerPos.Item2+1, PlayerPos.Item1+1);
            if(MapChar[PlayerPos.Item1][PlayerPos.Item2] == '^')
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else if (MapChar[PlayerPos.Item1][PlayerPos.Item2] == '~')
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else if (MapChar[PlayerPos.Item1][PlayerPos.Item2] == '-')
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            Console.Write(MapChar[PlayerPos.Item1][PlayerPos.Item2]);

            PlayerPos = NewPlayerPos;

            Console.SetCursorPosition(PlayerPos.Item2+1, PlayerPos.Item1+1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Player);

        }

        static void EnemyMovement()
        {
            int EnemyDistanceY = PlayerPos.Item1 - EnemyPos.Item1;
            int EnemyDistanceX = PlayerPos.Item2 - EnemyPos.Item2;

            float Distance = (float)Math.Sqrt(EnemyDistanceX * EnemyDistanceX + EnemyDistanceY * EnemyDistanceY);

            if(Distance > 0)
            {
                EnemyPosY += EnemyDistanceY / Distance * EnemySpeed;
                EnemyPosX += EnemyDistanceX / Distance * EnemySpeed;

                float NewEnemyPosY = (float)Math.Round(EnemyPosY);
                float NewEnemyPosX = (float)Math.Round(EnemyPosX);

                if (MapChar[(int)NewEnemyPosY][(int)NewEnemyPosX] == '-')
                {
                    Console.SetCursorPosition(EnemyPos.Item2 + 1, EnemyPos.Item1 + 1);
                    if (MapChar[EnemyPos.Item1][EnemyPos.Item2] == '^')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if (MapChar[EnemyPos.Item1][EnemyPos.Item2] == '~')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    else if (MapChar[EnemyPos.Item1][EnemyPos.Item2] == '-')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    Console.Write(MapChar[EnemyPos.Item1][EnemyPos.Item2]);

                    EnemyPos = ((int)NewEnemyPosY, (int)NewEnemyPosX);
                }
                else
                {
                    EnemyPosY = EnemyPos.Item1;
                    EnemyPosX = EnemyPos.Item2;
                }

                Console.SetCursorPosition(EnemyPos.Item2 + 1, EnemyPos.Item1 + 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(Enemy);

            }

        }

        static void EnemyAttacking()
        {
            if (Math.Abs(PlayerPos.Item1 - EnemyPos.Item1) < 1 && Math.Abs(PlayerPos.Item2 - EnemyPos.Item2) < 1)
            {
                PlayerHealth -= EnemyAttack;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, MapStringArray.Length + 2);
                Console.Write("PlayerHealth:                \n");

                if (PlayerHealth <= 0)
                {
                    isDead = true;
                }

                Thread.Sleep(EnemyAttackSpeed);
            }
        }

        static void Update()
        {

            PlayerPosYClamp = (1, MapStringArray.Length);
            PlayerPosXClamp = (1, MapStringArray[1].Length);

            PrintMap();

            Console.SetCursorPosition(PlayerPos.Item2 + 1, PlayerPos.Item1 + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Player);

            Console.SetCursorPosition(EnemyPos.Item2 + 1, EnemyPos.Item1 + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Enemy);

            while (!isDead)
            {
                EnemyMovement();

                if (Console.KeyAvailable)
                {
                    PlayerMovement();
                }

                EnemyAttacking();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, MapStringArray.Length + 2);
                Console.Write($"PlayerHealth:{PlayerHealth}\n");
                Console.Write($"EnemyHealth:{EnemyHealth}");
                Thread.Sleep(FrameTime);
            }
        }

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            InitializeMap();
            Update();

            Console.SetCursorPosition(24, 15);
            Console.WriteLine("Game Over!");
            Thread.Sleep(2000);
            Console.ReadKey(true);
        }
    }
}
