using UnityEngine;
using System.Collections;

public abstract class Attaque
{
    public int type; //1-Jet   2-Zone   3-Support
    protected int lvl;
	protected float cooldown;
    protected float lastShot;
    protected bool canShoot = true;
    protected float degats;
    protected EnumScript.Element element;
    protected string pseudoSort; //Name given by the player (e.g. SortTropStyley)
    protected string nameParticle; //Name of the particle to instantiate (e.g. RainMeteor)
    protected string nameInMenu; //Name as it appears in the menu (e.g. Pluie de Météorites)

    protected EnumScript.EffetPhysique effetPhysique;


    public abstract void AttackFromPlayer(Vector3 spawnPoint);
    public abstract void AttackFromEnemy(RaycastHit hit, Vector3 spawnPoint);

    public void reload()
    {
        if (lastShot + cooldown < Time.time )
            canShoot = true;
    }

    public void setAllTagsAndAddVelocityAndEmitter(string tag, GameObject go, Vector3 velocity, EnumScript.Character emitter)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).tag = tag;
            if (go.GetComponent<Rigidbody>() != null)
                go.GetComponent<Rigidbody>().velocity = velocity;
            setAllTagsAndAddVelocityAndEmitter(tag, go.transform.GetChild(i).transform.gameObject, velocity, emitter);
            if (go.GetComponent<ParticleCollisionBehaviour>() != null)
            {
                go.GetComponent<ParticleCollisionBehaviour>().emiter = emitter;
            }
        }
    }

    //pseudoSort
    public string getPseudoSort()
    {
        return pseudoSort;
    }
    public void setPseudoSort(string p_pseudo)
    {
        pseudoSort = p_pseudo;
    }
    //nameParticle
    public string getNameParticle()
    {
        return nameParticle;
    }
    public void setNameParticle(string p_namePart)
    {
        nameParticle = p_namePart;
    }
    //nameInMenu
    public string getNameInMenu()
    {
        return nameInMenu;
    }
    public void setNameInMenu(string p_nameInMenu)
    {
        nameInMenu = p_nameInMenu;
    }

    //Cooldown
    public float getCooldown()
    {
        return cooldown;
    }
    public void setCooldown(float cd)
    {
        cooldown = cd;
    }

    //degats
    public float getDegats()
    {
        return degats;
    }
    public void setDegats(float dg)
    {
        degats = dg;
    }

    //element
    public EnumScript.Element getElement()
    {
        return element;
    }
    public void setElement(EnumScript.Element elem)
    {
        element = elem;
    }

    //lvl
    public int getLvl()
    {
        return lvl;
    }
    public void setLvl(int p_lvl)
    {
        lvl = p_lvl;
    }
}
