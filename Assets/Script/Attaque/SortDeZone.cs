﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SortDeZone : Attaque
{
    public float portee;
    public float duree;
    

    public delegate void Del();
    Del fctDelegate;

    public SortDeZone()
    {
        type = 2;
        portee = 80;
        duree = 2;
        cooldown = 1;
        degats = 0;
        element = EnumScript.Element.Eau;
        pseudoSort = "none";
        nameParticle = "none";
        nameInMenu = "none";
    }

    public SortDeZone(SortDeZone copy)
    {
        type = 2;
        portee = copy.portee;
        duree = copy.duree;
        cooldown = copy.cooldown;
        degats = copy.degats;
        element = copy.element;
        pseudoSort = copy.pseudoSort;
        nameInMenu = copy.nameInMenu;
        nameParticle = copy.nameParticle;
    }

    
    public SortDeZone(string p_pseudoSort, string p_nameParticle, EnumScript.Element p_element, int p_lvl, int p_xpActuel)
    {
        type = 2;
        pseudoSort = p_pseudoSort;
        nameParticle = p_nameParticle;
        element = p_element;
        lvl = p_lvl;
        xpActuel = p_xpActuel;

        structSortDeZone str = GameObject.FindWithTag("CaracSorts").GetComponent<CaracZones>().getStructFromName(p_nameParticle);

        nameInMenu = str.nameInMenu;
        duree = str.duree;
        cooldown = str.cooldown;
        degats = str.degats;
    }

    public override void AttackFromPlayer(Vector3 spawnPoint) //spawnPoint used for SortDeJet
    {
        if (canShoot)
        {
            launchSortDeZone();
            lastShot = Time.time;
            canShoot = false;
        }
    }

    public override void AttackFromEnemy(RaycastHit hit, Vector3 spawnPoint)
    {
        throw new NotImplementedException();
    }

    public void launchSortDeZone()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2 + 0.08f * Screen.height));
        //Recuperation du layerMask Player et Projectile
        LayerMask layerPlayer = LayerMask.GetMask("Player");
        LayerMask layerProj = LayerMask.GetMask("Projectile");
        int layerValue = layerPlayer.value | layerProj.value;
        //Inversion (on avoir la detection de tout SAUF du joueur et des projectiles)
        layerValue = ~layerValue;

        Vector3 direction = new Vector3(0, 0, 0);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerValue))
        {
            Quaternion quat = Quaternion.LookRotation(new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0), Vector3.up);
            GameObject zone = GameObject.Instantiate(Resources.Load("Particle/Prefabs/SortsDeZone/" + nameParticle + element.ToString() + "1"), hit.point, quat) as GameObject;
            zone.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);

            zone.transform.tag = "AttaquePlayer";
            setAllTagsAndAddVelocityAndEmitter("AttaquePlayer", zone, new Vector3(0, 0, 0), EnumScript.Character.Player);

            ProjectileData projData = zone.AddComponent<ProjectileData>();
            projData.degats = degats;
            projData.element = element;
        }
    }
}