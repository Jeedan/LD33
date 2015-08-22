using UnityEngine;
using System.Collections;

public class CooldownTimer
{

    private float timer = 0.0f;
    private float cooldown;
    private float delay = 1.0f;


    public CooldownTimer(float shootDelay)
    {
        this.delay = shootDelay;
        this.timer = Time.time;
        this.cooldown = timer + shootDelay;

    }

    public void UpdateTimers()
    {
        timer = Time.time;
        cooldown = timer + delay;
    }

    public float Delay
    {
        get { return delay; }
        set { delay = value; }
    }

    // return true if the cooldown is complete, else return false
    public bool IsReady
    {
        get
        {
            if (Time.time > cooldown)
                return true ;
            else
                return false;
        }
        private set { }
    }
}
