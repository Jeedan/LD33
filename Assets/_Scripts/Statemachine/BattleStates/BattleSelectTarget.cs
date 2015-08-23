using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleSelectTarget : IState
{
    private GameObject battlemanagerGO;
    private BattleManager bm;
    private Player player;

    private bool pickedMove = false;


    private Entity targetEntity;

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
        Debug.Log("OnEnter " + this.ToString());

        bm.currStateText.text = "Current State: " + "Select Target";

        player = bm.player;

        player.UpdateAttackButtons(PickMove);

        //bm.attackBttn.onClick.RemoveAllListeners();
        //bm.attackBttn.onClick.AddListener(PickMove); // used to be Attack
        //bm.attackBttn.GetComponentInChildren<Text>().text = "Slash";
        //bm.attackBttn.interactable = true;
    }

    public void OnExit()
    {
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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, bm.interactMask))
            {
                bm.selectedTarget = hit.transform.gameObject;
                targetEntity = bm.selectedTarget.GetComponent<Entity>();
//                bm.healthLabel.text = targetEntity.health.ToString() + " / " + targetEntity.MaxHealth;

            }
        }

        if (bm.selectedTarget)
        {
            //bm.PerformMoveState();
            bm.PushState("PerformMove");
        }

        if (Input.GetMouseButtonDown(1))
        {
            // todo pop state to go to prevstate
            bm.PopState();
        }
    }

    public void PickMove()
    {
        pickedMove = true;

        //bm.attackBttn.GetComponentInChildren<Text>().text = "Select a target to attack";
    }
}
