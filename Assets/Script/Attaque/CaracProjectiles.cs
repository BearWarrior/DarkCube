﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct structSortJet
{
    public string nameInMenu;
    public string nomParticle;
    public float degats;
    public float cooldown;
    public float vitesse;
    public List<EnumScript.Element> listElement;

    public int lvl;
    public float xpActuel;
    public int lvlPrevious; //lvl before beginning a room
    public float xpPrevious; //xp before beginning a room
    public float xpTemp;
    public int nbXpPerShot;
    public int nbPointsDispo;
    public float degatsPerLevel; 
    public float vitessePerLevel;
    public float coolDownPerLevel;
}

public class CaracProjectiles : MonoBehaviour
{
    public int xpToLvlUp = 5;
    public float multXpByLvl = 1.25f;

    public float coeffCustom2MultiProj = 0.5f;
    public int nbPointsToUpCustom2 = 10;

    public int nbMultiProjBase = 2;

    public List<structSortJet> tabSort;

    public structSortJet METEOR;
    public structSortJet WAVE;
    public structSortJet BOMB;
    public structSortJet SHOT;
    public structSortJet BALL;

    public void Start()
    {
        tabSort = new List<structSortJet>();

        //METEOR
        METEOR.nameInMenu = "Météore";
        METEOR.nomParticle = "Meteor";
        METEOR.listElement = new List<EnumScript.Element>{ EnumScript.Element.Feu, EnumScript.Element.Eau, EnumScript.Element.Elec, EnumScript.Element.Toxic };
        METEOR.degats = 10;
        METEOR.cooldown = 1f;
        METEOR.vitesse = 10;
        METEOR.xpActuel = 0;
        METEOR.xpTemp = 0;
        METEOR.degatsPerLevel = 2;
        METEOR.vitessePerLevel = 1;
        METEOR.coolDownPerLevel = -.1f;
        METEOR.nbXpPerShot = 1;
        METEOR.lvl = 1;
        tabSort.Add(METEOR);

        //WAVE
        WAVE.nameInMenu = "Vague";
        WAVE.nomParticle = "Wave";
        WAVE.listElement = new List<EnumScript.Element> { EnumScript.Element.Eau};
        WAVE.degats = 10;
        WAVE.cooldown = 1f;
        WAVE.vitesse = 10;
        WAVE.xpActuel = 0;
        WAVE.xpTemp = 0;
        WAVE.degatsPerLevel = 2;
        WAVE.vitessePerLevel = 1;
        WAVE.coolDownPerLevel = -.1f;
        WAVE.nbXpPerShot = 1;
        WAVE.lvl = 1;
        tabSort.Add(WAVE);

        //BOMB
        BOMB.nameInMenu = "Bombe";
        BOMB.nomParticle = "Bomb";
        BOMB.listElement = new List<EnumScript.Element> { EnumScript.Element.Feu, EnumScript.Element.Eau, EnumScript.Element.Elec, EnumScript.Element.Toxic };
        BOMB.degats = 10;
        BOMB.cooldown = 1f;
        BOMB.vitesse = 10;
        BOMB.xpActuel = 0;
        BOMB.xpTemp = 0;
        BOMB.degatsPerLevel = 2;
        BOMB.vitessePerLevel = 1;
        BOMB.coolDownPerLevel = -.1f;
        BOMB.nbXpPerShot = 1;
        BOMB.lvl = 1;
        tabSort.Add(BOMB);

        //SHOT
        SHOT.nameInMenu = "Tir";
        SHOT.nomParticle = "Shot";
        SHOT.listElement = new List<EnumScript.Element> { EnumScript.Element.Feu, EnumScript.Element.Eau, EnumScript.Element.Elec};
        SHOT.degats = 10;
        SHOT.cooldown = 1f;
        SHOT.vitesse = 10;
        SHOT.xpActuel = 0;
        SHOT.xpTemp = 0;
        SHOT.degatsPerLevel = 2;
        SHOT.vitessePerLevel = 1;
        SHOT.coolDownPerLevel = -.1f;
        SHOT.nbXpPerShot = 1;
        SHOT.lvl = 1;
        tabSort.Add(SHOT);

        //BALL
        BALL.nameInMenu = "Boule";
        BALL.nomParticle = "Ball";
        BALL.listElement = new List<EnumScript.Element> { EnumScript.Element.Feu, EnumScript.Element.Eau, EnumScript.Element.Elec, EnumScript.Element.Toxic };
        BALL.degats = 10;
        BALL.cooldown = 1f;
        BALL.vitesse = 10;
        BALL.xpActuel = 0;
        BALL.xpTemp = 0;
        BALL.degatsPerLevel = 2;
        BALL.vitessePerLevel = 1;
        BALL.coolDownPerLevel = -.1f;
        BALL.nbXpPerShot = 1;
        BALL.lvl = 1;
        tabSort.Add(BALL);


        Load();
    }


