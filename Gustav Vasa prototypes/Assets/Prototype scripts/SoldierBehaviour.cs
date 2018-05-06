﻿using System;
using UnityEngine;
using System.Collections;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class SoldierBehaviour : MonoBehaviour
    {
        
        public NavMeshAgent agent;
        public ThirdPersonCharacter character;

        public enum State
        {
            PATROL,
            CHASE,
            INVESTIGATE,
            DISTRACTED,
        }
        public State state;
        private bool alive;
        
        // Variables for patrolling
        public Transform[] waypoints;
        [SerializeField]       
        private int waypointInd = 0;
        [SerializeField]
        private float patrolspeed = 0.5f;

        // Variables for chasing
        [SerializeField]
        private float chasespeed = 1f;

        // Variables for investigate
        private Vector3 investigateSpot;
        private float timer = 0;
        public float investigateWait = 10;

        // Variables for sight
        [SerializeField]
        private float heightMultiplier;
        [SerializeField]
        private float sightDist = 10;

        //variables for distracted
        private Vector3 pointOfDistraction;
        bool canHear = true;// bool for the enemy to hear, true for debug purposes

       private  void Awake()
        {
            
        }
        void Start()
        {
            //assign the references for the agents and character scripts
            
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();// this can be changed to other script when we have a more specific movement script created
            //allow navmesh agent to update movement and rotation
            // function that generates random ways for ai to walk on
            waypoints = GameManager.managerWasa.RandomizeWayPoints(); //has to work better 
            agent.updatePosition = true;
            agent.updateRotation = false;
            // set inital state to patrol= danish enemies goes between points
            state = SoldierBehaviour.State.PATROL;
            // ai is alivve
            alive = true;
            heightMultiplier = 1.36f;

        }
        //private void GenerateRandomWaypoints()
        //{
        //    // this method uses a for loop to generate 
        //    waypoints = new Transform[GameManager.managerWasa.wayPointsInScene.Length];
                        
        //    for(waypointInd = 0; waypointInd>=GameManager.managerWasa.wayPointsInScene.Length; waypointInd++)
        //    {
        //        if (waypoints[waypointInd] == null)
        //        {
        //            // lets see how this one works but there might be aneed for another checker that compares the randomized waypoint with the 
        //            // elements already in the list preenting two elements to be the same
        //            int randomIndex = UnityEngine.Random.Range(0, GameManager.managerWasa.wayPointsInScene.Length - 1); 
        //            // by some reason it needed to be clarified that i want to use Unitys random function rather than C# basic system.random
        //            waypoints[waypointInd] = GameManager.managerWasa.wayPointsInScene[randomIndex];
        //        }
        //    }
        //}
        IEnumerator FSM()
        {
            //statemachine defining the different states the ai goes through
            while (alive)
            {
                switch (state)
                {
                    case State.PATROL:
                        // calls method for patroling
                        Patrol();
                        break;
                    case State.CHASE:
                        // calls methd for chasing the player
                        Chase();
                        break;
                        // calls method for investigating a specific spot
                    case State.INVESTIGATE:
                        Investigate();
                        break;
                    case State.DISTRACTED:
                        // calls a method that make ai character walk in direction towards a distraction point
                        Distraction();
                        break;
                       
                }
                yield return null;
            }
        }

        void Patrol()
        {
            agent.speed = patrolspeed;
            if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 2)
            {
                agent.SetDestination(waypoints[waypointInd].transform.position);
                character.Move(agent.desiredVelocity, false, false);
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
                character.Move(Vector3.zero,false,false);
            }
        }

        void Chase()
        {
            agent.speed = chasespeed;
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            character.Move(agent.desiredVelocity, false, false);
        }

        void Investigate()
        {

            timer += Time.deltaTime;
            RaycastHit hit;

            agent.SetDestination(this.transform.position);
            character.Move(Vector3.zero, false, false);
            transform.LookAt(investigateSpot);
            if (timer <= investigateWait)
            {
                state = SoldierBehaviour.State.PATROL;
                timer = 0;
            }
            // debuggers
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * sightDist, Color.green);
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward+transform.right).normalized * sightDist, Color.green);
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward-transform.right).normalized * sightDist, Color.green);
            // rays
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, sightDist))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = SoldierBehaviour.State.CHASE;                   
                }
            }
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward+transform.right).normalized, out hit, sightDist))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = SoldierBehaviour.State.CHASE;
                }
            }
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward-transform.right).normalized, out hit, sightDist))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = SoldierBehaviour.State.CHASE;
                }
            }

           
        }
        /// <summary>
        /// distraction method that calls the enemies in the scene to a specific positions
        /// </summary>
        void Distraction()
        {
            if (canHear)
            {// change to run speed               
                agent.speed = chasespeed;                
                // set the alerting throwable object or other alert object to the navmesh agents destination
                pointOfDistraction = GameManager.managerWasa.temporaryPos;
                agent.SetDestination(pointOfDistraction);
                character.Move(agent.desiredVelocity, false, false);           
            }

            else
            {
                state = SoldierBehaviour.State.PATROL;
                Debug.Log("sent patroling");
            }
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
                state = SoldierBehaviour.State.INVESTIGATE;
                investigateSpot = coll.gameObject.transform.position;             
               
            }
        
        }

        void Update()
        {
            StartCoroutine("FSM");
            Debug.Log(state);       
        }
        
    }
}

