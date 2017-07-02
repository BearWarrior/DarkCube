using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : Character, IInputsObservable, ITakeDamages
{
    public GameObject sphere;
    public GameObject armature;

    private Shader oldShaderArmature;

    private bool isTransparent = false;
    private float alpha = 0.15f;

    private int cubeFace = 1;

    public GameObject spawnProjectile;
    [Space(15)]
    public Image lifeBar;
    public Text lifeDisplay;

    private bool dead = false;

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public List<GameObject> listLogosSorts;
    public List<Image> listCooldownDisplay;

    private Texture textureCubes;
    private string textureCubesStr;

    void Awake()
    {
         keys = GameObject.FindWithTag("InputsLoader").GetComponent<InputsLoader>().lookAtInputs(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        //Sorts équipé et sort inventaire
        listAttaque = new Attaque[6];
        for (int i = 0; i < 6; i++)
            listAttaque[i] = null;
        listAttaqueInventaire = new List<Attaque>();
        LoadSorts();

        PDVmax = 100;
        PDVactuel = PDVmax;
        armureMax = 10;
        armureActuel = armureMax;

        lifeBar.fillAmount =1;
        lifeDisplay.text = PDVmax.ToString();

        LoadSkin();
        GetComponent<PlayerCubeFlock>().changeSkin(textureCubes);
    }


    // Update is called once per frame
    void Update()
    {
        //Clic gauche 
        if (GetComponent<PlayerController>().getControllable())
            if (Input.GetKey(keys["Shoot"]) && GetComponent<PlayerController>().isAiming() && GetComponent<SortChooser>().playerCanShoot())
                if (listAttaque[cubeFace - 1] != null)
                    listAttaque[cubeFace - 1].AttackFromPlayer(spawnProjectile.transform.position);

        //Transparency
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 1.5f && !isTransparent)
            setTransparecy(true);
        else if (Vector3.Distance(transform.position, Camera.main.transform.position) > 1.5f && isTransparent)
            setTransparecy(false);

        float[] cooldownRestant = new float[6];
        //cooldowns
        for (int i = 0; i < 6; i++)
            if (listAttaque[i] != null)
                cooldownRestant[i] = listAttaque[i].reload();

        //Display cooldown
        for (int i = 0; i < 6; i++)
            if (listAttaque[i] != null)
                listCooldownDisplay[i].fillAmount = (listAttaque[i].getCooldown() - cooldownRestant[i])/(listAttaque[i].getCooldown());

        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveSorts();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void keysChanged(Dictionary<string, KeyCode> keys)
    {
        this.keys = keys;
    }

    public void cubeFaceChanged(int face)
    {
        cubeFace = face;
    }

    //if the player is too close from the camera
    public void setTransparecy(bool set)
    {
        if (set)
        {
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

    public override void takeDamages(float degats)
    {
        PDVactuel -= degats;
        //shakiness
        GetComponent<PlayerCubeFlock>().setShakiness(PDVactuel, PDVmax);
        //lifebar
        lifeBar.fillAmount = PDVactuel / PDVmax;
        lifeDisplay.text = PDVactuel.ToString();
        //dead ?
        if (PDVactuel <= 0)
            EndLvl(true);
    }

    public void Save()
    {
        SaveSorts();
        SaveSkin();
        if (GameObject.Find("Saving") != null)
            GameObject.Find("Saving").GetComponent<SavingLogo>().DisplayLogo();
    }

    public void Load()
    {
        LoadSorts();
        LoadSkin();
    }

    //   SAUVEGARDE / CHARGEMENT
    /* CE QU'IL FAUT SAUVER
    * type (1- JET 2- ZONE 3-SUPPORT)
    *   JET
    * PseudoSort
    * NomProjectile
    * Element
    * Custom1
    * Custom2
    *
    * xp et lvl stocké dans CaracProj et CaracZone
    */
    private void SaveSorts()
    {
        //Attaque equipé
        for (int i = 0; i < 6; i++)
        {
            string save = "";
            if (listAttaque[i] != null)
            {
                save = listAttaque[i].type + ";" + listAttaque[i].getPseudoSort() + ";" + listAttaque[i].getNameParticle() + ";" + listAttaque[i].getElement().ToString() + ";" +
                    ((SortDeJet)listAttaque[i]).pointsInVitesse + ";" + ((SortDeJet)listAttaque[i]).pointsInCooldown + ";" + ((SortDeJet)listAttaque[i]).pointsInDegats + ";" +
                    ((SortDeJet)listAttaque[i]).pointsInCustom1 + ";" + ((SortDeJet)listAttaque[i]).pointsInCustom2;
                switch (listAttaque[i].type)
                {
                    case 1:
                        save += ";" + ((SortDeJet)listAttaque[i]).getCustom1().ToString() + ";" + ((SortDeJet)listAttaque[i]).getCustom2().ToString();
                        break;
                }
            }
            else
            {
                save = "null";
            }
            PlayerPrefs.SetString("attaqueEquipe" + i.ToString(), save);
        }

        //Attaque inventaire
        PlayerPrefs.SetInt("attaqueInventaireCount", listAttaqueInventaire.Count);
        for (int i = 0; i < listAttaqueInventaire.Count; i++)
        {
            string save = listAttaqueInventaire[i].type + ";" + listAttaqueInventaire[i].getPseudoSort() + ";" + listAttaqueInventaire[i].getNameParticle() + ";" + listAttaqueInventaire[i].getElement().ToString() + ";" +
                   ((SortDeJet)listAttaqueInventaire[i]).pointsInVitesse + ";" + ((SortDeJet)listAttaqueInventaire[i]).pointsInCooldown + ";" + ((SortDeJet)listAttaqueInventaire[i]).pointsInDegats + ";" +
                   ((SortDeJet)listAttaqueInventaire[i]).pointsInCustom1 + ";" + ((SortDeJet)listAttaqueInventaire[i]).pointsInCustom2;
            switch (listAttaqueInventaire[i].type)
            {
                case 1:
                    save += ";" + ((SortDeJet) listAttaqueInventaire[i]).getCustom1().ToString() + ";" + ((SortDeJet)listAttaqueInventaire[i]).getCustom2().ToString();
                    break;
            }
            PlayerPrefs.SetString("attaqueInventaire" + i.ToString(), save);
        }
    }

    private void LoadSorts()
    {
        //Attaque equipé
        for (int i = 0; i < 6; i++)
        {
            string save = PlayerPrefs.GetString("attaqueEquipe" + i.ToString(), "default");
            if (save != "default")
            {
                if (save != "null")
                {
                    string[] array = save.Split(';');
                    switch (array[0])
                    {
                        case "1":
                            SortDeJet sortJ = new SortDeJet(array[1],
                                array[2],
                                EnumScript.getElemFromStr(array[3]),
                                GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().getLvlFromNamePart(array[2]),
                                Int32.Parse(array[4]),
                                Int32.Parse(array[5]),
                                Int32.Parse(array[6]),
                                Int32.Parse(array[7]),
                                Int32.Parse(array[8]),
                                EnumScript.getCustom1FromString(array[9]),
                                EnumScript.getCustom2FromString(array[10])
                                );
                            listAttaqueInventaire.Add(sortJ);
                            equipeAttaqueAt(i+1, 0);
                            break;
                        case "2":
                            //TODO ajouter ici idem que SortDeJet
                            SortDeZone sortZ = new SortDeZone(array[1], array[2], EnumScript.getElemFromStr(array[3]), 0);
                            listAttaqueInventaire.Add(sortZ);
                            equipeAttaqueAt(i + 1, 0);
                            break;
                    }
                }
            }
        }
        
        //Attaque inventaire
        int nbAtt = PlayerPrefs.GetInt("attaqueInventaireCount", -1);
        if(nbAtt != -1) 
        {
            for(int i = 0; i < nbAtt; i++)
            {
                string save = PlayerPrefs.GetString("attaqueInventaire" + i.ToString(), "default");
                if(save != "default")
                {
                    string[] array = save.Split(';');
                    switch(array[0])
                    {
                        case "1":
                            SortDeJet sortJ = new SortDeJet(array[1],
                                array[2], 
                                EnumScript.getElemFromStr(array[3]), 
                                GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().getLvlFromNamePart(array[2]),
                                Int32.Parse(array[4]),
                                Int32.Parse(array[5]),
                                Int32.Parse(array[6]),
                                Int32.Parse(array[7]),
                                Int32.Parse(array[8]),
                                EnumScript.getCustom1FromString(array[9]), 
                                EnumScript.getCustom2FromString(array[10])
                                );
                            listAttaqueInventaire.Add(sortJ);
                            break;
                        case "2":
                            SortDeZone sortZ = new SortDeZone(array[1], array[2], EnumScript.getElemFromStr(array[3]), 0);
                            listAttaqueInventaire.Add(sortZ);
                            break;
                    }
                }
            }
        }
    }

    private void SaveSkin()
    {
        PlayerPrefs.SetString("skinCubes", textureCubesStr);
    }

    private void LoadSkin()
    {
        textureCubesStr = PlayerPrefs.GetString("skinCubes", "none");
        if (textureCubesStr == "none")
            textureCubesStr = "hexRed";
        textureCubes = Resources.Load("Player/Textures/" + textureCubesStr) as Texture;
    }

    public Texture getSkin()
    {
        return textureCubes;
    }

    public void setSkin(Texture texture, string name)
    {
        textureCubesStr = name;
        textureCubes = texture;
        SaveSkin();
    }

    public void majSortsProjEquip(structSortJet s)
    {
        for(int i = 0; i < 6; i++)
        { 
            if(listAttaque[i] != null && (listAttaque[i].getNameParticle() == s.nomParticle))
            {
                listAttaque[i].setLvl(s.lvl);
                
            }
        }
    }
    public void majAllSort()
    {
        for (int i = 0; i < 6; i++)
            if(listAttaque[i] != null)
                listAttaque[i].majSort();
        foreach(Attaque atk in listAttaqueInventaire)
            atk.majSort();
    }

    public void majSortsZoneEquip(structSortDeZone s)
    {
        print(s.nomParticle);
        print(listAttaque[0].getNameParticle());
        for (int i = 0; i < 6; i++)
        {
            if (listAttaque[i] != null && (listAttaque[i].getNameParticle() == s.nomParticle))
            {
                listAttaque[i].setLvl(s.lvl);
            }
        }
    }

    //Player shot 
    /*void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttaqueEnemy")
        {
            takeDegats(other.GetComponent<ProjectileData>().degats);
            GetComponent<PlayerCubeFlock>().setShakiness(PDVactuel, PDVmax);

            lifeBar.fillAmount = PDVactuel / PDVmax;
            lifeDisplay.text = PDVactuel.ToString();

            if (PDVactuel <= 0)
            {
                EndLvl(true);
            }
        }
    }*/

    public void EndLvl(bool dead)
    {
        if (dead)
        {
            GetComponent<PlayerCubeFlock>().Die();
            GameObject.FindWithTag("World").GetComponent<EnemyBehaviour>().resetAllEnemies();
        }
        
        GetComponent<PlayerController>().setControllable(false);
        Camera.main.transform.gameObject.GetComponent<CameraDeath>().enabled = true;
        Camera.main.transform.gameObject.GetComponent<CameraController>().enabled = false;

        List<String> noms = new List<String>();
        List<String> nomsPart = new List<String>();
        List<int> types = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            if (listAttaque[i] != null)
            {
                if (!noms.Contains(listAttaque[i].getNameInMenu()))
                {
                    noms.Add(listAttaque[i].getNameInMenu());
                    nomsPart.Add(listAttaque[i].getNameParticle());
                    types.Add(listAttaque[i].type);
                }
            }
        }

        GameObject.Find("MenuDeath").GetComponent<MenuDeath>().displayResults(noms, nomsPart, types, dead);
    }

    public bool isDead()
    {
        return dead;
    }

    public override void equipeAttaqueAt(int face, int indexInList)
    {
        base.equipeAttaqueAt(face, indexInList);
        listLogosSorts[face-1].GetComponent<RawImage>().texture = Resources.Load("Player/LogosSorts/" + listAttaque[face-1].getNameParticle()) as Texture;
    }

    //Player touch enemy
    //void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log(other.transform.tag);

    //    if (other.transform.tag == "Enemy")
    //    {
    //        print("ARGHH enemy");
    //    }
    //}
}