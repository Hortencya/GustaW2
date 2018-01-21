using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    
    public Transform player;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Space offsetPostionSpace = Space.Self;
    [SerializeField]
    private bool cameraIsLocked = true;
    private IndoorCameraChange cameraChange;// this are set to the specific camera change-item the player collides with

    private void Update()
    {
        Refresh();
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
        if (cameraIsLocked)
        {
            transform.LookAt(player);
        }
        else
        {
            transform.rotation = player.rotation;
        }
	}
	
	
}
