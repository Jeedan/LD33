using UnityEngine;
using System.Collections;

public class BattleSwap : IState
{
    private BattleManager bm;

    public float delay = 2.0f;
    private float delayTimer = 0.0f;

    public void OnEnter(GameObject go)
    {
        Debug.Log("OnEnter " + this.ToString());
        bm = go.GetComponent<BattleManager>();

        bm.currStateText.text = "Current State: " + "Swap Turn";

        delayTimer += Time.deltaTime;

        bm.playerHealth--;
        Debug.Log("Monster deals damage to player" + bm.playerHealth);
    }

    public void OnExit()
    {
        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        delayTimer += Time.deltaTime;
        if (delayTimer >= delay)
        {
            bm.SelectActionState();
            delayTimer = 0.0f;
        }
    }
}
