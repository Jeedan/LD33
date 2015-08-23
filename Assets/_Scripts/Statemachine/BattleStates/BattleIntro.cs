using UnityEngine;
using System.Collections;
using System;

public class BattleIntro : IState
{
    public float introDelay = 2.0f;
    private float introTimer = 0.0f;

    private GameObject battlemanagerGO;
    private BattleManager bm;
    
    public BattleIntro()
    {

    }

    public BattleIntro(GameObject go)
    {
        battlemanagerGO = go;

        bm = battlemanagerGO.GetComponent<BattleManager>();
    }    

    public void OnEnter()
    {
        Debug.Log("OnEnter "+ this.ToString());
        
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
            //bm.SelectActionState();

            bm.ChangeState("SelectAction");
            //bm.PushState("SelectAction");
            introTimer = 0.0f;
        }
    }
}
