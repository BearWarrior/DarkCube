using UnityEngine;
using System.Collections;

public class PortalBehaviour : MonoBehaviour 
{
    public bool lastPortal = false; //Si c'est le dernier portal, on ne TP pas
    public int direction;
    public WorldBehaviour wolrdBehaviour;
    public bool usable; //Seul les portals de début sont à false, il faut sortir du portail pour l'activer

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        //TODO vérifier qu'il n'y ai plus d'enemy
        if (other.tag == "Player")
        {
            if(!lastPortal)
                if (usable)
                {
                    wolrdBehaviour.changeRoom(direction); //changement de salle
                }
            usable = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //TODO vérifier qu'il n'y ai plus d'enemy
        if (other.tag == "Player")
        {
            usable = true;
        }
    }
}
