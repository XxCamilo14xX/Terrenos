using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public float secondsPerIteration = 1.0f;
    private float time = 0f;

    public List<Bunny> bunnies = new List<Bunny>();

    void Start()
    {
        Bunny[] found = FindObjectsByType<Bunny>(FindObjectsSortMode.InstanceID);
        bunnies = new List<Bunny>(found);
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= secondsPerIteration )
        {
            time = 0f;
            Simulate();
        }
    }

    void Simulate()
    {
        foreach (Bunny b in bunnies)
        {
            if (b != null && b.isAlive)
            {
                b.Simulate(secondsPerIteration);
            }
        }
    }
}
