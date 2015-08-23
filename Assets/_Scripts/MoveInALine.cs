using UnityEngine;
using System.Collections;

public class MoveInALine : MonoBehaviour
{

    public float speed = 10.0f;
    public Vector3 direction;

    public Ability ability;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void ReachedTarget()
    {
        ability.isDone = true;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            ReachedTarget();
    }
}
