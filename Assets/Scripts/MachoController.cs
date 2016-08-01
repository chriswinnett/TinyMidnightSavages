using UnityEngine;
using System.Collections;

public enum Direction
{
    up,
    down,
    left,
    right,
    none
}

public enum Attack
{
    normal,
    elbowDrop
}


public class MachoController : MonoBehaviour
{
    public Vector2 targetDirection;
    public Direction moveDirection = Direction.none;
    public float damage = 10;
    public float attackRange = 1;
    public float elbowAttackRange = 5;
    public float elbowAttackDuration = 2;
    public float elbowAttackHeight = 3;
    public Vector3 attackTargetPos;
    public Vector3 startElbowPos;
    public float speed = 3;
    public float delayDamageAfterHit = 1f;
    public bool isActive = true;
    public bool isAttacking = false;
    public bool isElbowDropping = false;
    public bool isTakingDamage = false;
    public bool isJumping = false;
    public Transform shadow;
    public GameObject target;
    public AudioClip ohYeah;
    public AudioClip ohNo;
    Animator ani;
    Rigidbody2D rb;

    float moveTimer;
    public float randomMove = 0;

    Vector3 initialScale;
    Score score;
    public Attack nextAttack = Attack.normal;
    public float moveTimerCount = 1;
    float curElbowTime;
    float lastAttack;
    AudioSource aSource;

    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
        score = GameObject.Find("Score").GetComponent<Score>();
        aSource = GetComponent<AudioSource>();
        aSource.pitch = Random.Range(.8f, 1.2f);
    }

    void Update()
    {
        target = Player.instance; //update every frame in case the player object changes. Will need to be changed if we somehow add multiplayer.
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
        if (isActive && !isAttacking && !isTakingDamage && target != null)
        {
            if (lastAttack > 2)
            {
                if (nextAttack == Attack.normal)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
                    {
                        isElbowDropping = false;
                        isAttacking = true;
                    }
                }
                else if (nextAttack == Attack.elbowDrop)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < elbowAttackRange)
                    {
                        isAttacking = true;
                        isElbowDropping = true;
                        aSource.PlayOneShot(ohYeah);
                        ani.SetBool("isElbowDropping", true);
                        if (!isJumping)
                        {
                            attackTargetPos = target.transform.position;
                            startElbowPos = transform.position;
                        }
                        curElbowTime = 0;
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
        else
        {
            if (isElbowDropping && isAttacking)
            {
                curElbowTime += Time.deltaTime;
                transform.position = new Vector3(Mathf.Lerp(startElbowPos.x, attackTargetPos.x, curElbowTime / elbowAttackDuration), (Mathf.Sin((curElbowTime / elbowAttackDuration) * Mathf.PI) * elbowAttackHeight) + Mathf.Lerp(startElbowPos.y, attackTargetPos.y, curElbowTime / elbowAttackDuration), transform.position.z);
                shadow.position = new Vector3(Mathf.Lerp(startElbowPos.x, attackTargetPos.x, curElbowTime / elbowAttackDuration), Mathf.Lerp(startElbowPos.y, attackTargetPos.y, curElbowTime / elbowAttackDuration), transform.position.z);
                if (curElbowTime > elbowAttackDuration)
                {
                    curElbowTime = 0;
                    isAttacking = false;
                    isElbowDropping = false;
                    isJumping = false;
                    ani.SetBool("isAttacking", false);
                    ani.SetBool("isElbowDropping", false);
                }
            }
            else
            {
                targetDirection = Vector3.zero;
                moveDirection = Direction.none;
            }
            lastAttack = 0;
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
        if (rRange >= 50f)
        {
            nextAttack = Attack.normal;
        }
        else
        {
            nextAttack = Attack.elbowDrop;
        }

        isAttacking = false;
        ani.SetBool("isAttacking", false);
        ani.SetBool("isElbowDropping", false);
        isJumping = false;

    }

    public void TakingDamage()
    {
        GetComponent<Health>().invulnerable = true;
        isTakingDamage = true;
        ani.SetBool("isTakingDamage", true);
        ani.SetBool("isWalking", false);
        ani.SetBool("isAttacking", false);
    }

    public void StopTakingDamage()
    {
        Invoke("DisableInvulnerability", delayDamageAfterHit);
        isTakingDamage = false;
        ani.SetBool("isTakingDamage", false);
    }

    void DisableInvulnerability()
    {
        GetComponent<Health>().invulnerable = false;
    }

    public void SendDamage()
    {
        /*if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
        {
            target.GetComponent<Health>().TakeDamage(damage);
        }*/
    }
    public void Die()
    {
        Destroy(gameObject);
        score.score += 100;
    }

    public void JumpDown(Vector3 pos)
    {
        if (!isJumping && !isAttacking)
        {
            Debug.Log(pos);
            startElbowPos = transform.position;
            attackTargetPos = pos;
            nextAttack = Attack.elbowDrop;
            lastAttack = 10;
            isJumping = true;
            isElbowDropping = true;
            isAttacking = true;
            ani.SetBool("isElbowDropping", true);
            Debug.Log("Should have jumped: " + nextAttack);
        }
    }
}
