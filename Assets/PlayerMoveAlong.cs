using UnityEngine;
using System.Collections;

public class PlayerMoveAlong : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.SetParent(this.transform);
        }
        Debug.Log("Enter");
    }

    void OnCollisionExit(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.SetParent(null);
        }
        Debug.Log("Exit");
    }
}
