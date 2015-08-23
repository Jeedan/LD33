using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public enum BattleStates
    {
        Intro,
        SelectAction,
        SelectTarget,
        PerformMoves,
        Swap,
        Death
    }

    public BattleStates currentState = BattleStates.Intro;

    public LayerMask interactMask;
    public Button attackBttn;

    public Text currStateText;


    public GameObject canvasGO;

    public GameObject floattxtPrefab;
    private Text floatTxt;
    // todo refactor statemachine
    public StateMachine statemachine;


    public Text playerTimerLabel;
    public string monsterTimeText;
    public string playerTimeText;
    
    public float monsterTimer = 0.0f;
    public float playerTimer = 0.0f;
    public float monsterAttackReadyTime = 1.0f;
    public float playerAttackReadyTime = 2.0f;

    public bool monsterReady = false;
    public bool playerReady = false;

    public GameObject selectedTarget;

    public float dmg = 2.0f;

    public float playerHealth = 10.0f;

    public bool damageDone = false;

    public Entity[] entities;

    public Player player;
    private ObjectPooler pool;

    public Entity attacker = null;

    // Use this for initialization
    void Start()
    {
        statemachine = new StateMachine();
        selectedTarget = null;
        attackBttn.interactable = false;


        UpdateTimerLabels(monsterAttackReadyTime, player.readyTime);
        pool = ObjectPoolManager.objectPool;

        // TODO move the prefabs to a better location
        pool.InitPool("floatDmg", floattxtPrefab, 6, true);
        pool.InitPool("Fireball", player.attack.effectPrefab, 2, true);
        

        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].readyTime = Random.Range(4.0f, 8.0f);
        }

        //infoPanel.SetActive(false);
        //IntroState();
        InitializeStates();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        statemachine.OnUpdate();
        //ManageStates();
    }

    public void UpdateTimerLabels(float mTimer, float pTimer)
    {

        player.UpdateReadyBar(pTimer);
        //playerTimerLabel.text = playerTimeText + pTimer.ToString("0.0");
    }

    public void PerformMove()
    {
        currentState = BattleStates.PerformMoves;
        DealDamage();
        currentState = BattleStates.Swap;
    }

    void InitializeStates()
    {

        IState introState = new BattleIntro(gameObject);
        IState selectActionState = new BattleSelectAction(gameObject);
        IState selectTargetState = new BattleSelectTarget(gameObject);
        IState performMoveState = new BattlePerformMove(gameObject);
        IState swapState = new BattleSwap(gameObject);

        statemachine.AddState("Intro", introState);
        statemachine.AddState("SelectAction", selectActionState);
        statemachine.AddState("SelectTarget", selectTargetState);
        statemachine.AddState("PerformMove", performMoveState);
        statemachine.AddState("Swap", swapState);

        statemachine.ChangeState("Intro");
    }

    public void PushState(string state)
    {
        statemachine.PushState(state);
    }

    public void PopState()
    {
        statemachine.Pop();
    }

    public void ChangeState(string name)
    {
        statemachine.ChangeState(name);
    }
    
    public void SetStateToSelectTarget()
    {
        //IState selectTargetState = new BattleSelectTarget(gameObject);
        //statemachine.ChangeState(selectTargetState);
        PushState("SelectTarget");
    }

    public void DealDamage()
    {
        GameObject tar = selectedTarget;
        //Monster mon = tar.GetComponent<Monster>();
        Entity monster = tar.GetComponent<Entity>();
        float damage = player.baseDamage;
        monster.TakeDamage(damage);
        damageDone = true;
    }

    public void InitFloatText(string text, Vector3 pos)
    {
        pool = ObjectPoolManager.objectPool; //new ObjectPooler();
        GameObject fGO = pool.GetPooledObj("floatDmg");

        if (fGO == null) return;

        floatTxt = fGO.GetComponent<Text>();
        fGO.name = "floatText";
        floatTxt.text = text;
        fGO.transform.SetParent(canvasGO.transform);

        Vector3 p = Camera.main.WorldToScreenPoint(pos);// - (Vector3.right * 60.0f);
                                                                       // p.x += 150.0f; 
        p.z = 0.0f;

        fGO.transform.position = p;// - (Vector3.right * 60.0f);
        fGO.transform.localScale = new Vector3(1.5f, 1.5f, 1);  // change size, for better readablility, not sure if it is better to increase font.
        fGO.SetActive(true);
    }

    //void ManageStates()
    //{
    //    switch (currentState)
    //    {
    //        case BattleStates.Intro:
    //            currStateText.text = "Current State: " + "Intro";
    //            introTimer += Time.deltaTime;
    //            Debug.Log("Playing Intro: " + introTimer);
    //            if (introTimer >= introDelay)
    //            {
    //                currentState = BattleStates.SelectAction;
    //                introTimer = 0.0f;
    //            }

    //            break;
    //        case BattleStates.SelectAction:
    //            currStateText.text = "Current State: " + "Select Action";

    //            selectedTarget = null;
    //            attackBttn.interactable = true;
    //            break;
    //        case BattleStates.SelectTarget:
    //            currStateText.text = "Current State: " + "Select Target";
    //            if (Input.GetMouseButtonDown(0) && !selectedTarget)
    //            {
    //                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //                RaycastHit hit;
    //                if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactMask))
    //                {
    //                    selectedTarget = hit.transform.gameObject;

    //                    attackBttn.interactable = false;
    //                }
    //            }

    //            if (selectedTarget)
    //            {
    //                if (Input.GetKeyDown(KeyCode.Space))
    //                {
    //                    PerformMove();
    //                    //Attack();
    //                }
    //            }
    //            break;
    //        case BattleStates.PerformMoves:
    //            currStateText.text = "Current State: " + "Perform Move";
    //            Attack();
    //            break;
    //        case BattleStates.Swap:
    //            currStateText.text = "Current State: " + "Swap Turn";
    //            playerHealth--;
    //            Debug.Log("Monster deals damage to player" + playerHealth);
    //            currentState = BattleStates.SelectAction;
    //            break;
    //        case BattleStates.Death:
    //            currStateText.text = "Current State: " + "DEAD";
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
