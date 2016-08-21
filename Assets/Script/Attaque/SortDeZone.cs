using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortDeZone : Attaque
{
    public float portee;
    public EnumScript.TailleSortDeZone taille;
    public float largeur;
    public float duree;

    public EnumScript.GabaritSortDeZone gabarit;

    public delegate void Del();
    Del fctDelegate;

    public string nomZone;

    public string particleSystemStr;

    public SortDeZone()
    {
        type = 2;
        portee = 80;
        taille = EnumScript.TailleSortDeZone.Moyen;
        largeur = 4;
        duree = 2;
        cooldown = 1;
        degats = 0;
        element = EnumScript.Element.Eau;
        gabarit = EnumScript.GabaritSortDeZone.Cercle;
        nomSort = "none";
        nomZone = "none";
        particleSystemStr = "none";
    }

    public SortDeZone(SortDeZone copy)
    {
        type = 2;
        portee = copy.portee;
        taille = copy.taille;
        largeur = copy.largeur;
        duree = copy.duree;
        cooldown = copy.cooldown;
        degats = copy.degats;
        element = copy.element;
        gabarit = copy.gabarit;
        nomSort = copy.nomSort;
        nomZone = copy.nomZone;
        particleSystemStr = copy.particleSystemStr;
    }

    //paramétré
    public SortDeZone(EnumScript.TailleSortDeZone p_rayon, float p_largeur, float p_portee, float p_duree, float p_cooldown, float p_degats, EnumScript.Element p_element, EnumScript.GabaritSortDeZone p_gabarit, string p_nomSort, string p_nomZone, string p_partSysStr)
    {
        type = 2;
        portee = p_portee;
        taille = p_rayon;
        largeur = p_largeur;
        duree = p_duree;
        cooldown = p_cooldown;
        degats = p_degats;
        element = p_element;
        gabarit = p_gabarit;
        nomSort = p_nomSort;
        nomZone = p_nomZone;
        particleSystemStr = p_partSysStr;
    }


    public override void Attaquer()
    {
        if (canShoot)
        {
            if (gabarit == EnumScript.GabaritSortDeZone.Cercle)
            {
                GameObject goZone = new GameObject();
                goZone.AddComponent<CapsuleCollider>();
                switch(taille)
                {
                    case EnumScript.TailleSortDeZone.Petit:
                        goZone.GetComponent<CapsuleCollider>().radius = 10;
                        break;
                    case EnumScript.TailleSortDeZone.Moyen:
                        goZone.GetComponent<CapsuleCollider>().radius = 15;
                        break;
                    case EnumScript.TailleSortDeZone.Grand:
                        goZone.GetComponent<CapsuleCollider>().radius = 20;
                        break;
                }
                goZone.GetComponent<CapsuleCollider>().height = 25;
                goZone.GetComponent<CapsuleCollider>().isTrigger = true;
                goZone.transform.position = GameObject.FindWithTag("Projector").transform.position;
                goZone.AddComponent<ApplyDegatsZone>();
                //GameObject.Destroy(goZone, duree);
                RaycastHit hit;
                GameObject resource = Resources.Load(particleSystemStr) as GameObject;
                if (resource != null)
                {
                    GameObject partSys = GameObject.Instantiate(Resources.Load(particleSystemStr)) as GameObject;
                    if (Physics.Raycast(goZone.transform.position, -Vector3.up, out hit, Mathf.Infinity))
                    {
                        partSys.transform.position = hit.point;
                        partSys.transform.eulerAngles = GameObject.FindWithTag("Player").transform.eulerAngles;
                    }
                }
                

            }
            else if (gabarit == EnumScript.GabaritSortDeZone.Ligne)
            {
                GameObject goZone = new GameObject();
                goZone.AddComponent<BoxCollider>();
                switch (taille)
                {
                    case EnumScript.TailleSortDeZone.Petit:
                        goZone.GetComponent<BoxCollider>().size = new Vector3(30, 10, 8);
                        break;
                    case EnumScript.TailleSortDeZone.Moyen:
                        goZone.GetComponent<BoxCollider>().size = new Vector3(46, 10, 8);
                        break;
                    case EnumScript.TailleSortDeZone.Grand:
                        goZone.GetComponent<BoxCollider>().size = new Vector3(62, 10, 8);
                        break;
                }
                goZone.GetComponent<BoxCollider>().isTrigger = true;
                goZone.transform.position = GameObject.FindWithTag("Projector").transform.position + new Vector3(0, 1, 0);
                goZone.transform.eulerAngles = GameObject.FindWithTag("Player").transform.eulerAngles;
                goZone.AddComponent<ApplyDegatsZone>();
                //GameObject.Destroy(goZone, duree);
                RaycastHit hit;
                GameObject resource = Resources.Load(particleSystemStr) as GameObject;
                if (resource != null)
                {
                    GameObject partSys = GameObject.Instantiate(Resources.Load(particleSystemStr)) as GameObject;
                    if (Physics.Raycast(goZone.transform.position, -Vector3.up, out hit, Mathf.Infinity))
                    {
                        partSys.transform.position = hit.point;
                        partSys.transform.eulerAngles = GameObject.FindWithTag("Player").transform.eulerAngles;
                    }
                }

            }
            else if (gabarit == EnumScript.GabaritSortDeZone.Cone)
            {
                //Angle + distance
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().useZoneCone(duree);
            }




            lastShot = Time.time;
            canShoot = false;
        }
    }
}
