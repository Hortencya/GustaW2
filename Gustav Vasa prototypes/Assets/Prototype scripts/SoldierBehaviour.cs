using System;
using UnityEngine;
using System.Collections;

    public class SoldierBehaviour : MonoBehaviour
{
    public GameObject pathFinding;
    PathGrid grid;
    Journey myJourney;   
    public UnityEngine.AI.NavMeshAgent agent;
    [SerializeField]
    private bool activeSkiing;//this is a bool to determine if the ai should use skiing or not.
    public EnemyMove enemy;
    Shooter shooter;
    bool haveShoot = false;
        public enum State
        {
            PATROL,
            CHASE,
            DISTRACTED,
            ATTACK
        }
        public State state;
        private bool alive;
        [SerializeField]
        private float life;
        
        // Variables for patrolling
        public Transform[] waypoints;
        [SerializeField]       
        private int waypointInd = 0;

        [SerializeField]
        private string mystate;
        // Variables for investigate
        private Vector3 investigateSpot;
        

        // Variables for sight
        [SerializeField]
        private float heightMultiplier;
        [SerializeField]
        private float sightDist = 10;

        //variables for distracted
        private Vector3 pointOfDistraction;
        bool canHear;// bool for the enemy to hear
    

       private  void Awake()
        {
        canHear = false;
        }
        void Start()
        {
        Invoke("LateStart", 0.0001f);
            //assign the references for the agents and character scripts           
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            //character = GetComponent<ThirdPersonCharacter>();// this can be changed to other script when we have a more specific movement script created
            enemy = GetComponent<EnemyMove>();// get the movement script ref
            shooter = GetComponent<Shooter>();
            //allow navmesh agent to update movement and rotation
            // function that generates random ways for ai to walk on
            waypoints = GameManager.managerWasa.RandomizeWayPoints(); //has to work better 
            agent.updatePosition = false;
            agent.updateRotation = false;
            // set inital state to patrol= danish enemies goes between points
            state = SoldierBehaviour.State.CHASE;
            // ai is alivve
            alive = true;
            heightMultiplier = 0.36f;

        grid = pathFinding.GetComponent<PathGrid>();
        }
    void LateStart()
    {
        foreach (var journey in grid.journeys)
        {
            if (journey.OwnedBy(transform))
            {
                myJourney = journey;
            }
        }
    }
    //general update once per frame
    void FixedUpdate()
    {
        //if (myJourney == null && grid.journeys!=null)
        //{
        //    foreach (var journey in grid.journeys)
        //    {
        //        if (journey.OwnedBy(transform))
        //        {
        //            myJourney = journey;
        //        }
        //    }
        //}
        StartCoroutine("FSM");        
        agent.nextPosition = transform.position;
    }
   
    IEnumerator FSM()
        {
            //statemachine defining the different states the ai goes through
            if (alive)
            {
                switch (state)
                {
                    case State.PATROL:
                        // calls method for patroling
                       // Patrol();
                        break;
                    case State.CHASE:
                    // calls methd for chasing the player
                    Chase();
                        break;                                        
                    case State.DISTRACTED:
                    // calls a method that make ai character walk in direction towards a distraction point
                    DetermineHearingRange(GameManager.managerWasa.temporaryPos);
                    Distraction();
                        break;
                case State.ATTACK:
                    Attacking();
                    break;
                       
                }
                yield return null;
            }
        }

