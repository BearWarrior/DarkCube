//using UnityEngine;
//using System.Collections;

//public class SortDeSupport : Attaque
//{
//    public string nomSupport;
//    public GameObject support;
//    public float duree;

//    public SortDeSupport()
//    {
//        type = 3;
//        cooldown = 15;
//        degats = 0;
//        element = EnumScript.Element.Eau;
//        nomSupport = "none";
//        duree = 5;
//        nomSort = "none";
//    }

//    public SortDeSupport(SortDeSupport copy)
//    {
//        type = 3;
//        cooldown = copy.cooldown;
//        degats = copy.degats;
//        nomSupport = copy.nomSupport;
//        element = copy.element;
//        nomSort = copy.nomSort;
//    }

//    public SortDeSupport(float p_vitesse, float p_cd, float p_degats, EnumScript.Element p_element, string p_nomSupport, string p_nomSort)
//    {
//        type = 3;
//        cooldown = p_cd;
//        degats = p_degats;
//        nomSupport = p_nomSupport;
//        element = p_element;
//        nomSort = p_nomSort;
//    }

//    public override void Attaquer()
//    {
//        if (canShoot)
//        {
//            createProj(0, 0);
//            lastShot = Time.time;
//            canShoot = false;
//        }
//    }

//    public void createProj(float offsetH, float offsetW)
//    {
//        Debug.Log("SHOOT : " + "Projectiles/" + nomProj + element.ToString() + "Proj");
//        Debug.Log("Particle/prefabs/" + nomProj + element.ToString());

//        //proj = GameObject.Instantiate(Resources.Load("Projectiles/" + nomProj), GameObject.Find("SpawnPoint").transform.position + new Vector3(offsetW, offsetH, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
//        proj = GameObject.Instantiate(Resources.Load("Particle/prefabs/SortsDeJet/" + nomProj + element.ToString()), GameObject.Find("SpawnPoint").transform.position + new Vector3(offsetW, offsetH, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
//        proj.transform.parent = null;

//        RaycastHit hit;
//        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2 + 0.08f * Screen.height));
//        //Recuperation du layerMask Player et Projectile
//        LayerMask layerPlayer = LayerMask.GetMask("Player");
//        LayerMask layerProj = LayerMask.GetMask("Projectile");
//        int layerValue = layerPlayer.value | layerProj.value;
//        //Inversion (on avoir la detection de tout SAUF du joueur et des projectiles
//        layerValue = ~layerValue;

//        Vector3 direction = new Vector3(0, 0, 0);
//        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerValue))
//            direction = (hit.point - GameObject.Find("SpawnPoint").transform.position) / Vector3.Distance(hit.point, GameObject.Find("SpawnPoint").transform.position);

//        proj.transform.eulerAngles = new Vector3(0, GameObject.FindWithTag("Player").transform.eulerAngles.y, 0);

//        proj.AddComponent<Rigidbody>();
//        proj.GetComponent<Rigidbody>().velocity = 75 * direction * Time.deltaTime * vitesseProj;
//        proj.GetComponent<Rigidbody>().useGravity = false;
//    }
//}
