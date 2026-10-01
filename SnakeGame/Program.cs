using System.Diagnostics;

enum Difficulty
{
    Easy,
    Medium,
    Hard
}

enum Direction
{
    Right,
    Left,
    Up,
    Down
}

enum GameResult
{
    GameOver,
    MainMenu
}

readonly record struct Position(int X, int Y);

record GameSettings(
    Difficulty Difficulty,
    int Speed,
    bool HasBomb
);

class Program
{
    static readonly Random Random = new();

    static readonly List<Position> Snake = [];

    static readonly Queue<Direction> _directionQueue = new();

    static Direction _direction = Direction.Right;

    static Position _food;

    static Position? _bomb;

    static int _score;

    static DateTime _startTime;

    static GameSettings _settings =
        new(Difficulty.Easy, 150, false);

    static int BoardRight =>
        Math.Min(Console.WindowWidth - 2, 80);

    static int BoardBottom =>
        Math.Min(Console.WindowHeight - 5, 25);


    static void Main()
    {
        Console.CursorVisible = false;

        try
        {
            while (true)
            {
                if (!ShowMainMenu())
                    break;

                var difficulty = ShowDifficultyMenu();

                _settings = CreateSettings(difficulty);

                var result = RunGame();

                if (result == GameResult.GameOver)
                    ShowGameOver();
            }
        }
        finally
        {
            Console.ResetColor();
            Console.CursorVisible = true;
            Console.Clear();
        }
    }


    static bool ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();

            DrawTitle("SNAKE GAME");

            Console.WriteLine();
            Console.WriteLine("1. Start Game");
            Console.WriteLine("2. Exit");
            Console.WriteLine();

