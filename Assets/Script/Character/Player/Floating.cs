using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public List<GameObject> listCanvas;
    private float initYPos;
    private float amplitude = 0.1f;
    private float speed = 1;

	// Use this for initialization
	void Start ()
    {

        // Save the y position prior to start floating (maybe in the Start function):

        initYPos = transform.localPosition.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Put the floating movement in the Update function:

        transform.localPosition = new Vector3(transform.localPosition.x, initYPos + amplitude * Mathf.Sin(speed * Time.time), transform.localPosition.z); ;
    }
}
