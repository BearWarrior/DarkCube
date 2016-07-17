using UnityEngine;
using System.Collections;

public class ProjectileGraviteOnTouch : MonoBehaviour
{

    public float flyTime;
    public Collider childCollider;

    private bool flying;
    private float stopTime;

    // Use this for initialization
    void Start()
    {
        flying = true;
        this.stopTime = Time.time + this.flyTime;
    }


    // Update is called once per frame
    void Update()
    {
        if (this.stopTime <= Time.time && this.flying)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.flying)
        {
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
