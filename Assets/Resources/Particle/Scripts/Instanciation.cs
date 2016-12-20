using UnityEngine;
using System.Collections;

public class Instanciation : MonoBehaviour {

    public GameObject objectDuplicate;
    public int numberInstanciation;
    public float vitesseProj;
    public float TransformPosX;
    public float TransformPosY;
    public float TransformPosZ;
    public float ForceOnX;
    public float ForceOnY;
    public float ForceOnZ;

    private ProjectileData projDataParent;

    void Start ()
    {
        projDataParent = GetComponentInParent<ProjectileData>();
        for (int i = 0; i< numberInstanciation; i++)
        {
            GameObject go = (GameObject)Instantiate(objectDuplicate);
            go.transform.position = this.transform.position + new Vector3(TransformPosX, TransformPosY, TransformPosZ);
            go.GetComponent<Rigidbody>().velocity = vitesseProj * Time.deltaTime * new Vector3(ForceOnX + Random.Range(-10f, 10f), ForceOnY + Random.Range(5f, 10f), ForceOnZ + Random.Range(-10f, 10f));
            ProjectileData projData = go.AddComponent<ProjectileData>();
            projData.nomParticule = projDataParent.nomParticule;
            projData.degats = projDataParent.degats;
            projData.element = projDataParent.element;
            projData.type = projDataParent.type;
            go.tag = gameObject.transform.parent.tag;
        }
    }
}
