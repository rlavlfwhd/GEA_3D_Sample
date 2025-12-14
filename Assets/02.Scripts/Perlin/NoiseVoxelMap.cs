using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject woodPrefab;
    public GameObject stonePrefab;
    public GameObject diamondPrefab;
    public GameObject waterPrefab;

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;
    public int waterHeight = 4;

    [SerializeField] float noiseScale = 20f;

    private void Start()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for(int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;
                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.FloorToInt(noise * maxHeight);

                if (h <= 0) h = 1;

                for (int y = 0; y <= h; y++)
                {
                    if (y == h)
                    {
                        PlaceGrass(x, y, z);

                        if (y > waterHeight && Random.value < 0.1f)
                        {
                            PlaceWood(x, y + 1, z);
                        }
                    }
                    else if(y > h -3)
                    {
                        PlaceDirt(x, y, z);
                    }
                    else
                    {
                        if (y < 5 && Random.value < 0.05f)
                        {
                            PlaceDiamond(x, y, z);
                        }
                        else
                        {
                            PlaceStone(x, y, z);
                        }
                    }
                }

                for(int y = h + 1; y <= waterHeight; y++)
                {
                    PlaceWater(x, y, z);
                }
            }
        }
    }

    void PlaceDirt(int x, int y, int z)
    {
        CreateBlock(dirtPrefab, x, y, z, ItemType.Dirt, 2, 1, true);
    }

    void PlaceWater(int x, int y, int z)
    {
        CreateBlock(waterPrefab, x, y, z, ItemType.Water, 3, 1, false);
    }

    void PlaceGrass(int x, int y, int z)
    {
        CreateBlock(grassPrefab, x, y, z, ItemType.Grass, 1, 1, true);
    }

    void PlaceWood(int x, int y, int z)
    {
        CreateBlock(woodPrefab, x, y, z, ItemType.Wood, 4, 1, true);
    }
    void PlaceStone(int x, int y, int z)
    {
        CreateBlock(stonePrefab, x, y, z, ItemType.Stone, 6, 1, true);
    }

    void PlaceDiamond(int x, int y, int z)
    {
        CreateBlock(diamondPrefab, x, y, z, ItemType.Diamond, 10, 1, true);
    }

    void CreateBlock(GameObject prefab, int x, int y, int z, ItemType type, int hp, int drop, bool mineable)
    {
        if (prefab == null) return;

        var go = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"{type}_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = type;
        b.maxHP = hp;
        b.dropCount = drop;
        b.mineable = mineable;
    }

    public void PlaceTile(Vector3Int pos, ItemType type)
    {
        switch(type)
        {
            case ItemType.Dirt:
                PlaceDirt(pos.x, pos.y, pos.z);
                break;
            case ItemType.Grass:
                PlaceGrass(pos.x, pos.y, pos.z);
                break;
            case ItemType.Water:
                PlaceWater(pos.x, pos.y, pos.z);
                break;
            case ItemType.Diamond:
                PlaceDiamond(pos.x, pos.y, pos.z);
                break;
            case ItemType.Wood:
                PlaceWood(pos.x, pos.y, pos.z);
                break;
            case ItemType.Stone:
                PlaceStone(pos.x, pos.y, pos.z);
                break;
        }
    }
}
