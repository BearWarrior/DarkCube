using UnityEngine;
using System.Collections;

public class particleArrow : MonoBehaviour
{

    public ParticleSystem system;
    ParticleSystem.Particle[] m_Particles = new ParticleSystem.Particle[1000];

    public GameObject trailRenderer;
	
    // Use this for initialization
    void Start()
    {
        system = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        int nbparticle = system.GetParticles(m_Particles);
        Debug.Log("nbparticle" + nbparticle);
        for (int i = 0; i < nbparticle; i++)
        {
            trailRenderer.transform.localPosition = m_Particles[i].position;
        }
        system.SetParticles(m_Particles, nbparticle);
    }
}