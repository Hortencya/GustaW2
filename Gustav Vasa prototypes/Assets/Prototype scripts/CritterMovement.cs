using UnityEngine;
using System.Collections;
/// <summary>
/// This script handles the movements of all non direct enemy related characters such as for instance geese or crowds of people
/// that needs to move in one single specific way and at a syncronized pace.
/// </summary>
public class CritterMovement : MonoBehaviour {
    
    // these are the two variables that are needed for moving things like crowds of people or geese forwards
    private Rigidbody critterMass;// first we have the rigidbody used to translate their position
    private Transform pseudo;// then we have a transform point to use for translation of movemen
    private Vector3 direction;
    private bool isGrounded;// checks if the ai agent has contact with the floor
    /// <summary>
    /// Awake is called on instancaiation of program
    /// </summary>
    public void Start()
    {
        critterMass = GetComponent<Rigidbody>();// find the rigidbody component in the goose or peasant
    }
    /// <summary>
    /// function for walking moves the character forward
    /// </summary>
    /// <param name="movementSpeed"></param>
    /// <param name="direction"></param>
    public void Walk(Vector3 desiredVel,float movementSpeed)
    {
        direction = desiredVel;
        if(isGrounded)
        {
            pseudo.position = critterMass.transform.position;// set the position of the hypthetical transform to the same as the rigidbody
            critterMass.AddForce(transform.forward * movementSpeed, ForceMode.Acceleration);
            pseudo.LookAt(critterMass.transform.position + direction);
            // rotation
            critterMass.rotation = Quaternion.Slerp(transform.rotation, pseudo.rotation, Time.deltaTime * 15);// the critters do rotate slower than the enemies
        }
        else
        {
            critterMass.AddForce(transform.up * -1, ForceMode.VelocityChange);// stop all movements
        }
    }
    /// <summary>
    /// Stops agents from moving and rotating, can be used when distracted, seeing the player or being persued by guards.
    /// </summary>
    public void Stop()
    {
        critterMass.AddForce(transform.up * -1, ForceMode.VelocityChange);
    }
    /// <summary>
    /// collision detection for the ai they check for the ground/ terrain object. as long as the objects collided with is tagged as flooring the is grounded remains true.
    /// </summary>
    /// <param name="information"></param>
    private void OnCollisionStay(Collision information)
    {
        if (information.gameObject.tag == "flooring")
            isGrounded = true;
        else
            isGrounded = false;
    }
}
