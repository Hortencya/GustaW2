using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //Tis script manages all of the resources in the it will have a set of public lists that all other scripts
    // can acess
    //althogh other scripts in general has a quite strict ban towards public variables the game manager does have a set of public lists and variables
    // that are designed to be easily found by other game objects
    public static GameManager managerWasa = null;
    public Camera mainCamera;// reference to the main camera;
    public GameObject[] danishSoldiers;// reference to the enemy object
    public Transform[] wayPointsInScene;// reference to the waypints that marks enemymovement
    public Transform[] critterWaypoints;// waypoints for smaller animals like geese, cows, goats, hens etc. 
    public GameObject instructions;// Refers the ui object
    public GameObject playercharacter;// reference to the player object
    public GameObject holdobject;// reference needed specifically by throwable object
    public Transform cameraRotationTransform;
    public Transform camMaxZoomObj;
    public Vector3 temporaryPos;
    public bool callenemy;

// I know that throwableObjects and Hideable objects is lacking in this class but the reason for that is that these types of objects never are
// assigned in the inspector but through runtime functions, the player is refering differnet instances of these each time interacting with them
// In shorter words, runtime generated objects are not handled by gamemanager but by their respective scripts and functions
	void Awake ()
    {
        // calls a private initialization method that check if managerWasa is null
        InitializationOnAwake();
        DontDestroyOnLoad(gameObject);
        
	}
    // property för camera
    public Camera GetCamera
    {
        get { return mainCamera; }
    }
    private void InitializationOnAwake()
    {
        if(managerWasa == null)
        {
            managerWasa = this;
        }
        else if (managerWasa != this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Method that can be used by soliderobjects to randomize where they go in the scene
    /// </summary>
    public Transform[] RandomizeWayPoints()
    {
        //What I want to do here is to randomize a list of waypoints based on the list of waypoints in the scene
        Transform[] enemySpecifikWaypoints = new Transform[wayPointsInScene.Length];// works
        List<int> existing = new List<int>();//list that contins indexes already used by the program
        for (int iteration = 0; iteration <= wayPointsInScene.Length-1; iteration++)//for loop iteration that randomizes the enemies waypoint
        {
            int index = Random.Range(0, wayPointsInScene.Length); //+1 is for acessing the fourth waypoint that is otherwise never reached
            if(existing.Count == 0)// if our list of existing waypoints are empty
            {
                existing.Add(index);//add index to the existing
                enemySpecifikWaypoints[iteration] = wayPointsInScene[index];// add the item found at index in Waypoints in scene to the enemyspecifik index of iteration
            }
            else// if list is not empty
            {
                if (existing.Contains(index))// check if index is already iterated once
                {
                     index = Random.Range(0, wayPointsInScene.Length);// if so reiterate waypoint
                }
                enemySpecifikWaypoints[iteration] = wayPointsInScene[index];//when an uniquie element is found it is added to the enemies list of objects 
                existing.Add(index); //add index to the list of existing items               
            }          
            
        }
        return enemySpecifikWaypoints;

    }
    /// <summary>
    /// test out boolean that calls distraction function
    /// </summary>
    public void SetDistractedBool()
    {
        if (!callenemy)
            callenemy = true;
        else
            callenemy = false;
    }
	
}
