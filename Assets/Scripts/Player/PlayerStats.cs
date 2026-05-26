using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    // [HideInInspector]
    public float currentHealth;
    // [HideInInspector]
    public float currentRecovery;
    // [HideInInspector]
    public float currentMoveSpeed;
    // [HideInInspector]
    public float currentMight;
    // [HideInInspector]
    public float currentProjectileSpeed;
    // [HideInInspector]
    public float currentMagnet;

    public List<GameObject> spawnedWeapons;

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemtest, secondPassiveItemtest;

    void Awake()
    {
        // 1. Get the data first
        characterData = CharacterSelector.GetData();

        // 2. Cache ALL values locally before destroying the singleton
        float maxHealth        = characterData.MaxHealth;
        float recovery         = characterData.Recovery;
        float moveSpeed        = characterData.MoveSpeed;
        float might            = characterData.Might;
        float projectileSpeed  = characterData.ProjectileSpeed;
        float magnet           = characterData.Magnet;

        // 3. NOW destroy the singleton
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();   

        // 4. Apply cached values — no dependency on singleton anymore
        currentHealth          = maxHealth;
        currentRecovery        = recovery;
        currentMoveSpeed       = moveSpeed;
        currentMight           = might;
        currentProjectileSpeed = projectileSpeed;
        currentMagnet          = magnet;

        SpawnWeapon(characterData.StartingWeapon);
        SpawnWeapon(secondWeaponTest);
        SpawnPassiveItem(firstPassiveItemtest);
        SpawnPassiveItem(secondPassiveItemtest);
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update()
    {
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else
        {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }

    public void TakeDamage(float dmg)
    {
        if(!isInvincible)
        {
            currentHealth -= dmg;
            
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
        }

        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Debug.Log("Player has died.");
    }

    public void RestoreHealth(float amount)
    {
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;
            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;
            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if(weaponIndex >= inventory.weaponSlots.Count)
        {
            Debug.LogWarning("No more weapon slots available!");
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

        public void SpawnPassiveItem(GameObject passiveItem)
    {
        if(passiveItemIndex >= inventory.passiveItemSlots.Count)
        {
            Debug.LogWarning("No more passive item slots available!");
            return;
        }

        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }
}


