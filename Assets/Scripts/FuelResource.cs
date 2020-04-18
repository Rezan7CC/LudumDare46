using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood
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

    public GatherInfo Gather()
    {
        m_health -= m_resourcesPerHit;
        if (m_health <= 0)
        {
            Destroy(gameObject);
        }

        return new GatherInfo {amount = m_resourcesPerHit, resourceType = m_resourceType};
    }
}
