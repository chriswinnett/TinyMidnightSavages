using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour {
    public float duration = 30; //two minutes
    public float transitionTime = 10;

    public Color dayColor;
    public Color nightColor;

    public GameObject babyNormal;
    public GameObject babySavage;
    public GameObject camera;

    public Transform clockHand;

    SpriteRenderer sr;

    float curTime = 0;
    public static bool isDay = true;
    // Use this for initialization
    void Start() {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate() {
        curTime += Time.deltaTime;
        if (curTime >= duration)
        {
            isDay = !isDay;

            if (isDay)
            {
                babySavage.GetComponent<Animator>().SetBool("isTransforming", true);
            }
            else
            {
                babyNormal.GetComponent<Animator>().SetBool("isTransforming", true);
            }

            curTime -= duration;
        }

        if (isDay)
        {
            clockHand.transform.eulerAngles = new Vector3(0, 0, (-180 * (curTime / duration)));

            if (curTime > duration - transitionTime)
            {
                sr.enabled = true;
            }
            sr.color = Color.Lerp(dayColor, nightColor, (curTime - (duration - transitionTime)) / transitionTime);
        }
        else
        {
            clockHand.transform.eulerAngles = new Vector3(0, 0, 180 - (180 * (curTime / duration)));
            sr.color = Color.Lerp(nightColor, dayColor, (curTime - (duration - transitionTime)) / transitionTime);
        }
    }

    public void StopTransformation()
    {
        if (isDay)
        {
            babySavage.SetActive(false);
            babyNormal.SetActive(true);
            babyNormal.GetComponent<Health>().currentHealth = babySavage.GetComponent<Health>().currentHealth;
            babyNormal.transform.position = babySavage.transform.position;
            camera.GetComponent<SimpleCameraFollow>().objectToFollow = babyNormal;
        }
        else
        {
            babySavage.SetActive(true);
            babyNormal.SetActive(false);
            babySavage.GetComponent<Health>().currentHealth = babyNormal.GetComponent<Health>().currentHealth;
            babySavage.transform.position = babyNormal.transform.position;
            camera.GetComponent<SimpleCameraFollow>().objectToFollow = babySavage;
        }
    }
}
