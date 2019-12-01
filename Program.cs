using System;
using System.Threading;
using System.Threading.Tasks;

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

        private enum SnakeDirection : int
        {
            Forward, 
            Right, 
            Left, 
            Back
        }

        private static SnakeDirection Direction = SnakeDirection.Forward;

        private static async void HandleConsoleInputAsync()
        {
            await Task.Yield();

            ConsoleKey handledKey = Console.ReadKey().Key;
            SnakeDirection newDirection = Direction;

            switch (handledKey)
            {
                case ConsoleKey.UpArrow:
                    newDirection = SnakeDirection.Back;
                    break;
                case ConsoleKey.DownArrow:
                    newDirection = SnakeDirection.Forward;
                    break;
                case ConsoleKey.RightArrow:
                    newDirection = SnakeDirection.Right;
                    break;
                case ConsoleKey.LeftArrow:
                    newDirection = SnakeDirection.Left;
                    break;
            }

            if (newDirection != Direction)
                Direction = newDirection;
        }

        private static void SnakeTest()
        {
            Console.Title = "Snake";
            Console.CursorVisible = false;

            const char snakeFragmentChar = '=';
            const char snakeHeadFragmentChar = '*';
            const char fruitFragmetChar = 'A';
            const char snakeTextGroundChar = '.';
            
            decimal speedDelay = 1000;

            Numeric2D groundBounds = new Numeric2D(50, 25);
            SnakeTextGround ground = new SnakeTextGround(snakeTextGroundChar, groundBounds.X, groundBounds.Y);

            TextSnakeHead snakeHead = new TextSnakeHead
            {
                FragmentChar = snakeHeadFragmentChar,
                Position = groundBounds.RandomIn(),
                Health = 100
            };

            var fruit = new TextSnakeFragment
            {
                Position = groundBounds.RandomIn(),
                FragmentChar = fruitFragmetChar
            };

            TextSnake snake = new TextSnake(snakeHead, 2, snakeFragmentChar);

            ground.ToDraw.Add(fruit);
            ground.ToDraw.Add(snakeHead);
            ground.ToDraw.AddRange(snake.Fragments);

            Numeric2D newSnakePos = snake.Head.Position;
            
            void showInformation()
            {
                Console.WriteLine($"Length - {snake.Fragments.Count}");
                Console.WriteLine($"Health - {snake.Head.Health}");
                Console.WriteLine($"Speed - {Decimal.Round(1 / speedDelay, 3)}");
            }

            while (snake.Head.Health > 0)
            {
                HandleConsoleInputAsync();

                switch (Direction)
                {
                    case SnakeDirection.Back:
                        newSnakePos.Y--;
                        break;
                    case SnakeDirection.Forward:
                        newSnakePos.Y++;
                        break;
                    case SnakeDirection.Left:
                        newSnakePos.X--;
                        break;
                    case SnakeDirection.Right:
                        newSnakePos.X++;
                        break;
                }                

                Console.Clear();

                if (newSnakePos.Y >= ground.Heigth)
                    newSnakePos.Y = 0;
                else if (newSnakePos.X >= ground.Width)
                    newSnakePos.X = 0;
                else if (newSnakePos.Y < 0)
                    newSnakePos.Y = ground.Heigth - 1;
                else if (newSnakePos.X < 0)
                    newSnakePos.X = ground.Width - 1;

                snake.Move(newSnakePos);

                foreach (var fragment in ground.ToDraw)
                {
                    if (snake.Head.Position != fragment.Position)
                        continue;

                    if (snake.Fragments.Contains(fragment) && fragment != snake.Head)
                    {
                        snake.Head.Health = 0;

                        break;
                    }
                    else if (fragment == fruit)
                    {
                        fruit.Position = groundBounds.RandomIn();

                        var newFragmentToDraw = new TextSnakeFragment
                        {
                            Position = snake.Head.Position,
                            FragmentChar = snakeFragmentChar
                        };

                        snake.Fragments.Add(newFragmentToDraw);
                        ground.ToDraw.Add(newFragmentToDraw);

                        speedDelay *= 0.75M;

                        break;
                    }
                }

                Console.WriteLine(ground.ToString());
                showInformation();

                Thread.Sleep((int)Math.Ceiling(speedDelay));
            }
            
            Console.Clear();
            Console.WriteLine("GAME OVER!");
            showInformation();
        }     
    }
}