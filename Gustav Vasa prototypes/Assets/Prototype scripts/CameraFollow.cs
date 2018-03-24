using UnityEngine;
using System.Collections;
<<<<<<< HEAD

public class CameraFollow : MonoBehaviour {
    
    private  Transform player;
=======
/// <summary>
/// Needs a softer manipulation of the camera movements right now they come of as harsh, instead of locking the position of the camera to a specific point
/// We should use a rotation formula that make the camera to always move towards the cameraposition modifier  object
/// </summary>
public class CameraFollow : MonoBehaviour
{

    private Transform camlookatPos;// target the camera looks after to follow
>>>>>>> Ingeborg
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Space offsetPostionSpace = Space.Self;
    [SerializeField]
    private bool cameraIsLocked = true;// bool that tells us the camera is locked ie outside of player control
<<<<<<< HEAD
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Cameratransform").transform;// find the player from within the scene
    }
   
=======
    private Camera main;// ref to main camera is set to the variable set in game manager
    
    // Camera manipulation variables, these are used for manipulaing the cameras position in the scene 
    private bool zoomOut;//bool that determines if the camera shall be zoomed out
    private Vector3 zoomPos;// vector used to change the values of the offset variable in order to move the camera
    private Vector3 origPos;// variable that saves down the original position of the camera
    Vector3 distanceToPlayer;// this variable is calculated during runtime based on current position of player compared to camera
    public GameObject playerchar;// refers to the player as defined in game manager   
    [SerializeField]// this value might be needed to be adjusted in the inspector and is thus serialized
    private float smoothSpeed;// the camera uses this to determine how fast it moves toward the target object/OBS it has to be bigger than 0 but smaller than 1
    [SerializeField]
    private float maxYSpin;// max value the player can rotate the camera after
    [SerializeField]
    private float minYSpin;// min value the player can rotate the camera after(has to be negative)
    [SerializeField]
    private float zoomspeed;
    private float rotspeed = 3f;// speed the camera rotates with
    private bool zoomed;

