using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleOver : IState
{
    public float delay = 4.0f;
    private float delayTimer = 0.0f;

    private GameObject battlemanagerGO;
    private BattleManager bm;

    private GameObject gameOver;
    Entity attacker;

    public BattleOver()
    {

    }

    public BattleOver(GameObject go)
    {
        battlemanagerGO = go;
        bm = battlemanagerGO.GetComponent<BattleManager>();

    }

    public void OnEnter()
    {
        Debug.Log("OnEnter " + this.ToString());

        bm.currStateText.text = "Main menu in: " + delay;

        gameOver = bm.Pool.GetPooledObj("GameOver");

        gameOver.transform.SetParent(bm.canvasGO.transform);
        gameOver.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameOver.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
        Text txt = gameOver.GetComponent<Text>();
        txt.text = "GameOver, I'll take you to the main menu, because that sounds fun ;) - Satan";
        gameOver.SetActive(true);
        Entity ent = bm.player;
    }

    public void OnExit()
    {
        gameOver.SetActive(false);

        Debug.Log("OnExit " + this.ToString());
    }

    public void OnUpdate()
    {
        bm.currStateText.text = "Main menu in: " + delayTimer.ToString("0.0");
        delayTimer += Time.deltaTime;
        if (delayTimer >= delay)
        {
            Application.LoadLevel("mainMenu");
            delayTimer = 0.0f;
        }
    }
}
