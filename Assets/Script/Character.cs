using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    protected float PDVmax;
    protected float PDVactuel;
    protected float armureMax;
    protected float armureActuel;
    //protected float enduranceMax;
    //protected float enduranceActuel;

    //protected float timeBeforeRunningMax;
    //protected float timeBeforeRunningAct;
    //protected bool regenEndurance;

    protected Attaque[] listAttaque;
    protected List<Attaque> listAttaqueInventaire;


    // Use this for initialization
    /*void Start ()
    {
	
	}*/
	
	// Update is called once per frame
	/*void Update ()
    {
	
	}*/

   

    //Coroutine de lancement de projectile en rafale (voir SortDeJet)
    public void launchProjRafale(int p_nbProj, float delay, SortDeJet.Del p_function)
    {
        StartCoroutine(coroutinelaunchProjRafale(p_nbProj, delay, p_function));
    }

    public IEnumerator coroutinelaunchProjRafale(int p_nbProj, float delay, SortDeJet.Del p_function)
    {
        p_function();
        for (int i = 0; i < p_nbProj - 1; i++)
        {
            yield return new WaitForSeconds(delay);
            p_function();
        }
    }






    public void equipeAttaqueAt(int face, int indexInList)
    {
        listAttaque[face - 1] = listAttaqueInventaire[indexInList];
    }

    public void desequipeAttaque(int face)
    {
        listAttaque[face - 1] = null;
    }

    public void addAttaqueToInventaire(Attaque atk)
    {
        string nom = checkDoubleName(atk.getNomSort());
        while (nom != checkDoubleName(nom))
            nom = checkDoubleName(nom);

        atk.setNomSort(nom);

        if (atk.type == 1)
            listAttaqueInventaire.Add(new SortDeJet((SortDeJet)atk));
        else if (atk.type == 2)
            listAttaqueInventaire.Add(new SortDeZone((SortDeZone)atk));
    }

    public string checkDoubleName(string name)
    {
        foreach (Attaque atk in listAttaqueInventaire)
        {
            if (name == atk.getNomSort())
            {
                return name + "_";
            }
        }
        return name;
    }

    public bool supprimerAttaqueInventaireAt(int p_index, bool forceDelete)
    {
        if (!forceDelete)
        {
            for (int i = 0; i < listAttaque.Length; i++)
            {
                if (listAttaque[i] != null)
                {
                    if (listAttaque[i].getNomSort().Equals(listAttaqueInventaire[p_index].getNomSort()))
                    {
                        return false;
                    }
                }
            }
            listAttaqueInventaire.RemoveAt(p_index);
            return true;
        }
        else
        {
            for (int i = 0; i < listAttaque.Length; i++)
            {
                if (listAttaque[i].getNomSort().Equals(listAttaqueInventaire[p_index].getNomSort()))
                {
                    listAttaque[i] = null;
                    break;
                }
            }
            listAttaqueInventaire.RemoveAt(p_index);
            return true;
        }
    }

    public virtual List<Attaque> getListAttaqueInventaire()
    {
        return listAttaqueInventaire;
    }

    public Attaque getAttaqueEquipOnFace(int face)
    {
        return listAttaque[face - 1];
    }

}
