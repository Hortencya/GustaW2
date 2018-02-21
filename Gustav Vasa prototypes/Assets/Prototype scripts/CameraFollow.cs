using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    
    private  Transform player;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Space offsetPostionSpace = Space.Self;
    [SerializeField]
    private bool cameraIsLocked = true;// bool that tells us the camera is locked ie outside of player control
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Cameratransform").transform;// find the player from within the scene
    }
   
    public bool CameraLocked
    {
        get { return cameraIsLocked; }
        set { cameraIsLocked = value; }// can be either true or false
    }

    private void Update()
    {
        Refresh();// updates the cameras position runtime
    }
   
    /// <summary>
    /// set up default camera behaviour
    /// </summary>
    public void Refresh ()
    {
        if (player == null)
        {
            Debug.LogWarning("Missing player ref !", this);
            return;
        }
        if (offsetPostionSpace == Space.Self)
        {
            transform.position = player.TransformPoint(offset);
        }
        else
        {
            transform.position = player.transform.position + offset;
        }
        if (CameraLocked)// changed to use property
        {
            transform.LookAt(player);
        }        
	}
	
	
}
