using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    public List<IState> states;

    public IState currState;

    public GameObject manager; // gameObject that passes data around

    // Use this for initialization
    void Start()
    {
        manager = gameObject;
        states = new List<IState>();
    }

    public void OnUpdate()
    {
        if (currState != null)
        {
            currState.OnUpdate();
        }
    }

    public void ChangeState(IState _state)
    {
        IState prevState = currState;
        if (_state != currState && _state != null)
        {
            prevState.OnExit();
            currState = _state;
            currState.OnEnter(manager);
        }
    }

    public void AddState(IState _state)
    {
        if (!states.Contains(_state) && _state != null)
        {
            states.Add(_state);
        }
        
        if(currState == null)
        {
            currState = _state;
            currState.OnEnter(manager);
        }
    }

    public void RemoveState(IState _state)
    {
        IState prevState = _state;
        if (states.Contains(_state))
        {
            prevState.OnExit();
            states.Remove(_state);
        }
    }
}
