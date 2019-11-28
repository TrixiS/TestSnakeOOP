using System.Collections.Generic;

namespace TestSnake
{
    public class TextSnake : ISnake
    {
        public IList<IDrawble> Fragments { get; }
        public ISnakeHead Head { get; }

        public TextSnake(TextSnakeHead head, int startFragmentsCount, char fragmentChar)
        {
            this.Head = head;

            var fragments = new List<IDrawble>(startFragmentsCount);

            for (int i = 0; i < startFragmentsCount; i++)
            {
                fragments.Add(new TextSnakeFragment { FragmentChar = fragmentChar, Position = new Numeric2D(head.Position.X + (i + 1), head.Position.Y)});
            }

            this.Fragments = fragments;
        }

        public void Move(Numeric2D newPosition)
        {
            if (this.Head.Position == newPosition)
                return;

            Numeric2D lastFragmentPos = this.Head.Position;
            Numeric2D currentFragmentLastPos = Numeric2D.NoneExistent;

            this.Head.Position = newPosition;

            foreach (var fragment in this.Fragments)
            {
                currentFragmentLastPos = fragment.Position;
                fragment.Position = lastFragmentPos;
                lastFragmentPos = currentFragmentLastPos;
            }
        }
    }
}
