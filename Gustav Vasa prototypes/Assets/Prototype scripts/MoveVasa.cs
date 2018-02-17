using UnityEngine;
using System.Collections;

public class MoveVasa : MonoBehaviour {
    float x;// movement x direction
    float z;// movement z direction
    float rota;// rotation
    public float movementSpeed;// movement speed
    public float turnSpeed;// speed for turning gustav vasa around

    Vector3 movement;// vector for movement
    Transform player;
    // Use this for initialization
    void Start () {
       
        player = GetComponent<Transform>();// get the players transform component
    }
	
	// Update is called once per frame
	void Update () {

        z = Input.GetAxis("Vertical");// get  input
        x = Input.GetAxis("Horizontal");// get input
        rota = rota + x *turnSpeed;// rotation equal to rotation + x-axis*turnspeed 
        movement.Set(0, 0, z * movementSpeed);// set movement  speed
        player.Translate(movement.x  * Time.deltaTime, 0, movement.z * Time.deltaTime);// updated movement over time
        Quaternion target =  Quaternion.Euler(0, rota, 0);//set rotation
        player.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime* 30 );//update rotation
    }
}
