using UnityEngine;

/// <summary>
/// Scriptable Object 資料清單
/// </summary>
public abstract class DataList<T> : ScriptableObject
{
    /// <summary>
    /// 透過 Id 來取得資料
    /// </summary>
    public abstract T GetData(string id);

    /// <summary>
    /// 透過索引來取得資料
    /// </summary>
    public abstract T GetData(int position);

    /// <summary>
    /// 取得資料長度
    /// </summary>
    public abstract int GetLength();
}
