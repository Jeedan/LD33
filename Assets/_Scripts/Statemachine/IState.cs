using UnityEngine;
using System.Collections;

public interface IState
{
    void OnEnter(GameObject go);
    void OnExit();
    void OnUpdate();
}
