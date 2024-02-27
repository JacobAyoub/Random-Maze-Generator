using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDrawer : MonoBehaviour
{
    public int width = 10;

    public int height = 10;

    public float size = 1f;

    public float cellSize = 1f;

    public bool randomExits;

    public Transform walls;

    public Transform floor;

    public Transform[] cells;

    void Start()
    {
        var maze = MazeGenerator.Generate(width, height, randomExits);
        Create(maze);
    }

    private void Create(CellState[,] maze)
    {

        //Instantiates a plane that fits exactly to the mazes dimensions
        var plane = Instantiate(floor, transform);
        plane.localScale = new Vector3(width / 10, 1, height / 10);
        plane.position = transform.position + new Vector3(-.5f, 0, -.5f);

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = transform.position + new Vector3(-width / 2 + i, 0, -height / 2 + j);

                //Checks each cell and whether they have walls on each part. Instantiates the walls depending on which of the booleans in cell[i, j] are set to true
                if (cell.hasUpper)
                {
                    var upperWall = Instantiate(walls, transform) as Transform;
                    upperWall.position = position + new Vector3(0, 0, size / 2);
                    upperWall.localScale = new Vector3(size, upperWall.localScale.y, upperWall.localScale.z);
                }

                if (cell.hasLeft)
                {
                    var leftWall = Instantiate(walls, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if (cell.hasRight)
                {
                    var rightWall = Instantiate(walls, transform) as Transform;
                    rightWall.position = position + new Vector3(+size / 2, 0, 0);
                    rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                    rightWall.eulerAngles = new Vector3(0, 90, 0);
                }
                

                
                if (cell.hasLower)
                {
                    var lowerWall = Instantiate(walls, transform) as Transform;
                    lowerWall.position = position + new Vector3(0, 0, -size / 2);
                    lowerWall.localScale = new Vector3(size, lowerWall.localScale.y, lowerWall.localScale.z);
                }
                

                //Grabs a random object in cells[] to put inside each of the cells. This could add variety if we wanted to put something in our maze
                var access = Instantiate(cells[UnityEngine.Random.Range(0, cells.Length)], transform) as Transform;
                access.position = position + new Vector3(-cellSize / 10 + .1f, 0, -cellSize / 10 + .1f);
                access.localScale = new Vector3(cellSize / 10, cellSize / 10, cellSize / 10);

            }

        }

    }
}
