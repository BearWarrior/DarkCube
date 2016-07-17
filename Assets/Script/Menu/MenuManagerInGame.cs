using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuManagerInGame : MonoBehaviour
{
    public GameObject accessMenu;
    [Space(15)]

    public Sprite buttonNotPressed;
    public Sprite buttonPressed;

    private const float buttonHeight = 40;
    private const float delayDoubleClick = 0.4f;

    public GameObject buttonTemplate;
    [Space(15)]
    public GameObject classChooser;
    public GameObject classEditor;
    public GameObject popUpYesNo;

    public GameObject menuActif;
    public GameObject menuTop;
    public GameObject menuBottom;

    [Space(15)]
    public GameObject editorButtonsFaces;
    public GameObject editorTextFacesChosen;
    [Space(40)]
    public GameObject editorButtonProj;
    public GameObject editorButtonZone;
    public GameObject editorButtonCaC;
    public GameObject editorButtonSupport;
    public GameObject editorPanelProj;
    public GameObject editorPanelZone;
    public GameObject editorPanelCaC;
    public GameObject editorPanelSupport;
    [Space(15)]
    public GameObject editorProjNomSort;
    public GameObject editorProjElement;
    public GameObject editorProjProjectile;
    public GameObject editorProjGravite;
    public GameObject editorProjNbProj;
    public GameObject editorProjPattern;
    public GameObject editorProjVitesse;
    public GameObject editorProjCoolDown;
    public GameObject editorProjDegats;
    [Space(15)]
    public GameObject editorZoneNomSort;
    public GameObject editorZoneElement;
    public GameObject editorZoneGabarit;
    public GameObject editorZoneSort;
    public GameObject editorZoneTaille;
    public GameObject editorZoneCoolDown;
    public GameObject editorZoneDegats;

    [Space(40)]
    public GameObject chooserPanelProj;
    public GameObject chooserPanelZone;
    [Space(15)]
    public GameObject chooserProjNomSort;
    public GameObject chooserProjElement;
    public GameObject chooserProjProjectile;
    public GameObject chooserProjGravite;
    public GameObject chooserProjNbProj;
    public GameObject chooserProjPattern;
    public GameObject chooserProjVitesse;
    public GameObject chooserProjCoolDown;
    public GameObject chooserProjDegats;
    [Space(15)]
    public GameObject chooserZoneNomSort;
    public GameObject chooserZoneElement;
    public GameObject chooserZoneGabarit;
    public GameObject chooserZoneSort;
    public GameObject chooserZoneCoolDown;
    public GameObject chooserZoneDegats;
    [Space(15)]

    public GameObject chooserButtonNouveauSort;
    public GameObject chooserScrollListSortInv;
    public GameObject chooserScrollListSortEq;

    private int selectedSort; // >= 0 inventaire    <0 équipéss
    private int selectedFace;
    private int selectedType;

    private SortDeJet sortDeJetEnConstruction;
    private SortDeZone sortDeZoneEnConstruction;

    private Color colorButtonFaceNormal;
    private Color colorButtonFaceSelected;

    private bool popUpYesNoAnswered;
    private bool popUpYesNoResult;
    private bool delegateYesNoUsed;
    public delegate void DelYesNo();
    DelYesNo fctToCallWhenPopUpYesNoAnswer;



    private List<List<string>> listPatternsSortJet = new List<List<string>>{new List<string> { "Unique"}, //1proj
                                                                     new List<string> { "Rafale", "Simultané ligne" }, //2proj
                                                                     new List<string> { "Rafale", "Simultané ligne", "Simultané triangle" }, //3proj
                                                                     new List<string> { "Rafale", "Simultané ligne", "Simultané carré" }}; //4proj

    private float doubleClickStart = -1;

    int canvasChooserEditor; //1 = Chooser   2 = Editor
    bool lerpingCanvas;

    // Use this for initialization
    void Start()
    {
        canvasChooserEditor = 1; // Chooser
        lerpingCanvas = false;

        sortDeJetEnConstruction = new SortDeJet();
        sortDeZoneEnConstruction = new SortDeZone();

        selectedType = 1;
        editorButtonProj.GetComponent<Image>().sprite = buttonPressed;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        selectedFace = 1;
        selectedSort = 0;

        popUpYesNoAnswered = false;
        popUpYesNoResult = false;
        delegateYesNoUsed = false;

        //On remplit les dropview des elements en prenant l'enum
        foreach (EnumScript.Element elem in EnumScript.Element.GetValues(typeof(EnumScript.Element)))
        {
            editorProjElement.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
            editorZoneElement.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
        }
        editorProjElement.GetComponent<Dropdown>().captionText = editorProjElement.GetComponent<Dropdown>().captionText;
        editorZoneElement.GetComponent<Dropdown>().captionText = editorZoneElement.GetComponent<Dropdown>().captionText;
        //On récupère la liste des projectiles de cet element puis affichage dans la dropview
        List<structSortJet> listSortJet = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorProjElement.GetComponent<Dropdown>().value);
        editorProjProjectile.GetComponent<Dropdown>().options.Clear();
        foreach (structSortJet proj in listSortJet)
            editorProjProjectile.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(proj.nomProj));
        editorProjProjectile.GetComponent<Dropdown>().value = 0;
        editorProjProjectile.GetComponent<Dropdown>().captionText = editorProjProjectile.GetComponent<Dropdown>().captionText;
        editorElementChanged();

        //On remplit le dropview de Gabarit des sorts de zone et le dropview des taille des sort de zone
        foreach (EnumScript.GabaritSortDeZone elem in EnumScript.GabaritSortDeZone.GetValues(typeof(EnumScript.GabaritSortDeZone)))
            editorZoneGabarit.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
        editorZoneGabarit.GetComponent<Dropdown>().captionText = editorZoneGabarit.GetComponent<Dropdown>().captionText;
        foreach (EnumScript.TailleSortDeZone taille in EnumScript.TailleSortDeZone.GetValues(typeof(EnumScript.TailleSortDeZone)))
            editorZoneTaille.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(taille.ToString()));
        editorZoneTaille.GetComponent<Dropdown>().captionText = editorZoneTaille.GetComponent<Dropdown>().captionText;


        //editor
        editorGabaritChanged();
        majPattern();

        //chooser
        fillClassChooserTable();
        majCaracSortClicked();
        setSelectedFace(selectedFace);
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpingCanvas) lerpCanvas();
    }


    public void changeCanvas(int can)
    {
        if (can != canvasChooserEditor)
        {
            if (can == 1) //On veut le chooser
            {
                //On passe le chooser en actif
                classChooser.GetComponent<CanvasGroup>().interactable = true;
                classChooser.GetComponent<CanvasGroup>().alpha = 1f;
                classChooser.GetComponent<RectTransform>().localScale = new Vector3(0.03f, 0.03f, 0.03f);
                //classChooser.GetComponent<RectTransform>().position = menuActif.transform.position;

                //On passe le editor en top
                classEditor.GetComponent<CanvasGroup>().interactable = false;
                classEditor.GetComponent<CanvasGroup>().alpha = .5f;
                classEditor.GetComponent<RectTransform>().localScale = new Vector3(0.027f, 0.027f, 0.027f);
                //classEditor.GetComponent<RectTransform>().position = menuTop.transform.position;

                lerpingCanvas = true;
            }
            else if (can == 2) //On veut l'editor
            {
                //On passe le chooser en bottom
                classChooser.GetComponent<CanvasGroup>().interactable = false;
                classChooser.GetComponent<CanvasGroup>().alpha = 0.5f;
                classChooser.GetComponent<RectTransform>().localScale = new Vector3(0.027f, 0.027f, 0.027f);
                //classChooser.GetComponent<RectTransform>().position = menuBottom.transform.position;

                //On passe le editor en actif
                classEditor.GetComponent<CanvasGroup>().interactable = true;
                classEditor.GetComponent<CanvasGroup>().alpha = 1;
                classEditor.GetComponent<RectTransform>().localScale = new Vector3(0.03f, 0.03f, 0.03f);
                //classEditor.GetComponent<RectTransform>().position = menuActif.transform.position;

                lerpingCanvas = true;
            }
        }
        canvasChooserEditor = can;
    }


    public void lerpCanvas()
    {
        if (canvasChooserEditor == 1) // on veut le chooser
        {
            classChooser.transform.position = Vector3.Lerp(classChooser.transform.position, menuActif.transform.position, .05f);
            classEditor.transform.position = Vector3.Lerp(classEditor.transform.position, menuTop.transform.position, .05f);
        }
        else if (canvasChooserEditor == 2) // On veut l'editor
        {
            classChooser.transform.position = Vector3.Lerp(classChooser.transform.position, menuBottom.transform.position, .05f);
            classEditor.transform.position = Vector3.Lerp(classEditor.transform.position, menuActif.transform.position, .05f);
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


    public void chooserTypeChanged(int type) //1 proj - 2 zone - 3 CaC - 4 support
    {
        //mettre en surbrillance le bouton
        //afficher le panel correcspondant

        //NOT_PRESSED
        if (selectedType == 1)
        {
            editorButtonProj.GetComponent<Image>().sprite = buttonNotPressed;
            editorPanelProj.SetActive(false);
        }
        else if (selectedType == 2)
        {
            editorButtonZone.GetComponent<Image>().sprite = buttonNotPressed;
            editorPanelZone.SetActive(false);
        }
        else if (selectedType == 3)
        {
            editorButtonCaC.GetComponent<Image>().sprite = buttonNotPressed;
            editorPanelCaC.SetActive(false);
        }
        else if (selectedType == 4)
        {
            editorButtonSupport.GetComponent<Image>().sprite = buttonNotPressed;
            editorPanelSupport.SetActive(false);
        }
        //PRESSED
        if (type == 1) //Proj
            editorButtonProj.GetComponent<Image>().sprite = buttonPressed;
        else if (type == 2)//Zone
            editorButtonZone.GetComponent<Image>().sprite = buttonPressed;
        else if (type == 3)//Cac
            editorButtonCaC.GetComponent<Image>().sprite = buttonPressed;
        else if (type == 4)//Support
            editorButtonSupport.GetComponent<Image>().sprite = buttonPressed;
        selectedType = type;
        //PANEL
        if (selectedType == 1)
            editorPanelProj.SetActive(true);
        else if (selectedType == 2)
            editorPanelZone.SetActive(true);
        else if (selectedType == 2)
            editorPanelCaC.SetActive(true);
        else if (selectedType == 2)
            editorPanelSupport.SetActive(true);
    }


    public void editorElementChanged()
    {
        //JET
        //On récupère la liste des projectiles de cet element puis affichage dans la dropview
        List<structSortJet> listSort = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorProjElement.GetComponent<Dropdown>().value);
        editorProjProjectile.GetComponent<Dropdown>().options.Clear();
        foreach (structSortJet proj in listSort)
            editorProjProjectile.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(proj.nomProj));
        editorProjProjectile.GetComponent<Dropdown>().value = 0;
        editorProjProjectile.GetComponent<Dropdown>().captionText = editorProjProjectile.GetComponent<Dropdown>().captionText;
        //changement de l'élément et du projectile
        sortDeJetEnConstruction.setElement((EnumScript.Element)editorProjElement.GetComponent<Dropdown>().value);
        string nomProj = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorProjElement.GetComponent<Dropdown>().value)[editorProjProjectile.GetComponent<Dropdown>().value].nomProj;
        sortDeJetEnConstruction.nomProj = nomProj;

        //ZONE
        //On récupère la liste des projectiles de cet element puis affichage dans la dropview
        List<structSortDeZone> listSortZ = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value);
        editorZoneSort.GetComponent<Dropdown>().options.Clear();
        foreach (structSortDeZone proj in listSortZ)
            editorZoneSort.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(proj.nomZone));
        editorZoneSort.GetComponent<Dropdown>().captionText = editorZoneSort.GetComponent<Dropdown>().captionText;
        //changement de l'élément et du projectile
        sortDeZoneEnConstruction.setElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value);
        string nomSort = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value)[editorZoneSort.GetComponent<Dropdown>().value].nomZone;
        sortDeZoneEnConstruction.setNomSort(nomSort);

        majCaracSortEnConstr();
    }

    public void editorProjectileChanged()
    {
        //On récupere le nom du prjectile et on l'affecte au sort
        string nomProj = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorProjElement.GetComponent<Dropdown>().value)[editorProjProjectile.GetComponent<Dropdown>().value].nomProj;
        sortDeJetEnConstruction.nomProj = nomProj;

        //Mise a jour des carac + affichage
        majCaracSortEnConstr();
    }

    public void editorZoneChanged()
    {
        string nomZone = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value)[editorZoneSort.GetComponent<Dropdown>().value].nomZone;
        string partSys = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value)[editorZoneSort.GetComponent<Dropdown>().value].partSysStr;
        sortDeZoneEnConstruction.nomZone = nomZone;
        sortDeZoneEnConstruction.particleSystemStr = partSys;

        //Mise a jour des carac + affichage
        majCaracSortEnConstr();
    }

    public void editorPatternChanged()
    {
        switch (listPatternsSortJet[editorProjNbProj.GetComponent<Dropdown>().value][editorProjPattern.GetComponent<Dropdown>().value])
        {
            case "Unique":
                sortDeJetEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.Unique;
                break;
            case "Rafale":
                sortDeJetEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.Rafale;
                break;
            case "Simultané ligne":
                sortDeJetEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.SimultaneLigne;
                break;
            case "Simultané triangle":
                sortDeJetEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.SimultaneTriangle;
                break;
            case "Simultané carré":
                sortDeJetEnConstruction.patternEnvoi = EnumScript.PatternSortDeJet.SimultaneCarre;
                break;
        }
    }

    public void editorRayonChanged() //TODO
    {

    }

    public void editorAngleChanged() //TODO
    {

    }

    public void editorGabaritChanged()
    {
        sortDeZoneEnConstruction.gabarit = (EnumScript.GabaritSortDeZone)editorZoneGabarit.GetComponent<Dropdown>().value;
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

    public void sauvegarderSort()
    {
        if (selectedType == 1) //Projectile / Jet
        {
            if (editorProjNomSort.GetComponent<InputField>().text != "")
                sortDeJetEnConstruction.setNomSort(editorProjNomSort.GetComponent<InputField>().text);
            else
                sortDeJetEnConstruction.setNomSort("_defaut_");
            sortDeJetEnConstruction.gravite = editorProjGravite.GetComponent<Toggle>().isOn;

            GameObject.Find("Player").GetComponent<Player>().addAttaqueToInventaire(sortDeJetEnConstruction);
        }
        else if (selectedType == 2) //Zone
        {
            if (editorZoneNomSort.GetComponent<InputField>().text != "")
                sortDeZoneEnConstruction.setNomSort(editorZoneNomSort.GetComponent<InputField>().text);
            else
                sortDeZoneEnConstruction.setNomSort("_defaut_");

            string partSys = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value)[editorZoneSort.GetComponent<Dropdown>().value].partSysStr;
            sortDeZoneEnConstruction.particleSystemStr = partSys;
            switch ((EnumScript.GabaritSortDeZone) editorZoneGabarit.GetComponent<Dropdown>().value)
            {
                case EnumScript.GabaritSortDeZone.Cercle:
                    sortDeZoneEnConstruction.particleSystemStr += "Circle";
                    break;
                case EnumScript.GabaritSortDeZone.Ligne:
                    sortDeZoneEnConstruction.particleSystemStr += "Line";
                    break;
                case EnumScript.GabaritSortDeZone.Cone:
                    sortDeZoneEnConstruction.particleSystemStr += "Cone";
                    break;
            }
            switch (editorZoneTaille.GetComponent<Dropdown>().value)
            {
                case 0:
                    sortDeZoneEnConstruction.particleSystemStr += "S";
                    break;
                case 1:
                    sortDeZoneEnConstruction.particleSystemStr += "M";
                    break;
                case 2:
                    sortDeZoneEnConstruction.particleSystemStr += "L";
                    break;
            }
            string nomZone = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value)[editorZoneSort.GetComponent<Dropdown>().value].nomZone;
            sortDeZoneEnConstruction.nomZone = nomZone;

            
            Debug.Log(sortDeZoneEnConstruction.getNomSort());
            Debug.Log(sortDeZoneEnConstruction.nomZone);
            Debug.Log(sortDeZoneEnConstruction.particleSystemStr);


            GameObject.Find("Player").GetComponent<Player>().addAttaqueToInventaire(sortDeZoneEnConstruction);
        }

        resetClassChooser();
        fillClassChooserTable();
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
            selectedSort = position;
        }

        majCaracSortClicked();
    }

    public void EquiperDesequiperSort()
    {
        if (selectedSort >= 0) //Sort inventaire
        {
            GameObject.Find("Player").GetComponent<Player>().equipeAttaqueAt(selectedFace, selectedSort); //On le met dans les sorts équipé pour la face
            for (int nbButton = 0; nbButton < chooserScrollListSortEq.transform.GetChild(0).GetChild(0).childCount; nbButton++) //On supprime le/les sort équipé de la list
                Destroy(chooserScrollListSortEq.transform.GetChild(0).GetChild(0).GetChild(nbButton).gameObject);
            GameObject button = Instantiate(buttonTemplate) as GameObject; //On met le nouveau
            button.transform.SetParent(chooserScrollListSortEq.transform.GetChild(0).GetChild(0));
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Player").GetComponent<Player>().getListAttaqueInventaire()[selectedSort].getNomSort();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //-1 -> sort équipé
            button.GetComponent<Button>().onClick.AddListener(() => buttonClicked(-1));
        }
        else
        {
            if (selectedSort == -1) //Sort équipé
            {
                GameObject.Find("Player").GetComponent<Player>().desequipeAttaque(selectedFace);
                Destroy(chooserScrollListSortEq.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
                selectedSort = 0;
            }
        }
    }


    public void resetEditorWindow()
    {
        editorProjNomSort.GetComponent<InputField>().text = "";
        editorProjElement.GetComponent<Dropdown>().value = 0; editorProjElement.GetComponent<Dropdown>().captionText = editorProjElement.GetComponent<Dropdown>().captionText;
        editorProjProjectile.GetComponent<Dropdown>().value = 0; editorProjProjectile.GetComponent<Dropdown>().captionText = editorProjProjectile.GetComponent<Dropdown>().captionText;
        editorProjGravite.GetComponent<Toggle>().isOn = true;
        editorProjNbProj.GetComponent<Dropdown>().value = 0; editorProjNbProj.GetComponent<Dropdown>().captionText = editorProjNbProj.GetComponent<Dropdown>().captionText;
        editorProjPattern.GetComponent<Dropdown>().value = 0; editorProjPattern.GetComponent<Dropdown>().captionText = editorProjPattern.GetComponent<Dropdown>().captionText;

        editorZoneNomSort.GetComponent<InputField>().text = "";
        editorZoneElement.GetComponent<Dropdown>().value = 0; editorZoneElement.GetComponent<Dropdown>().captionText = editorZoneElement.GetComponent<Dropdown>().captionText;
        editorZoneGabarit.GetComponent<Dropdown>().value = 0; editorZoneGabarit.GetComponent<Dropdown>().captionText = editorZoneGabarit.GetComponent<Dropdown>().captionText;
        editorZoneSort.GetComponent<Dropdown>().value = 0; editorZoneSort.GetComponent<Dropdown>().captionText = editorZoneSort.GetComponent<Dropdown>().captionText;
        editorZoneTaille.GetComponent<Dropdown>().value = 0; editorZoneTaille.GetComponent<Dropdown>().captionText = editorZoneTaille.GetComponent<Dropdown>().captionText;

        majCaracSortEnConstr();
    }

    public void resetClassChooser()
    {
        for (int nbButton = 0; nbButton < chooserScrollListSortInv.transform.GetChild(0).GetChild(0).childCount; nbButton++)
            Destroy(chooserScrollListSortInv.transform.GetChild(0).GetChild(0).GetChild(nbButton).gameObject);
        for (int nbButton = 0; nbButton < chooserScrollListSortEq.transform.GetChild(0).GetChild(0).childCount; nbButton++)
            Destroy(chooserScrollListSortEq.transform.GetChild(0).GetChild(0).GetChild(nbButton).gameObject);
    }

    public void majCaracSortEnConstr()
    {
        //JET
        //On récupère la structure du sort depuis l'element et le nom
        structSortJet structureDuSortJ = GetComponent<CaracProjectiles>().getStructFromNameAndElement(sortDeJetEnConstruction.nomProj, sortDeJetEnConstruction.getElement());
        //On modifie le sort en conséquent
        sortDeJetEnConstruction.nbProjectile = editorProjNbProj.GetComponent<Dropdown>().value + 1;
        sortDeJetEnConstruction.vitesseProj = structureDuSortJ.vitesse;
        sortDeJetEnConstruction.setCooldown(structureDuSortJ.cooldown);
        sortDeJetEnConstruction.setDegats(structureDuSortJ.degats / sortDeJetEnConstruction.nbProjectile);
        //On affiche les nouvelles caractéristiques
        editorProjVitesse.GetComponent<Text>().text = sortDeJetEnConstruction.vitesseProj.ToString();
        editorProjCoolDown.GetComponent<Text>().text = sortDeJetEnConstruction.getCooldown().ToString();
        editorProjDegats.GetComponent<Text>().text = sortDeJetEnConstruction.getDegats().ToString();

        //ZONE
        structSortDeZone structureDuSortZ = GetComponent<CaracZones>().getStructFromNameAndElement(sortDeZoneEnConstruction.nomZone, sortDeZoneEnConstruction.getElement());
        //On modifie le sort en conséquent
        sortDeZoneEnConstruction.setCooldown(structureDuSortZ.cooldown);
        sortDeZoneEnConstruction.setDegats(structureDuSortZ.degats);
        sortDeZoneEnConstruction.taille = (EnumScript.TailleSortDeZone)editorZoneTaille.GetComponent<Dropdown>().value;
        //On affiche les nouvelles caractéristiques
        editorZoneCoolDown.GetComponent<Text>().text = sortDeZoneEnConstruction.getCooldown().ToString();
        editorZoneDegats.GetComponent<Text>().text = sortDeZoneEnConstruction.getDegats().ToString();
    }


    //TODO changer le panel d'affchage (zone -> jet   jet -> zone)
    public void majCaracSortClicked()
    {
        if (selectedSort >= 0) //Sort de l'inventaire
        {
            int type = GameObject.FindWithTag("Player").GetComponent<Player>().getListAttaqueInventaire()[selectedSort].type;
            if (type == 1) // sort de jet
            {
                chooserPanelProj.SetActive(true);
                chooserPanelZone.SetActive(false);

                SortDeJet sortChoisi = (SortDeJet)GameObject.FindWithTag("Player").GetComponent<Player>().getListAttaqueInventaire()[selectedSort];
                chooserProjNomSort.GetComponent<Text>().text = sortChoisi.getNomSort();
                chooserProjElement.GetComponent<Text>().text = sortChoisi.getElement().ToString();
                chooserProjProjectile.GetComponent<Text>().text = sortChoisi.nomProj;
                chooserProjGravite.GetComponent<Text>().text = (sortChoisi.gravite) ? "oui" : "non";
                chooserProjNbProj.GetComponent<Text>().text = sortChoisi.nbProjectile.ToString();
                chooserProjPattern.GetComponent<Text>().text = sortChoisi.patternEnvoi.ToString();
                chooserProjVitesse.GetComponent<Text>().text = sortChoisi.vitesseProj.ToString();
                chooserProjCoolDown.GetComponent<Text>().text = sortChoisi.getCooldown().ToString();
                chooserProjDegats.GetComponent<Text>().text = sortChoisi.getDegats().ToString();
            }
            else if (type == 2) // sort de zone
            {
                chooserPanelProj.SetActive(false);
                chooserPanelZone.SetActive(true);

                SortDeZone sortChoisi = (SortDeZone)GameObject.FindWithTag("Player").GetComponent<Player>().getListAttaqueInventaire()[selectedSort];
                chooserZoneNomSort.GetComponent<Text>().text = sortChoisi.getNomSort();
                chooserZoneElement.GetComponent<Text>().text = sortChoisi.getElement().ToString();
                chooserZoneGabarit.GetComponent<Text>().text = sortChoisi.gabarit.ToString();
                chooserZoneSort.GetComponent<Text>().text = sortChoisi.nomZone;
                chooserZoneCoolDown.GetComponent<Text>().text = sortChoisi.getCooldown().ToString();
                chooserZoneDegats.GetComponent<Text>().text = sortChoisi.getDegats().ToString();
            }
        }
        else //Sort équipé
        {
            int type = GameObject.FindWithTag("Player").GetComponent<Player>().getAttaqueEquipOnFace(selectedFace).type;
            if (type == 1) // sort de jet
            {
                chooserPanelProj.SetActive(true);
                chooserPanelZone.SetActive(false);

                SortDeJet sortChoisi = (SortDeJet)GameObject.FindWithTag("Player").GetComponent<Player>().getAttaqueEquipOnFace(selectedFace);
                chooserProjNomSort.GetComponent<Text>().text = sortChoisi.getNomSort();
                chooserProjElement.GetComponent<Text>().text = sortChoisi.getElement().ToString();
                chooserProjProjectile.GetComponent<Text>().text = sortChoisi.nomProj;
                chooserProjGravite.GetComponent<Text>().text = (sortChoisi.gravite) ? "oui" : "non";
                chooserProjNbProj.GetComponent<Text>().text = sortChoisi.nbProjectile.ToString();
                chooserProjPattern.GetComponent<Text>().text = sortChoisi.patternEnvoi.ToString();
                chooserProjVitesse.GetComponent<Text>().text = sortChoisi.vitesseProj.ToString();
                chooserProjCoolDown.GetComponent<Text>().text = sortChoisi.getCooldown().ToString();
                chooserProjDegats.GetComponent<Text>().text = sortChoisi.getDegats().ToString();
            }
            else if (type == 2) // sort de zone
            {
                chooserPanelProj.SetActive(false);
                chooserPanelZone.SetActive(true);

                SortDeZone sortChoisi = (SortDeZone)GameObject.FindWithTag("Player").GetComponent<Player>().getAttaqueEquipOnFace(selectedFace);
                chooserZoneNomSort.GetComponent<Text>().text = sortChoisi.getNomSort();
                chooserZoneElement.GetComponent<Text>().text = sortChoisi.getElement().ToString();
                chooserZoneGabarit.GetComponent<Text>().text = sortChoisi.gabarit.ToString();
                chooserZoneSort.GetComponent<Text>().text = sortChoisi.nomZone;
                chooserZoneCoolDown.GetComponent<Text>().text = sortChoisi.getCooldown().ToString();
                chooserZoneDegats.GetComponent<Text>().text = sortChoisi.getDegats().ToString();
            }
        }
    }

    public void fillClassChooserTable()
    {
        GameObject button;
        for (int nbButton = 0; nbButton < GameObject.Find("Player").GetComponent<Player>().getListAttaqueInventaire().Count; nbButton++)
        {
            button = Instantiate(buttonTemplate) as GameObject;
            button.transform.SetParent(chooserScrollListSortInv.transform.GetChild(0).GetChild(0));
            button.transform.localPosition = new Vector3(0, -1 * nbButton * button.GetComponent<RectTransform>().rect.height, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Player").GetComponent<Player>().getListAttaqueInventaire()[nbButton].getNomSort();

            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //Ne pas enlever : permet au bouton de marcher
            int nbButtonTemp = nbButton;
            button.GetComponent<Button>().onClick.AddListener(() => { this.buttonClicked(nbButtonTemp); });
            //Redimensionnement bouton

        }
        //On adapte la taille du content pour que les boutons prennent la bonne taille
        float hauteurFinal = 10 + 10 + GameObject.Find("Player").GetComponent<Player>().getListAttaqueInventaire().Count * buttonHeight + 10 * (GameObject.Find("Player").GetComponent<Player>().getListAttaqueInventaire().Count - 1);
        //chooserScrollListSortInv.GetComponent<RectTransform>().sizeDelta = new Vector2(chooserScrollListSortInv.GetComponent<RectTransform>().sizeDelta.x, hauteurFinal);
        chooserScrollListSortInv.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(chooserScrollListSortInv.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.x, hauteurFinal);
        //Ajout du bouton du sort équipé
        if (GameObject.Find("Player").GetComponent<Player>().getAttaqueEquipOnFace(selectedFace) != null)
        {
            button = Instantiate(buttonTemplate) as GameObject;
            button.transform.SetParent(chooserScrollListSortEq.transform.GetChild(0).GetChild(0));
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Player").GetComponent<Player>().getAttaqueEquipOnFace(selectedFace).getNomSort();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //-1 -> sort équipé
            button.GetComponent<Button>().onClick.AddListener(() => buttonClicked(-1));
        }
        //On adapte la taille du content pour que les boutons prennent la bonne taille
        float hauteurFinale = 10 + 10 + buttonHeight;
        chooserScrollListSortEq.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(chooserScrollListSortEq.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.x, hauteurFinale);
    }

    public void majPattern()
    {
        //edit.GetComponent<Dropdown>().enabled = true;
        editorProjPattern.GetComponent<Dropdown>().options.Clear();
        foreach (string pat in listPatternsSortJet[editorProjNbProj.GetComponent<Dropdown>().value]) //En fonction du nombre de projectile
            editorProjPattern.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(pat));

        editorProjPattern.GetComponent<Dropdown>().captionText = editorProjPattern.GetComponent<Dropdown>().captionText;

        sortDeJetEnConstruction.nbProjectile = editorProjNbProj.GetComponent<Dropdown>().value;

        majCaracSortEnConstr();
    }

    public void exitMenu()
    {
        accessMenu.GetComponent<accessMenu>().exitMenu();
    }

    public void setPopUpYesNoResult(bool p_result)
    {
        popUpYesNoResult = p_result;
        popUpYesNoAnswered = true;
        fctToCallWhenPopUpYesNoAnswer();
    }
}
