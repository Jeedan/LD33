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

    protected string readyText = "Attack in: ";
    public string entityName = "Demon";
    protected GameObject infotab;
    // Use this for initialization
    protected virtual void Start()
    {
        maxHealth = health;

        infotab = Instantiate(entityInfoPrefab) as GameObject;
        infotab.transform.SetParent(infoPanel.transform);
        infotab.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        readyBarLabel = infotab.transform.FindChild("AttackTimer").GetComponent<Text>();
        healthLabel = infotab.transform.FindChild("Health").GetComponent<Text>();
        nameLabel = infotab.transform.FindChild("EntityName").GetComponent<Text>();
        nameLabel.text = entityName;
    }

    public virtual void UpdateTimerLabel()
    {
        readyBarLabel.text = readyText + readyTime.ToString("0.0");
    }

    public virtual void UpdateHealthLabel()
    {
        healthLabel.text = "Health: " + health.ToString() + " / " + maxHealth.ToString();
    }

    public virtual void DealDamage(Entity target)
    {

        Entity monster = target;//target.GetComponent<Entity>();
        float damage = baseDamage;
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
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        //healthLabel.text = "DEAD";
        //testExp.AwardExp((testExp.level + expBonus) * expMultiplier);
        gameObject.SetActive(false);
        if (infotab)
        {
            infotab.SetActive(false);
        }
        Debug.Log("I am dead, play death animation and award reward (exp or somethign), unless i am the player then suffer");
    }
}
