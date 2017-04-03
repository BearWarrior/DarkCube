using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTeleporter : MonoBehaviour
{
    private Vector3 from;
    private Vector3 to;
    private float startTime;
    private float journeyLength;
    private float speed = 5;

    private bool moving;
    private Vector3 lookAt;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void FixedUpdate()
    {
        if (moving)
        {
            Camera.main.transform.LookAt(lookAt);
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            Camera.main.transform.position = Vector3.Lerp(from, to, fracJourney);

            if (fracJourney > 1)
            {
                moving = false;

                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<PlayerCubeFlock>().disapear();
            }
        }
    }

    public void newDest(Vector3 newDest, Vector3 lookAt)
    {
        this.lookAt = lookAt;
        from = Camera.main.transform.position;
        to = newDest;
        startTime = Time.time;
        journeyLength = Vector3.Distance(from, to);
        moving = true;
    }
}
