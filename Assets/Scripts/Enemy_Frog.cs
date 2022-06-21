using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform leftpoint, rightpoint;
    public float Speed;

    private bool Faceleft = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (Faceleft)
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
            if(transform.position.x < leftpoint.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Faceleft = false;
            }
        }

        else
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
            if (transform.position.x > rightpoint.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Faceleft = true;
            }
        }
    }


}
