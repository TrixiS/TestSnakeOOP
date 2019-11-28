using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace TestSnake
{
    public class SnakeTextGround : TextGround
    {
        public List<IDrawble> ToDraw { get; }

        public SnakeTextGround(char groundChar, int width, int heigth) : base(groundChar, width, heigth)
        {
            this.ToDraw = new List<IDrawble>();
        }

        public override string ToString()
        {
            StringBuilder groundStringBuilder = new StringBuilder(this.Heigth * 2);
            StringBuilder rowStringBuilder = new StringBuilder(this.Width);

            IEnumerable<IDrawble> fragments;
            Numeric2D fragmentPositionToCheck = Numeric2D.NoneExistent;

            for (int y = 0; y < this.Heigth; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    fragmentPositionToCheck.X = x;
                    fragmentPositionToCheck.Y = y;

                    fragments = this.ToDraw.Where(fragment => fragment.Position == fragmentPositionToCheck);

                    if (fragments.Count() > 0)
                    {
                        rowStringBuilder.Append(fragments.ElementAt(0).FragmentChar);
                    }
                    else
                    {
                        rowStringBuilder.Append(this.area[y][x]);
                    }
                }

                groundStringBuilder.Append(rowStringBuilder.ToString());
                groundStringBuilder.Append(Environment.NewLine);

                rowStringBuilder.Clear();
            }

            return groundStringBuilder.ToString();
        }
    }
}
