using UnityEngine;
using System.Collections;

public class PlaneRotate : MonoBehaviour {

    public int speed = 1;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
