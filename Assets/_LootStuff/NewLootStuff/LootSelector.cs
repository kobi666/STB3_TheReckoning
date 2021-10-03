using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class LootSelector : MonoBehaviour
{
    public static LootSelector instance;
    
    [Required]
    public LootItemManager LootItemManager;
    private ItemSelectionMode _itemSelectionMode;
    [ShowInInspector][LabelText("Read Only")]
    public ItemSelectionMode ItemSelectionMode
    {
        get => _itemSelectionMode;
        set => _itemSelectionMode = value;
    }

    [FormerlySerializedAs("TowerPlacementMenu")] [Required]
    public PlacementSelectionMenu placementSelectionMenu;

    public PlayerInput PlayerInput;
    
    [ShowInInspector][SerializeField]
    private NewLootSlot SelectedLootSlot;

    private SpriteRenderer SpriteRenderer;


    public void CancelItemPlacement()
    {
        DisableItemPlacementMenu();
        SpriteRenderer.sprite = SelectedLootSlot.LootObject.Value.ObjectSprite;
        transform.position = SelectedLootSlot.transform.position;
    }

    

    public void ConcludeLootAction()
    {
        
    }
    
    public void DisableItemPlacementMenu()
    {
        placementSelectionMenu.gameObject.SetActive(false);
        placementSelectionMenu.transform.parent.gameObject.SetActive(false);
        
        SelectedTowerController = null;
        ItemSelectionMode = ItemSelectionMode.LootItemSelect;
    }

    private TowerSelectionDisplay SelectedTowerSelectionDisplay;

    public void OpenTowerPlacementMenu(TowerLootObject towerLootObject)
    {
        ItemSelectionMode = ItemSelectionMode.TowerPlacement;
        placementSelectionMenu.gameObject.SetActive(true);
        placementSelectionMenu.transform.parent.gameObject.SetActive(true);
        SpriteRenderer.sprite = towerLootObject.ObjectSprite;
        transform.position = placementSelectionMenu.transform.position;
        SelectedTowerController = towerLootObject.TowerPrefab;
    }

    public event Action<NewLootSlot> onLootItemSelect;
    public event Action<ButtonDirectionsNames?> onTowerSlotConfirm;
    

    public void OnConfirmButton()
    {
        if (ItemSelectionMode == ItemSelectionMode.LootItemSelect)
        {
            SelectedLootSlot.LootObject.Value.OnLootObjectSelect(this);
            return;
        }

        if (ItemSelectionMode == ItemSelectionMode.TowerPlacement)
        {
            GameManager.Instance.PlayerTowersManager.SetTowerByButtonName(SelectedTowerSelectionDisplay.MyDirectionalButtonName,SelectedTowerController);
            return;
        }
    }

    public TowerController SelectedTowerController;

    private void Awake()
    {
        instance = this;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerInput = new PlayerInput();
        //testing
        PlayerInput.ItemSelection.Enable();
        PlayerInput.ItemSelection.MoveItemCursor.performed += ctx =>
            MoveItemCursor(ctx.ReadValue<Vector2>());
        PlayerInput.ItemSelection.ConfirmButton.performed += ctx => OnConfirmButton();
    }
    
    

    public void MoveItemCursor(Vector2 normalizedVector2)
    {
        if (ItemSelectionMode == ItemSelectionMode.LootItemSelect)
        {
            MoveCursorToNewItem(TowerUtils.GetCardinalDirectionFromAxis(normalizedVector2));
            return;
        }

        if (ItemSelectionMode == ItemSelectionMode.TowerPlacement)
        {
            MoveCursorToTowerPlacementSlot(TowerUtils.GetCardinalDirectionFromAxis(normalizedVector2));
            return;
        }
    }


    public void MoveCursorToTowerPlacementSlot(Vector2 cardinalV2)
    {
        if (SelectedTowerSelectionDisplay == null)
        {
            SelectedTowerSelectionDisplay = placementSelectionMenu.InitialSlot;
            transform.position = (Vector2)SelectedTowerSelectionDisplay.transform.position;
        }
        else
        {
            var tsd = SelectedTowerSelectionDisplay.GetSlotByNormalizedVector(cardinalV2);
            if (tsd != null)
            {
                SelectedTowerSelectionDisplay = tsd;
                transform.position = (Vector2)SelectedTowerSelectionDisplay.transform.position;
            }
        }
    }
    

    public void MoveCursorToNewItem(Vector2 cardinalV2)
    {
        if (SelectedLootSlot == null)
        {
            var slot = LootItemManager.AllSlots.Any(x => x.IsSlotAvailable()) ? LootItemManager.AllSlots.First() : null;
            if (slot != null)
            {
                transform.position = slot.transform.position;
                SelectedLootSlot = slot;
                return;
            }
        }
        else
        {
            var slot = SelectedLootSlot.GetSlotByNormalizedVector(cardinalV2);
            if (slot != null)
            {
                transform.position = slot.transform.position;
                SelectedLootSlot = slot;
                return;
            }
        }
        
    }
}

public enum ItemSelectionMode
{
    LootItemSelect,
    TowerPlacement
}


