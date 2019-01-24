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
    private Animator playerAnim;//reference to the animator in the player object
    //Test with different animantor object 
    private Animator gustavSkiing;//animator gustavskiing
    //Test end
    private float tilt; // variable for the current y axis rotation of gustav vasa and used mainly in the function that switches between different
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
        playerAnim = player.GetComponentInChildren<Animator>();
        gustavSkiing = vasaWithSkiis.GetComponent<Animator>();
        gustavSkiing.enabled = false;
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
    public void SkiAnimationCarousel()
    {
        //put on skiis on gustav vasa 
        playerAnim.SetBool("Walks", false);
        vasaWithSkiis.SetActive(true);
        playerAnim.enabled = false;
        vasaWithoutSkiis.SetActive(false);
        gustavSkiing.enabled = true;
        skiingmodelactive = true;       
        //switch between the models to the one that has skiis.
        // here a switch statement could be implemented to give Vasa different movements depending on y axis tilt
               
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
