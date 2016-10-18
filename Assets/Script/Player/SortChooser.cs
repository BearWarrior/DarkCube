using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private Quaternion fdefault;

    private Quaternion angleToGo;

    public bool rotationning;

    public List<List<GameObject>> horizFaces = new List<List<GameObject>>();

    // Use this for initialization
    void Start()
    {
        rotationning = false;

        cubeFace = 1;
        armature = GameObject.FindWithTag("Armature");

        f1 = Quaternion.Euler(new Vector3(0, 0, 0));
        f2 = Quaternion.Euler(new Vector3(0, 90, 0));
        f3 = Quaternion.Euler(new Vector3(0, 180, 0));
        f4 = Quaternion.Euler(new Vector3(0, 270, 0));
        f5 = Quaternion.Euler(new Vector3(90, 0, 0));
        f6 = Quaternion.Euler(new Vector3(-90, 0, 0));
        fdefault = Quaternion.Euler(new Vector3(0, 0, 0));

        angleToGo = f1;
    }

    private void GoToFace(Quaternion f)
    {
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

    public void Update()
    {
        GoToFace(angleToGo);

        int oldCubeFace = cubeFace;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cubeFace = 1;
            rotationning = true;
            angleToGo = fdefault;
            StopAllCoroutines();
            StartCoroutine(coroutineRotationHorizAll(f1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cubeFace = 2;
            rotationning = true;
            angleToGo = fdefault;
            StopAllCoroutines();
            StartCoroutine(coroutineRotationHorizAll(f2));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            cubeFace = 3;
            rotationning = true;
            angleToGo = fdefault;
            StopAllCoroutines();
            StartCoroutine(coroutineRotationHorizAll(f3));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            cubeFace = 4;
            rotationning = true;
            angleToGo = fdefault;
            StopAllCoroutines();
            StartCoroutine(coroutineRotationHorizAll(f4));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            cubeFace = 5;
            rotationning = true;
            angleToGo = f5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            cubeFace = 6;
            rotationning = true;
            angleToGo = f6;
        }

        /*if (oldCubeFace != cubeFace)
            GetComponent<Player>().cubeFaceChanged(cubeFace);*/
    }

    public IEnumerator coroutineRotationHorizAll(Quaternion f)
    {
        for (int i = 0; i < 6; i++)
        {
            StartCoroutine(coroutineRotationHoriz(f, i));
            yield return new WaitForSeconds(0.05f);
        }
    }
    public IEnumerator coroutineRotationHoriz(Quaternion f, int numFace)
    {
        Vector3 center;
        float angle = (f.eulerAngles.y - horizFaces[numFace][0].transform.localEulerAngles.y);
        if (angle != 0)
        {
            if (angle > 180) //Pour avoir un angle entre -180 et 180
                angle -= 360;
            else if (angle < -180)
                angle += 360;
            float value = Mathf.Abs(angle); //Decompte
            float vitesse = Mathf.Abs(angle) * 7;

            while (value > 0)
            {
                center = getcenter(horizFaces[numFace]);
                for (int i = 0; i < horizFaces[numFace].Count; i++)
                    horizFaces[numFace][i].transform.RotateAround(center, horizFaces[numFace][i].transform.up, angle / Mathf.Abs(angle) * vitesse * Time.deltaTime);
                yield return new WaitForEndOfFrame();
                value -= vitesse * Time.deltaTime;
            }

            angle = f.eulerAngles.y - horizFaces[numFace][0].transform.localEulerAngles.y;
            if (angle > 180) //Pour avoir un angle entre -180 et 180
                angle -= 360;
            else if (angle < -180)
                angle += 360;

            center = getcenter(horizFaces[numFace]);
            for (int i = 0; i < horizFaces[numFace].Count; i++)
                horizFaces[numFace][i].transform.RotateAround(center, horizFaces[numFace][i].transform.up, angle);
        }
    }

    public Vector3 getcenter(List<GameObject> l)
    {
        Vector3 center = new Vector3(0, 0, 0);
        int nbCube = l.Count;
        for (int cube = 0; cube < nbCube; cube++)
            center += l[cube].transform.position;
        center /= nbCube;
        return center;
    }

    public void setListCubes(List<List<GameObject>> horiz)
    {
        horizFaces = horiz;
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
