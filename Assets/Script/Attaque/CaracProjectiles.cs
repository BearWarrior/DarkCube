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
    public EnumScript.Element element;
}


public class CaracProjectiles : MonoBehaviour
{
    public List<List<structSortJet>> tabSort;

    //public List<structSortJet> LIGHTNINGBALL;
    //public List<structSortJet> LASER;
    public List<structSortJet> METEOR;
    public List<structSortJet> WAVE;
    public List<structSortJet> BOMB;
    public List<structSortJet> SHOT;
    public List<structSortJet> BALL;

    public void Start()
    {
        tabSort = new List<List<structSortJet>>();

        structSortJet sort;

        //METEOR
        METEOR = new List<structSortJet>();
        sort.nameInMenu = "Météore";
        sort.nomParticle = "Meteor";
        sort.element = EnumScript.Element.Feu;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        METEOR.Add(sort);
        sort.nameInMenu = "Météore";
        sort.nomParticle = "Meteor";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        METEOR.Add(sort);
        sort.nameInMenu = "Météore";
        sort.nomParticle = "Meteor";
        sort.element = EnumScript.Element.Elec;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        METEOR.Add(sort);
        sort.nameInMenu = "Météore";
        sort.nomParticle = "Meteor";
        sort.element = EnumScript.Element.Toxic;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        METEOR.Add(sort);
        tabSort.Add(METEOR);

        //WAVE
        WAVE = new List<structSortJet>();
        sort.nameInMenu = "Vague";
        sort.nomParticle = "Wave";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        WAVE.Add(sort);
        tabSort.Add(WAVE);

        //BOMB
        BOMB = new List<structSortJet>();
        sort.nameInMenu = "Bombe";
        sort.nomParticle = "Bomb";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        BOMB.Add(sort);
        sort.nameInMenu = "Bombe";
        sort.nomParticle = "Bomb";
        sort.element = EnumScript.Element.Elec;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        BOMB.Add(sort);
        sort.nameInMenu = "Bombe";
        sort.nomParticle = "Bomb";
        sort.element = EnumScript.Element.Feu;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        BOMB.Add(sort);
        sort.nameInMenu = "Bombe";
        sort.nomParticle = "Bomb";
        sort.element = EnumScript.Element.Toxic;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        BOMB.Add(sort);
        tabSort.Add(BOMB);

        //SHOT
        SHOT = new List<structSortJet>();
        sort.nameInMenu = "Tir";
        sort.nomParticle = "Shot";
        sort.element = EnumScript.Element.Feu;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        SHOT.Add(sort);
        sort.nameInMenu = "Tir";
        sort.nomParticle = "Shot";
        sort.element = EnumScript.Element.Elec;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        SHOT.Add(sort);
        sort.nameInMenu = "Tir";
        sort.nomParticle = "Shot";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        SHOT.Add(sort);
        tabSort.Add(SHOT);

        //BALL
        BALL = new List<structSortJet>();
        sort.nameInMenu = "Boule";
        sort.nomParticle = "Ball";
        sort.element = EnumScript.Element.Feu;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        BALL.Add(sort);
        sort.nameInMenu = "Boule";
        sort.nomParticle = "Ball";
        sort.element = EnumScript.Element.Elec;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        BALL.Add(sort);
        sort.nameInMenu = "Boule";
        sort.nomParticle = "Ball";
        sort.element = EnumScript.Element.Eau;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        BALL.Add(sort);
        sort.nameInMenu = "Boule";
        sort.nomParticle = "Ball";
        sort.element = EnumScript.Element.Toxic;
        sort.degats = 10;
        sort.cooldown = 0.5f;
        sort.vitesse = 10;
        BALL.Add(sort);
        tabSort.Add(BALL);
    }


    public structSortJet getStructFromNameAndElement(string p_name, EnumScript.Element p_elem)
    {
        foreach (List<structSortJet> sorts in tabSort)
        {
            if (sorts[0].nomParticle.Equals(p_name))
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
