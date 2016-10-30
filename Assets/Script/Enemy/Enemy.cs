using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{

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

    public void shoot()
    {
        Debug.Log("shoot");
        GetComponent<ListSorts>().getAttaqueEquiped().AttackFromEnemy(new Vector3(0, 0, 0));
    }
}
