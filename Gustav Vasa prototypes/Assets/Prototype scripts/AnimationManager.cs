using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this class handles all of the animations for players and ai alike although using different methods to 
/// apply animations to them, since the enemies choices of animations are state 
/// </summary>
public class AnimationManager : MonoBehaviour
{
    private GameObject player;// reference to the playercharacter to figure out the current y rotation
    private Rigidbody rigid;//rigidbody for control of the speed 
    MoveVasa movement;// gets the movement script for gustav vasa 
    [SerializeField]
    private float speed;//speed of player saved once the player start to go downhill
    private Animator playerAnim;//reference to the animator in the player object    
    private Animator gustavSkiing;//animator gustavskiing   
    private Quaternion oldRota;// takes gustavs original rotation in all angles to be used in comparison to the current rotation
    private bool skiingmodelactive;
    //objects for testing if this work put it inside the gamemanager and get them from there
    [SerializeField]
    private GameObject vasaWithSkiis;
    [SerializeField]
    private GameObject vasaWithoutSkiis;
    // ski animations.
    /// <summary>
    /// Awake function that finds the player character
    /// </summary>
    public void Start()
    {
        player = GameManager.managerWasa.playercharacter;
        rigid = player.GetComponent<Rigidbody>();
        movement = player.GetComponent<MoveVasa>();
        playerAnim = player.GetComponentInChildren<Animator>();
        gustavSkiing = vasaWithSkiis.GetComponent<Animator>();
        skiingmodelactive = false;
        gustavSkiing.enabled = false;
        oldRota = player.transform.rotation;// save players rotation in the beginning of the game
        
    }
    public void Update()
    {
        speed = rigid.velocity.magnitude;
        if (skiingmodelactive && movement.IsGrounded)
        {
            SkiiChangeAfterPlayerTilt();
        }
        else if (skiingmodelactive && !movement.IsGrounded)
        {
            gustavSkiing.SetBool("Downhill", true);
            gustavSkiing.SetBool("Planar", false);            
        }
    }
    /// <summary>
    /// property for a boolean determining if vasa wears skiis 
    /// </summary>
    public bool SkiingModelActive
    {
        set { skiingmodelactive = value; }
    }
    //PlayerAnimations
    /// <summary>
    /// sets the bools for starting the walkanimation in the player character
    /// </summary>
	public void StartWalkanimation()
    {
        if (skiingmodelactive)
        {
            vasaWithoutSkiis.SetActive(true);            
            vasaWithSkiis.SetActive(false);
            gustavSkiing.enabled = false;
            playerAnim.enabled = true;
            skiingmodelactive = false;
        }
        playerAnim.SetBool("Walks",true);       
    }
    /// <summary>
    /// Starts the animations for skiing enabling and disabling different animations depending on the tilt of the 
    /// player character.
    /// </summary>
    public void SkiingAnimation()
    {
        //put on skiis on gustav vasa 
        playerAnim.SetBool("Walks", false);
        vasaWithSkiis.SetActive(true);
        playerAnim.enabled = false;
        vasaWithoutSkiis.SetActive(false);
        gustavSkiing.enabled = true;
        skiingmodelactive = true;       
    }
    /// <summary>
    /// This method is called by update and only if the skiingmodelactive bool is true, it checks if the players tilt exeeds
    /// a specific degree or if gustav goes in a left or right direction and plays different animation tilt in y direction and
    /// as well as x direction
    /// </summary>
    private void SkiiChangeAfterPlayerTilt()
    {
        bool tilts = TiltDown(oldRota, player.transform.rotation);
        
        //bool right = TurnRight(oldRota, player.transform.rotation);
        if (tilts)
        {
            gustavSkiing.SetBool("Downhill", true);
            gustavSkiing.SetBool("Planar", false);           
            if (Input.GetKey(KeyCode.D))
            {
                //Debug.Log("right");
                gustavSkiing.SetBool("Left", false);
                gustavSkiing.SetBool("Right", true);
            }                          
            if (Input.GetKey(KeyCode.A))
             {
                //Debug.Log("left");
                gustavSkiing.SetBool("Right",false);
                gustavSkiing.SetBool("Left",true);                   
             }
            if(Input.GetKey(KeyCode.W))
            {
                gustavSkiing.SetBool("Right", false);
                gustavSkiing.SetBool("Left", false);
            }
            
        }
        if(!tilts||rigid.velocity.magnitude<=10)
        {
            //play stop haste animation
            gustavSkiing.SetBool("Stop haste", true);
            gustavSkiing.SetBool("Downhill", false);
            //Debug.Log("haste lowers");
            if (rigid.velocity.magnitude < 15)
            {
                VelocityChangeLow();
            }               
        }
        if (!player.GetComponent<MoveVasa>().IsGrounded)
        {
            gustavSkiing.SetBool("Downhill", true);
            gustavSkiing.SetBool("Planar", false);
        }

    }
    /// <summary>
    /// Check if the players velocity.Magnitude is lower than what it was when starting going downhills this triggers the animation
    /// for useage of staffs to get forward and if velocity.magnitude is lower than [specified value] the animations swich down to
    /// the planar skiing animation otherwise it goes back to the skiing downhill animation
    /// </summary>
    private void VelocityChangeLow()
    {      
       // if we goes so slow it is motivated put the stop haste animation to false too and start planar
       if (rigid.velocity.magnitude < 8)
         {
            gustavSkiing.SetBool("Stop haste", false);
            gustavSkiing.SetBool("Planar", true);
            //Debug.Log("Planar");
         }
       // if the velocity goes up again the speed will increase once again
       if (rigid.velocity.magnitude >10)
         {
           gustavSkiing.SetBool("DownHill", true);
           gustavSkiing.SetBool("Stop haste", false);
            gustavSkiing.SetBool("Planar", false);
           //Debug.Log("speed up");
         }
        

    }
    
