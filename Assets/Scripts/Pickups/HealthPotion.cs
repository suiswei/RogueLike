using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthPotion : Pickup, ICollectible
{
    public int heathToRestore;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(heathToRestore);
    }
}
