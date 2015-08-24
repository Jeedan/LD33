using UnityEngine;
using System.Collections;

public class MoveInALine : MonoBehaviour
{

    public float speed = 10.0f;
    public Vector3 direction;

    public Ability ability;
    TrailRenderer trail;
    void Start()
    {

        trail = gameObject.GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void ReachedTarget()
    {
        TrailRenderer trail = gameObject.GetComponentInChildren<TrailRenderer>();
        trail.time = 0.25f;
        ability.isDone = true;
        trail.time = 0.8f;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            ReachedTarget();
    }
}
