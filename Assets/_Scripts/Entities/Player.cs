using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : Entity
{
    public GameObject ActionsPanel;
    public Button actionBtn;

    public GameObject AttacksPanel;
    public Button attackBtn;
    public Button healBtn;
    public Button aoeBtn;

    public float speed = 2.0f;
    public float expPoints = 0.0f;
    public float expToLevel = 10.0f;

    public float level = 1;

    public Ability attack;

    public int currentAttack = -1;

    public float mana = 10.0f;
    public float maxMana;
    // Use this for initialization
    protected override void Start()
    {
        ActionsPanel.SetActive(false);
        AttacksPanel.SetActive(false);
        maxMana = mana;
        maxHealth = health;

        infotab = infoPanel.transform.FindChild("PlayerInfo").gameObject;
        readyBarLabel = infotab.transform.FindChild("AttackTimer").GetComponent<Text>();
        healthLabel = infotab.transform.FindChild("Health").GetComponent<Text>();
        nameLabel = infotab.transform.FindChild("EntityName").GetComponent<Text>();
        nameLabel.text = entityName;

        //healthLabel.text = "Health: " + health + " / " + maxHealth;
    }

    public override void UpdateHealthLabel()
    {
        healthLabel.text = "Health: " + health.ToString("0") + "/" + maxHealth.ToString("0") + " Mana: " + mana.ToString("0") + "/" + maxMana.ToString("0");
    }
    public void PerformAOE(Vector3 target, BattleManager _bm)
    {
        float prevDmg = baseDamage;
        List<Entity> victims = new List<Entity>();
        
        Collider[] cols = Physics.OverlapSphere(target, 4.0f, _bm.interactMask);
        Entity selected = _bm.selectedTarget.GetComponent<Entity>();

        victims.Add(selected);
       // DealDamage(selected);
        //_bm.SpawnFloatText(baseDamage.ToString(), selected.transform.position);
        for (int i = 0; i < cols.Length; i++)
        {
            Entity mon = cols[i].GetComponent<Entity>();

            if(!victims.Contains(mon))
            {
                victims.Add(mon);
            }
        }

        for(int j = 0; j < victims.Count; j++)
        {

            DealDamage(victims[j]);

            _bm.SpawnFloatText(baseDamage.ToString(), victims[j].transform.position);

            float dmg = baseDamage * 0.5f;
            baseDamage =  (int)dmg;
        }

        mana -= 5.0f;
        baseDamage = prevDmg;
    }

    public void UpdateExperience()
    {
        if (expPoints >= expToLevel)
        {
            level++;
            base.LevelUp();
            health = maxHealth;
            expPoints = 0.0f;
            float xpr = expToLevel * 1.5f;
            expToLevel = Mathf.FloorToInt(xpr);
            float manaUp = mana * 1.5f;
            mana = Mathf.FloorToInt(manaUp);
        }
    }

    public void UpdateReadyBar(float pTimer)
    {
        //readyBarLabel.text = readyText + pTimer.ToString("0.0");
    }


    public void UpdateActionButton(UnityAction listener)
    {
        ActionsPanel.SetActive(true);
        actionBtn.onClick.RemoveAllListeners();
        actionBtn.onClick.AddListener(listener); // used to be Attack
        actionBtn.GetComponentInChildren<Text>().text = "Attack";
        actionBtn.interactable = false;
    }

    public void UpdateAttackButtons(UnityAction listener)
    {
        ActionsPanel.SetActive(false);
        ToggleAttacksPanel(true);
        attackBtn.onClick.RemoveAllListeners();
        attackBtn.onClick.AddListener(listener); // used to be Attack
        attackBtn.GetComponentInChildren<Text>().text = "Fireball";
        attackBtn.interactable = true;
    }

    public void UpdateHealButton(UnityAction listener)
    {
        ActionsPanel.SetActive(false);
        ToggleAttacksPanel(true);
        healBtn.onClick.RemoveAllListeners();
        healBtn.onClick.AddListener(listener); // used to be Attack
        healBtn.GetComponentInChildren<Text>().text = "Heal";
        healBtn.interactable = true;
    }

    public void UpdateAOEMove(UnityAction listener)
    {
        ActionsPanel.SetActive(false);
        ToggleAttacksPanel(true);
        aoeBtn.onClick.RemoveAllListeners();
        aoeBtn.onClick.AddListener(listener); // used to be Attack
        aoeBtn.GetComponentInChildren<Text>().text = "AOE";
        aoeBtn.interactable = true;

        if(mana > 0.0f)
        {
            aoeBtn.interactable = true;
        }
        else
        {
            aoeBtn.interactable = false;
        }
    }

    public void ToggleAttacksPanel(bool toggle)
    {
        AttacksPanel.SetActive(toggle);
    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
    }
}
