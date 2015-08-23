using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleSelectAction : IState
{
    private GameObject battlemanagerGO;
    private BattleManager bm;

    private Player player;
    public GameObject hoverTarget;

    Entity attacker;

    private float delayTimer = 0.0f;
    private float delay = 1.0f;

    public BattleSelectAction()
    {

    }

    public BattleSelectAction(GameObject go)
    {
        battlemanagerGO = go;
        bm = battlemanagerGO.GetComponent<BattleManager>();
    }

    public void OnEnter()
    {
        Debug.Log("OnEnter " + this.ToString());
        player = bm.player;

        player.UpdateActionButton(bm.SetStateToSelectTarget);

        //bm.attackBttn.onClick.RemoveAllListeners();
        //bm.attackBttn.onClick.AddListener(bm.SetStateToSelectTarget); // used to be Attack
        //bm.attackBttn.GetComponentInChildren<Text>().text = "Attack";
        //bm.attackBttn.interactable = false;

        bm.currStateText.text = "Current State: " + "Select Action";

        bm.selectedTarget = null;
        attacker = null;
    }

    public void OnExit()
    {
        //bm.attackBttn.interactable = false;
        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        // TODO ATTACK TIMER
        //HoverOverEnemy();
        delayTimer += Time.deltaTime;

        AreMonstersReady();
      
        IsPlayerReady();
    }

    void AreMonstersReady()
    {

        for (int i = 0; i < bm.entities.Length; i++)
        {
            if (bm.entities[i].isDead) continue;

            if (!bm.entities[i].isReady)
            {
                bm.entities[i].readyTime -= Time.deltaTime;
                bm.entities[i].UpdateTimerLabel();
            }


            if (bm.entities[i].attackTimer >= bm.entities[i].readyTime)
            {
                // TODO switch state to attack state, set attacker to be the monster.
                bm.entities[i].isReady = true;

                bm.attacker = attacker;
                // bm.entities[i].attackTimer = bm.entities[i].readyTime;
            }
        }

        float aTime = Mathf.Infinity;

        for (int j = 0; j < bm.entities.Length; j++)
        {
            if (bm.entities[j].isReady)
            {
                float tempTime = bm.entities[j].attackTimer;
                if (tempTime <= aTime)
                {
                    attacker = bm.entities[j];
                }
            }
        }


        if (delayTimer >= delay)
        {
            if (attacker != null)
            {
                bm.attacker = attacker;
                delayTimer = 0.0f;
                bm.PushState("Swap");
            }
        }

        //if (!bm.monsterReady)
        //    bm.monsterTimer += Time.deltaTime;


        //if (bm.monsterTimer >= bm.monsterAttackReadyTime)
        //{
        //    // TODO switch state to attack state, set attacker to be the monster.
        //    bm.monsterReady = true;
        //    bm.monsterTimer = bm.monsterAttackReadyTime;
        //    bm.PushState("Swap");
        //}
    }

    void IsPlayerReady()
    {

        if (!player.isReady)
            player.attackTimer += Time.deltaTime;

        player.UpdateTimerLabel();
        if (player.attackTimer >= player.readyTime)
        {
            player.isReady = true;
            player.actionBtn.interactable = true;
            //bm.attackBttn.interactable = true;
            player.attackTimer = player.readyTime;
            bm.PushState("SelectAction");
        }
        else
        {
            player.isReady = false;
        }
    }

    void HoverOverEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, bm.interactMask))
        {
            hoverTarget = hit.transform.gameObject;
        }
        else
        {
            hoverTarget = null;
        }

        if (hoverTarget)
        {
            //bm.PerformMoveState();
            //            bm.infoPanel.SetActive(true);
            Entity targetEntity = hoverTarget.GetComponent<Entity>();
            bm.UpdateTimerLabels(targetEntity.attackTimer, player.attackTimer);

            //          bm.healthLabel.text = targetEntity.health.ToString() + " / " + targetEntity.MaxHealth;
        }
        else
        {
            //            bm.infoPanel.SetActive(false);
        }

    }
}
