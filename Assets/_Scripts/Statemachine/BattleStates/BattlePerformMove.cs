using UnityEngine;
using System.Collections;

public class BattlePerformMove : IState
{
    public float delay = 1.0f;
    private float delayTimer = 0.0f;

    private GameObject battlemanagerGO;
    private BattleManager bm;

    private Player player;
    private Entity targetEntity;

    private Quaternion startRotation;
    float prevDmg;
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
        startRotation = player.transform.rotation;

        // TODO bm.currentAttacker.DoMove();
        targetEntity = bm.selectedTarget.GetComponent<Entity>();

        prevDmg = player.baseDamage;
        if (player.currentAttack == 0)
        {
            float dmg = player.baseDamage * 1.5f;
            player.baseDamage = (int)dmg;
            player.attack.SetTarget(bm.selectedTarget);
            player.attack.SpawnEffect();
        }
        else if (player.currentAttack == 1)
        {
            player.PerformAOE(bm.selectedTarget.transform.position, bm);

        }


        // bm.infoPanel.SetActive(true);
        // bm.healthLabel.text = targetEntity.health.ToString() + " / " + targetEntity.MaxHealth;
        //bm.DealDamage();

        delayTimer = 0.0f;
    }

    public void OnExit()
    {

        if (player.currentAttack == 0)
        {
            player.DealDamage(targetEntity);
            player.readyTime = player.speed;
            player.baseDamage = prevDmg;
        }
        else
        {
            player.readyTime = 3.0f;
        }


        player.transform.rotation = startRotation;
        delayTimer = 0.0f;

        player.UpdateExperience();
        bm.UpdatePlayerLevelLabels();
        // spawn float exp txt
        if (targetEntity.health <= 0.0f)
            bm.SpawnFloatText(targetEntity.expReward.ToString("0") + "xp", player.gameObject.transform.position + Vector3.right);

        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        player.transform.LookAt(bm.selectedTarget.transform.position);

        if (player.attack.isDone)
        {
            bm.SpawnFloatText(player.baseDamage.ToString(), bm.selectedTarget.transform.position);
            //    bm.healthLabel.text = targetEntity.health.ToString() + " / " + targetEntity.MaxHealth;

            bm.PushState("SelectAction");
        }

        if (player.currentAttack == 1)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delay)
            {
                bm.PushState("SelectAction");

                //bm.SwapState();
                // TODO move back to SelectAction state
                // bm.PushState("SelectAction");
                delayTimer = 0.0f;
            }
        }
    }
}
