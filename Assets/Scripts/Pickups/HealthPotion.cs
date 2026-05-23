using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthPotion : MonoBehaviour, ICollectible
{
    public int heathToRestore;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(heathToRestore);
        Destroy(gameObject);
    }
}
