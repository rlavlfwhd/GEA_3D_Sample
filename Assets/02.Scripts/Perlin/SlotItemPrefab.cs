using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotItemPrefab : MonoBehaviour
{
    [Header("UI Components")]
    public Image icon;
    public TextMeshProUGUI countText;

    [Header("Block Icons")]
    public Sprite dirtIcon;
    public Sprite grassIcon;
    public Sprite waterIcon;

    public void Setup(BlockType type, int count)
    {
        if (icon != null)
        {
            icon.sprite = GetIcon(type);
            icon.enabled = (icon.sprite != null);
        }

        if (countText != null)
        {
            countText.text = count > 0 ? count.ToString() : "";
        }
    }

    Sprite GetIcon(BlockType type)
    {
        switch (type)
        {
            case BlockType.Dirt: return dirtIcon;
            case BlockType.Grass: return grassIcon;
            case BlockType.Water: return waterIcon;
            default: return null;
        }
    }
}
