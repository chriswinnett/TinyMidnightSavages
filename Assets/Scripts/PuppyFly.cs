using UnityEngine;
using System.Collections;

public class PuppyFly : MonoBehaviour {
    public GameObject target;
    public float speed = 50;
    public float minDistance = .5f;

    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        if(target == null)
        {
            target = Player.instance;
            startPos = transform.position;
            endPos = target.transform.position;
        }
    }

    void Update()
    {
        if (target != null)
        {
            transform.position += (endPos - startPos).normalized * speed * Time.deltaTime;
        }
        if ((endPos - startPos).x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if ((endPos - startPos).x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if(Vector3.Distance(transform.position, target.transform.position) <= minDistance)
        {
            Destroy(gameObject);
        }
    }
}
