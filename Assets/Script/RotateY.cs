using UnityEngine;
using System.Collections;

public class RotateY : MonoBehaviour 
{
    public float vitesse;
    public bool world;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(world)
            this.transform.Rotate(0, vitesse * Time.deltaTime, 0, Space.World);
        else
            this.transform.Rotate(0, vitesse * Time.deltaTime, 0, Space.Self);
	}
}
