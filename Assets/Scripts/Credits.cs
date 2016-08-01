using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Credits : MonoBehaviour {

    public Canvas credits;
    public Text textLogo;
    public Text textLogoShadow;
    public string destination = "";

    Color[] colors = new Color[5];

	// Use this for initialization
	void Start () {
        colors[0] = Color.cyan;
        colors[1] = Color.red;
        colors[2] = Color.green;
        colors[3] = Color.yellow;
        colors[4] = Color.magenta;

    }

    // Update is called once per frame
    void Update () {
        textLogo.color = colors[Random.Range(0, colors.Length)];
        textLogoShadow.color = colors[Random.Range(0, colors.Length)];
    }
}
