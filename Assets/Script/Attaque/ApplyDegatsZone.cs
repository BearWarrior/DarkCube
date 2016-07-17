using UnityEngine;
using System.Collections;

public class ApplyDegatsZone : MonoBehaviour
{
    float degats;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log(other.name);
        }
    }
}
