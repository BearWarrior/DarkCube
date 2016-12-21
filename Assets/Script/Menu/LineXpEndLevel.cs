using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LineXpEndLevel : MonoBehaviour
{
    public GameObject nom;
    public GameObject lvl;
    public GameObject xp;
    public GameObject progressBar;

    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void display(string nom, string lvl, string xp, float xpAct)
    {
        this.nom.GetComponent<Text>().text = nom;
        this.lvl.GetComponent<Text>().text = lvl;
        this.xp.GetComponent<Text>().text = xp;
        this.progressBar.GetComponent<Image>().fillAmount = xpAct;
    }
}
