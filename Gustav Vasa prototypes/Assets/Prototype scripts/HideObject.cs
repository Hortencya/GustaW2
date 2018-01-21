using UnityEngine;
using System.Collections;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class HideObject : MonoBehaviour
    {
        //instance variables
        [SerializeField]
        private GameObject hideObject;//object haybale
        private ThirdPersonCharacter character;
        private bool isHiding;// bool determining if the player are hiding or not
        private bool inRange;// determines if the player is in reach or not
        private bool firstInteract;// determines if this is the first time we interact with the object 
        private int reactionIndex; 
        private Vector3 origpos;// this are updated after the players positon
        [SerializeField]
        private GameObject instruction;// in order to tie the game object of instructions to the hay, it is in a serialized field

        private float instTimer;// used as timer similar to how this work in jug
        private float txtWait;//

        //Here I decided to add a property for the  originalposition for greater accesibility
        public Vector3 Originalpos
        {
            get { return origpos; }
            set { origpos = value; }
        }
        public bool IsHiding
        {
            // get accessibility for the ishiding variable for other objects that are tagged as hideable
            get { return isHiding; }
        }
        public bool InRrange
        {
            // get acessibility to the inrange bool
            get { return inRange; }
        }
        public int ReactionIndex
        {
            get { return reactionIndex; }
            set { reactionIndex = value; }
        }

        public void Awake()
        {
            //initialization of values
            isHiding = false;
            inRange = false;
            firstInteract = false;
            reactionIndex = -1; // always initiated as -1 to ensure that proper changing is            
            txtWait = 10f;
            instTimer = 0f;//set timer to zero at start
            
            instruction = GameObject.Find("Instructions¨");// find the object for instruction
            character = GetComponent<ThirdPersonCharacter>();

        }
        private void Hideplayer()
        {
            // this method moves the players position to the position of the hay object and disable the meshrenderer
            hideObject.GetComponent<BoxCollider>().enabled = false;//remove the boxcollider from the hay bale       
            Originalpos = transform.position;// sets the original position to that of the player
            transform.position = hideObject.transform.position;// moves the player object to the position of the haystack
            character.enabled = false;// disable the script for player movement
            isHiding = true;
            // Here is also soundeffects and animations added later down the line should we decide to keep this part of the protoype
            DetermineReactions();
        }
        private void RevealPlayer()
        {
            transform.position = origpos;// the player jumps back to where he was before hiding
            hideObject.GetComponent<BoxCollider>().enabled = true;
            character.enabled = true;// enabels the script for player movement
            Debug.Log("revealed");
            // sound and visual effects handled by code below
            DetermineReactions();

        }
      private void DetermineReactions()
        {
            switch(reactionIndex)
        {
                case 0:
                    // call visuals from the haybale
                    hideObject.GetComponent<Haybale>().DecideWhichEffect();
                    break;
                    // how only one object(haybale have been implemented) but below here several moore cases can be added
                    // when more types of hidingplaces is relevant to handle.
        }
        }
        private void InteractWithHay()
        {     // the code below only runs when the player is in range of a stack of hay
            if (inRange)// check if the player is in range for hiding in the hay
            {
                //Timer variables      
                instTimer = 0f;
                instTimer += Time.deltaTime;
                // This block handles GUI information to the player
                if (!firstInteract && instTimer < txtWait)// check if this is the first time we interacts with the hay
                {
                    // set texmesh property text to an instruction about what to do
                    instruction.GetComponent<TextMesh>().text = "Press X to hide in the stack of hay";// gives instruction the first time the object are interacted with
                }
                else if (!firstInteract && instTimer < txtWait && isHiding)
                    instruction.GetComponent<TextMesh>().text = "Left-Click to leave hay";

                // This block handles the input from the player
                if (Input.GetKeyDown(KeyCode.X))
                {
                    instTimer = 0;// reset timer
                    instTimer = Time.deltaTime;//restart timer                                               // Hides player in the hay
                    Hideplayer();
                }
                else if (Input.GetMouseButtonDown(1) && isHiding)// check the following, is the player in the hay?Is leftmousebutton pressed
                {
                    // calls revealplayer method this make the playr reappear onn his original postion
                    RevealPlayer();
                }
            }
        }
        public void OnTriggerStay(Collider other)
        {
            if (other.tag == "Hideable")
            {
                hideObject = other.gameObject;//set the hideobject to the collided object
                inRange = true;
                InteractWithHay();
                //Boolean works
                //collision works
            }

        }
        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "Hideable")
            {
                inRange = false;
                //boolean works
                //collison works
            }

        }
    }
}
