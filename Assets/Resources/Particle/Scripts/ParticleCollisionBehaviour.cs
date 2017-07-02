using UnityEngine;
using System.Collections;

public class ParticleCollisionBehaviour : MonoBehaviour {

    public GameObject core;
    public GameObject explosion;
    public float timeBeforeDestroy;
    public EnumScript.Character emiter;
    public CaracProjectiles caracProjectiles;

    public bool isBomb;

    private bool alreadyTouched = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isBomb)
        {
            if (!other.GetComponent<Collider>().isTrigger)
            {
                if (emiter == EnumScript.Character.Player)
                {
                    if (other.tag != "Player" && other.tag != "AttaquePlayer" && other.tag != "AttaqueEnemy")
                    {
                        core.SetActive(false);
                        explosion.SetActive(true);
                        Destroy(this.gameObject, timeBeforeDestroy);
                        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                        GetComponent<Rigidbody>().useGravity = false;

                        //Only class implementing interface ITakeDamages will be affected (and only once)
                        Enemy enemyScript = other.GetComponent<Enemy>();
                        if (enemyScript != null)
                        {
                            //Deal damages
                            enemyScript.takeDamages(GetComponent<ProjectileData>().degats);
                            GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().gagnerXP(GetComponent<ProjectileData>().nomParticule, GetComponent<ProjectileData>().nbProj);
                        }
                        enabled = false;
                    }
                }
                else if (emiter == EnumScript.Character.Enemy)
                {
                    if (other.tag != "Enemy" && other.tag != "AttaquePlayer" && other.tag != "AttaqueEnemy")
                    {
                        core.SetActive(false);
                        explosion.SetActive(true);
                        Destroy(this.gameObject, timeBeforeDestroy);
                        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                        GetComponent<Rigidbody>().useGravity = false;

                        //Only class implementing interface ITakeDamages will be affected (and only once)
                        Player playerScript = other.GetComponent<Player>();
                        if (playerScript != null)
                        {
                            //Deal damages
                            playerScript.takeDamages(GetComponent<ProjectileData>().degats);
                        }
                        enabled = false;
                    }
                }
            }
        }
        else
        {
            if (!alreadyTouched)
            {
                /*
                Create a sphere trigger
                list all enemies in sphere 
                ray from center to enemy
                if no obstable
                dealDamages
                */
                if (!other.GetComponent<Collider>().isTrigger)
                {
                    if (emiter == EnumScript.Character.Player)
                    {
                        if (other.tag != "Player" && other.tag != "AttaquePlayer" && other.tag != "AttaqueEnemy")
                        {
                            //If the bomb explodes on the player, touch it (it won't be hit by the ray)
                            if(other.tag == "Enemy") 
                            {
                                Enemy enemyScript = other.gameObject.GetComponent<Enemy>();
                                if (enemyScript != null) //it's an enemy
                                {
                                    enemyScript.takeDamages(GetComponent<ProjectileData>().degats);
                                    GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().gagnerXP(GetComponent<ProjectileData>().nomParticule, 1);
                                }
                            }
                            alreadyTouched = true;
                            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
                            core.SetActive(false);
                            explosion.SetActive(true);
                            Destroy(this.gameObject, timeBeforeDestroy);
                            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                            GetComponent<Rigidbody>().useGravity = false;

                            Destroy(GetComponent<SphereCollider>());

                            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.5f);
                            int i = 0;
                            while (i < hitColliders.Length)
                            {
                                Enemy enemyScript = hitColliders[i].gameObject.GetComponent<Enemy>();
                                if (enemyScript != null) //it's an enemy
                                {
                                    //Check if the enemy can be touch by explosion (no wall of etc.)
                                    int oldLayer = hitColliders[i].gameObject.layer;
                                    hitColliders[i].gameObject.layer = 31; //change layer of the enemy to trace a ray ignoring all other enemies
                                    RaycastHit hit;
                                    //Recuperation du layerMask Player et Projectile
                                    LayerMask layerEnemy = LayerMask.GetMask("Enemy");
                                    LayerMask layerPlayer = LayerMask.GetMask("Player");
                                    LayerMask layerProj = LayerMask.GetMask("Projectile");
                                    LayerMask layerIgnore = LayerMask.GetMask("Ignore Aim");
                                    int layerValue = layerEnemy.value | layerPlayer.value | layerProj.value | layerIgnore.value;
                                    //Inversion (on avoir la detection de tout SAUF du joueur et des projectiles)
                                    layerValue = ~layerValue;

                                    Vector3 from = this.transform.position + new Vector3(0, 0.25f, 0);
                                    Vector3 direction = hitColliders[i].transform.position - from;
                                    if (Physics.Raycast(from, direction, out hit, 2.5f, layerValue))
                                    {
                                        if (hit.transform.gameObject.Equals(hitColliders[i].transform.gameObject))
                                        {
                                            //Deal damages
                                            float multiplier = Vector3.Distance(transform.position, hitColliders[i].transform.position) / 2.5f;
                                            float degatsFinal = GetComponent<ProjectileData>().degats * multiplier;
                                            //Debug.Log(degatsFinal);
                                            enemyScript.takeDamages(degatsFinal);
                                            GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().gagnerXP(GetComponent<ProjectileData>().nomParticule, 1 / multiplier);
                                        }
                                    }
                                    hitColliders[i].gameObject.layer = oldLayer; //Put back old layer "Enemy"
                                }
                                i++;
                            }
                        }
                    }
                    else if (emiter == EnumScript.Character.Enemy)
                    {
                        if (other.tag != "Enemy" && other.tag != "AttaquePlayer" && other.tag != "AttaqueEnemy")
                        {
                            core.SetActive(false);
                            explosion.SetActive(true);
                            Destroy(this.gameObject, timeBeforeDestroy);
                            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                            GetComponent<Rigidbody>().useGravity = false;
                        }
                    }
                }
                Destroy(this.GetComponent<ParticleCollisionBehaviour>());
            }
        }
    }
}
