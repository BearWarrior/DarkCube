using UnityEngine;
using System.Collections;

public class PortalBehaviour : MonoBehaviour 
{
    public bool lastPortal = false; //Si c'est le dernier portal, on ne TP pas
    public int direction;
    public WorldBehaviour wolrdBehaviour;
    public bool usable; //Seul les portals de début sont à false, il faut sortir du portail pour l'activer

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!lastPortal)
            {
                if (usable)
                {
                    wolrdBehaviour.changeRoom(direction); //changement de salle
                }
            }
            else
            {
                GameObject.FindWithTag("CanvasEndDungeon").transform.GetChild(0).transform.gameObject.SetActive(true);

                other.GetComponent<PlayerController>().setControllable(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            usable = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            usable = true;
        }
    }
}
