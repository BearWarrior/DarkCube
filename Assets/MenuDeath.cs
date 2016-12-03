using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuDeath : MonoBehaviour
{
    public GameObject lineXp;
    public GameObject caracSorts;
    private Vector3 posInit;
    private float offsetY;
    public GameObject containerLine;

	// Use this for initialization
	void Start ()
    {
        posInit = new Vector3(0, 180, 0);
        offsetY = 70;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void displayResults(List<string> nomSorts, List<int> types)
    {
        int lvl = 0;
        int xpGagne = 0;
        float xpAct = 0;

        for(int i = 0; i < nomSorts.Count; i++)
        {
            if (types[i] == 1)
            {
                structSortJet structure = caracSorts.GetComponent<CaracProjectiles>().getStructFromName(nomSorts[i]);
                lvl = structure.lvl;
                xpGagne = structure.xpTemp;
                xpAct = structure.xpActuel / caracSorts.GetComponent<CaracProjectiles>().xpToLvlUp * Mathf.Pow(caracSorts.GetComponent<CaracProjectiles>().multXpByLvl, lvl);
            }
            else
            {
                structSortDeZone structure = caracSorts.GetComponent<CaracZones>().getStructFromName(nomSorts[i]);
                lvl = structure.lvl;
                xpGagne = structure.xpTemp;
                xpAct = structure.xpActuel / caracSorts.GetComponent<CaracZones>().xpToLvlUp * Mathf.Pow(caracSorts.GetComponent<CaracZones>().multXpByLvl, lvl);
            }



            GameObject line = Instantiate(lineXp, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), containerLine.transform) as GameObject;
            line.transform.localPosition = posInit - i * new Vector3(0, offsetY, 0);
            line.GetComponent<LineXpEndLevel>().display(nomSorts[i], lvl.ToString(), xpGagne.ToString(), xpAct);
        }
    }

    public void quitter()
    {
        GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().sauvegarder();
        GameObject.FindWithTag("CaracSorts").GetComponent<CaracZones>().sauvegarder();
        SceneManager.LoadScene("CustomRoom");
    }
}
