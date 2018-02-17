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
    [SerializeField]
    private float rotationXValue ;// the value on the xaxis the camera is rotated with (has public constructor)
    private float roationYValue;// the value on Y axis the camera is rotated with(is locked to player rotation and therefore do not have a constructor.)
    private Quaternion cameraRotation;  // this is the quarternion for the rotation of the camera 
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // public constructors for camera script 
    public float RotationX
    {
        get { return rotationXValue; }// get the value of the x rotation value used lower down in this script to rotate the camera
        set { rotationXValue = value; }// assigns new value to said variable
    }
    public Quaternion CameraRotation
    {
        get { return cameraRotation; }
        set { cameraRotation = value; }// assign new values for x,y,z&w properties of the quarternion
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
        else
        {
            roationYValue = player.transform.rotation.y;// update the cameras y rotation to go along the same rotation as the player
            //transform.rotation = player.rotation;// this is currently commented out for experimentation with different solutions for the camera rotation
            CameraRotation.Set(RotationX, roationYValue, 0, 0);// refers to property(should return wathever is assigned in the inspector)
            transform.rotation = CameraRotation;// use of property

        }
	}
	
	
}
