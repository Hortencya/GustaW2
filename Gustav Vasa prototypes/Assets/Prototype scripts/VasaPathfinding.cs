using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VasaPathfinding : MonoBehaviour {

    // Use this for initialization
    public Transform[] targets;
    public Transform[] seekers;
    public List<Journey> journeys;
    PathGrid grid;
	void Awake()
    {
        grid = GetComponent<PathGrid>();
    }
    void Start()
    {
        journeys = grid.journeys;
        foreach (var seeker in seekers)
        {
            journeys.Add(new Journey(seeker));
         }
        InvokeRepeating("PathUpdate", 0,0.1f);
    }
    void Update()
    {
        //foreach (var journey in journeys)
        //{
        //    if (!journey.HaveTarget()) journey.SetTarget(target);
        //    FindPath(journey);
        //}
        
    }
    void PathUpdate()
    {
        for (int i = 0; i < seekers.Length; i++)
        {
            if (!journeys[i].HaveTarget()) journeys[i].SetTarget(targets[i]);
            FindPath(journeys[i]);
        }
    }
    void FindPath(Journey journey)
    {
        Node startNode = grid.GetNodeFromWorldPos(journey.owner.position);
        Node targetNode = grid.GetNodeFromWorldPos(journey.target.position);

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
                RetracePath(startNode, targetNode, journey);
                return;
            }
            foreach (Node neighbour in grid.getNeighbours(currentNode))
            {
                if(!neighbour.wakable || closedSet.Contains(neighbour)) {
                    continue;
                }
                float newCostToNeighbour = currentNode.disFromStart + GetDistanceNeigbour(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.disFromStart || !openSet.Contains(neighbour))
                {
                    neighbour.disFromStart = newCostToNeighbour;
                    neighbour.disFromEnd = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
                //Debug.Log((neighbour.gridX - currentNode.gridX)+  "  " + (neighbour.gridY - currentNode.gridY));
                //if ( GetDistance(currentNode, neighbour)   == currentNode.GetDicNeigbour(neighbour.gridX-currentNode.gridX +1,neighbour.gridY-currentNode.gridY +1) ) Debug.Log("truo"); else Debug.Log("fals");
            }
        }
    }
    public float SlopeModefiedDistanceToNeigbour(Node a, Node b)
    {
        float ret = 1;
        if (a.worldPos.y < b.worldPos.y)
        {
            ret = 1*( (b.worldPos.y - a.worldPos.y) + 1);
            
        }
        else if (a.worldPos.y > b.worldPos.y )
        {
            ret =1/( (a.worldPos.y - b.worldPos.y)+1);
           
        } return ret;
     
    }
    void RetracePath( Node startNode, Node endNode, Journey journey)
    {
        List<Node> path = new List<Node>();
        Node currentNude = endNode;

        while (currentNude != startNode)
        {
            path.Add(currentNude);
            currentNude = currentNude.parent;
        }
        path.Reverse();
        journey.path = path;
        //grid.path = path;
    }

    float GetDistanceNeigbour(Node nodeA, Node nodeb)
    {
        return nodeA.GetDicNeigbour(nodeb.gridX - nodeA.gridX + 1, nodeb.gridY - nodeA.gridY + 1);
    }
        float GetDistance(Node nodeA, Node nodeb)
    {
        return Vector3.Distance(nodeA.worldPos, nodeb.worldPos);
    }
        int GetDistanceOldOld(Node nodeA, Node nodeb)
    {
       int disX = Mathf.Abs(nodeA.gridX - nodeb.gridX);
        int disY = Mathf.Abs(nodeA.gridY - nodeb.gridY);

        if (disX > disY) {
            // modefiering Här
            
            return 14 * disY + 10 * (disX - disY); }
       
        return 14 * disX + 10 * (disY - disX);
    }

    
}
