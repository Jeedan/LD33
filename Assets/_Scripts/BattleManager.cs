using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject selectedTarget;

    public LayerMask interactMask;
    public Button attackBttn;

    public float dmg = 2.0f;

    public enum BattleStates
    {
        Intro,
        SelectAction,
        SelectTarget,
        PerformMoves,
        Swap,
        Death
    }

    public Text currStateText;
    public BattleStates currentState = BattleStates.Intro;

    public float playerHealth = 10.0f;

    public float introDelay = 2.0f;
    private float introTimer = 0.0f;

    public float performActionDelay = 2.0f;
    private float performActionTimer = 0.0f;

    public bool damageDone = false;

    // todo refactor statemachine
    public StateMachine statemachine;

    // Use this for initialization
    void Start()
    {
        selectedTarget = null;
        attackBttn.onClick.AddListener(SetStateToSelectTarget); // used to be Attack
        attackBttn.interactable = false;
        IntroState();
    }

    // Update is called once per frame
    void Update()
    {
        statemachine.OnUpdate();
        //ManageStates();
    }



    void ManageStates()
    {
        switch (currentState)
        {
            case BattleStates.Intro:
                currStateText.text = "Current State: " + "Intro";
                introTimer += Time.deltaTime;
                Debug.Log("Playing Intro: " + introTimer);
                if (introTimer >= introDelay)
                {
                    currentState = BattleStates.SelectAction;
                    introTimer = 0.0f;
                }

                break;
            case BattleStates.SelectAction:
                currStateText.text = "Current State: " + "Select Action";

                selectedTarget = null;
                attackBttn.interactable = true;
                break;
            case BattleStates.SelectTarget:
                currStateText.text = "Current State: " + "Select Target";
                if (Input.GetMouseButtonDown(0) && !selectedTarget)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactMask))
                    {
                        selectedTarget = hit.transform.gameObject;

                        attackBttn.interactable = false;
                    }
                }

                if (selectedTarget)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        PerformMove();
                        //Attack();
                    }
                }
                break;
            case BattleStates.PerformMoves:
                currStateText.text = "Current State: " + "Perform Move";
                Attack();
                break;
            case BattleStates.Swap:
                currStateText.text = "Current State: " + "Swap Turn";
                playerHealth--;
                Debug.Log("Monster deals damage to player" + playerHealth);
                currentState = BattleStates.SelectAction;
                break;
            case BattleStates.Death:
                currStateText.text = "Current State: " + "DEAD";
                break;
            default:
                break;
        }
    }

    // currently not used
    public void ActivateButtons()
    {
        if (!selectedTarget) return;

        if (selectedTarget)
        {
            attackBttn.interactable = true;
        }
    }

    public void PerformMove()
    {
        currentState = BattleStates.PerformMoves;
        DealDamage();
        currentState = BattleStates.Swap;
    }

    public void IntroState()
    {
        IState introState = new BattleIntro();
        statemachine.AddState(introState);
        statemachine.ChangeState(introState);
    }

    public void SelectActionState()
    {
        IState selectActionState = new BattleSelectAction();
        //  statemachine.AddState(selectActionState);
        statemachine.ChangeState(selectActionState);
    }

    public void SetStateToSelectTarget()
    {
        IState selectTargetState = new BattleSelectTarget();
        statemachine.ChangeState(selectTargetState);
        //currentState = BattleStates.SelectTarget;
        //Debug.Log("PRESS SPACE TO ATTACK");
        //attackBttn.interactable = false;
    }

    public void PerformMoveState()
    {
        IState performMoveState = new BattlePerformMove();
        statemachine.ChangeState(performMoveState);
    }

    public void SwapState()
    {
        IState swapState = new BattleSwap();
        statemachine.ChangeState(swapState);
    }

    public void Attack()
    {
        if (!selectedTarget)
        {
            SetStateToSelectTarget();
            //currentState = BattleStates.SelectTarget;
        }
        else
        {
            DealDamage();
            selectedTarget = null;
            attackBttn.interactable = false;
        }
    }

    public void DealDamage()
    {
        GameObject tar = selectedTarget;
        Monster mon = tar.GetComponent<Monster>();

        mon.TakeDamage(dmg);
        damageDone = true;
    }
}
