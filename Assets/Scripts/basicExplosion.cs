using UnityEngine;
using System.Collections;

public class basicExplosion : MonoBehaviour {
    float timedExplosion = 5;
	// Use this for initialization
	void Start () {
        InvokeRepeating("Explode", 5, 5);
	}
	
	void Explode()
    {
        Rigidbody2D[] objs = FindObjectsOfType<Rigidbody2D>();
        foreach(Rigidbody2D rb in objs)
        {
            if(Vector3.Distance(rb.transform.position, transform.position) <= 5)
            {
                Debug.Log("Found object " + rb.name + ", Force: " + ((rb.transform.position - transform.position) * 1000));
                rb.AddForce((rb.transform.position - transform.position)*1000);
            }
        }
        Debug.Log("Exploded");
	}
}
