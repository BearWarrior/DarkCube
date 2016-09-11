using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCaracteristics : MonoBehaviour 
{
    private Attaque[] listAttaqueEquiped;
    private List<Attaque> listAttaqueInventaire;

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this);
        listAttaqueEquiped = new Attaque[6];
        listAttaqueInventaire = new List<Attaque>();
        listAttaqueInventaire.Add(new SortDeJet(1, 45, 0.05f, 10, EnumScript.Element.Metal, "Canon", "canon", EnumScript.PatternSortDeJet.Rafale));
        listAttaqueInventaire.Add(new SortDeJet(1, 60, 2, 10, EnumScript.Element.Eau, "Prisme", "Prisme", EnumScript.PatternSortDeJet.SimultaneLigne));
        listAttaqueInventaire.Add(new SortDeJet(3, 70, 2, 10, EnumScript.Element.Eau, "Shuriken", "ShuShu", EnumScript.PatternSortDeJet.SimultaneTriangle));
        listAttaqueInventaire.Add(new SortDeJet(4, 45, 2, 10, EnumScript.Element.Metal, "Canon", "4ligne", EnumScript.PatternSortDeJet.SimultaneLigne));
        listAttaqueInventaire.Add(new SortDeJet(4, 45, 2, 10, EnumScript.Element.Eau, "Prisme", "Prisme-rafale", EnumScript.PatternSortDeJet.Rafale));
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void equipeAttaqueAt(int face, int indexInList)
    {
        if(indexInList != -1)
            listAttaqueEquiped[face - 1] = listAttaqueInventaire[indexInList];
        else
            listAttaqueEquiped[face - 1] = null;
    }

    public void addAttaqueToInventaire(SortDeJet sort)
    {
        string nom = checkDoubleName(sort.getNomSort());
        while (nom != checkDoubleName(nom))
            nom = checkDoubleName(nom);

        sort.setNomSort(nom);
        listAttaqueInventaire.Add(new SortDeJet(sort));
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

    public List<Attaque> getListAttaqueInventaire()
    {
        return listAttaqueInventaire;
    }

    public Attaque getAttaqueEquipOnFace(int face)
    {
        return listAttaqueEquiped[face-1];
    }

    public Attaque[] getListAttaqueEquiped()
    {
        return listAttaqueEquiped;
    }

    public bool supprimerAttaqueInventaireAt(int p_index, bool forceDelete)
    {
        if (!forceDelete)
        {
            for (int i = 0; i < listAttaqueEquiped.Length; i++)
            {
                if (listAttaqueEquiped[i] != null)
                {
                    if (listAttaqueEquiped[i].getNomSort().Equals(listAttaqueInventaire[p_index].getNomSort()))
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
            for (int i = 0; i < listAttaqueEquiped.Length; i++)
            {
                if (listAttaqueEquiped[i].getNomSort().Equals(listAttaqueInventaire[p_index].getNomSort()))
                {
                    listAttaqueEquiped[i] = null;
                    break;
                }
            }
            listAttaqueInventaire.RemoveAt(p_index);
            return true;
        }
    }
}
