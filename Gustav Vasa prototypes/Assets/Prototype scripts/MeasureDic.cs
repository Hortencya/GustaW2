using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureDic : MonoBehaviour {
    public GameObject pathFinding;
    PathGrid grid;
    public Vector3 X_Y = new Vector3(0, 1, 2);
    public Vector3 _0;
    public Vector3 _1;
    public Vector3 _2;
    Node node;
    // Use this for initialization
    void Start () {
        grid = pathFinding.GetComponent<PathGrid>();
    }
	
	// Update is called once per frame
	void Update () {
        node = grid.GetNodeFromWorldPos(transform.position);
        _0.x = node.dicNeighbour[0, 0];
        _0.y = node.dicNeighbour[1, 0];
        _0.z = node.dicNeighbour[2, 0];

        _1.x = node.dicNeighbour[0, 1];
        _1.y = node.dicNeighbour[1, 1];
        _1.z = node.dicNeighbour[2, 1];

        _2.x = node.dicNeighbour[0, 2];
        _2.y = node.dicNeighbour[1, 2];
        _2.z = node.dicNeighbour[2, 2];
        
    }
}
