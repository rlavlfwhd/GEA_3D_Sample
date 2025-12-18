using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCondition : MonoBehaviour
{
    public int maxHP = 10;
    public int currentHP;

    public float damageInterval = 2.0f;
    public int coldDamage = 1;

    public float zoneLength = 30f;

    public ItemData winterSuitData;
    public Inventory inventory;
    public Slider hpSlider;

    float _timer;

    private void Start()
    {
        currentHP = maxHP;
        if (inventory == null) inventory = FindObjectOfType<Inventory>();

        UpdateUI();
    }

    private void Update()
    {
        CheckBiomePenalty();
    }
    void CheckBiomePenalty()
    {
        float z = transform.position.z;

        bool inSnowZone = (z >= zoneLength && z < zoneLength * 2);

        if (inSnowZone)
        {
            bool hasSuit = CheckWinterSuit();

            if (!hasSuit)
            {
                _timer += Time.deltaTime;
                if (_timer >= damageInterval)
                {
                    TakeDamage(coldDamage);
                    _timer = 0f;
                }
            }
        }
        else
        {
            _timer = 0f;
        }
    }

    bool CheckWinterSuit()
    {
        if (inventory == null || winterSuitData == null) return false;

        return inventory.GetCount(winterSuitData) > 0;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateUI();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateUI()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    void Die()
    {
        SceneManager.LoadScene(0);
    }
}
