using UnityEngine;
using System.Collections;

public class MoveUp : MonoBehaviour
{

    public float speed = 5.0f;

    public float disableDelay = 2.0f;
    private float disableTimer = 0.0f;

    // Use this for initialization
    void Start()
    {
    }

    void OnEnable()
    {
        disableTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);

        if(Time.time > disableDelay + disableTimer)
        {
            gameObject.SetActive(false);
            disableTimer = Time.time;
        }
    }
}
