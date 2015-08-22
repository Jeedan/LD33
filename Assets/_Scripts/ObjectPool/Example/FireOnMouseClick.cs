using UnityEngine;
using System.Collections;

public class FireOnMouseClick : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Vector3 offset;
    private float shootTimer = 0.0f;
    private float shootCooldown;
    public float shootDelay = 0.2f;

    ObjectPooler pool;
    ObjectPooler p2;

    CooldownTimer leftShootTimer;

    // Use this for initialization
    void Start()
    {
        leftShootTimer = new CooldownTimer(shootDelay);

        pool = new ObjectPooler();


        pool.InitPool("bullets", bulletPrefab, 5, true);

        pool.InitPool("bb", bulletPrefab, 5, false);

        //p2.InitPool("bb", bulletPrefab, 5, false);

        shootCooldown = shootTimer + shootDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && leftShootTimer.IsReady)
        {
            //  GameObject b = Instantiate(bulletPrefab, transform.position + (offset), Quaternion.identity) as GameObject;
            Fire();

            leftShootTimer.UpdateTimers();
            //shootTimer = Time.time;
            //shootCooldown = shootTimer + shootDelay;
        }

        if (Input.GetKey(KeyCode.A) && Time.time > shootCooldown)
        {
            Fire2();

            shootTimer = Time.time;
            shootCooldown = shootTimer + (shootDelay + Random.Range(0.0f, 0.1f));


        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("bullet count: " + pool.GetCount("bullets"));
            Debug.Log("bb count: " + pool.GetCount("bb"));
            Debug.Log("Dict count: " + pool.PoolCount());
        }
    }

    void Fire()
    {
        //GameObject obj = ObjectPooler.current.GetPooledObject();
        GameObject obj = pool.GetPooledObj("bullets");
        if (obj == null) return;

        obj.transform.position = transform.position + (offset);
        obj.transform.rotation = transform.rotation;

        obj.SetActive(true);
    }


    void Fire2()
    {
        GameObject obj = pool.GetPooledObj("bb");
        if (obj == null) return;


        obj.transform.position = transform.position + (transform.right * 2);
        obj.transform.rotation = transform.rotation;

        obj.SetActive(true);
    }
}
