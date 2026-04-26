using UnityEngine;
using System.Collections.Generic; // Added this so List<T> works

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    void Start()
    {
        // Call it once at the start of the game
        SpawnProps();
    }

    void SpawnProps()
    {
        // Safety check to make sure you assigned things in the inspector
        if (propPrefabs.Count == 0 || propSpawnPoints.Count == 0) return;

        foreach (GameObject sp in propSpawnPoints)
        {
            int rand = Random.Range(0, propPrefabs.Count);
            
            // Spawn the prop and parent it to the spawn point to keep the Hierarchy clean
            Instantiate(propPrefabs[rand], sp.transform.position, Quaternion.identity, sp.transform);
        }
    }
}
