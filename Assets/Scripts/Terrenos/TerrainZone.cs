using UnityEngine;

public class TerrainZone : MonoBehaviour
{
    public enum ZoneType { Mud, Water, Snow }
    
    [Header("Configuración de Zona")]
    public ZoneType zoneType = ZoneType.Mud;
    
    [Header("Permisos de Cruce")]
    public bool bunniesCanCross = true;
    public bool predatorsCanCross = true;
    
    private BoxCollider2D zoneCollider;
    
    void Start()
    {
        // Obtener el collider
        zoneCollider = GetComponent<BoxCollider2D>();
        if (zoneCollider == null)
        {
            zoneCollider = gameObject.AddComponent<BoxCollider2D>();
            zoneCollider.isTrigger = true;
        }
        
        // Asegurar que el collider tenga el mismo tamaño que el sprite
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && zoneCollider != null)
        {
            zoneCollider.size = new Vector2(
                transform.localScale.x,
                transform.localScale.y
            );
        }
        
        // Registrar en el manager
        if (TerrainManager.Instance != null)
        {
            TerrainManager.Instance.RegisterZone(this);
        }
        
        Debug.Log($"Zona {zoneType} lista y VISIBLE");
    }
    
    public bool IsPositionInside(Vector3 position)
    {
        if (zoneCollider != null)
        {
            return zoneCollider.bounds.Contains(position);
        }
        return false;
    }
    
    public float GetSpeedMultiplierForAgent(string agentType)
    {
        if (agentType == "Bunny" && !bunniesCanCross) return 0f;
        if (agentType == "Predator" && !predatorsCanCross) return 0f;
        
        switch (zoneType)
        {
            case ZoneType.Mud: return TerrainManager.Instance.mudSpeedMultiplier;
            case ZoneType.Water: return TerrainManager.Instance.waterSpeedMultiplier;
            case ZoneType.Snow: return TerrainManager.Instance.snowSpeedMultiplier;
            default: return 1f;
        }
    }
}