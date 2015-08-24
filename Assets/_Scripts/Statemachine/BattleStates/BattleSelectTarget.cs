using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleSelectTarget : IState
{
    private GameObject battlemanagerGO;
    private BattleManager bm;
    private Player player;

    private bool pickedMove = false;

    private bool targetAlly = false;

    private Entity targetEntity;

    private GameObject selection;

    public BattleSelectTarget()
    {

    }

    public BattleSelectTarget(GameObject go)
    {
        battlemanagerGO = go;
        bm = battlemanagerGO.GetComponent<BattleManager>();
    }

    public void OnEnter()
    {
        targetAlly = false;
        Debug.Log("OnEnter " + this.ToString());

        bm.currStateText.text = "Current State: " + "Select Target";
        bm.Selection = bm.Pool.GetPooledObj("selection");
        selection = bm.Selection;
        player = bm.player;

        player.UpdateAttackButtons(PickMove);

        player.UpdateHealButton(HealPlayer);
        player.UpdateAOEMove(AOEMove);

    }

    public void OnExit()
    {
        targetAlly = false;
        player.ToggleAttacksPanel(false);
        //bm.attackBttn.GetComponentInChildren<Text>().text = "Using Slash";
        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        if (pickedMove && Input.GetMouseButtonDown(0) && !bm.selectedTarget)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!targetAlly && Physics.Raycast(ray, out hit, Mathf.Infinity, bm.interactMask))
            {
                bm.selectedTarget = hit.transform.gameObject;
                targetEntity = bm.selectedTarget.GetComponent<Entity>();
                // position the selection circle
                Vector3 scPos = hit.transform.position;
                scPos.y = 1.8f;
                selection.transform.position = scPos;
                selection.SetActive(true);
                //bm.healthLabel.text = targetEntity.health.ToString() + " / " + targetEntity.MaxHealth;

            }

            if (targetAlly && Physics.Raycast(ray, out hit, Mathf.Infinity, bm.allyLayer))
            {
                    bm.selectedTarget = hit.transform.gameObject;
                    targetEntity = player;
                    // position the selection circle
                    Vector3 scPos = hit.transform.position;
                    scPos.y = 1.8f;
                    selection.transform.position = scPos;
                    selection.SetActive(true);
            }
        }

        if (bm.selectedTarget && !targetAlly)
        {
            //bm.PerformMoveState();
            bm.PushState("PerformMove");
        }
        else if (bm.selectedTarget && targetAlly)
        {
            player.health += 2.0f;
            bm.SpawnFloatText(2.0f.ToString("0"), player.gameObject.transform.position + Vector3.right);
            player.UpdateHealthLabel();
            bm.PushState("SelectAction");
        }

        if (Input.GetMouseButtonDown(1))
        {
            // todo pop state to go to prevstate
            bm.PopState();
        }
    }

    public void AOEMove()
    {
        if(player.mana >= 0.0f)
        {
            pickedMove = true;
            targetAlly = false;
            player.currentAttack = 1;
        }else
        {
            pickedMove = false;
            player.currentAttack = -1;
        }
    }

    public void HealPlayer()
    {
        pickedMove = true;
        targetAlly = true;
    }

    public void PickMove()
    {
        pickedMove = true;
        targetAlly = false;

        player.currentAttack = 0;
    }
}
