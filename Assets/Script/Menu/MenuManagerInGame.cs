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
    public GameObject player;
    [Space(15)]

    public Sprite buttonNotPressed;
    public Sprite buttonPressed;

    private const float buttonHeight = 40;
    private const float delayDoubleClick = 0.4f;

    public GameObject buttonTemplate;
    [Space(15)]
    public GameObject classChooser;
    public GameObject classCreator;
    public GameObject classEditor;
    public GameObject popUpYesNo;

    public GameObject menuActif;
    public GameObject menuTop;
    public GameObject menuBottom;

    [Space(15)]
    public GameObject creatorButtonsFaces;
    public GameObject creatorTextFacesChosen;
    [Space(40)]
    public GameObject creatorButtonProj;
    public GameObject creatorButtonZone;
    public GameObject creatorButtonCaC;
    public GameObject creatorButtonSupport;
    public GameObject creatorPanelProj;
    public GameObject creatorPanelZone;
    public GameObject creatorPanelCaC;
    public GameObject creatorPanelSupport;
    public GameObject creatorPanelHorizBarre;
    [Space(15)]
    public GameObject creatorProjPseudoSort;
    public GameObject creatorProjElement;
    public GameObject creatorProjProjectile;
    public GameObject creatorProjNbProj;
    public GameObject creatorProjPattern;
    public GameObject creatorProjVitesse;
    public GameObject creatorProjCoolDown;
    public GameObject creatorProjDegats;
    [Space(15)]
    public GameObject creatorZonePseudoSort;
    public GameObject creatorZoneElement;
    public GameObject creatorZoneGabarit;
    public GameObject creatorZoneSort;
    public GameObject creatorZoneCoolDown;
    public GameObject creatorZoneDegats;

    [Space(40)]
    public GameObject chooserPanelProj;
    public GameObject chooserPanelZone;
    [Space(15)]
    public GameObject chooserProjPseudoSort;
    public GameObject chooserProjElement;
    public GameObject chooserProjProjectile;
    public GameObject chooserProjNbProj;
    public GameObject chooserProjPattern;
    public GameObject chooserProjVitesse;
    public GameObject chooserProjCoolDown;
    public GameObject chooserProjDegats;
    [Space(15)]
    public GameObject chooserZonePseudoSort;
    public GameObject chooserZoneElement;
    public GameObject chooserZoneGabarit;
    public GameObject chooserZoneSort;
    public GameObject chooserZoneCoolDown;
    public GameObject chooserZoneDegats;
    [Space(15)]
    public GameObject chooserButtonNouveauSort;
    public GameObject chooserScrollListSortInv;
    public GameObject chooserScrollListSortEq;
    [Space(15)]
    public GameObject editorProgressBar;
    public GameObject editorLvl;
    public GameObject editorPanelProj;
    public GameObject editorPanelZone;
    [Space(15)]
    public GameObject editorProjNom;
    public GameObject editorProjElement;
    public GameObject editorProjProj;
    public GameObject editorProjVitesse;
    public GameObject editorProjCooldown;
    public GameObject editorProjDegats;
    [Space(8)]
    public GameObject editorProjBonusVitesse;
    public GameObject editorProjBonusCooldown;
    public GameObject editorProjBonusDegats;
    [Space(15)]
    public GameObject editorZoneNom;
    public GameObject editorZoneElement;
    public GameObject editorZoneSort;
    public GameObject editorZoneCooldown;
    public GameObject editorZoneDegats;
    [Space(15)]
    public GameObject editorZoneBonusVitesse;
    public GameObject editorZoneBonusCooldown;
    public GameObject editorZoneBonusDegats;
    [Space(15)]
    public GameObject caracSorts;



    private int selectedSort = -100; // >= 0 inventaire    <0 équipéss    -100 pas de sort
    private int selectedFace = 1;
    private int selectedType;

    private SortDeJet sortDeJetEnConstruction;
    private SortDeZone sortDeZoneEnConstruction;
    private Attaque sortSelectionne;
    private structSortJet structSortDeJetEnEdition;
    private structSortDeZone structSortDeZoneEnEdition;

    private Color colorButtonFaceNormal;
    private Color colorButtonFaceSelected;

    private bool popUpYesNoAnswered;
    private bool popUpYesNoResult;
    private bool delegateYesNoUsed;
    public delegate void DelYesNo();
    DelYesNo fctToCallWhenPopUpYesNoAnswer;
    
    private float doubleClickStart = -1;

    int canvasType; //1 = Chooser   2 = Editor
    bool lerpingCanvas;

    void Start()
    {
        StartCoroutine(seekPlayer());
    }

    public IEnumerator seekPlayer()
    {
        while (GameObject.FindWithTag("Player") == null)
            yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(0.1f);
        InitMenu();
    }

    // Use this for initialization
    void InitMenu()
    {
        player = GameObject.FindWithTag("Player");
        
        canvasType = 1; // Chooser
        lerpingCanvas = false;

        sortDeJetEnConstruction = new SortDeJet();
        sortDeZoneEnConstruction = new SortDeZone();

        selectedType = 1;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        selectedFace = 1;
        selectedSort = -100;

        popUpYesNoAnswered = false;
        popUpYesNoResult = false;
        delegateYesNoUsed = false;

        //PROJECTILE
        //On remplit la Dropdown des projectiles
        creatorProjProjectile.GetComponent<Dropdown>().options.Clear();
        foreach (structSortJet sort in caracSorts.GetComponent<CaracProjectiles>().tabSort)
            creatorProjProjectile.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(sort.nameInMenu));
        creatorProjProjectile.GetComponent<Dropdown>().value = 0;
        creatorProjProjectile.GetComponent<Dropdown>().captionText = creatorProjProjectile.GetComponent<Dropdown>().captionText;
        //Ensuite on charge les elements dispo avec le sort choisi
        creatorProjElement.GetComponent<Dropdown>().options.Clear();
        foreach (EnumScript.Element elem in caracSorts.GetComponent<CaracProjectiles>().getElemFromProj(caracSorts.GetComponent<CaracProjectiles>().tabSort[0].nomParticle))
            creatorProjElement.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
        creatorProjElement.GetComponent<Dropdown>().value = 0;
        creatorProjElement.GetComponent<Dropdown>().captionText = creatorProjElement.GetComponent<Dropdown>().captionText;

        //ZONE
        creatorZoneSort.GetComponent<Dropdown>().options.Clear();
        foreach (structSortDeZone sort in caracSorts.GetComponent<CaracZones>().tabSort)
            creatorZoneSort.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(sort.nameInMenu));
        creatorZoneSort.GetComponent<Dropdown>().value = 0;
        creatorZoneSort.GetComponent<Dropdown>().captionText = creatorZoneSort.GetComponent<Dropdown>().captionText;
        //Ensuite on charge les elements dispo avec le sort choisi
        creatorZoneElement.GetComponent<Dropdown>().options.Clear();
        foreach (EnumScript.Element elem in caracSorts.GetComponent<CaracZones>().getElemFromZone(caracSorts.GetComponent<CaracZones>().tabSort[0].nomParticle))
            creatorZoneElement.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
        creatorZoneElement.GetComponent<Dropdown>().value = 0;
        creatorZoneElement.GetComponent<Dropdown>().captionText = creatorZoneElement.GetComponent<Dropdown>().captionText;

        creatorZoneChanged();
        creatorProjectileChanged();
        creatorElementChanged();
        fillClassChooserTable();
        majCaracSortClicked();
        setSelectedFace(selectedFace);

        classChooser.transform.position = menuActif.transform.position;
        classCreator.transform.position = menuTop.transform.position;
        classEditor.transform.position = menuTop.transform.position + new Vector3(0, 0, .25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpingCanvas) lerpCanvas();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(canvasType == 1)
                accessMenu.GetComponent<accessMenu>().exitMenu();
            else
                changeCanvas(1);
        }
    }

    public void changeCanvas(int can)
    {
        if (can != canvasType)
        {
            if (can == 1) //On veut le chooser
            {
                //On passe le chooser en actif
                classChooser.GetComponent<CanvasGroup>().interactable = true;
                classChooser.GetComponent<CanvasGroup>().alpha = 1f;
                classChooser.GetComponent<RectTransform>().localScale = new Vector3(0.03f, 0.03f, 0.03f);

                //On passe le creator en top
                classCreator.GetComponent<CanvasGroup>().interactable = false;
                classCreator.GetComponent<CanvasGroup>().alpha = .5f;
                classCreator.GetComponent<RectTransform>().localScale = new Vector3(0.027f, 0.027f, 0.027f);

                //On passe le creator en top
                classEditor.GetComponent<CanvasGroup>().interactable = false;
                classEditor.GetComponent<CanvasGroup>().alpha = .5f;
                classEditor.GetComponent<RectTransform>().localScale = new Vector3(0.027f, 0.027f, 0.027f);

                lerpingCanvas = true;

                canvasType = can;
            }
            else if (can == 2) //On veut le creator
            {
                //On passe le chooser en bottom
                classChooser.GetComponent<CanvasGroup>().interactable = false;
                classChooser.GetComponent<CanvasGroup>().alpha = 0.5f;
                classChooser.GetComponent<RectTransform>().localScale = new Vector3(0.027f, 0.027f, 0.027f);

                //On passe le creator en actif
                classCreator.GetComponent<CanvasGroup>().interactable = true;
                classCreator.GetComponent<CanvasGroup>().alpha = 1;
                classCreator.GetComponent<RectTransform>().localScale = new Vector3(0.03f, 0.03f, 0.03f);

                lerpingCanvas = true;

                canvasType = can;
            }
            else if (can == 3) //On veut l'editor
            {
                print(selectedSort);
                if (selectedSort != -100)
                {
                    //On passe le chooser en bottom
                    classChooser.GetComponent<CanvasGroup>().interactable = false;
                    classChooser.GetComponent<CanvasGroup>().alpha = 0.5f;
                    classChooser.GetComponent<RectTransform>().localScale = new Vector3(0.027f, 0.027f, 0.027f);

                    //On passe le editor en actif
                    classEditor.GetComponent<CanvasGroup>().interactable = true;
                    classEditor.GetComponent<CanvasGroup>().alpha = 1;
                    classEditor.GetComponent<RectTransform>().localScale = new Vector3(0.03f, 0.03f, 0.03f);

                    setEditorCanvas(true);

                    lerpingCanvas = true;

                    canvasType = can;
                }
            }
        }
    }

    public void lerpCanvas()
    {
        if (canvasType == 1) // on veut le chooser
        {
            classChooser.transform.position = Vector3.Lerp(classChooser.transform.position, menuActif.transform.position, .05f);
            classCreator.transform.position = Vector3.Lerp(classCreator.transform.position, menuTop.transform.position, .05f);
            classEditor.transform.position = Vector3.Lerp(classEditor.transform.position, menuTop.transform.position + new Vector3(0, 0, .25f), .05f);
        }
        else if (canvasType == 2) // On veut l'editor
        {
            classChooser.transform.position = Vector3.Lerp(classChooser.transform.position, menuBottom.transform.position, .05f);
            classCreator.transform.position = Vector3.Lerp(classCreator.transform.position, menuActif.transform.position, .05f);
        }
        else if (canvasType == 3) // On veut l'editor
        {
            classChooser.transform.position = Vector3.Lerp(classChooser.transform.position, menuBottom.transform.position, .05f);
            classEditor.transform.position = Vector3.Lerp(classEditor.transform.position, menuActif.transform.position, .05f);
        }
    }

    public void setSelectedFace(int face)
    {
        //Changement titre fenetre
        creatorTextFacesChosen.GetComponent<Text>().text = "Personnalitsation de la face n° " + face;

        //On met le bouton de l'ancienne face en blanc et le nouveau en cyan
        creatorButtonsFaces.transform.GetChild(selectedFace - 1).GetComponent<Button>().image.color = Color.white;
        selectedFace = face;
        creatorButtonsFaces.transform.GetChild(selectedFace - 1).GetComponent<Button>().image.color = Color.cyan;

        if (selectedSort == -1) //sort équipé selectionné
            if (player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace) == null)
                selectedSort = -100;

        //Mise a jour caractéristiques affichés
        majCaracSortClicked();
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
            creatorPanelProj.SetActive(false);
        else if (selectedType == 2)
            creatorPanelZone.SetActive(false);
        else if (selectedType == 3)
            creatorPanelCaC.SetActive(false);
        else if (selectedType == 4)
            creatorPanelSupport.SetActive(false);
        selectedType = type;
        //PANEL
        if (selectedType == 1)
        {
            creatorPanelProj.SetActive(true);
            creatorPanelHorizBarre.transform.SetAsLastSibling();
            creatorButtonProj.transform.SetAsLastSibling();
        }
        else if (selectedType == 2)
        {
            creatorPanelZone.SetActive(true);
            creatorPanelHorizBarre.transform.SetAsLastSibling();
            creatorButtonZone.transform.SetAsLastSibling();
        }
        else if (selectedType == 3)
        {
            creatorPanelCaC.SetActive(true);
            creatorPanelHorizBarre.transform.SetAsLastSibling();
            creatorButtonCaC.transform.SetAsLastSibling();
        }
        else if (selectedType == 4)
        {
            creatorPanelSupport.SetActive(true);
            creatorPanelHorizBarre.transform.SetAsLastSibling();
            creatorButtonSupport.transform.SetAsLastSibling();
        }
    }

    public void creatorElementChanged()
    {
        //JET
        sortDeJetEnConstruction.setElement(caracSorts.GetComponent<CaracProjectiles>().getElemFromProj(sortDeJetEnConstruction.getNameParticle())[creatorProjElement.GetComponent<Dropdown>().value]);
        //ZONE
        sortDeZoneEnConstruction.setElement(caracSorts.GetComponent<CaracZones>().getElemFromZone(sortDeZoneEnConstruction.getNameParticle())[creatorZoneElement.GetComponent<Dropdown>().value]);
        //Mise a jour des carac + affichage
        majCaracSortEnConstr();
    }

    public void creatorProjectileChanged()
    {
        //On récupere le nom du prjectile et on l'affecte au sort
        string nomPart = caracSorts.GetComponent<CaracProjectiles>().tabSort[creatorProjProjectile.GetComponent<Dropdown>().value].nomParticle;
        sortDeJetEnConstruction.setNameParticle(nomPart);

        //On charge les elements disponible pour ce sort
        creatorProjElement.GetComponent<Dropdown>().options.Clear();
        foreach (EnumScript.Element elem in caracSorts.GetComponent<CaracProjectiles>().getElemFromProj(nomPart))
            creatorProjElement.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
        creatorProjElement.GetComponent<Dropdown>().value = 0;
        creatorProjElement.GetComponent<Dropdown>().captionText = creatorProjElement.GetComponent<Dropdown>().captionText;

        //Mise a jour des carac + affichage
        majCaracSortEnConstr();
    }

    public void creatorZoneChanged()
    {
        //On récupere le nom du sortdezone et on l'affecte au sort
        string nameZone = caracSorts.GetComponent<CaracZones>().tabSort[creatorZoneSort.GetComponent<Dropdown>().value].nomParticle;
        sortDeZoneEnConstruction.setNameParticle(nameZone);

        //On charge les elements disponible pour ce sort
        creatorZoneElement.GetComponent<Dropdown>().options.Clear();
        foreach (EnumScript.Element elem in caracSorts.GetComponent<CaracZones>().getElemFromZone(nameZone))
            creatorZoneElement.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
        creatorZoneElement.GetComponent<Dropdown>().value = 0;
        creatorZoneElement.GetComponent<Dropdown>().captionText = creatorZoneElement.GetComponent<Dropdown>().captionText;

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
        if (selectedSort != -1) //Sort non équipé
        {
            if (popUpYesNoAnswered)
            {
                if (popUpYesNoResult)
                {
                    player.GetComponent<Player>().supprimerAttaqueInventaireAt(selectedSort, false);
                }

                popUpYesNo.GetComponent<Canvas>().enabled = false;
                classChooser.GetComponent<CanvasGroup>().blocksRaycasts = true;
                classChooser.GetComponent<CanvasGroup>().alpha = 1f;

                selectedSort = -100;
                popUpYesNoAnswered = false;
                delegateYesNoUsed = true;
            }
            else
            {
                popUpYesNo.GetComponent<Canvas>().enabled = true;
                popUpYesNo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Voulez vous vraiment supprimer cette attaque ?";
                popUpYesNo.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "Supprimer";
                popUpYesNo.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = "Annuler";
                classChooser.GetComponent<CanvasGroup>().blocksRaycasts = false;
                classChooser.GetComponent<CanvasGroup>().alpha = 0.3f;

                fctToCallWhenPopUpYesNoAnswer = supprimerClassDel;
            }
        }
        else if (selectedSort == -1) //Sort équipé
        {
            if (popUpYesNoAnswered)
            {
                if (popUpYesNoResult)
                {
                    player.GetComponent<Player>().supprimerAttaqueInventaireAt(selectedFace, true);
                }

                popUpYesNo.GetComponent<Canvas>().enabled = false;
                classChooser.GetComponent<CanvasGroup>().blocksRaycasts = true;
                classChooser.GetComponent<CanvasGroup>().alpha = 1f;

                selectedSort = -100;
                popUpYesNoAnswered = false;
                delegateYesNoUsed = true;
            }
            else
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
        resetClassChooser();
        fillClassChooserTable();
    }

    public void sauvegarderSort()
    {
        if (selectedType == 1) //Projectile / Jet
        {
            if (creatorProjPseudoSort.GetComponent<InputField>().text != "")
                sortDeJetEnConstruction.setPseudoSort(creatorProjPseudoSort.GetComponent<InputField>().text);
            else
                sortDeJetEnConstruction.setPseudoSort("_defaut_");
            
            player.GetComponent<Player>().addAttaqueToInventaire(sortDeJetEnConstruction);
        }
        else if (selectedType == 2) //Zone
        {
            if (creatorZonePseudoSort.GetComponent<InputField>().text != "")
                sortDeZoneEnConstruction.setPseudoSort(creatorZonePseudoSort.GetComponent<InputField>().text);
            else
                sortDeZoneEnConstruction.setPseudoSort("_defaut_");

            player.GetComponent<Player>().addAttaqueToInventaire(sortDeZoneEnConstruction);
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

    /* Lorsque un sort est équipé, il disparait de l'inventaire 
    *  Lorsque il est déséquipé, il revient dan l'inventaire
    */
    public void EquiperDesequiperSort()
    {
        if (selectedSort >= 0) //Sort inventaire
        {
            player.GetComponent<Player>().equipeAttaqueAt(selectedFace, selectedSort); //On le met dans les sorts équipé pour la face
            for (int nbButton = 0; nbButton < chooserScrollListSortEq.transform.GetChild(0).GetChild(0).childCount; nbButton++) //On supprime le/les sort équipé de la list
                Destroy(chooserScrollListSortEq.transform.GetChild(0).GetChild(0).GetChild(nbButton).gameObject);
            //On supprime le sort de l'inventaire
            Destroy(chooserScrollListSortInv.transform.GetChild(0).GetChild(0).GetChild(selectedSort).gameObject);
            GameObject button = Instantiate(buttonTemplate) as GameObject; //On met le nouveau
            button.transform.SetParent(chooserScrollListSortEq.transform.GetChild(0).GetChild(0));
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.transform.GetChild(0).GetComponent<Text>().text = player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace).getPseudoSort();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //-1 -> sort équipé
            button.GetComponent<Button>().onClick.AddListener(() => buttonClicked(-1));
            //On garde en selection le sort qu'on vient d'équiper
            selectedSort = -1;
        }
        else if (selectedSort == -1) //Sort équipé
        {
            player.GetComponent<Player>().desequipeAttaque(selectedFace);
            Destroy(chooserScrollListSortEq.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
            selectedSort = player.GetComponent<Player>().getListAttaqueInventaire().Count-1;
        }
        resetClassChooser();
        fillClassChooserTable();
    }

    public void resetCreatorWindow()
    {
        creatorProjPseudoSort.GetComponent<InputField>().text = "";
        creatorProjElement.GetComponent<Dropdown>().value = 0; creatorProjElement.GetComponent<Dropdown>().captionText = creatorProjElement.GetComponent<Dropdown>().captionText;
        creatorProjProjectile.GetComponent<Dropdown>().value = 0; creatorProjProjectile.GetComponent<Dropdown>().captionText = creatorProjProjectile.GetComponent<Dropdown>().captionText;
        creatorProjNbProj.GetComponent<Dropdown>().value = 0; creatorProjNbProj.GetComponent<Dropdown>().captionText = creatorProjNbProj.GetComponent<Dropdown>().captionText;
        creatorProjPattern.GetComponent<Dropdown>().value = 0; creatorProjPattern.GetComponent<Dropdown>().captionText = creatorProjPattern.GetComponent<Dropdown>().captionText;

        creatorZonePseudoSort.GetComponent<InputField>().text = "";
        creatorZoneElement.GetComponent<Dropdown>().value = 0; creatorZoneElement.GetComponent<Dropdown>().captionText = creatorZoneElement.GetComponent<Dropdown>().captionText;
        creatorZoneSort.GetComponent<Dropdown>().value = 0; creatorZoneSort.GetComponent<Dropdown>().captionText = creatorZoneSort.GetComponent<Dropdown>().captionText;

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
        structSortJet structureDuSortJ = caracSorts.GetComponent<CaracProjectiles>().getStructFromName(sortDeJetEnConstruction.getNameParticle());
        //On modifie le sort en conséquent
        sortDeJetEnConstruction.vitesseProj = structureDuSortJ.vitesse;
        sortDeJetEnConstruction.setCooldown(structureDuSortJ.cooldown);
        sortDeJetEnConstruction.setNameInMenu(structureDuSortJ.nameInMenu);
        sortDeJetEnConstruction.setDegats(structureDuSortJ.degats);
        //On affiche les nouvelles caractéristiques
        creatorProjVitesse.GetComponent<Text>().text = sortDeJetEnConstruction.vitesseProj.ToString();
        creatorProjCoolDown.GetComponent<Text>().text = sortDeJetEnConstruction.getCooldown().ToString();
        creatorProjDegats.GetComponent<Text>().text = sortDeJetEnConstruction.getDegats().ToString();

        //ZONE
        structSortDeZone structureDuSortZ = caracSorts.GetComponent<CaracZones>().getStructFromName(sortDeZoneEnConstruction.getNameParticle());
        //On modifie le sort en conséquent
        sortDeZoneEnConstruction.setCooldown(structureDuSortZ.cooldown);
        sortDeZoneEnConstruction.setDegats(structureDuSortZ.degats);
        sortDeZoneEnConstruction.setNameInMenu(structureDuSortZ.nameInMenu);
        //On affiche les nouvelles caractéristiques
        creatorZoneCoolDown.GetComponent<Text>().text = sortDeZoneEnConstruction.getCooldown().ToString();
        creatorZoneDegats.GetComponent<Text>().text = sortDeZoneEnConstruction.getDegats().ToString();
    }

    //TODO changer le panel d'affchage (zone -> jet   jet -> zone)
    public void majCaracSortClicked()
    {
        if (selectedSort >= 0) //Sort de l'inventaire
        {
            int type = player.GetComponent<Player>().getListAttaqueInventaire()[selectedSort].type;
            if (type == 1) // sort de jet
            {
                sortSelectionne = (SortDeJet)player.GetComponent<Player>().getListAttaqueInventaire()[selectedSort];
                structSortDeJetEnEdition = caracSorts.GetComponent<CaracProjectiles>().getStructFromName(sortSelectionne.getNameParticle());

                chooserPanelProj.SetActive(true);
                chooserPanelZone.SetActive(false);

                chooserProjPseudoSort.GetComponent<Text>().text = sortSelectionne.getPseudoSort();
                chooserProjElement.GetComponent<Text>().text = sortSelectionne.getElement().ToString();
                chooserProjProjectile.GetComponent<Text>().text = sortSelectionne.getNameInMenu();
                chooserProjVitesse.GetComponent<Text>().text = ( ((SortDeJet)sortSelectionne).vitesseProj + (structSortDeJetEnEdition.pointsInVitesse * structSortDeJetEnEdition.vitessePerLevel) ).ToString();
                chooserProjCoolDown.GetComponent<Text>().text = ( sortSelectionne.getCooldown() + (structSortDeJetEnEdition.pointsInCooldown * structSortDeJetEnEdition.coolDownPerLevel) ).ToString();
                chooserProjDegats.GetComponent<Text>().text = ( sortSelectionne.getDegats() + (structSortDeJetEnEdition.pointsInDegats * structSortDeJetEnEdition.degatsPerLevel) ).ToString();
            }
            else if (type == 2) // sort de zone
            {
                chooserPanelProj.SetActive(false);
                chooserPanelZone.SetActive(true);

                sortSelectionne = (SortDeZone)player.GetComponent<Player>().getListAttaqueInventaire()[selectedSort];
                chooserZonePseudoSort.GetComponent<Text>().text = sortSelectionne.getPseudoSort();
                chooserZoneElement.GetComponent<Text>().text = sortSelectionne.getElement().ToString();
                chooserZoneSort.GetComponent<Text>().text = sortSelectionne.getNameInMenu();
                chooserZoneCoolDown.GetComponent<Text>().text = sortSelectionne.getCooldown().ToString();
                chooserZoneDegats.GetComponent<Text>().text = sortSelectionne.getDegats().ToString();
            }
        }
        else if(selectedSort == -1)//Sort équipé
        {
            int type = player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace).type;
            if (type == 1) // sort de jet
            {
                sortSelectionne = (SortDeJet)player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace);

                chooserPanelProj.SetActive(true);
                chooserPanelZone.SetActive(false);

                structSortDeJetEnEdition = caracSorts.GetComponent<CaracProjectiles>().getStructFromName(sortSelectionne.getNameParticle());

                chooserProjPseudoSort.GetComponent<Text>().text = sortSelectionne.getPseudoSort();
                chooserProjElement.GetComponent<Text>().text = sortSelectionne.getElement().ToString();
                chooserProjProjectile.GetComponent<Text>().text = sortSelectionne.getNameInMenu();
                chooserProjVitesse.GetComponent<Text>().text = (((SortDeJet)sortSelectionne).vitesseProj + (structSortDeJetEnEdition.pointsInVitesse * structSortDeJetEnEdition.vitessePerLevel)).ToString();
                chooserProjCoolDown.GetComponent<Text>().text = (sortSelectionne.getCooldown() + (structSortDeJetEnEdition.pointsInCooldown * structSortDeJetEnEdition.coolDownPerLevel)).ToString();
                chooserProjDegats.GetComponent<Text>().text = (sortSelectionne.getDegats() + (structSortDeJetEnEdition.pointsInDegats * structSortDeJetEnEdition.degatsPerLevel)).ToString();
            }
            else if (type == 2) // sort de zone
            {
                chooserPanelProj.SetActive(false);
                chooserPanelZone.SetActive(true);

                sortSelectionne = (SortDeZone)player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace);
                chooserZonePseudoSort.GetComponent<Text>().text = sortSelectionne.getPseudoSort();
                chooserZoneElement.GetComponent<Text>().text = sortSelectionne.getElement().ToString();
                chooserZoneSort.GetComponent<Text>().text = sortSelectionne.getNameInMenu();
                chooserZoneCoolDown.GetComponent<Text>().text = sortSelectionne.getCooldown().ToString();
                chooserZoneDegats.GetComponent<Text>().text = sortSelectionne.getDegats().ToString();
            }
        }
    }

    public void fillClassChooserTable()
    {
        GameObject button;
        for (int nbButton = 0; nbButton < player.GetComponent<Player>().getListAttaqueInventaire().Count; nbButton++)
        {
            button = Instantiate(buttonTemplate) as GameObject;
            button.transform.SetParent(chooserScrollListSortInv.transform.GetChild(0).GetChild(0));
            button.transform.localPosition = new Vector3(0, -1 * nbButton * button.GetComponent<RectTransform>().rect.height, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.transform.GetChild(0).GetComponent<Text>().text = player.GetComponent<Player>().getListAttaqueInventaire()[nbButton].getPseudoSort();

            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //Ne pas enlever : permet au bouton de marcher
            int nbButtonTemp = nbButton;
            button.GetComponent<Button>().onClick.AddListener(() => { this.buttonClicked(nbButtonTemp); });
            //Redimensionnement bouton
        }
        //On adapte la taille du content pour que les boutons prennent la bonne taille
        float hauteurFinal = 10 + 10 + player.GetComponent<Player>().getListAttaqueInventaire().Count * buttonHeight + 10 * (player.GetComponent<Player>().getListAttaqueInventaire().Count - 1);
        //chooserScrollListSortInv.GetComponent<RectTransform>().sizeDelta = new Vector2(chooserScrollListSortInv.GetComponent<RectTransform>().sizeDelta.x, hauteurFinal);
        chooserScrollListSortInv.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(chooserScrollListSortInv.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.x, hauteurFinal);
        //Ajout du bouton du sort équipé
        if (player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace) != null)
        {
            button = Instantiate(buttonTemplate) as GameObject;
            button.transform.SetParent(chooserScrollListSortEq.transform.GetChild(0).GetChild(0));
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.transform.GetChild(0).GetComponent<Text>().text = player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace).getPseudoSort();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            //-1 -> sort équipé
            button.GetComponent<Button>().onClick.AddListener(() => buttonClicked(-1));
        }
        //On adapte la taille du content pour que les boutons prennent la bonne taille
        float hauteurFinale = 10 + 10 + buttonHeight;
        chooserScrollListSortEq.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(chooserScrollListSortEq.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.x, hauteurFinale);
    }

    public void setEditorCanvas(bool debutEdition)
    {
        int type = sortSelectionne.type;
        if (type == 1) //Sort de jet
        {
            if(debutEdition)
                structSortDeJetEnEdition = caracSorts.GetComponent<CaracProjectiles>().getStructFromName(sortSelectionne.getNameParticle());

            editorPanelProj.SetActive(true);
            editorPanelZone.SetActive(false);

            //info
            editorProjNom.GetComponent<InputField>().text = sortSelectionne.getPseudoSort();
            editorProjProj.GetComponent<Text>().text = sortSelectionne.getNameInMenu();
            editorProjVitesse.GetComponent<Text>().text = ((SortDeJet)sortSelectionne).vitesseProj.ToString() + " + " + (structSortDeJetEnEdition.pointsInVitesse * structSortDeJetEnEdition.vitessePerLevel);
            editorProjCooldown.GetComponent<Text>().text = sortSelectionne.getCooldown().ToString() + " + " + (structSortDeJetEnEdition.pointsInCooldown * structSortDeJetEnEdition.coolDownPerLevel);
            editorProjDegats.GetComponent<Text>().text = sortSelectionne.getDegats().ToString() + " + " + (structSortDeJetEnEdition.pointsInDegats * structSortDeJetEnEdition.degatsPerLevel);

            editorProjElement.GetComponent<Dropdown>().options.Clear();
            int cpt = 0;
            foreach (EnumScript.Element elem in caracSorts.GetComponent<CaracProjectiles>().getElemFromProj(sortSelectionne.getNameParticle()))
            {
                editorProjElement.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
                if(elem == sortSelectionne.getElement())
                    editorProjElement.GetComponent<Dropdown>().value = cpt;
                cpt++;
            }
            editorProjElement.GetComponent<Dropdown>().captionText = editorProjElement.GetComponent<Dropdown>().captionText;

            //bonus
            editorProjBonusVitesse.GetComponent<Text>().text = structSortDeJetEnEdition.pointsInVitesse.ToString();
            editorProjBonusCooldown.GetComponent<Text>().text = structSortDeJetEnEdition.pointsInCooldown.ToString();
            editorProjBonusDegats.GetComponent<Text>().text = structSortDeJetEnEdition.pointsInDegats.ToString();

            //ProgressBar and Lvl
            editorProgressBar.GetComponent<Image>().fillAmount = structSortDeJetEnEdition.xpActuel / caracSorts.GetComponent<CaracProjectiles>().xpToLvlUp * Mathf.Pow(caracSorts.GetComponent<CaracProjectiles>().multXpByLvl, sortDeJetEnConstruction.getLvl());
            editorLvl.GetComponent<Text>().text = structSortDeJetEnEdition.lvl.ToString();
        }
        else if (type == 2) //Sort de zone
        {
            if (debutEdition)
                structSortDeZoneEnEdition = caracSorts.GetComponent<CaracZones>().getStructFromName(sortSelectionne.getNameParticle());

            editorPanelProj.SetActive(false);
            editorPanelZone.SetActive(true);

            //info
            editorZoneNom.GetComponent<InputField>().text = sortSelectionne.getPseudoSort();
            editorZoneSort.GetComponent<Text>().text = sortSelectionne.getNameInMenu();
            editorZoneCooldown.GetComponent<Text>().text = sortSelectionne.getCooldown().ToString() + " + " + (structSortDeZoneEnEdition.pointsInCooldown * structSortDeZoneEnEdition.coolDownPerLevel);
            editorZoneDegats.GetComponent<Text>().text = sortSelectionne.getDegats().ToString() + " + " + (structSortDeZoneEnEdition.pointsInDegats * structSortDeZoneEnEdition.degatsPerLevel);

            editorZoneElement.GetComponent<Dropdown>().options.Clear();
            int cpt = 0;
            foreach (EnumScript.Element elem in caracSorts.GetComponent<CaracZones>().getElemFromZone(sortSelectionne.getNameParticle()))
            {
                editorZoneElement.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(elem.ToString()));
                if (elem == sortSelectionne.getElement())
                    editorZoneElement.GetComponent<Dropdown>().value = cpt;
                cpt++;
            }
            editorZoneElement.GetComponent<Dropdown>().captionText = editorZoneElement.GetComponent<Dropdown>().captionText;

            //bonus
            editorZoneBonusCooldown.GetComponent<Text>().text = structSortDeZoneEnEdition.pointsInCooldown.ToString();
            editorZoneBonusDegats.GetComponent<Text>().text = structSortDeZoneEnEdition.pointsInDegats.ToString();

            //ProgressBar and Lvl
            editorProgressBar.GetComponent<Image>().fillAmount = structSortDeZoneEnEdition.xpActuel / caracSorts.GetComponent<CaracZones>().xpToLvlUp * Mathf.Pow(caracSorts.GetComponent<CaracZones>().multXpByLvl, sortDeZoneEnConstruction.getLvl());
            editorLvl.GetComponent<Text>().text = structSortDeZoneEnEdition.lvl.ToString();
        }
    }

    public void editerSort()
    {
        int type = sortSelectionne.type;
        //Pour l'instant, on en peut changer que l'élément
        if (type == 1) //Sort de jet
        {
            //Element
            EnumScript.Element newElem = caracSorts.GetComponent<CaracProjectiles>().getElemFromProj(sortSelectionne.getNameParticle())[editorProjElement.GetComponent<Dropdown>().value];
            sortSelectionne.setElement(newElem);
            //Nom
            String newName = editorProjNom.GetComponent<InputField>().text;
            sortSelectionne.setPseudoSort(newName);

            caracSorts.GetComponent<CaracProjectiles>().setStruct(structSortDeJetEnEdition);
        }
        else if(type == 2)
        {
            //Element
            EnumScript.Element newElem = caracSorts.GetComponent<CaracZones>().getElemFromZone(sortSelectionne.getNameParticle())[editorZoneElement.GetComponent<Dropdown>().value];
            sortSelectionne.setElement(newElem);
            //Nom
            String newName = editorZoneNom.GetComponent<InputField>().text;
            sortSelectionne.setPseudoSort(newName);
        }
        majCaracSortClicked();
        resetClassChooser();
        fillClassChooserTable();
    }

    public void editerProjVitesse(int point)
    {
        if (point == 1)
        {
            if(structSortDeJetEnEdition.nbPointsDispo > 0)
            {
                structSortDeJetEnEdition.pointsInVitesse++;
                structSortDeJetEnEdition.nbPointsDispo--;
            }
        }
        else if(point == -1)
        {
            if(structSortDeJetEnEdition.pointsInVitesse > 0)
            {
                structSortDeJetEnEdition.pointsInVitesse--;
                structSortDeJetEnEdition.nbPointsDispo++;
            }
        }
        setEditorCanvas(false);
    }
    public void editerProjCooldown(int point)
    {
        if (point == 1)
        {
            if (structSortDeJetEnEdition.nbPointsDispo > 0)
            {
                structSortDeJetEnEdition.pointsInCooldown++;
                structSortDeJetEnEdition.nbPointsDispo--;
            }
        }
        else if (point == -1)
        {
            if (structSortDeJetEnEdition.pointsInCooldown > 0)
            {
                structSortDeJetEnEdition.pointsInCooldown--;
                structSortDeJetEnEdition.nbPointsDispo++;
            }
        }
        setEditorCanvas(false);
    }
    public void editerProjDegats(int point)
    {
        if (point == 1)
        {
            if (structSortDeJetEnEdition.nbPointsDispo > 0)
            {
                structSortDeJetEnEdition.pointsInDegats++;
                structSortDeJetEnEdition.nbPointsDispo--;
            }
        }
        else if (point == -1)
        {
            if (structSortDeJetEnEdition.pointsInDegats > 0)
            {
                structSortDeJetEnEdition.pointsInDegats--;
                structSortDeJetEnEdition.nbPointsDispo++;
            }
        }
        setEditorCanvas(false);
    }

    public void editerZoneDegats(int point)
    {
        if (point == 1)
        {
            if (structSortDeZoneEnEdition.nbPointsDispo > 0)
            {
                structSortDeZoneEnEdition.pointsInDegats++;
                structSortDeZoneEnEdition.nbPointsDispo--;
            }
        }
        else if (point == -1)
        {
            if (structSortDeZoneEnEdition.pointsInDegats > 0)
            {
                structSortDeZoneEnEdition.pointsInDegats--;
                structSortDeZoneEnEdition.nbPointsDispo++;
            }
        }
        setEditorCanvas(false);
    }
    public void editerZoneCooldown(int point)
    {
        if (point == 1)
        {
            if (structSortDeZoneEnEdition.nbPointsDispo > 0)
            {
                structSortDeZoneEnEdition.pointsInCooldown++;
                structSortDeZoneEnEdition.nbPointsDispo--;
            }
        }
        else if (point == -1)
        {
            if (structSortDeZoneEnEdition.pointsInCooldown > 0)
            {
                structSortDeZoneEnEdition.pointsInCooldown--;
                structSortDeZoneEnEdition.nbPointsDispo++;
            }
        }
        setEditorCanvas(false);
    }

    public void exitMenu()
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().sauvegarderSorts();
        accessMenu.GetComponent<accessMenu>().exitMenu();
    }

    public void setPopUpYesNoResult(bool p_result)
    {
        popUpYesNoResult = p_result;
        popUpYesNoAnswered = true;
        fctToCallWhenPopUpYesNoAnswer();
    }
}
