using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListBase<T> : DataList<T> where T:ItemBase
{
    [SerializeField, Header("道具清單")]
    protected List<ItemBase> itemList;

    /// <summary>
    /// 透過 Id 來取得資料
    /// </summary>
    public override T GetData(string id)
    {
        return (T)itemList.Find((data) => data.GetId == id);
    }

    /// <summary>
    /// 透過索引來取得資料
    /// </summary>
    public override T GetData(int position)
    {
        if (position >= itemList.Count)
        {
            Debug.LogWarning("索引已超過資料大小!");
            return null;
        }

        return (T)itemList[position];
    }

    /// <summary>
    /// 取得資料長度
    /// </summary>
    public override int GetLength()
    {
        return itemList.Count;
    }
}
