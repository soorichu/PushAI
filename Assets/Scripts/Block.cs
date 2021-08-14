using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool scored = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("goal"))
        {
            if (!scored)
            {
                scored = true;
                GameManager.singlton.AddScore();
                scored = false;
            }
        }
    }
}
