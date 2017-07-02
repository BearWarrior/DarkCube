using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStellarSystem : MonoBehaviour
{
    private bool isRotating;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private float velocity;
    private float sensitivity;

	// Use this for initialization
	void Start ()
    {
        sensitivity = 0.4f;
        rotation = Vector3.zero;
        velocity = 300;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isRotating)
        {
            mouseOffset = (Input.mousePosition - mouseReference);
            rotation.y = -mouseOffset.x * sensitivity;
            transform.Rotate(rotation);
            mouseReference = Input.mousePosition;
        }
    }

    void OnMouseDown()
    {
        isRotating = true;
        mouseReference = Input.mousePosition;
        //Raycast between click on camera and world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        Vector3 mouse = hit.point - transform.position;

        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
    }

    void OnMouseUp()
    {
        isRotating = false;
        GetComponent<Rigidbody>().AddTorque(0, -1 * mouseOffset.x * Time.deltaTime * velocity, 0);
    }

    public void stopRotation()
    {
        isRotating = false;
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
    }
}
