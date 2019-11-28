using System;

namespace TestSnake
{
    class Program
    {
        public static void Main(string[] args)
        {
            SnakeTest();
        }

        private static void Numeric2DTest()
        {
            Numeric2D numeric = new Numeric2D(5, 5);
            Numeric2D numericRandonIn = numeric.RandomIn();

            TextGround ground = new TextGround('.', numeric.X, numeric.Y);
            TextGround randomInGround = new TextGround('.', numericRandonIn.X, numericRandonIn.Y);

            Console.WriteLine(numeric.ToString());
            Console.WriteLine(numericRandonIn.ToString());

            Console.WriteLine(ground.ToString());
            Console.WriteLine(randomInGround.ToString());
        }

        private static void SnakeTest()
        {
            Console.Title = "Snake";
            Console.CursorVisible = false;

            const char snakeFragmentChar = '=';

            SnakeTextGround ground = new SnakeTextGround('.', 50, 20);

            TextSnakeHead snakeHead = new TextSnakeHead
            {
                FragmentChar = '*',
                Position = new Numeric2D(6, 9),
                Health = 100
            };

            TextSnake snake = new TextSnake(snakeHead, 2, snakeFragmentChar);

            ConsoleKey handledKey;
            Numeric2D newSnakePos = snake.Head.Position;

            ground.ToDraw.Add(snakeHead);
            ground.ToDraw.AddRange(snake.Fragments);

            while (snake.Head.Health > 0)
            {
                handledKey = Console.ReadKey().Key;

                switch (handledKey)
                {
                    case ConsoleKey.DownArrow:
                        newSnakePos.Y++;
                        break;
                    case ConsoleKey.UpArrow:
                        newSnakePos.Y--;
                        break;
                    case ConsoleKey.LeftArrow:
                        newSnakePos.X--;
                        break;
                    case ConsoleKey.RightArrow:
                        newSnakePos.X++;
                        break;

                    default:
                        continue;
                }

                Console.Clear();

                if (newSnakePos.Y >= ground.Heigth)
                    newSnakePos.Y = 0;
                else if (newSnakePos.X >= ground.Width)
                    newSnakePos.X = 0;
                else if (newSnakePos.Y < 0)
                    newSnakePos.Y = ground.Heigth;
                else if (newSnakePos.X < 0)
                    newSnakePos.X = ground.Width;

                snake.Move(newSnakePos);

                foreach (var fragment in ground.ToDraw)
                {
                    if (fragment.Position == snake.Head.Position && fragment != snake.Head && !snake.Fragments.Contains(fragment))
                    {
                        IDrawble newFragmentToDraw = new TextSnakeFragment
                        {
                            Position = snake.Head.Position,
                            FragmentChar = snakeFragmentChar
                        };

                        snake.Fragments.Add(newFragmentToDraw);
                        ground.ToDraw.Add(newFragmentToDraw);

                        ground.ToDraw.Remove(fragment);
                        break;
                    }
                    else if (snake.Fragments.Contains(fragment) && fragment.Position == snake.Head.Position && fragment != snake.Head)
                    {
                        snake.Head.Health = 0;

                        break;
                    }
                }

                Console.WriteLine(ground.ToString());
            }

            Console.Clear();
            Console.WriteLine("GAME OVER!");
        }     
    }
}
