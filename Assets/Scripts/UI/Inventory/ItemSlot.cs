using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    public ItemData itemData;
    public Inventory inventory;

    public int slotIndex;
    public bool equipped;
    public int quantity;

    public Button button;
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = itemData.itemIcon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if ( outline != null )
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        itemData = null;
        itemIcon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnClickItem()
    {
        inventory.OnClickSlot(slotIndex);
    }
}
