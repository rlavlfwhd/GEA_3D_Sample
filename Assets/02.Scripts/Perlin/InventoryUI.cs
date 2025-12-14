using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Transform> Slot = new List<Transform>();
    public GameObject SlotItem;
    List<GameObject> items = new List<GameObject>();

    public int selectedIndex = -1;
    Inventory inventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();       
        if (inventory != null) inventory.OnInventoryChanged += UpdateInventoryUI;
    }

    private void Update()
    {
        for(int i = 0; i < Mathf.Min(9, Slot.Count); i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetSelectedIndex(i);
            }
        }
    }

    public void SetSelectedIndex(int idx)
    {
        ResetSelection();
        if (selectedIndex == idx || idx >= items.Count)
        {
            selectedIndex = -1;
        }
        else
        {
            SetSelection(idx);
            selectedIndex = idx;
        }
    }

    public void ResetSelection()
    {
        foreach(var slot in Slot)
        {
            slot.GetComponent<Image>().color = Color.white;
        }
    }

    public void SetSelection(int _idx)
    {
        Slot[_idx].GetComponent<Image>().color = Color.yellow;
    }

    public ItemData GetSelectedData()
    {
        if (selectedIndex < 0 || selectedIndex >= items.Count) return null;
        return items[selectedIndex].GetComponent<SlotItemPrefab>().itemData;
    }

    public void UpdateInventoryUI()
    {
        foreach(var slotItems in items)
        {
            Destroy(slotItems);
        }
        items.Clear();

        int idx = 0;

        foreach (var kvp in inventory.items)
        {
            if (idx >= Slot.Count) break;
            ItemData data = kvp.Key;
            int count = kvp.Value;

            var go = Instantiate(SlotItem, Slot[idx].transform);
            go.transform.localPosition = Vector3.zero;

            SlotItemPrefab sItem = go.GetComponent<SlotItemPrefab>();
            sItem.ItemSetting(data, count);

            items.Add(go);
            idx++;
        }

        if (selectedIndex >= items.Count)
        {
            selectedIndex = -1;
            ResetSelection();
        }
    }
}
