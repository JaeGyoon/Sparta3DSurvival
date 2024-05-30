using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    public string GetInteractionDescription()
    {
        string description = $"{itemData.itemName}\n{itemData.itemDescription}";

        return description;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = itemData;

        CharacterManager.Instance.Player.addItem?.Invoke();

        Destroy(gameObject);
    }    
}
