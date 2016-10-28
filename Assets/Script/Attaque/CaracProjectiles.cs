using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct structSortJet
{
    public string nomProj;
    public float degats;
    public float cooldown;
    public float vitesse;
    public EnumScript.Element element;
}


public class CaracProjectiles : MonoBehaviour
{
    public List<List<structSortJet>> tabSort;

    public List<structSortJet> DEFAULT;
    public List<structSortJet> CANON;
    public List<structSortJet> PRISME;
    public List<structSortJet> SHURIKEN;
    public List<structSortJet> ARROW;

    public List<structSortJet> LIGHTNINGBALL;
    public List<structSortJet> METEOR;
    public List<structSortJet> WAVE;
    public List<structSortJet> LASER;

    public void Start()
    {
        tabSort = new List<List<structSortJet>>();

        structSortJet sort;

        //CANON
        CANON = new List<structSortJet>();
        sort.nomProj = "Canon";
        sort.element = EnumScript.Element.Aucun;
        sort.degats = 10;
        sort.cooldown = 1;
        sort.vitesse = 30;
        CANON.Add(sort);
        tabSort.Add(CANON);

        //PRISME
        PRISME = new List<structSortJet>();
        sort.nomProj = "Prisme";
        sort.element = EnumScript.Element.Aucun;
        sort.degats = 30;
        sort.cooldown = 3;
        sort.vitesse = 60;
        PRISME.Add(sort);
        sort.nomProj = "Prisme";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 30;
        sort.cooldown = 3;
        sort.vitesse = 60;
        PRISME.Add(sort);
        tabSort.Add(PRISME);

        //SHURIKEN
        SHURIKEN = new List<structSortJet>();
        sort.nomProj = "Shuriken";
        sort.element = EnumScript.Element.Aucun;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 80;
        SHURIKEN.Add(sort);
        sort.nomProj = "Shuriken";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 5;
        sort.cooldown = 0.75f;
        sort.vitesse = 40;
        SHURIKEN.Add(sort);
        tabSort.Add(SHURIKEN);

        //ARROW
        ARROW = new List<structSortJet>();
        sort.nomProj = "Fleche";
        sort.element = EnumScript.Element.Aucun;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 80;
        ARROW.Add(sort);
        sort.nomProj = "Fleche";
        sort.element = EnumScript.Element.Feu;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 80;
        ARROW.Add(sort);
        tabSort.Add(ARROW);

        //LIGHTNINGBALL
        LIGHTNINGBALL = new List<structSortJet>();
        sort.nomProj = "LightningBall";
        sort.element = EnumScript.Element.Elec;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        LIGHTNINGBALL.Add(sort);
        tabSort.Add(LIGHTNINGBALL);

        //METEOR
        METEOR = new List<structSortJet>();
        sort.nomProj = "Meteor";
        sort.element = EnumScript.Element.Feu;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        METEOR.Add(sort);
        sort.nomProj = "Meteor";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        METEOR.Add(sort);
        tabSort.Add(METEOR);

        //WAVE
        WAVE = new List<structSortJet>();
        sort.nomProj = "Wave";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        WAVE.Add(sort);
        tabSort.Add(WAVE);

        //LASER
        LASER = new List<structSortJet>();
        sort.nomProj = "ShotLaser";
        sort.element = EnumScript.Element.Elec;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        LASER.Add(sort);
        tabSort.Add(LASER);
    }


    public structSortJet getStructFromNameAndElement(string p_name, EnumScript.Element p_elem)
    {
        foreach (List<structSortJet> sorts in tabSort)
        {
            if (sorts[0].nomProj.Equals(p_name))
            {
                foreach (structSortJet sort in sorts)
                {
                    if (sort.element == p_elem)
                    {
                        return sort;
                    }
                }
            }
        }

        return tabSort[0][0];

    }

    public List<structSortJet> getProjsFromElement(EnumScript.Element p_elem)
    {
        List<structSortJet> listProj = new List<structSortJet>();
        if (tabSort == null)
            Start();
        foreach (List<structSortJet> sorts in tabSort)
        {
            foreach (structSortJet sort in sorts)
            {
                if (sort.element == p_elem)
                {
                    listProj.Add(sort);
                    break;
                }
            }
        }
        return listProj;
    }
}
