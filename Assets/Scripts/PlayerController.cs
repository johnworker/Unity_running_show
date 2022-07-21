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
    private bool isHurt; // �q�{�Ofalse
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

    void Movement() // ����
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedircetion = Input.GetAxisRaw("Horizontal");

        // ����O�ʶq���
        //float horizontalMove = joystick.Horizontal;  // �L���Ȥ��� 1f �� -1f
        //float facedircetion = joystick.Horizontal;

        // ���Ⲿ��
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(horizontalMove));
        }

        //if(facedircetion != 0)
        //{
        //    transform.localScale = new Vector3(facedircetion, 1, 1);
        //}

        // ����O���k���ʳ]�m
        if (facedircetion > 0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (facedircetion < 0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }



    }

    // �����ʵe�ĪG
    void SwitchAnim()
    {
        //anim.SetBool("idle", false);

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

    // ����w�U
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CellingCheck.position,0.3f,ground)) {
            if (Input.GetButton("Crouch"))

            //����O����w�U��k
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

    // ������D
    void Jump()
    {
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))

        //����O������D��k
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
            rb.velocity = Vector2.up * JumpForce;  //  Vector2.up �N�O new Vector2 (0,1)
            extraJump--;
            anim.SetBool("jumping", true);
        }

        if (Input.GetButtonDown("Jump") && extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * JumpForce;  //  Vector2.up �N�O new Vector2 (0,1)
            anim.SetBool("jumping", true);

        }

    }*/

    // ���m��e����
    void Restart()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GherryCount()
    {
        Cherry += 1;
    }
}
