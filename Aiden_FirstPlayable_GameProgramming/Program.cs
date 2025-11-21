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
        static int EndGameWait = 2000;

        static ConsoleKeyInfo PlayerInput;

        #region Player

        static Char Player = '0';

        static (int, int) PlayerPos = (6, 12);
        static (int, int) NewPlayerPos;

        static int MaxPlayerHealth = 100;
        static int PlayerHealth = 100;
        static int PlayerAttack = 25;
        static int PlayerSpeed = 1;

        static (int, int) PlayerPosYClamp;
        static (int, int) PlayerPosXClamp;

        static int PlayerScore = 0;

        #endregion

        #region Enemy

        static Char Enemy = 'X';

        static int EnemyCount = 3;

        static int EnemyAttackSpeed = 200;
        static int[] EnemyAttackTime = { 0, 0, 0 };

        static int[] InitialEnemyPosY = { 11, 10, 9 };
        static int[] InitialEnemyPosX = { 45, 20, 54 };

        static int[] EnemyPosY = { 11, 10, 9 };
        static int[] EnemyPosX = { 45, 20, 54 };

        static float[] EnemyMoveY = { 11, 10, 9 };
        static float[] EnemyMoveX = { 45, 20, 54 };

        static int MaxEnemyHealth = 75;
        static int[] EnemyHealth = { 75, 75, 75 };
        static int EnemyAttack = 10;
        static float EnemySpeed = 0.25f;

        static (int, int) EnemyPosYClamp;
        static (int, int) EnemyPosXClamp;

        static int EnemyKillScore = 50;

        #endregion

        #region Map

        static string MapPath = "Map.txt";
        static string[] MapStringArray;

        static Char[][] MapChar;

        #endregion

        #region Leaderboard

        static string LeaderboardPath = "Leaderboard.txt";
        static string[] LeaderboardStringArray;

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
            MapStringArray = File.ReadAllLines(MapPath);
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
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(MapChar[PlayerPos.Item1][PlayerPos.Item2]);

            PlayerAttacking();

            PlayerPos = NewPlayerPos;

            Console.SetCursorPosition(PlayerPos.Item2+1, PlayerPos.Item1+1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Player);

        }

        static void PlayerAttacking()
        {
            for(int i = 0; i < EnemyCount; i++)
            {
                if (Math.Abs(EnemyPosY[i] - NewPlayerPos.Item1) < 1 && Math.Abs(EnemyPosX[i] - NewPlayerPos.Item2) < 1)
                {
                    EnemyHealth[i] -= PlayerAttack;

                    if (EnemyHealth[i] == 0)
                    {
                        PlayerScore += EnemyKillScore;
                        PlayerHealth = MaxPlayerHealth;

                        EnemyHealth[i] = MaxEnemyHealth;
                        EnemyPosY[i] = InitialEnemyPosY[i];
                        EnemyPosX[i] = InitialEnemyPosX[i];
                        EnemyMoveY[i] = InitialEnemyPosY[i];
                        EnemyMoveX[i] = InitialEnemyPosX[i];
                    }
                }
            }
        }

        static void EnemyMovement()
        {

            for (int i = 0; i < EnemyCount; i++)
            {
                int EnemyDistanceY = PlayerPos.Item1 - EnemyPosY[i];
                int EnemyDistanceX = PlayerPos.Item2 - EnemyPosX[i];

                float distance = (float)Math.Sqrt(EnemyDistanceX * EnemyDistanceX + EnemyDistanceY * EnemyDistanceY);

                if (distance > 0)
                {
                    EnemyMoveX[i] += EnemyDistanceX / distance * EnemySpeed;
                    EnemyMoveY[i] += EnemyDistanceY / distance * EnemySpeed;

                    int NewEnemyPosY = (int)Math.Round(EnemyMoveY[i]);
                    int NewEnemyPosX = (int)Math.Round(EnemyMoveX[i]);

                    NewEnemyPosY = Math.Max(EnemyPosYClamp.Item1, Math.Min(EnemyPosYClamp.Item2, NewEnemyPosY));
                    NewEnemyPosX = Math.Max(EnemyPosXClamp.Item1, Math.Min(EnemyPosXClamp.Item2, NewEnemyPosX));


                    if (MapChar[NewEnemyPosY][NewEnemyPosX] == '-')
                    {
                        Console.SetCursorPosition(EnemyPosX[i] + 1, EnemyPosY[i] + 1);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(MapChar[EnemyPosY[i]][EnemyPosX[i]]);

                        EnemyPosY[i] = NewEnemyPosY;
                        EnemyPosX[i] = NewEnemyPosX;
                    }

                    Console.SetCursorPosition(EnemyPosX[i] + 1, EnemyPosY[i] + 1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(Enemy);
                }
            }
        }


        static void EnemyAttacking()
        {
            int currentTime = Environment.TickCount;

            for (int i = 0; i < EnemyCount; i++)
            {
                if(Math.Abs(PlayerPos.Item1 - EnemyPosY[i]) < 1 && Math.Abs(PlayerPos.Item2 - EnemyPosX[i]) < 1)
                {
                    if(currentTime - EnemyAttackTime[i] >= EnemyAttackSpeed)
                    {
                        EnemyAttackTime[i] = currentTime;

                        PlayerHealth -= EnemyAttack;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(0, MapStringArray.Length + 2);
                        Console.Write($"PlayerHealth:                        \n");

                        if (PlayerHealth <= 0)
                        {
                            isDead = true;
                        }
                    }
                }
            }
        }

        static void Update()
        {

            PlayerPosYClamp = (1, MapStringArray.Length);
            PlayerPosXClamp = (1, MapStringArray[1].Length);

            EnemyPosYClamp = (1, MapStringArray.Length - 1);
            EnemyPosXClamp = (1, MapStringArray[1].Length - 1);

            PrintMap();

            Console.SetCursorPosition(PlayerPos.Item2 + 1, PlayerPos.Item1 + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Player);

            for(int i = 0; i < EnemyCount; i++)
            {
                Console.SetCursorPosition(EnemyPosX[i] + 1, EnemyPosY[i] + 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(Enemy);
            }

            while (!isDead)
            {
                EnemyMovement();

                if (Console.KeyAvailable)
                {
                    PlayerMovement();
                }

                EnemyAttacking();

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, MapStringArray.Length + 2);
                Console.Write($"Player Health:{PlayerHealth}\n");
                Console.Write($"Player Score:{PlayerScore}\n\n");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Enemy1 Health:{EnemyHealth[0]}\n");
                Console.Write($"Enemy2 Health:{EnemyHealth[1]}\n");
                Console.Write($"Enemy3 Health:{EnemyHealth[2]}\n");
                Thread.Sleep(FrameTime);
            }
        }

        static void Menu()
        {
            Console.WriteLine("Welcome To.... Uhhh\n");
            Thread.Sleep(2000);
            Console.WriteLine("Uhm");
            Thread.Sleep(2000);
            Console.Clear();

            Console.WriteLine("Yeah I Don't Know, I Didn't Come Up With A Name");
            Thread.Sleep(1000);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Yeah I Don't Know, I Didn't Come Up With A Name.");
            Thread.Sleep(1000);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Yeah I Don't Know, I Didn't Come Up With A Name..");
            Thread.Sleep(1000);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Yeah I Don't Know, I Didn't Come Up With A Name...");
            Thread.Sleep(1000);
            Console.Clear();

            Console.WriteLine("Press Any Key To Play...");
            Console.ReadKey(true);
            Console.Clear();
        }

        static void ShowLeaderboard(bool AddScore)
        {
            bool Answer = false;

            if(AddScore)
            {
                while (!Answer)
                {
                    Console.Clear();
                    Console.WriteLine("Please Input 3 Initials:");
                    Console.SetCursorPosition(25, 0);

                    string PlayerInput = Console.ReadLine().ToUpper();

                    if (PlayerInput.Length != 3 || PlayerInput.Any(Char.IsDigit))
                    {
                        Console.Clear();
                        Console.CursorVisible = false;
                        Console.WriteLine("Your Input Was Invalid... Please Try Again.\n");
                        Console.WriteLine("Press Any Key To Continue...");
                        Console.SetCursorPosition(29, 2);
                        Console.ReadKey(true);
                        Console.Clear();
                    }
                    else
                    {
                        Answer = true;
                        File.AppendAllText(LeaderboardPath, $"{PlayerScore}: {PlayerInput},");
                        Console.CursorVisible = false;
                        Console.Clear();
                    }
                }
            }

            Console.Clear();
            Console.CursorVisible = false;
            string ScoreFromFile = File.ReadAllText(LeaderboardPath);
            LeaderboardStringArray = ScoreFromFile.Split(',');
            Array.Sort(LeaderboardStringArray);

            Console.WriteLine("Leaderboard:\n");

            for(int i = 0; i < LeaderboardStringArray.Length; i++)
            {
                Console.WriteLine($"{LeaderboardStringArray[i]}\n");
            }

            Console.WriteLine("Press Any Key To Exit...");
            Console.ReadKey(true);

        }

        static void EndGame()
        {
            bool Answer = false;

            Console.ForegroundColor= ConsoleColor.Red;

            Console.Clear();
            Console.WriteLine("Game Over!");
            Thread.Sleep(EndGameWait);
            Console.Clear();

            while (!Answer)
            {
                Console.Write($"Your Score Was: {PlayerScore}!\n\n");

                Console.Write($"Would You Like To See The Leaderboard?\n");
                Console.Write($"Y/N:");

                Console.SetCursorPosition(5, 3);
                Console.CursorVisible = true;
                string PlayerInput = Console.ReadLine().ToUpper();

                if (PlayerInput == "Y")
                {
                    while(!Answer)
                    {
                        Console.Clear();
                        Console.WriteLine("Would You Like To Add Your Score To The Leaderboard?\n");
                        Console.Write($"Y/N:");
                        Console.SetCursorPosition(5, 2);
                        PlayerInput = Console.ReadLine().ToUpper();

                        if (PlayerInput == "Y")
                        {
                            Answer = true;
                            ShowLeaderboard(true);
                        }
                        else if (PlayerInput == "N")
                        {
                            Answer = true;
                            ShowLeaderboard(false);
                        }
                        else
                        {
                            Console.Clear();
                            Console.CursorVisible = false;
                            Console.WriteLine("Your Input Was Invalid... Please Try Again.\n");
                            Console.WriteLine("Press Any Key...");
                            Console.SetCursorPosition(29, 2);
                            Console.ReadKey(true);
                            Console.Clear();
                        }
                    }
                }
                else if (PlayerInput == "N")
                {
                    Answer = true;
                    Console.CursorVisible= false;
                    Console.Clear();
                    Console.WriteLine("Well Okay Then...\n");
                    Thread.Sleep(2000);
                    Console.WriteLine("Didn't Want To Show You Anyway\n");
                    Thread.Sleep(2000);
                    Console.WriteLine("Press Any Key...");
                    Console.ReadKey();
                }
                else
                {
                    Console.Clear();
                    Console.CursorVisible = false;
                    Console.WriteLine("Your Input Was Invalid... Please Try Again.\n");
                    Console.WriteLine("Press Any Key...");
                    Console.SetCursorPosition(29, 2);
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Menu();

            InitializeMap();
            Update();

            EndGame();
        }
    }
}