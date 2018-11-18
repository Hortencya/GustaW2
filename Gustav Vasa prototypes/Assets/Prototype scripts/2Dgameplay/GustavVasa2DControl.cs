using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The goal with this script is to provide movement for the player character in the 2DWiew 
/// In order to accomplish that the script shall:
/// 1. Get the animator component from the 2D Player character 
/// 2. Define the basic movement patterns for walking from the left to the ringht
/// 3. Create/call an inpút function that call the movements dependent on input(A= left, D= right)
/// 4. Call the animator state for walking when the movement are called(walking is active)
///
/// In this script basic interaction with objects(ie houses animals people, etc )can be loosly defined.
/// I am not sure if this functionality shall call other scripts or define user functions for these
/// other scripts to call( through inheritance)
/// </summary>
public class GustavVasa2DControl : MonoBehaviour {
    // instance variables
    private Animator vasaAnim;// player character animator
    private Vector3 newScale;// value used to temporarly save data about gustav Vasas current scale
    private MoveCamera2D move2D;
    private GameObject camera;
    [SerializeField]
    private float walkingSpeed;// how fast Gustav Vasa walks forward
    // awake function that calls the animator function and initiate the vector 2 for movement
	void Awake ()
    {
        // step1
        vasaAnim = this.GetComponent<Animator>();// get component for player characters animations
        vasaAnim.SetBool("Walking", false);
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        move2D = camera.GetComponent<MoveCamera2D>();
        //move2D = 
        
    }
    //step2
    // this function will cover walking righ and left and is called in the update function whenever
    // the player gives input as either A/left arrowkey or D/right arrowkey. Sprite scale is multiplied
    // with minus 1 when walking to the right.
    void Walking()
    {
        // step 3
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            WalkToTheLeft();
            // get the animation state for walking
            vasaAnim.SetBool("Walking", true);
            move2D.MoveCamera = true;          
            move2D.MoveMainCamera();

        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // walk to the right           
            WalkToTheRight();
            vasaAnim.SetBool("Walking", true);
            move2D.MoveCamera = true;
            move2D.MoveMainCamera();
        }
        else
        {
            //stand still
            vasaAnim.SetBool("Walking", false);
            // get idle
                move2D.MoveCamera = false;
        }
        
    }
    /// <summary>
    /// Function for walking to the left
    /// </summary>
    private void WalkToTheLeft()
    {       
        // check how the character is facing and scale negatively if facing wrong direaction
        if (this.transform.localScale.x < 0)
        {
            newScale = transform.localScale;// get the current over all scale
            newScale.x *= -1;// multiply x value with -1
            this.gameObject.transform.localScale = newScale;// assign the new scale to the old scale
        }
        // move neagtively along the X axis
        this.transform.Translate(-walkingSpeed * Time.deltaTime, 0, 0);
        
        // step 4       
    }
    /// <summary>
    /// Function to walk to the right
    /// </summary>
    private void WalkToTheRight()
    {
        if (this.transform.localScale.x > 0)
        {
            newScale = transform.localScale;// same as in function above
            newScale.x *= -1;// flip horisontally, eventually this will have to be changed
            this.gameObject.transform.localScale = newScale;
        }
        this.transform.Translate(walkingSpeed * Time.deltaTime, 0, 0);       
    }
	void Update ()
    {
        Walking();
	}
}