        void Patrol()
        {
            //agent.speed = patrolspeed;
            if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 2)
            {
                agent.SetDestination(waypoints[waypointInd].transform.position);
                //character.Move(agent.desiredVelocity, false, false);

                // if statement aroud the below determining if we are skiing or not
                if (activeSkiing)
                    enemy.MoveForward(agent.desiredVelocity, true);//skiing movement test
                else
                    enemy.MoveForward(agent.desiredVelocity, false);
            }
            else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
            {
                waypointInd += 1;
                if (waypointInd >= waypoints.Length)
                {
                    waypointInd = 0;
                }
            }
            else
            {
                //character.Move(Vector3.zero,false,false);
            }
        }
   
    void Chase()
    {
       
        //timer += Time.deltaTime;
        //if(timer <= investigateWait)

        //agent.SetDestination(this.transform.position);
        if (myJourney!=null && myJourney.path != null)if(myJourney.path.Count>0) { 
        investigateSpot = myJourney.path[0].worldPos;
                //transform.LookAt(investigateSpot);
                if (Vector3.Distance(myJourney.target.position, this.transform.position) < 10)
                {
                    state = SoldierBehaviour.State.ATTACK;
                }
            }
            //Debug.Log("hey! You");                
            //agent.speed = chasespeed;
            //agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        //character.Move(agent.desiredVelocity, false, false);
        if (activeSkiing)
            enemy.MoveForward(investigateSpot, true);
        else
            enemy.MoveForward(investigateSpot, false);

        
        
              
    }


    /// <summary>
    /// function for attacking Gustav Vasa
    /// </summary>
    private void Attacking()
    {
        //Debug.DrawRay(this.transform.position, this.transform.InverseTransformPoint( myJourney.target.position));
        
        enemy.Stop(myJourney.target.position);

        shooter.Shoot(myJourney.target.GetChild(5).position);
        if(!haveShoot) StartCoroutine(backToWork());
        haveShoot = true;
    
    }
    private IEnumerator backToWork()
    {
        yield return new WaitForSeconds(shooter.shootInteval);
        state = SoldierBehaviour.State.CHASE;
        haveShoot = false;
    }

        //stop navmesh agent
        // get player health 
        // determine if hit
        // if the attack does hit remove health from gustav vasa 

        //void Investigate()
        //{

        //    timer += Time.deltaTime;
        //    RaycastHit hit;

        //    agent.SetDestination(this.transform.position);
        //    //character.Move(Vector3.zero, false, false);
        //    transform.LookAt(investigateSpot);
        //    if (timer <= investigateWait)
        //    {
        //        state = SoldierBehaviour.State.PATROL;
        //        timer = 0;
        //    }
        //    // debuggers
        //    Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * sightDist, Color.green);
        //    Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward+transform.right).normalized * sightDist, Color.green);
        //    Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward-transform.right).normalized * sightDist, Color.green);
        //    // rays
        //    if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, sightDist))
        //    {
        //    Debug.Log(hit.collider.gameObject.tag);
        //        if (hit.collider.gameObject.tag == "Player")
        //        {
        //        Debug.Log("fgiu");
        //            state = SoldierBehaviour.State.CHASE;                   
        //        }
        //    }
        //    if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward+transform.right).normalized, out hit, sightDist))
        //    {
        //        if (hit.collider.gameObject.tag == "Player")
        //        {
        //            state = SoldierBehaviour.State.CHASE;
        //        }
        //    }
        //    if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward-transform.right).normalized, out hit, sightDist))
        //    {
        //        if (hit.collider.gameObject.tag == "Player")
        //        {
        //            state = SoldierBehaviour.State.CHASE;
        //        }
        //    }


        //}
        /// <summary>
        /// hearing range for ai enemy. Uses a vector 3. distance to determine how far away the ai is from the point of distraction
        /// </summary>
        /// <param name="pointofHearing"></param>
    private void DetermineHearingRange(Vector3 pointofHearing)
        {
        if (Vector3.Distance(transform.position, pointofHearing) <= 25)
        {
            canHear = true;
        }
        else
            canHear = false;
        }
        /// <summary>
        /// distraction method that calls the enemies in the scene to a specific positions
        /// </summary>
       void Distraction()
       {
        // determine if the ai can hear
            if (canHear)
            {// change to run speed               
                pointOfDistraction = GameManager.managerWasa.temporaryPos;
            //Move towards the point of distraction while more than 2 units away from it
                if(Vector3.Distance(this.transform.position, pointOfDistraction) >= 2)
                { 
                    agent.SetDestination(pointOfDistraction);

                    if (activeSkiing)// distractions may happen while skiing such as threes falling or Vasa throwing some equipment to the side beside him/ vasa traverse nearby a station of danish soliders
                        enemy.MoveForward(agent.desiredVelocity, true);
                    else
                        enemy.MoveForward(agent.desiredVelocity, false);
             }
                // to omitt orbit reaktion when the goal is reached that will otherwise make the ai circle arount the destination while searching a new path
                // i decided to stop them for a shorter period of time before going back to patrolling
            else if (Vector3.Distance(this.transform.position, pointOfDistraction) <= 2)
            {                
                // we stop the enemy when they have reached their point of distraction
                agent.SetDestination(this.transform.position);              
                //enemy.Stop();
                //transform.LookAt(pointOfDistraction);
                //call a yield return wait for secounds before reseting to patrolling
                StartCoroutine(ReactWait());                                         
           }
       }

            else
            {
                state = SoldierBehaviour.State.PATROL;
                
            }
        }
    /// <summary>
    /// Ienumerator used to make enemies paus between different actions such as being distracted or searhing an area and returning to the patrol bussiness
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReactWait()
    {
        yield return new WaitForSeconds(4);     
        state = SoldierBehaviour.State.PATROL;
        canHear = false;
    }
        /// <summary>
        /// Method that set the distraction state to be active it is called by other methods
        /// </summary>
        public void SetDistractionState()
        {
            if (GameManager.managerWasa.callenemy)
            {
                state = SoldierBehaviour.State.DISTRACTED;
                Debug.Log(state);
            }

         }
            void OnTriggerStay(Collider coll)
        {
            if (coll.tag == "Player")
            {               
                state = SoldierBehaviour.State.CHASE;
                //investigateSpot = coll.gameObject.transform.position;                            
            }
        
        }

        
        
}


