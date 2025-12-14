using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject blockPrefab;
    public int maxStack = 64;
    public bool isTool = false;
    public int attackDamage = 1;
    public string effectiveTag;
    public int maxHP = 3;
    public bool isMineable = true;
}
