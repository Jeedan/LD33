using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleIncreaseWave : IState
{
    public float delay = 2.0f;
    private float delayTimer = 0.0f;

    private GameObject battlemanagerGO;
    private BattleManager bm;

    private GameObject waveText;
    Entity attacker;

    public BattleIncreaseWave()
    {

    }

    public BattleIncreaseWave(GameObject go)
    {
        battlemanagerGO = go;
        bm = battlemanagerGO.GetComponent<BattleManager>();

    }

    public void OnEnter()
    {
        Debug.Log("OnEnter " + this.ToString());

        bm.currStateText.text = "Current State: " + "Swap Turn";

        waveText = bm.Pool.GetPooledObj("GameOver");

        waveText.transform.SetParent(bm.canvasGO.transform);
        waveText.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        waveText.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
        Text txt = waveText.GetComponent<Text>();
        bm.currentWave++;
        txt.text = "Wave " + bm.currentWave;
        waveText.SetActive(true);
        Entity ent = bm.player;
        bm.monsterSpawnCount++;

     
    }

    public void OnExit()
    {
        bm.SpawnMonster();
        for (int i = 0; i < bm.monsterSpawnCount; i++)
        {
            bm.entities[i].readyTime = Random.Range(6.0f, 10.0f);
            bm.entities[i].LevelUp();
            bm.entities[i].ResetValues();

            bm.entities[i].UpdateHealthLabel();
        }
        waveText.SetActive(false);

        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        if (bm.monsterSpawnCount >= 3)
        {
            bm.monsterSpawnCount = 3;
        }
        //for (int i = 0; i < bm.monsterSpawnCount; i++)
        //{
        //    bm.entities[i].isReady = false;
        //    bm.entities[i].readyTime = Random.Range(6.0f, 10.0f);
        //    bm.entities[i].LevelUp();
        //    bm.entities[i].ResetValues();
        //}

        delayTimer += Time.deltaTime;
        if (delayTimer >= delay)
        {
            bm.ChangeState("SelectAction");
            delayTimer = 0.0f;
        }
    }
}
