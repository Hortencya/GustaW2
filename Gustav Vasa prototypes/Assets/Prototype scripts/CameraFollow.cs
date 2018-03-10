using UnityEngine;
using System.Collections;
/// <summary>
/// Needs a softer manipulation of the camera movements right now they come of as harsh, instead of locking the position of the camera to a specific point
/// We should use a rotation formula that make the camera to always move towards the cameraposition modifier  object
/// </summary>
public class CameraFollow : MonoBehaviour
{

    private Transform camlookatPos;// target the camera looks after to follow
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
    [SerializeField]
    private float zoomspeed;
    private Transform lookbehindDist;//The distance the camera looks behind itself
    private bool zoomed;

    private void Awake()
    {
        origpos = new Vector3(0f, 1.23f, -3.77f);
        offset = origpos; // set correct offset to origpos
        lookbehindDist = GameObject.FindGameObjectWithTag("CamLookbehind").transform;
        camlookatPos = GameObject.FindGameObjectWithTag("Cameratransform").transform;// find the player from within the scene   
        zoomed = false;  

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
        Zoomafterplayer();// the camera offset are changed        
    }
    private void Update()
    {
        DefineRotation();// rotates the camera according to the position defined  
                   
    }
    /// <summary>
    /// set up default camera behaviour, can be modified later on dependent on states 
    /// </summary>
    public void Refresh()
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
        // testing rays
        Ray camray = main.ScreenPointToRay(GameManager.managerWasa.playercharacter.transform.position);
        //Ray backward = main.ScreenPointToRay(lookbehindDist.transform.position);
       
        //calculate the distance between the main camera and the player character
        distanceToPlayer = main.transform.position.z - playerchar.transform.position.z;
        float distanceBehind = main.transform.position.z - lookbehindDist.transform.position.z;
        RaycastHit hit;
        // Debugger rays that shows how the camera shoots its rays in relation to its position in world space 
        Debug.DrawRay(main.transform.position, Vector3.forward, Color.red);// debug ray for the cameras front viewing
        Debug.DrawRay(main.transform.position, Vector3.back, Color.green);
        //Ray that determines the if the camera should be zoomed in or out
        if(Physics.Raycast(main.transform.position, Vector3.forward, out hit))
        {
            if(hit.collider.gameObject.tag == "Obstacle")
            {
                offset = new Vector3(0f,0.8f,-1.62f);                
            }
            else if(hit.collider.gameObject.tag == "Player")
            {
                offset = origpos;
            }
        }
        if (Physics.Raycast(main.transform.position, Vector3.back,out hit))
        {
            if (hit.collider.gameObject.transform.position.z <distanceBehind)
            {
                //drive function that allows zooming out using the V key
                if (Input.GetKeyDown(KeyCode.V)&&!zoomed)
                {
                    ZoomoutFar();
                    zoomed = true;
                }
                else if (Input.GetKeyDown(KeyCode.V)&&zoomed)
                {
                    offset = origpos;
                    StartCoroutine(waitforSec());// calls method that wait a certain amount of secounds
                    zoomed = false;
                }
              
             
            }
        }
    }
    /// <summary>
    /// enumerator that tells the program to wait two secounds before turning the zoomed bool false.
    /// </summary>
    /// <returns></returns>
    IEnumerator waitforSec()
    {
        yield return new WaitForSeconds(1);
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
    /// Zoom out far behind vasa whenever there are nothing behind gustav giving a better field of view when moving in open spaces
    /// </summary>
    public void ZoomoutFar()
    {
       
        zoomPos = new Vector3(0, 2.36f, -10.14f);
        offset = zoomPos;
    }
 
    

}
