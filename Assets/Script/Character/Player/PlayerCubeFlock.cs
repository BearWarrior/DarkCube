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

    //SHADER DISSOLVE
    private bool dissolving;
    private float from = 0.2f;
    private float to = .8f;
    private float startTime;
    private float journeyLength = 1.0f;
    private float speed = .4f;

    // Use this for initialization
    void Start ()
    {
        dissolving = false;

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
        if(dissolving)
        {
            float dissolveCovered = (Time.time - startTime) * speed;
            float fracJourney = dissolveCovered / journeyLength;

            for (int i = 0; i < cubes.transform.childCount; i++)
            {
                if (i != cubes.transform.childCount - 1)
                {
                    cubes.transform.GetChild(i).GetComponent<Renderer>().material.SetFloat("_SliceAmount", Mathf.Lerp(from, to, fracJourney));
                }
            }

            if (fracJourney > 1)
            {
                dissolving = false;
                for (int i = 0; i < cubes.transform.childCount; i++)
                {
                    if (i != cubes.transform.childCount - 1)
                    {
                        cubes.transform.GetChild(i).GetComponent<Renderer>().material.SetFloat("_SliceAmount", 1);
                    }
                }
            }
        }

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

    public void disapear()
    {
        dissolving = true;
        startTime = Time.time;

        Material dissolverMat = Resources.Load("Player/Materials/Dissolver") as Material;
        Texture textureCube = GameObject.FindWithTag("Player").GetComponent<Player>().getSkin();
        dissolverMat.mainTexture = textureCube;

        for (int i = 0; i < cubes.transform.childCount; i++)
        {
            if (i != cubes.transform.childCount - 1)
            {
                cubes.transform.GetChild(i).GetComponent<Renderer>().material = dissolverMat;

                cubes.transform.GetChild(i).GetComponent<Renderer>().material.SetTextureOffset("_Dissolver", new Vector2(Random.Range(0, 1.0f), Random.Range(0, 1.0f)));
            }
            else
            {
                cubes.transform.GetChild(i).gameObject.AddComponent<FadeOutParticl>();
            }
        }
    }

    public void changeSkin(Texture newText)
    {
        dissolving = true;
        startTime = Time.time;

        Material changeSkin = Resources.Load("Player/Materials/ChangeSkin") as Material;
        Texture textureCube = GameObject.FindWithTag("Player").GetComponent<Player>().getSkin();
        changeSkin.SetTexture("_MainTex", textureCube);
        changeSkin.SetTexture("_SecondaryTex", newText);

        for (int i = 0; i < cubes.transform.childCount; i++)
        {
            if (i != cubes.transform.childCount - 1)
            {
                float x = Random.Range(0, 1.0f);
                float y = Random.Range(0, 1.0f);
                cubes.transform.GetChild(i).GetComponent<Renderer>().material = changeSkin;
                cubes.transform.GetChild(i).GetComponent<Renderer>().material.SetTextureOffset("_Dissolver", new Vector2(x, y));
                cubes.transform.GetChild(i).GetComponent<Renderer>().material.SetTextureOffset("_DissolverOpposite", new Vector2(x, y));
            }
            else
            {
                cubes.transform.GetChild(i).gameObject.AddComponent<FadeOutParticl>();
            }
        }
    }
}