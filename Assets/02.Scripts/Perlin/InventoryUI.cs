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

    public void UpdateInventory(Inventory myInven)
    {
        foreach(var slotItems in items)
        {
            Destroy(slotItems);
        }
        items.Clear();

        int idx = 0;
        foreach(var item in myInven.items)
        {
            if (idx >= Slot.Count) break;

            var parent = Slot[idx];
            if (parent == null)
            {
                idx++;
                continue;
            }

            var go = Instantiate(SlotItem, parent);
            go.transform.localPosition = Vector3.zero;
            SlotItemPrefab sItem = go.GetComponent<SlotItemPrefab>();

            if (sItem != null)
            {
                sItem.Setup(item.Key, item.Value);
            }

            items.Add(go);

            idx++;
        }
    }
}
