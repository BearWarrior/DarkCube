using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class PortalBehaviour : MonoBehaviour, IInputsObservable
{
    public bool lastPortal = false; //Si c'est le dernier portal, on ne TP pas
    public int direction;
    public WorldBehaviour wolrdBehaviour;
    public bool usable; //Seul les portals de début sont à false, il faut sortir du portail pour l'activer
    public List<GameObject> pillars;
    public GameObject center;
    public Text text;

    private Color colorRed = new Color(0.7794118f, 0.4126298f, 0.4126298f);
    private Color colorGreen = new Color(0.4126298f, 0.7794118f, 0.4151593f);

    private float startTime;
    private List<Vector3> downPos;
    private List<Vector3> upPos;
    private float speedPillar;
    private float journeyLengthPillar;

    private Vector3 centerFrom;
    private Vector3 centerTo;
    private Vector3 centerOldPlayer;
    private float journeyLengthCenter;
    private float speedCenter;

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    private bool cubeCentered;
    private bool pillarUp;

    private GameObject player;

    private float spawnAngle;
    
    // Use this for initialization
    void Awake()
    {
        //keys = GameObject.FindWithTag("InputsLoader").GetComponent<InputsLoader>().getInputs();

        keys = GameObject.FindWithTag("InputsLoader").GetComponent<InputsLoader>().lookAtInputs(this.gameObject);

        upPos = new List<Vector3>();
        downPos = new List<Vector3>();
        foreach (GameObject pillar in pillars)
        {
            upPos.Add(pillar.transform.localPosition);
            pillar.transform.localPosition = new Vector3(pillar.transform.localPosition.x, -0.872f, pillar.transform.localPosition.z);
            downPos.Add(pillar.transform.localPosition);
        }
        speedPillar = .5f;
        speedCenter = .5f;
        journeyLengthPillar = upPos[0].y - downPos[0].y;

        cubeCentered = false;
        pillarUp = false;
        usable = false;
    }

    public void keysChanged(Dictionary<string, KeyCode> keys)
    {
        this.keys = keys;
    }

    void Update()
    {
        if (cubeCentered && pillarUp)
        {
            //TODO put particle teleport here
            //TODO change room only when particl finished

            wolrdBehaviour.changeRoom(direction);

            cubeCentered = false;
            pillarUp = false;
        }
    }

    public void setSpawnAngle(float angle)
    {
        spawnAngle = angle;
    }

    public void setPlayer(GameObject pl)
    {
        player = pl;
    }

    public void setNumRoom(int t)
    {
        if(t != 10000)
            text.text = "Accès salle " + t.ToString();
        else
            text.text = "Finir le niveau";
    }

    public void downPillars()
    {
        player.transform.localEulerAngles = new Vector3(0, spawnAngle, 0);
        for (int i = 0; i < pillars.Count; i++)
            pillars[i].transform.localPosition = upPos[i];
        startTime = Time.time;
        StartCoroutine(downPillarsCoroutine());
    }

    public void setUsable()
    {
        Renderer renderer = center.GetComponentInParent<Renderer>();
        Material mat = renderer.material;
        mat.SetColor("_EmissionColor", colorGreen);

        usable = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (usable)
            {
                other.GetComponent<PlayerController>().displayInteractionHelper(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().displayInteractionHelper(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!lastPortal)
            {
                if (usable)
                {
                    if (Input.GetKeyDown(keys["Interact"]))
                    {
                        startTime = Time.time;
                        StartCoroutine(upPillarsCoroutine());

                        //center
                        centerFrom = new Vector3(other.transform.position.x, 0, other.transform.position.z);
                        centerTo = new Vector3(this.GetComponent<BoxCollider>().transform.TransformPoint(Vector3.zero).x, 0, this.GetComponent<BoxCollider>().transform.TransformPoint(Vector3.zero).z);
                        centerOldPlayer = other.transform.position;
                        journeyLengthCenter = Vector3.Distance(centerFrom, centerTo);
                        StartCoroutine(centerCube(other.gameObject));
                        //Camera
                        Camera.main.GetComponent<Crosshair>().display = false;
                        other.GetComponent<PlayerController>().setControllable(false);
                    }
                }
            }
            else
            {
                if (usable)
                {
                    if (Input.GetKeyDown(keys["Interact"]))
                    {
                        GameObject.FindWithTag("Player").GetComponent<Player>().EndLvl(false);
                    }
                }
            }

        }
    }

    public IEnumerator centerCube(GameObject other)
    {
        float distCovered = (Time.time - startTime) * speedCenter;
        float fracJourney = distCovered / journeyLengthCenter;
        while (fracJourney < 1)
        {
            distCovered = (Time.time - startTime) * speedCenter;
            fracJourney = distCovered / journeyLengthCenter;
            other.transform.position = Vector3.Lerp(new Vector3(centerFrom.x, centerOldPlayer.y, centerFrom.z), new Vector3(centerTo.x, centerOldPlayer.y, centerTo.z), fracJourney);
            yield return new WaitForEndOfFrame();
        }
        cubeCentered = true;
    }

    public IEnumerator upPillarsCoroutine()
    {
        float distCovered = (Time.time - startTime) * speedPillar;
        float fracJourney = distCovered / journeyLengthPillar;
       
        while (fracJourney < 1)
        {
            distCovered = (Time.time - startTime) * speedPillar;
            fracJourney = distCovered / journeyLengthPillar;
            for (int i = 0; i < pillars.Count; i++)
            {
                pillars[i].transform.localPosition = Vector3.Lerp(downPos[i], upPos[i], fracJourney);
            }

            yield return new WaitForEndOfFrame();
        }
        pillarUp = true;
    }

    IEnumerator downPillarsCoroutine()
    {

        float distCovered = (Time.time - startTime) * speedPillar;
        float fracJourney = distCovered / journeyLengthPillar;

        while (fracJourney < 1)
        {
            distCovered = (Time.time - startTime) * speedPillar;
            fracJourney = distCovered / journeyLengthPillar;
            for (int i = 0; i < pillars.Count; i++)
            {
                pillars[i].transform.localPosition = Vector3.Lerp(upPos[i], downPos[i], fracJourney);
            }

            yield return new WaitForEndOfFrame();
        }
        Camera.main.GetComponent<Crosshair>().display = true;
        player.gameObject.GetComponent<PlayerController>().setControllable(true);
    }
}




/*


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
                //GameObject.FindWithTag("CanvasEndDungeon").transform.GetChild(0).transform.gameObject.SetActive(true);

                //other.GetComponent<PlayerController>().setControllable(false);
                //Cursor.visible = true;
               // Cursor.lockState = CursorLockMode.None;

GameObject.FindWithTag("Player").GetComponent<Player>().EndLvl(false);
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



*/
