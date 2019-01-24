using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid : MonoBehaviour {
    public Transform testObj;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    public Node[,] grid;
    public List<Journey> journeys;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    public float slopPenelty = 0;
    

    void Start()
    {
        journeys = new List<Journey>();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt( gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        createGrid();
    }

    void createGrid() {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 gridBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldpoint = gridBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                worldpoint = GridDrop(worldpoint);
                bool walkale = !(Physics.CheckSphere(worldpoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkale, worldpoint, x, y);


                if (y > 0)
                {
                    float dic = Vector3.Distance(grid[x, y].worldPos, grid[x, y - 1].worldPos);
                    grid[x, y].dicNeighbour[1, 0] = dic* SlopeModefiedDistanceToNeigbour(grid[x, y], grid[x, y - 1]);
                    grid[x, y - 1].dicNeighbour[1, 2] = dic * SlopeModefiedDistanceToNeigbour(grid[x, y-1], grid[x, y]);
                }

                if (x > 0)
                {
                    float dic = Vector3.Distance(grid[x, y].worldPos, grid[x - 1, y].worldPos);
                    grid[x, y].dicNeighbour[0, 1] = dic * SlopeModefiedDistanceToNeigbour(grid[x, y], grid[x-1, y ]); ;
                    grid[x - 1, y].dicNeighbour[2, 1] = dic * SlopeModefiedDistanceToNeigbour(grid[x-1, y], grid[x, y ]); ;


                    if (y < gridSizeY - 1)
                    {
                        dic = Vector3.Distance(grid[x, y].worldPos, grid[x - 1, y + 1].worldPos);
                        grid[x, y].dicNeighbour[0, 2] = dic * SlopeModefiedDistanceToNeigbour(grid[x, y], grid[x-1, y + 1]); ;
                        grid[x - 1, y + 1].dicNeighbour[2, 0] = dic * SlopeModefiedDistanceToNeigbour(grid[x-1, y+1], grid[x, y ]); ;
                    }


                    if (y > 0)
                    {
                        dic = Vector3.Distance(grid[x, y].worldPos, grid[x - 1, y - 1].worldPos);
                        grid[x, y].dicNeighbour[0, 0] = dic * SlopeModefiedDistanceToNeigbour(grid[x, y], grid[x-1, y - 1]); ;
                        grid[x - 1, y - 1].dicNeighbour[2, 2] = dic * SlopeModefiedDistanceToNeigbour(grid[x-1, y-1], grid[x, y ]); ;
                    }

                }



            }

        }
        
    }
    public float SlopeModefiedDistanceToNeigbour(Node a, Node b)
    {
        float ret = 1;
        if (a.worldPos.y < b.worldPos.y)
        {
            ret = 1 * ((b.worldPos.y - a.worldPos.y )+ 1);

        }
        else if (a.worldPos.y > b.worldPos.y)
        {
            ret = 1 / ((a.worldPos.y - b.worldPos.y) + 1);

        }
        return ret *slopPenelty;

    }
    Vector3 GridDrop( Vector3 worldPos  )
    {
        RaycastHit hit;
        
            if (Physics.Raycast(worldPos, Vector3.down, out hit))
            {
                return  hit.point;
            }
        return worldPos;
    }
    public List<Node> getNeighbours (Node node ){
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <=1; y++)
            {
                if (x == 0 && y == 0) continue;
                

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    if(checkX>=0 && checkX < gridSizeX && checkY >= 0 && checkY< gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }

                
            }
        }
        return neighbours;
     }

    public Node GetNodeFromWorldPos(Vector3 worldPos )
    {
        float percentX = (worldPos.x -transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.z - transform.position.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x,y];

    }


    public List<Node> path;
    public bool drawGrid;
    void OnDrawGizmos()
    {
        if (drawGrid)
        {
            path = new List<Node>();
            if (journeys != null)// ?skum sack händer om den inte finns
                foreach (var journey in journeys)
                {
                    path.AddRange(journey.path);
                }
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null)
            {
                Node testNode = GetNodeFromWorldPos(testObj.position);
                foreach (Node n in grid)
                {

                    Gizmos.color = (n.wakable) ? Color.white : Color.red;

                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = Color.black;
                        }
                    }


                    if (testNode == n) Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - nodeDiameter / 20));
                }
            }
        }
    }
}

