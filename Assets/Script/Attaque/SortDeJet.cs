using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortDeJet : Attaque
{
    public float vitesseProj;
    public GameObject proj;

    public EnumScript.CustomProj1 custom1;
    public EnumScript.CustomProj2 custom2;
    public int nbDePointsTot;
    public int pointsInVitesse; 
    public int pointsInCooldown; 
    public int pointsInDegats;  
    public int pointsInCustom1; 
    public int pointsInCustom2; 

    public int nbProjSec;

    public delegate void Del();
    Del fctDelegate;

    public SortDeJet()
    {
        type = 1;
        vitesseProj = 0;
        cooldown = 1;
        degats = 0;
        nameParticle = "none";
        element = EnumScript.Element.Eau;
        pseudoSort = "none";
        nameInMenu = "none";
        lvl = 1;
        nbXpPerShot = 1;
        custom1 = EnumScript.CustomProj1.Normal;
        custom2 = EnumScript.CustomProj2.Normal;
        nbProjSec = 2;
        pointsInVitesse = 0;
        pointsInCooldown = 0;
        pointsInDegats = 0;
        pointsInCustom1 = 0;
        pointsInCustom2 = 0;
}

    public SortDeJet(SortDeJet copy)
    {
        type = 1;
        vitesseProj = copy.vitesseProj;
        cooldown = copy.cooldown;
        degats = copy.degats;
        nameParticle = copy.nameParticle;
        element = copy.element;
        pseudoSort = copy.pseudoSort;
        nameInMenu = copy.nameInMenu;
        lvl = copy.lvl;
        nbXpPerShot = copy.nbXpPerShot;
        custom1 = copy.custom1;
        custom2 = copy.custom2;
        nbProjSec = copy.nbProjSec;
        pointsInVitesse = copy.pointsInVitesse;
        pointsInCooldown = copy.pointsInCooldown;
        pointsInDegats = copy.pointsInDegats;
        pointsInCustom1 = copy.pointsInCustom1;
        pointsInCustom2 = copy.pointsInCustom2;
    }

    public SortDeJet(string p_nomSort, string p_nomProj, EnumScript.Element p_element, int p_lvl, int p_ptVit, int p_ptCd, int p_ptDeg, int p_ptCust1, int p_ptCust2, EnumScript.CustomProj1 p_custom1, EnumScript.CustomProj2 p_custom2)
    {
        type = 1;
        pseudoSort = p_nomSort;
        nameParticle = p_nomProj;
        element = p_element;
        lvl = p_lvl;
        custom1 = p_custom1;
        custom2 = p_custom2;
        pointsInVitesse = p_ptVit;
        pointsInCooldown = p_ptCd;
        pointsInDegats = p_ptDeg;
        pointsInCustom1 = p_ptCust1;
        pointsInCustom2 = p_ptCust2;
        nbDePointsTot = pointsInVitesse + pointsInCooldown + pointsInDegats + pointsInCustom1 + pointsInCustom2;

        structSortJet str = GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().getStructFromName(p_nomProj);

        vitesseProj = str.vitesse + pointsInVitesse * str.vitessePerLevel;
        cooldown = str.cooldown + pointsInCooldown * str.coolDownPerLevel;
        degats = str.degats + pointsInDegats * str.degatsPerLevel;
        nbXpPerShot = str.nbXpPerShot;
        nameInMenu = str.nameInMenu;
        nbProjSec = GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().nbMultiProjBase + pointsInCustom2;
    }

    public override void majSort()
    {
        structSortJet str = GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().getStructFromName(nameParticle);

        vitesseProj = str.vitesse + pointsInVitesse * str.vitessePerLevel;
        cooldown = str.cooldown + pointsInCooldown * str.coolDownPerLevel;
        degats = str.degats + pointsInDegats * str.degatsPerLevel;
        nbXpPerShot = str.nbXpPerShot;
        nameInMenu = str.nameInMenu;
        nbProjSec = GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().nbMultiProjBase + pointsInCustom2;
    }

    public override void AttackFromTestWeapons(Vector3 spawnPoint)
    {
        if (canShoot)
        {
            launchProjTestWeapons(spawnPoint);
            lastShot = Time.time;
            canShoot = false;
        }
    }

    public override void AttackFromPlayer(Vector3 spawnPoint)
    { 
        if (canShoot)
        {
            launchProjPlayer(spawnPoint);
            lastShot = Time.time;
            canShoot = false;
        }
    }

    public override void AttackFromEnemy(RaycastHit hit, Vector3 spawnPoint)
    {
        if (canShoot)
        {
            launchProjEnemy(hit, spawnPoint);
            lastShot = Time.time;
            canShoot = false;
        }
    }

    public void launchProjTestWeapons(Vector3 spawnPoint)
    {
        string partToLoad = "Particle/Prefabs/SortsDeJet/" + nameParticle + element.ToString();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2 + 0.08f * Screen.height));
        //Recuperation du layerMask Player et Projectile
        LayerMask layerPlayer = LayerMask.GetMask("Player");
        LayerMask layerProj = LayerMask.GetMask("Projectile");
        LayerMask layerIgnore = LayerMask.GetMask("Ignore Aim");
        int layerValue = layerPlayer.value | layerProj.value | layerIgnore.value;
        //Inversion (on avoir la detection de tout SAUF du joueur et des projectiles)
        layerValue = ~layerValue;

        /* Vector3 direction = new Vector3(0, 0, 0);
         if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerValue))
             direction = (hit.point - spawnPoint) / Vector3.Distance(hit.point, spawnPoint);*/
        Vector3 direction = new Vector3(1, 0, 0);

        //CUSTOM2
        proj = new GameObject("projComplexe");
        proj.transform.position = spawnPoint;
        GameObject projPrin = GameObject.Instantiate(Resources.Load(partToLoad), spawnPoint, new Quaternion(0, 0, 0, 0)) as GameObject;
        projPrin.transform.parent = null;
        projPrin.transform.SetParent(proj.transform);
        projPrin.GetComponent<Rigidbody>().velocity = 75 * direction * Time.deltaTime * vitesseProj;
        if (custom2 == EnumScript.CustomProj2.MultiProj)
        {
            int interval = 360 / nbProjSec;
            float angle = 0;
            float rayon = 0.3f;
            
            for(int i = 0; i < nbProjSec; i++)
            {
                angle = i * interval;
                angle = (float)(angle * Mathf.PI / 180.0);
                float z, y;
                if(angle  > 3.0 * Mathf.PI / 2.0) //3Pi/4
                {
                    angle -= (float) (3.0 * Mathf.PI / 2.0);
                    z = Mathf.Sin(angle) * rayon;
                    y = - Mathf.Cos(angle) * rayon;
                }
                else if(angle >  Mathf.PI) //2PI/4
                {
                    angle -= (float)( Mathf.PI);
                    z = - Mathf.Cos(angle) * rayon;
                    y = - Mathf.Sin(angle) * rayon;
                }
                else if(angle > Mathf.PI / 2.0) //Pi/4
                {
                    angle -= (float)(Mathf.PI / 2.0);
                    z = - Mathf.Sin(angle) * rayon;
                    y = Mathf.Cos(angle) * rayon;
                }
                else
                {
                    z = Mathf.Cos(angle) * rayon;
                    y = Mathf.Sin(angle) * rayon;
                }
                GameObject projSec = GameObject.Instantiate(Resources.Load(partToLoad), spawnPoint + new Vector3(0, y, z), new Quaternion(0, 0, 0, 0)) as GameObject;
                projSec.transform.SetParent(proj.transform);
                projSec.transform.localScale = new Vector3(.5f, .5f, .5f);
                Vector3 newDirection = Quaternion.Euler(0, 0, 0) * new Vector3(0, y, z);
                projSec.GetComponent<Rigidbody>().velocity = 75 * (direction + 0.5f*newDirection) * Time.deltaTime * vitesseProj;
            }
        }



        proj.AddComponent<DestroyIfNoChildren>();

        proj.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);

        proj.transform.tag = "AttaquePlayer";
        setAllTagsAndAddVelocityAndEmitter("AttaquePlayer", proj, 75 * direction * Time.deltaTime * vitesseProj, EnumScript.Character.Player);
        setAllProjData(proj, degats, element, 1, nameParticle);
    }

    public void launchProjPlayer(Vector3 spawnPoint)
    {
        string partToLoad = "Particle/Prefabs/SortsDeJet/" + nameParticle + element.ToString();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2 + 0.08f * Screen.height));
        //Recuperation du layerMask Player et Projectile
        LayerMask layerPlayer = LayerMask.GetMask("Player");
        LayerMask layerProj = LayerMask.GetMask("Projectile");
        LayerMask layerIgnore = LayerMask.GetMask("Ignore Aim");
        int layerValue = layerPlayer.value | layerProj.value | layerIgnore.value;
        //Inversion (on avoir la detection de tout SAUF du joueur et des projectiles)
        layerValue = ~layerValue;

        Vector3 direction = new Vector3(0, 0, 0);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerValue))
            direction = (hit.point - spawnPoint) / Vector3.Distance(hit.point, spawnPoint);

        //CUSTOM2
        proj = new GameObject("projComplexe");
        proj.transform.position = spawnPoint;
        //Debug.Log(partToLoad);
        GameObject projPrin = GameObject.Instantiate(Resources.Load(partToLoad), spawnPoint, new Quaternion(0, 0, 0, 0)) as GameObject;
        projPrin.transform.parent = null;
        projPrin.transform.SetParent(proj.transform);
        projPrin.GetComponent<Rigidbody>().velocity = 75 * direction * Time.deltaTime * vitesseProj;
        if (custom2 == EnumScript.CustomProj2.MultiProj)
        {
            int interval = 360 / nbProjSec;
            float angle = 0;
            float rayon = 0.3f;
            
            for(int i = 0; i < nbProjSec; i++)
            {
                angle = i * interval;
                angle = (float)(angle * Mathf.PI / 180.0);
                float x, y;
                if(angle  > 3.0 * Mathf.PI / 2.0) //3Pi/4
                {
                    angle -= (float) (3.0 * Mathf.PI / 2.0);
                    x = Mathf.Sin(angle) * rayon;
                    y = - Mathf.Cos(angle) * rayon;
                }
                else if(angle >  Mathf.PI) //2PI/4
                {
                    angle -= (float)( Mathf.PI);
                    x = - Mathf.Cos(angle) * rayon;
                    y = - Mathf.Sin(angle) * rayon;
                }
                else if(angle > Mathf.PI / 2.0) //Pi/4
                {
                    angle -= (float)(Mathf.PI / 2.0);
                    x = - Mathf.Sin(angle) * rayon;
                    y = Mathf.Cos(angle) * rayon;
                }
                else
                {
                    x = Mathf.Cos(angle) * rayon;
                    y = Mathf.Sin(angle) * rayon;
                }
                GameObject projSec = GameObject.Instantiate(Resources.Load(partToLoad), spawnPoint + new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
                projSec.transform.SetParent(proj.transform);
                projSec.transform.localScale = new Vector3(.5f, .5f, .5f);
                Vector3 newDirection = Quaternion.Euler(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0) * new Vector3(x, y, 0);
                projSec.GetComponent<Rigidbody>().velocity = 75 * (direction + 0.5f*newDirection) * Time.deltaTime * vitesseProj;
            }
        }



        proj.AddComponent<DestroyIfNoChildren>();

        proj.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);

        proj.transform.tag = "AttaquePlayer";
        setAllTagsAndAddVelocityAndEmitter("AttaquePlayer", proj, 75 * direction * Time.deltaTime * vitesseProj, EnumScript.Character.Player);
        setAllProjData(proj, degats, element, 1, nameParticle);


        //ProjectileData projData = proj.AddComponent<ProjectileData>();
        //projData.degats = degats;
        //projData.element = element;
        //projData.type = 1;
        //projData.nomParticule = nameParticle;
    }

    public void launchProjEnemy(RaycastHit hit, Vector3 spawnPoint)
    {
        string partToLoad = "Particle/Prefabs/SortsDeJet/" + nameParticle + element.ToString();
        proj = GameObject.Instantiate(Resources.Load(partToLoad), spawnPoint, new Quaternion(0, 0, 0, 0)) as GameObject;
        proj.transform.parent = null;
        Vector3 direction = (hit.point - spawnPoint) / Vector3.Distance(hit.point, spawnPoint);
        proj.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);

        proj.transform.tag = "AttaqueEnemy";
        setAllTagsAndAddVelocityAndEmitter("AttaqueEnemy", proj, 75 * direction * Time.deltaTime * vitesseProj, EnumScript.Character.Enemy);

        setAllProjData(proj, degats, element, 1, nameParticle);
    }

    
    public EnumScript.CustomProj1 getCustom1()
    {
        return custom1;
    }
    public void setCustom1(EnumScript.CustomProj1 custom)
    {
        custom1 = custom;
    }

    public EnumScript.CustomProj2 getCustom2()
    {
        return custom2;
    }
    public void setCustom2(EnumScript.CustomProj2 custom)
    {
        custom2 = custom;
    }

    //public void createProj(float offsetH, float offsetW)
    //{


    //    proj = GameObject.Instantiate(Resources.Load("Projectiles/" + nomProj), GameObject.Find("SpawnProjectile").transform.position + new Vector3(offsetW, offsetH, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
    //    proj.transform.parent = null;

    //    if (listProjCreated == null)
    //        listProjCreated = new List<GameObject>();

    //    listProjCreated.Add(proj);

    //}

    //with old projectile (gravity etc)
    //public void launchProj()
    //{
    //    Debug.Log("SHOOT : " + "Projectiles/" + nomProj + element.ToString() + "Anim");


    //    RaycastHit hit;
    //    Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2 + 0.08f * Screen.height));
    //    //Recuperation du layerMask Player et Projectile
    //    LayerMask layerPlayer = LayerMask.GetMask("Player");
    //    LayerMask layerProj = LayerMask.GetMask("Projectile");
    //    int layerValue = layerPlayer.value | layerProj.value;
    //    //Inversion (on avoir la detection de tout SAUF du joueur et des projectiles
    //    layerValue = ~layerValue;

    //    Vector3 direction = new Vector3(0, 0, 0);
    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerValue))
    //        direction = (hit.point - GameObject.Find("SpawnProjectile").transform.position) / Vector3.Distance(hit.point, GameObject.Find("SpawnProjectile").transform.position);


    //    GameObject whole = new GameObject();
    //    whole.transform.position = GameObject.Find("SpawnProjectile").transform.position;
    //    foreach (GameObject go in listProjCreated)
    //    {
    //        go.transform.SetParent(whole.transform);
    //    }

    //    whole.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);



    //    foreach (GameObject go in listProjCreated)
    //    {
    //        //Debug.Log("direction : " + direction);
    //        //Debug.Log("vitesseProj : " + vitesseProj);
    //        go.transform.parent = null;
    //        go.GetComponent<Rigidbody>().velocity = 75 * direction * Time.deltaTime * vitesseProj;
    //        go.GetComponent<Rigidbody>().useGravity = false;
    //    }


    //    listProjCreated.Clear();
    //    GameObject.Destroy(whole.gameObject);
    //}

    //Appelé depuis le script Player (Coroutine)
    //public void shootRafale()
    //{
    //    createProj(0, 0);
    //    launchProj();
    //}
}
