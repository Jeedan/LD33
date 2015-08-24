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
        AreMonstersAlive();
        player = bm.player;

        player.UpdateActionButton(bm.SetStateToSelectTarget);
        if (bm.Selection)
            bm.Selection.SetActive(false);

        //bm.attackBttn.onClick.RemoveAllListeners();
        //bm.attackBttn.onClick.AddListener(bm.SetStateToSelectTarget); // used to be Attack
        //bm.attackBttn.GetComponentInChildren<Text>().text = "Attack";
        //bm.attackBttn.interactable = false;

        bm.currStateText.text = "Current State: " + "Select Action";
        bm.UpdatePlayerLevelLabels();
        player.UpdateHealthLabel();
        player.UpdateTimerLabel();
        bm.selectedTarget = null;
        attacker = null;
    }

    public void OnExit()
    {
        delayTimer = 0.0f;
        //bm.attackBttn.interactable = false;
        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        // TODO ATTACK TIMER
        //HoverOverEnemy();
        AreMonstersReady();

        IsPlayerReady();

        if (player.isDead)
        {
            bm.ChangeState("GameOver");
        }
    }

    void AreMonstersAlive()
    {
        int deathCounter = 0;
        for(int i=0; i < bm.entities.Count; i++)
        {
            if (bm.entities[i].isDead)
            {
                deathCounter++;
            }
        }

        if(deathCounter >= bm.entities.Count)
        {
            bm.ChangeState("WaveIncrease");
            deathCounter = 0;
        }
    }

    void AreMonstersReady()
    {
        float aTime = Mathf.Infinity;

        for (int i = 0; i < bm.entities.Count; i++)
        {
            if (bm.entities[i].isDead) continue;

            if (!bm.entities[i].isReady)
            {
                bm.entities[i].readyTime -= Time.deltaTime;//0.01f;
            }


            bm.entities[i].UpdateTimerLabel();
            if (bm.entities[i].attackTimer >= bm.entities[i].readyTime)
            {
                // TODO switch state to attack state, set attacker to be the monster.
                bm.entities[i].isReady = true;

                //bm.attacker = attacker;
                // bm.entities[i].attackTimer = bm.entities[i].readyTime;
            }

            if (bm.entities[i].isReady)
            {
                float tempTime = bm.entities[i].readyTime;
                if (tempTime <= aTime)
                {
                    attacker = bm.entities[i];
                }

                delayTimer += Time.deltaTime;
            }
        }

        if (delayTimer >= delay)
        {
            if (attacker != null)
            {
                bm.attacker = attacker;
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
            player.readyTime -= Time.deltaTime;

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
}
