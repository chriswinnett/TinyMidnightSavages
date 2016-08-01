using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

    public int score = 0;

    private Text textScore;

    void Start ()
    {
        textScore = GameObject.Find("Score").GetComponent<Text>();
    }

	// Update is called once per frame
	void Update ()
    {
        // Set the score
        textScore.text = "Score: " + score;
	}
}
