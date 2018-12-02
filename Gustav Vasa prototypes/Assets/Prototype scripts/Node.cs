using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node  {

    public bool wakable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;


    public int disFromStart;
    public int disFromEnd;

    public Node parent;

    public Node(bool _walkabe, Vector3 _worldpos, int _gridX, int _gridY )
{
        wakable = _walkabe;
        worldPos = _worldpos;
        gridX = _gridX;
        gridY = _gridY;

}
    public int cost
    {
        get { return disFromStart + disFromEnd; }
    }
}
