using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public ItemData data;
    public int hp;
    public int dropCount = 1;

    private void Awake()
    {
        if(GetComponent<Collider>() == null) gameObject.AddComponent<BoxCollider>();
        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
            gameObject.tag = "Block";
    }

    public void Hit(int damage, Inventory inven)
    {
        if (data == null || !data.isMineable) return;

        hp -= damage;

        if(hp <= 0)
        {
            if (inven != null && dropCount > 0 && data != null)
            {
                inven.Add(data, dropCount);
            }

            Destroy(gameObject);
        }
    }
}
