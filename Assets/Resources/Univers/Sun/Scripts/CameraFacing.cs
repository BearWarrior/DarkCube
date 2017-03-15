using UnityEngine;
using System.Collections;

public class CameraFacing : MonoBehaviour
{
    void LateUpdate()
    {
	    transform.LookAt(Camera.main.transform,transform.up);
    }
}