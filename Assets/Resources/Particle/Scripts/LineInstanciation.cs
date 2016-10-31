using UnityEngine;
using System.Collections;

public class LineInstanciation : MonoBehaviour {

    public GameObject ParticuleASpawn;
    public int NumberOfElement;
    public float DistanceBetweenElement;
    public float TimeBetweenSpawn;
    public float timeBeforeDestroy;
    public Vector3 Direction = Vector3.forward;

    void Start()
    {
        StartCoroutine(coroutineLancement());
    }

    public IEnumerator coroutineLancement()
    {
        for (int i = 0; i < NumberOfElement; i++)
        {
            GameObject go = (GameObject)Instantiate(ParticuleASpawn);
            go.transform.localPosition = this.transform.localPosition + (this.transform.forward * DistanceBetweenElement * i);
            Destroy(go, timeBeforeDestroy);
            go.transform.SetParent(this.transform);
            go.tag = this.tag;
            yield return new WaitForSeconds(TimeBetweenSpawn); 
        }
        Destroy(this.gameObject, timeBeforeDestroy);
    }
}
