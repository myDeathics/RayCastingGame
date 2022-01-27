using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static System.Math;
using System.Threading;

namespace RayCastingGame
{
    internal class Program : MethodsAndStructures
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Game.Render();
            }
        }

        class Game : MethodsAndStructures
        {
            private static Player player = new Player(new vec2(3, 3), 90);
            private static char[] buffer = new char[width * height + 1];
            private static char color = ' ';

            public static void Render()
            {
                ConsoleKey key = Console.ReadKey().Key;
                player.Control(key);

                for (int x = 0;x < width;x++)
                {
                    float rayAngle = (player.angle - player.FOV) + (float)x / (float)width * player.FOV;
                    float posOnRay = 0;
                    for (;posOnRay < 15;posOnRay += 0.01f)
                    {
                        vec2 posRay = new vec2((int)Round(player.pos.x + Cos(rayAngle) * posOnRay), (int)Round(player.pos.y + Sin(rayAngle) * posOnRay));

                        if (posRay.x < 0 || posRay.x > map[0].Length - 1 || posRay.y < 0 || posRay.y > map.Length - 1)
                        {
                            posOnRay = 15;
                            break;
                        }
                        else if (map[(int)posRay.y][(int)posRay.x] == '#')  break;

                        if (posOnRay >= 15) posOnRay = 15;
                    }
                    int Ceiling = (int)((height / 2.0) - height / ((float)posOnRay));
                    int Floor = height - Ceiling;

                    color = ' ';
                    GetWallCharByDist(ref color, posOnRay, gradientWall, 15);

                    for (int y = 0;y < height;y++)
                    {
                        if (y <= Ceiling) { buffer[y * width + x] = ' '; }
                        else if (y > Ceiling && y <= Floor) { buffer[y * width + x] = color; }
                        else
                        {
                            GetFloorCharByDist(ref color, y, gradientFloor);
                            buffer[y * width + x] = color;
                        }
                    }
                }
                ConsoleHelper.WriteToBufferAt(new string(buffer), 0, 0);
            }
        }
    }
    class MethodsAndStructures
    {
        public struct vec2
        {
            public double x { get; set; }
            public double y { get; set; }

            public vec2(double x, double y)
            {
                this.x = x;
                this.y = y;
            }

            public static vec2 operator +(vec2 a, vec2 b) { return new vec2(a.x + b.x, a.y + b.y); }
            public static vec2 operator +(vec2 a, double b) { return new vec2(a.x + b, a.y + b); }
            public static vec2 operator +(double b, vec2 a) { return new vec2(a.x + b, a.y + b); }

            public static vec2 operator *(vec2 a, vec2 b) { return new vec2(a.x * b.x, a.y * b.y); }
            public static vec2 operator *(vec2 a, double b) { return new vec2(a.x * b, a.y * b); }
            public static vec2 operator *(double b, vec2 a) { return new vec2(a.x * b, a.y * b); }

            public static vec2 operator /(vec2 a, vec2 b) { return new vec2(a.x / b.x, a.y / b.y); }
            public static vec2 operator /(vec2 a, double b) { return new vec2(a.x / b, a.y / b); }
            public static vec2 operator /(double b, vec2 a) { return new vec2(b / a.x, b / a.y); }

            public static vec2 operator -(vec2 a, vec2 b) { return new vec2(a.x - b.x, a.y - b.y); }
            public static vec2 operator -(vec2 a, double b) { return new vec2(a.x - b, a.y - b); }
            public static vec2 operator -(double b, vec2 a) { return new vec2(b - a.x, b - a.y); }
        }
        public struct vec3
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }

            public vec3(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public static vec3 operator +(vec3 a, vec3 b) { return new vec3(a.x + b.x, a.y + b.y, a.z + b.z); }
            public static vec3 operator +(vec3 a, double b) { return new vec3(a.x + b, a.y + b, a.z + b); }
            public static vec3 operator +(double b, vec3 a) { return new vec3(a.x + b, a.y + b, a.z + b); }

            public static vec3 operator *(vec3 a, vec3 b) { return new vec3(a.x * b.x, a.y * b.y, a.z * b.z); }
            public static vec3 operator *(vec3 a, double b) { return new vec3(a.x * b, a.y * b, a.z * b); }
            public static vec3 operator *(double b, vec3 a) { return new vec3(a.x * b, a.y * b, a.z * b); }

            public static vec3 operator /(vec3 a, vec3 b) { return new vec3(a.x / b.x, a.y / b.y, a.z / b.z); }
            public static vec3 operator /(vec3 a, double b) { return new vec3(a.x / b, a.y / b, a.z / b); }
            public static vec3 operator /(double b, vec3 a) { return new vec3(b / a.x, b / a.y, b / a.z); }

            public static vec3 operator -(vec3 a, vec3 b) { return new vec3(a.x - b.x, a.y - b.y, a.z - b.z); }
            public static vec3 operator -(vec3 a, double b) { return new vec3(a.x - b, a.y - b, a.z - b); }
            public static vec3 operator -(double b, vec3 a) { return new vec3(b - a.x, b - a.y, b - a.z); }

            public static bool operator ==(vec3 vec3, vec3 b)
            {
                if (vec3 == b)
                {
                    return true;
                }
                return false;
            }
            public static bool operator !=(vec3 vec3, vec3 b)
            {
                if (vec3 != b)
                {
                    return true;
                }
                return false;
            }
        }
        protected static ushort width = (ushort)Console.WindowWidth;
        protected static ushort height = (ushort)Console.WindowHeight;
        public static readonly string[] map = {"#################",
                                               "#.P.............#",
                                               "#...............#",
                                               "#......####.....#",
                                               "#...............#",
                                               "#...............#",
                                               "#.....#....#....#",
                                               "#.....#....#....#",
                                               "#.....#....#....#",
                                               "#.....#....#....#",
                                               "#...............#",
                                               "#...............#",
                                               "#################"};
        public static class ConsoleHelper
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
            private static extern bool WriteConsoleOutputCharacter(IntPtr hConsoleOutput, string lpCharacter, uint nLength, Point16 dwWriteCoord, out uint lpNumberOfCharsWritten);
            [DllImport("kernel32.dll")]
            private static extern IntPtr GetStdHandle(int nStdHandle);

            private const int STD_OUTPUT_HANDLE = -11;
            private const int STD_INPUT_HANDLE = -10;
            private const int STD_ERROR_HANDLE = -12;
            private static readonly IntPtr _stdOut = GetStdHandle(STD_OUTPUT_HANDLE);

            [StructLayout(LayoutKind.Sequential)]
            private struct Point16
            {
                public short X;
                public short Y;

                public Point16(short x, short y)
                    => (X, Y) = (x, y);
            };

            public static void WriteToBufferAt(string text, int x, int y)
            {
                WriteConsoleOutputCharacter(_stdOut, text, (uint)text.Length, new Point16((short)x, (short)y), out uint _);
            }
            public static void SetConsoleSize(ushort Width, ushort Height)
            {
                width = Width;
                height = (ushort)(Height + 1);
                Console.SetWindowSize(width, height);
                Console.SetBufferSize(width, height + 1);
            }
        }
        public static string gradientWall = "█▓▒░ ";
        public static string gradientFloor = "@#;:.";
        public static void GetWallCharByDist(ref char symbol, float distance, string gradientOf5El, int depth)
        {
            if (distance <= 2) { symbol = gradientOf5El[0]; }
            else if (distance <= 4) { symbol = gradientOf5El[1]; }
            else if (distance <= 6) { symbol = gradientOf5El[2]; }
            else if (distance <= 8) { symbol = gradientOf5El[3]; }
            else if (distance <= 10) { symbol = gradientOf5El[4]; }
        }
        public static void GetFloorCharByDist(ref char color, float y, string gradientOf5El)
        {
            if (y > 80) { color = gradientOf5El[0]; }
            else if (y > 75) { color = gradientOf5El[1]; }
            else if (y > 70) { color = gradientOf5El[2]; }
            else if (y > 65) { color = gradientOf5El[3]; }
            else if (y > 60) { color = gradientOf5El[4]; }
        }
    }
    class Player : MethodsAndStructures
    {
        public vec2 pos;
        public float FOV = (float)PI / 3.0f;
        public float angle = 350 * (float)PI / 180;

        public Player(vec2 pos, int FOV)
        {
            this.pos = pos;
            //this.FOV = FOV;
        }

        public void Control(ConsoleKey key)
        {
            float speed = 0.1f;
            float speedRotate = 0.1f;
            switch (key)
            {
                case ConsoleKey.W:
                    pos.x += speed * Cos(angle);
                    pos.y += speed * Sin(angle);
                    if (map[(int)pos.y][(int)pos.x] == '#')
                    {
                        pos.x -= speed * Cos(angle);
                        pos.y -= speed * Sin(angle);
                    }
                    break;

                case ConsoleKey.A:
                    angle -= speedRotate;
                    break;

                case ConsoleKey.S:
                    pos.x -= speed * Cos(angle);
                    pos.y -= speed * Sin(angle);
                    if (map[(int)pos.y][(int)pos.x] == '#')
                    {
                        pos.x += speed * Cos(angle);
                        pos.y += speed * Sin(angle);
                    }
                    break;

                case ConsoleKey.D:
                    angle += speedRotate;
                    break;
            }
        }
    }
}
