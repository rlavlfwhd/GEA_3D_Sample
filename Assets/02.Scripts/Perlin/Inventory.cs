using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();
    public event Action OnInventoryChanged;

    public int GetCount(ItemData data)
    {
        if (data == null) return 0;
        items.TryGetValue(data, out var count);
        return count;
    }

    public void Add(ItemData data, int count = 1)
    {
        if (data == null) return;

        if (!items.ContainsKey(data)) items[data] = 0;
        items[data] += count;
        OnInventoryChanged?.Invoke();
    }

    public bool Consume(ItemData data, int count = 1)
    {
        if (data == null) return false;
        if (!items.TryGetValue(data, out var have) || have < count) return false;

        items[data] = have - count;

        if (items[data] == 0)
        {
            items.Remove(data);
        }

        OnInventoryChanged?.Invoke();
        return true;
    }
}
