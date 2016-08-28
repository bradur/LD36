// Date   : 28.08.2016 13:01
// Project: LD36
// Author : bradur

using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {

    [SerializeField]
    private Text txtItemName;

    [SerializeField]
    private Image imgItem;
    int pos = 0;
    [SerializeField]
    RectTransform rt;

    private Item item;
    public Item Item { get { return item; } }

    float size = 80f;

    public void Init(Item item, string itemName, Sprite itemImage, int count)
    {
        this.item = item;
        imgItem.sprite = itemImage;
        txtItemName.text = itemName;
        this.pos = count; 
    }

    public void UpdatePosition(int pos)
    {
        this.pos = pos;
        rt.anchoredPosition = new Vector2(5f + pos * size, rt.anchoredPosition.y);
    }

}
