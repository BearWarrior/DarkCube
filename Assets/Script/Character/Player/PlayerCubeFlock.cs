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

    private float shakiness;
    private float colorCore;

    private bool dead;
    private bool deathApplied;

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

        shakiness = 0;
        colorCore = 1; //white
        dead = false;
        deathApplied = false;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!dead)
        {

            directionPoint.transform.localPosition = 1f * direction;

            //Calcul de la distance entre le cube et la sphere de direction
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

            //déplacement des cubes
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

                    cubes.transform.GetChild(i).transform.position += Random.insideUnitSphere * shakiness;

                    i++;
                }
            }

            //Déplacement du core
            cubes.transform.GetChild(cubes.transform.childCount - 1).transform.position = Vector3.Lerp(cubes.transform.GetChild(cubes.transform.childCount - 1).transform.position, posToGo.transform.GetChild(6).transform.position, 1.4f - ((distance[i] / distMax)));
            sortie += (distance[i] / distMax) * (distance[i] / distMax) + "\n";
            cubes.transform.GetChild(cubes.transform.childCount - 1).transform.rotation = Quaternion.Lerp(cubes.transform.GetChild(cubes.transform.childCount - 1).transform.rotation, posToGo.transform.GetChild(6).transform.rotation, 1.4f - ((distance[i] / distMax)));

            cubes.transform.GetChild(cubes.transform.childCount - 1).transform.GetChild(cubes.transform.GetChild(cubes.transform.childCount - 1).transform.childCount - 1).GetComponent<Light>().color = new Color(1, colorCore, colorCore);
        }
        else if(!deathApplied)
        {
            for (int i = 0; i < cubes.transform.childCount; i++)
            {
                GameObject.FindWithTag("Player").GetComponent<BoxCollider>().isTrigger = true;
                GameObject.FindWithTag("Player").GetComponent<Rigidbody>().useGravity = false;
                cubes.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                cubes.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();

                if(i == cubes.transform.childCount-1)
                {
                    Vector3 explosionPos = cubes.transform.GetChild(cubes.transform.childCount - 1).transform.position;
                    Collider[] colliders = Physics.OverlapSphere(explosionPos, 2);
                    for(int j = 0; j < colliders.Length; j++)
                    {
                        if(colliders[j].GetComponent<Rigidbody>() != null)
                            colliders[j].GetComponent<Rigidbody>().AddExplosionForce(Random.Range(200, 300), explosionPos, 5, 0);
                    }

                    cubes.transform.GetChild(cubes.transform.childCount - 1).gameObject.SetActive(false);
                }
            }
            deathApplied = true;
        }
    }


    public void init(GameObject p_cubes, GameObject p_posCubes)
    {
        cubes = p_cubes;
        posToGo = p_posCubes;
    }

    public void setShakiness(float PDV, float PDVMax)
    {
        //              [0, 1]         [0, 0.01]
        float temp = (PDV / PDVMax);
        shakiness = 0.01f - temp/100;

        colorCore = temp;
    }

    public void Die()
    {
        dead = true;
    }
}