    public structSortJet getStructFromName(string p_name)
    {
        foreach (structSortJet sorts in tabSort)
            if (sorts.nomParticle.Equals(p_name))
                return sorts;
        return tabSort[0];
    }
    
    //utilisé pour l'édition
    public void setStruct(structSortJet sort)
    {
        for(int i = 0; i < tabSort.Count; i++)
            if (tabSort[i].nomParticle.Equals(sort.nomParticle))
                tabSort[i] = sort;
    }

    public List<EnumScript.Element> getElemFromProj(string p_name)
    {
        foreach(structSortJet sort in tabSort)
            if(sort.nomParticle == p_name)
                return sort.listElement;
        return null;
    }

    /*  To save :
    *   nomParticle
    *   lvl
    *   xpActuel
    *   nbPointsDispo
    */
    public void Save()
    {
        PlayerPrefs.SetInt("CaracProjectiles", tabSort.Count);
        for(int i = 0; i < tabSort.Count; i++)
        {
            string save = tabSort[i].nomParticle + ";" + tabSort[i].lvl + ";" + tabSort[i].xpActuel + ";" + tabSort[i].nbPointsDispo + ";";
            PlayerPrefs.SetString("CaracProjectiles" + i, save);
        }
        if (GameObject.Find("Saving") != null)
            GameObject.Find("Saving").GetComponent<SavingLogo>().DisplayLogo();
    }

    public void Load()
    {
        int nbSort = PlayerPrefs.GetInt("CaracProjectiles", -1);
        if(nbSort != -1)
        {
            for(int i = 0; i < nbSort; i++)
            {
                string save = PlayerPrefs.GetString("CaracProjectiles" + i, "default");
                if(save != "default")
                {
                    string[] array = save.Split(';');
                    for(int j = 0; j < tabSort.Count; j++)
                    {
                        if(array[0].Equals(tabSort[j].nomParticle))
                        {
                            structSortJet s = tabSort[j];
                            s.lvl = Int32.Parse(array[1]);
                            s.lvlPrevious = s.lvl;
                            s.xpActuel = float.Parse(array[2]);
                            s.xpPrevious = s.xpActuel;
                            s.nbPointsDispo = Int32.Parse(array[3]);
                            tabSort[j] = s;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void gagnerXP(string nameParticule, float divider)
    {  
        for(int i = 0; i < tabSort.Count; i++)
        {
            if (tabSort[i].nomParticle.Equals(nameParticule))
            {
                structSortJet s = tabSort[i];
                //Debug.Log("Gagner XP : " + nameParticule + "  nbPart : " + divider + "   xpGagné : " + (float)(s.nbXpPerShot) / divider);
                s.xpActuel += (float)(s.nbXpPerShot) / divider;
                s.xpTemp += (float)(s.nbXpPerShot / divider);

                //LVL UP sort
                if (s.xpActuel > xpToLvlUp * Math.Pow(multXpByLvl, s.lvl))
                {
                    //print("lvl UP");
                    s.xpActuel -= (float)(xpToLvlUp * Math.Pow(multXpByLvl, s.lvl));
                    s.lvl++;
                    s.nbPointsDispo++;
                    GameObject.FindWithTag("Player").GetComponent<Player>().majSortsProjEquip(s);
                }
                tabSort[i] = s;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Save();
    }

    public int getLvlFromNamePart(string nameParticule)
    {
        foreach (structSortJet sort in tabSort)
            if (sort.nomParticle == nameParticule)
                return sort.lvl;
        return 0;
    }
}
