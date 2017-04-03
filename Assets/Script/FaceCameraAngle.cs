using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraAngle : MonoBehaviour
{
    public float offsetAngleY;

	// Use this for initialization
	void Start ()
    {

	}
    
    void LateUpdate()
    {
        Vector3 oldRot = gameObject.transform.eulerAngles;
        transform.LookAt(Camera.main.transform);
        transform.eulerAngles = new Vector3(oldRot.x, transform.eulerAngles.y, oldRot.z);
        transform.Rotate(new Vector3(0, offsetAngleY, 0));
    }
}
