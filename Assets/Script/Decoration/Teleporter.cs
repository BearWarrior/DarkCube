using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour, IMenuAccessible
{
    public GameObject circles;
    private bool moving;
    
    private float from ;
    private float to ;
    private float startTime;
    private float journeyLength = 2.1f;
    private float speed = .4f;

    // Use this for initialization
    void Start ()
    {
        from = circles.transform.localPosition.y;
        to = from - 2.1f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (moving)
        {
            
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            circles.transform.localPosition = new Vector3(circles.transform.localPosition.x, Mathf.Lerp(from, to, fracJourney), circles.transform.localPosition.z);

            if (fracJourney > 1)
            {
                moving = false;
            }
        }
    }

    public void launchMenu()
    {
        //TODO make animation if it's necessary
        //GetComponent<accessMenu>().setReady(true);
    }

    public void quitMenu()
    {
        //TODO make animation if it's necessary
        //GetComponent<accessMenu>().setReady(true);
    }

    public void startTP()
    {
        startTime = Time.time;
        moving = true;
    }
}
