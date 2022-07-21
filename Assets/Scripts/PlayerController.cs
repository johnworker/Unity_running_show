using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // [SerializeField] 只要加上這個程式寫法，即使是設為私密還是會顯示於畫面中
    private Rigidbody2D rb;
    private Animator anim;

    public Collider2D coll;
    public Collider2D DidColl;
    public Transform CellingCheck, GroundCheck;
    //public AudioSource jumpAudio,hurtAudio,cherryAudio;
    public Joystick joystick;

    [Space]
    public float speed;
    public float JumpForce;
    [Space]
    public LayerMask ground;
    [SerializeField]
    public int Cherry,Gem;
    private bool isHurt; // 默認是false
    private bool isGround;
    private int extraJump;

    [SerializeField] private TextMeshProUGUI CherryNum;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isHurt)
        {
            Movement();
        }

        Crouch();
        SwitchAnim();
        Jump();
        isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, ground);
        //newJump();
    }

    public void Update()
    {
        CherryNum.text = Cherry.ToString();
    }

    void Movement() // 移動
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedircetion = Input.GetAxisRaw("Horizontal");

        // 手機板動量方位
        //float horizontalMove = joystick.Horizontal;  // 他的值介於 1f ～ -1f
        //float facedircetion = joystick.Horizontal;

        // 角色移動
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(horizontalMove));
        }

        //if(facedircetion != 0)
        //{
        //    transform.localScale = new Vector3(facedircetion, 1, 1);
        //}

        // 手機板左右移動設置
        if (facedircetion > 0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (facedircetion < 0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }



    }

    // 切換動畫效果
    void SwitchAnim()
    {
        //anim.SetBool("idle", false);

        //切換動畫效果時先判斷，如果向上的速度沒有了，同時他又不再地面上→就直接觸發掉落動畫
        if(rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }

        if (anim.GetBool("jumping"))
        {
            if(rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    // 碰撞觸發器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 收集物品
        if (collision.tag == "Collection")
        {
            //cherryAudio.Play();
            SoundManager.instance.CherryAudio();
            Destroy(collision.gameObject);
            Cherry += 1;
            collision.GetComponent<Animator>().Play("isGot");
            //CherryNum.text = Cherry.ToString();
        }

        if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart",2f);
        }

    }

    // 消滅敵人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") 
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(anim.GetBool("falling"))
            {
                enemy.JumpOn();

                rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }
            //受傷
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }

            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }

        }
    }

    // 角色趴下
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CellingCheck.position,0.3f,ground)) {
            if (Input.GetButton("Crouch"))

            //手機板角色趴下方法
            //if (joystick.Vertical<-0.5f)
        {
            anim.SetBool("crouching", true);
            DidColl.enabled = false;
        }

            else 
        {
            anim.SetBool("crouching", false);
            DidColl.enabled = true;
        }

       }
    }

    // 角色跳躍
    void Jump()
    {
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))

        //手機板角色跳躍方法
        //if (joystick.Vertical>0.5f && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.deltaTime);
            SoundManager.instance.JumpAudio();
            //jumpAudio.Play();
            anim.SetBool("jumping", true);
        }

    }

    /*void newJump()
    {
        if (isGround)
        {
            extraJump = 1;
        }

        if(Input.GetButtonDown("Jump") && extraJump > 0)
        {
            rb.velocity = Vector2.up * JumpForce;  //  Vector2.up 就是 new Vector2 (0,1)
            extraJump--;
            anim.SetBool("jumping", true);
        }

        if (Input.GetButtonDown("Jump") && extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * JumpForce;  //  Vector2.up 就是 new Vector2 (0,1)
            anim.SetBool("jumping", true);

        }

    }*/

    // 重置當前場景
    void Restart()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GherryCount()
    {
        Cherry += 1;
    }
}
