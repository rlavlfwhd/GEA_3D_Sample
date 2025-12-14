using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotItemPrefab : MonoBehaviour, IPointerClickHandler
{
    public Image itemImage;
    public TextMeshProUGUI itemText;
    public ItemData itemData;
    public CraftingPanel craftingPanel;

    public void ItemSetting(ItemData data, int count)
    {
        itemData = data;
        itemImage.sprite = data.icon;

        if (!data.isTool && count > 1)
        {
            itemText.text = count.ToString();
        }
        else
        {
            itemText.text = "";
        }
    }

    void Awake()
    {
        if (!craftingPanel) craftingPanel = FindObjectOfType<CraftingPanel>(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;
        if (!craftingPanel) return;

        craftingPanel.AddPlanned(itemData, 1);
    }
}
