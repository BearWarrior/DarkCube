using UnityEngine;
using System.Collections;

public class ParticleCollisionBehaviour : MonoBehaviour {

    public GameObject core;
    public GameObject explosion;
    public float timeBeforeDestroy;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "AttaquePlayer")
        {
            core.SetActive(false);
            explosion.SetActive(true);
            Destroy(this.gameObject, timeBeforeDestroy);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().useGravity = false;
        }
    }
}
