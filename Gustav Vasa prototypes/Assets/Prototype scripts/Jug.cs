using UnityEngine;
using System.Collections;



    public class Jug : MonoBehaviour
    {
        // This script have been reworked to only represent the functions of the actual Jug object, all the throwing and picking up of object are now handled by the throwable object class
        private bool jugPickedup;//determines if the jug has been just been picked up(hinders effects from happen over and over again)
        private bool hitOnce;// hinders the jug from playing crasch effects more than once
        bool setDistractioncall;// determines if we have sent the point of distraction to the enemies yet.
        private ThrowableObject throwable;
        private GameObject player;
        private bool contact;// Check if the vase has contact with the ground/terrain
        [SerializeField]

        AudioSource jugsound; // is played when hiting the ground
        public AudioClip[] reactionsound; // array of soundclips for different results

        void Awake()
        {
            jugsound = GetComponent<AudioSource>();
            jugPickedup = false;  
            hitOnce = false;
            contact = false;
        }
        void Start()
        {
            throwable = GameManager.managerWasa.playercharacter.GetComponent<ThrowableObject>();
        }
        // Important note the method called GrabEffect is called the same for every object used by throwable object
        // the methods are named the same but contains different things and refer different objects, ie jug.Grabeffects, glass.Grabeffects, etc
        private void GrabEffects()
        {
            jugsound.clip = reactionsound[0];
            jugsound.Play();
            jugPickedup = true;           

        }
        private void Crasheffects()
        {
            jugsound.clip = reactionsound[1];
            jugsound.Play();
            // shatter effect is played up  
            hitOnce = true;
        }
        private void DecideEffects()
        {
            // Number of if-statements that determines which effects are playing
            if (throwable.PickedUP && !jugPickedup)
            {
                Debug.Log(throwable.PickedUP);
                GrabEffects();// plays the sound once for grabbing the jug when pcikedup is set to true
            }
            else if (throwable.HitGround && !hitOnce)
            {
                Crasheffects();// plays the sound and visual effect for when the Jug hits the ground
                               //GameManager.managerWasa.DrawAttention();

            }
        }
        //this checks for when the jug hits the ground and then sets off the function inside of the ai that calls the soldiers to the spot
        void OnCollisionEnter(Collision collision)
        {        
        //check if the throwable object is thrown
            if (throwable.IsThrown == true)// ask for the property rather than instance variable
            {
            // set hit ground to true
                throwable.HitGround = true;// set the property rather than the instance variable  
                if (!contact)// if bool contact is false
                {
                    ContactPoint con = collision.contacts[0];// contact point zero found
                    Vector3 pos = con.point;// contact point zeros vector 3 found
                    throwable.SendPosition(pos);// send position of contactpoint 0 to the throwable object
                    GameManager.managerWasa.SetDistractedBool();// set bool for distraction to true                   
                    contact = true;  // no contact is true.                                
                }
                if (!setDistractioncall)
                {
                // if set distractionbool above is false the program goes through every enemy in the scene and starts their distraction state
                foreach (GameObject o in GameManager.managerWasa.danishSoldiers)
                    {
                        o.GetComponent<SoldierBehaviour>().SetDistractionState();// set the state of distraction inside of the ai.
                    }
                    setDistractioncall = true;// when the above is done once the setdistraction bool is turned true thus it would not happen more than once.
                
                }
                
            }
        }
        void Update()
        {
            DecideEffects(); // checks a number of different conditions to see which of the effects should be playing

        }
    }

    
