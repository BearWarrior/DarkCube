using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    private const float buttonHeight = 60;
    private const float delayDoubleClick = 0.4f;

    public GameObject buttonTemplate;
    public GameObject lineServerTemplate;
    [Space(15)]
    public GameObject menuPrincipal;
    public GameObject classChooser;
    public GameObject classEditor;
    public GameObject option;
    public GameObject multiplayer;
    public GameObject popUpYesNo;
    public GameObject popUpCreateServer;
    [Space(15)]
    public GameObject editorVerticalNames;
    public GameObject editorVerticalInput;
    public GameObject editorTypeNamesProj;
    public GameObject editorTypeInputProj;
    public GameObject editorTypeNamesZone;
    public GameObject editorTypeInputZone;

    [Space(10)]
    public GameObject editorDropDownType;
    public GameObject editorDropDownElement;
    public GameObject editorInputNomSort;
    [Space(10)]
    public GameObject editorJetDropDownProjectile;
    public GameObject editorJetDropDownNbProj;
    public GameObject editorJetDropDownPattern;
    public GameObject editorJetToggleGravite;
    [Space(10)]
    public GameObject editorTextVitesse;
    public GameObject editorTextCoolDown;
    public GameObject editorTextDegats;

    [Space(10)]
    public GameObject editorZoneDropDownPattern;
    public GameObject editorZoneDropDownSort;

    [Space(15)]
    public GameObject editorButtonsFaces;
    public GameObject editorTextFacesChosen;
    [Space(30)]

    public GameObject chooserScrollRectMyClass;
    public GameObject chooserScrollRectEquiped;
    public GameObject chooserScrollViewMyClass;
    public GameObject chooserScrollViewEquiped;
    public GameObject chooserTextNom;
    public GameObject chooserTextElement;
    public GameObject chooserTextNomProj;
    public GameObject chooserTextGravite;
    public GameObject chooserTextNbProj;
    public GameObject chooserTextPattern;
    public GameObject chooserTextVitesse;
    public GameObject chooserTextCooldow;
    public GameObject chooserTextDegats;
    [Space(30)]

    public GameObject multiScrollView;
    [Space(30)]
    public GameObject optionInputResolution;
    public GameObject optionToggleFullScreen;

    [Space(30)]
    public GameObject network;

    private int selectedSort;
    private int selectedFace;

    private float distFromCam;
    private bool changingCamera;
    Quaternion posMenuPrin;
    Quaternion posOption;
    Quaternion posClassChooser;
    Quaternion posClassEditor;
    Quaternion posMultiplayer;
    Quaternion toAngle;

    private SortDeJet sortEnConstruction;

    private Color colorButtonFaceNormal;
    private Color colorButtonFaceSelected;

    private bool popUpYesNoAnswered;
    private bool popUpYesNoResult;
    private bool delegateYesNoUsed;
    public delegate void DelYesNo();
    DelYesNo fctToCallWhenPopUpYesNoAnswer;


    private bool popUpCreateServerAnswered;
    private bool popUpCreateServerResult;
    private bool delegateCreateServerUsed;
    public delegate void DelCreateServer();
    DelCreateServer fctToCallWhenPopUpCreateServerAnswer;


    private List<List<string>> listPatterns = new List<List<string>>{new List<string> { "Unique"}, 
                                                                     new List<string> { "Rafale", "Simultané ligne" }, 
                                                                     new List<string> { "Rafale", "Simultané ligne", "Simultané triangle" }, 
                                                                     new List<string> { "Rafale", "Simultané ligne", "Simultané carré" }};

    private float doubleClickStart = -1;

    private int lineClicked;



    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(1024, 768, false);
        changeRes();

        chooserScrollRectMyClass.GetComponent<ScrollRect>().scrollSensitivity = 20;
        chooserScrollRectEquiped.GetComponent<ScrollRect>().scrollSensitivity = 20;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        selectedFace = 1;
        selectedSort = -1;

        popUpCreateServerAnswered = false;
        popUpCreateServerResult = false;
        delegateCreateServerUsed = false;

        popUpYesNoAnswered = false;
        popUpYesNoResult = false;
        delegateYesNoUsed = false;

        changingCamera = false;

        //Positionnement dynamique et mise a l'échelle du menu sous forme de cube en fonction de la résolution d'écran
        posMenuPrin = Quaternion.Euler(0, 0, 0);
        posClassChooser = Quaternion.Euler(90, 0, 0);
        posClassEditor = Quaternion.Euler(-180, 0, 0);
        posOption = Quaternion.Euler(0, 90, 0);
        posMultiplayer = Quaternion.Euler(0, 270, 0);
        toAngle = posMenuPrin;




        adaptCanvasToRes();

        //On remplit le dropview des elements en prenant l'enum
        foreach (EnumScript.Element elem in EnumScript.Element.GetValues(typeof(EnumScript.Element)))
            editorDropDownElement.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
        editorDropDownElement.GetComponent<Dropdown>().captionText = editorDropDownElement.GetComponent<Dropdown>().captionText;

        //On remplit le dropview des types en prenant l'enum
        foreach (EnumScript.TypeSort type in EnumScript.TypeSort.GetValues(typeof(EnumScript.TypeSort)))
            editorDropDownType.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(type.ToString()));
        editorDropDownType.GetComponent<Dropdown>().captionText = editorDropDownType.GetComponent<Dropdown>().captionText;
        editorTypeChanged();

        //On remplit le dropview des types en prenant l'enum
        foreach (EnumScript.GabaritSortDeZone pattern in EnumScript.GabaritSortDeZone.GetValues(typeof(EnumScript.GabaritSortDeZone)))
            editorZoneDropDownPattern.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(pattern.ToString()));
        editorZoneDropDownPattern.GetComponent<Dropdown>().captionText = editorZoneDropDownPattern.GetComponent<Dropdown>().captionText;

        //OPTIONS 
        List<string> listResolution = new List<string> { "800x600", "1024x768", "1600x900", "1920x1080" };
        optionInputResolution.GetComponent<Dropdown>().options.Clear();
        foreach (string reso in listResolution)
            optionInputResolution.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(reso));
        optionInputResolution.GetComponent<Dropdown>().captionText = optionInputResolution.GetComponent<Dropdown>().captionText;

        majPattern();
    }

    // Update is called once per frame
    void Update()
    {
        cameraGoTo(toAngle);

    }

    public void launchSolo()
    {
        Application.LoadLevel("Game");
    }

    public void quitApplication()
    {
        Application.Quit();
    }

    public void changeCamera(int face)
    {
        changingCamera = true;
        switch (face)
        {
            case 1:
                toAngle = posMenuPrin;
                break;
            case 2:
                toAngle = posOption;
                break;
            case 3:
                toAngle = posClassChooser;
                break;
            case 4:
                toAngle = posClassEditor;
                break;
            //case 5:
            //    roomsList = network.GetComponent<NetworkControllerOLD>().getRooms();
            //    Debug.Log(roomsList.Length);
            //    if(roomsList.Length>0)
            //        Debug.Log("name : " + roomsList[0].name + "   nbPlayer : " + roomsList[0].playerCount + "/" + roomsList[0].maxPlayers);
            //    toAngle = posMultiplayer;
            //    break;
        }
    }

    public void cameraGoTo(Quaternion f)
    {
        if (changingCamera)
        {
            GameObject.FindWithTag("Menu").transform.localRotation = Quaternion.Lerp(GameObject.FindWithTag("Menu").transform.localRotation, f, Time.deltaTime * 8);

            if (Quaternion.Angle(GameObject.FindWithTag("Menu").transform.localRotation, f) < 0.1)
            {
                GameObject.FindWithTag("Menu").transform.localRotation = f;
                changingCamera = false;
            }
        }
    }

    public void setSelectedFace(int face)
    {
        //Changement titre fenetre
        editorTextFacesChosen.GetComponent<Text>().text = "Personnalitsation de la face n° " + face;

        //Mise a jour caractéristiques affichés
        majCaracSortClicked();

        //On met le bouton de l'ancienne face en blanc et le nouveau en cyan
        editorButtonsFaces.transform.GetChild(selectedFace - 1).GetComponent<Button>().image.color = Color.white;
        selectedFace = face;
        editorButtonsFaces.transform.GetChild(selectedFace - 1).GetComponent<Button>().image.color = Color.cyan;

        //On vide les scrollView et on le reremplit avec les bons sort
        resetClassChooser();
        fillClassChooserTable();
    }


    public void editorElementChanged()
    {
        //On récupère la liste des projectiles de cet element puis affichage dans la dropview
        List<structSortJet> listSort = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorDropDownElement.GetComponent<Dropdown>().value);
        editorJetDropDownProjectile.GetComponent<Dropdown>().options.Clear();
        foreach (structSortJet proj in listSort)
            editorJetDropDownProjectile.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(proj.nomProj));
        editorJetDropDownProjectile.GetComponent<Dropdown>().captionText = editorJetDropDownProjectile.GetComponent<Dropdown>().captionText;

        //changement de l'élément et du projectile
        sortEnConstruction.setElement((EnumScript.Element)editorDropDownElement.GetComponent<Dropdown>().value);
        string nomProj = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorDropDownElement.GetComponent<Dropdown>().value)[editorJetDropDownProjectile.GetComponent<Dropdown>().value].nomProj;
        sortEnConstruction.nomProj = nomProj;

        majCaracSortEnConstr();
    }

    public void editorProjectileChanged()
    {
        //On récupere le nom du prjectile et on l'affecte au sort
        string nomProj = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorDropDownElement.GetComponent<Dropdown>().value)[editorJetDropDownProjectile.GetComponent<Dropdown>().value].nomProj;
        sortEnConstruction.nomProj = nomProj;

        //Mise a jour des carac + affichage
        majCaracSortEnConstr();
    }

    public void nouvelleClasse()
    {
        //Récupération de tous les projectiles de l'élément choisi + affichage dans dropview
        List<structSortJet> listSort = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorDropDownElement.GetComponent<Dropdown>().value);
        editorJetDropDownProjectile.GetComponent<Dropdown>().options.Clear();
        foreach (structSortJet proaj in listSort)
            editorJetDropDownProjectile.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(proaj.nomProj));
        editorJetDropDownProjectile.GetComponent<Dropdown>().captionText = editorJetDropDownProjectile.GetComponent<Dropdown>().captionText;

        //Récupération des variables du sort pour création
        int nbProj = editorJetDropDownNbProj.GetComponent<Dropdown>().value + 1;
        bool grav = editorJetToggleGravite.GetComponent<Toggle>().isOn;
        EnumScript.Element elem = (EnumScript.Element)editorJetDropDownProjectile.GetComponent<Dropdown>().value;
        int proj = editorJetDropDownProjectile.GetComponent<Dropdown>().value;
        structSortJet structureDuSort = GetComponent<CaracProjectiles>().getStructFromNameAndElement(GetComponent<CaracProjectiles>().getProjsFromElement(elem)[editorJetDropDownProjectile.GetComponent<Dropdown>().value].nomProj, (EnumScript.Element)editorDropDownElement.GetComponent<Dropdown>().value);
        float vitesse = structureDuSort.vitesse;
        float cooldown = structureDuSort.cooldown;
        float degats = structureDuSort.degats;
        string nomProj = GetComponent<CaracProjectiles>().getProjsFromElement(elem)[editorJetDropDownProjectile.GetComponent<Dropdown>().value].nomProj;
        string nomSort = editorInputNomSort.GetComponent<InputField>().text;

        sortEnConstruction = new SortDeJet(nbProj, vitesse, grav, cooldown, degats, elem, nomProj, nomSort, EnumScript.PatternSortDeJet.Rafale);

        //Mise a jour des carac + affichage
        majCaracSortEnConstr();
    }

    public void supprimerClassDel()
    {
        supprimerClasse();
        delegateYesNoUsed = false;
    }

    public void supprimerClasse()
    {
        if (selectedSort != -1)
        {
            if (popUpYesNoAnswered)
            {
                if (popUpYesNoResult)
                {
                    GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().supprimerAttaqueInventaireAt(selectedSort, true);
                }

                popUpYesNo.GetComponent<Canvas>().enabled = false;
                classChooser.GetComponent<CanvasGroup>().blocksRaycasts = true;
                classChooser.GetComponent<CanvasGroup>().alpha = 1f;

                popUpYesNoAnswered = false;
                delegateYesNoUsed = true;
            }
            else
            {
                //Si le sort a supprimer est équipé sur une des faces
                if (!GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().supprimerAttaqueInventaireAt(selectedSort, false))
                {
                    popUpYesNo.GetComponent<Canvas>().enabled = true;
                    popUpYesNo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "L'attaque que vous voulez supprimer est affectée à une face, voulez vous vraiment supprimer cette attaque ?";
                    popUpYesNo.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "Supprimer";
                    popUpYesNo.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = "Annuler";
                    classChooser.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    classChooser.GetComponent<CanvasGroup>().alpha = 0.3f;

                    fctToCallWhenPopUpYesNoAnswer = supprimerClassDel;
                }
            }
        }

        resetClassChooser();
        fillClassChooserTable();
    }

    public void createServerDel()
    {
        createServer();
        delegateCreateServerUsed = false;
    }

    public void createServer()
    {
        if (popUpCreateServerAnswered)
        {
            if (popUpCreateServerResult) //Si le server doit etre créé
            {
                Debug.Log(popUpCreateServer.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<InputField>().text);

                //network.GetComponent<NetworkControllerOLD>().setNameServer(popUpCreateServer.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<InputField>().text);
                //network.GetComponent<NetworkControllerOLD>().fctToCallAtBegin = network.GetComponent<NetworkControllerOLD>().createRandomRoom;

                launchMultiplayer();

                popUpCreateServer.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<InputField>().text = "Nom du serveur";
            }

            popUpCreateServer.GetComponent<Canvas>().enabled = false;
            multiplayer.GetComponent<CanvasGroup>().blocksRaycasts = true;
            multiplayer.GetComponent<CanvasGroup>().alpha = 1f;

            popUpCreateServerAnswered = false;
            delegateCreateServerUsed = true;
        }
        else
        {
            popUpCreateServer.GetComponent<Canvas>().enabled = true;
            multiplayer.GetComponent<CanvasGroup>().blocksRaycasts = false;
            multiplayer.GetComponent<CanvasGroup>().alpha = 0.3f;

            fctToCallWhenPopUpCreateServerAnswer = createServerDel;
        }
        
    }

    public void sauvegarderSort()
    {
        Debug.Log("avant");
        //On récupère le nb de projectile, on ajuste les dégats et on récupère le nom avant de sauver le sort
        sortEnConstruction.patternEnvoi = (EnumScript.PatternSortDeJet)editorJetDropDownPattern.GetComponent<Dropdown>().value;
        EnumScript.Element elem = (EnumScript.Element)editorDropDownElement.GetComponent<Dropdown>().value;

        structSortJet structureDuSort = GetComponent<CaracProjectiles>().getStructFromNameAndElement(GetComponent<CaracProjectiles>().getProjsFromElement(elem)[editorJetDropDownProjectile.GetComponent<Dropdown>().value].nomProj, (EnumScript.Element)editorDropDownElement.GetComponent<Dropdown>().value);
        sortEnConstruction.stuck = structureDuSort.stuck;
        sortEnConstruction.gravite = editorJetToggleGravite.GetComponent<Toggle>().isOn;
        switch (listPatterns[editorJetDropDownNbProj.GetComponent<Dropdown>().value][editorJetDropDownPattern.GetComponent<Dropdown>().value])
        {
            case "Unique":
                sortEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.Unique;
                break;
            case "Rafale":
                sortEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.Rafale;
                break;
            case "Simultané ligne":
                sortEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.SimultaneLigne;
                break;
            case "Simultané triangle":
                sortEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.SimultaneTriangle;
                break;
            case "Simultané carré":
                sortEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.SimultaneCarre;
                break;
        }

        if (editorInputNomSort.GetComponent<InputField>().text != "")
            sortEnConstruction.setNomSort(editorInputNomSort.GetComponent<InputField>().text);
        else
            sortEnConstruction.setNomSort("_defaut_");
        sortEnConstruction.nbProjectile = editorJetDropDownNbProj.GetComponent<Dropdown>().value + 1;
        sortEnConstruction.setDegats(sortEnConstruction.getDegats());
        GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().addAttaqueToInventaire(sortEnConstruction);

        //On enleve les boutons
        for (int nbButton = 0; nbButton < chooserScrollViewMyClass.transform.childCount; nbButton++)
        {
            Destroy(chooserScrollViewMyClass.transform.GetChild(nbButton).gameObject);
        }

        GameObject button;
        //On les remet avec le tableau de sort du joueur mis a jour
        for (int nbButton = 0; nbButton < GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire().Count; nbButton++)
        {
            button = Instantiate(buttonTemplate) as GameObject;
            button.transform.SetParent(chooserScrollViewMyClass.transform);
            button.transform.localPosition = new Vector3(0, -1 * nbButton * button.GetComponent<RectTransform>().rect.height, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire()[nbButton].getNomSort();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //Ne pas enlever : permet au bouton de marcher
            int nbButtonTemp = nbButton;
            button.GetComponent<Button>().onClick.AddListener(() => buttonClicked(nbButtonTemp));
        }
        float hauteurFinal = 10 + 10 + GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire().Count * buttonHeight + 10 * (GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire().Count - 1);
        chooserScrollViewMyClass.GetComponent<RectTransform>().sizeDelta = new Vector2(chooserScrollViewMyClass.GetComponent<RectTransform>().sizeDelta.x, hauteurFinal);

        resetClassEditorWindow();

        sortEnConstruction = null;
    }



    public void buttonClicked(int position)
    {
        int oldPos = selectedSort;
        
        if (doubleClickStart > 0 && (Time.time - doubleClickStart) < delayDoubleClick) //double click
        {
            if (position == oldPos) //Si le clic se fait au même endroit que le précédent
                EquiperDesequiperSort();

            doubleClickStart = -1;
        }
        else // premier click ou click trop tard, on considere ce clic comme le nouveau premier click
        {
            doubleClickStart = Time.time;
        }

        selectedSort = position;
        majCaracSortClicked();

    }

    public void EquiperDesequiperSort()
    {
        if (selectedSort != -1 && selectedSort != -2)
        {
            GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().equipeAttaqueAt(selectedFace, selectedSort);
            if (chooserScrollViewEquiped.transform.childCount != 0)
                Destroy(chooserScrollViewEquiped.transform.GetChild(0).gameObject);
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.transform.SetParent(chooserScrollViewEquiped.transform);
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire()[selectedSort].getNomSort();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //-1 -> sort équipé
            button.GetComponent<Button>().onClick.AddListener(() => buttonClicked(-1));
        }
        else
        {
            if (selectedSort == -1)
            {
                GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().equipeAttaqueAt(selectedFace, -1);
                Destroy(chooserScrollViewEquiped.transform.GetChild(0).gameObject);
            }
        }
    }


    public void resetClassEditorWindow()
    {
        editorDropDownElement.GetComponent<Dropdown>().value = 0;
        editorJetDropDownProjectile.GetComponent<Dropdown>().value = 0;
        editorInputNomSort.GetComponent<InputField>().text = "";
        editorJetDropDownNbProj.GetComponent<Dropdown>().value = 0;
    }

    public void resetClassChooser()
    {
        for (int nbButton = 0; nbButton < chooserScrollViewMyClass.transform.childCount; nbButton++)
            Destroy(chooserScrollViewMyClass.transform.GetChild(nbButton).gameObject);
        for (int nbButton = 0; nbButton < chooserScrollViewEquiped.transform.childCount; nbButton++)
            Destroy(chooserScrollViewEquiped.transform.GetChild(nbButton).gameObject);
    }

    public void majCaracSortEnConstr()
    {
        //On récupère la structure du sort depuis l'element et le nom
        structSortJet structureDuSort = GetComponent<CaracProjectiles>().getStructFromNameAndElement(sortEnConstruction.nomProj, sortEnConstruction.getElement());

        //On modifie le sort en conséquent
        sortEnConstruction.nbProjectile = editorJetDropDownNbProj.GetComponent<Dropdown>().value + 1;
        sortEnConstruction.vitesseProj = structureDuSort.vitesse;
        sortEnConstruction.setCooldown(structureDuSort.cooldown);
        sortEnConstruction.setDegats(structureDuSort.degats / sortEnConstruction.nbProjectile);

        //On affiche les nouvelles caractéristiques
        editorTextVitesse.GetComponent<Text>().text = sortEnConstruction.vitesseProj.ToString();
        editorTextCoolDown.GetComponent<Text>().text = sortEnConstruction.getCooldown().ToString();
        editorTextDegats.GetComponent<Text>().text = sortEnConstruction.getDegats().ToString();
    }

    public void majCaracSortClicked()
    {
        //On récupère le sort séléctionné
        SortDeJet sort;
        if (selectedSort != -1)
            sort = (SortDeJet)GameObject.FindWithTag("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire()[selectedSort];
        else if (selectedSort == 1)
            sort = (SortDeJet)GameObject.FindWithTag("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getAttaqueEquipOnFace(selectedFace);
        else
            sort = null;

        if (sort != null)
        {
            //Et on affiche ses caractéristiques
            chooserTextNom.GetComponent<Text>().text = sort.getNomSort();
            chooserTextElement.GetComponent<Text>().text = sort.getElement().ToString();
            chooserTextNomProj.GetComponent<Text>().text = sort.nomProj;
            if (sort.gravite)
                chooserTextGravite.GetComponent<Text>().text = "oui";
            else
                chooserTextGravite.GetComponent<Text>().text = "non";
            chooserTextNbProj.GetComponent<Text>().text = sort.nbProjectile.ToString();
            chooserTextPattern.GetComponent<Text>().text = sort.patternEnvoi.ToString();
            chooserTextVitesse.GetComponent<Text>().text = sort.vitesseProj.ToString();
            chooserTextCooldow.GetComponent<Text>().text = sort.getCooldown().ToString();
            chooserTextDegats.GetComponent<Text>().text = sort.getDegats().ToString();
        }
    }

    public void fillClassChooserTable()
    {
        GameObject button;

        for (int nbButton = 0; nbButton < GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire().Count; nbButton++)
        {
            button = Instantiate(buttonTemplate) as GameObject;
            button.transform.SetParent(chooserScrollViewMyClass.transform);
            button.transform.localPosition = new Vector3(0, -1 * nbButton * button.GetComponent<RectTransform>().rect.height, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire()[nbButton].getNomSort();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //Ne pas enlever : permet au bouton de marcher
            int nbButtonTemp = nbButton;
            button.GetComponent<Button>().onClick.AddListener(() => buttonClicked(nbButtonTemp));
        }
        //On adapte la taille du content pour que les boutons prennent la bonne taille
        float hauteurFinal = 10 + 10 + GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire().Count * buttonHeight + 10 * (GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getListAttaqueInventaire().Count - 1);
        chooserScrollViewMyClass.GetComponent<RectTransform>().sizeDelta = new Vector2(chooserScrollViewMyClass.GetComponent<RectTransform>().sizeDelta.x, hauteurFinal);

        //Ajout du bouton du sort équipé
        if (GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getAttaqueEquipOnFace(selectedFace) != null)
        {
            button = Instantiate(buttonTemplate) as GameObject;
            button.transform.SetParent(chooserScrollViewEquiped.transform);
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("PlayerCaracteristics").GetComponent<PlayerCaracteristics>().getAttaqueEquipOnFace(selectedFace).getNomSort();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //-1 -> sort équipé
            button.GetComponent<Button>().onClick.AddListener(() => buttonClicked(-1));
        }
        //On adapte la taille du content pour que les boutons prennent la bonne taille
        float hauteurFinale = 10 + 10 + buttonHeight;
        chooserScrollViewEquiped.GetComponent<RectTransform>().sizeDelta = new Vector2(chooserScrollViewEquiped.GetComponent<RectTransform>().sizeDelta.x, hauteurFinale);
    }

    public void majPattern()
    {
        editorJetDropDownPattern.GetComponent<Dropdown>().enabled = true;
        editorJetDropDownPattern.GetComponent<Dropdown>().options.Clear();
        foreach (string pat in listPatterns[editorJetDropDownNbProj.GetComponent<Dropdown>().value])
            editorJetDropDownPattern.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(pat));

        editorJetDropDownPattern.GetComponent<Dropdown>().captionText = editorJetDropDownPattern.GetComponent<Dropdown>().captionText;
    }


    public void editorTypeChanged()
    {
        

        switch ((EnumScript.TypeSort) editorDropDownType.GetComponent<Dropdown>().value)
        {
            case EnumScript.TypeSort.Projectile :
                editorTypeInputProj.SetActive(true);
                editorTypeNamesProj.SetActive(true);

                editorTypeInputZone.SetActive(false);
                editorTypeNamesZone.SetActive(false);

                editorVerticalInput.GetComponent<RectTransform>().sizeDelta = new Vector2(editorVerticalInput.GetComponent<RectTransform>().rect.width, 10 * 60);
                editorVerticalNames.GetComponent<RectTransform>().sizeDelta = new Vector2(editorVerticalNames.GetComponent<RectTransform>().rect.width, 10 * 60);
                editorVerticalInput.GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 2, -((editorVerticalInput.GetComponent<RectTransform>().rect.height / 2) + 70));
                editorVerticalNames.GetComponent<RectTransform>().anchoredPosition = new Vector2(editorVerticalNames.GetComponent<RectTransform>().anchoredPosition.x, -((editorVerticalNames.GetComponent<RectTransform>().rect.height / 2) + 70));
                break;
            case EnumScript.TypeSort.Zone:
                editorTypeInputZone.SetActive(true);
                editorTypeNamesZone.SetActive(true);

                editorTypeInputProj.SetActive(false);
                editorTypeNamesProj.SetActive(false);          
      
                editorVerticalInput.GetComponent<RectTransform>().sizeDelta = new Vector2(editorVerticalInput.GetComponent<RectTransform>().rect.width, 7 * 60);
                editorVerticalNames.GetComponent<RectTransform>().sizeDelta = new Vector2(editorVerticalNames.GetComponent<RectTransform>().rect.width, 7 * 60);
                editorVerticalInput.GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 2, -((editorVerticalInput.GetComponent<RectTransform>().rect.height / 2) + 70));
                editorVerticalNames.GetComponent<RectTransform>().anchoredPosition = new Vector2(editorVerticalNames.GetComponent<RectTransform>().anchoredPosition.x, -((editorVerticalNames.GetComponent<RectTransform>().rect.height / 2) + 70));
                break;
        }
    }

    //public void joinServer()
    //{
    //    network.GetComponent<NetworkControllerOLD>().setNameServer(roomsList[lineClicked -1].name);
    //    network.GetComponent<NetworkControllerOLD>().fctToCallAtBegin = network.GetComponent<NetworkControllerOLD>().joinRoom;
    //    launchMultiplayer();
    //}

    public void launchMultiplayer()
    {
        Debug.Log(lineClicked);
        //GameObject.FindWithTag("NetworkManager").GetComponent<NOMDUSCRIPT>().NOMDELAFCT;
        Application.LoadLevel("Multi");
    }

    public void setPopUpYesNoResult(bool p_result)
    {
        popUpYesNoResult = p_result;
        popUpYesNoAnswered = true;
        fctToCallWhenPopUpYesNoAnswer();
    }

    public void setPopUpCreateServerResult(bool p_result)
    {
        popUpCreateServerResult = p_result;
        popUpCreateServerAnswered = true;
        fctToCallWhenPopUpCreateServerAnswer();
    }

    public void adaptCanvasToRes()
    {
        distFromCam = (Screen.height / 2) / Mathf.Tan((Camera.main.fieldOfView / 2) * Mathf.Deg2Rad);
        distFromCam *= 1.1f;
        if (distFromCam > Camera.main.farClipPlane)
            Camera.main.farClipPlane = distFromCam + 100;

        menuPrincipal.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        menuPrincipal.transform.position = new Vector3(0, 0, distFromCam);
        menuPrincipal.transform.rotation = Quaternion.Euler(0, 0, 0);

        classChooser.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        classChooser.transform.position = new Vector3(0, distFromCam, 0);
        classChooser.transform.rotation = Quaternion.Euler(270, 0, 0);

        classEditor.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        classEditor.transform.position = new Vector3(0, 0, -distFromCam);
        classEditor.transform.rotation = Quaternion.Euler(180, 0, 0);

        option.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        option.transform.position = new Vector3(-distFromCam, 0, 0);
        option.transform.rotation = Quaternion.Euler(0, 270, 0);

        multiplayer.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        multiplayer.transform.position = new Vector3(distFromCam, 0, 0);
        multiplayer.transform.rotation = Quaternion.Euler(0, 90, 0);

        popUpYesNo.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 3, Screen.height / 4);
        popUpYesNo.transform.position = new Vector3(0, 0, distFromCam - 100);
        popUpYesNo.transform.rotation = Quaternion.Euler(0, 0, 0);
        popUpYesNo.GetComponent<Canvas>().enabled = false;

        popUpCreateServer.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2, Screen.height / 3);
        popUpCreateServer.transform.position = new Vector3(0, 0, distFromCam - 100);
        popUpCreateServer.transform.rotation = Quaternion.Euler(0, 0, 0);
        popUpCreateServer.GetComponent<Canvas>().enabled = false;
    }



    //public void refreshServers()
    //{

    //    /*
    //     *  private List<List<string>> listServers = new List<List<string>>{new List<string> { "Server ou on s'enjaille", "3/4", "Oui", "120"}, 
    //                                                               new List<string> { "ServerTest", "2/4", "Non", "999" }, 
    //                                                               new List<string> { "Server Français", "1/4", "Oui", "12" }, 
    //                                                               new List<string> { "GéPluDidé", "0/4", "Oui", "1" }};
    //     * */

    //    List<List<string>> listServersREAL = new List<List<string>>();
        
    //    for (int i = 0; i < roomsList.Length; i++)
    //    {
    //        List<string> server = new List<string>();
    //        //  Debug.Log("name : " + roomsList[0].name + "   nbPlayer : " + roomsList[0].playerCount + "/" + roomsList[0].maxPlayers);
    //        server.Add(roomsList[i].name);
    //        server.Add(roomsList[i].playerCount + "/" + roomsList[i].maxPlayers);
    //        server.Add("NON");
    //        server.Add("---");
    //        listServersREAL.Add(server);
    //    }


    //    for(int i = 1; i < multiScrollView.transform.childCount; i++)
    //        Destroy(multiScrollView.transform.GetChild(i).gameObject);

    //    for (int server = 0; server < listServersREAL.Count; server++)
    //    {
    //        GameObject lineGO = GameObject.Instantiate(lineServerTemplate, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
    //        lineGO.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (server + 1).ToString();
    //        for (int i = 0; i < 4; i++)
    //            lineGO.transform.GetChild(i + 1).GetChild(0).GetComponent<Text>().text = listServersREAL[server][i];
    //        lineGO.transform.SetParent(multiScrollView.transform);
    //        lineGO.transform.localRotation = new Quaternion(0, 0, 0, 0);
    //        lineGO.transform.localPosition = new Vector3( 0, 0, 0);
    //        lineGO.name = UnityEngine.Random.value.ToString();
    //        lineGO.GetComponent<LineServer>().setLineNumber(server + 1);
    //    }
    //    multiScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 40 * (listServersREAL.Count +1));
    //}

    public void highlightLine(int line)
    {
        if (line != lineClicked)
        {
            for (int i = 0; i < 4; i++)
            {
                ColorBlock cb = multiScrollView.transform.GetChild(line).GetChild(i + 1).GetComponent<Button>().colors;
                cb.normalColor = new Color(.81f, .97f, .99f, 1);    //bleu clair
                multiScrollView.transform.GetChild(line).GetChild(i + 1).GetComponent<Button>().colors = cb;
            }
        }
    }

    public void deselectgLines()
    {
        for (int line = 1; line < multiScrollView.transform.childCount; line++)
        {
            if (line != lineClicked)
            {
                for (int col = 0; col < 4; col++)
                {
                    ColorBlock cb = multiScrollView.transform.GetChild(line).GetChild(col + 1).GetComponent<Button>().colors;
                    cb.normalColor = new Color(1, 1, 1, 1);
                    multiScrollView.transform.GetChild(line).GetChild(col + 1).GetComponent<Button>().colors = cb;
                }
            }
            else
            {
                for (int col = 0; col < 4; col++)
                {
                    ColorBlock cb = multiScrollView.transform.GetChild(line).GetChild(col + 1).GetComponent<Button>().colors;
                    cb.normalColor = new Color(.55f, .89f, .92f, 1);    //bleu plus foncé
                    multiScrollView.transform.GetChild(line).GetChild(col + 1).GetComponent<Button>().colors = cb;
                }
            }

        }
    }

    public void optionsValidation()
    {
        //List<string> listResolution = new List<string>{"800x600", "1024x768", "1600x900", "1920x1080"};
        bool fullScreen = optionToggleFullScreen.GetComponent<Toggle>().isOn;
        switch (optionInputResolution.GetComponent<Dropdown>().value)
        {
            case 0:
                Screen.SetResolution(800, 600, fullScreen);
                break;
            case 1:
                Screen.SetResolution(1024, 768, fullScreen);
                break;
            case 2:
                Screen.SetResolution(1600, 900, fullScreen);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, fullScreen);
                break;
        }

        StartCoroutine(changeRes());

        /*GameObject.FindWithTag("Menu").transform.localRotation = posMenuPrin;
        adaptCanvasToRes();
        GameObject.FindWithTag("Menu").transform.localRotation = posOption;*/
    }

    public IEnumerator changeRes()
    {
        yield return new WaitForEndOfFrame();
        GameObject.FindWithTag("Menu").transform.localRotation = posMenuPrin;
        yield return new WaitForEndOfFrame();
        adaptCanvasToRes();
        yield return new WaitForEndOfFrame();
        GameObject.FindWithTag("Menu").transform.localRotation = posOption;
        yield return new WaitForEndOfFrame();
    }

    public void clicLine(int line)
    {
        lineClicked = line;
        deselectgLines();
    }

    public void sortServerBy(int sortBy)
    {
        Debug.Log(sortBy);
        //0 -> N°
        //1 -> nom
        //2 -> NbJoueur
        //3 -> Mdp
        //4 -> Ping
        switch (sortBy)
        {
            case 0 :
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
 
    }
}




//OPTIONS  EN ATTENDE DE REPARATION DE LA PART DE UNITY

/*public void optionsValidation()
{
    //List<string> listResolution = new List<string>{"800x600", "1024x768", "1600x900", "1920x1080"};
    bool fullScreen = optionToggleFullScreen.GetComponent<Toggle>().isOn;
    switch (optionInputResolution.GetComponent<Dropdown>().value)
    {
        case 0:
            Screen.SetResolution(800, 600, false);
            break;
        case 1:
            Screen.SetResolution(1024, 768, false);
            break;
        case 2:
            Screen.SetResolution(1600, 900, false);
            break;
        case 3:
            Screen.SetResolution(1920, 1080, false);
            break;
    }

    changeCanvasResolution();
}*/


/*distFromCam = (Screen.height / 2) / Mathf.Tan((Camera.main.fieldOfView / 2) * Mathf.Deg2Rad) + 250;
if (distFromCam > Camera.main.farClipPlane)
    Camera.main.farClipPlane = distFromCam + 100;*/