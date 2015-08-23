using UnityEngine;
using System.Collections;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPooler objectPool;

    // Use this for initialization
    void Awake()
    {
        objectPool = new ObjectPooler();
    }
}
