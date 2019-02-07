using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    public int timeAlive=10;
	// Use this for initialization
	
    void OnTriggerStay(Collider collision)
    {
        Destroy(this.gameObject,timeAlive);
        if (collision.transform.tag != "Danish" && collision.transform.tag !="Player" )
        {
            //Debug.Log(collision.transform.tag);
          if(collision.transform.tag =="Target") this.transform.parent = collision.transform;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().detectCollisions = false;
        }
        
    }
  
    }
