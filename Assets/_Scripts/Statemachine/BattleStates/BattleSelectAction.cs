using UnityEngine;
using System.Collections;

public class BattleSelectAction : IState
{
    private BattleManager bm;

    public void OnEnter(GameObject go)
    {
        Debug.Log("OnEnter " + this.ToString());
        bm = go.GetComponent<BattleManager>();

        bm.currStateText.text = "Current State: " + "Select Action";

        bm.selectedTarget = null;
        bm.attackBttn.interactable = true;
    }

    public void OnExit()
    {

        bm.attackBttn.interactable = false;
        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
    }
}
