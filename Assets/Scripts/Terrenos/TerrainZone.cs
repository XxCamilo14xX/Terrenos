using UnityEngine;

public class TerrainZone : MonoBehaviour
{
    public enum ZoneType { Mud, Water, Snow }
    
    [Header("Configuración de Zona")]
    public ZoneType zoneType = ZoneType.Mud;
    
    [Header("Permisos de Cruce")]
    public bool bunniesCanCross = true;
    public bool predatorsCanCross = true;
    
    [Header("Apariencia Visual")]
    public Color zoneColor = Color.blue;
    [Range(0.1f, 0.8f)]
    public float transparency = 0.3f;
    
    private BoxCollider2D zoneCollider;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        // Configurar componentes
        SetupComponents();
        // Configurar apariencia visual
        SetupVisualAppearance();
        // Registrar en el manager
        RegisterWithManager();
    }
    
    void SetupComponents()
    {
        // Obtener o agregar SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // Obtener o agregar BoxCollider2D
        zoneCollider = GetComponent<BoxCollider2D>();
        if (zoneCollider == null)
        {
            zoneCollider = gameObject.AddComponent<BoxCollider2D>();
            zoneCollider.isTrigger = true;
        }
    }
    
    void SetupVisualAppearance()
    {
        // Configurar el sprite
        SetupSprite();
        
        // Configurar color con transparencia
        Color finalColor = GetZoneColor();
        finalColor.a = transparency;
        spriteRenderer.color = finalColor;
        
        // Configurar orden de renderizado
        spriteRenderer.sortingOrder = -10;
        
        Debug.Log($"Zona {zoneType} configurada con color {finalColor}");
    }
    
    void SetupSprite()
    {
        // Intentar encontrar un sprite cuadrado por defecto
        GameObject tempGo = new GameObject("TempSprite");
        SpriteRenderer tempRenderer = tempGo.AddComponent<SpriteRenderer>();
        
        // Unity tiene un sprite cuadrado por defecto en los sprites primitivos
        // Usaremos el mismo método que Unity usa para crear sprites primitivos
        CreatePrimitiveSprite();
        
        Destroy(tempGo);
    }
    
    void CreatePrimitiveSprite()
    {
        // Crear una textura básica
        int textureSize = 64;
        Texture2D texture = new Texture2D(textureSize, textureSize);
        
        // Rellenar la textura de blanco
        Color[] pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        texture.SetPixels(pixels);
        texture.Apply();
        
        // Crear el sprite
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, textureSize, textureSize),
            new Vector2(0.5f, 0.5f),
            100f
        );
        
        spriteRenderer.sprite = sprite;
        
        // Ajustar el tamaño según el collider
        if (zoneCollider != null)
        {
            transform.localScale = new Vector3(zoneCollider.size.x, zoneCollider.size.y, 1f);
        }
    }
    
    Color GetZoneColor()
    {
        switch (zoneType)
        {
            case ZoneType.Mud: return new Color(0.65f, 0.5f, 0.2f);
            case ZoneType.Water: return new Color(0.2f, 0.4f, 0.8f);
            case ZoneType.Snow: return new Color(0.8f, 0.9f, 1.0f);
            default: return Color.white;
        }
    }
    
    void RegisterWithManager()
    {
        if (TerrainManager.Instance != null)
        {
            TerrainManager.Instance.RegisterZone(this);
        }
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
    
    void OnDrawGizmos()
    {
        if (zoneCollider != null)
        {
            Gizmos.color = zoneColor;
            Gizmos.DrawWireCube(transform.position + (Vector3)zoneCollider.offset, zoneCollider.size);
        }
    }
}