using UnityEngine;
using System.Collections;

public class TargetCollisionBehaviour : MonoBehaviour {

    public GameObject core;
    public GameObject explosion;
    public float timeBeforeDestroy;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            core.SetActive(false);
            explosion.SetActive(true);
            Destroy(this.gameObject, timeBeforeDestroy);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}
