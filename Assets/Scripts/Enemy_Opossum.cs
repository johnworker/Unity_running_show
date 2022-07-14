using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Opossum : Enemy
{
    private Rigidbody2D rb;
    private Animator Anim;
    private Collider2D Coll;
    public LayerMask Ground;
    public Transform leftpoint, rightpoint;
    public float Speed;
    private float leftx, rightx;

    private bool Faceleft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //Anim = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();

        transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
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
            if (transform.position.x < leftpoint.position.x)
            {
                Anim.SetBool("run", true);
                transform.localScale = new Vector3(-1, 1, 1);
                Faceleft = false;
            }
        }

        else
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
            if (transform.position.x > leftpoint.position.x)
            {
                Anim.SetBool("run", true);
                transform.localScale = new Vector3(1, 1, 1);
                Faceleft = true;
            }
        }
    }




}
