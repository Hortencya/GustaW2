using System;
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
        public GameObject[] waypoints;
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

        void Start()
        {
            //assign the references for the agents and character scripts
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();// this can be changed to other script when we have a more specific movement script created
            //allow navmesh agent to update movement and rotation
            agent.updatePosition = true;
            agent.updateRotation = false;
            // set inital state to patrol= danish enemies goes between points
            state = SoldierBehaviour.State.PATROL;
            // ai is alivve
            alive = true;
            heightMultiplier = 1.36f;

        }
        IEnumerator FSM()
        {
            while (alive)
            {
                switch (state)
                {
                    case State.PATROL:
                        Patrol();
                        break;
                    case State.CHASE:
                        Chase();
                        break;
                    case State.INVESTIGATE:
                        Investigate();
                        break;
                    case State.DISTRACTED:
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
        void Distraction()
        {
            // change to run speed
            // set the alerting Jug or other alert object to the navmesh agents destination

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
        }
        
    }
}

