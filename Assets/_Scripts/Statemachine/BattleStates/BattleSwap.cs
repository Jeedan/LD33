using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleSwap : IState
{
    public float delay = 2.0f;
    private float delayTimer = 0.0f;

    private GameObject battlemanagerGO;
    private BattleManager bm;

    private ObjectPooler pool;
    private GameObject canvasGO;

    Entity attacker;

    public BattleSwap()
    {

    }

    public BattleSwap(GameObject go)
    {
        battlemanagerGO = go;
        bm = battlemanagerGO.GetComponent<BattleManager>();
        pool = ObjectPoolManager.objectPool;
        canvasGO = GameObject.Find("Canvas");

        // pool.InitPool("floatDmg", floattxtPrefab, 4, true);
    }

    public void OnEnter()
    {
        Debug.Log("OnEnter " + this.ToString());

        bm.currStateText.text = "Current State: " + "Swap Turn";

        delayTimer += Time.deltaTime;
        //bm.playerHealth--;
        //SpawnFloatText(1);
        Debug.Log("Monster deals damage to player health is now " + bm.playerHealth);

        Entity ent = bm.player;

        
        attacker = bm.attacker;
        if (attacker.isReady)
        {
            attacker.DealDamage(ent);
            bm.InitFloatText(attacker.baseDamage.ToString(), bm.player.transform.position);
        }

        attacker.readyTime = Random.Range(3.0f, 10.0f);
        attacker.isReady = false;
        //bm.monsterTimer = -2.0f;
        //bm.monsterReady = false;

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
            //bm.SelectActionState();
            bm.PopState();
            //bm.PushState("SelectAction");
            delayTimer = 0.0f;
        }
    }

    //void SpawnFloatText(float dmg)
    //{
    //    GameObject fGO = pool.GetPooledObj("floatDmg");

    //    if (fGO == null) return;
    //    Text floatTxt = fGO.GetComponent<Text>();
    //    fGO.name = "floatText";
    //    floatTxt.text = dmg.ToString();
    //    fGO.transform.SetParent(canvasGO.transform);

    //    Vector3 p = Camera.main.WorldToScreenPoint(new Vector3(-26.0f, 2.524011f, 0.0f));// - (Vector3.right * 60.0f);
    //                                                                                     // p.x += 150.0f; 
    //    p.z = 0.0f;

    //    fGO.transform.position = p;// - (Vector3.right * 60.0f);
    //    fGO.transform.localScale = new Vector3(1.5f, 1.5f, 1);  // change size, for better readablility, not sure if it is better to increase font.
    //    fGO.SetActive(true);
    //}
}
