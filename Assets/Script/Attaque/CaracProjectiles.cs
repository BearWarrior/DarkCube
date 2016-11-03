using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct structSortJet
{
    public string nameInMenu;
    public string nomParticle;
    public float degats;
    public float cooldown;
    public float vitesse;
    public List<EnumScript.Element> listElement;

    public float degatsPerLevel;
    public float vitessePerLevel;
    public float coolDownPerLevel;
    public int nbXpPerShot;
}


public class CaracProjectiles : MonoBehaviour
{
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
        METEOR.cooldown = 0.5f;
        METEOR.vitesse = 10;
        METEOR.degatsPerLevel = 2;
        METEOR.vitessePerLevel = 1;
        METEOR.coolDownPerLevel = -1;
        METEOR.nbXpPerShot = 1;
        tabSort.Add(METEOR);

        //WAVE
        WAVE.nameInMenu = "Vague";
        WAVE.nomParticle = "Wave";
        WAVE.listElement = new List<EnumScript.Element> { EnumScript.Element.Eau};
        WAVE.degats = 10;
        WAVE.cooldown = 0.5f;
        WAVE.vitesse = 10;
        WAVE.degatsPerLevel = 2;
        WAVE.vitessePerLevel = 1;
        WAVE.coolDownPerLevel = -1;
        WAVE.nbXpPerShot = 1;
        tabSort.Add(WAVE);

        //BOMB
        BOMB.nameInMenu = "Bombe";
        BOMB.nomParticle = "Bomb";
        BOMB.listElement = new List<EnumScript.Element> { EnumScript.Element.Feu, EnumScript.Element.Eau, EnumScript.Element.Elec, EnumScript.Element.Toxic };
        BOMB.degats = 10;
        BOMB.cooldown = 0.5f;
        BOMB.vitesse = 10;
        BOMB.degatsPerLevel = 2;
        BOMB.vitessePerLevel = 1;
        BOMB.coolDownPerLevel = -1;
        BOMB.nbXpPerShot = 1;
        tabSort.Add(BOMB);

        //SHOT
        SHOT.nameInMenu = "Tir";
        SHOT.nomParticle = "Shot";
        SHOT.listElement = new List<EnumScript.Element> { EnumScript.Element.Feu, EnumScript.Element.Eau, EnumScript.Element.Elec};
        SHOT.degats = 10;
        SHOT.cooldown = 0.5f;
        SHOT.vitesse = 10;
        SHOT.degatsPerLevel = 2;
        SHOT.vitessePerLevel = 1;
        SHOT.coolDownPerLevel = -1;
        SHOT.nbXpPerShot = 1;
        tabSort.Add(SHOT);

        //BALL
        BALL.nameInMenu = "Boule";
        BALL.nomParticle = "Ball";
        BALL.listElement = new List<EnumScript.Element> { EnumScript.Element.Feu, EnumScript.Element.Eau, EnumScript.Element.Elec, EnumScript.Element.Toxic };
        BALL.degats = 10;
        BALL.cooldown = 0.5f;
        BALL.vitesse = 10;
        BALL.degatsPerLevel = 2;
        BALL.vitessePerLevel = 1;
        BALL.coolDownPerLevel = -1;
        BALL.nbXpPerShot = 1;
        tabSort.Add(BALL);
    }


    public structSortJet getStructFromName(string p_name)
    {
        foreach (structSortJet sorts in tabSort)
        {
            if (sorts.nomParticle.Equals(p_name))
            {
                return sorts;
            }
        }
        return tabSort[0];
    }

    public List<EnumScript.Element> getElemFromProj(string p_name)
    {
        foreach(structSortJet sort in tabSort)
        {
            if(sort.nomParticle == p_name)
            {
                return sort.listElement;
            }
        }
        return null;
    }
}