            Console.Write("Select: ");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return true;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                case ConsoleKey.Escape:
                    return false;
            }
        }
    }


    static Difficulty ShowDifficultyMenu()
    {
        while (true)
        {
            Console.Clear();

            DrawTitle("SELECT DIFFICULTY");

            Console.WriteLine();
            Console.WriteLine("1. Easy");
            Console.WriteLine("   Normal speed + Food");

            Console.WriteLine();
            Console.WriteLine("2. Medium");
            Console.WriteLine("   Faster speed + Food");

            Console.WriteLine();
            Console.WriteLine("3. Hard");
            Console.WriteLine("   Fast speed + Food + Bomb");

            Console.WriteLine();
            Console.WriteLine("Esc. Back");
            Console.WriteLine();

            Console.Write("Select: ");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return Difficulty.Easy;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    return Difficulty.Medium;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    return Difficulty.Hard;

                case ConsoleKey.Escape:
                    return Difficulty.Easy;
            }
        }
    }


    static GameSettings CreateSettings(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy =>
                new GameSettings(
                    Difficulty.Easy,
                    150,
                    false
                ),

            Difficulty.Medium =>
                new GameSettings(
                    Difficulty.Medium,
                    100,
                    false
                ),

            Difficulty.Hard =>
                new GameSettings(
                    Difficulty.Hard,
                    60,
                    true
                ),

            _ =>
                new GameSettings(
                    Difficulty.Easy,
                    150,
                    false
                )
        };
    }


    static GameResult RunGame()
    {
        InitializeGame();

        Console.Clear();

        DrawFrame();
        DrawGame();

        var stopwatch = Stopwatch.StartNew();

        while (true)
        {
            if (HandleInput())
                return GameResult.MainMenu;

            if (stopwatch.ElapsedMilliseconds < _settings.Speed)
            {
                Thread.Sleep(1);
                continue;
            }

            stopwatch.Restart();

            ClearObjects();

            MoveSnake();

            if (IsGameOver())
                return GameResult.GameOver;

            if (Snake[^1] == _food)
                EatFood();

            DrawGame();
        }
    }


    static void InitializeGame()
    {
        Snake.Clear();

        _directionQueue.Clear();

        _direction = Direction.Right;

        _score = 0;

        _startTime = DateTime.Now;

        for (var i = 0; i < 10; i++)
        {
            Snake.Add(
                new Position(i + 5, 5)
            );
        }

        _bomb = null;

        _food = GeneratePosition();

        if (_settings.HasBomb)
            _bomb = GeneratePosition();
    }


    static bool HandleInput()
    {
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Escape:
                    return true;

                case ConsoleKey.RightArrow:
                    AddDirection(Direction.Right);
                    break;

                case ConsoleKey.LeftArrow:
                    AddDirection(Direction.Left);
                    break;

                case ConsoleKey.UpArrow:
                    AddDirection(Direction.Up);
                    break;

                case ConsoleKey.DownArrow:
                    AddDirection(Direction.Down);
                    break;
            }
        }

        return false;
    }

    static void AddDirection(Direction direction)
    {
        var lastDirection =
            _directionQueue.Count > 0
                ? _directionQueue.Last()
                : _direction;

        if (IsOpposite(lastDirection, direction))
            return;

        if (lastDirection == direction)
            return;

        _directionQueue.Enqueue(direction);
    }

    static bool IsOpposite(
    Direction first,
    Direction second)
    {
        return
            (first == Direction.Right &&
             second == Direction.Left)

            ||

            (first == Direction.Left &&
             second == Direction.Right)

            ||

            (first == Direction.Up &&
             second == Direction.Down)

            ||

            (first == Direction.Down &&
             second == Direction.Up);
    }

    static void MoveSnake()
    {
        if (_directionQueue.Count > 0)
        {
            _direction = _directionQueue.Dequeue();
        }

        var head = Snake[^1];

        var newHead = _direction switch
        {
            Direction.Right =>
                new Position(head.X + 1, head.Y),

            Direction.Left =>
                new Position(head.X - 1, head.Y),

            Direction.Up =>
                new Position(head.X, head.Y - 1),

            Direction.Down =>
                new Position(head.X, head.Y + 1),

            _ => head
        };

        Snake.Add(newHead);

        Snake.RemoveAt(0);
    }


    static bool IsGameOver()
    {
        var head = Snake[^1];

        if (head.X <= 0 ||
            head.X >= BoardRight ||
            head.Y <= 0 ||
            head.Y >= BoardBottom)
        {
            return true;
        }

        for (var i = 0; i < Snake.Count - 1; i++)
        {
            if (Snake[i] == head)
                return true;
        }


        return _bomb.HasValue &&
               head == _bomb.Value;
    }


    static void EatFood()
    {
        _score++;


        Snake.Insert(0, Snake[0]);

        _food = GeneratePosition();

        if (_settings.HasBomb)
            _bomb = GeneratePosition();
    }


    static Position GeneratePosition()
    {
        while (true)
        {
            var position = new Position(
                Random.Next(1, BoardRight),
                Random.Next(1, BoardBottom)
            );

            if (Snake.Contains(position))
                continue;

            if (_bomb.HasValue &&
                position == _bomb.Value)
                continue;

            if (position == _food)
                continue;

            return position;
        }
    }


    static void DrawGame()
    {
        DrawSnake();

        DrawObject(
            _food,
            '@',
            ConsoleColor.Green
        );

        if (_bomb.HasValue)
        {
            DrawObject(
                _bomb.Value,
                '#',
                ConsoleColor.Red
            );
        }

        DrawInfo();
    }


    static void DrawSnake()
    {
        for (var i = 0; i < Snake.Count; i++)
        {
            var position = Snake[i];

            if (!IsInsideConsole(position))
                continue;

            Console.SetCursorPosition(
                position.X,
                position.Y
            );

            Console.ForegroundColor =
                i == Snake.Count - 1
                    ? ConsoleColor.Yellow
                    : i % 2 == 0
                        ? ConsoleColor.Red
                        : ConsoleColor.Blue;

            Console.Write('█');
        }

        Console.ResetColor();
    }


    static void DrawObject(
        Position position,
        char symbol,
        ConsoleColor color)
    {
        if (!IsInsideConsole(position))
            return;

        Console.SetCursorPosition(
            position.X,
            position.Y
        );

        Console.ForegroundColor = color;

        Console.Write(symbol);

        Console.ResetColor();
    }



    static void ClearObjects()
    {
        foreach (var position in Snake)
            ClearPosition(position);

        ClearPosition(_food);

        if (_bomb.HasValue)
            ClearPosition(_bomb.Value);
    }


    static void ClearPosition(Position position)
    {
        if (!IsInsideConsole(position))
            return;

        Console.SetCursorPosition(
            position.X,
            position.Y
        );

        Console.Write(' ');
    }


    static void DrawFrame()
    {
        Console.BackgroundColor = ConsoleColor.Yellow;

        for (var x = 0; x <= BoardRight; x++)
        {
            DrawCell(x, 0);
            DrawCell(x, BoardBottom);
        }

        for (var y = 0; y <= BoardBottom; y++)
        {
            DrawCell(0, y);
            DrawCell(BoardRight, y);
        }

        Console.BackgroundColor = ConsoleColor.Blue;

        for (var x = 1; x < BoardRight; x++)
        {
            for (
                var y = BoardBottom + 1;
                y < Console.WindowHeight;
                y++)
            {
                DrawCell(x, y);
            }
        }

        Console.ResetColor();
    }


    static void DrawCell(int x, int y)
    {
        if (!IsInsideConsole(new Position(x, y)))
            return;

        Console.SetCursorPosition(x, y);

        Console.Write(' ');
    }


    static void DrawInfo()
    {
        var y = BoardBottom + 2;

        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.SetCursorPosition(3, y);
        Console.Write($"Score: {_score}");

        Console.SetCursorPosition(20, y);
        Console.Write($"Difficulty: {_settings.Difficulty}");

        Console.SetCursorPosition(40, y);
        Console.Write($"Speed: {_settings.Speed}ms");

        Console.SetCursorPosition(3, y + 1);
        Console.Write(
            $"Time: {DateTime.Now - _startTime:hh\\:mm\\:ss}"
        );

        Console.ResetColor();
    }


    static void ShowGameOver()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("              GAME OVER");

        Console.ResetColor();

        Console.WriteLine();
        Console.WriteLine($"              Score: {_score}");
        Console.WriteLine($"              Difficulty: {_settings.Difficulty}");
        Console.WriteLine(
            $"              Time: {DateTime.Now - _startTime:hh\\:mm\\:ss}"
        );

        Console.WriteLine();
        Console.WriteLine(
            "              Press any key to return..."
        );

        Console.ReadKey(true);
    }


    static void DrawTitle(string title)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine();
        Console.WriteLine(
            "========================================"
        );

        Console.WriteLine(
            $"              {title}"
        );

        Console.WriteLine(
            "========================================"
        );

        Console.ResetColor();
    }


    static bool IsInsideConsole(Position position)
    {
        return position.X >= 0 &&
               position.X < Console.WindowWidth &&
               position.Y >= 0 &&
               position.Y < Console.WindowHeight;
    }
}