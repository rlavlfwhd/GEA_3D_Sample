using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject waterPrefab;

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;
    public int waterHeight = 4;

    private int[,] heightMap;

    [SerializeField] float noiseScale = 20f;

    private void Start()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        heightMap = new int[width, depth];

        for (int x = 0; x < width; x++)
        {
            for(int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;

                float noise = Mathf.PerlinNoise(nx, nz);

                int h = Mathf.FloorToInt(noise * maxHeight);

                if (h <= 0) 
                {
                    heightMap[x, z] = 0;
                    continue;
                }

                for (int y = 0; y < h; y++)
                {
                    Place(dirtPrefab, x, y, z);
                }

                Place(grassPrefab, x, h, z);

                heightMap[x, z] = h;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                int groundTop = heightMap[x, z];

                if (groundTop < waterHeight)
                {
                    for (int y = groundTop + 1; y <= waterHeight; y++)
                    {
                        Place(waterPrefab, x, y, z);
                    }
                }
            }
        }
    }

    void Place(GameObject prefab, int x, int y, int z)
    {
        if (prefab == null) return;

        var go = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"{prefab.name}_{x}_{y}_{z}";
    }
}
