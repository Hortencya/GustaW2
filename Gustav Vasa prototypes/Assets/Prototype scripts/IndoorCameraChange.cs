using UnityEngine;
using System.Collections;

public class IndoorCameraChange :MonoBehaviour
{
    [SerializeField]
    private float maxZoomDistance;// the maximum distance the camera can go/ the camera cant go any further away than this
    [SerializeField]
    private float minZoomDistane; // the minimum distance the camera can go/ the camera can't go any close than this
    [SerializeField]
    private float zoomspeed;   
    private Vector3 ChangedCameraPostion;
    private bool zoom; // true if we are zoomed in with the camera
    CameraFollow camscript;//reference to the camera 

    private void Start()
    {
        zoom = false;// zoom starts as false
        zoomspeed = 20f;// initally set to 20 float this may be changed later
        minZoomDistane = 4f;// initially set to 4 float but may be altered later on
        camscript = GameManager.managerWasa.mainCamera.GetComponent<CameraFollow>();
        maxZoomDistance = GameManager.managerWasa.mainCamera.fieldOfView;// set the maxzoom distance to the already defined field of view
    }
    // function for zooming the camera in at the minzoomdistance
    private void ZoomInOnPlayer()
    {
        GameManager.managerWasa.mainCamera.fieldOfView -= zoomspeed / 8;// assigns the camera to zoom after the set zoomspeed divided by eight
        if (GameManager.managerWasa.mainCamera.fieldOfView < minZoomDistane)// checks if the field of view on the camera is more than the minimum distance of the camera
        {
            camscript.CameraLocked = false;// camera unlocked
            GameManager.managerWasa.mainCamera.fieldOfView = minZoomDistane;// if so change the field of view to the minimum zoomdistance
        }

    }
    // function for zooming the camera out to the original position, using maxdistance
    private void ZoomOutToNormal()
    {
        GameManager.managerWasa.mainCamera.fieldOfView += zoomspeed / 8;// assigns the camera to zoom at the specified zoomspeed divided with eight. 
        if (GameManager.managerWasa.mainCamera.fieldOfView > maxZoomDistance)// checks if the field of view on the main camera is less than the maxdistance
        {
            GameManager.managerWasa.mainCamera.fieldOfView = maxZoomDistance;// if so change the field of view to the maximum distance.
            camscript.CameraLocked = true;// set cameralocked bool to true again
        }
    }
   // trigger function that fires the camera zooming event
   private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trigger")
        {
            if (!zoom)
            {              
                ZoomInOnPlayer();
            }
            else if (zoom)
            {               
                ZoomOutToNormal();
            }
        }
    }
}
