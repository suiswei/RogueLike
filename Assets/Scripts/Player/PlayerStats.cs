using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;


    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;

    #region Current Stats Properties

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if(currentHealth != value)
            {
                currentHealth = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if(currentRecovery != value)
            {
                currentRecovery = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if(currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if(currentMight != value)
            {
                currentMight = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if(currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if(currentMagnet != value)
            {
                currentMagnet = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
            }
        }
    }

    #endregion

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

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public Text levelText;

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
        CurrentHealth          = maxHealth;
        CurrentRecovery        = recovery;
        CurrentMoveSpeed       = moveSpeed;
        CurrentMight           = might;
        CurrentProjectileSpeed = projectileSpeed;
        CurrentMagnet          = magnet;

        SpawnWeapon(characterData.StartingWeapon);
        //SpawnWeapon(secondWeaponTest);
        //SpawnPassiveItem(firstPassiveItemtest);
        SpawnPassiveItem(secondPassiveItemtest);
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

        GameManager.instance.currentHealthDisplay.text = "Health: " + CurrentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + CurrentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + CurrentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + CurrentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + CurrentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + CurrentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();

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
        UpdateExpBar();
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

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
        }
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "LVL " + level.ToString();
    }

    public void TakeDamage(float dmg)
    {
        if(!isInvincible)
        {
            CurrentHealth -= dmg;
            
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
        }

        if (currentHealth <= 0)
        {
            Kill();
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = CurrentHealth / characterData.MaxHealth;
    }

    public void Kill()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponSlots, inventory.passiveItemSlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;
            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
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


