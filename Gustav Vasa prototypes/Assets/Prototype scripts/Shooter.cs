using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    // Use this for initialization
    public GameObject arrow;
    public int force;
    float timer = 0;
    public float shootInteval;
    GameObject sudo;
    void Start()
    {
        sudo = new GameObject();
       
    }
    public void Shoot(Vector3 dir)
    {
        Debug.DrawRay(sudo.transform.position, sudo.transform.TransformDirection(Vector3.up),Color.red);
        if (timer>= shootInteval) { 
        GameObject clone;
            sudo.transform.position = this.transform.position + Vector3.up;
            sudo.transform.LookAt(dir);
            sudo.transform.Rotate(90, 0, 0);
        clone = Instantiate(arrow, this.transform.TransformPoint(Vector3.forward + Vector3.up), sudo.transform.rotation);
        clone.GetComponent<Rigidbody>().AddForce(sudo.transform.TransformDirection(Vector3.up) * force);

            timer = 0;
        }
        timer += Time.fixedDeltaTime;
        
    }

    
    }
