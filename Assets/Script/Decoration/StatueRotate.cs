using UnityEngine;
using System.Collections;

public class StatueRotate : MonoBehaviour 
{
    public GameObject circle1;
    public GameObject circle2;
    public GameObject circle3;
    public GameObject circle4; 

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        circle1.transform.Rotate(new Vector3(50 * Time.deltaTime, 50 * Time.deltaTime, 50 * Time.deltaTime), Space.World);
        circle2.transform.Rotate(new Vector3(-50 * Time.deltaTime, -50 * Time.deltaTime, -50 * Time.deltaTime), Space.World);
        circle3.transform.Rotate(new Vector3(50f * Time.deltaTime, 50f * Time.deltaTime, 50f * Time.deltaTime), Space.World);
        circle4.transform.Rotate(new Vector3(-50 * Time.deltaTime, -50 * Time.deltaTime, -50 * Time.deltaTime), Space.World);
	}
}