    /// <summary>
    /// Bool that checks if we are tilting downwards comparing the old rotation with the current one.
    /// It takes two quarternions as parameters and returns true if we are tilting down false if we are tilting up
    /// </summary>
    /// <returns></returns>
    private bool TiltDown(Quaternion from, Quaternion towards)
    {
        float fromX = from.eulerAngles.x;
        float toX = towards.eulerAngles.x;
        float tiltDown = 0f;
        float tiltUp = 0f;
        //if the original axis Y aspect is bigger than the y aspect of tha axis our player are turning towards
        //the below code is executed
        if (fromX <= toX)
        {
            tiltDown = toX - fromX;
            tiltUp = fromX + (360 - toX);
        }
        //else it is will be reversed
        else
        {
            tiltDown = (360 - fromX) + toX;
            tiltDown = fromX - toX;
        }
        // true if tiltdown is equal to or bigger than tiltup
        return (tiltDown <= tiltUp);
    }
    /// <summary>
    /// Bool that checks if we are turning left or right. This is not used right now but will be saved for later useage
    /// when the NPCs or other characters has their direction changed or other scripts want to know how gustav is turned
    /// </summary>
    /// <returns></returns>
    private bool TurnRight(Quaternion from, Quaternion towards)
    {
        float fromY = from.eulerAngles.y;
        float toY = towards.eulerAngles.y;
        float clockwise = 0f;
        float counterClockwise = 0f;
        if (fromY <= toY)
        {
            clockwise = toY - fromY;
            counterClockwise = fromY + (360 - toY);
        }
        else
        {
            clockwise = (360 - fromY) + toY;
            counterClockwise = fromY - toY;
        }
        return (clockwise <= counterClockwise);
    }
    /// <summary>
    /// Shifts between a set of idle animations when the player does nothing
    /// </summary>
    public void IdleCarousel()
    {
        //check if gustav vasa is wearing his skiis.
        if (skiingmodelactive)
        {
            vasaWithSkiis.SetActive(false);
            vasaWithoutSkiis.SetActive(true);
            playerAnim.enabled = true;
            gustavSkiing.enabled = false;
            skiingmodelactive = false;
        }           
        playerAnim.SetBool("Walks", false);
        //Here a switch state-machine for different idles could be implemented here
    }
}
