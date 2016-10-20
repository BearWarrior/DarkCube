using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCubeFlock : MonoBehaviour
{
    public int nbCube;
    public GameObject cubes;
    public GameObject posToGo;
    public List<float> vitesses;
    public List<float> distance;
    public GameObject directionPoint;
    public Vector3 direction;

    // Use this for initialization
    void Start ()
    {
        nbCube = cubes.transform.childCount;

        distance = new List<float>();
        vitesses = new List<float>();
        for (int i = 0; i < nbCube; i++)
        {
            vitesses.Add(Random.Range(0.45f, 0.55f));
            distance.Add(0);
        }

        directionPoint = new GameObject();
        directionPoint.transform.localPosition = new Vector3(0, 0, 0);
        directionPoint.transform.SetParent(gameObject.transform);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        directionPoint.transform.localPosition = 1f*direction;

        float distMax = 0;
        int i = 0;
        for (i = 0; i < nbCube; i++)
        {
            distance[i] = Mathf.Sqrt(Vector3.Distance(directionPoint.transform.position, cubes.transform.GetChild(i).transform.position));

            distMax = (distance[i] > distMax) ? distance[i] : distMax;
        }
        distMax -= 0.0f;

        float max = 01000;
        string sortie = "";

        i = 0;
        for (int cptFace = 0; cptFace < posToGo.transform.childCount; cptFace++)
        {
            for (int j = 0; j < posToGo.transform.GetChild(cptFace).childCount; j++)
            {

                if (1 - distance[i] / distMax < 0)
                    vitesses[i] = 0.1f;
                else
                    vitesses[i] = 1 - distance[i] / distMax;


                if (distance[i] / distMax < max)
                    max = distance[i] / distMax;

                cubes.transform.GetChild(i).transform.position = Vector3.Lerp(cubes.transform.GetChild(i).transform.position, posToGo.transform.GetChild(cptFace).GetChild(j).transform.position, 1.4f - ((distance[i] / distMax)));
                sortie += (distance[i] / distMax) * (distance[i] / distMax) + "\n";
                cubes.transform.GetChild(i).transform.rotation = Quaternion.Lerp(cubes.transform.GetChild(i).transform.rotation, posToGo.transform.GetChild(cptFace).GetChild(j).transform.rotation, 1.4f - ((distance[i] / distMax)));


                i++;
            }
        }
	}

    public void init(GameObject p_cubes, GameObject p_posCubes)
    {
        cubes = p_cubes;
        posToGo = p_posCubes;
    }
}
