using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
/// <summary>
/// Playerscript containing the main information about the player such as
/// health/ a method to take damage as well as the awareness of when certain events happen(combat)
/// and check for colliders// switch scene/ finding new part of level in levels where the level consists
/// of several parts the player are seemlessly transported between(if that becomes nesessary)
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField]
    private float health;// depleete when hitting stone colliders or threes.
    [SerializeField]
    private bool hasTakenDamage;
    private Rigidbody rig; // rigidbody for speed registration used to check how fast vasa collides with rocks/firs
    /// <summary>
    /// Start function that tells the system what rigidbody rig is. Eventually other things may be set up here as well
    /// </summary>
    public void Start()
    {
        rig = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Method for awareness of colliders in the scene, colliders may involve the goalpost at the end
    /// of each level, the stones and threes, stones have higher damage than threes
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goalpost")
        {
            Debug.Log("scene switch");
            EditorSceneManager.LoadScene(1);//switch to 2dView
        }
        else
            TakeDamage(other.gameObject);        
    }
    /// <summary>
    /// Function that triggers when gustav is exiting a triggerzone and in this version of the game
    /// resets the bool for dealing damage after having waited in a set amount of secounds.
    /// </summary>
    /// <param name="other"></param>
    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.gameObject.tag== "Rock" || other.gameObject.tag == "Fir")
    //    {           
    //        CanTakeDamageAgain();
    //    }
    //}
    /// <summary>
    /// When colliding with a rock or three the players health will deplete with a different amount dependent on what
    /// was collided with.
    /// </summary>
    private void TakeDamage(GameObject game)
    {
        float nHealh;
        string type = game.tag;
        switch (type)
         {
            case "Rock":
                    nHealh = health - DamageAfterVelocity();
                    health = nHealh;
                    Debug.Log("collided with obstacle" + health.ToString());
                CanTakeDamageAgain();
                break;               
            case "Projectile":
                  nHealh = health - 15;
                  health = nHealh;
                  Debug.Log("Got hit" + health.ToString());                   
                  break;
            case "Deathwall":
                nHealh = health - health;
                health = nHealh;
                Debug.Log("Reload My try");
                break;
            }
               
    }
    /// <summary>
    /// Function used to calculate the damage gustav Vasa should take dependent on his speed when colliding with the object
    /// </summary>
    /// <returns></returns>
    private float DamageAfterVelocity()
    {
        float retvalue = 0f;
        if (!hasTakenDamage)
        {
            if (rig.velocity.magnitude < 5)
                retvalue = 0f;
            if (rig.velocity.magnitude > 5 && rig.velocity.magnitude < 15)
                retvalue = 2f;
            if (rig.velocity.magnitude > 15)
                retvalue = 5f;            
        }       
        return retvalue;
    }
    /// <summary>
    /// Wait five secounds before enabling damage
    /// </summary>
    /// <returns></returns>
    private IEnumerator CanTakeDamageAgain()
    {
        yield return new WaitForSeconds(5);
        hasTakenDamage = false;
        
    }
    /// <summary>
    /// Method that advance the levels dependent on which level the player currently are on
    /// a similar method will be found in the 2d cutscene version of Vasa, but in that case 
    /// finding the startposition and 
    /// </summary>
    private void AdvanceLevel()
    {
    }

}
