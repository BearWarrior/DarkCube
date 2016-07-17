using UnityEngine;
using System.Collections;

public class ProjectileRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.transform.Rotate(new Vector3(0, 5*Time.deltaTime, 0));	
	}
}
