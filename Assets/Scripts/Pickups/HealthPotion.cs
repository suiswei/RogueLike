using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthPotion : Pickup
{
    public int heathToRestore;

    public override void Collect()
    {
        if(hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(heathToRestore);
    }
}
