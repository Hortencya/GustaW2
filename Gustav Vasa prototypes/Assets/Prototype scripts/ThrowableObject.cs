using UnityEngine;
using System.Collections;
 
public class ThrowableObject : MonoBehaviour {
    // this script is meant to do what the jug script earlier did but with any object tagged as "Throwable"    
    //private  GameObject holdobj;// use game manager instead
    [SerializeField]
    private GameObject[] enemiesInrange; // contains  the enemies that are inside of the hearing distance
    [SerializeField]
    private GameObject currentPickup;// the object currently hold by the player
   // I decided that I do not want a first time picked up bool due to that the same gui could appear to let the player grab an object whenever he is in range for something throwable

        //booleans used by throwable object
    private bool pickedUp;// determines if the player have grabbed it, attaches object to the player
    private bool hitGround; // when true the objects specific sounds and hit animations are played and the enemy are alerted through method in this script
    private bool isthrown;// calls method that handles the physics for throwing
    private bool inRange; //determines if the player are in range of the object to pick it up
    private bool pressedXonce;// bool to check if a player has pressed x(reset this when hitground is true)
    private float throwforce = 450;// assigned according to which object are being used

    // Scripts refered to in throweable object, here Jug and other scripts for instance glass, woodenpiece, cheramics etc
    private Jug jugObject;
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
        inRange = false;// Inrange is false at start
        pickedUp = false;// sets pickedup to false
        pressedXonce = false;
       
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
           GameManager.managerWasa.instructions.GetComponent<TextMesh>().text = "Press X to pick up";
            PickUp();// calls the function for picking the Jug up from the ground
        }                   

        if (pickedUp)
        {
            Throwdistraction();// Here it is possible for the player to throw the Jug
        }
        else
        {
           GameManager.managerWasa.instructions.GetComponent<TextMesh>().text = string.Empty;
        }

    }
  
    void PickUp()
    {
        // Check if the player press the X button on the keyboard
        if (!pressedXonce&&Input.GetKeyDown(KeyCode.X))
        {
            GameManager.managerWasa.instructions.GetComponent<TextMesh>().text = "Press X to throw";
            pickedUp = true;
            // parent the current object to the holdobject
            currentPickup.transform.parent = GameManager.managerWasa.holdobject.transform;
            // change transform.position of the jug to where the players hands are supposed to be
            currentPickup.transform.position = GameManager.managerWasa.holdobject.transform.position;
            StartCoroutine(InteractWait());
                       
            
            }
    }
    void Throwdistraction()
    {
        if ( pressedXonce&& Input.GetKeyDown(KeyCode.X))
            {
                isthrown = true;// allow checkground to be run
                GameManager.managerWasa.instructions.GetComponent<TextMesh>().text = string.Empty;       
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
                    StartCoroutine(InteractWait());                    
                    // a crach sound plays
                    //object destroyed
                   // SendPosition(); kan kallas av underliggande script
                 }
        }
         
    }
    private IEnumerator InteractWait()
    {
            yield return new WaitForSeconds(1);
            if (pressedXonce)
                pressedXonce = false;
            else
                pressedXonce = true;
    }
    public void SendPosition(Vector3 pos)// send in the hitposition
    {
        GameManager.managerWasa.temporaryPos = pos;
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




