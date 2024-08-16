using System;
using System.Collections.Generic;
using System.Threading; // Necessary for adding delays

public class Program
{
    public static void Main()
    {
        // Define the maze layout where 1 is a wall, 0 is a space, -1 is the start, and 9 is the target.
        int[,] maze = {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 9, 1, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1},
            {1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1},
            {1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1},
            {1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };

        // Start and target positions are specified based on the -1 and 9 values in the maze array.
        var start = new Point(2, 2);  // Initialize start position where robot starts
        var target = new Point(4, 20); // Initialize target position to reach

        // Perform BFS to find a path from start to target
        BFS_Maze_Solver(maze, start, target);
    }

    // Breadth-First Search (BFS) to navigate the maze
    public static void BFS_Maze_Solver(int[,] maze, Point start, Point target)
    {
        int rows = maze.GetLength(0);
        int cols = maze.GetLength(1);
        var queue = new Queue<Point>();
        var visited = new HashSet<Point>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            // Check if current position is the target
            if (current.Equals(target))
            {
                Console.WriteLine("Target reached!");
                return;
            }

            // Explore adjacent cells: right, down, left, up
            var directions = new List<Point>
            {
                new Point(0, 1), // Right
                new Point(1, 0), // Down
                new Point(0, -1), // Left
                new Point(-1, 0) // Up
            };

            foreach (var direction in directions)
            {
                Point next = new Point(current.X + direction.X, current.Y + direction.Y);

                // Validate the next position is within bounds, not visited, and not a wall
                if (next.X >= 0 && next.X < rows && next.Y >= 0 && next.Y < cols &&
                    !visited.Contains(next) && maze[next.X, next.Y] != 1)
                {
                    queue.Enqueue(next);
                    visited.Add(next);

                    // Update the maze display to show path
                    if (maze[next.X, next.Y] != 9) // Don't overwrite the target
                        maze[next.X, next.Y] = 2;

                    PrintMaze(maze);  // Print updated maze state
                    Thread.Sleep(500); // Add delay to visualize robot movement step by step
                }
            }
        }

        Console.WriteLine("No path found.");
    }

    // Print the maze with visual representation
    public static void PrintMaze(int[,] maze)
    {
        Console.Clear();
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                switch (maze[i, j])
                {
                    case 1:
                        Console.Write("\u2588"); // Wall
                        break;
                    case -1:
                        Console.Write("S"); // Start
                        break;
                    case 9:
                        Console.Write("T"); // Target
                        break;
                    case 2:
                        Console.Write("."); // Path traced by the robot
                        break;
                    default:
                        Console.Write(" "); // Free space
                        break;
                }
            }
            Console.WriteLine();
        }
    }
}

// Represents a point in the maze
public struct Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        return obj is Point p && X == p.X && Y == p.Y;
    }

    public override int GetHashCode()
    {
        return X ^ Y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

