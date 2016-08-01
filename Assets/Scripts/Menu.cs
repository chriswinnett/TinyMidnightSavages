using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {
    public Canvas menuMain;
    public Canvas menuSettings;
    public Camera mainCamera;
    public Slider sliderVolume;
    public string destination = "";

    Color[] colors = new Color[5];
	// Use this for initialization
	void Start () {
        sliderVolume.value = PlayerPrefs.GetFloat("Music Volume", 1);
        Camera.main.GetComponent<AudioSource>().volume = sliderVolume.value;
    }

    // Update is called once per frame
    void Update () {


    }

    public void LoadScene()
    {
        SceneManager.LoadScene(destination);
    }

    public void ShowSettings()
    {
        menuMain.gameObject.SetActive(false);
        menuSettings.gameObject.SetActive(true);
    }

    public void HideSettings()
    {
        menuSettings.gameObject.SetActive(false);
        menuMain.gameObject.SetActive(true);
    }

    public void ChangeVolume()
    {
        mainCamera.GetComponent<AudioSource>().volume = sliderVolume.value;
        PlayerPrefs.SetFloat("Music Volume", sliderVolume.value);
    }

    public void OpenBrowser(string url)
    {
        Application.OpenURL(url);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
