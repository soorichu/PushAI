using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetect : MonoBehaviour
{
    public PushAgent agent;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("goal"))
        {
            agent.ScoredAGoal();
        }
    }
}
