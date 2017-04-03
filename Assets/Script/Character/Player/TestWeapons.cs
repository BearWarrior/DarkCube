using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapons : Character
{
    public GameObject spawnProjectile;

    // Use this for initialization
    void Start()
    {
        listAttaque = new Attaque[1];
        listAttaque[0] = null;
    }

    void Update()
    {
        if (listAttaque[0] != null)
            listAttaque[0].AttackFromTestWeapons(spawnProjectile.transform.position);

        //cooldowns
        if (listAttaque[0] != null)
            listAttaque[0].reload();
    }

    public void setAttaque(Attaque attaque)
    {
        listAttaque[0] = attaque;
    }
}
