using System.Collections.Generic;

namespace TestSnake
{
    public interface ISnake
    {
        ISnakeHead Head { get; }
        IList<IDrawble> Fragments { get; }
        void Move(Numeric2D newPosition);
    }
}
