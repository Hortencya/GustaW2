using UnityEngine;
using System.Collections;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class Haybale : MonoBehaviour
    {
        private GameObject playerRef;// this variable stores the information about the player
        private HideObject hiding;// this variable stores an reference to the HideObject, in order to access certain properties
        private AudioSource haysound;
        private bool hideEffects;// determines if the visuals/ sound for hiding already played once.
        private bool revealEffects;// determines if the visuals/ sound for revealing is already played once.
        [SerializeField]
        private AudioClip[] hayAudio = new AudioClip[2];// used for soundeffects
        private ParticleSystem hayvisual;//this is a particle system used for displaying rustling hay
        
       private  void Awake()
        {
            SetUp();
            
        }
       private void SetUp()
        {
            // ensures that the particle system on haybale object is not set to loop or start playing when the game is first turned on
            playerRef = GameObject.FindGameObjectWithTag("Player");// this are used to acess the hideobject class as well as player animations
            hiding = playerRef.GetComponent<HideObject>();// finds the componenet of hideobject in the player object
            haysound = GetComponent<AudioSource>();// finds the audiosource on the haybale object
            hayvisual = GetComponent<ParticleSystem>();// finds the particle system on the haybaleObject
            hayvisual.loop = false;
            hayvisual.playOnAwake = false;
            // set  the booleans of the individual haybale object to false
            hideEffects = false;
            revealEffects = false;     
        }
       
       private void PlayJumpInEffects()
        {         
            // play back rustle effect 1
            haysound.clip = hayAudio[0];// the audiosources clip to represent jumping in to the hay for hiding
            haysound.Play();
            // start the particle system making it play once
            hayvisual.Play();
            // later on player animations for jumping into the hay can be added
            hideEffects = true;// this has played once already
          
        }
        private void PlayJumpOutEffects()
        {
            // play back rustle effect 2
            haysound.clip = hayAudio[1];
            haysound.Play();
            // start the particle system making it play once
            hayvisual.Play();
            // later on player animations for jumping out of the hay can be added.
            revealEffects = true;// this has played one already           
        }
        public void DecideWhichEffect()
        {
            if (hiding.IsHiding && !hideEffects)
            {
                // play up sound and visulas for hiding
                PlayJumpInEffects();
                hideEffects = true;
               
            }
            else if (!hiding.IsHiding && !revealEffects)
            {
                // play up sound and visuals for revealing
                PlayJumpOutEffects();
                revealEffects = true;              
            }
        }
        public void Update()
        {
            // small ifstatements that determines how the visual effects and sfx will work, active if set to 0, inactive if set to -1.
            if (hiding.InRrange)
            {
                hiding.ReactionIndex = 0;
            }
            else if (!hiding.InRrange)
            {
                hiding.ReactionIndex = -1;
            }
        }
    }
}
