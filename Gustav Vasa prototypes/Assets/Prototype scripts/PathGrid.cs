using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid : MonoBehaviour {
    public Transform testObj;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    public Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    

    void Start()
    {
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
                grid[x, y] = new Node(walkale, worldpoint,x,y);
            }
        }


        
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
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y)  );

        if (grid!= null)
        {
            Node testNode = GetNodeFromWorldPos(testObj.position);
            foreach (Node n in grid)
            {
                Gizmos.color = (n.wakable) ? Color.white : Color.red;

                if(path!= null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }


                if (testNode == n) Gizmos.color = Color.cyan;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - nodeDiameter/20 ));
            }
        }
    }
}

