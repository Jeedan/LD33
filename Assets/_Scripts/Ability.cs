using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour
{

    public GameObject effectPrefab;

    public GameObject target;

    public bool isDone = false;
    private ObjectPooler pool;

    public void SetTarget(GameObject tar)
    {
        target = tar;
    }

    public void SpawnEffect()
    {
        pool = ObjectPoolManager.objectPool;
        isDone = false;
        //GameObject eff = Instantiate(effectPrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject eff = pool.GetPooledObj("Fireball");

        eff.transform.position = transform.position;
        eff.transform.rotation = Quaternion.identity;

        eff.SetActive(true);
        Vector3 dir = target.transform.position - transform.position;
        MoveInALine m = eff.GetComponent<MoveInALine>();
        m.direction = dir.normalized;
        m.ability = this;

    }
}
