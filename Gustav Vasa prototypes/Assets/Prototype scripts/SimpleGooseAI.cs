using UnityEngine;
using System.Collections;
/// <summary>
/// Simple movement and detection script for geese enemies.
/// </summary>
public class SimpleGooseAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent goose;// this moves the goose 
    [SerializeField]
    //private float walkingspeed;// this are used to regulate how fast the geese moves.
    private int waypointIndex = 0; // which waypoint is we on, we always start at 0.
    private GameManager vasaManage; // this helps with getting the references to all of the enemies as well as the players position
    //private CritterMovement movements;// reference to the movement script for critters-Not needed?
    private GeeseState state;
    private bool calledGuardsOnce;
    /// <summary>
    /// states for the geese used to tell them what to do starting out easy 
    /// </summary>
    public enum GeeseState
    {
        PATROL,// walking normally
        HISS// stop turn towards the player and make a noise that make that alerts guards
    }
    void Awake()
    {
        //movements = GetComponent<CritterMovement>();
    }
    void Start()
    {
        vasaManage = GameManager.managerWasa;
        goose = GetComponent<UnityEngine.AI.NavMeshAgent>();        
    }
	//here the states are driven simutaneosly as everything else	
	void FixedUpdate ()
    {
        StartCoroutine(GooseLogic());// starts the ienumerator controlling the geese states.
	}
    /// <summary>
    /// the summmarised logic of the geese ai.
    /// </summary>
    /// <returns></returns>
    IEnumerator GooseLogic()
    {
        switch (state)
        {
            case GeeseState.PATROL:
                Patrol();
                break;
            case GeeseState.HISS:
                Hiss();
                break;
        }
        yield return null;
    }
    IEnumerator WaitBeforeChange()
    {
        yield return new WaitForSeconds(5);
        // more if statements can be added based on what actions is available if we want to have a chase state
        // the geese can be in that too or it can be about to switch to it.
        if (state == GeeseState.HISS)
        {
            state = GeeseState.PATROL;
            calledGuardsOnce = false;
        }
            
    }
    /// <summary>
    /// patrolling geese
    /// </summary>
    private void Patrol()
    {
        // check the distance between the goose and its next waypoint
        if (Vector3.Distance(this.transform.position, vasaManage.critterWaypoints[waypointIndex].transform.position) >= 2)
        {
            goose.SetDestination(vasaManage.critterWaypoints[waypointIndex].transform.position);
            //movements.Walk(goose.desiredVelocity,goose.speed);// walk towards the waypoint of selection
            
        }
        else if(Vector3.Distance(this.transform.position, vasaManage.critterWaypoints[waypointIndex].transform.position) < 2)
        {
            waypointIndex += 1;// increment with one
            if (waypointIndex == vasaManage.critterWaypoints.Length)// if we are aleready on the highest waypoint 
                waypointIndex = 0;// pick waypoint 0 and proceed towards that
        }
    }
    /// <summary>
    /// Hissing geese calls guards to the spot of the guards
    /// </summary>
    private void Hiss()
    {
        goose.SetDestination(Vector3.zero);
        transform.LookAt(vasaManage.playercharacter.transform.position);
        //Make hisssound and animation
        if (!calledGuardsOnce)
        {
            vasaManage.callenemy = true;
            vasaManage.temporaryPos = vasaManage.playercharacter.transform.position;            
            foreach (GameObject o in vasaManage.danishSoldiers)
            {                
                o.GetComponent<SoldierBehaviour>().SetDistractionState();
            }
            calledGuardsOnce = true;
            Debug.Log("hissss");
        }
        StartCoroutine(WaitBeforeChange());// tells ai to wait before it changes back to bussiness as usual       
    }
    /// <summary>
    /// trigger function for the geese
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            state = GeeseState.HISS;
        }
    }


}
