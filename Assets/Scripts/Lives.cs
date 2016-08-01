using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Lives : MonoBehaviour {

    public int lives = 3;

    private Text textLives;

    void Start () {
        textLives = GameObject.Find("Lives").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        textLives.text = "Lives: " + lives;
	}

    public void LivesAdd()
    {
        lives++;
    }

    public void LivesSubtract()
    {
        if(lives > 0)
        {
            lives--;
        }
        else
        {
            // Game Over
            SceneManager.LoadScene("Start Screen");
        }
    }
}
