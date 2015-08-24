using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Entity : MonoBehaviour
{
    protected GameObject canvasGO;
    public GameObject infoPanel;
    public GameObject entityInfoPrefab;

    protected Text nameLabel;
    protected Text healthLabel;
    protected Text readyBarLabel;

    private ObjectPooler pool;

    public float attackTimer = 0.0f;
    public float readyTime = 2.0f;
    public bool isReady = false;

    public float health = 10.0f;
    protected float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        private set { }
    }

    public bool isDead = false;

    public float baseDamage = 2.0f;
    private float startHealth = 10.0f;
    private float startMaxHealth;

    protected string readyText = "Attack in: ";
    public string entityName = "Demon";
    protected GameObject infotab;
    public float expReward = 5.0f;
    public Entity attacker = null; // the 1 who attacked me 

    private string entName;
    
    protected virtual void Awake()
    {
        startHealth = health;
        maxHealth = health;
        startMaxHealth = startHealth;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        createGUI();
        entName = entityName;
        //canvasGO = GameObject.Find("Canvas");
        //infoPanel = canvasGO.transform.FindChild("EnemyInfoPanel").gameObject;
        //infotab = Instantiate(entityInfoPrefab) as GameObject;
        //infotab.name = entityName;
        //infotab.transform.SetParent(infoPanel.transform);
        //infotab.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //readyBarLabel = infotab.transform.FindChild("AttackTimer").GetComponent<Text>();
        //healthLabel = infotab.transform.FindChild("Health").GetComponent<Text>();
        //nameLabel = infotab.transform.FindChild("EntityName").GetComponent<Text>();
        //nameLabel.text = entityName;
        //UpdateHealthLabel();
    }

    public virtual void UpdateTimerLabel()
    {
        readyBarLabel.text = readyText + readyTime.ToString("0.0");
        if (readyTime <= 0.0f)
        {
            readyBarLabel.text = "Ready to attack!";
        }
    }

    public virtual void UpdateHealthLabel()
    {
        healthLabel.text = "Health: " + health.ToString() + " / " + maxHealth;
    }

    public virtual void DealDamage(Entity target)
    {

        target.attacker = this;

        attacker = target.attacker;
        Entity monster = target;//target.GetComponent<Entity>();
        float damage = baseDamage;

        Debug.LogWarning("Attacker is: " + attacker.entityName);
        monster.TakeDamage(damage);
        //Monster mon = tar.GetComponent<Monster>();

    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        UpdateHealthLabel();
        if (health <= 0.0f)
        {
            health = 0.0f;
            isDead = true;
            isReady = false;
            Player player = null;
            if (attacker is Player)
            {
                player = attacker as Player;
                player.expPoints += expReward;
                player.mana += 2.0f;
            }

            OnDeath();
        }
        else
            attacker = null;
    }

    public virtual void OnDeath()
    {
        //healthLabel.text = "DEAD";
        //testExp.AwardExp((testExp.level + expBonus) * expMultiplier);


        Destroy(infotab);
        gameObject.SetActive(false);
        //if (infotab)
        //{
        //    infotab.SetActive(false);
        //}
        Debug.Log("I am dead, play death animation and award reward (exp or somethign), unless i am the player then suffer");
    }

    public virtual void LevelUp()
    {
        float mh = startMaxHealth * 1.5f;
        float bdmg = baseDamage * 1.5f;
        float xpr = expReward * 1.5f;
        health = Mathf.FloorToInt(mh);
        baseDamage = Mathf.FloorToInt(bdmg);
        expReward = Mathf.FloorToInt(xpr);
    }

    public virtual void ResetValues()
    {
        health = startMaxHealth;
        maxHealth = health;
        isDead = false;
        isReady = false;
        //gameObject.SetActive(true);
    }

    public void createGUI()
    {
        if (!infotab)
        {
          //  maxHealth = health;
            canvasGO = GameObject.Find("Canvas");
            infoPanel = canvasGO.transform.FindChild("EnemyInfoPanel").gameObject;

            infotab = Instantiate(entityInfoPrefab) as GameObject;
            infotab.transform.SetParent(infoPanel.transform);
            infotab.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            infotab.name = entityName;

            readyBarLabel = infotab.transform.FindChild("AttackTimer").GetComponent<Text>();
            healthLabel = infotab.transform.FindChild("Health").GetComponent<Text>();
            nameLabel = infotab.transform.FindChild("EntityName").GetComponent<Text>();
            nameLabel.text = entityName;
            UpdateHealthLabel();
        }
    }
}
