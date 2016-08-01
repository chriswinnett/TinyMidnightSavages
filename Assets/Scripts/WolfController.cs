using UnityEngine;
using System.Collections;

public enum WolfAttack
{
    normal
}

public class WolfController : MonoBehaviour {

    public Vector2 targetDirection;
    public Direction moveDirection = Direction.none;
    public float damage = 10f;
    public float attackRange = 1f;
    public float biteAttackRange = 5f;
    public float biteAttackDuration = 2f;
    public float biteAttackHeight = 3f;
    public Vector3 attackTargetPos;
    public float speed = 5f;
    public float delayDamageAfterHit = 1f;
    public bool isActive = true;
    public bool isAttacking = false;
    public bool isTakingDamage = false;
    public bool isRunningAway = false;
    public Transform shadow;
    public GameObject target;
    public AudioClip whine;
    public AudioClip snarl;
    Animator ani;
    Rigidbody2D rb;

    float moveTimer;
    public float randomMove = 0;

    Vector3 initialScale;
    Score score;
    public WolfAttack nextAttack = WolfAttack.normal;
    public float moveTimerCount = 1;
    float lastAttack;
    AudioSource aSource;


	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
        score = GameObject.Find("Score").GetComponent<Score>();
        aSource = GetComponent<AudioSource>();
        aSource.pitch = Random.Range(.8f, 1.2f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        target = Player.instance;
        if (!isActive)
        {
            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                if (sr.isVisible)
                {
                    isActive = true;
                }
            }
        }
        lastAttack += Time.deltaTime;
        if (isActive && !isAttacking && !isTakingDamage && !isRunningAway && target != null)
        {
            if (lastAttack > 2)
            {
                if (nextAttack == WolfAttack.normal)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
                    {
                        isAttacking = true;
                        aSource.PlayOneShot(snarl);
                    }
                }
            }
            moveTimer += Time.deltaTime;
            if (moveTimer >= moveTimerCount)
            {
                targetDirection = (target.transform.position - transform.position).normalized;
                if (targetDirection.x < 0)
                {
                    transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
                }
                else if (targetDirection.x > 0)
                {
                    transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
                }
                if (targetDirection.x < 0)
                {
                    moveDirection = Direction.left;
                }
                else if (targetDirection.x > 0)
                {
                    moveDirection = Direction.right;
                }

                if (Mathf.Abs(targetDirection.x) <= Mathf.Abs(targetDirection.y))
                {
                    if (targetDirection.y < 0)
                    {
                        moveDirection = Direction.down;
                    }
                    else if (targetDirection.y > 0)
                    {
                        moveDirection = Direction.up;
                    }
                }

                if (targetDirection.x == 0 && targetDirection.y == 0)
                {
                    moveDirection = Direction.none;
                }
                moveTimer = Random.Range(0, randomMove);
            }
            if (moveDirection == Direction.left)
                rb.position += Vector2.left * speed * Time.deltaTime;
            if (moveDirection == Direction.right)
                rb.position += Vector2.right * speed * Time.deltaTime;
            if (moveDirection == Direction.up)
                rb.position += Vector2.up * speed * Time.deltaTime;
            if (moveDirection == Direction.down)
                rb.position += Vector2.down * speed * Time.deltaTime;
        }
        else if (isRunningAway)
        {
            if (targetDirection.x < 0)
            {
                transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            }
            else if (targetDirection.x > 0)
            {
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            }
            if (targetDirection.x < 0)
            {
                moveDirection = Direction.right;
            }
            else if (targetDirection.x > 0)
            {
                moveDirection = Direction.left;
            }
            if (moveDirection == Direction.left)
                rb.position += Vector2.left * speed * Time.deltaTime;
            if (moveDirection == Direction.right)
                rb.position += Vector2.right * speed * Time.deltaTime;
        }
        else
        {
            targetDirection = Vector3.zero;
        }

        if (Mathf.Abs(targetDirection.x) + Mathf.Abs(targetDirection.y) != 0) //We get the absolute so that we never get a negative canceling a positive.
        {
            ani.SetBool("isWalking", true);
        }
        else
        {
            ani.SetBool("isWalking", false);
        }
        ani.SetBool("isAttacking", isAttacking);
    }

    public void StopAttacking()
    {
        float rRange = Random.Range(0f, 100f);
        if (rRange >= 30f)
        {
            nextAttack = WolfAttack.normal;
        }

        isAttacking = false;
        ani.SetBool("isAttacking", false);
    }

    public void TakingDamage()
    {
        GetComponent<Health>().invulnerable = true;
        isTakingDamage = true;
        ani.SetBool("isWalking", false);
        ani.SetBool("isAttacking", false);
    }

    public void StopTakingDamage()
    {
        Invoke("DisableInvulnerability", delayDamageAfterHit);
        isTakingDamage = false;
        ani.SetBool("isTakingDamage", false);
    }

    public void DisableInvulnerability()
    {
        GetComponent<Health>().invulnerable = false;
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
    public void Die()
    {
        isRunningAway = true;
        aSource.PlayOneShot(whine);
        Invoke("RemoveObject", 5f);
        foreach(BoxCollider2D thisbox in GetComponents<BoxCollider2D>())
        {
            thisbox.enabled = false;
        }
        score.score += 100;
    }

}
