using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    [SerializeField, Header("物品ID")]
    protected string id;

    [SerializeField, Header("物品名稱")]
    protected string itemName;

    [SerializeField, Header("物品圖片")]
    protected Sprite itemSprite;

    public string GetId { get { return this.id; } }

    public string GetName { get { return this.itemName; } }

    public Sprite GetImage { get { return this.itemSprite; } }
}
