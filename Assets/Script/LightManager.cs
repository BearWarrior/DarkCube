using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public List<Light> listLight;

    private float journeyLength;
    private float startTime;
    private bool fadingIn;
    private bool fadingOut;
    private float speed;

    private float lightsOn = 2;
    private float lightsOff = .5f;

    public void fadeOutLight(float p_speed)
    {
        speed = p_speed;
        startTime = Time.time;
        fadingOut = true;
    }

    public void fadeInLight(float p_speed)
    {
        speed = p_speed;
        startTime = Time.time;
        fadingIn = true; 
    }

    void Start ()
    {
        fadingIn = false;
        fadingOut = false;
        journeyLength = lightsOn - lightsOff;
    }
	
	void Update ()
    {
		if(fadingIn)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            for (int i = 0; i < listLight.Count; i++)
                listLight[i].intensity = Mathf.Lerp(lightsOff, lightsOn, fracJourney);
            
            if (fracJourney > 1)
                fadingIn = false;
        }

        if (fadingOut)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            for (int i = 0; i < listLight.Count; i++)
                listLight[i].intensity = Mathf.Lerp(lightsOn, lightsOff, fracJourney);

            if (fracJourney > 1)
                fadingOut = false;
        }
    }
}
