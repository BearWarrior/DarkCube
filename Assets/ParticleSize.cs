using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSize : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        
	    for(int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).GetComponent<ParticleSystem>().startSize *= 2;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
