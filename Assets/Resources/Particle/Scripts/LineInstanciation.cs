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
            go.transform.position = this.transform.position + (Direction * DistanceBetweenElement * i);
            Destroy(go, timeBeforeDestroy);
            yield return new WaitForSeconds(TimeBetweenSpawn); 
        }
    }
}
