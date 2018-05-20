using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    Rigidbody body;
    float movementSpeed=5f;
    float SkiTurnSpeed = 0.5f;
    float orgDrag;
    bool isMoving = true;
    [SerializeField]
    Transform sudo;
    [SerializeField]
    Vector3 desiredVelocity;
    GameObject targetPlayer;
    [SerializeField]
    bool IsGrounded;
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start()
    {
        orgDrag = body.drag;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Skiing(Vector3 direction)
    {
        if (isMoving && IsGrounded)
        {
            sudo.position = body.transform.position;
            body.AddForce(transform.forward * movementSpeed, ForceMode.Acceleration);
            sudo.LookAt(body.position + direction);

            body.rotation = Quaternion.Slerp(transform.rotation, sudo.rotation, Time.deltaTime * 30);
        }
        else
        {
            body.AddForce(transform.up* -1, ForceMode.VelocityChange);
        }
        SkiVelocityChange();
    }
    /// <summary>
    /// This method are called when the ai is walking normally this is implemented similar to the above method for skiing
    /// </summary>
    public void Walking(Vector3 direction)
    {
        movementSpeed = 3;
        if (IsGrounded)
        {
            // define our transform
            sudo.position = body.transform.position;
            body.AddForce(transform.forward * movementSpeed);// I dont know if I should include a forcemode yet
            sudo.LookAt(body.position + direction);
            body.rotation = Quaternion.Slerp(transform.rotation, sudo.rotation, Time.deltaTime * 30);
        }
        else
        {
            // if we are not grounded do not move
            body.AddForce(transform.up * -1, ForceMode.VelocityChange);
        }
        
    }
    /// <summary>
    /// Method for stoping movement of enemies
    /// </summary>
    public void Stop()
    {
        sudo.position = body.transform.position;
        body.transform.Translate(Vector3.zero);      
        //keeps the rotation consistent
        body.rotation = Quaternion.Slerp(transform.rotation, sudo.rotation, Time.deltaTime * 30);
    }
    void SkiVelocityChange()
    {

        if (body.transform.InverseTransformDirection(body.velocity).z < -0.4)// bakvord sliding
        {
            body.drag = orgDrag;
            body.AddForce(body.transform.TransformDirection(Vector3.back).normalized - body.velocity.normalized, ForceMode.VelocityChange);
        }
        else if (body.transform.InverseTransformDirection(body.velocity).z > 0.4)// forvord sliding
        {
            body.drag = orgDrag;
            body.AddForce(body.transform.TransformDirection(Vector3.forward).normalized - body.velocity.normalized, ForceMode.VelocityChange);
        }
        else if ((body.transform.eulerAngles.x > 350 || body.transform.eulerAngles.x < 10) && !isMoving)//  no sliding when purpendikeler on slop
        {

            body.drag = 100;
        }
        else // other sliding
        {
            body.drag = orgDrag;
        }

    }
    public void MoveForward(Vector3 direction, bool skiing)// send in bool determining if we are skiing or walking as well as the desired speed
    {
        desiredVelocity = direction;
        if (skiing)
        {
            Skiing(direction);      
        }
        else if (!skiing)
        {
            // calls the walk method
            Walking(direction);
        }
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "flooring")
            IsGrounded = true;

    }
    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "flooring")
            IsGrounded = false;
    }
}