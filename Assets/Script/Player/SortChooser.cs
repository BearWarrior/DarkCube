using UnityEngine;
using System.Collections;

public class SortChooser : MonoBehaviour 
{
    public int cubeFace;
    private GameObject armature;
   
    private Quaternion f1;
    private Quaternion f2;
    private Quaternion f3;
    private Quaternion f4;
    private Quaternion f5;
    private Quaternion f6;

    private Quaternion angleToGo;

    public bool rotationning;

    // Use this for initialization
    void Start () 
    {
        rotationning = false;

        cubeFace = 1;
        armature = GameObject.FindWithTag("Armature");
		
        f1 = Quaternion.Euler (new Vector3(0, 0, 0));
        f2 = Quaternion.Euler (new Vector3(0, 90, 0));
        f3 = Quaternion.Euler (new Vector3(0, 180, 0));
        f4 = Quaternion.Euler (new Vector3(0, 270, 0));
        f5 = Quaternion.Euler (new Vector3(0, 0, -90));
        f6 = Quaternion.Euler (new Vector3(0, 0, 90));

        angleToGo = f1;
    }

	// Update is called once per frame
	void Update () {

        GoToFace(angleToGo);

        int oldCubeFace = cubeFace;

        if (Input.GetKeyDown (KeyCode.Alpha1)) {
			cubeFace = 1;
            rotationning = true;
            angleToGo = f1;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			cubeFace = 2;
            rotationning = true;
            angleToGo = f2;
        }
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			cubeFace = 3;
            rotationning = true;
            angleToGo = f3;
        }
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			cubeFace = 4;
            rotationning = true;
            angleToGo = f4;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			cubeFace = 5;
            rotationning = true;
            angleToGo = f5;
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			cubeFace = 6;
            rotationning = true;
            angleToGo = f6;
		}

        if (oldCubeFace != cubeFace)
            GetComponent<Player>().cubeFaceChanged(cubeFace);
    }

    private void GoToFace(Quaternion f)
    {
        //Time.deltaTime // temps par frame
        if (rotationning)
        {
            armature.transform.localRotation = Quaternion.Slerp(armature.transform.localRotation, f, Time.deltaTime * 10);

            if (Quaternion.Angle(armature.transform.localRotation, f) < 1)
            {
                armature.transform.localRotation = f;
                rotationning = false;
            }
        }
    
    }

    public bool isRotationning()
    {
        return rotationning;
    }

    public Quaternion getRotation()
    {
        return angleToGo;
    }
		
}
