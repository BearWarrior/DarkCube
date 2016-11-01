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
    public GameObject editorProjPseudoSort;
    public GameObject editorProjElement;
    public GameObject editorProjProjectile;
    public GameObject editorProjNbProj;
    public GameObject editorProjPattern;
    public GameObject editorProjVitesse;
    public GameObject editorProjCoolDown;
    public GameObject editorProjDegats;
    [Space(15)]
    public GameObject editorZonePseudoSort;
    public GameObject editorZoneElement;
    public GameObject editorZoneGabarit;
    public GameObject editorZoneSort;
    public GameObject editorZoneCoolDown;
    public GameObject editorZoneDegats;

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
        selectedSort = -100;

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
            editorProjProjectile.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(proj.nameInMenu));
        editorProjProjectile.GetComponent<Dropdown>().value = 0;
        editorProjProjectile.GetComponent<Dropdown>().captionText = editorProjProjectile.GetComponent<Dropdown>().captionText;
        editorElementChanged();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpingCanvas) lerpCanvas();
    }

    public void Init(GameObject p)
    {
        player = p;
        fillClassChooserTable();
        majCaracSortClicked();
        setSelectedFace(selectedFace);
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

                //On passe le editor en top
                classEditor.GetComponent<CanvasGroup>().interactable = false;
                classEditor.GetComponent<CanvasGroup>().alpha = .5f;
                classEditor.GetComponent<RectTransform>().localScale = new Vector3(0.027f, 0.027f, 0.027f);

                lerpingCanvas = true;
            }
            else if (can == 2) //On veut l'editor
            {
                //On passe le chooser en bottom
                classChooser.GetComponent<CanvasGroup>().interactable = false;
                classChooser.GetComponent<CanvasGroup>().alpha = 0.5f;
                classChooser.GetComponent<RectTransform>().localScale = new Vector3(0.027f, 0.027f, 0.027f);

                //On passe le editor en actif
                classEditor.GetComponent<CanvasGroup>().interactable = true;
                classEditor.GetComponent<CanvasGroup>().alpha = 1;
                classEditor.GetComponent<RectTransform>().localScale = new Vector3(0.03f, 0.03f, 0.03f);

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
            editorProjProjectile.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(proj.nameInMenu));
        editorProjProjectile.GetComponent<Dropdown>().value = 0;
        editorProjProjectile.GetComponent<Dropdown>().captionText = editorProjProjectile.GetComponent<Dropdown>().captionText;
        //changement de l'élément et du projectile
        sortDeJetEnConstruction.setElement((EnumScript.Element)editorProjElement.GetComponent<Dropdown>().value);
        string nomProj = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorProjElement.GetComponent<Dropdown>().value)[editorProjProjectile.GetComponent<Dropdown>().value].nomParticle;
        sortDeJetEnConstruction.setNameParticle(nomProj);

        //ZONE
        //On récupère la liste des projectiles de cet element puis affichage dans la dropview
        List<structSortDeZone> listSortZ = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value);
        editorZoneSort.GetComponent<Dropdown>().options.Clear();
        foreach (structSortDeZone proj in listSortZ)
            editorZoneSort.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(proj.nameInMenu));
        editorZoneSort.GetComponent<Dropdown>().captionText = editorZoneSort.GetComponent<Dropdown>().captionText;
        //changement de l'élément et du projectile
        sortDeZoneEnConstruction.setElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value);
        string nomParticle = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value)[editorZoneSort.GetComponent<Dropdown>().value].nomParticle;
        sortDeZoneEnConstruction.setPseudoSort(nomParticle);
        editorZoneChanged();

        majCaracSortEnConstr();
    }

    public void editorProjectileChanged()
    {
        //On récupere le nom du prjectile et on l'affecte au sort
        string namePart = GetComponent<CaracProjectiles>().getProjsFromElement((EnumScript.Element)editorProjElement.GetComponent<Dropdown>().value)[editorProjProjectile.GetComponent<Dropdown>().value].nomParticle;
        sortDeJetEnConstruction.setNameParticle(namePart);

        //Mise a jour des carac + affichage
        majCaracSortEnConstr();
    }

    public void editorZoneChanged()
    {
        string nameParticle = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value)[editorZoneSort.GetComponent<Dropdown>().value].nomParticle;

        sortDeZoneEnConstruction.setNameParticle(nameParticle);

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
                    player.GetComponent<Player>().supprimerAttaqueInventaireAt(selectedFace-1, true);
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
            if (editorProjPseudoSort.GetComponent<InputField>().text != "")
                sortDeJetEnConstruction.setPseudoSort(editorProjPseudoSort.GetComponent<InputField>().text);
            else
                sortDeJetEnConstruction.setPseudoSort("_defaut_");
            
            player.GetComponent<Player>().addAttaqueToInventaire(sortDeJetEnConstruction);
        }
        else if (selectedType == 2) //Zone
        {
            if (editorZonePseudoSort.GetComponent<InputField>().text != "")
                sortDeZoneEnConstruction.setPseudoSort(editorZonePseudoSort.GetComponent<InputField>().text);
            else
                sortDeZoneEnConstruction.setPseudoSort("_defaut_");

            string nomParticle = GetComponent<CaracZones>().getZoneFromElement((EnumScript.Element)editorZoneElement.GetComponent<Dropdown>().value)[editorZoneSort.GetComponent<Dropdown>().value].nomParticle;
            sortDeZoneEnConstruction.setNameParticle(nomParticle);

            
            Debug.Log(sortDeZoneEnConstruction.getPseudoSort());


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

    public void resetEditorWindow()
    {
        editorProjPseudoSort.GetComponent<InputField>().text = "";
        editorProjElement.GetComponent<Dropdown>().value = 0; editorProjElement.GetComponent<Dropdown>().captionText = editorProjElement.GetComponent<Dropdown>().captionText;
        editorProjProjectile.GetComponent<Dropdown>().value = 0; editorProjProjectile.GetComponent<Dropdown>().captionText = editorProjProjectile.GetComponent<Dropdown>().captionText;
        editorProjNbProj.GetComponent<Dropdown>().value = 0; editorProjNbProj.GetComponent<Dropdown>().captionText = editorProjNbProj.GetComponent<Dropdown>().captionText;
        editorProjPattern.GetComponent<Dropdown>().value = 0; editorProjPattern.GetComponent<Dropdown>().captionText = editorProjPattern.GetComponent<Dropdown>().captionText;

        editorZonePseudoSort.GetComponent<InputField>().text = "";
        editorZoneElement.GetComponent<Dropdown>().value = 0; editorZoneElement.GetComponent<Dropdown>().captionText = editorZoneElement.GetComponent<Dropdown>().captionText;
        editorZoneSort.GetComponent<Dropdown>().value = 0; editorZoneSort.GetComponent<Dropdown>().captionText = editorZoneSort.GetComponent<Dropdown>().captionText;

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
        structSortJet structureDuSortJ = GetComponent<CaracProjectiles>().getStructFromNameAndElement(sortDeJetEnConstruction.getNameParticle(), sortDeJetEnConstruction.getElement());
        //On modifie le sort en conséquent
        sortDeJetEnConstruction.vitesseProj = structureDuSortJ.vitesse;
        sortDeJetEnConstruction.setCooldown(structureDuSortJ.cooldown);
        sortDeJetEnConstruction.setNameInMenu(structureDuSortJ.nameInMenu);
        sortDeJetEnConstruction.setDegats(structureDuSortJ.degats);
        //On affiche les nouvelles caractéristiques
        editorProjVitesse.GetComponent<Text>().text = sortDeJetEnConstruction.vitesseProj.ToString();
        editorProjCoolDown.GetComponent<Text>().text = sortDeJetEnConstruction.getCooldown().ToString();
        editorProjDegats.GetComponent<Text>().text = sortDeJetEnConstruction.getDegats().ToString();

        //ZONE
        structSortDeZone structureDuSortZ = GetComponent<CaracZones>().getStructFromNameAndElement(sortDeZoneEnConstruction.getNameParticle(), sortDeZoneEnConstruction.getElement());
        //On modifie le sort en conséquent
        sortDeZoneEnConstruction.setCooldown(structureDuSortZ.cooldown);
        sortDeZoneEnConstruction.setDegats(structureDuSortZ.degats);
        sortDeZoneEnConstruction.setNameInMenu(structureDuSortZ.nameInMenu);
        //On affiche les nouvelles caractéristiques
        editorZoneCoolDown.GetComponent<Text>().text = sortDeZoneEnConstruction.getCooldown().ToString();
        editorZoneDegats.GetComponent<Text>().text = sortDeZoneEnConstruction.getDegats().ToString();
    }


    //TODO changer le panel d'affchage (zone -> jet   jet -> zone)
    public void majCaracSortClicked()
    {
        if (selectedSort >= 0) //Sort de l'inventaire
        {
            int type = player.GetComponent<Player>().getListAttaqueInventaire()[selectedSort].type;
            if (type == 1) // sort de jet
            {
                chooserPanelProj.SetActive(true);
                chooserPanelZone.SetActive(false);

                SortDeJet sortChoisi = (SortDeJet)player.GetComponent<Player>().getListAttaqueInventaire()[selectedSort];
                chooserProjPseudoSort.GetComponent<Text>().text = sortChoisi.getPseudoSort();
                chooserProjElement.GetComponent<Text>().text = sortChoisi.getElement().ToString();
                chooserProjProjectile.GetComponent<Text>().text = sortChoisi.getNameInMenu();
                chooserProjVitesse.GetComponent<Text>().text = sortChoisi.vitesseProj.ToString();
                chooserProjCoolDown.GetComponent<Text>().text = sortChoisi.getCooldown().ToString();
                chooserProjDegats.GetComponent<Text>().text = sortChoisi.getDegats().ToString();
            }
            else if (type == 2) // sort de zone
            {
                chooserPanelProj.SetActive(false);
                chooserPanelZone.SetActive(true);

                SortDeZone sortChoisi = (SortDeZone)player.GetComponent<Player>().getListAttaqueInventaire()[selectedSort];
                chooserZonePseudoSort.GetComponent<Text>().text = sortChoisi.getPseudoSort();
                chooserZoneElement.GetComponent<Text>().text = sortChoisi.getElement().ToString();
                chooserZoneSort.GetComponent<Text>().text = sortChoisi.getNameInMenu();
                chooserZoneCoolDown.GetComponent<Text>().text = sortChoisi.getCooldown().ToString();
                chooserZoneDegats.GetComponent<Text>().text = sortChoisi.getDegats().ToString();
            }
        }
        else if(selectedSort == -1)//Sort équipé
        {
            int type = player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace).type;
            if (type == 1) // sort de jet
            {
                chooserPanelProj.SetActive(true);
                chooserPanelZone.SetActive(false);

                SortDeJet sortChoisi = (SortDeJet)player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace);
                chooserProjPseudoSort.GetComponent<Text>().text = sortChoisi.getPseudoSort();
                chooserProjElement.GetComponent<Text>().text = sortChoisi.getElement().ToString();
                chooserProjProjectile.GetComponent<Text>().text = sortChoisi.getNameInMenu();
                chooserProjVitesse.GetComponent<Text>().text = sortChoisi.vitesseProj.ToString();
                chooserProjCoolDown.GetComponent<Text>().text = sortChoisi.getCooldown().ToString();
                chooserProjDegats.GetComponent<Text>().text = sortChoisi.getDegats().ToString();
            }
            else if (type == 2) // sort de zone
            {
                chooserPanelProj.SetActive(false);
                chooserPanelZone.SetActive(true);

                SortDeZone sortChoisi = (SortDeZone)player.GetComponent<Player>().getAttaqueEquipOnFace(selectedFace);
                chooserZonePseudoSort.GetComponent<Text>().text = sortChoisi.getPseudoSort();
                chooserZoneElement.GetComponent<Text>().text = sortChoisi.getElement().ToString();
                chooserZoneSort.GetComponent<Text>().text = sortChoisi.getNameInMenu();
                chooserZoneCoolDown.GetComponent<Text>().text = sortChoisi.getCooldown().ToString();
                chooserZoneDegats.GetComponent<Text>().text = sortChoisi.getDegats().ToString();
            }
        }
        else
        {

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
