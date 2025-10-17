using UnityEngine;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager Instance;
    
    [Header("Configuraciones de Terreno")]
    public float mudSpeedMultiplier = 0.3f;
    public float waterSpeedMultiplier = 0.8f;
    public float snowSpeedMultiplier = 0.5f;
    
    private List<TerrainZone> terrainZones = new List<TerrainZone>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        // Encontrar todas las zonas automáticamente
        TerrainZone[] zones = FindObjectsByType<TerrainZone>(FindObjectsSortMode.None);
        terrainZones = new List<TerrainZone>(zones);
        Debug.Log($"TerrainManager: {terrainZones.Count} zonas encontradas");
    }
    
    public void RegisterZone(TerrainZone zone)
    {
        if (!terrainZones.Contains(zone))
        {
            terrainZones.Add(zone);
        }
    }
    
    public float GetSpeedMultiplier(Vector3 position, string agentType)
    {
        foreach (TerrainZone zone in terrainZones)
        {
            if (zone.IsPositionInside(position))
            {
                return zone.GetSpeedMultiplierForAgent(agentType);
            }
        }
        return 1f;
    }
}