class Program

{



    static void Main(string[] args)

    {

        // 게임 속도를 조정하기 위한 변수입니다. 숫자가 클수록 게임이 느려집니다.

        int gameSpeed = 100; int foodCount = 0; // 먹은 음식 수 // 게임을 시작할 때 벽을 그립니다.

        DrawWalls();

        Point p = new Point(4, 5, '*');

        Snake snake = new Snake(p, 4, Direction.RIGHT);

        snake.Draw();

        FoodCreator foodCreator = new FoodCreator(80, 20, '$');

        Point food = foodCreator.CreateFood(); food.Draw(); // 게임 루프: 이 루프는 게임이 끝날 때까지 계속 실행됩니다.              while (true)

        {

            if (Console.KeyAvailable)

            {

                var key = Console.ReadKey(true).Key;

                switch (key)

                {

                    case ConsoleKey.UpArrow:

                        snake.direction = Direction.UP;

                        break;

                    case ConsoleKey.DownArrow:

                        snake.direction = Direction.DOWN;

                        break;

                    case ConsoleKey.LeftArrow:

                        snake.direction = Direction.LEFT;

                        break;

                    case ConsoleKey.RightArrow:

                        snake.direction = Direction.RIGHT;

                        break;

                }

            }

            if (snake.Eat(food))

            {
                foodCount++;

                food.Draw();



                food = foodCreator.CreateFood();

                food.Draw();

                if (gameSpeed > 10)

                {

                    gameSpeed -= 10;

                }

            }

            else

            {

                snake.Move();

            }

            Thread.Sleep(gameSpeed);
            //벽이나 자신의 몸에 부딫히면 게임을 종료합니다.
            // 벽이나 자신의 몸에 부딪히면 게임을 끝냅니다.
            if (snake.IsHitTail() || snake.IsHitWall())
            {
                break;
            }

            Console.SetCursorPosition(0, 21); // 커서 위치 설정
            Console.WriteLine($"먹은 음식 수: {foodCount}"); // 먹은 음식 수 출력
        }

        WriteGameOver();

        Console.ReadLine();

    }

    static void WriteGameOver()

    {

        int xOffset = 25; int yOffset = 22;

        Console.SetCursorPosition(xOffset, yOffset++);

        WriteText("============================", xOffset, yOffset++);

        WriteText(" GAME OVER", xOffset, yOffset++);

        WriteText("============================", xOffset, yOffset++);

    }

    static void WriteText(string text, int xOffset, int yOffset)

    {

        Console.SetCursorPosition(xOffset, yOffset);

        Console.WriteLine(text);

    }

    static void DrawWalls()

    {

        for (int i = 0; i < 80; i++)

        {

            Console.SetCursorPosition(i, 0);

            Console.Write("#");

            Console.SetCursorPosition(i, 20);

            Console.Write("#");

        }

        for (int i = 0; i < 20; i++)

        {

            Console.SetCursorPosition(0, i);

            Console.Write("#");

            Console.SetCursorPosition(80, i);

            Console.Write("#");

        }

    }

}

public class Point

{

    public int x { get; set; }

    public int y { get; set; }

    public char sym { get; set; }

    public Point(int _x, int _y, char _sym)

    {

        x = _x; y = _y; sym = _sym;

    }

    public void Draw()

    {

        Console.SetCursorPosition(x, y); Console.Write(sym);

    }

    public void Clear()

    {

        sym = ' '; Draw();

    }

    public bool IsHit(Point p)

    {

        return p.x == x && p.y == y;

    }

}

public enum Direction { LEFT, RIGHT, UP, DOWN }

public class Snake
{
    public List<Point> body;

    public Direction direction;

    public Snake(Point tail, int length, Direction _direction)

    {

        direction = _direction; body = new List<Point>();

        for (int i = 0; i < length; i++)

        {

            Point p = new Point(tail.x, tail.y, '*');

            body.Add(p); tail.x += 1;

        }

    }

    public void Draw()

    {

        foreach (Point p in body)
        {
            p.Draw();

        }

    }

    public bool Eat(Point food)

    {

        Point head = GetNextPoint();

        if (head.IsHit(food))

        {

            food.sym = head.sym;

            body.Add(food);

            return true;

        }

        else

        {

            return false;

        }

    }

    public void Move()

    {

        Point tail = body.First();

        body.Remove(tail);

        Point head = GetNextPoint();

        body.Add(head);

        tail.Clear();

        head.Draw();

    }

    public Point GetNextPoint()

    {

        Point head = body.Last();

        Point nextPoint = new Point(head.x, head.y, head.sym);

        switch (direction)

        {

            case Direction.LEFT:
                nextPoint.x -= 2;

                break;

            case Direction.RIGHT:
                nextPoint.x += 2;

                break;

            case Direction.UP:
                nextPoint.y -= 1;

                break;

            case Direction.DOWN:
                nextPoint.y += 1;

                break;

        }

        return nextPoint;

    }

    public bool IsHitTail()

    {

        var head = body.Last();

        for (int i = 0; i < body.Count - 2; i++)

        {

            if (head.IsHit(body[i])) return true;

        }

        return false;

    }

    public bool IsHitWall()

    {

        var head = body.Last();

        if (head.x <= 0 || head.x >= 80 || head.y <= 0 || head.y >= 20) return true;

        return false;

    }

}

public class FoodCreator

{

    int mapWidth;

    int mapHeight;

    char sym;

    Random random = new Random();

    public FoodCreator(int mapWidth, int mapHeight, char sym)

    {

        this.mapWidth = mapWidth;

        this.mapHeight = mapHeight;

        this.sym = sym;

    }

    public Point CreateFood()

    {

        int x = random.Next(2, mapWidth - 2);

        x = x % 2 == 1 ? x : x + 1;

        int y = random.Next(2, mapHeight - 2);

        return new Point(x, y, sym);

    }

}