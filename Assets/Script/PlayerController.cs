using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask ground=0;
    [SerializeField] private int speed = 7;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private Text cherryText;
    [SerializeField] private int cherries = 0;
    [SerializeField] private float damageForce = 100f;
    [SerializeField] private AudioSource cherrySFX;
    private AudioSource footstep;
    private Rigidbody2D rb;
    private float distToGround;
    private CapsuleCollider2D coll;

    private bool canJump = true;
    private int jumps = 0;

    private enum State { idle, running, jumping,falling,hurt }
    State state = State.idle;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        footstep = GetComponent<AudioSource>();
        distToGround =coll.bounds.extents.y;
    }


    void FixedUpdate()
    {
        if (state != State.hurt)
        {
            InputManager();
        }
        VelocityState();
        anim.SetInteger("State", (int)state);
    }

    private void InputManager()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }
        if (IsGrounded())
        {
            canJump = true;
            jumps = 0;
        }

        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;
            ++jumps;
            if (jumps >= 2)
            {
                canJump = false;
            }

        }
    }

    private void VelocityState()
    {
        if (rb.velocity.y > 10f)
        {
            state = State.jumping;
        }

        else if (!IsGrounded())
            {
                state = State.falling;
            }
        
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > .5f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherrySFX.Play();
            Destroy(collision.gameObject);
            cherries++;
            cherryText.text = "";
            cherryText.text= "X "+cherries.ToString();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {   if (state == State.falling)
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.JumpOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                if(collision.transform.position.x<transform.position.x )
                {
                    rb.velocity = new Vector2(damageForce,0);
                }
                else if(collision.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-damageForce, 0);
                }
            }
        }
    }
    private bool IsGrounded(){
        return Physics2D.Raycast(transform.position, Vector2.down, distToGround + .75f,ground);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }
    private void Footstep()
    {
        if (IsGrounded())
        {
            footstep.Play();
        }
    }
}
