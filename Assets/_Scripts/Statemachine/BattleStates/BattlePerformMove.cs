using UnityEngine;
using System.Collections;

public class BattlePerformMove : IState
{
    public float delay = 2.0f;
    private float delayTimer = 0.0f;

    private GameObject battlemanagerGO;
    private BattleManager bm;

    private Player player;
    private Entity targetEntity;

    public BattlePerformMove()
    {

    }

    public BattlePerformMove(GameObject go)
    {
        battlemanagerGO = go;
        bm = battlemanagerGO.GetComponent<BattleManager>();
    }

    public void OnEnter()
    {
        Debug.Log("OnEnter " + this.ToString());

        bm.currStateText.text = "Current State: " + "Perform Move";
        player = bm.player;

        // TODO bm.currentAttacker.DoMove();
        targetEntity = bm.selectedTarget.GetComponent<Entity>();

        player.attack.SetTarget(bm.selectedTarget);
        player.attack.SpawnEffect();
       // bm.infoPanel.SetActive(true);
       // bm.healthLabel.text = targetEntity.health.ToString() + " / " + targetEntity.MaxHealth;
        //bm.DealDamage();

        delayTimer = 0.0f;
    }

    public void OnExit()
    {
        player.DealDamage(targetEntity);
        bm.playerTimer = -1.0f;

        player.attackTimer = 0;

        delayTimer = 0.0f;
        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        player.transform.LookAt(bm.selectedTarget.transform.position);

        if (player.attack.isDone)
        {
            bm.InitFloatText(player.baseDamage.ToString(), bm.selectedTarget.transform.position);

            //    bm.healthLabel.text = targetEntity.health.ToString() + " / " + targetEntity.MaxHealth;

            bm.PushState("SelectAction");
        }

        delayTimer += Time.deltaTime;
        if (delayTimer >= delay)
        {
            //bm.SwapState();
            // TODO move back to SelectAction state
           // bm.PushState("SelectAction");
            delayTimer = 0.0f;
        }
    }
}
