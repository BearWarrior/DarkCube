using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct structSortDeZone
{
    public string nameInMenu;
    public string nomParticle;
    public float degats;
    public float cooldown;
    public float duree;
    public List<EnumScript.Element> listElement;

    public int lvl;
    public int xpActuel;
    public int nbXpPerShot;
    public int nbPointsDispo;
    public float degatsPerLevel;
    public float dureePerLevel;
    public float coolDownPerLevel;
    public int pointsInDuree;
    public int pointsInCooldown;
    public int pointsInDegats;
}

public class CaracZones : MonoBehaviour
{
    public int xpToLvlUp = 5;
    public float multXpByLvl = 1.25f;

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
        WALL.xpActuel = 0;
        WALL.degatsPerLevel = 2;
        WALL.dureePerLevel = 1;
        WALL.coolDownPerLevel = -1;
        WALL.pointsInDuree = 0;
        WALL.pointsInDegats = 0;
        WALL.pointsInCooldown = 0;
        WALL.nbXpPerShot = 1;
        WALL.lvl = 1;
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

    //utilisé pour l'édition
    public void setStruct(structSortDeZone sort)
    {
        for (int i = 0; i < tabSort.Count; i++)
        {
            if (tabSort[i].nomParticle.Equals(sort.nomParticle))
            {
                tabSort[i] = sort;
            }
        }
    }

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

    /* Points a sauvegarder :
    *   nomParticle
    *   lvl
    *   xpActuel
    *   nbPointsDispo
    *   pointsInVitesse
    *   pointsInCooldown
    *   pointsInDegats
    */
    public void sauvegarder()
    {
        PlayerPrefs.SetInt("CaracZones", tabSort.Count);
        for (int i = 0; i < tabSort.Count; i++)
        {
            string save = tabSort[i].nomParticle + ";" + tabSort[i].lvl + ";" + tabSort[i].xpActuel + ";" + tabSort[i].nbPointsDispo + ";" +
                          tabSort[i].pointsInDuree + ";" + tabSort[i].pointsInCooldown + ";" + tabSort[i].pointsInDegats; ;
            PlayerPrefs.SetString("CaracZones" + i, save);
            print(save);
        }
    }

    public void charger()
    {
        int nbSort = PlayerPrefs.GetInt("CaracZones", -1);
        if (nbSort != -1)
        {
            for (int i = 0; i < nbSort; i++)
            {
                string save = PlayerPrefs.GetString("CaracZones" + i, "default");
                if (save != "default")
                {
                    string[] array = save.Split(';');
                    for (int j = 0; j < tabSort.Count; j++)
                    {
                        if (array[0].Equals(tabSort[j].nomParticle))
                        {
                            structSortDeZone s = tabSort[j];
                            s.lvl = Int32.Parse(array[1]);
                            s.xpActuel = Int32.Parse(array[2]);
                            s.nbPointsDispo = Int32.Parse(array[3]);
                            s.pointsInDuree = Int32.Parse(array[4]);
                            s.pointsInCooldown = Int32.Parse(array[5]);
                            s.pointsInDegats = Int32.Parse(array[6]);
                            tabSort[j] = s;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void gagnerXP(string nameParticule)
    {
        for (int i = 0; i < tabSort.Count; i++)
        {
            if (tabSort[i].nomParticle.Equals(nameParticule))
            {
                structSortDeZone s = tabSort[i];
                s.xpActuel += s.nbXpPerShot;

                //LVL UP sort
                if (s.xpActuel > xpToLvlUp * Math.Pow(multXpByLvl, s.lvl))
                {
                    s.xpActuel -= (int)(xpToLvlUp * Math.Pow(multXpByLvl, s.lvl));
                    s.lvl++;
                    s.nbPointsDispo++;
                    GameObject.FindWithTag("Player").GetComponent<Player>().majSortsZoneEquip(s);
                }

                tabSort[i] = s;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            sauvegarder();
        }
    }

    public int getLvlFromNamePart(string nameParticule)
    {
        foreach (structSortDeZone sort in tabSort)
        {
            if (sort.nomParticle == nameParticule)
            {
                return sort.lvl;
            }
        }
        return 0;
    }
}
