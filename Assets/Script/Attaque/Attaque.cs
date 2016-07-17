using UnityEngine;
using System.Collections;

public class Attaque
{
    public int type; //1-Jet  2-Zone

	protected float cooldown;
    protected float lastShot;
    protected bool canShoot = true;

    protected float degats;

    protected EnumScript.Element element;

    protected string nomSort;

    protected EnumScript.EffetPhysique effetPhysique;
    

    public virtual void Attaquer()
    {

    }

    public void reload()
    {
        if (lastShot + cooldown < Time.time )
            canShoot = true;
    }

    public string getNomSort()
    {
        return nomSort;
    }

    public void setNomSort(string p_nom)
    {
        nomSort = p_nom;
    }


    public float getCooldown()
    {
        return cooldown;
    }

    public void setCooldown(float cd)
    {
        cooldown = cd;
    }


    public float getDegats()
    {
        return degats;
    }

    public void setDegats(float dg)
    {
        degats = dg;
    }

    public EnumScript.Element getElement()
    {
        return element;
    }

    public void setElement(EnumScript.Element elem)
    {
        element = elem;
    }





    void Start()
    {

    }
    void Update()
    {

    }
}
