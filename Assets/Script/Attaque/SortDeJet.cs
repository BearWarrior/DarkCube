using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortDeJet : Attaque
{
    public float vitesseProj;
    public float rayonEffet;
    public GameObject proj;

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
    }

    public SortDeJet(float p_vitesse, float p_cd, float p_degats, EnumScript.Element p_element, string p_nomProj, string p_nomSort, string p_nameInMenu, int p_lvl)
    {
        type = 1;
        vitesseProj = p_vitesse;
        cooldown = p_cd;
        degats = p_degats;
        nameParticle = p_nomProj;
        element = p_element;
        pseudoSort = p_nomSort;
        nameInMenu = p_nameInMenu;
        lvl = p_lvl;
    }

    public override void AttackFromPlayer()
    { 
        if (canShoot)
        {
            launchProjPlayer(0, 0);
            lastShot = Time.time;
            canShoot = false;
        }
    }

    public override void AttackFromEnemy(Vector3 direction)
    {
        if (canShoot)
        {
            launchProjPlayer(0, 0);
            lastShot = Time.time;
            canShoot = false;
        }
    }

    public void launchProjPlayer(float offsetH, float offsetW)
    {
        Debug.Log("Particle / Prefabs / SortsDeJet / " + nameParticle + element.ToString() +"1");
        proj = GameObject.Instantiate(Resources.Load("Particle/Prefabs/SortsDeJet/" + nameParticle + element.ToString() +"1"), GameObject.FindWithTag("SpawnProjectile").transform.position + new Vector3(offsetW, offsetH, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
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
            direction = (hit.point - GameObject.FindWithTag("SpawnProjectile").transform.position) / Vector3.Distance(hit.point, GameObject.FindWithTag("SpawnProjectile").transform.position);

        proj.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);

        proj.transform.tag = "AttaquePlayer";
        setAllTagsAndAddVelocity("AttaquePlayer", proj, 75 * direction * Time.deltaTime * vitesseProj);

        ProjectileData projData = proj.AddComponent<ProjectileData>();
        projData.degats = degats;
        projData.element = element;

    }

    public void launchProjEnemy(float offsetH, float offsetW)
    {
        proj = GameObject.Instantiate(Resources.Load("Particle/Prefabs/SortsDeJet/" + nameParticle + element.ToString() + "1"), GameObject.FindWithTag("SpawnProjectile").transform.position + new Vector3(offsetW, offsetH, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
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
            direction = (hit.point - GameObject.FindWithTag("SpawnProjectile").transform.position) / Vector3.Distance(hit.point, GameObject.FindWithTag("SpawnProjectile").transform.position);

        proj.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);

        proj.transform.tag = "AttaquePlayer";
        setAllTagsAndAddVelocity("AttaquePlayer", proj, 75 * direction * Time.deltaTime * vitesseProj);

        ProjectileData projData = proj.AddComponent<ProjectileData>();
        projData.degats = degats;
        projData.element = element;

    }

    public void setAllTagsAndAddVelocity(string tag, GameObject go, Vector3 velocity)
    {
        for(int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).tag = "AttaquePlayer";
            if (go.GetComponent<Rigidbody>() != null)
                go.GetComponent<Rigidbody>().velocity = velocity;
            setAllTagsAndAddVelocity(tag, go.transform.GetChild(i).transform.gameObject, velocity);
        }
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
