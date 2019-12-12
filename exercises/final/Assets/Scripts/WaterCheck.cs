using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCheck : MonoBehaviour
{

    /*
     * Checks when a type 1 (blue water) Cubi is touching water and disconnects it from the Cubi stack
     * if it is in a stac.
     */

    public PlayerScript player;             // Parent player object
    
    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("water") && !player.selected)
        {
            Debug.Log("Watter Dog");
            player.inWater = true;
            if(player.hooked)
            {
                player.hooked = false;
                player.transform.parent = null;
                if (player.bottom.holding == player)
                {
                    player.bottom.holding = null;
                }
                player.bottom.canJump = true;
                player.bottom = null;
            }
        }
             
    }
}
