using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Transform fireParent;
    List<ParticleSystem> particles = new List<ParticleSystem>();
    List<int> maxParticleList = new List<int>();

    public float extinguishTime = 3f;
    public float hasExtinguishTime = 0f;

    public float waitTime = .5f;
    public float hasWait = 0f;

    private void Start()
    {
        foreach (Transform t in fireParent)
        {
            var p = t.GetComponent<ParticleSystem>();
            particles.Add(p);
            maxParticleList.Add(p.main.maxParticles);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "Powder")
        {
            print("extinguishing");
            if (hasExtinguishTime < extinguishTime)
            {
                hasExtinguishTime += Time.deltaTime;
                hasWait = 0;
            }
            else
            {
                foreach (ParticleSystem p in particles)
                    p.Stop();

                GameHandler.Singleton.StageFinish();
                GetComponent<BoxCollider>().enabled=false;
            }

        }
    }

    private void Update()
    {
        hasWait += Time.deltaTime;

        for (int i = 0; i < particles.Count; ++i)
        {
            var particle = particles[i].main;
            particle.maxParticles = (int)(maxParticleList[i] * (1 - (hasExtinguishTime / extinguishTime)));

            if (hasWait > waitTime)
            {
                particle.maxParticles = maxParticleList[i];
                hasExtinguishTime = 0f;
            }
        }
    }
}
