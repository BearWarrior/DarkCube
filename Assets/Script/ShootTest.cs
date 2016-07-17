using UnityEngine;
using System.Collections;

public class ShootTest : MonoBehaviour {

    public Rigidbody projectile;
    public float speed;
    //private bool tir = GameObject.FindWithTag("Player").GetComponent<PlayerController>().getShortLeftClic();
    //private int cubeFace = GameObject.FindWithTag("Player").GetComponent<SortChooser>().cubeFace;
    private int cubeFace;
    private bool cubeSpawn;

    // Use this for initialization
    void Start () {
	    speed = 90;
        cubeFace = 0;
        cubeSpawn = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cubeFace = 1;
        }

       /* if (cubeFace == 1  && cubeSpawn == true*) // cubeFace = 1
        {

            Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;

            if (Input.GetButtonDown("Fire1"))
            {
                instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, speed));
                //cubeSpawn = true;
            }

            //cubeSpawn = false;
            Debug.Log("cubeSpawn :" + cubeSpawn);
        }*/

        //Debug.Log("cubeFace :" + cubeFace);

        /*---------------------------------------------------*/

        /*
        if (Input.GetButtonDown("Fire1"))
        //  if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().getShortLeftClic())
        //if(tir)
        {
            Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;
            instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, speed));
        }

        //Debug.Log("tir : " + tir);
        //Debug.Log("tir : " + GameObject.FindWithTag("Player").GetComponent<PlayerController>().getShortLeftClic());
        */
    }

   /* public void proj ()
    {
        if (cubeFace == 1 && cubeSpawn == true) // cubeFace = 1
        {

            Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;

            if (Input.GetButtonDown("Fire1"))
            {
                instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, speed));
                cubeSpawn = true;
            }

            cubeSpawn = false;
            //Debug.Log("cubeSpawn :" + cubeSpawn);
        }
        //Debug.Log("cubeSpawn :" + cubeSpawn);
    }*/

}
