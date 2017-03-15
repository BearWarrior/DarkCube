using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour, IMenuAccessible
{
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void launchMenu()
    {
        //TODO make animation if it's necessary
        GetComponent<accessMenu>().setReady(true);
    }

    public void quitMenu()
    {
        //TODO make animation if it's necessary
        GetComponent<accessMenu>().setReady(true);
    }
}
