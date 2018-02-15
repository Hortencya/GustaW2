using UnityEngine;
using System.Collections;

public class MoveVasa : MonoBehaviour {
    float x;
    float z;
    float rota;
    public float movementSpeed;
    public float turnSpeed;

    Vector3 movement;
    Transform player;
    // Use this for initialization
    void Start () {
        player = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {

        z = Input.GetAxis("Vertical");
        x = Input.GetAxis("Horizontal");
        rota = rota + x *turnSpeed;
        movement.Set(0, 0, z * movementSpeed);
        player.Translate(movement.x  * Time.deltaTime, 0, movement.z * Time.deltaTime);
        Quaternion target =  Quaternion.Euler(0, rota, 0);
        player.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime* 30 );
    }
}
