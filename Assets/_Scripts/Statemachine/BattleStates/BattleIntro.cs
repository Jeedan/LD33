using UnityEngine;
using System.Collections;
using System;

public class BattleIntro : IState
{
    public float introDelay = 2.0f;
    private float introTimer = 0.0f;

    private BattleManager bm;

    public void OnEnter(GameObject go)
    {
        Debug.Log("OnEnter "+ this.ToString());

        bm = go.GetComponent<BattleManager>();

        bm.currStateText.text = "Current State: " + "Intro";
    }

    public void OnExit()
    {
        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        introTimer += Time.deltaTime;
        Debug.Log("Playing Intro: " + introTimer);
        if (introTimer >= introDelay)
        {
            // currentState = BattleStates.SelectAction;
            bm.SelectActionState();
            introTimer = 0.0f;
        }
    }
}
