using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// code to move the camera with the character
/// </summary>
public class MoveCamera2D : MonoBehaviour {
    //Instance variable
    private GameObject player;// reference to the player
    private bool panning;
	// properties
    /// <summary>
    /// bool that looks for if the camera shall move with Gustav Vasa or not
    /// </summary>
    public bool MoveCamera
    {
        get { return panning; }
        set { panning = value; }
    }   
    private void Awake()
    {
       
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    /// <summary>
    /// Function that moves the camera with the player
    /// </summary>
    public void  MoveMainCamera()
    {
        if (panning)
        {
            transform.position = new Vector3(player.transform.position.x -1.5f, 60.28f, -9.54f);
        }
    }
    
}
