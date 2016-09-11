using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortDeZone : Attaque
{
    public float portee;
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
    public SortDeZone(float p_largeur, float p_portee, float p_duree, float p_cooldown, float p_degats, EnumScript.Element p_element, EnumScript.GabaritSortDeZone p_gabarit, string p_nomSort, string p_nomZone, string p_partSysStr)
    {
        type = 2;
        portee = p_portee;
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
                //TODO CHANGER CA
                /*switch(taille)
                {
                    case EnumScript.TailleSortDeZone.Petit:
                        goZone.GetComponent<CapsuleCollider>().radius = 2;
                        break;
                    case EnumScript.TailleSortDeZone.Moyen:
                        goZone.GetComponent<CapsuleCollider>().radius = 3;
                        break;
                    case EnumScript.TailleSortDeZone.Grand:
                        goZone.GetComponent<CapsuleCollider>().radius = 4;
                        break;
                }*/
                goZone.GetComponent<CapsuleCollider>().radius = 3;


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
                //TO DO CHANGER CA
                /*switch (taille)
                {
                    case EnumScript.TailleSortDeZone.Petit:
                        goZone.GetComponent<BoxCollider>().size = new Vector3(30 / 5, 10 / 5, 8 / 5);
                        break;
                    case EnumScript.TailleSortDeZone.Moyen:
                        goZone.GetComponent<BoxCollider>().size = new Vector3(46 / 5, 10 / 5, 8 / 5);
                        break;
                    case EnumScript.TailleSortDeZone.Grand:
                        goZone.GetComponent<BoxCollider>().size = new Vector3(62 / 5, 10 / 5, 8 / 5);
                        break;
                }*/
                goZone.GetComponent<BoxCollider>().size = new Vector3(46 / 5, 10 / 5, 8 / 5);


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
