using UnityEngine;
using System.Collections;

public class BattleSelectTarget : IState
{
    private BattleManager bm;

    public void OnEnter(GameObject go)
    {
        Debug.Log("OnEnter " + this.ToString());
        bm = go.GetComponent<BattleManager>();

        bm.currStateText.text = "Current State: " + "Select Target";

    }

    public void OnExit()
    {
        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !bm.selectedTarget)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, bm.interactMask))
            {
                bm.selectedTarget = hit.transform.gameObject;
            }
        }

        if (bm.selectedTarget)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bm.PerformMoveState();
            }
        }
    }
}
