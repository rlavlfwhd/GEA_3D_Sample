using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public ItemData dirtData;
    public ItemData grassData;
    public ItemData stoneData;
    public ItemData waterData;

    [System.Serializable]
    public struct OreRule
    {
        public string name;
        public ItemData data;
        [Range(0, 100)] public int minHeight;
        [Range(0, 100)] public int maxHeight; 
        [Range(0f, 1f)] public float probability;
    }

    [System.Serializable]
    public struct DecorationRule
    {
        public string name;
        public ItemData data;
        [Range(0f, 1f)] public float probability;
    }

    public List<OreRule> oreRules = new List<OreRule>();
    public List<DecorationRule> decorationRules = new List<DecorationRule>();

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;
    public int waterHeight = 4;

    [SerializeField] float noiseScale = 20f;

    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
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
                        CreateBlock(x, y, z, grassData);

                        if (y > waterHeight)
                        {
                            foreach (var deco in decorationRules)
                            {
                                if (Random.value < deco.probability)
                                {
                                    CreateBlock(x, y + 1, z, deco.data);
                                    break;
                                }
                            }
                        }
                    }
                    else if(y > h -3)
                    {
                        CreateBlock(x, y, z, dirtData);
                    }
                    else
                    {
                        ItemData blockToPlace = stoneData;

                        foreach (var rule in oreRules)
                        {
                            if (y >= rule.minHeight && y <= rule.maxHeight)
                            {
                                if (Random.value < rule.probability)
                                {
                                    blockToPlace = rule.data;
                                    break;
                                }
                            }
                        }
                        CreateBlock(x, y, z, blockToPlace);
                    }
                }

                for(int y = h + 1; y <= waterHeight; y++)
                {
                    CreateBlock(x, y, z, waterData);
                }
            }
        }
    }

    void CreateBlock(int x, int y, int z, ItemData data)
    {
        if (data == null || data.blockPrefab == null) return;

        var go = Instantiate(data.blockPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"{data.itemName}_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.data = data;
        b.hp = data.maxHP;
    }

    public void PlaceTile(Vector3Int pos, ItemData data)
    {
        if (data == null || data.blockPrefab == null) return;

        CreateBlock(pos.x, pos.y, pos.z, data);
    }
}
