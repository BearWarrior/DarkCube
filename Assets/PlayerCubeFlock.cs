using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCubeFlock : MonoBehaviour
{
    public int nbCube;
    public GameObject cubes2;
    public GameObject posToGo2;
    public List<float> vitesses2;

    // Use this for initialization
    void Start ()
    {
        nbCube = cubes2.transform.childCount;

        for(int i = 0; i < nbCube; i++)
        {
            vitesses2.Add(Random.Range(0.45f, 0.55f));
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        for(int i = 0; i < nbCube; i++)
        {
            cubes2.transform.GetChild(i).transform.position = Vector3.Lerp(cubes2.transform.GetChild(i).transform.position, posToGo2.transform.GetChild(i).transform.position, vitesses2[i]);
            cubes2.transform.GetChild(i).transform.rotation = Quaternion.Lerp(cubes2.transform.GetChild(i).transform.rotation, posToGo2.transform.GetChild(i).transform.rotation, vitesses2[i]);
        }
	}
}
