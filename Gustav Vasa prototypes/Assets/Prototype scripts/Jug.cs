﻿using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class Jug : MonoBehaviour
    {
        // This script have been reworked to only represent the functions of the actual Jug object, all the throwing and picking up of object are now handled by the throwable object class
        private bool jugPickedup;//determines if the jug has been just been picked up(hinders effects from happen over and over again)
        private bool hitOnce;// hinders the jug from playing crasch effects more than once
        bool setDistractioncall;
        private ThrowableObject throwable;
        private GameObject player;
        private bool contact;
        [SerializeField]

        AudioSource jugsound; // is played when hiting the ground
        public AudioClip[] reactionsound; // array of soundclips for different results

        void Awake()
        {
            jugsound = GetComponent<AudioSource>();
            throwable = GameManager.managerWasa.playercharacter.GetComponent<ThrowableObject>();
            jugPickedup = false;
            hitOnce = false;
            contact = false;
        }
        // Important note the method called GrabEffect is called the same for every object used by throwable object
        // the methods are named the same but contains different things and refer different objects, ie jug.Grabeffects, glass.Grabeffects, etc
        private void GrabEffects()
        {
            jugsound.clip = reactionsound[0];
            jugsound.Play();
            jugPickedup = true;
            Debug.Log("played grabeffects");

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
        //this checks for when the jup hits the ground
        void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint con in collision.contacts)
            {
                Debug.DrawRay(con.point, con.normal, Color.red);
            }
            if (throwable.IsThrown == true)// ask for the property rather than instance variable
            {
                throwable.HitGround = true;// set the property rather than the instance variable  
                if (!contact)
                {
                    ContactPoint con = collision.contacts[0];
                    Vector3 pos = con.point;
                    throwable.SendPosition(pos);
                    GameManager.managerWasa.SetDistractedBool();
                    Debug.Log("distraction started");
                    contact = true;                                  
                }
                if (!setDistractioncall)
                {
                    foreach(GameObject o in GameManager.managerWasa.danishSoldiers)
                    {
                        o.GetComponent<SoldierBehaviour>().SetDistractionState();
                    }
                    setDistractioncall = true;
                }
                
            }
        }
        void Update()
        {
            DecideEffects(); // checks a number of different conditions to see which of the effects should be playing

        }
    }
}
    
