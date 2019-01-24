using UnityEngine;
using System.Collections;
//namespace UnityStandardAssets.Characters.ThirdPerson{
    public class MoveVasa : MonoBehaviour
    {
        
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
        AnimationManager myAnimations;//reference to animationmanager
        Animator GustavAnim;//animator reference
        //}
        float orgDrag;
        public bool testDrag; // test

    //test variabler

    // Use this for initialization
    void Start()
        {
            // geting componets
            skin = GetComponent<CapsuleCollider>();
            player = GetComponent<Rigidbody>();// get the players transform component              
            //trail = GetComponent<TrailRenderer>();
            camera = GameManager.managerWasa.GetCamera;
            orgDrag = player.drag;
            // deactivates the skiis if that is not already done
            //GameManager.managerWasa.VasaSkiis.SetActive(false);
            GustavAnim = GetComponentInChildren<Animator>();//Get animatorcomponent from player
            myAnimations = GameObject.FindGameObjectWithTag("AnimationManager").GetComponent<AnimationManager>();//later call from the game manager
            myAnimations.SkiingModelActive = true;
    }

        // Update is called once per frame
        void Update()
        {
            z = Input.GetAxis("Vertical");// get  input for movment
            y = Input.GetAxis("Horizontal");// get input for rotation

        }
        void FixedUpdate()
        {
            FixtZRotation();// stops rotation on the Z axis

            InputDelay("Jump", 2.5f); // stops double klicking

            if (IsSkiing && IsGrounded)
            {

                player.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.None; // enable rotation on the X axis
                                                                                                       // lays the capsulecollidern down and use is as skies{
                skin.direction = 2;
                skin.center = new Vector3(0, 0.3f, 0);
                //}

                //trail.enabled = true;

                Skiing();
                SkiVelocityChange();

            }
            //if the player has contact with the ground but skiing is turned off
            else if (IsGrounded)
            {
                player.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // disable rotation on the X axis
                player.useGravity = false;// stops sliding when walking
                                          //stands the capsulecollidern up to use it normaly {
                skin.direction = 1;
                skin.center = new Vector3(0, 0.8f, 0);
                Walking();
                Spining();
        }
        //trail.enabled = false;
        //checking for input via WASD controls as standard for many applications this day
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        //{
        //    
        //    
        //}
        //else
        //    Idle();
              
        else
        {
                player.useGravity = true;
                Spining();
                
            }

        //player.AddTorque(Vector3.up * x * 1000, ForceMode.VelocityChange);
    }
        /// <summary>
        /// non phisikal movment
        /// </summary>
        void Walking()
        {
            // the below line shut of the skiis visual represetation while walking
            GameManager.managerWasa.VasaSkiis.SetActive(false);       
            //player.velocity = (player.transform.TransformDirection(Vector3.forward) * z * movementSpeed);// updated movement over time
            player.velocity = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z) * z * movementSpeed + camera.transform.right * y * movementSpeed;
            player.AddTorque(Vector3.up * y * turnSpeed, ForceMode.VelocityChange);
        //Enables animations             
            myAnimations.IdleCarousel();
          if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
          {
            myAnimations.StartWalkanimation();
            TurnAfterMousePos();
          }
        
        }
    /// <summary>
    /// A method that sets the walkcycle to false right now it only contains one single idle animation
    /// but later on more idles as well as conditions to start them can be added here 
    /// </summary>
    void Idle()
    {
        GameManager.managerWasa.VasaSkiis.SetActive(false);
        //enable animation
        myAnimations.IdleCarousel();
        //put rotation values as with normal walking
        rota = camera.transform.rotation.eulerAngles.y;
        Quaternion target = Quaternion.Euler(0, rota, 0);//set rotation
        player.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 30);//update rotation
    }
    /// <summary>
    /// non physikal rotation
    /// </summary>
    void Spining()
        {
        //rota = player.rotation.eulerAngles.y + y * turnSpeed;// rotation equal to rotation + x-axis*turnspeed
        if (IsSkiing == false)
        {
            rota = camera.transform.rotation.eulerAngles.y;
            Quaternion target = Quaternion.Euler(0, rota, 0);//set rotation
            player.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 30);//update rotation
        }
        else
            player.AddTorque(Vector3.up * y * SkiTurnSpeed, ForceMode.VelocityChange);
        }
        /// <summary>
        /// phisikal movment and rotation
        /// </summary>
        void Skiing()
        {           
            //GameManager.managerWasa.VasaSkiis.SetActive(true);// enables the skiis on Gustav Vasa when he is skiing
            player.AddForce(transform.forward * z * movementSpeed, ForceMode.Acceleration);
            player.AddTorque(Vector3.up * y * SkiTurnSpeed, ForceMode.VelocityChange);           
            if (Input.GetButton("Fire2") == true)
            {
                player.drag = 2;
            }
            else player.drag = orgDrag;
        //animation enabling
        myAnimations.SkiingModelActive = true;
        myAnimations.SkiAnimationCarousel();                
        }
        /// <summary>
        /// alter velosity vektor baset on ski orentation, observe this does only affect speed not rotation
        /// </summary>
        void SkiVelocityChange()
        {
            Vector3 V = player.velocity.normalized;
            if (IsGrounded)

            {
                if (player.transform.InverseTransformDirection(player.velocity).z < -0.4)// bakvord sliding
                {
                    // player.drag = orgDrag;
                    player.AddForce(player.transform.TransformDirection(Vector3.back).normalized - player.velocity.normalized , ForceMode.VelocityChange);
                   
                }
                else if (player.transform.InverseTransformDirection(player.velocity).z > 0.4)// forvord sliding
                {
                    //player.drag = orgDrag;

                    player.AddForce(player.transform.TransformDirection(Vector3.forward).normalized  - player.velocity.normalized , ForceMode.VelocityChange);
                    //player.AddForce(V, ForceMode.Force);
                }

                else if ((player.transform.eulerAngles.x > 350 || player.transform.eulerAngles.x < 10) && z == 0 && Input.GetButton("Fire2"))//  no sliding when purpendikeler on slop
                {

                    player.drag = 100;
                    //if (IsGrounded) { player.velocity = Vector3.zero;}
                }
                else // other sliding
                {
                    player.drag = orgDrag;
                }
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
            if(collisionInfo.gameObject.tag=="flooring")
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
        void InputDelay(string button, float sec)
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
    /// <summary>
    /// method that rotates the charcter after the mouse 
    /// </summary>
    private void TurnAfterMousePos()
    {
        Vector3 playerRot = new Vector3(player.rotation.x, 0, 0);
        Vector2 mousepos = new Vector2(Input.mousePosition.x, 0);
        float newX = playerRot.x + mousepos.x;
        if (newX > -360 && newX < 360)
        {
            player.rotation = Quaternion.Euler(newX, 0, 0);
        }
    }
        //}
    }
//}