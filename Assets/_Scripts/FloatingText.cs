using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    public Vector3 direction = new Vector3(0.0f, 1.0f, 0.0f);
    public float speed = 2.0f;

    private float aliveTimer = 0.0f;
    public float aliveDelay = 2.0f;

    void OnEnable()
    {
        aliveTimer = 0.0f;
    }

    void OnDisable()
    {
        aliveTimer = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += (direction.normalized * speed) * Time.deltaTime;

        aliveTimer += Time.deltaTime;

        if(aliveTimer >= aliveDelay)
        {
            gameObject.SetActive(false);
        }
    }
}
