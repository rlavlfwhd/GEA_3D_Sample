using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    public LayerMask hitMask = ~0;
    public int handDamage = 1;
    public float rayDistance = 5f;
    public float hitCooldown = 0.15f;
    public Inventory inventory;
    public GameObject selectedBlockPreview;

    float _nextHitTime;
    Camera _cam;
    InventoryUI inventoryUI;
    CraftingPanel craftingPanel;
    NoiseVoxelMap map;

    private void Awake()
    {
        _cam = Camera.main;
        if(inventory == null) inventory = gameObject.AddComponent<Inventory>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        craftingPanel = FindObjectOfType<CraftingPanel>();
        map = FindObjectOfType<NoiseVoxelMap>();
    }

    private void Update()
    {
        if (craftingPanel != null && craftingPanel.isOpen) return;

        ItemData currentItem = inventoryUI.GetSelectedData();

        if (selectedBlockPreview) selectedBlockPreview.transform.localScale = Vector3.zero;

        if (Input.GetMouseButton(0) && Time.time >= _nextHitTime)
        {
            _nextHitTime = Time.time + hitCooldown;
            Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                var block = hit.collider.GetComponent<Block>();
                if (block != null)
                {
                    int damage = GetDamage(currentItem, block);
                    block.Hit(damage, inventory);
                }
            }
        }

        if (currentItem != null && currentItem.blockPrefab != null)
        {
            Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                Vector3Int placePos = AdjacentCellOnHitFace(hit);

                if (selectedBlockPreview != null)
                {
                    selectedBlockPreview.transform.localScale = Vector3.one;
                    selectedBlockPreview.transform.position = placePos;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (inventory.Consume(currentItem, 1))
                    {
                        map.PlaceTile(placePos, currentItem);
                        _nextHitTime = Time.time + hitCooldown;
                    }
                }
            }
        }
    }

    int GetDamage(ItemData tool, Block block)
    {
        if (tool == null || !tool.isTool) return handDamage;

        if (!string.IsNullOrEmpty(tool.effectiveTag) && block.CompareTag(tool.effectiveTag))
        {
            return tool.attackDamage;
        }

        return handDamage;
    }

    static Vector3Int AdjacentCellOnHitFace(in RaycastHit hit)
    {
        Vector3 baseCenter = hit.collider.transform.position;
        Vector3 adjCenter = baseCenter + hit.normal;
        return Vector3Int.RoundToInt(adjCenter);
    }
}
