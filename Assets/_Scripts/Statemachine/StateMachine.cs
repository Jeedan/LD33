using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
    [SerializeField]
   // public List<IState> statesList = new List<IState>();
    public Stack<IState> statesStack = new Stack<IState>();
    public Dictionary<string, IState> mStates = new Dictionary<string, IState>();

    public IState currState;

    // Use this for initialization
    public StateMachine()
    {
    }

    public IState currentState()
    {
        if (statesStack.Count > 0)
        {
            currState = statesStack.Peek();
            return currState;
        }

        return null;
    }

    public void OnUpdate()
    {

        currentState().OnUpdate();
    }

    public void ChangeState(string name)
    {
        IState newState = mStates[name];
        if (newState != null && newState != currentState())
        {
            PushState(name);
        }
        else
        {
            Debug.Log(currentState().ToString() + " " + name);
            Debug.Log("dict count: " + mStates.Count);
            Debug.LogError("State does not exist, or we are already in the state");
        }
    }

    public void ChangeState(IState _state)
    {
        IState prevState = currState;
        if (_state != currState && _state != null)
        {
            prevState.OnExit();
            currState = _state;
            currState.OnEnter();
        }
    }

    public void PushState(IState _state)
    {
        IState state = _state;
        if (state != null && state != currState)
        {
            statesStack.Push(state);
        }
    }

    public void PushState(string name)
    {
        IState state = mStates[name];
        IState prevState = currentState();

        if (state != null &&  state != currentState())
        {
            if (statesStack.Count > 0)
            {
                //statesStack.Pop().OnExit();
                prevState.OnExit();
            }

            statesStack.Push(state);
            currentState().OnEnter();
        }
    }

    public void Pop()
    {
        IState popState = PopState();

        Debug.Log("dict count: " + mStates.Count);
        popState.OnExit();
        currentState().OnEnter();
    }

    public IState PopState()
    {
        if (statesStack.Count > 0)
            return statesStack.Pop();
        else
            return null;
    }

    public void AddState(string name, IState state)
    {
        if (!mStates.ContainsKey(name) && state != null)
        {
            mStates.Add(name, state);
        }
    }

    //public void AddState(IState _state)
    //{
    //    if (!statesList.Contains(_state) && _state != null)
    //    {
    //        statesList.Add(_state);
    //    }

    //    if (currState == null)
    //    {
    //        currState = _state;
    //        currState.OnEnter();
    //    }
    //}

    //public void RemoveState(IState _state)
    //{
    //    IState prevState = _state;
    //    if (statesList.Contains(_state))
    //    {
    //        prevState.OnExit();
    //        statesList.Remove(_state);
    //    }
    //}
}
