using UnityEngine;
using System.Collections;

public class accessMenu : MonoBehaviour
{
    bool inMenu;

    public GameObject menu;
    public GameObject menuActif;
    private GameObject lookAt;
    private GameObject player;


	// Use this for initialization
	void Start () 
    {
        inMenu = false;

        menu.SetActive(false);

        lookAt = new GameObject(); //Ne pas supprimer (voir ligne 34)
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lookAt.transform.position = other.transform.GetChild(3).position;
            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        player = null;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!inMenu)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    inMenu = true;
                    //On retire le control du joueur
                    other.GetComponent<PlayerController>().setControllable(false);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    other.transform.parent = this.transform;

                    Camera.main.GetComponent<CameraController>().playerInMenu = true;

                    //On enleve le crossHair
                    Camera.main.GetComponent<Crosshair>().display = false;

                    //On affiche le menu
                    menu.SetActive(true);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    exitMenu();
                }
            }


            if(inMenu)
            {
                //On le tourne vers (0, 45, 0)
                Quaternion quat = new Quaternion(0, 0.4f, 0, 0.9f); //(0, 45, 0)
                if (Quaternion.Angle(other.transform.localRotation, quat) > .2f)
                    other.transform.localRotation = Quaternion.Lerp(other.transform.localRotation, quat, 5 * Time.deltaTime);

                //On le déplace au centre (en le laissant au sol)
                Vector3 from = new Vector3(other.transform.localPosition.x, 0, other.transform.localPosition.z);
                Vector3 to = new Vector3(this.GetComponent<BoxCollider>().center.x, 0, this.GetComponent<BoxCollider>().center.z);
                Vector3 oldPlayer = other.transform.localPosition;
                if (Vector3.Distance(from, to) > .1f)
                    other.transform.localPosition = Vector3.Lerp(other.transform.localPosition, this.GetComponent<BoxCollider>().center, 3 * Time.deltaTime);
                other.transform.localPosition = new Vector3(other.transform.localPosition.x, oldPlayer.y, other.transform.localPosition.z);

                //Camera
                lookAt.transform.position = Vector3.Lerp(lookAt.transform.position, menuActif.transform.position, .05f);
                Camera.main.GetComponent<CameraController>().cameraLookAtMenuTR = lookAt.transform.position;
            }          
        }
    }

    public void exitMenu()
    {
        if(player != null)
        {
            Debug.Log("exitMenu");
            inMenu = false;
            //On redonne le control au joueur
            player.GetComponent<PlayerController>().setControllable(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            player.transform.parent = null;

            //On change le point de vue de la caméra
            Camera.main.GetComponent<CameraController>().playerInMenu = false;

            //On remet le crossHair
            Camera.main.GetComponent<Crosshair>().display = true;

            //On désactive le menu
            menu.SetActive(false);

            //On actualise le sort qu'il avait en main
            player.GetComponent<Player>().cubeFaceChanged(player.GetComponent<Player>().getCubeFace());
        }
    }
}
