using UnityEngine;
using System.Collections;

public class ZoneInstantiate : MonoBehaviour {

    public GameObject ParticuleASpawn;
    public int NumberOfElement;
    public float Range;
    public float TimeBetweenSpawn;
    public float timeBeforeDestroy;
    //public Vector3 Direction;

    void Start()
    {
        StartCoroutine(coroutineLancement());
    }

    public IEnumerator coroutineLancement()
    {
        for (int i = 0; i < NumberOfElement; i++)
        {
            GameObject go = (GameObject)Instantiate(ParticuleASpawn);
            go.transform.position = this.transform.position + new Vector3(0, 0, 0);
            Destroy(go, timeBeforeDestroy);
            yield return new WaitForSeconds(TimeBetweenSpawn);
        }
    }
}
