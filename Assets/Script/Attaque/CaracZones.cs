using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct structSortDeZone
{
    public EnumScript.Element element;
    public string nomZone;
    public float degats;
    public float cooldown;
    public float duree;
    public string partSysStr;
    public EnumScript.GabaritSortDeZone gabarit;
}


public class CaracZones : MonoBehaviour
{
    public List<List<structSortDeZone>> tabSort;

    public List<structSortDeZone> DEFAULT;
    public List<structSortDeZone> BRASIER;
    public List<structSortDeZone> PLUIE_DE_FLECHE;
    public List<structSortDeZone> PLUIE_DE_GLACE;

    public void Start()
    {
        tabSort = new List<List<structSortDeZone>>();

        structSortDeZone sort;

        //BRASIER
        BRASIER = new List<structSortDeZone>();
        sort.nomZone = "Brasier";
        sort.element = EnumScript.Element.Feu;
        sort.degats = 10;
        sort.cooldown = 1;
        sort.duree = 10;
        sort.partSysStr = "fire_ground/Brasier";
        sort.gabarit = EnumScript.GabaritSortDeZone.Ligne;
        BRASIER.Add(sort);
        tabSort.Add(BRASIER);

        //PLUIE FLECHE
        PLUIE_DE_FLECHE = new List<structSortDeZone>();
        sort.nomZone = "Pluie de flêche";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 1;
        sort.duree = 10;
        sort.partSysStr = "none";
        sort.gabarit = EnumScript.GabaritSortDeZone.Cercle;
        PLUIE_DE_FLECHE.Add(sort);
        tabSort.Add(PLUIE_DE_FLECHE);

        //PLUIE GLACE
        PLUIE_DE_GLACE = new List<structSortDeZone>();
        sort.nomZone = "Pluie de glace";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 1;
        sort.duree = 10;
        sort.partSysStr = "none";
        sort.gabarit = EnumScript.GabaritSortDeZone.Cercle;
        PLUIE_DE_GLACE.Add(sort);
        sort.nomZone = "Pluie de glace";
        sort.element = EnumScript.Element.Elec;
        sort.degats = 10;
        sort.cooldown = 1;
        sort.duree = 10;
        sort.partSysStr = "none";
        sort.gabarit = EnumScript.GabaritSortDeZone.Cercle;
        PLUIE_DE_GLACE.Add(sort);
        tabSort.Add(PLUIE_DE_GLACE);
    }


    public structSortDeZone getStructFromNameAndElement(string p_name, EnumScript.Element p_elem)
    {
        foreach (List<structSortDeZone> sorts in tabSort)
        {
            if (sorts[0].nomZone.Equals(p_name))
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
