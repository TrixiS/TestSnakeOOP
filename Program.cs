﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestSnake
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            await SnakeTestAsync();
        }

        private static void Numeric2DTest()
        {
            Numeric2D numeric = new Numeric2D(5, 5);
            Numeric2D numericRandomIn = numeric.RandomIn();

            TextGround ground = new TextGround('.', numeric.X, numeric.Y);
            TextGround randomInGround = new TextGround('.', numericRandomIn.X, numericRandomIn.Y);

            Console.WriteLine(numeric.ToString());
            Console.WriteLine(numericRandomIn.ToString());

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

        private static SnakeDirection direction = SnakeDirection.Forward;

        private static async void HandleConsoleInputAsync()
        {
            await Task.Yield();

            while (true)
            {
                ConsoleKey handledKey = Console.ReadKey().Key;
                SnakeDirection newDirection = direction;

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

                if (newDirection != direction)
                    direction = newDirection;
            }
        }

        private static async Task SnakeTestAsync()
        {
            Console.Title = "Snake";
            Console.CursorVisible = false;

            const char snakeFragmentChar = '=';
            const char snakeHeadFragmentChar = '*';
            const char fruitFragmentChar = 'A';
            const char snakeTextGroundChar = '.';

            const decimal speedMultiplier = 0.75M;
            const int groundBoundX = 50;
            const int groundBoundY = 25;
            
            decimal speedDelay = 1000;

            Numeric2D groundBounds = new Numeric2D(groundBoundX, groundBoundY);
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
                FragmentChar = fruitFragmentChar
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

            HandleConsoleInputAsync();

            while (snake.Head.Health > 0)
            {
                switch (direction)
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

                        speedDelay *= speedMultiplier;

                        break;
                    }
                }

                Console.WriteLine(ground.ToString());
                showInformation();

                await Task.Delay((int)Math.Ceiling(speedDelay));
            }
            
            Console.Clear();
            Console.WriteLine("GAME OVER!");
            showInformation();
        }     
    }
}