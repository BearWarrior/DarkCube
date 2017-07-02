using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuDeath : MonoBehaviour
{
    public GameObject lineXp;
    public GameObject caracSorts;
    private Vector3 posInitLine;
    private float offsetYLine;
    public GameObject containerLine;
    public GameObject text;

    private string textDead = "Vous êtes mort.";
    private string textAlive = "Niveau terminé.";

    private List<GameObject> listLines;

    // Use this for initialization
    void Start ()
    {
        posInitLine = new Vector3(0, 180, 0);
        offsetYLine = 70;
        listLines = new List<GameObject>();
    }

    public void displayResults(List<string> nomSorts, List<string> nomsPart, List<int> types, bool dead)
    {
        //Affichage text Win/Lose
        text.GetComponent<Text>().text = (dead) ? textDead : textAlive;
        //One line per sort
        for (int i = 0; i < nomSorts.Count; i++)
        {
            GameObject line = Instantiate(lineXp, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), containerLine.transform) as GameObject;
            line.transform.localPosition = posInitLine - i * new Vector3(0, offsetYLine, 0);
            line.GetComponent<LineXpEndLevel>().display(caracSorts, nomSorts[i], nomsPart[i], types[i]);
            listLines.Add(line);
        }
    }

    public void quitter()
    {
        //Sauvegarde + retour CustomRoom
        GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().Save();
        GameObject.FindWithTag("CaracSorts").GetComponent<CaracZones>().Save();
        SceneManager.LoadScene("RoomMaintenance");
    }

    public void animateMenu()
    {
        foreach(GameObject line in listLines)
            line.GetComponent<LineXpEndLevel>().animate();
    }
}