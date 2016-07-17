using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct structSortJet
{
    public string nomProj;
    public float degats;
    public float cooldown;
    public float vitesse;
    public bool stuck;
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
        sort.stuck = false;
        sort.vitesse = 30;
        CANON.Add(sort);
        sort.nomProj = "Canon";
        sort.element = EnumScript.Element.Metal;
        sort.degats = 20;
        sort.cooldown = 1.5f;
        sort.vitesse = 20;
        sort.stuck = false;
        CANON.Add(sort);
        tabSort.Add(CANON);

        //PRISME
        PRISME = new List<structSortJet>();
        sort.nomProj = "Prisme";
        sort.element = EnumScript.Element.Aucun;
        sort.degats = 30;
        sort.cooldown = 3;
        sort.vitesse = 60;
        sort.stuck = true;
        PRISME.Add(sort);
        sort.nomProj = "Prisme";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 30;
        sort.cooldown = 3;
        sort.vitesse = 60;
        sort.stuck = true;
        PRISME.Add(sort);
        tabSort.Add(PRISME);

        //SHURIKEN
        SHURIKEN = new List<structSortJet>();
        sort.nomProj = "Shuriken";
        sort.element = EnumScript.Element.Aucun;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 80;
        sort.stuck = true;
        SHURIKEN.Add(sort);
        sort.nomProj = "Shuriken";
        sort.element = EnumScript.Element.Metal;
        sort.degats = 15;
        sort.cooldown = 1;
        sort.vitesse = 40;
        sort.stuck = true;
        SHURIKEN.Add(sort);
        sort.nomProj = "Shuriken";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 5;
        sort.cooldown = 0.75f;
        sort.vitesse = 40;
        sort.stuck = true;
        SHURIKEN.Add(sort);
        tabSort.Add(SHURIKEN);

        //ARROW
        ARROW = new List<structSortJet>();
        sort.nomProj = "Fleche";
        sort.element = EnumScript.Element.Aucun;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 80;
        sort.stuck = true;
        ARROW.Add(sort);
        sort.nomProj = "Fleche";
        sort.element = EnumScript.Element.Feu;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 80;
        sort.stuck = true;
        ARROW.Add(sort);
        tabSort.Add(ARROW);
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
