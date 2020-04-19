using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGeneration : MonoBehaviour
{
    public GameObject m_treePrefab;
    public GameObject m_coalPrefab;
    public LayerMask m_terrainLayer;

    // Start is called before the first frame update
    void Start()
    {
        int worldSize = 2048;

        // Trees
        {
            int treeGridCellSize = 48;
            Vector2Int minTreeGridCell = new Vector2Int(worldSize, worldSize) / -2 / treeGridCellSize;
            Vector2Int maxTreeGridCell = new Vector2Int(worldSize, worldSize) / 2 / treeGridCellSize;
            for (int treeGridCellX = minTreeGridCell.x; treeGridCellX < maxTreeGridCell.x; ++treeGridCellX)
            {
                for (int treeGridCellY = minTreeGridCell.y; treeGridCellY < maxTreeGridCell.y; ++treeGridCellY)
                {
                    Vector2 perlinNoiseInput = new Vector2(treeGridCellX, treeGridCellY) * 0.35f;
                    float noise = Mathf.PerlinNoise(perlinNoiseInput.x, perlinNoiseInput.y);
                    if (noise > 0.25f)
                    {
                        Vector3 treePosition =
                            new Vector3(treeGridCellX * treeGridCellSize + (treeGridCellSize * 0.5f), 0,
                                treeGridCellY * treeGridCellSize + (treeGridCellSize * 0.5f));

                        treePosition += new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));

                        bool hit = Physics.Raycast(treePosition + new Vector3(0, 100.0f, 0), Vector3.down,
                            out RaycastHit hitInfo, 150.0f, m_terrainLayer);
                        if (!hit)
                        {
                            continue;
                        }

                        treePosition.y = hitInfo.point.y - 0.35f;

                        GameObject treeObject = GameObject.Instantiate(m_treePrefab, treePosition,
                            Quaternion.Euler(0, Random.Range(0, 360), 0));

                        treeObject.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f),
                            Random.Range(0.8f, 1.2f),
                            Random.Range(0.8f, 1.2f));
                    }
                }
            }
        }
        
        //Coal
        {
            int treeGridCellSize = 48;
            Vector2Int minTreeGridCell = new Vector2Int(worldSize, worldSize) / -2 / treeGridCellSize;
            Vector2Int maxTreeGridCell = new Vector2Int(worldSize, worldSize) / 2 / treeGridCellSize;
            for (int treeGridCellX = minTreeGridCell.x; treeGridCellX < maxTreeGridCell.x; ++treeGridCellX)
            {
                for (int treeGridCellY = minTreeGridCell.y; treeGridCellY < maxTreeGridCell.y; ++treeGridCellY)
                {
                    Vector2 perlinNoiseInput = new Vector2(treeGridCellX + 55.0f, treeGridCellY + 55.0f) * 0.35f;
                    float noise = Mathf.PerlinNoise(perlinNoiseInput.x, perlinNoiseInput.y);
                    if (noise > 0.2f)
                    {
                        Vector3 treePosition =
                            new Vector3(treeGridCellX * treeGridCellSize + (treeGridCellSize * 0.5f), 0,
                                treeGridCellY * treeGridCellSize + (treeGridCellSize * 0.5f));

                        treePosition += new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));

                        bool hit = Physics.Raycast(treePosition + new Vector3(0, 100.0f, 0), Vector3.down,
                            out RaycastHit hitInfo, 150.0f, m_terrainLayer);
                        if (!hit || hitInfo.collider.CompareTag("Tree"))
                        {
                            continue;
                        }

                        treePosition.y = hitInfo.point.y - 0.1f;

                        GameObject treeObject = GameObject.Instantiate(m_coalPrefab, treePosition,
                            Quaternion.Euler(0, Random.Range(0, 360), 0));

                        treeObject.transform.localScale = new Vector3(Random.Range(0.75f, 1.25f),
                            Random.Range(0.75f, 1.25f),
                            Random.Range(0.75f, 1.25f));
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
