using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singlton;

    public Text scoreText;
    int score = 0;

    public GameObject goal;
    int goalState;

    public GameObject agent;
    public GameObject box;

    void Awake()
    {
        if(GameManager.singlton == null)
        {
            GameManager.singlton = this;
        }
        else
        {
            Debug.Log("singlton instance error");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetGoal();
    }

    void Update()
    {
        
    }

    public void AddScore()
    {
        scoreText.text = "score : " + (++score);
        ResetGoal();
    }

    void ResetGoal()
    {
        goalState = Random.Range(0, 4);

        float plusminus = (float)(goalState % 2 == 0 ? 8 : -8);
        // groundState = 0 : +Z, groundState = 1 : -Z
        if (goalState < 3)
        {
            goal.transform.position = new Vector3(0, 0, plusminus);
            goal.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        // groundState = 2 : +X, groundstate = 3 : -X
        else
        {
            goal.transform.position = new Vector3(plusminus, 0, 0);
            goal.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        agent.transform.position = new Vector3(-2f, 0.7f, 0f);
        box.transform.position = new Vector3(2f, 0.7f, 0f);
    }
}
