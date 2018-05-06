using UnityEngine;
using System.Collections;

public class MoveVasa : MonoBehaviour {
    float y;// rotation y direction input
    float z;// movement z direction input

    float rota;// rotation while walking 
    public float movementSpeed;// movement speed while walking 
    public float turnSpeed;// speed for turning while walking 

    public float SkiingSpeed; // movement speed while skiing
    public float SkiTurnSpeed; // turning speed While skiing
    public bool IsSkiing; // skiing or walking
    bool skiingKlicked = false; // to stop duble klicking IsSkiing
    public bool IsGrounded; // on the gound or faling

    Vector3 fixtRota;// is used to stop Z rotation
    //components{
    Rigidbody player;
    CapsuleCollider skin;
    TrailRenderer trail;
    Camera camera;
    //}
    float orgDrag;




    public bool testDrag; // test

    //test variabler
    
    
    // Use this for initialization
    void Start () {
        
        // geting componets
        skin = GetComponent<CapsuleCollider>();
        player = GetComponent<Rigidbody>();// get the players transform component
        trail = GetComponent<TrailRenderer>();
        camera = GameManager.managerWasa.GetCamera;
        orgDrag = player.drag;
    }
	
	// Update is called once per frame
	void Update () {

        z = Input.GetAxis("Vertical");// get  input for movment
        y = Input.GetAxis("Horizontal");// get input for rotation
        
    }
    void FixedUpdate()
    {
        FixtZRotation();// stops rotation on the Z axis

        InputDelay("Fire2", 2.5f); // stops double klicking
        
        if (IsSkiing && IsGrounded)
        {
           
            player.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.None; // enable rotation on the X axis
           // lays the capsulecollidern down and use is as skies{
            skin.direction = 2;
            skin.center= new Vector3(0,0.3f,0);
            //}

            trail.enabled = true;

            Skiing();
            SkiVelocityChange();

        }
        else if (IsGrounded)
        {
            player.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // disable rotation on the X axis
            player.useGravity = false;// stops sliding when walking
            //stands the capsulecollidern up to use it normaly {
            skin.direction = 1;
            skin.center = new Vector3(0, 0.8f, 0);
            //}
            trail.enabled = false;

            Walking();
            Spining();
        }

        else
        {
            player.useGravity = true;
            Spining();
        }




        
        
        //player.AddTorque(Vector3.up* x*1000, ForceMode.VelocityChange);
    }
    /// <summary>
    /// non phisikal movment
    /// </summary>
    void Walking()
    {
        //player.velocity = (player.transform.TransformDirection(Vector3.forward) * z * movementSpeed);// updated movement over time
        player.velocity = new Vector3( camera.transform.forward.x,0, camera.transform.forward.z) * z * movementSpeed+ camera.transform.right * y * movementSpeed;
    }
    /// <summary>
    /// non phisikal rotation
    /// </summary>
    void Spining()
    {
        //rota = player.rotation.eulerAngles.y + y * turnSpeed;// rotation equal to rotation + x-axis*turnspeed
        rota = camera.transform.rotation.eulerAngles.y;
        Quaternion target = Quaternion.Euler(0, rota, 0);//set rotation

       
        player.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 30);//update rotation
    }
    /// <summary>
    /// phisikal movment and rotation
    /// </summary>
    void Skiing()
    {
        player.AddForce(transform.forward * z * movementSpeed, ForceMode.Acceleration);
        player.AddTorque(Vector3.up * y * SkiTurnSpeed, ForceMode.VelocityChange);
    }
    /// <summary>
    /// alter velosity vektor baset on ski orentation
    /// </summary>
    void SkiVelocityChange()
    {
        
        if ( player.transform.InverseTransformDirection(player.velocity).z < -0.4)// bakvord sliding
        {
            player.drag = orgDrag;
            player.AddForce(player.transform.TransformDirection(Vector3.back).normalized - player.velocity.normalized, ForceMode.VelocityChange);
        }
        else if(player.transform.InverseTransformDirection(player.velocity).z > 0.4)// forvord sliding
        {
            player.drag = orgDrag;
            player.AddForce(player.transform.TransformDirection(Vector3.forward).normalized - player.velocity.normalized, ForceMode.VelocityChange);
        }
        else if((player.transform.eulerAngles.x> 350 || player.transform.eulerAngles.x < 10) && z==0 && testDrag)//  no sliding when purpendikeler on slop
        {
            
            player.drag = 100;
        }
        else // other sliding
        {
            player.drag = orgDrag;
        }
       
    }
    /// <summary>
    /// sheks if chrekter is standing on a surfase
    /// may ned more work
    /// </summary>
    /// <returns></returns>
    /// {
    bool Grounded()// not in use, but might be useful
    {
        return Physics.Raycast(transform.position, Vector3.down, skin.bounds.extents.y + 0.05f);
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        IsGrounded = true;

    }

    void OnCollisionExit(Collision collisionInfo)
    {
        IsGrounded = false;
    }
    //}

    /// <summary>
    /// stops rotation on Z axis
    /// </summary>
    void FixtZRotation()
    {
        fixtRota = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(fixtRota.x, fixtRota.y, 0);
    }

    /// <summary>
    /// stops double kliking
    /// </summary>
    /// <param name="button"></param>
    /// <param name="sec"></param>
    /// {
    void InputDelay(string button, float sec )
    {
        if (Input.GetButton(button))
        {

            if (IsSkiing && !skiingKlicked)
            {
                IsSkiing = false;
                skiingKlicked = true;
            }
            else if (!skiingKlicked)
            {
                IsSkiing = true;
                skiingKlicked = true;
            }
            StartCoroutine(Reset(sec));
        }
    }
    IEnumerator Reset(float sec)
    {
        yield return new WaitForSeconds(sec);
        skiingKlicked = false; 
    }
    //}
}
