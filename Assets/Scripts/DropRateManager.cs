using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return; // ✅ prevents spawn during scene cleanup

        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate); // ✅ collect eligible drops instead of spawning immediately
            }
        }
        
        if (possibleDrops.Count > 0)
        {
            Drops selectedDrop = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(selectedDrop.itemPrefab, transform.position, Quaternion.identity); // ✅ spawn only one
        }
    }
}
