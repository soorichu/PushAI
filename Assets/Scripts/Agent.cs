using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    float horizontal;
    float vertical;
    public float speed = 10f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        transform.position += (new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime);
    }
}
