using UnityEngine;
using System.Collections;

public class accessMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject menuActif;
    public GameObject playerLookAt;
    public GameObject lookAt;

    private GameObject player;
    private bool inMenu;

    //Center Cube
    private float centerStartTime;
    private float centerSpeed;
    private float centerJourneyLength;
    private Vector3 centerFrom;
    private Vector3 centerTo;
    private Vector3 centerOldPlayer;

    //Used en MenuManagerInGame to exit or not the menu when escape pressed
    private bool menuReady;

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
            player = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
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
                    //On centre le cube sur la platform
                    centerStartTime = Time.time;
                    centerFrom = new Vector3(other.transform.position.x, 0, other.transform.position.z);
                    centerTo = new Vector3(this.GetComponent<BoxCollider>().transform.TransformPoint(Vector3.zero).x, 0, this.GetComponent<BoxCollider>().transform.TransformPoint(Vector3.zero).z);
                    centerOldPlayer = other.transform.position;
                    centerJourneyLength = Vector3.Distance(centerFrom, centerTo);
                    StartCoroutine(centerCube(other.gameObject));
                    //On lance le mvt de camera pour regarder le menu (+ toutes les actions anneces
                    Camera.main.GetComponent<CameraController>().launchMenu();
                }
            }
            if(inMenu)
            {
                //look straight
                player.transform.LookAt(playerLookAt.transform);
                Camera.main.GetComponent<CameraController>().cameraLookAtMenuTR = lookAt.transform.position;
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
        GetComponent<MecaArm>().launchMenu();
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
            //On remet le MecaArm en posInit
            GetComponent<MecaArm>().quitMenu();
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

//Place camera
/*cameraStartTime = Time.time;
cameraFrom = Camera.main.transform.position;
cameraTo = posCamera.transform.position;
cameraJourneyLength = Vector3.Distance(cameraFrom, cameraTo);
StartCoroutine(placeCamera());*/
/*  public IEnumerator placeCamera()
  {
      float distCovered = (Time.time - cameraStartTime) * cameraSpeed;
      float fracJourney = distCovered / cameraJourneyLength;
      while (fracJourney < 1)
      {
          distCovered = (Time.time - cameraStartTime) * cameraSpeed;
          fracJourney = distCovered / cameraJourneyLength;
          Camera.main.transform.position = Vector3.Lerp(cameraFrom, cameraTo, fracJourney);
          yield return new WaitForEndOfFrame();
      }
  }*/

//ResetCamera
/*cameraStartTime = Time.time;
cameraFrom = Camera.main.transform.position;
cameraTo = GameObject.FindWithTag("CameraTarget").transform.position;
cameraJourneyLength = Vector3.Distance(cameraFrom, cameraTo);
StartCoroutine(resetCamera());*/
/* public IEnumerator resetCamera()
 {
     float distCovered = (Time.time - cameraStartTime) * cameraSpeed;
     float fracJourney = distCovered / cameraJourneyLength;
     while (fracJourney < 1)
     {
         distCovered = (Time.time - cameraStartTime) * cameraSpeed;
         fracJourney = distCovered / cameraJourneyLength;
         Camera.main.transform.position = Vector3.Lerp(cameraFrom, cameraTo, fracJourney);
         yield return new WaitForEndOfFrame();
     }
     Camera.main.GetComponent<CameraController>().playerInMenu = false;
 }*/
