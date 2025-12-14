using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Recipe", menuName = "조합법 생성")]
public class CraftingRecipe : ScriptableObject
{
    [Serializable] public struct Ingredient 
    {
        public ItemData data;
        public int count;
    }

    [Serializable] public struct Product
    {
        public ItemData data;
        public int count;
    }

    public string displayName;
    public List<Ingredient> inputs = new List<Ingredient>();
    public List<Product> outputs = new List<Product>();
}
