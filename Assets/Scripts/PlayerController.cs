using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // [SerializeField] �u�n�[�W�o�ӵ{���g�k�A�Y�ϬO�]���p�K�٬O�|��ܩ�e����
    private Rigidbody2D rb;
    private Animator anim;

    public Collider2D coll;
    public Collider2D DidColl;
    public Transform CellingCheck;
    public AudioSource jumpAudio,hurtAudio,cherryAudio;
    [Space]
    public float speed;
    public float JumpForce;
    [Space]
    public LayerMask ground;
    public int Cherry;
    private bool isHurt;

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
        SwitchAnim();
    }

    void Movement()//����
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedircetion = Input.GetAxisRaw("Horizontal");

        //���Ⲿ��
        if(horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(horizontalMove));
        }

        if(facedircetion != 0)
        {
            transform.localScale = new Vector3(facedircetion, 1, 1);
        }

        //������D
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.deltaTime);
            jumpAudio.Play();
            anim.SetBool("jumping", true);
        }

        Crouch();
    }

    // �����ʵe�ĪG
    void SwitchAnim()
    {
        anim.SetBool("idle", false);

        //�����ʵe�ĪG�ɥ��P�_�A�p�G�V�W���t�רS���F�A�P�ɥL�S���A�a���W���N����Ĳ�o�����ʵe
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

    // �I��Ĳ�o��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �������~
        if (collision.tag == "Collection")
        {
            cherryAudio.Play();
            Destroy(collision.gameObject);
            Cherry += 1;
            CherryNum.text = Cherry.ToString();
        }

        if (collision.tag == "DeadLine")
        {
            Invoke("Restart",2f);
        }


    }

    // �����ĤH
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
            //����
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }

            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }

        }
    }

    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CellingCheck.position,0.2f,ground)) { 
        if (Input.GetButton("Crouch"))
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

    void Restart()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
