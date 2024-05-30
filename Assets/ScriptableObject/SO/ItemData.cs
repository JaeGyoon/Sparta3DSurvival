using UnityEngine;

public enum ItemType
{
    Equipable = 0,
    Consumable = 1,
    Resource = 2,
}

public enum ConsumableType
{
    Health = 0,
    Hunger = 1,
}

[CreateAssetMenu(fileName ="Item", menuName = "new Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite itemIcon;
    public GameObject itemObject;

    [Header("Stacking")]
    public bool canStack;
    public int maxStack;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}
