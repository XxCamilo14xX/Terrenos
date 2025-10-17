using UnityEngine;

public class PredatorTerrainHandler : MonoBehaviour
{
    private Predator predator;
    private float originalSpeed;
    
    void Start()
    {
        predator = GetComponent<Predator>();
        if (predator != null)
        {
            originalSpeed = predator.speed;
            Debug.Log($"PredatorTerrainHandler configurado para {gameObject.name} - Velocidad original: {originalSpeed}");
        }
        else
        {
            Debug.LogError("PredatorTerrainHandler necesita componente Predator en: " + gameObject.name);
        }
    }
    
    void Update()
    {
        if (predator != null && predator.isAlive && TerrainManager.Instance != null)
        {
            float multiplier = TerrainManager.Instance.GetSpeedMultiplier(transform.position, "Predator");
            predator.speed = originalSpeed * multiplier;
        }
    }
}