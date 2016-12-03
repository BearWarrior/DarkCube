using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{

    public GameObject spawnPoint;
    public int numRoom;
    private GameObject caracSorts;

	// Use this for initialization
	void Start ()
    {
        PDVmax = 10;
        PDVactuel = 10;
        armureMax = 10;
        armureActuel = 10;

        caracSorts = GameObject.FindWithTag("CaracSorts");
        GetComponent<ListSorts>().initSort();
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<ListSorts>().getAttaqueEquiped().reload();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AttaquePlayer")
        {
            takeDamage(other.GetComponent<ProjectileData>().degats);
            if (other.GetComponent<ProjectileData>().type == 1)
            {
                caracSorts.GetComponent<CaracProjectiles>().gagnerXP(other.GetComponent<ProjectileData>().nomParticule);
            }
        }
    }

    private void takeDamage(float damage)
    {
        PDVactuel -= damage;
        if (PDVactuel <= 0)
        {
            GameObject.FindWithTag("World").GetComponent<EnemyBehaviour>().enemyDied(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void shoot(RaycastHit hit)
    {
        GetComponent<ListSorts>().getAttaqueEquiped().AttackFromEnemy(hit, this.spawnPoint.transform.position);
    }
}
