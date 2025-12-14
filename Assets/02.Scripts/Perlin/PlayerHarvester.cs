using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    public LayerMask hitMask = ~0;
    public int toolDamage = 1;
    public float rayDistance = 5f;
    public float hitCooldown = 0.15f;
    public Inventory inventory;
    public GameObject selectedBlock;

    float _nextHitTime;
    Camera _cam;
    InventoryUI inventoryUI;
    CraftingPanel craftingPanel;

    private void Awake()
    {
        _cam = Camera.main;
        if(inventory == null) inventory = gameObject.AddComponent<Inventory>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        craftingPanel = FindObjectOfType<CraftingPanel>();
    }

    private void Update()
    {
        if (craftingPanel.isOpen) return;

        bool isHand = inventoryUI.selectedIndex < 0;
        ItemType currentItem = ItemType.Dirt;
        if (!isHand) currentItem = inventoryUI.GetInventorySlot();

        if (isHand || IsTool(currentItem))
        {
            selectedBlock.transform.localScale = Vector3.zero;

            if (Input.GetMouseButton(0) && Time.time >= _nextHitTime)
            {
                _nextHitTime = Time.time + hitCooldown;

                Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
                {
                    var block = hit.collider.GetComponent<Block>();
                    if (block != null)
                    {
                        int finalDamage = GetDamage(currentItem, block.type, isHand);
                        block.Hit(finalDamage, inventory);
                    }
                }
            }
        }
        else
        {
            Ray rayDebug = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(rayDebug, out var hitDebug, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                Vector3Int placePos = AdjacentCellOnHitFace(hitDebug);

                if (selectedBlock != null)
                {
                    selectedBlock.transform.localScale = Vector3.one; // 보이게 켜기
                    selectedBlock.transform.position = placePos;      // 위치 이동
                    selectedBlock.transform.rotation = Quaternion.identity;
                }
            }
            else
            {
                // 허공을 보고 있으면 미리보기 숨기기
                if (selectedBlock != null)
                    selectedBlock.transform.localScale = Vector3.zero;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Vector3Int placePos = AdjacentCellOnHitFace(hit);

                    if (inventory.Consume(currentItem, 1))
                    {
                        FindObjectOfType<NoiseVoxelMap>().PlaceTile(placePos, currentItem);
                        _nextHitTime = Time.time + hitCooldown;
                    }
                }
            }
        }
    }

    bool IsTool(ItemType type)
    {
        return type == ItemType.Axe || type == ItemType.Shovel || type == ItemType.Pickaxe;
    }

    int GetDamage(ItemType tool, ItemType blockType, bool isHand)
    {
        if (isHand) return toolDamage;

        int dmg = toolDamage;

        switch (tool)
        {
            case ItemType.Axe:
                if (blockType == ItemType.Wood) dmg += 1;
                break;

            case ItemType.Shovel:
                if (blockType == ItemType.Dirt || blockType == ItemType.Grass) dmg += 1;
                break;

            case ItemType.Pickaxe:
                if (blockType == ItemType.Stone || blockType == ItemType.Diamond) dmg += 1;
                break;
        }

        return dmg;
    }

    static Vector3Int AdjacentCellOnHitFace(in RaycastHit hit)
    {
        Vector3 baseCenter = hit.collider.transform.position;
        Vector3 adjCenter = baseCenter + hit.normal;
        return Vector3Int.RoundToInt(adjCenter);
    }
}
