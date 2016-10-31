using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{

    public GameObject spawnPoint;

	// Use this for initialization
	void Start ()
    {
        PDVmax = 10;
        PDVactuel = 30;
        armureMax = 10;
        armureActuel = 10;
        //enduranceMax = 3;
        //enduranceActuel = 3;

        //regenEndurance = false;
        //timeBeforeRunningMax = 3;
        //timeBeforeRunningAct = 0;
        GetComponent<ListSorts>().initSort();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //print(GetComponent<ListSorts>().getAttaqueEquiped());
        GetComponent<ListSorts>().getAttaqueEquiped().reload();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AttaquePlayer")
        {
            takeDamage(other.GetComponent<ProjectileData>().degats);
        }
    }

    private void takeDamage(float damage)
    {
        PDVactuel -= damage;
        if(PDVactuel <= 0)
            Destroy(this.gameObject);
    }

    public void shoot(RaycastHit hit)
    {
        GetComponent<ListSorts>().getAttaqueEquiped().AttackFromEnemy(hit, this.spawnPoint.transform.position);
    }
}
