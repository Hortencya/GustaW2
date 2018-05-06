using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

    Rigidbody player;
    float movementSpeed;
    float SkiTurnSpeed;
    float orgDrag;
    bool isMoving;
    Transform sudo;
    GameObject targetPlayer;

	// Use this for initialization
	void Start () {
        player.GetComponent < Rigidbody > ();
        orgDrag = player.drag;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void Skiing()
    {
        if (isMoving)
        {
            sudo.position = player.position;
            player.AddForce(transform.forward * movementSpeed, ForceMode.Acceleration);
            sudo.LookAt(targetPlayer.transform);

            player.rotation = Quaternion.Slerp(transform.rotation, sudo.rotation, Time.deltaTime * 30);
        }
    }
    void SkiVelocityChange()
    {

        if (player.transform.InverseTransformDirection(player.velocity).z < -0.4)// bakvord sliding
        {
            player.drag = orgDrag;
            player.AddForce(player.transform.TransformDirection(Vector3.back).normalized - player.velocity.normalized, ForceMode.VelocityChange);
        }
        else if (player.transform.InverseTransformDirection(player.velocity).z > 0.4)// forvord sliding
        {
            player.drag = orgDrag;
            player.AddForce(player.transform.TransformDirection(Vector3.forward).normalized - player.velocity.normalized, ForceMode.VelocityChange);
        }
        else if ((player.transform.eulerAngles.x > 350 || player.transform.eulerAngles.x < 10) && !isMoving)//  no sliding when purpendikeler on slop
        {

            player.drag = 100;
        }
        else // other sliding
        {
            player.drag = orgDrag;
        }

    }
}
