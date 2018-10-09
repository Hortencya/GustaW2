using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// code to move the camera with the character
/// </summary>
public class MoveCamera2D : MonoBehaviour {
    //Instance variable
    private Camera Maincam;// reference to maincamera
    private GameObject player;// reference to the player
    private float speed;
	// properties
    /// <summary>
    /// bool that looks for if the camera shall move with Gustav Vasa or not
    /// </summary>
    public bool MoveCamera
    {
        get { return MoveCamera; }
        set { MoveCamera = value; }
    }   
    private void Awake()
    {
        GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        speed = 0.5f;
    }
    /// <summary>
    /// Function that moves the camera with the player
    /// </summary>
    private void  MoveMainCamera()
    {
        if (MoveCamera)
        {
            transform.position = new Vector3(player.transform.position.x + 6, 60.28f, -9.54f);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        MoveMainCamera();
    }
}
