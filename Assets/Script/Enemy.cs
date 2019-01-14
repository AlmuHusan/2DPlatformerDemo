using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource death;
    protected Rigidbody2D rb;
    protected virtual void Start()
    
    {
        anim = GetComponent<Animator>();
        death = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void JumpOn()
    {
        anim.SetTrigger("Death");
        death.Play();
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
    }
    private void Death()
    {
        Destroy(this.gameObject);
    }
}
