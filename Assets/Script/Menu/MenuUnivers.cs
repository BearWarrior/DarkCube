using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuUnivers : MonoBehaviour, IDisplayable
{
    private int panelAct; // 0 galaxy   1 stellar system
    [Space(15)]

    public accessMenu accessMenu;
    public RotateStellarSystem rotateStellarSystem;

    public float speedCentering; //20
    public float speedScaling; //100

    private int activeStellarSystem = 0;
    private int missionChosen = 0;

    private bool isCentering;
    private bool isUnCentering;
    private Vector3 startPosCentering;
    private Vector3 endPosCentering;
    private bool isScaling;
    private bool isUnScaling;

    private float startTimeCentering;
    private float startTimeScaling;
    private float journeyLength;

    private List<float> rotationSpeedSSs;

    [Space(15)]
    public Text canvasTitle;
    public Text canvasDescr;
    public Text buttonText;
    [Space(15)]
    public List<MissionChoose> missionChooser;
    public List<GameObject> listStellarSystem;
    public List<GameObject> listPanels;

    private List<Vector3> listInitStellarSystem;

    private bool isMenuActive;

    public GameObject arrow;

    public GameObject destinationTP;
    public GameObject lookAtTP;

    public GameObject teleporter;

    private List<string> listNameStellarSystem = new List<string>
    {
        "SS-X28",
        "Andromeda SS",
        "Brendadirk Cramplescrunch"
    };
    private List<string> listDescrStellarSystem = new List<string>
    {
        "BLABLABLALBLALBLLBALBLABLABLALBLALBLLBALBLABLABLALBLALBLLBALBLABLABLALBLALBLLBALBLABLABLALBLALBLLBALBLABLABLALBLALBLLBAL",
        "LBOLBOBLOBOLBLOBLOBOLLBOLBOBLOBOLBLOBLOBOLLBOLBOBLOBOLBLOBLOBOLLBOLBOBLOBOLBLOBLOBOLLBOLBOBLOBOLBLOBLOBOLLBOLBOBLOBOLBLOBLOBOLLBOLBOBLOBOLBLOBLOBOL",
        "BLUBLBUBLBUBLUBLBUBLBUBLUBLBUBLBUBLUBLBUBLBUBLUBLBUBLBUBLUBLBUBLBUBLUBLBUBLBUBLUBLBUBLBUBLUBLBUBLBU"
    };
    private List<List<string>> listNameMissions = new List<List<string>>
    {
        new List<String>
        {
            "Avant poste X-42",
            "Flotte ennemi Yasai",
            "Avant poste G31"
        },
        new List<String>
        {
            "Avant poste X-42",
            "Flotte ennemi Yasai",
            "Avant poste G31",
            "Avant poste G31"
        },
        new List<String>
        {
            "Avant poste X-42",
            "Flotte ennemi Yasai",
            "Avant poste G31",
            "Avant poste G31"
        }
    };
    private List<List<string>> listDescrMissions = new List<List<string>>
    {
        new List<String>
        {
            "Cette avant poste controllé par l'ennemi se trouve proche de la planete YS-854-XCV. Cette planete fortement armée est apte à envoyer des renforts si une alarme se déclenche. Prenez " +
            "garde à ne pas vous faire repérer.",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consequat nisl nec porta pharetra. Etiam quis leo quis purus tempor malesuada vel et metus. " +
            "Vestibulum sed fermentum orci, et suscipit tortor. Cras tempor, tortor sit amet rutrum accumsan, dui ante ultricies eros, in porta diam dolor eleifend elit. Aenean quis justo tortor.",
            "Je n'ai plus d'idée de description donc je vais laisser un texte qui ne veut rien dire, faudra bien penser à le changer quand on aura des idée (si jamais on en a LOL)."
        },
        new List<String>
        {
            "Cette avant poste controllé par l'ennemi se trouve proche de la planete YS-854-XCV. Cette planete fortement armée est apte à envoyer des renforts si une alarme se déclenche. Prenez " +
            "garde à ne pas vous faire repérer.",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consequat nisl nec porta pharetra. Etiam quis leo quis purus tempor malesuada vel et metus. " +
            "Vestibulum sed fermentum orci, et suscipit tortor. Cras tempor, tortor sit amet rutrum accumsan, dui ante ultricies eros, in porta diam dolor eleifend elit. Aenean quis justo tortor.",
            "Je n'ai plus d'idée de description donc je vais laisser un texte qui ne veut rien dire, faudra bien penser à le changer quand on aura des idée (si jamais on en a LOL).",
            "Je n'ai plus d'idée de description donc je vais laisser un texte qui ne veut rien dire, faudra bien penser à le changer quand on aura des idée (si jamais on en a LOL)."
        },
        new List<String>
        {
            "Cette avant poste controllé par l'ennemi se trouve proche de la planete YS-854-XCV. Cette planete fortement armée est apte à envoyer des renforts si une alarme se déclenche. Prenez " +
            "garde à ne pas vous faire repérer.",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consequat nisl nec porta pharetra. Etiam quis leo quis purus tempor malesuada vel et metus. " +
            "Vestibulum sed fermentum orci, et suscipit tortor. Cras tempor, tortor sit amet rutrum accumsan, dui ante ultricies eros, in porta diam dolor eleifend elit. Aenean quis justo tortor.",
            "Je n'ai plus d'idée de description donc je vais laisser un texte qui ne veut rien dire, faudra bien penser à le changer quand on aura des idée (si jamais on en a LOL).",
            "Je n'ai plus d'idée de description donc je vais laisser un texte qui ne veut rien dire, faudra bien penser à le changer quand on aura des idée (si jamais on en a LOL)."
        }
    };


    private void updateCanvasScreen(string title, string descr, string button)
    {
        canvasTitle.text = title;
        canvasDescr.text = descr;
        buttonText.text = button;
    }

    void Start()
    {
        rotationSpeedSSs = new List<float> { 12, -8, 10 };

        listInitStellarSystem = new List<Vector3>();
        for (int i = 0; i < listStellarSystem.Count; i++)
            listInitStellarSystem.Add(listStellarSystem[i].transform.localPosition);

        panelAct = 0; //galaxy

        for (int i = 0; i < missionChooser.Count; i++)
            missionChooser[i].setMissionDetails(listNameMissions[i]);

        for (int i = 0; i < listPanels.Count; i++)
            listPanels[i].GetComponent<Image>().fillAmount = 0;
        stellarSystemClic(0);
        isMenuActive = false;
        hide();
    }

    public void stellarSystemClic(int ss)
    {
        unclicAllMission();
        activeStellarSystem = ss;
        updateCanvasScreen(listNameStellarSystem[ss], listDescrStellarSystem[ss], "Choisir");
        listPanels[ss].GetComponent<Image>().fillAmount = 1;
    }

    private void unclicAllMission()
    {
        for (int i = 0; i < listPanels.Count; i++)
            listPanels[i].GetComponent<Image>().fillAmount = 0;
    }

    //TODO From 0 to 1 slowwwwlllyyy
    public void SSOnHoverEnter(int ss)
    {
        if (ss != activeStellarSystem)
            listPanels[ss].GetComponent<Image>().fillAmount = 1;
    }

    //TODO From 1 to 0 slowwwwlllyyy
    public void SSOnHoverExit(int ss)
    {
        if (ss != activeStellarSystem)
            listPanels[ss].GetComponent<Image>().fillAmount = 0;
    }

    public void chooseSS()
    {
        listStellarSystem[activeStellarSystem].GetComponent<RescaleStellarSystem>().choseStellarSystem(); //set the SS size
        isCentering = true;
        startPosCentering = transform.position;
        endPosCentering = transform.TransformPoint(listStellarSystem[activeStellarSystem].transform.localPosition * 10) - transform.position; //(local * 10 --> global) - reference
        isScaling = true;
        startTimeScaling = Time.time;
        startTimeCentering = Time.time; //Start time of lerping 
        journeyLength = Vector3.Distance(listInitStellarSystem[activeStellarSystem], Vector3.zero); //Calcul the distance between their pos and their destination
        rotateStellarSystem.stopRotation();
        
        //change RigidBodies
        Destroy(GetComponent<Rigidbody>());
        Rigidbody rigidbody;
        for(int i = 0; i < listStellarSystem.Count; i++)
        {
            rigidbody = listStellarSystem[i].AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.angularDrag = 1.5f;
        }
    }

    public void returnToGalaxy()
    {
        listStellarSystem[activeStellarSystem].GetComponent<RescaleStellarSystem>().expandStellarSystem(); //set the SS size
        isUnCentering = true;
        isUnScaling = true;
        startTimeScaling = Time.time;
        startTimeCentering = Time.time; //Start time of lerping 
        journeyLength = Vector3.Distance(listInitStellarSystem[activeStellarSystem], Vector3.zero); //Calcul the distance between their pos and their destination

        //change RigidBodies
        Rigidbody rigidbody;
        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.angularDrag = 1.5f;
        for (int i = 0; i < listStellarSystem.Count; i++)
        {
            Destroy(listStellarSystem[i].GetComponent<Rigidbody>());
        }
    }

    public void fromGalaxyToStellarSystem()
    {
        panelAct = 1; //Stellar System
        GetComponent<SphereCollider>().enabled = false;
        for (int i = 0; i < listPanels.Count; i++)
            listPanels[i].gameObject.SetActive(false);
        missionChooser[activeStellarSystem].show();
        arrow.SetActive(false);
    }

    public void fromStellarSystemToGalaxy()
    {
        panelAct = 0; //Galaxy
        GetComponent<SphereCollider>().enabled = true;
        for (int i = 0; i < listPanels.Count; i++)
            listPanels[i].gameObject.SetActive(true);
        missionChooser[activeStellarSystem].hide();
        arrow.SetActive(true);
    }

    public void oK()
    {
        if (!isUnCentering && !isUnScaling)
        {
            if (panelAct == 0)
            {
                chooseSS();
                fromGalaxyToStellarSystem();
            }
            else if (panelAct == 1)
            {
                Destroy(Camera.main.GetComponent<CameraController>());
                CameraTeleporter cameraTp = Camera.main.gameObject.AddComponent<CameraTeleporter>();
                cameraTp.newDest(destinationTP.transform.position, lookAtTP.transform.position);
                teleporter.GetComponent<Teleporter>().startTP();
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().showCanvas(false);

                StartCoroutine(launchLvlInSec(4.5f));
            }
        }
    }

    public IEnumerator launchLvlInSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene("ProceduralRoom");
    }

    public void back()
    {
        if (!isCentering && !isScaling)
        {
            if (panelAct == 0)
            {
               /* if (accessMenu.GetComponent<accessMenu>().isReady())
                {*/
                    accessMenu.exitMenu();
               /*}*/
            }
            else if (panelAct == 1)
            {
                returnToGalaxy();
                fromStellarSystemToGalaxy();
            }
        }
    }

    void Update()
    {
        if (panelAct == 0) //rotate the SSs
        {
            for (int i = 0; i < listStellarSystem.Count; i++)
                listStellarSystem[i].transform.Rotate(new Vector3(0, rotationSpeedSSs[i], 0) * Time.deltaTime);
        }

        //Center on the galaxy
        if (isCentering)
        {
            //When are we in the Lerp
            float distCovered = (Time.time - startTimeCentering) * speedCentering;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosCentering, startPosCentering - endPosCentering, fracJourney);
            
            if (fracJourney > 1)
                isCentering = false;
        }
        if (isUnCentering)
        {
            //When are we in the Lerp
            float distCovered = (Time.time - startTimeCentering) * speedCentering;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosCentering - endPosCentering, startPosCentering, fracJourney);

            if (fracJourney > 1)
                isUnCentering = false;
        }

        //Scale on the galaxy
        if (isScaling)
        {
            //When are we in the Lerp
            float distCovered = (Time.time - startTimeScaling) * speedScaling;
            float fracJourney = distCovered / journeyLength;

            float newScale = Mathf.Lerp(.1f, 1f, fracJourney);
            transform.localScale = new Vector3(newScale, newScale, newScale);

            if (fracJourney > 1)
                isScaling = false;
        }
        if (isUnScaling)
        {
            //When are we in the Lerp
            float distCovered = (Time.time - startTimeScaling) * speedScaling;
            float fracJourney = distCovered / journeyLength;

            float newScale = Mathf.Lerp(1f, .1f, fracJourney);
            transform.localScale = new Vector3(newScale, newScale, newScale);

            if (fracJourney > 1)
                isUnScaling = false;
        }

        //Back to game
        if (Input.GetKeyDown(KeyCode.Escape))
            if (isMenuActive)
                if (accessMenu.GetComponent<accessMenu>().isReady())
                    accessMenu.GetComponent<accessMenu>().exitMenu();
    }


    //GETTERS SETTERS
    public void setMissionChosen(int p_missionChosen)
    {
        missionChosen = p_missionChosen;
        updateCanvasScreen(listNameMissions[activeStellarSystem][p_missionChosen], listDescrMissions[activeStellarSystem][p_missionChosen], "Déployer");
    }

    public void hide()
    {
        isMenuActive = false;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    public void show()
    {
        isMenuActive = true;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }
}
