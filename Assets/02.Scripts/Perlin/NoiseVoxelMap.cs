using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public int width = 30;
    public int depth = 90;
    public int maxHeight = 16;
    public int waterHeight = 4;

    [SerializeField] float noiseScale = 20f;

    public ItemData forestDirt;
    public ItemData forestGrass;
    public ItemData forestStone;
    public ItemData forestWater;
    [Range(0f, 1f)] public float treeProbability = 0.05f;
    public ItemData treeLog;

    public ItemData snowGrass;
    public ItemData snowDirt;
    public ItemData iceBlock;
    public ItemData ironOre;
    [Range(0f, 1f)] public float ironProbability = 0.08f;

    public ItemData volcanoAsh;
    public ItemData volcanoRock;
    public ItemData lavaBlock;
    public ItemData diamondOre;
    [Range(0f, 1f)] public float diamondProbability = 0.06f;


    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        int zoneLength = depth / 3;

        for (int x = 0; x < width; x++)
        {
            for(int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;
                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.FloorToInt(noise * maxHeight);

                if (h <= 0) h = 1;

                int currentBiome = 0;
                if (z >= zoneLength && z < zoneLength * 2) currentBiome = 1;
                else if (z >= zoneLength * 2) currentBiome = 2;

                bool isUnderwater = (h < waterHeight);

                for (int y = 0; y <= h; y++)
                {
                    ItemData blockToPlace = null;

                    if (currentBiome == 0) blockToPlace = forestDirt;
                    else if (currentBiome == 1) blockToPlace = snowDirt;
                    else blockToPlace = volcanoRock;

                    if (y < h - 2)
                    {
                        if (currentBiome == 1 && Random.value < ironProbability) blockToPlace = ironOre;
                        else if (currentBiome == 2 && Random.value < diamondProbability) blockToPlace = diamondOre;
                        else if (currentBiome == 0 && y < h - 4) blockToPlace = forestStone;
                    }

                    CreateBlock(x, y, z, blockToPlace);
                }

                if (!isUnderwater)
                {
                    if (currentBiome == 0)
                    {
                        if (forestGrass != null) CreateBlock(x, h + 1, z, forestGrass);

                        if (Random.value < treeProbability && treeLog != null)
                        {
                            CreateBlock(x, h + 1, z, treeLog);
                            CreateBlock(x, h + 2, z, treeLog);
                        }
                    }
                    else if (currentBiome == 1)
                    {
                        if (snowGrass != null) CreateBlock(x, h + 1, z, snowGrass);
                    }
                    else if (currentBiome == 2)
                    {
                        if (volcanoAsh != null) CreateBlock(x, h + 1, z, volcanoAsh);
                    }
                }

                for (int y = h + 1; y <= waterHeight; y++)
                {
                    ItemData liquidToPlace = forestWater;

                    if (currentBiome == 1) liquidToPlace = iceBlock;
                    else if (currentBiome == 2) liquidToPlace = lavaBlock;

                    CreateBlock(x, y, z, liquidToPlace);
                }
            }
        }
    }

    void CreateBlock(int x, int y, int z, ItemData data)
    {
        if (data == null || data.blockPrefab == null) return;

        Vector3 spawnPos = new Vector3(x, y + data.verticalOffset, z);

        var go = Instantiate(data.blockPrefab, spawnPos, Quaternion.identity, transform);
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