    private void Awake()
    {
        origPos = new Vector3(0f, 1.23f, -3.77f);
        offset = origPos; // set correct offset to origpos        
        camlookatPos = GameObject.FindGameObjectWithTag("Cameratransform").transform;// find the player from within the scene   
        zoomed = false;

    }
    private void Start()
    {
        main = GameManager.managerWasa.mainCamera;// set main to refer to the game managers camera reference this way long names avoided throughout the script
        playerchar = GameManager.managerWasa.playercharacter;
    }
>>>>>>> Ingeborg
    public bool CameraLocked
    {
        get { return cameraIsLocked; }
        set { cameraIsLocked = value; }// can be either true or false
    }

<<<<<<< HEAD
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
	
	
=======
    private void FixedUpdate()
    {
        Refresh();// updates the cameras position runtime 
        //Zoomafterplayer();// the camera offset are changed 
    }
    private void Update()
    {
        DefineRotation();// rotates the camera according to the position defined  
        CheckPressedMousebutton();
    }
    //    /// <summary>
    //    /// set up default camera behaviour, can be modified later on dependent on states 
    //    /// </summary>
    public void Refresh()
    {
        Vector3 desiredpos;// this position is what is desired

        // set the camlookatpos with the help of the rayconditions method
        //main.transform.position = RayConditions(main.transform).position;
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
    //    /// <summary>
    //    /// zooms the camera in toward the player if something else are between the camera and the player
    //    /// </summary>
    	public void Zoomafterplayer()
        {   
            //// testing rays
            //Ray camray = main.ScreenPointToRay(GameManager.managerWasa.playercharacter.transform.position);
            ////Ray backward = main.ScreenPointToRay(lookbehindDist.transform.position);

            ////calculate the distance between the main camera and the player character
            //
            //float distanceBehind =  0.5f;


        }
        private Transform RayConditions(Transform pos)
        {
            RaycastHit hit;
            RaycastHit backHit;

            // the following code initiates two rays cast to use in a set of ifstatements to check how the camera should move
            // in relation to the player
            Ray forwardRay = new Ray(pos.transform.position,pos.transform.TransformDirection(Vector3.forward));        
            Ray backwardRay = new Ray(pos.transform.position,pos.transform.TransformDirection(Vector3.back));
        Debug.DrawRay(pos.transform.position, pos.transform.TransformDirection(Vector3.forward), Color.blue);
        Debug.DrawRay(pos.transform.position, pos.transform.TransformDirection(Vector3.back), Color.red);
            //Raycasts used to see which position the player has in relation to the sent in position which is the cameras original position
 
            if(Physics.Raycast(forwardRay,out hit)&&Physics.Raycast(backwardRay,out backHit))
            {// check if the gameobject hit with the forward vector is the player
                if (hit.collider.gameObject.tag == "Player")
                {
                // if so check there is something behind the player
                    if (backHit.collider.gameObject.tag != "Player")
                    {
                        // vector 3 that get position of the object for the farthest distance the camera can go
                        Vector3 lookback = GameObject.FindGameObjectWithTag("CamLookbehind").transform.position;
                        // check if the distance between the hit object and the player are bigger than the distance between the maximum zoom out value and the player
                         if (Vector3.Distance(playerchar.transform.position,backHit.point)>Vector3.Distance(playerchar.transform.position,lookback))
                         {
                        pos.position = Vector3.Lerp(playerchar.transform.position, main.transform.position, 0.2f);
                        //return RayConditions(pos);
                        // pos.transform.position = origPos;// set position to the original one
                    }
                    }
                else
                    {
                    // no changes to position                     
                    }                   
                }
                else if(hit.collider.gameObject.tag != "Player")
                {
                Debug.Log("Where is the player?");
                   //if hit does not see the player 
                    if (backHit.collider.gameObject.tag != "Player")
                    {
                    //pos zooms in as close as it can 
                    distanceToPlayer = main.transform.position - playerchar.transform.position;
                    pos.position = Vector3.Lerp(playerchar.transform.position, main.transform.position, 0.2f);

                    //return RayConditions(pos);
                    // define the minimumdistance for the camera 
                    Vector3 miniDist = GameObject.FindGameObjectWithTag("MinimumDist").transform.position;
                    // check if the camera----->player distance is less than the minimumdistance
                    if (Vector3.Distance(playerchar.transform.position, main.transform.position) < Vector3.Distance(playerchar.transform.position, miniDist))
                        {
                        // rotate camera around so that player is in view.
                        Vector3 correction = new Vector3(0f, 0.1f, 0f);
                        camlookatPos.transform.Rotate(correction, rotspeed);
                        }
                   
                  }
               
                }
            }
            
            return pos;
            //Ray that determines the if the camera should be zoomed in or out
            //if (Physics.Raycast(main.transform.position, main.transform.TransformDirection(Vector3.back), out hit))
            //{
            //    if (hit.distance < distanceBehind)
            //    {
            //        offset += new Vector3(0, 0, 0.1f);
            //        //drive function that allows zooming out using the V key
            //        ZoomOutandIn();
            //    }
            //}
            //if (Physics.Raycast(main.transform.position, main.transform.TransformDirection(Vector3.forward), out hit))
            //{
            //    if (hit.collider.gameObject.tag != "Player")
            //    {
            //        offset += new Vector3(0, 0, 0.1f);
            //    }
            //    else if (hit.collider.gameObject.tag == "Player" && hit.distance < 3f)
            //    {
            //        offset -= new Vector3(0, 0, 0.1f);
            //        // offset = origPos;
            //    }
            //}
        }
    //    /// <summary>
    //    /// Zoom out far behind vasa whenever there are nothing behind gustav giving a better field of view when moving in open spaces
    //    /// </summary>
    public void ZoomOutandIn()
    {
        if (Input.GetKeyDown(KeyCode.V) && !zoomed)
        {
            zoomPos = new Vector3(0, 2.36f, -10.14f);
            offset = zoomPos; zoomed = true;
        }
        else if (Input.GetKeyDown(KeyCode.V) && zoomed)
        {
            offset = origPos;
            StartCoroutine(waitforSec());// calls method that wait a certain amount of secounds
            zoomed = false;
        }

    }
    //    /// <summary>
    //    /// enumerator that tells the program to wait two secounds before turning the zoomed bool false.
    //    /// </summary>
    //    /// <returns></returns>
    IEnumerator waitforSec()
    {
        yield return new WaitForSeconds(2);
    }
    //    /// <summary>
    //    /// Use a from-to rotation to rotate the camera after camlookatpos
    //    /// </summary>
    private void DefineRotation()
    {
        transform.eulerAngles = new Vector3(16.706f, transform.eulerAngles.y, 0);
    }
    //    /// <summary>
    //    /// rotate the camera after the mouse position tracked runtime
    //    /// </summary>
    public void RotAfterMousePos()
    {
        // rotation of camera after mouse position while left mous button is held
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 mouseRot = new Vector3(0, mouseX, 0); // create vector for camera rotation after the xposition of the mouse.
        camlookatPos.transform.Rotate(mouseRot * rotspeed);
    }
    //    /// <summary>
    //    /// Method that checks if the left mouse button are currently pressed and if so drives the CameraRotAfterMouse method
    //    /// </summary>
    private void CheckPressedMousebutton()
    {
        // if statement that checks if the current rotation value is within the range set as max and min rotation
        if (Input.GetMouseButton(0))
        {
            RotAfterMousePos();
        }
    }

>>>>>>> Ingeborg
}
