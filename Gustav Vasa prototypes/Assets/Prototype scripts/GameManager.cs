using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //Tis script manages all of the resources in the it will have a set of public lists that all other scripts
    // can acess
    //althogh other scripts in general has a quite strict ban towards public variables the game manager does have a set of public lists and variables
    // that are designed to be easily found by other game objects
    public static GameManager managerWasa = null;
    public Camera mainCamera;// reference to the main camera;
    public GameObject[] danishSoldiers;// reference to the enemy object
    public Transform[] wayPointsInScene;// reference to the waypints that marks enemymovement
    public GameObject instructions;// Refers the ui object
    public GameObject playercharacter;// reference to the player object
    public GameObject holdobject;// reference needed specifically by throwable object
    public Transform cameraRotationTransform;
    public Transform camMaxZoomObj;

// I know that throwableObjects and Hideable objects is lacking in this class but the reason for that is that these types of objects never are
// assigned in the inspector but through runtime functions, the player is refering differnet instances of these each time interacting with them
// In shorter words, runtime generated objects are not handled by gamemanager but by their respective scripts and functions
	void Awake ()
    {
        // calls a private initialization method that check if managerWasa is null
        InitializationOnAwake();
        DontDestroyOnLoad(gameObject);
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
    
	
	
}
