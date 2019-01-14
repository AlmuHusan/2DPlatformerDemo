using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private LayerMask ground = 0;
    [SerializeField] private float jumpLength=20;
    [SerializeField] private float jumpHeight=20;
    //[SerializeField] private Animator anim;

    private bool facingLeft = true;
    private float distToGround;
    private Collider2D coll;
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        distToGround = coll.bounds.extents.y;
    }

    


    void Update()
    {
        if (anim.GetBool("Jumping") && rb.velocity.y < .1f)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", true);
        }
        if (IsGrounded() && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }
    }

    private void Movement()
    {
        if (facingLeft)
        {
            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector2(1, 1);
            }
            if (transform.position.x > leftCap)
            {
                if (IsGrounded())
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        if (!facingLeft)
        {
            if (transform.localScale.x != -1)
            {
                transform.localScale = new Vector2(-1, 1);
            }

            if (transform.position.x < rightCap)
            {
                if (IsGrounded())
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }

        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, distToGround + .2f, ground);
    }




}
