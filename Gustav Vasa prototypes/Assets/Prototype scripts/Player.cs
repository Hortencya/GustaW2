using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    /// <summary>
    /// Method for awareness of colliders in the scene, colliders may involve the goalpost at the end
    /// of each level, the stones and threes, stones have higher damage than threes
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Goalpost")
        {
            Debug.Log("scene switch");
            Application.LoadLevel(1);//switch to 2dView
        }
        else
            TakeDamage(other.gameObject);
    }
    /// <summary>
    /// When colliding with a rock or three the players health will deplete with a different amount dependent on what
    /// was collided with.
    /// </summary>
    private void TakeDamage(GameObject game)
    {
        string type = game.tag;
        switch (type)
        {
            case "Rock":
                health = health - 10;
                break;
            case "Fir":
                health = health - 7;
                break;
            case "Projectile":
                health = health - 15;
                break;
        }
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
