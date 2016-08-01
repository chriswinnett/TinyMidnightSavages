using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {

    public float healthBonus = 25f;
    public AudioClip collect;
	
	void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            Health playerHealth = other.GetComponent<Health>();

            playerHealth.currentHealth += healthBonus;
            playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth, 0f, 100f);

            Camera.main.GetComponent<AudioSource>().PlayOneShot(collect);

            Destroy(gameObject);
        }
    }
}
