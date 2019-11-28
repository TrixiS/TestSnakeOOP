using System;
using System.Collections.Generic;
using System.Text;

namespace TestSnake
{
    public class TextGround
    {
        public int Width { get; }
        public int Heigth { get; }

        public char GroundChar { get; }

        public IReadOnlyCollection<IReadOnlyCollection<char>> Area => this.area;

        protected readonly char[][] area;

        public TextGround(char groundChar, int width, int heigth)
        {
            this.Width = width;
            this.Heigth = heigth;
            this.GroundChar = groundChar;

            char[][] area = new char[heigth][];

            for (int y = 0; y < heigth; y++)
            {
                char[] areaSegment = new char[width];

                for (int x = 0; x < width; x++)
                {
                    areaSegment[x] = groundChar;
                }

                area[y] = areaSegment;
            }

            this.area = area;
        }

        public override string ToString()
        {
            StringBuilder areaStringBuilder = new StringBuilder();

            for (int i = 0; i < this.Heigth; i++)
            {
                areaStringBuilder.Append(new String(this.area[i]));
                areaStringBuilder.Append(Environment.NewLine);
            }

            return areaStringBuilder.ToString();
        }

        protected void SetCharAt(char chr, int x, int y)
        {
            this.area[y][x] = chr;
        }
    }
}
