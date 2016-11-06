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
    public float degatsPerLevel;
    public float vitessePerLevel;
    public float coolDownPerLevel;
    public int nbXpPerShot;
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
        WALL.vitessePerLevel = 1;
        WALL.coolDownPerLevel = -1;
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

    public void sauvegarder()
    {
        PlayerPrefs.SetInt("CaracZones", tabSort.Count);
        for (int i = 0; i < tabSort.Count; i++)
        {
            string save = tabSort[i].nomParticle + ";" + tabSort[i].lvl + ";" + tabSort[i].xpActuel;
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
