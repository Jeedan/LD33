using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Experience : MonoBehaviour
{
    public int level = 1;
    public float experience = 0.0f;

    public float experienceToNextLevel = 100.0f;
    public Text expText;
    public Text lvlText;


    public float levelDelay = 1.0f;
    private float levelTimer = 0.0f;
    private bool levelUP = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (levelUP)
        {
            lvlText.text = "LEVEL UP!";
            levelTimer += Time.deltaTime;

            if(levelTimer >=  levelDelay)
            {
                lvlText.text = "Level " + level;
                expText.text = "Exp: " + experience + " / " + experienceToNextLevel;
                levelTimer = 0.0f;
                levelUP = false;
            }
        }
    }

    public void AwardExp(float amount)
    {
        experience += amount;
        expText.text = "Exp: " + experience + " / " + experienceToNextLevel;
        if (experience >= experienceToNextLevel)
        {
            level++;
            experienceToNextLevel *= 2;
            levelUP = true;
        }
    }
}
