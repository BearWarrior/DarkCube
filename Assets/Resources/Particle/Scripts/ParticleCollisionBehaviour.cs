using UnityEngine;
using System.Collections;

public class ParticleCollisionBehaviour : MonoBehaviour {

    public GameObject core;
    public GameObject explosion;
    public float timeBeforeDestroy;
    public EnumScript.Character emiter;

    void OnTriggerEnter(Collider other)
    {
        if (emiter == EnumScript.Character.Player)
        {
            if (other.tag != "Player" && other.tag != "AttaquePlayer" && other.tag != "AttaqueEnemy")
            {
                core.SetActive(false);
                explosion.SetActive(true);
                Destroy(this.gameObject, timeBeforeDestroy);
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                GetComponent<Rigidbody>().useGravity = false;
            }
        }
        else if (emiter == EnumScript.Character.Enemy)
        {
            if (other.tag != "Enemy" && other.tag != "AttaquePlayer" && other.tag != "AttaqueEnemy")
            {
                core.SetActive(false);
                explosion.SetActive(true);
                Destroy(this.gameObject, timeBeforeDestroy);
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                GetComponent<Rigidbody>().useGravity = false;
            }
        }

    }
}
