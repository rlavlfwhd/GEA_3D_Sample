using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearManager : MonoBehaviour
{
    public ItemData targetItem; 
    public GameObject clearPanel;

    private Inventory inventory;
    private bool isCleared = false;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        if (clearPanel != null) clearPanel.SetActive(false);
    }

    void Update()
    {
        if (isCleared) return;
        if (inventory == null || targetItem == null) return;

        if (inventory.GetCount(targetItem) > 0)
        {
            Win();
        }
    }

    void Win()
    {
        isCleared = true;

        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
        }

        var player = FindObjectOfType<PlayerController_MC>();
        if (player) player.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}