using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct structSortDeZone
{
    public string nameInMenu;
    public string nomParticle;
    public float degats;
    public float cooldown;
    public float duree;
    public EnumScript.Element element;
}


public class CaracZones : MonoBehaviour
{
    public List<List<structSortDeZone>> tabSort;

    public List<structSortDeZone> DEFAULT;
    public List<structSortDeZone> BRASIER;
    public List<structSortDeZone> PLUIE_DE_FLECHE;
    public List<structSortDeZone> PLUIE_DE_GLACE;

    public List<structSortDeZone> WALL;

    public void Start()
    {
        tabSort = new List<List<structSortDeZone>>();

        structSortDeZone sort;

        //WALL
        WALL = new List<structSortDeZone>();
        sort.nomParticle = "Wall";
        sort.nameInMenu = "Mur";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 1;
        sort.duree = 10;
        WALL.Add(sort);
        sort.nomParticle = "Wall";
        sort.nameInMenu = "Mur";
        sort.element = EnumScript.Element.Feu;
        sort.degats = 10;
        sort.cooldown = 1;
        sort.duree = 10;
        WALL.Add(sort);
        sort.nomParticle = "Wall";
        sort.nameInMenu = "NE PAS CREER";
        sort.element = EnumScript.Element.Elec;
        sort.degats = 10;
        sort.cooldown = 1;
        sort.duree = 10;
        WALL.Add(sort);
        sort.nomParticle = "Wall";
        sort.nameInMenu = "NE PAS CREER";
        sort.element = EnumScript.Element.Toxic;
        sort.degats = 10;
        sort.cooldown = 1;
        sort.duree = 10;
        WALL.Add(sort);
        tabSort.Add(WALL);
    }


    public structSortDeZone getStructFromNameAndElement(string p_name, EnumScript.Element p_elem)
    {
        foreach (List<structSortDeZone> sorts in tabSort)
        {
            if (sorts[0].nomParticle.Equals(p_name))
            {
                foreach (structSortDeZone sort in sorts)
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

    public List<structSortDeZone> getZoneFromElement(EnumScript.Element p_elem)
    {
        List<structSortDeZone> listZone= new List<structSortDeZone>();
        if (tabSort == null)
            Start();
        foreach (List<structSortDeZone> sorts in tabSort)
        {
            foreach (structSortDeZone sort in sorts)
            {
                if (sort.element == p_elem)
                {
                    listZone.Add(sort);
                    break;
                }
            }
        }
        return listZone;
    }
}
