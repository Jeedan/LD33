using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : Entity
{
    public GameObject ActionsPanel;
    public Button actionBtn;

    public GameObject AttacksPanel;
    public Button attackBtn;

    public float expPoints = 0.0f;
    public float expToLevel = 10.0f;

    public float level = 1;

    public Ability attack;

    // Use this for initialization
    protected override void Start()
    {
        ActionsPanel.SetActive(false);
        AttacksPanel.SetActive(false);

        maxHealth = health;

        infotab = infoPanel.transform.FindChild("PlayerInfo").gameObject;
        readyBarLabel = infotab.transform.FindChild("AttackTimer").GetComponent<Text>();
        healthLabel = infotab.transform.FindChild("Health").GetComponent<Text>();
        nameLabel = infotab.transform.FindChild("EntityName").GetComponent<Text>();
        nameLabel.text = entityName;

        //healthLabel.text = "Health: " + health + " / " + maxHealth;
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
        attackBtn.GetComponentInChildren<Text>().text = "Slash";
        attackBtn.interactable = true;
    }

    public void ToggleAttacksPanel(bool toggle)
    {
        AttacksPanel.SetActive(toggle);
    }
}
