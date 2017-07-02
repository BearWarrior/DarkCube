using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public GameObject splashScreen;
    public GameObject blackBG;

    private float speed = 1;
    private float from = 1;
    private float to = 0;
    private float journeyLength = 2;
    private float startTime;

    private bool fadeIn = true;

	// Use this for initialization
	void Start ()
    {
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.anyKey)
            SceneManager.LoadScene("MainMenu");

        if (fadeIn)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            float alpha = Mathf.Lerp(from, to, fracJourney);

            blackBG.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

            if (fracJourney > 1)
            {
                fadeIn = false;
                from = 0;
                to = 1;
                startTime = Time.time;
            }
        }
        else
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            float alpha = Mathf.Lerp(from, to, fracJourney);

            blackBG.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

            if (fracJourney > 1)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
