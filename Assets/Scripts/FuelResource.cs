using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ResourceType
{
    Wood,
    Coal
}

public struct GatherInfo
{
    public int amount;
    public ResourceType resourceType;
}

public class FuelResource : MonoBehaviour
{
    public int m_health = 100;
    public int m_resourcesPerHit = 10;
    public ResourceType m_resourceType = ResourceType.Wood;
    public GameObject m_destructEffectPrefab;
    public float m_destructEffectHeightOffset = 1.0f;

    public GatherInfo Gather(ToolType toolType)
    {
        if (m_resourceType == ResourceType.Wood && toolType != ToolType.Axe)
        {
            return  new GatherInfo{amount = 0, resourceType = m_resourceType};
        }
        
        if (m_resourceType == ResourceType.Coal && toolType != ToolType.Pickaxe)
        {
            return  new GatherInfo{amount = 0, resourceType = m_resourceType};
        }
        
        m_health -= m_resourcesPerHit;
        if (m_health <= 0)
        {
            Instantiate(m_destructEffectPrefab, transform.position + new Vector3(0, m_destructEffectHeightOffset, 0),
                Quaternion.identity);
            Destroy(gameObject);
        }

        return new GatherInfo {amount = m_resourcesPerHit, resourceType = m_resourceType};
    }
}
