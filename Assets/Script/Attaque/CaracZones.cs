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
    public List<EnumScript.Element> listElement;

    public float degatsPerLevel;
    public float vitessePerLevel;
    public float coolDownPerLevel;
}


public class CaracZones : MonoBehaviour
{
    public List<structSortDeZone> tabSort;

    public structSortDeZone WALL;

    public void Start()
    {
        tabSort = new List<structSortDeZone>();

        //WALL
        WALL.nomParticle = "Wall";
        WALL.nameInMenu = "Mur";
        WALL.listElement = new List<EnumScript.Element> { EnumScript.Element.Eau, EnumScript.Element.Feu };
        WALL.degats = 10;
        WALL.cooldown = 1;
        WALL.duree = 10;

        WALL.degatsPerLevel = 2;
        WALL.vitessePerLevel = 1;
        WALL.coolDownPerLevel = -1;
        tabSort.Add(WALL);
    }


    public structSortDeZone getStructFromName(string p_name)
    {
        foreach (structSortDeZone sorts in tabSort)
        {
            if (sorts.nomParticle.Equals(p_name))
            {
                return sorts;
            }
        }
        return tabSort[0];
    }

    /*public List<structSortDeZone> getZoneFromElement(EnumScript.Element p_elem)
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
    }*/

    public List<EnumScript.Element> getElemFromZone(string p_name)
    {
        foreach (structSortDeZone sort in tabSort)
        {
            if (sort.nomParticle == p_name)
            {
                return sort.listElement;
            }
        }
        return null;
    }
}
