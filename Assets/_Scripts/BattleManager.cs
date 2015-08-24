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
    public LayerMask allyLayer;


    public Text currStateText;


    public GameObject canvasGO;

    public GameObject floattxtPrefab;
    private Text floatTxt;
    // todo refactor statemachine
    public StateMachine statemachine;

    public GameObject selectedTarget;

    public Transform[] enemySpawns;
    public GameObject[] entPrefabs; // for spawning
    [SerializeField]
    public List<Entity> entities; // for logic stuff
    public int monsterSpawnCount = 3;

    public Player player;
    private ObjectPooler pool;
    public ObjectPooler Pool
    {
        get
        {
            if (pool == null)
                pool = ObjectPoolManager.objectPool;

            return pool;
        }
        private set
        {
            if (pool == null)
                pool = ObjectPoolManager.objectPool;
        }
    }

    public Entity attacker = null;

    public GameObject selectCirclePrefab;

    private GameObject selection;
    public GameObject Selection { get { return selection; } set { selection = value; } }

    public Text gameOverText;
    public Text playerLevel;
    public Text expText;
    public GameObject introDialoguePanel;
    public int currentWave = 1;

    void Awake()
    {
        entities = new List<Entity>(); // new entity list
    }

    // Use this for initialization
    void Start()
    {

        statemachine = new StateMachine();
        selectedTarget = null;

        // UpdateTimerLabels(monsterAttackReadyTime, player.readyTime);
        pool = ObjectPoolManager.objectPool;

        // TODO move the prefabs to a better location
        pool.InitPool("floatDmg", floattxtPrefab, 6, true);
        pool.InitPool("Fireball", player.attack.effectPrefab, 2, true);
        pool.InitPool("selection", selectCirclePrefab, 3, true);
        pool.InitPool("GameOver", gameOverText.gameObject, 1, false);
        pool.InitPool("scrub", entPrefabs[0].gameObject, 3, false);
        pool.InitPool("leader", entPrefabs[1].gameObject, 3, false);

        SpawnMonster();

        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].readyTime = Random.Range(4.0f, 8.0f);
        }

        InitializeStates();
    }

    public void SpawnMonster()
    {
        entities.Clear();

        for (int i = 0; i < monsterSpawnCount; i++)
        {

            int randomIndex = Random.Range(0, entPrefabs.Length);

            GameObject monst;
            Entity ent;
            //GameObject en = Instantiate(entPrefabs[randomIndex], enemySpawns[j].position, enemySpawns[j].rotation) as GameObject;

            if (randomIndex == 0)
            {
                monst = pool.GetPooledObj("scrub");
                ent = monst.GetComponent<Entity>();
                ent.entityName = "Demonscrub " + i;
            }
            else
            {
                monst = pool.GetPooledObj("leader");
                ent = monst.GetComponent<Entity>();
                ent.entityName = "Demonlord " + i;

            }

            monst.transform.position = enemySpawns[i].position;
            monst.transform.rotation = enemySpawns[i].rotation;
            monst.SetActive(true);

            //ent.ResetValues();
            //ent.readyTime = Random.Range(6.0f, 10.0f);
            ent.createGUI();
            //    bm.entities[i].isReady = false;
            //    bm.entities[i].readyTime = Random.Range(6.0f, 10.0f);
            //    bm.entities[i].LevelUp();

    //        ent.UpdateHealthLabel();
            entities.Add(ent);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        statemachine.OnUpdate();
        //ManageStates();
    }

    public void UpdatePlayerLevelLabels()
    {
        playerLevel.text = "Level " + player.level;
        expText.text = "Exp: " + player.expPoints.ToString("0") + "/" + player.expToLevel.ToString("0");
    }

    public void UpdateTimerLabels(float mTimer, float pTimer)
    {

        player.UpdateReadyBar(pTimer);
        //playerTimerLabel.text = playerTimeText + pTimer.ToString("0.0");
    }

    void InitializeStates()
    {

        IState introState = new BattleIntro(gameObject);
        IState selectActionState = new BattleSelectAction(gameObject);
        IState selectTargetState = new BattleSelectTarget(gameObject);
        IState performMoveState = new BattlePerformMove(gameObject);
        IState swapState = new BattleSwap(gameObject);
        IState gameOverState = new BattleOver(gameObject);
        IState waveIncreaseState = new BattleIncreaseWave(gameObject);

        statemachine.AddState("Intro", introState);
        statemachine.AddState("SelectAction", selectActionState);
        statemachine.AddState("SelectTarget", selectTargetState);
        statemachine.AddState("PerformMove", performMoveState);
        statemachine.AddState("Swap", swapState);
        statemachine.AddState("GameOver", gameOverState);
        statemachine.AddState("WaveIncrease", waveIncreaseState);


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

    public void SpawnFloatText(string text, Vector3 pos)
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
