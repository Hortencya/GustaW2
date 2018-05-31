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

    // Camera manipulation variables, these are used for manipulaing the cameras position in the scene 

    GameObject player;
    private Vector3 origPos;// variable that saves down the original position of the camera
    [SerializeField]
    private Quaternion origRot;
    
    [SerializeField]// this value might be needed to be adjusted in the inspector and is thus serialized
    private float smoothSpeed;// the camera uses this to determine how fast it moves toward the target object/OBS it has to be bigger than 0 but smaller than 1
    [SerializeField]
    private float maxYSpin;// max value the player can rotate the camera after
    [SerializeField]
    private float minYSpin;// min value the player can rotate the camera after(has to be negative)
    [SerializeField]
    private float zoomspeed;
    [SerializeField]
    private float camSpeedY;
    [SerializeField]
    private float camSpeedX;
    Vector3 desiredLookatPos;
    float rotaY = 0;
    float zoom=0;
    private void Awake()
    {
        origPos = offset;

       // offset = origPos; // set correct offset to origpos        
        camlookatPos = GameObject.FindGameObjectWithTag("Cameratransform").transform;// find the player from within the scene   
       
        

    }
    private void Start()
    {
        main = GameManager.managerWasa.mainCamera;// set main to refer to the game managers camera reference this way long names avoided throughout the script
        player = GameManager.managerWasa.playercharacter;
        origRot = camlookatPos.localRotation;
        desiredLookatPos = camlookatPos.position;
    }
    public bool CameraLocked
    {
        get { return cameraIsLocked; }
        set { cameraIsLocked = value; }// can be either true or false
    }

    private void FixedUpdate()
    {
        
        Refresh();// updates the cameras position runtime 
        ObsticalZoom();
        //Zoomafterplayer();// the camera offset are changed 
       // CheckPressedMousebutton();
    }
    private void Update()
    {
        
        //DefineRotation();// rotates the camera according to the position defined  
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
            
            Vector3 smoothedposition = Vector3.Lerp(transform.position, desiredpos, smoothSpeed * Time.deltaTime);// lerp determine how the camera follow the targeted object
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
            desiredLookatPos = Vector3.Lerp(desiredLookatPos, camlookatPos.position, smoothSpeed * Time.deltaTime);
            transform.LookAt(desiredLookatPos);
        }
        //main.transform.rotation. = new Vector3(playerchar.transform.eulerAngles.x, main.transform.rotation.eulerAngles.y, main.transform.rotation.eulerAngles.z);
    }
    //    /// <summary>
    //    /// zooms the camera in toward the player if something else are between the camera and the player
    //    /// </summary>
    	
       
    //    /// <summary>
    //    /// Zoom out far behind vasa whenever there are nothing behind gustav giving a better field of view when moving in open spaces
    //    /// </summary>
    public Vector3 ZoomOutandIn()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if((zoom <0.5 || scrollInput<0)&& (zoom > 0 || scrollInput > 0))
        zoom += scrollInput * zoomspeed;
        return Vector3.Lerp(camlookatPos.TransformPoint(origPos), camlookatPos.position, zoom);
       

    }
    public void ObsticalZoom()
    {
        RaycastHit hit;
        Vector3 zoomPos = ZoomOutandIn();
        Debug.DrawRay(camlookatPos.transform.position, camlookatPos.transform.TransformVector(origPos), Color.blue);
        if (Physics.Raycast(new Ray(camlookatPos.transform.position, camlookatPos.transform.TransformVector(origPos) ), out hit ))
        {
            if (hit.collider.tag!= main.tag&& hit.distance<Vector3.Distance(camlookatPos.position, zoomPos))
            {
                offset = Vector3.Lerp( camlookatPos.transform.InverseTransformPoint(hit.point), Vector3.zero,0.1f);
            }
            else
            {
                offset = camlookatPos.transform.InverseTransformPoint(zoomPos);
            }
        }
    }
    //    /// <summary>
    //    /// enumerator that tells the program to wait two secounds before turning the zoomed bool false.
    //    /// </summary>
    //    /// <returns></returns>
   
    //    /// <summary>
    //    /// Use a from-to rotation to rotate the camera after camlookatpos
    //    /// </summary>
    
    //    /// <summary>
    //    /// rotate the camera after the mouse position tracked runtime
    //    /// </summary>
    public void RotAfterMousePos()
    {

        // rotation of camera after mouse position while left mous button is held

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetButton("Fire1") || !player.GetComponent<MoveVasa>().IsSkiing)
        {
            if ((rotaY < 50 || -mouseY < 0) && (rotaY > -50 || -mouseY > 0))
            {
                rotaY -= mouseY * camSpeedY;
            }
            camlookatPos.transform.rotation = Quaternion.Euler(rotaY, transform.rotation.eulerAngles.y + mouseX * camSpeedX, 0);
        }//camlookatPos.transform.rotation = Quaternion.Euler(rotaY, transform.TransformDirection( camlookatPos.parent.rotation.eulerAngles).y , 0); }
        else
        {
             camlookatPos.transform.localRotation = Quaternion.Slerp(camlookatPos.transform.localRotation, origRot, .1f); 
        }
        //Vector3 mouseRot = new Vector3(-mouseY, mouseX, 0); // create vector for camera rotation after the xposition of the mouse.
       // camlookatPos.transform.Rotate(mouseRot * rotspeed);
        
    }
    //    /// <summary>
    //    /// Method that checks if the left mouse button are currently pressed and if so drives the CameraRotAfterMouse method
    //    /// </summary>
    private void CheckPressedMousebutton()
    {
        // if statement that checks if the current rotation value is within the range set as max and min rotation
        //if (Input.GetMouseButton(0))
       // {
            RotAfterMousePos();
       // }
    }

}
