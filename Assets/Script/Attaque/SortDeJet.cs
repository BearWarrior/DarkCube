using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortDeJet : Attaque
{
    public int nbProjectile;
    public float vitesseProj;
    public float rayonEffet;
    public bool gravite;
    public string nomProj;
    public bool stuck;
    public EnumScript.PatternSortDeJet patternEnvoi;

    public List<GameObject> listProjCreated;
    public GameObject proj;

    private int nbProjActuel;

    public delegate void Del();
    Del fctDelegate;

    public SortDeJet()
    {
        type = 1;
        nbProjActuel = 0;
        nbProjectile = 0;
        gravite = false;
        vitesseProj = 0;
        cooldown = 1;
        degats = 0;
        nomProj = "none";
        element = EnumScript.Element.Eau;
        nomSort = "none";
        patternEnvoi = EnumScript.PatternSortDeJet.Rafale;
    }

    public SortDeJet(SortDeJet copy)
    {
        type = 1;
        nbProjActuel = 0;
        nbProjectile = copy.nbProjectile;
        gravite = copy.gravite;
        vitesseProj = copy.vitesseProj;
        cooldown = copy.cooldown;
        degats = copy.degats;
        nomProj = copy.nomProj;
        element = copy.element;
        nomSort = copy.nomSort;
        patternEnvoi = copy.patternEnvoi;
    }

    public SortDeJet(int p_nbProj, float p_vitesse, bool p_grav, float p_cd, float p_degats, EnumScript.Element p_element, string p_nomProj, string p_nomSort, EnumScript.PatternSortDeJet p_pat)
    {
        type = 1;
        nbProjActuel = 0;
        nbProjectile = p_nbProj;
        gravite = p_grav;
        vitesseProj = p_vitesse;
        cooldown = p_cd;
        degats = p_degats;
        nomProj = p_nomProj;
        element = p_element;
        nomSort = p_nomSort;
        patternEnvoi = p_pat;
    }

    public override void Attaquer()
    {
        if (canShoot)
        {
            //Si ce n'est pas une rafale
            if (patternEnvoi != EnumScript.PatternSortDeJet.Rafale)
            {
                switch (nbProjectile)
                {
                    case 1:
                        createProj(0, 0);
                        break;
                    case 2:
                        createProj(0, -0.5f);
                        createProj(0, 0.5f);
                        break;
                    case 3:
                        switch (patternEnvoi)
                        {
                            case EnumScript.PatternSortDeJet.SimultaneLigne:
                                createProj(0, -1);
                                createProj(0, 0);
                                createProj(0, 1);
                                break;
                            case EnumScript.PatternSortDeJet.SimultaneTriangle:
                                createProj(1f, 0);
                                createProj(-1f, -1f);
                                createProj(-1f, 1f);
                                break;
                        }
                        break;
                    case 4:
                        switch (patternEnvoi)
                        {
                            case EnumScript.PatternSortDeJet.SimultaneLigne:
                                createProj(0, -1.5f);
                                createProj(0, -.5f);
                                createProj(0, .5f);
                                createProj(0, 1.5f);
                                break;
                            case EnumScript.PatternSortDeJet.SimultaneCarre:
                                createProj(0.5f, 0.5f);
                                createProj(0.5f, -0.5f);
                                createProj(-0.5f, 0.5f);
                                createProj(-0.5f, -0.5f);
                                break;
                        }
                        break;
                }

                //Une fois tous les projectiles créés, on les lance
                launchProj();
            }
            else //C'est une rafale
            {
                //On crée un delegate pour décenbtraliser la tempo sur le script player qui viendra apeler la méthode shootRafale à intervals réguliers (Du au fait que ce script n'est pas MonoBehavior)
                fctDelegate = shootRafale;
                GameObject.FindWithTag("Player").GetComponent<Player>().launchProjRafale(nbProjectile, cooldown / (2.0f * nbProjectile), fctDelegate);
            }

            lastShot = Time.time;
            canShoot = false;
        }
    }

    public void createProj(float offsetH, float offsetW)
    {
        proj = GameObject.Instantiate(Resources.Load("Projectiles/" + nomProj), GameObject.Find("SpawnPoint").transform.position + new Vector3(offsetW, offsetH, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        proj.transform.parent = null;

        if (listProjCreated == null)
            listProjCreated = new List<GameObject>();

        listProjCreated.Add(proj);

    }

    public void launchProj()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2 + 0.08f * Screen.height));
        //Recuperation du layerMask Player et Projectile
        LayerMask layerPlayer = LayerMask.GetMask("Player");
        LayerMask layerProj = LayerMask.GetMask("Projectile");
        int layerValue = layerPlayer.value | layerProj.value;
        //Inversion (on avoir la detection de tout SAUF du joueur et des projectiles
        layerValue = ~layerValue;

        Vector3 direction = new Vector3(0, 0, 0);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerValue))
            direction = (hit.point - GameObject.Find("SpawnPoint").transform.position) / Vector3.Distance(hit.point, GameObject.Find("SpawnPoint").transform.position);


        GameObject whole = new GameObject();
        whole.transform.position = GameObject.Find("SpawnPoint").transform.position;
        foreach (GameObject go in listProjCreated)
        {
            go.transform.SetParent(whole.transform);
        }

        whole.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);

        if (gravite)
        {
            
            float rot = Camera.main.transform.localEulerAngles.x;
            if (rot > 270)
                rot -= 360;

            rot = (rot - 17) * -1;

            foreach (GameObject go in listProjCreated)
            {
                go.transform.parent = null;
                go.GetComponent<Rigidbody>().velocity = GameObject.FindWithTag("Player").transform.TransformDirection(new Vector3(0, rot, vitesseProj * 1.5f));
                go.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else
        {
            foreach (GameObject go in listProjCreated)
            {
                Debug.Log("direction : " + direction);
                Debug.Log("vitesseProj : " + vitesseProj);
                go.transform.parent = null;
                go.GetComponent<Rigidbody>().velocity = 75 * direction * Time.deltaTime * vitesseProj;
                go.GetComponent<Rigidbody>().useGravity = false;
                
            }
        }

        if (stuck)
        {
            foreach (GameObject go in listProjCreated)
            {
                go.GetComponent<ProjectileStuck>().setInitRotation(go.transform.eulerAngles);
            }
        }

        listProjCreated.Clear();
        GameObject.Destroy(whole.gameObject);
    }

    //Appelé depuis le script Player (Coroutine)
    public void shootRafale()
    {
        createProj(0, 0);
        launchProj();
    }
}
