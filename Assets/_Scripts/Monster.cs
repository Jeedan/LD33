using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{

    public float health = 10.0f;
    private float maxHealth;

    public Text healthText;
    public Experience testExp;

    private bool isDead = false;
    private float deadTimer = 0.0f;
    public float deadDelay = 2.0f;

    public float expBonus = 5.0f;
    public float expMultiplier = 2.0f;


    public GameObject floattxtPrefab;
    private Text floatTxt;
    private ObjectPooler pool;

    private GameObject canvas;
    // Use this for initialization
    void Start()
    {
        maxHealth = health;

        canvas = GameObject.Find("Canvas");
        pool = ObjectPoolManager.objectPool; //new ObjectPooler();
        pool.InitPool("floatDmg", floattxtPrefab, 4, true);    
    }

    void InitFloatText(string text)
    {
        GameObject fGO = pool.GetPooledObj("floatDmg");

        if (fGO == null) return;

        floatTxt = fGO.GetComponent<Text>();
        fGO.name = "floatText";
        floatTxt.text = text;
        fGO.transform.SetParent(canvas.transform);

        Vector3 p = Camera.main.WorldToScreenPoint(transform.position);// - (Vector3.right * 60.0f);
       // p.x += 150.0f; 
        p.z = 0.0f;

        fGO.transform.position = p;// - (Vector3.right * 60.0f);
        fGO.transform.localScale = new Vector3(1.5f, 1.5f, 1);  // change size, for better readablility, not sure if it is better to increase font.
        fGO.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {
            deadTimer += Time.deltaTime;

            if (deadTimer >= deadDelay)
            {
                health = maxHealth;
                healthText.text = "Health: " + health + " / " + maxHealth;
                deadTimer = 0.0f;
                isDead = false;
            }
        }
    }
    
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        InitFloatText(amount.ToString());
        healthText.text = "Health: " + health + " / " + maxHealth;
        if (health <= 0.0f)
        {
            health = 0.0f;
            isDead = true;
            OnDeath();
        }
    }

    void OnDeath()
    {
        healthText.text = "DEAD";
        testExp.AwardExp((testExp.level + expBonus) * expMultiplier);
        Debug.Log("I am dead, play death animation and award reward (exp or somethign)");

    }

}
