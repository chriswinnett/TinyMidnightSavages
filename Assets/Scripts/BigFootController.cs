using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum AttackBigFoot
{
    normal,
    puppyThrow
}

public class BigFootController : MonoBehaviour
{

    public Vector2 targetDirection;
    public Direction moveDirection = Direction.none;
    public float damage = 10f;
    public float attackRange = 1f;
    public GameObject puppyProjectile;
    public Transform puppySpawn;
    public float puppyAttackRange = 25f;
    public float puppyAttackDuration = 2f;
    public float puppyAttackHeight = 3f;
    public Vector3 attackTargetPos;
    public Vector3 startPuppyPos;
    public float speed = 3f;
    public float delayDamageAfterHit = 1f;
    public bool isActive = true;
    public bool isAttacking = false;
    public bool isPuppyThrowing = false;
    public bool isTakingDamage = false;
    public Transform shadow;
    public GameObject target;
    Animator ani;
    Rigidbody2D rb;

    float moveTimer;
    public float randomMove = 0;

    Vector3 initialScale;
    Score score;
    AttackBigFoot nextAttack = AttackBigFoot.normal;
    public float moveTimerCount = 1f;
    float curPuppyTime;
    float lastAttack;

    public AudioClip musicToPlay;

    // Use this for initialization
    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
        score = GameObject.Find("Score").GetComponent<Score>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target.activeSelf == false)
        {
            target = Camera.main.GetComponent<SimpleCameraFollow>().objectToFollow;
        }
        if (!isActive)
        {
            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                if (sr.isVisible)
                {
                    isActive = true;
                    Camera.main.GetComponent<AudioSource>().clip = musicToPlay;
                    Camera.main.GetComponent<AudioSource>().Play();
                }
            }
        }
        lastAttack += Time.deltaTime;
        if (isActive && !isAttacking && !isTakingDamage)
        {
            if (lastAttack > 2)
            {
                if (nextAttack == AttackBigFoot.normal)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
                    {
                        isPuppyThrowing = false;
                        isAttacking = true;
                    }
                }
                else if (nextAttack == AttackBigFoot.puppyThrow)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < puppyAttackRange)
                    {
                        isAttacking = true;
                        isPuppyThrowing = true;
                        ani.SetBool("hasPuppy", true);
                        attackTargetPos = target.transform.position;
                        startPuppyPos = transform.position;
                        curPuppyTime = 0;
                    }
                }
            }
            Walk();
        }
        else
        {
            if (isPuppyThrowing && isAttacking)
            {
                /*curPuppyTime += Time.deltaTime;
                transform.position = new Vector3(Mathf.Lerp(startPuppyPos.x, attackTargetPos.x, curPuppyTime / puppyAttackDuration), (Mathf.Sin((curPuppyTime / puppyAttackDuration) * Mathf.PI) * puppyAttackHeight) + Mathf.Lerp(startPuppyPos.y, attackTargetPos.y, curPuppyTime / puppyAttackDuration), transform.position.z);
                shadow.position = new Vector3(Mathf.Lerp(startPuppyPos.x, attackTargetPos.x, curPuppyTime / puppyAttackDuration), Mathf.Lerp(startPuppyPos.y, attackTargetPos.y, curPuppyTime / puppyAttackDuration), transform.position.z);
                if (curPuppyTime > puppyAttackDuration)
                {
                    curPuppyTime = 0;
                    isAttacking = false;
                    isPuppyThrowing = false;
                    ani.SetBool("isAttacking", false);
                    ani.SetBool("isPuppyThrowing", false);
                }*/
                
            }
            else
            {
                targetDirection = Vector3.zero;
                moveDirection = Direction.none;
            }
            lastAttack = 0;
        }

        if (Mathf.Abs(targetDirection.x) + Mathf.Asin(targetDirection.y) != 0)
        {
            ani.SetBool("isWalking", true);
        }
        else
        {
            ani.SetBool("isWalking", false);
        }
        ani.SetBool("isAttacking", isAttacking);
    }

    void Walk()
    {
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

    public void StopAttacking()
    {
        lastAttack = 0;
        isAttacking = false;
        ani.SetBool("hasPuppy", false);
        ani.SetBool("isAttacking", false);
        if (Random.Range((float)0, (float)1) >= .5f)
        {
            nextAttack = AttackBigFoot.normal;
        }
        else
        {
            nextAttack = AttackBigFoot.puppyThrow;
            isPuppyThrowing = true;
        }
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
        //Destroy(gameObject);
        ani.SetBool("isDead", true);
        score.score += 100;
    }

    public void SpawnPuppy()
    {
        GameObject puppy = (GameObject)Instantiate(puppyProjectile, puppySpawn.position, Quaternion.identity);
        if (targetDirection.x < 0)
        {
            puppy.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            puppy.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Credits");
    }
}
