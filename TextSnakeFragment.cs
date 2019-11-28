using System;

namespace TestSnake
{
    public class TextSnakeFragment : IDrawble
    {
        public Numeric2D Position { get; set; }
        public char FragmentChar { get; set; }
    }
}
