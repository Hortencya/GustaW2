using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {
    // this script is meant to do what the jug script earlier did but with any object tagged as "Throwable"
   
     // Gameobjects needed by throwable object
   
     
    private  GameObject holdobj;// same as with player
    [SerializeField]
    private GameObject[] danishEnemy; // reference to the enemies always contain all the enemies in the scene
    [SerializeField]
    private GameObject[] enemiesInrange; // contains only the enemies that are inside of the hearing distance
    [SerializeField]
    private GameObject currentPickup;// the object currently hold by the player
   // I decided that I do not want a first time picked up bool due to that the same gui could appear to let the player grab an object whenever he is in range for something throwable

        //booleans used by throwable object
    private bool pickedUp;// determines if the player have grabbed it, attaches object to the player
    private bool hitGround; // when true the objects specific sounds and hit animations are played and the enemy are alerted through method in this script
    private bool isthrown;// calls method that handles the physics for throwing
    private bool inRange; //determines if the player are in range of the object to pick it up
    private float throwforce = 450;// assigned according to which object are being used

    // Scripts refered to in throweable object, here Jug and other scripts for instance glass, woodenpiece, cheramics etc
    private Jug jugObject;
    

    // GUI variables these can be attached to a GUi Script separate from this if needed
    private GameObject instruction;// reference the object that holds the instructions for the player
   

    // properties for this throwable object class, these can be used by any objects referencing this code
   public bool IsThrown
    {
        // read only property
        get { return isthrown; }
    }
    public bool PickedUP
    {
        // read only property
        get { return pickedUp; }
    }
    public bool HitGround
    {
        // both read and write properties in this class
        get { return hitGround; }
        set { hitGround = value; }
    }
    void SetUpInteractability()
    {
        // This method sets up the script so that the player can pick up certain objects
        holdobj = GameObject.FindGameObjectWithTag("Hold");// assigns the holdobject a reference
        instruction = GameObject.FindGameObjectWithTag("InstructObject");// assings the instruction object a reference
        danishEnemy = GameObject.FindGameObjectsWithTag("Danish");// list with all danish enemies
        inRange = false;// Inrange is false at start
        pickedUp = false;// sets pickedup to false
       
    }
    void Awake ()
    {
        SetUpInteractability();
	}

    // Update is called once per frame
    void Interact()
    {
        if (inRange && !pickedUp)
        {      
            // Change the instructions from empty to given instruction  
            instruction.GetComponent<TextMesh>().text = "Press X to pick up";
            PickUp();// calls the function for picking the Jug up from the ground
        }
        
                    

        if (pickedUp)
        {
            Throwdistraction();// Here it is possible for the player to throw the Jug
        }
        else
        {
            instruction.GetComponent<TextMesh>().text = string.Empty;
        }

    }


    void PickUp()
    {
        // Check if the player press the X button on the keyboard
        if (Input.GetKeyDown(KeyCode.X))
        {
            instruction.GetComponent<TextMesh>().text = "Left-click to throw";
            pickedUp = true;
            // parent the current object to the holdobject
            currentPickup.transform.parent = holdobj.transform;
            // change transform.position of the jug to where the players hands are supposed to be
            currentPickup.transform.position = holdobj.transform.position;        
        }
    }
    void Throwdistraction()
    {
        if (Input.GetMouseButtonDown(1))
            {
                isthrown = true;// allow checkground to be run
                instruction.GetComponent<TextMesh>().text = string.Empty;       
                currentPickup.transform.parent = null;
                // add rigidbody component and enable kinematicness
                Rigidbody temporaryRigid = currentPickup.AddComponent(typeof(Rigidbody)) as Rigidbody;
                temporaryRigid = currentPickup.GetComponent<Rigidbody>();
                temporaryRigid.isKinematic = false;
                // Throw the jug in the direction the player are turned
                //get throwforce through a property
                temporaryRigid.AddForce(transform.up + transform.forward * throwforce);// call property instead of instance variable
                temporaryRigid.useGravity = true;
                // when the jug hits the ground hitgroud = true;
                if (HitGround== true)// use of property instead of instance variable ensures that right value is assigned
                {
                Debug.Log("hitground");
                    // a crach sound plays
                    //object destroyed
                    CheckRange();
                 }
        }
         
    }
    void CheckRange()
    {

        // Check how many of the enemies are in range
        // Place these enemies in separate list
        // Alert all enemies in the enemiesInrange list and set their State change to DISTRACTED

    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Throwable")
        {                  
            currentPickup = other.gameObject;// sets the current pickedup reference to the object the player is currently interacting with
            inRange = true;
            
           
        }
         
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Throwable")
            inRange = false;
    }
    void Update()
    {
        Interact();
    }

}
    


