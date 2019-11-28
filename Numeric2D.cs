using System;

namespace TestSnake
{
    public struct Numeric2D : IEquatable<Numeric2D>
    {
        public int X;
        public int Y;

        public static readonly Numeric2D NoneExistent = new Numeric2D(-1, -1);

        public Numeric2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Numeric2D RandomIn()
        {
            Random random = new Random();

            return new Numeric2D(random.Next(0, this.X), random.Next(0, this.Y));
        }

        public bool Equals(Numeric2D other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public static bool operator ==(Numeric2D first, Numeric2D second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Numeric2D first, Numeric2D second)
        {
            return !first.Equals(second);
        }

        public override string ToString()
        {
            return $"({this.X}; {this.Y})";
        }
    }
}
