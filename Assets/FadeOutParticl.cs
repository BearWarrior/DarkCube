using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutParticl : MonoBehaviour
{
    private ParticleSystem[] listPart;

    // Use this for initialization
    void Start()
    {
        listPart = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in listPart)
        {
            var main = ps.main;
            main.loop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
