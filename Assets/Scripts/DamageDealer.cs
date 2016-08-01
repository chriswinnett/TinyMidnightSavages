using UnityEngine;
using System.Collections;

public class DamageDealer : MonoBehaviour {
    public float damage = 50;

    public bool ignorePlayer = false;
    public bool ignoreEnemy = true;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    
    void OnTriggerEnter2D(Collider2D c)
    {
        if (ignorePlayer && !ignoreEnemy)
        {
            if(c.tag != "Player")
            {
                DoDamage(c);
            }
        }else if(ignoreEnemy && !ignorePlayer)
        {
            if(c.tag != "Enemy")
            {
                DoDamage(c);
            }
        }
        else
        {
            DoDamage(c);
        }
    }

    void DoDamage(Collider2D c)
    {
        Health health = c.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
