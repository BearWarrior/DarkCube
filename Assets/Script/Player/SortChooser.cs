using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortChooser : MonoBehaviour
{
    public int cubeFace;
    private GameObject posAllCubes;

    private Quaternion f1;
    private Quaternion f2;
    private Quaternion f3;
    private Quaternion f4;
    private Quaternion f5;
    private Quaternion f6;
    private Quaternion fdefault;

    public bool changingFaceV;
    public bool changingFaceH;

    public List<List<GameObject>> horizFaces = new List<List<GameObject>>();
    public List<GameObject> vertFaces = new List<GameObject>();

    public bool rotHFinished = true;
    public bool rotVFinished = true;

    // Use this for initialization
    void Start()
    {
        changingFaceV = false;
        changingFaceH = false;

        cubeFace = 1;
        //posAllCubes = GameObject.FindWithTag("Armature");
        posAllCubes = this.transform.GetChild(5).gameObject;

        f1 = Quaternion.Euler(new Vector3(0, 0, 0));
        f2 = Quaternion.Euler(new Vector3(0, 90, 0));
        f3 = Quaternion.Euler(new Vector3(0, 180, 0));
        f4 = Quaternion.Euler(new Vector3(0, 270, 0));
        f5 = Quaternion.Euler(new Vector3(90, 0, 0));
        f6 = Quaternion.Euler(new Vector3(-90, 0, 0));
        fdefault = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void Update()
    {
        int oldCubeFace = cubeFace;
 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (rotHFinished && rotVFinished)
            {
                StopAllCoroutines();
                if (oldCubeFace >= 5)
                    StartCoroutine(coroutineRotationVH(f1, fdefault, 1));
                else
                    StartCoroutine(coroutineRotationHV(f1, fdefault, 1));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (rotHFinished && rotVFinished)
            {
                StopAllCoroutines();
                if (oldCubeFace >= 5)
                    StartCoroutine(coroutineRotationVH(f2, fdefault, 2));
                else
                    StartCoroutine(coroutineRotationHV(f2, fdefault, 2));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (rotHFinished && rotVFinished)
            {
                StopAllCoroutines();
                if (oldCubeFace >= 5)
                    StartCoroutine(coroutineRotationVH(f3, fdefault, 3));
                else
                    StartCoroutine(coroutineRotationHV(f3, fdefault, 3));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (rotHFinished && rotVFinished)
            {
                StopAllCoroutines();
                if (oldCubeFace >= 5)
                    StartCoroutine(coroutineRotationVH(f4, fdefault, 4));
                else
                    StartCoroutine(coroutineRotationHV(f4, fdefault, 4));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (rotHFinished && rotVFinished)
            {
                StopAllCoroutines();
                if (oldCubeFace >= 5)
                    StartCoroutine(coroutineRotationVH(f1, f5, 5));
                else
                    StartCoroutine(coroutineRotationHV(f1, f5, 5));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (rotHFinished && rotVFinished)
            {
                StopAllCoroutines();
                if (oldCubeFace >= 5)
                    StartCoroutine(coroutineRotationVH(f1, f6, 6));
                else
                    StartCoroutine(coroutineRotationHV(f1, f6, 6));
            }
        }
        /*if (oldCubeFace != cubeFace)
            GetComponent<Player>().cubeFaceChanged(cubeFace);*/
    }

    public IEnumerator coroutineRotationHV(Quaternion fH, Quaternion fV, int newCubeFace)
    {
        changingFaceV = true;
        changingFaceH = true;

        rotHFinished = false;
        rotVFinished = false;
        StartCoroutine(coroutineRotationHorizAll(fH));
        while (rotHFinished == false)
            yield return new WaitForEndOfFrame();
        StartCoroutine(coroutineRotationVertAll(fV));

        while (rotVFinished == false)
            yield return new WaitForEndOfFrame();
        cubeFace = newCubeFace;
    }

    public IEnumerator coroutineRotationVH(Quaternion fH, Quaternion fV, int newCubeFace)
    {
        changingFaceV = true;
        changingFaceH = true;

        rotHFinished = false;
        rotVFinished = false;
        StartCoroutine(coroutineRotationVertAll(fV));
        while (rotVFinished == false)
            yield return new WaitForEndOfFrame();
        StartCoroutine(coroutineRotationHorizAll(fH));

        while (rotHFinished == false)
            yield return new WaitForEndOfFrame();
        cubeFace = newCubeFace;
    }

    public IEnumerator coroutineRotationHorizAll(Quaternion f)
    {
        float angle = (f.eulerAngles.y - horizFaces[0][0].transform.localEulerAngles.y);
        if (angle != 0)
        {
            for (int i = 0; i < 6; i++)
            {
                StartCoroutine(coroutineRotationHoriz(f, i));
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            rotHFinished = true;
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
        if (numFace == 5)
        {
            //Permet de lancer le deuxieme
            rotHFinished = true;
        }
    }

    public IEnumerator coroutineRotationVertAll(Quaternion f)
    {
        if (Quaternion.Angle(posAllCubes.transform.GetChild(0).transform.localRotation, f) > 1)
        {
            for (int i = 0; i < 6 - 1; i++)
            {
                StartCoroutine(coroutineRotationVert(f, i));
                yield return new WaitForSeconds(0.05f);
            }
            StartCoroutine(coroutineRotationVert(f, 5));
        }
        else
        {
            rotVFinished = true;
        }
    }

    public IEnumerator coroutineRotationVert(Quaternion f, int numFace)
    {
        float angle = Quaternion.Angle(posAllCubes.transform.GetChild(numFace).transform.localRotation, f);
        float vitesse = Mathf.Abs(angle) / 15;

        float rotTime = 0.15f;
        float startTime = Time.time;

        Quaternion begin = posAllCubes.transform.GetChild(numFace).transform.localRotation;

        while (Quaternion.Angle(posAllCubes.transform.GetChild(numFace).transform.localRotation, f) > 1)
        {
            float frac = (Time.time - startTime) / rotTime;
            posAllCubes.transform.GetChild(numFace).transform.localRotation = Quaternion.Lerp(begin, f, frac);
            yield return new WaitForEndOfFrame();
        }
        if (Quaternion.Angle(posAllCubes.transform.GetChild(numFace).transform.localRotation, f) < 1)
        {
            posAllCubes.transform.GetChild(numFace).transform.localRotation = f;
        }
        if (numFace == 5)
        {
            //Permet de lancer le deuxieme
            rotVFinished = true;
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

    public void setListCubes(List<List<GameObject>> horiz, GameObject posAC)
    {
        horizFaces = horiz;
        posAllCubes = posAC;
    }
}