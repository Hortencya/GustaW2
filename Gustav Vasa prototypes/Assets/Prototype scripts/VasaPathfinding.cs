using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VasaPathfinding : MonoBehaviour {

    // Use this for initialization
    public Transform seeker, target;
    PathGrid grid;
	void Awake()
    {
        grid = GetComponent<PathGrid>();
    }
    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 starPos, Vector3 targetPos)
    {
        Node startNode = grid.GetNodeFromWorldPos(starPos);
        Node targetNode = grid.GetNodeFromWorldPos(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].cost< currentNode.cost || openSet[i].cost == currentNode.cost && openSet[i].disFromEnd < currentNode.disFromEnd) {
                    currentNode = openSet[i];
                }
                
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
            foreach (Node neighbour in grid.getNeighbours(currentNode))
            {
                if(!neighbour.wakable || closedSet.Contains(neighbour)) {
                    continue;
                }
                int newCostToNeighbour = currentNode.disFromStart + GetDistance(currentNode, neighbour);
                if(newCostToNeighbour < neighbour.disFromStart || !openSet.Contains(neighbour))
                {
                    neighbour.disFromStart = newCostToNeighbour;
                    neighbour.disFromEnd = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }
    void RetracePath( Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNude = endNode;

        while (currentNude != startNode)
        {
            path.Add(currentNude);
            currentNude = currentNude.parent;
        }
        path.Reverse();

        grid.path = path;
    }


    int GetDistance(Node nodeA, Node nodeb)
    {
        return (int)Vector3.Distance(nodeA.worldPos, nodeb.worldPos);
    }
        int GetDistanceOld(Node nodeA, Node nodeb)
    {
       int disX = Mathf.Abs(nodeA.gridX - nodeb.gridX);
        int disY = Mathf.Abs(nodeA.gridY - nodeb.gridY);

        if (disX > disY) {
            // modefiering Här
            
            return 14 * disY + 10 * (disX - disY); }
       
        return 14 * disX + 10 * (disY - disX);
    }

    
}
