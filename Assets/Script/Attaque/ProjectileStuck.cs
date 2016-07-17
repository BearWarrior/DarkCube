using UnityEngine;
using System.Collections;

public class ProjectileStuck : MonoBehaviour
{

    public float flyTime;
    public Collider childCollider;

    private bool flying;
    private float stopTime;
    //private Transform anchor;

    private Vector3 rotationInit;

    private int nbFrame = 0;

    // Use this for initialization
    void Start()
    {
        flying = true;
        this.stopTime = Time.time + this.flyTime;
    }

    public void setInitRotation(Vector3 p_rot)
    {
        rotationInit = p_rot;
    }

    // Update is called once per frame
    void Update()
    {
        if(nbFrame == 1)
        {
            setInitRotation(this.transform.eulerAngles);
        }

        nbFrame++;

        if (this.stopTime <= Time.time && this.flying)
        {
            GameObject.Destroy(gameObject);
        }

        if (this.flying)
        {
            this.rotate();
        }
        /*else if (this.anchor != null)
        {
            //this.transform.position = anchor.transform.position;
            //this.transform.rotation = anchor.transform.rotation;
        }*/

    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.flying)
        {
            this.flying = false;
            this.transform.position = collision.contacts[0].point;
            this.childCollider.isTrigger = true;

            //GameObject anchor = new GameObject("ARROW ANCHOR");
            /*anchor.transform.position = this.transform.position;
            anchor.transform.eulerAngles = rotationInit;
            anchor.transform.parent = collision.transform;*/
            transform.eulerAngles = rotationInit;

            Debug.Log(rotationInit);

            //transform.parent = collision.transform;
            transform.SetParent(collision.transform, true);

            Destroy(GetComponent<Rigidbody>());
            //collision.gameObject.SendMessage("arrowhit", SendMessageOption.DontRequirReceiver);
        }
    }

    void rotate()
    {
        transform.LookAt(transform.position + GetComponent<Rigidbody>().velocity);
    }

}
