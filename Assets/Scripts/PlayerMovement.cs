using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float bounceForce;

    public Transform foot1;
    public Transform foot2;

    private float horizontal;
    private bool isGrounded;
    private bool isJumping=false;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        
    }

    void Update()
    {
        Inputs();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        VisualsPlayer();
    }



    void Inputs()
    {
        horizontal = Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true; 
        }

    }

    void MovePlayer()
    {
        if (TimeManager.isRewinding)
        {
            return;
        }
        isGrounded = Physics2D.OverlapArea(foot1.position, foot2.position);
        animator.SetBool("isgrounded", isGrounded);

        Vector3 velocity = new Vector3(horizontal, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocity, ref velocity, 0.05f);

        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            animator.SetTrigger("jump");
            isJumping = false;
        }
    }
    private void VisualsPlayer()
    {
        if (rb.velocity.x>0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }


        animator.SetFloat("speed", Mathf.Abs( horizontal));

    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!TimeManager.isRewinding && collision.GetComponent<BulletManager>())
        {
            Debug.Log("JumpBullet");

            rb.velocity = new Vector2(0, bounceForce);
            //rb.AddForce(new Vector2(0f, jumpForce*3));
            animator.SetTrigger("jump");
        }
        

    }


}
