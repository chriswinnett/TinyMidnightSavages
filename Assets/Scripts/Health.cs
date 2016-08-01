using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {
    public float currentHealth = 100;
    public float maxHealth = 100;
    public bool flashSpeedAsHealth = false;
    public float flashSpeed = 1;
    public Color flashColor = Color.red;
    public bool invulnerable = false;

    public AudioClip onDamage;
    public AudioClip onDeath;

    public List<Color> initialColors = new List<Color>();
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    float curFlash = 0;
    bool flashUp = false;
    bool updateColor = false;
    void Start()
    {
        SpriteRenderer[] rends = GetComponentsInChildren<SpriteRenderer>(true);
        foreach(SpriteRenderer renderer in rends)
        {
            renderers.Add(renderer);
            initialColors.Add(renderer.color);
        }
    }
    void Update()
    {
        if (invulnerable)
        {
            updateColor = true;
            for (int i = 0; i < renderers.Count; i++)
            {
                if (curFlash > flashSpeed)
                {
                    flashUp = false;
                }
                else if (curFlash < 0)
                {
                    flashUp = true;
                }
                if (flashUp)
                {
                    if (flashSpeedAsHealth)
                    {
                        curFlash += (1-(currentHealth/maxHealth)) * Time.deltaTime;
                    }
                    else
                    {
                        curFlash += Time.deltaTime;
                    }
                }
                else
                {
                    if (flashSpeedAsHealth)
                    {
                        curFlash -= (1 - (currentHealth / maxHealth)) * Time.deltaTime;
                    }
                    else { 
                        curFlash -= Time.deltaTime;
                    }
                }
                renderers[i].color = Color.Lerp(initialColors[i], flashColor, curFlash / flashSpeed);
            }
        }
        else
        {
            if (updateColor) // for speed purposes. no reason to cycle a for loop every frame if we can just do a bool check.
            {
                for (int i = 0; i < renderers.Count; i++)
                {
                    renderers[i].color = initialColors[i];
                    curFlash = 0;
                }
                updateColor = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (!invulnerable)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                transform.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
                PlaySound(onDeath);
            }
            else
            {
                transform.SendMessage("TakingDamage", SendMessageOptions.DontRequireReceiver);
                PlaySound(onDamage);
            }
        }
    }

    void PlaySound(AudioClip s)
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(s);
    }

}
