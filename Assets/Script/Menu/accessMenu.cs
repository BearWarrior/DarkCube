using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class accessMenu : MonoBehaviour, IInputsObservable
{
    public GameObject menu;
    public GameObject menuActif;
    public GameObject playerLookAt;
    public GameObject cameraLookAt;

    private GameObject player;
    private bool inMenu;

    public GameObject menuAssociatedWith; 
    
    //Camera
    public GameObject posCameraMenu;
    public float cameraToMenuSpeed;

    //Center Cube
    private float centerStartTime;
    private float centerSpeed;
    private float centerJourneyLength;
    private Vector3 centerFrom;
    private Vector3 centerTo;
    private Vector3 centerOldPlayer;

    //Used en MenuManagerInGame to exit or not the menu when escape pressed
    private bool menuReady;

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    void Awake()
    {
        keys = GameObject.FindWithTag("InputsLoader").GetComponent<InputsLoader>().lookAtInputs(this.gameObject);
    }

    public void keysChanged(Dictionary<string, KeyCode> keys)
    {
        this.keys = keys;
    }

    // Use this for initialization
    void Start () 
    {
        inMenu = false;
        menuReady = false;
        
        centerSpeed = .5f;
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            other.GetComponent<PlayerController>().displayInteractionHelper(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player = null;
            other.GetComponent<PlayerController>().displayInteractionHelper(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!inMenu)
            {
                //if (Input.GetKeyDown(KeyCode.E))
                if (Input.GetKeyDown(keys["Interact"]))
                {
                    if (player.GetComponent<PlayerController>().getControllable())
                    {
                        inMenu = true;
                        //On centre le cube sur la platform
                        centerStartTime = Time.time;
                        centerFrom = new Vector3(other.transform.position.x, 0, other.transform.position.z);
                        centerTo = new Vector3(this.GetComponent<BoxCollider>().transform.TransformPoint(Vector3.zero).x, 0, this.GetComponent<BoxCollider>().transform.TransformPoint(Vector3.zero).z);
                        centerOldPlayer = other.transform.position;
                        centerJourneyLength = Vector3.Distance(centerFrom, centerTo);
                        StartCoroutine(centerCube(other.gameObject));
                        //On lance le mvt de camera pour regarder le menu (+ toutes les actions anneces
                        Camera.main.GetComponent<CameraController>().posCameraMenu = posCameraMenu;
                        Camera.main.GetComponent<CameraController>().cameraToMenuSpeed = cameraToMenuSpeed;
                        Camera.main.GetComponent<CameraController>().menuAssociatedWith = menuAssociatedWith;
                        Camera.main.GetComponent<CameraController>().launchMenu();

                        GameObject.FindWithTag("Lights").GetComponent<LightManager>().fadeOutLight(cameraToMenuSpeed / 2);
                    }
                }
            }
            if(inMenu)
            {
                //look straight
                player.transform.LookAt(playerLookAt.transform);
                CameraController cc = Camera.main.GetComponent<CameraController>();
                if(cc != null)
                    cc.cameraLookAtMenuTR = cameraLookAt.transform.position;
            }        
        }
    }

    public IEnumerator centerCube(GameObject other)
    {
        float distCovered = (Time.time - centerStartTime) * centerSpeed;
        float fracJourney = distCovered / centerJourneyLength;
        while (fracJourney < 1)
        {
            distCovered = (Time.time - centerStartTime) * centerSpeed;
            fracJourney = distCovered / centerJourneyLength;
            other.transform.position = Vector3.Lerp(new Vector3(centerFrom.x, centerOldPlayer.y, centerFrom.z), new Vector3(centerTo.x, centerOldPlayer.y, centerTo.z), fracJourney);
            yield return new WaitForEndOfFrame();
        }
        gameObject.SendMessage("launchMenu", SendMessageOptions.DontRequireReceiver);
    }

    public void exitMenu()
    {
        if(player != null)
        {
            inMenu = false;
            //On actualise le sort qu'il avait en main
            player.GetComponent<Player>().cubeFaceChanged(player.GetComponent<Player>().getCubeFace());
            //On sort du menu (camera)
            Camera.main.GetComponent<CameraController>().exitMenu();
            GameObject.FindWithTag("Lights").GetComponent<LightManager>().fadeInLight(cameraToMenuSpeed/2);
            //On remet le MecaArm en posInit
            gameObject.SendMessage("quitMenu", SendMessageOptions.DontRequireReceiver);
            //On sauve les sorts équipé et dans l'inventaire
            player.GetComponent<Player>().sauvegarderSorts();
            GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().sauvegarder();
        }
    }

    public bool isReady()
    {
        return menuReady;
    }
    public void setReady(bool p_menuReady)
    {
        menuReady = p_menuReady;
    }
}