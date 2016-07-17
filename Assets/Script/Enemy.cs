using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{

	// Use this for initialization
	void Start ()
    {
        PDVmax = 10;
        PDVactuel = 10;
        armureMax = 10;
        armureActuel = 10;
        enduranceMax = 3;
        enduranceActuel = 3;

        regenEndurance = false;
        timeBeforeRunningMax = 3;
        timeBeforeRunningAct = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
