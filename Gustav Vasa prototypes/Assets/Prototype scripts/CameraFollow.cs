using UnityEngine;
using System.Collections;

/// <summary>
/// Needs a softer manipulation of the camera movements right now they come of as harsh, instead of locking the position of the camera to a specific point
/// We should use a rotation formula that make the camera to always move towards the cameraposition modifier  object
/// </summary>
public class CameraFollow : MonoBehaviour {
    
    private  Transform player;
    private  Transform camlookatPos;// target the camera looks after to follow
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Space offsetPostionSpace = Space.Self;
    [SerializeField]
    private bool cameraIsLocked = true;// bool that tells us the camera is locked ie outside of player control
    
    private Camera main;// ref to main camera is set to the variable set in game manager

    // testvariables for cameramanipulation
    private bool zoomOut;//bool that determines if the camera shall be zoomed out
    private Vector3 zoomPos;// vector used to change the values of the offset variable in order to move the camera
    private Vector3 origpos;// variable that saves down the original position of the camera
    float distanceToPlayer;// this variable is calculated during runtime based on current position of player compared to camera
    public GameObject playerchar;// refers to the player as defined in game manager   
    [SerializeField]// this value might be needed to be adjusted in the inspector and is thus serialized
    private float smoothSpeed;// the camera uses this to determine how fast it moves toward the target object/OBS it has to be bigger than 0 but smaller than 1

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Cameratransform").transform;// find the player from within the scene
        origpos = new Vector3(0f, 1.23f, -3.77f);
        offset = origpos; // set correct offset to origpos  
        camlookatPos = GameObject.FindGameObjectWithTag("Cameratransform").transform;// find the player from within the scene       
    }
    private void Start()
    {
        main = GameManager.managerWasa.mainCamera;// set main to refer to the game managers camera reference this way long names avoided throughout the script
        playerchar = GameManager.managerWasa.playercharacter;
    } 
    public bool CameraLocked
    {
        get { return cameraIsLocked; }
        set { cameraIsLocked = value; }// can be either true or false
    }

    private void FixedUpdate()
    {       
        Refresh();// updates the cameras position runtime        
    }
    private void Update()
    {
        Refresh();// updates the cameras position runtime
        DefineRotation();// rotates the camera according to the position defined
    }
   
    /// <summary>
    /// set up default camera behaviour
    /// set up default camera behaviour, can be modified later on dependent on states 
    /// </summary>
    public void Refresh ()
    {
        Vector3 desiredpos;// this position is what is desired
                           // aim to follow the object at set speed after the smoothedspeed assigned in the inspector
                           if (camlookatPos == null)
                           {
                               Debug.LogWarning("Missing player ref !", this);
                               return;
                           }

                           if (offsetPostionSpace == Space.Self)
                           {
                               //transform.position = camlookatPos.Transformpoint(offset);
                               desiredpos = camlookatPos.TransformPoint(offset);
                               Vector3 smoothedposition = Vector3.Lerp(transform.position, desiredpos, smoothSpeed);// lerp determine how the camera follow the targeted object
                               transform.position = smoothedposition;                            
                           }
                           else
                           {
                              // transform.position = camlookatPos.transform.position + offset;
                               desiredpos = camlookatPos.transform.position + offset;
                               Vector3 smoothedposition = Vector3.Lerp(transform.position, desiredpos, smoothSpeed);// lerp determine how the camera follow the targeted object
                               transform.position = smoothedposition;
                            }

                           if (CameraLocked)// changed to use property
                           {                            
                            transform.LookAt(camlookatPos);
                           }
                             
    }
    /// <summary>
    /// zooms the camera in toward the player if something else are between the camera and the player
    /// </summary>
	public void Zoomafterplayer()
    {
        //points a ray towards the players current position in scene
        Ray camray = main.ScreenPointToRay(GameManager.managerWasa.playercharacter.transform.position);
        // calculate the distance between the main camera and the player character
        distanceToPlayer = main.transform.position.z - playerchar.transform.position.z;
        RaycastHit hit;
        if(Physics.Raycast(camray,out hit, distanceToPlayer))
        {
            // look for objects that is not the player
            if (hit.collider.gameObject.tag != "Player")
            {
                // move and rotate the camera so that it focuses on the player
                zoomPos = new Vector3(0f, 0.06f, -0.84f);//defines zoomto position
                offset = zoomPos;//set the offset to the value of zoompos, can also be prequeled by a slow movement by the camera

            }
            else if(hit.collider.gameObject.tag == "Player")
            {
                offset = origpos;
            }
        }
    }
    /// <summary>
    /// Use a from-to rotation to rotate the camera after camlookatpos
    /// </summary>
    private void DefineRotation()
    {
        transform.eulerAngles = new Vector3(16.706f, transform.eulerAngles.y, 0);
    }
    /// <summary>
    /// rotate the camera after the mouse position
    /// </summary>
    public void CameraRotAfterMouse()
    {       
    }
    /// <summary>
    /// Find the mouseposition on screen in order to use for rotation of camera
    /// could be of return type float or returntype vector2.
    /// </summary>
    public void FindMousePosition()
    {
    }
    /// <summary>
    /// Zoom out far behind vasa when needed for example when skiing or running from guards
    /// </summary>
    public void ZoomoutFar()
    {
    }
   
}
