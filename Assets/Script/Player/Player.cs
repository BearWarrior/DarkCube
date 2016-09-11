using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Character
{
    public List<GameObject> feet;
    public GameObject sphere;
    public GameObject armature;
    public GameObject masque;

    private Shader oldShaderSphere;
    private List<Shader> oldShaderFoot;
    private Shader oldShaderArmature;

    private bool isTransparent = false;
    private float alpha = 0.15f;

    int cubeFace;


    // Use this for initialization
    void Start()
    {
        oldShaderFoot = new List<Shader>();

        listAttaque = new Attaque[6];
        for (int i = 0; i < 6; i++)
            listAttaque[i] = null;

        listAttaqueInventaire = new List<Attaque>();

        //listAttaqueInventaire.Add(new SortDeZone());

        listAttaqueInventaire.Add(new SortDeZone(4, 80, 1, 1, 1, EnumScript.Element.Aucun, EnumScript.GabaritSortDeZone.Cercle, "ppp", "ppp", "none"));
        listAttaqueInventaire.Add(new SortDeZone(4, 80, 1, 1, 1, EnumScript.Element.Aucun, EnumScript.GabaritSortDeZone.Cercle, "ppp", "ppp", "none"));
        listAttaqueInventaire.Add(new SortDeZone(4, 80, 1, 1, 1, EnumScript.Element.Aucun, EnumScript.GabaritSortDeZone.Cercle, "ppp", "ppp", "none"));
        listAttaqueInventaire.Add(new SortDeZone(4, 80, 1, 1, 1, EnumScript.Element.Aucun, EnumScript.GabaritSortDeZone.Ligne, "ppp", "ppp", "fire_ground/BrasierLineS"));
        listAttaqueInventaire.Add(new SortDeZone(4, 80, 1, 1, 1, EnumScript.Element.Aucun, EnumScript.GabaritSortDeZone.Ligne, "ppp", "ppp", "none"));
        listAttaqueInventaire.Add(new SortDeZone(4, 80, 1, 1, 1, EnumScript.Element.Aucun, EnumScript.GabaritSortDeZone.Ligne, "ppp", "ppp", "none"));
        listAttaqueInventaire.Add(new SortDeZone(4, 80, 1, 1, 1, EnumScript.Element.Aucun, EnumScript.GabaritSortDeZone.Cone, "ppp", "ppp", "none"));
        listAttaqueInventaire.Add(new SortDeZone(4, 80, 1, 1, 1, EnumScript.Element.Aucun, EnumScript.GabaritSortDeZone.Cone, "ppp", "ppp", "none"));
        listAttaqueInventaire.Add(new SortDeZone(4, 80, 1, 1, 1, EnumScript.Element.Aucun, EnumScript.GabaritSortDeZone.Cone, "ppp", "ppp", "none"));


        listAttaqueInventaire.Add(new SortDeJet(1, 45, 0.05f, 10, EnumScript.Element.Metal, "Canon", "canon", EnumScript.PatternSortDeJet.Rafale));
        listAttaqueInventaire.Add(new SortDeJet(1, 60, 2, 10, EnumScript.Element.Eau, "Prisme", "Prisme", EnumScript.PatternSortDeJet.SimultaneLigne));
        listAttaqueInventaire.Add(new SortDeJet(3, 70, 2, 10, EnumScript.Element.Eau, "Shuriken", "ShuShu", EnumScript.PatternSortDeJet.SimultaneTriangle));
        listAttaqueInventaire.Add(new SortDeJet(4, 45, 2, 10, EnumScript.Element.Metal, "Canon", "4ligne", EnumScript.PatternSortDeJet.SimultaneLigne));
        listAttaqueInventaire.Add(new SortDeJet(4, 45, 2, 10, EnumScript.Element.Eau, "Prisme", "Prisme-rafale", EnumScript.PatternSortDeJet.Rafale));

        equipeAttaqueAt(1, 0);
        equipeAttaqueAt(2, 1);
        equipeAttaqueAt(3, 2);
        equipeAttaqueAt(4, 3);
        equipeAttaqueAt(5, 4);
        equipeAttaqueAt(6, 5);

        PDVmax = 10;
        PDVactuel = 10;
        armureMax = 10;
        armureActuel = 10;
        enduranceMax = 3;   
        enduranceActuel = 3;

        regenEndurance = false;
        timeBeforeRunningMax = 3;
        timeBeforeRunningAct = 0;

        cubeFace = 1;
        if (listAttaque[cubeFace - 1] != null)
        {
            if (listAttaque[cubeFace - 1].type == 2) //Sort de Zone
                GetComponent<PlayerController>().setGabarit(true, (SortDeZone)listAttaque[cubeFace - 1]);
            else if (listAttaque[cubeFace - 1].type == 1) // Sort de jet
                GetComponent<PlayerController>().setGabarit(false, null);
        }
    }


    // Update is called once per frame
    void Update()
    {
        //ENDURANCE
        if (!GetComponent<PlayerController>().isRunning())
            timeBeforeRunningAct += Time.deltaTime;
        else
            timeBeforeRunningAct = 0;
        if (timeBeforeRunningAct > timeBeforeRunningMax)
            regenEndurance = true;
        if (regenEndurance)
        {
            enduranceActuel += Time.deltaTime;
            if (enduranceActuel > enduranceMax)
                enduranceActuel = enduranceMax;
            regenEndurance = false;
        }

        //Clic gauche 
        if (GetComponent<PlayerController>().getControllable())
            if (Input.GetMouseButtonDown(0) && GetComponent<PlayerController>().isAiming())
                if (listAttaque[cubeFace - 1] != null)
                    listAttaque[cubeFace - 1].Attaquer();
                
        //Transparency
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 2 && !isTransparent)
            setTransparecy(true);
        if (Vector3.Distance(transform.position, Camera.main.transform.position) > 2 && isTransparent)
            setTransparecy(false);

        //cooldowns
        for (int i = 0; i < 6; i++)
            if (listAttaque[i] != null)
                listAttaque[i].reload();

        //Get back to menu
        //if (Input.GetKey(KeyCode.F12))
        //    Application.LoadLevel("Menu");
    }

    public void cubeFaceChanged(int face)
    {
        cubeFace = face;
        if (GetComponent<PlayerController>().getControllable())
        {
            if (listAttaque[cubeFace - 1] != null)
            {
                if (listAttaque[cubeFace - 1].type == 2) //Sort de Zone
                    GetComponent<PlayerController>().setGabarit(true, (SortDeZone)listAttaque[cubeFace -1]);
                else if (listAttaque[cubeFace - 1].type == 1) // Sort de jet
                    GetComponent<PlayerController>().setGabarit(false, null);
            }
        }
    }

 

 

    public void setTransparecy(bool set)
    {
        if (set)
        {
            if (!isTransparent)
            {
                oldShaderFoot.Clear();
                foreach (GameObject foot in feet)
                oldShaderFoot.Add(foot.GetComponent<Renderer>().material.shader);
                oldShaderSphere = armature.GetComponent<Renderer>().material.shader;
                oldShaderArmature = armature.GetComponent<Renderer>().material.shader;

                foreach(GameObject foot in feet)
                {
                    foot.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
                    foot.GetComponent<Renderer>().material.color = new Color(foot.GetComponent<Renderer>().material.color.r, foot.GetComponent<Renderer>().material.color.g, foot.GetComponent<Renderer>().material.color.b, alpha);
                }
                armature.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
                armature.GetComponent<Renderer>().material.color = new Color(armature.GetComponent<Renderer>().material.color.r, armature.GetComponent<Renderer>().material.color.g, armature.GetComponent<Renderer>().material.color.b, alpha);
                sphere.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
                sphere.GetComponent<Renderer>().material.color = new Color(sphere.GetComponent<Renderer>().material.color.r, sphere.GetComponent<Renderer>().material.color.g, sphere.GetComponent<Renderer>().material.color.b, alpha);

                masque.GetComponent<Renderer>().material.color = new Color(masque.GetComponent<Renderer>().material.color.r, masque.GetComponent<Renderer>().material.color.g, masque.GetComponent<Renderer>().material.color.b, 0.1f);

                isTransparent = true;
            }
        }
        else
        {
            armature.GetComponent<Renderer>().material.shader = oldShaderArmature;
            for(int i = 0; i < 4; i++)
                feet[i].GetComponent<Renderer>().material.shader = oldShaderFoot[i];
            sphere.GetComponent<Renderer>().material.shader = oldShaderSphere;
            isTransparent = false;

            masque.GetComponent<Renderer>().material.color = new Color(masque.GetComponent<Renderer>().material.color.r, masque.GetComponent<Renderer>().material.color.g, masque.GetComponent<Renderer>().material.color.b, 1f);
        }
       
    }


    public int getCubeFace()
    {
        return cubeFace;
    }


    

    




    //TODO vérifier tout ça
    

   

   

}