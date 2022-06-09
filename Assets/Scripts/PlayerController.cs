using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedircetion = Input.GetAxisRaw("Horizontal");

        if(horizontalmove != 0)
        {
            rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);
        }

        if(facedircetion != 0)
        {
            transform.localScale = new Vector3(facedircetion, 1, 1);
        }
    }
}
