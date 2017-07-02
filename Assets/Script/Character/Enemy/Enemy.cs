using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character, ITakeDamages
{

    public List<GameObject> spawnPoints;
    public int numRoom;
    private GameObject caracSorts;

    private int spawnPointAct;

	// Use this for initialization
	void Start ()
    {
        //PDVmax = 10;
        PDVactuel = PDVmax;
        armureMax = 10;
        armureActuel = 10;

        caracSorts = GameObject.FindWithTag("CaracSorts");
        GetComponent<ListSorts>().initSort();

        spawnPointAct = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<ListSorts>().getAttaqueEquiped().reload();
    }

    /*void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AttaquePlayer")
        {
            takeDamages(other.GetComponent<ProjectileData>().degats);
            if (other.GetComponent<ProjectileData>().type == 1)
            {
                caracSorts.GetComponent<CaracProjectiles>().gagnerXP(other.GetComponent<ProjectileData>().nomParticule);
            }
        }
    }*/

    public override void takeDamages(float damages)
    {
        PDVactuel -= damages;
        if (PDVactuel <= 0)
        {
            GameObject.FindWithTag("World").GetComponent<EnemyBehaviour>().enemyDied(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void shoot(RaycastHit hit)
    {
        GetComponent<ListSorts>().getAttaqueEquiped().AttackFromEnemy(hit, this.spawnPoints[spawnPointAct].transform.position);
        spawnPointAct++;
        if (spawnPointAct > spawnPoints.Count -1)
            spawnPointAct = 0;
    }
}
