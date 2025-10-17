using UnityEngine;

public class BunnyTerrainHandler : MonoBehaviour
{
    private Bunny bunny;
    private float originalSpeed;
    
    void Start()
    {
        bunny = GetComponent<Bunny>();
        if (bunny != null)
        {
            originalSpeed = bunny.speed;
            Debug.Log($"BunnyTerrainHandler configurado para {gameObject.name} - Velocidad original: {originalSpeed}");
        }
        else
        {
            Debug.LogError("BunnyTerrainHandler necesita componente Bunny en: " + gameObject.name);
        }
    }
    
    void Update()
    {
        if (bunny != null && bunny.isAlive && TerrainManager.Instance != null)
        {
            float multiplier = TerrainManager.Instance.GetSpeedMultiplier(transform.position, "Bunny");
            
            if (multiplier == 0f)
            {
                AvoidUncrossableTerrain();
                bunny.speed = 0f; // Detener movimiento
            }
            else
            {
                bunny.speed = originalSpeed * multiplier;
            }
        }
    }
    
    void AvoidUncrossableTerrain()
    {
        Vector3 newDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0
        ).normalized;
        
        // Usar reflexión para acceder al campo privado destination
        var destinationField = typeof(Bunny).GetField("destination", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
        if (destinationField != null)
        {
            Vector3 newDestination = transform.position + newDirection * bunny.visionRange;
            destinationField.SetValue(bunny, newDestination);
        }
    }
}