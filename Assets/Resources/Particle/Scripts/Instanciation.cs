using UnityEngine;
using System.Collections;

public class Instanciation : MonoBehaviour {

    public GameObject objectDuplicate;
    public int numberInstanciation;
    public float vitesseProj;
    public float ForceOnX;
    public float ForceOnY;
    public float ForceOnZ;

    void Start ()
    {
        for(int i = 0; i< numberInstanciation; i++)
        {
            GameObject go = (GameObject)Instantiate(objectDuplicate);
            go.transform.position = this.transform.position + new Vector3(0, 1, 0);
            go.GetComponent<Rigidbody>().velocity = vitesseProj * Time.deltaTime * new Vector3(Random.Range(-10f, 10f), Random.Range(5f, 10f), Random.Range(-10f, 10f));
        }
    }
	
}
