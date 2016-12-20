using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortDeJet : Attaque
{
    public float vitesseProj;
    public GameObject proj;

    public EnumScript.CustomProj1 custom1;
    public EnumScript.CustomProj2 custom2;

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
    }

    public SortDeJet(string p_nomSort, string p_nomProj, EnumScript.Element p_element, int p_lvl, EnumScript.CustomProj1 p_custom1, EnumScript.CustomProj2 p_custom2)
    {
        type = 1;
        pseudoSort = p_nomSort;
        nameParticle = p_nomProj;
        element = p_element;
        lvl = p_lvl;
        custom1 = p_custom1;
        custom2 = p_custom2;

        structSortJet str = GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().getStructFromName(p_nomProj);

        vitesseProj = str.vitesse + str.pointsInVitesse * str.vitessePerLevel;
        cooldown = str.cooldown + str.pointsInCooldown * str.coolDownPerLevel;
        degats = str.degats + str.pointsInDegats * str.degatsPerLevel;
        nbXpPerShot = str.nbXpPerShot;
        nameInMenu = str.nameInMenu;
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

    public void launchProjPlayer(Vector3 spawnPoint)
    {
        string lvlPart = (lvl < 3) ? "1" : (lvl < 6) ? "2" : "3";
        string partToLoad = "Particle/Prefabs/SortsDeJet/" + nameParticle + element.ToString() + lvlPart;
        proj = GameObject.Instantiate(Resources.Load(partToLoad), spawnPoint, new Quaternion(0, 0, 0, 0)) as GameObject;
        proj.transform.parent = null;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2 + 0.08f * Screen.height));
        //Recuperation du layerMask Player et Projectile
        LayerMask layerPlayer = LayerMask.GetMask("Player");
        LayerMask layerProj = LayerMask.GetMask("Projectile");
        int layerValue = layerPlayer.value | layerProj.value;
        //Inversion (on avoir la detection de tout SAUF du joueur et des projectiles)
        layerValue = ~layerValue;

        Vector3 direction = new Vector3(0, 0, 0);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerValue))
            direction = (hit.point - spawnPoint) / Vector3.Distance(hit.point, spawnPoint);

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
        string lvlPart = (lvl < 3) ? "1" : (lvl < 6) ? "2" : "3";
        string partToLoad = "Particle/Prefabs/SortsDeJet/" + nameParticle + element.ToString() + lvlPart;
        proj = GameObject.Instantiate(Resources.Load(partToLoad), spawnPoint, new Quaternion(0, 0, 0, 0)) as GameObject;
        proj.transform.parent = null;
        Vector3 direction = (hit.point - spawnPoint) / Vector3.Distance(hit.point, spawnPoint);
        proj.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);

        proj.transform.tag = "AttaqueEnemy";
        setAllTagsAndAddVelocityAndEmitter("AttaqueEnemy", proj, 75 * direction * Time.deltaTime * vitesseProj, EnumScript.Character.Enemy);

        setAllProjData(proj, degats, element, 1, nameParticle);
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
