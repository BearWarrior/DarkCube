using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Character
{
    public GameObject sphere;
    public GameObject armature;

    private Shader oldShaderSphere;
    private Shader oldShaderArmature;

    private bool isTransparent = false;
    private float alpha = 0.15f;

    private int cubeFace;

    // Use this for initialization
    void Start()
    {
        listAttaque = new Attaque[6];
        for (int i = 0; i < 6; i++)
            listAttaque[i] = null;

        listAttaqueInventaire = new List<Attaque>();

        listAttaqueInventaire.Add(new SortDeJet(10, 0.5f, 10, EnumScript.Element.Elec, "LightningBall", "perso1"));
        listAttaqueInventaire.Add(new SortDeJet(10, 0.05f, 10, EnumScript.Element.Elec, "LightningBall", "perso1"));
        listAttaqueInventaire.Add(new SortDeJet(1, 0.05f, 10, EnumScript.Element.Elec, "LightningBall", "perso1"));
        listAttaqueInventaire.Add(new SortDeJet(60, 2, 10, EnumScript.Element.Eau, "Prisme", "Prisme"));
        listAttaqueInventaire.Add(new SortDeJet(70, 2, 10, EnumScript.Element.Eau, "Shuriken", "ShuShu"));
        listAttaqueInventaire.Add(new SortDeJet(45, 2, 10, EnumScript.Element.Aucun, "Canon", "4ligne"));
        listAttaqueInventaire.Add(new SortDeJet(45, 2, 10, EnumScript.Element.Eau, "Prisme", "Prisme-rafale"));
        listAttaqueInventaire.Add(new SortDeJet(45, 2, 10, EnumScript.Element.Eau, "Prisme", "Prisme-rafale"));

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
        enduranceMax = 0.25f;
        enduranceActuel = 0.25f;

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
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 1.5f && !isTransparent)
            setTransparecy(true);
        else if (Vector3.Distance(transform.position, Camera.main.transform.position) > 1.5f && isTransparent)
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
                    GetComponent<PlayerController>().setGabarit(true, (SortDeZone)listAttaque[cubeFace - 1]);
                else if (listAttaque[cubeFace - 1].type == 1) // Sort de jet
                    GetComponent<PlayerController>().setGabarit(false, null);
            }
        }
    }

    //if the player is too close of the camera
    public void setTransparecy(bool set)
    {
        if (set)
        {
            oldShaderSphere = armature.transform.GetChild(0).GetComponent<Renderer>().material.shader;
            oldShaderArmature = armature.transform.GetChild(0).GetComponent<Renderer>().material.shader;

            for(int i = 0; i < armature.transform.childCount-1; i++) //-1 because of the core
            {
                armature.transform.GetChild(i).GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
                armature.transform.GetChild(i).GetComponent<Renderer>().material.color = new Color(armature.transform.GetChild(i).GetComponent<Renderer>().material.color.r, armature.transform.GetChild(i).GetComponent<Renderer>().material.color.g, armature.transform.GetChild(i).GetComponent<Renderer>().material.color.b, alpha);
            }
            
            //sphere.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            //sphere.GetComponent<Renderer>().material.color = new Color(sphere.GetComponent<Renderer>().material.color.r, sphere.GetComponent<Renderer>().material.color.g, sphere.GetComponent<Renderer>().material.color.b, alpha);

            isTransparent = true;
        }
        else
        {
            for (int i = 0; i < armature.transform.childCount -1 ; i++)//-1 because of the core
                armature.transform.GetChild(i).GetComponent<Renderer>().material.shader = oldShaderArmature;
            //sphere.GetComponent<Renderer>().material.shader = oldShaderSphere;

            isTransparent = false;
        }
    }

    public int getCubeFace()
    {
        return cubeFace;
    }
}