using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class Player : MonoBehaviour {
    public float speed = 6;
    public float delayDamageAfterHit = 1;

    public bool invertScale = false;

    Vector3 moveDirection;
    Rigidbody2D rb;
    Animator ani;


    bool isDead = false;
    bool isAttacking = false;
    bool isTakingDamage = false;

    private Lives lives;

    public DayNight dayNightObject;

    Vector3 initialScale;

    public static GameObject instance;

    void Start ()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lives = GameObject.Find("Lives").GetComponent<Lives>();
        instance = gameObject;
        initialScale = transform.localScale;
        Application.targetFrameRate = 20;
    }
	
	void Update () {
        if (!isDead)
        {
            if (!ani.GetBool("isTransforming"))
            {
                AttackCheck();
                Movement();
            }
            ani.SetBool("isAttacking", isAttacking);
        }
        else
        {
            // is dead x_x
        }
	}

    void Movement()
    {
        if (!isAttacking)
        {
            moveDirection.x = Input.GetAxisRaw("Horizontal") * speed;
            moveDirection.y = Input.GetAxisRaw("Vertical") * speed;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        //Direction Flip. We don't check for exact 0 so that we don't change direction when we stop.
        if (!invertScale)
        {
            if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            }
            else if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            }
        }
        else
        {
            if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            }
            else if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            }
        }

        if (Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.y) > 0) //We get the absolute so that we never get a negative canceling a positive.
        {
            ani.SetBool("isWalking", true);
            rb.position = new Vector3(rb.position.x, rb.position.y, rb.position.y) + (moveDirection * Time.deltaTime);
        }
        else
        {
            ani.SetBool("isWalking", false);
        }
    }

    void AttackCheck()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
        }
    }

    public void Die()
    {
        isDead = true;
        ani.SetBool("isDead", true);

        //TEMPORARY RESET OF LEVEL - Until we get the gui in or add lives or whatever.
        SceneManager.LoadScene(0);
    }

    public void StopAttacking()
    {
        isAttacking = false;
        ani.SetBool("isAttacking", false);
        ani.SetBool("isWalking", false);
    }

    public void TakingDamage()
    {
        GetComponent<Health>().invulnerable = true;
        isTakingDamage = true;
        //ani.SetBool("isTakingDamage", true);
        ani.SetBool("isWalking", false);
        ani.SetBool("isAttacking", false);
        isAttacking = false;
        Invoke("StopTakingDamage", delayDamageAfterHit);
    }

    public void StopTakingDamage()
    {
        isTakingDamage = false;
        //ani.SetBool("isTakingDamage", false);
        DisableInvulnerability();
    }

    void DisableInvulnerability()
    {
        GetComponent<Health>().invulnerable = false;
    }
    
    public void StopTransformation()
    {
        ani.SetBool("isTransforming", false);
        ani.SetBool("isAttacking", false);
        ani.SetBool("isWalking", false);
        isAttacking = false;
        dayNightObject.StopTransformation();
    }
}
