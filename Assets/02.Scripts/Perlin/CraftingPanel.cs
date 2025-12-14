using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class CraftingPanel : MonoBehaviour
{
    public Inventory inventory;
    public List<CraftingRecipe> recipeList;

    public GameObject root;
    public TMP_Text plannedText;
    public Button craftButton;
    public Button clearButton;
    public TMP_Text hintText;

    public bool isOpen;

    readonly Dictionary<ItemData, int> planned = new Dictionary<ItemData, int>();
    
    CrosshairUI crosshairUI;
    PlayerController_MC playerController;

    private void Awake()
    {
        crosshairUI = FindObjectOfType<CrosshairUI>();
        playerController = FindObjectOfType<PlayerController_MC>();
        inventory = FindObjectOfType<Inventory>();
    }

    private void Start()
    {
        SetOpen(false);
        craftButton.onClick.AddListener(DoCraft);
        clearButton.onClick.AddListener(ClearPlanned);
        RefreshPlannedUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SetOpen(!isOpen);
        }
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
        if(root) root.SetActive(open);
        if (!open) ClearPlanned();

        if (crosshairUI) crosshairUI.SetCursorStatus(open);
        if (playerController) playerController.isUiOpen = open;
    }

    public void AddPlanned(ItemData data, int count = 1)
    {
        if (data == null) return;

        int myStock = inventory.GetCount(data);
        int currentPlanned = 0;

        if (planned.ContainsKey(data))
        {
            currentPlanned = planned[data];
        }

        if (currentPlanned + count > myStock)
        {
            SetHint("더 이상 재료가 없습니다.");
            return;
        }

        if (!planned.ContainsKey(data)) planned[data] = 0;
        planned[data] += count;

        RefreshPlannedUI();
        SetHint($"{data.itemName} x{count} 추가 완료");
    }


    public void ClearPlanned()
    {
        planned.Clear();
        RefreshPlannedUI();
        SetHint("초기화 완료");
    }

    void RefreshPlannedUI()
    {
        if (!plannedText) return;
        if(planned.Count == 0)
        {
            plannedText.text = "우클릭으로 재료를 추가하세요";
            return;
        }

        var sb = new StringBuilder();

        foreach (var item in planned)
        {
            sb.AppendLine($"{item.Key.itemName} x{item.Value}");
        }

        plannedText.text = sb.ToString();
    }

    void SetHint(string msg)
    {
        if(hintText) hintText.text = msg;
    }

    void DoCraft()
    {
        if(planned.Count == 0)
        {
            SetHint("재료가 부족합니다.");
            return;
        }

        foreach(var plannedItem in planned)
        {
            if(inventory.GetCount(plannedItem.Key) < plannedItem.Value)
            {
                SetHint($"{plannedItem.Key.itemName} 가 부족합니다.");
                return;
            }
        }

        var matchedRecipe = FindMatch(planned);

        if (matchedRecipe == null)
        {
            SetHint("알맞는 레시피가 없습니다.");
            return;
        }

        foreach (var item in planned) inventory.Consume(item.Key, item.Value);
        foreach (var p in matchedRecipe.outputs) inventory.Add(p.data, p.count);

        ClearPlanned();
        SetHint($"조합 완료 : {matchedRecipe.displayName}");
    }

    CraftingRecipe FindMatch(Dictionary<ItemData, int> currentPlan)
    {
        foreach(var recipe in recipeList)
        {
            if (recipe.inputs.Count != currentPlan.Count) continue;

            bool isMatch = true;

            foreach (var ing in recipe.inputs)
            {
                if(!currentPlan.TryGetValue(ing.data, out int plannedCount) || plannedCount != ing.count)
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch) return recipe;
        }

        return null;
    }
}
