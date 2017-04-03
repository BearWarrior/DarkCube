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






    public virtual void equipeAttaqueAt(int face, int indexInList)
    {
        if(listAttaque[face-1] != null)
            listAttaqueInventaire.Add(listAttaque[face - 1]);
        listAttaque[face - 1] = listAttaqueInventaire[indexInList];
        listAttaqueInventaire.RemoveAt(indexInList);
    }

    public void desequipeAttaque(int face)
    {
        listAttaqueInventaire.Add(listAttaque[face - 1]);
        print(listAttaqueInventaire.Count);
        listAttaque[face - 1] = null;
    }

    public void addAttaqueToInventaire(Attaque atk)
    {
        string nom = checkDoubleName(atk.getPseudoSort());
        while (nom != checkDoubleName(nom))
            nom = checkDoubleName(nom);

        atk.setPseudoSort(nom);

        if (atk.type == 1)
            listAttaqueInventaire.Add(new SortDeJet((SortDeJet)atk));
        else if (atk.type == 2)
            listAttaqueInventaire.Add(new SortDeZone((SortDeZone)atk));
    }

    public string checkDoubleName(string name)
    {
        foreach (Attaque atk in listAttaqueInventaire)
        {
            if (name == atk.getPseudoSort())
            {
                return name + "_";
            }
        }
        return name;
    }

    public void supprimerAttaqueInventaireAt(int p_index, bool sortEquipe)
    {
        if (!sortEquipe)
            listAttaqueInventaire.RemoveAt(p_index);
        else
            listAttaque[p_index-1] = null;
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
