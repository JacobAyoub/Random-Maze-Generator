using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct CellState
{
    /*       
        Attributes:
        hasLeft(bool): Whether cell has a Left Wall 
        hasRight(bool): Whether cell has a Right Wall 
        hasUpper(bool): Whether cell has a Top Wall 
        hasLower(bool): Whether cell has a Bottom Wall 
        visited(bool): Checks to see if cell has been visited during Recursive Backtracker
     */
    public bool hasLeft;
    public bool hasRight;
    public bool hasUpper;
    public bool hasLower;
    public bool visited;
}

public struct NeighbourCell
{
    /*
        Attributes:
        Position(Positions): Stores position of neighbouring cell
        sharedWall(String): Stores what wall is being shared by both cells
     */
    public Positions Position;
    public string sharedWall;
}

public struct Positions
{
    public int X;
    public int Y;
}


public static class MazeGenerator
{
    private static CellState[,] RecursiveBacktracker(CellState[,] maze, int width, int height)
    {
        var seed = new System.Random(/*seed*/);
        var positionStack = new Stack<Positions>();
        var position = new Positions { X = seed.Next(0, width), Y = seed.Next(0, height) };

        //Starts at random position and starts iteration
        maze[position.X, position.Y].visited = true;
        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            //Gets front of Stack and gets the unvisited neighbours of that cell
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);
            if (neighbours.Count > 0)
            {
                //Puts current position at the end of Stack in case we need to go back to find new unvisited cells
                positionStack.Push(current);

                //Gets random position of the neighbour
                var randomIndex = seed.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randomIndex];
                var neighbourPosition = randomNeighbour.Position;

                //Sets current wall to false depending on what sharedWall equals and sets neighbours opposite wall false
                if (randomNeighbour.sharedWall.Equals("Left"))
                {
                    maze[current.X, current.Y].hasLeft = false;
                    maze[neighbourPosition.X, neighbourPosition.Y].hasRight = false;
                }
                else if (randomNeighbour.sharedWall.Equals("Upper"))
                {
                    maze[current.X, current.Y].hasUpper = false;
                    maze[neighbourPosition.X, neighbourPosition.Y].hasLower = false;
                }
                else if (randomNeighbour.sharedWall.Equals("Down"))
                {
                    maze[current.X, current.Y].hasLower = false;
                    maze[neighbourPosition.X, neighbourPosition.Y].hasUpper = false;
                }
                else if (randomNeighbour.sharedWall.Equals("Right"))
                {
                    maze[current.X, current.Y].hasRight = false;
                    maze[neighbourPosition.X, neighbourPosition.Y].hasLeft = false;
                }
                //Neighbour position has officially been visited and is moved at back of the Stack
                maze[neighbourPosition.X, neighbourPosition.Y].visited = true;
                positionStack.Push(neighbourPosition);
            }
        }
        return maze;
    }


    private static List<NeighbourCell> GetUnvisitedNeighbours(Positions p, CellState[,] maze, int width, int height)
    {
        //Creates new List of Neighbours
        var unvivistedNeighbours = new List<NeighbourCell>();

        //Checks all immediate surrounding cells to check if they have been visited
        if (p.X > 0)
        {
            if (!maze[p.X - 1, p.Y].visited)
            {
                unvivistedNeighbours.Add(new NeighbourCell
                {
                    Position = new Positions
                    {X = p.X - 1, Y = p.Y},
                    sharedWall = "Left",
                });
            }
        }

        if (p.Y > 0)
        {
            if (!maze[p.X, p.Y - 1].visited)
            {
                unvivistedNeighbours.Add(new NeighbourCell
                {
                    Position = new Positions
                    {X = p.X, Y = p.Y - 1},
                    sharedWall = "Down",
                });
            }
        }

        if (p.Y < height - 1)
        {
            if (!maze[p.X, p.Y + 1].visited)
            {
                unvivistedNeighbours.Add(new NeighbourCell
                {
                    Position = new Positions
                    {X = p.X, Y = p.Y + 1},
                    sharedWall = "Upper",
                });
            }
        }

        if (p.X < width - 1)
        {
            if (!maze[p.X + 1, p.Y].visited)
            {
                unvivistedNeighbours.Add(new NeighbourCell
                {
                    Position = new Positions
                    {X = p.X + 1, Y = p.Y},
                    sharedWall = "Right",
                });
            }
        }

        return unvivistedNeighbours;
    }

    private static CellState[,] CreateExits(CellState[,] maze, int width, int height, bool randomExits)
    {
        if (randomExits == true)
        {
            int upLeft = UnityEngine.Random.Range(0, 2);
            if (upLeft == 0) //Create Entrance at top of maze, Exit at bottom of maze
            {
                maze[0, UnityEngine.Random.Range(0, width)].hasLeft = false;
                maze[height - 1, UnityEngine.Random.Range(0, width)].hasRight = false;
            }
            else if (upLeft == 1) // Create Entrance at left of maze, Exit at right of maze
            {
                maze[UnityEngine.Random.Range(0, height), 0].hasLower = false;
                maze[UnityEngine.Random.Range(0, height), width - 1].hasUpper = false;
            }
        }
        else
        {

            //Create Our Entrance at top Left and our Exit at bottom Right
            maze[0, 0].hasLeft = false;
            maze[height - 1, width - 1].hasRight = false;
        }
        return maze;
    }

    public static CellState[,] Generate(int width, int height, bool randomExits)
    {
        CellState[,] maze = new CellState[width, height];
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                //Sets all walls to true and visited to truth (Creates a empty grid)
                maze[i, j].hasLeft = true;
                maze[i, j].hasRight = true;
                maze[i, j].hasUpper = true;
                maze[i, j].hasLower = true;
                maze[i, j].visited = false;
            }
        }

        //Randomly generates the maze and exits
        maze = CreateExits(maze, width, height, randomExits);
        return RecursiveBacktracker(maze, width, height);
    }
}
