using UnityEngine;
using System.Collections;

public class BattlePerformMove : IState
{
    private BattleManager bm;

    public float delay = 2.0f;
    private float delayTimer = 0.0f;

    public void OnEnter(GameObject go)
    {
        Debug.Log("OnEnter " + this.ToString());
        bm = go.GetComponent<BattleManager>();

        bm.currStateText.text = "Current State: " + "Perform Move";

        bm.DealDamage();

        delayTimer = 0.0f;
    }

    public void OnExit()
    {

        delayTimer = 0.0f;
        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {

        delayTimer += Time.deltaTime;
        if (delayTimer >= delay)
        {
            bm.SwapState();
            delayTimer = 0.0f;
        }
    }
}
