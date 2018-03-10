using UnityEngine;
using System.Collections;

public class MoveVasa : MonoBehaviour {
    float y;// movement x direction
    public float z;// movement z direction
    public float rota;// rotation
    float fall;
   
    public float movementSpeed;// movement speed
    public float turnSpeed;// speed for turning gustav vasa around
    public float SkiingSpeed;
    public float SkiTurnSpeed;
    public float SkiTurnDrag = 1;
    public bool IsSkiing;
    
    Vector3 movement;// vector for movement
    Rigidbody player;
    CapsuleCollider skin;
    TrailRenderer trail;
    bool IsGrounded;
    
    Vector3 oldRot;
    Quaternion target;
    bool klicked = false;

    //test variabler
    public Vector3 testAngel;
    public Vector3 teatOfV;
    // Use this for initialization
    void Start () {
        skin = GetComponent<CapsuleCollider>();
        player = GetComponent<Rigidbody>();// get the players transform component
        trail = GetComponent<TrailRenderer>();
        
    }
	
	// Update is called once per frame
	void Update () {

        z = Input.GetAxis("Vertical");// get  input
        y = Input.GetAxis("Horizontal");// get input
        
    }
    void FixedUpdate()
    {
        FixtZRotation();

        InputDelay("Fire2", 2.5f);
        
        if (IsSkiing && IsGrounded)
        {
           
            player.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.None;
           
            skin.direction = 2;
            skin.center= new Vector3(0,0.3f,0);

            trail.enabled = true;

            Skiing();
            SkiVelocityChange();

        }
        else if (IsGrounded)
        {
            player.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            player.useGravity = false;
            skin.direction = 1;
            skin.center = new Vector3(0, 0.8f, 0);
            trail.enabled = false;

            Walking();
            Spining();
        }

        else
        {
            player.useGravity = true;
            Spining();
        }

        teatOfV = player.transform.InverseTransformDirection(player.velocity) ;

       
        
        
        //player.AddTorque(Vector3.up* x*1000, ForceMode.VelocityChange);
    }
    void Walking()
    {
        player.velocity = (player.transform.TransformDirection(Vector3.forward) * z * movementSpeed);// updated movement over time
        
    }
    void Spining()
    {
        rota = player.rotation.eulerAngles.y + y * turnSpeed;// rotation equal to rotation + x-axis*turnspeed
        Quaternion target = Quaternion.Euler(0, rota, 0);//set rotation
        player.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 30);//update rotation
    }
    void Skiing()
    {
        player.AddForce(transform.forward * z * movementSpeed, ForceMode.Acceleration);
        player.AddTorque(Vector3.up * y * SkiTurnSpeed, ForceMode.VelocityChange);
    }
    void SkiVelocityChange()
    {

        if ( player.transform.InverseTransformDirection(player.velocity).z < -0.5)
        {
            player.AddForce(player.transform.TransformDirection(Vector3.back).normalized - player.velocity.normalized, ForceMode.VelocityChange);
        }
        else if(player.transform.InverseTransformDirection(player.velocity).z > 0.5)
        {
            player.AddForce(player.transform.TransformDirection(Vector3.forward).normalized - player.velocity.normalized, ForceMode.VelocityChange);
        }
        else
        {
           
        }
       
    }
    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, skin.bounds.extents.y + 0.05f);
    }
    void FixtZRotation()
    {
        Vector3 oldRot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(oldRot.x, oldRot.y, 0);
    }

    void OnCollisionStay(Collision collisionInfo) {
        IsGrounded = true;
        testAngel = collisionInfo.contacts[0].normal;
    }

    void OnCollisionExit(Collision collisionInfo) {
        IsGrounded = false;
    }
    void InputDelay(string button, float sec )
    {
        if (Input.GetButton(button))
        {

            if (IsSkiing && !klicked)
            {
                IsSkiing = false;
                klicked = true;
            }
            else if (!klicked)
            {
                IsSkiing = true;
                klicked = true;
            }
            StartCoroutine(Reset(sec));
        }
    }
    IEnumerator Reset(float sec)
    {
        yield return new WaitForSeconds(sec);
        klicked = false; 
    }
}
