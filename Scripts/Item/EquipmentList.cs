using System.Linq;
using UnityEngine;
using MoreMountains.Tools;

[CreateAssetMenu(fileName = "New Item List", menuName = "HyperCasual/Item/Create Equipment List", order = 50)]
public class EquipmentList : ItemListBase<EquipmentData>
{
    [Header("自動排序")]
    [MMInspectorButton("Sort", order = 0)]
    public bool autoSort;

    /// <summary>
    /// 透過 Id 來取得資料
    /// </summary>
    public override EquipmentData GetData(string id)
    {
        return (EquipmentData)itemList.Find((data) => data.GetId == id);
    }

    /// <summary>
    /// 透過索引來取得資料
    /// </summary>
    public override EquipmentData GetData(int position)
    {
        if (position >= itemList.Count)
        {
            Debug.LogWarning("索引已超過資料大小!");
            return null;
        }
 
        return (EquipmentData)itemList[position];
    }

    /// <summary>
    /// 取得資料長度
    /// </summary>
    public override int GetLength()
    {
        return itemList.Count;
    }

    /// <summary>
    /// 資料排序，由花費開始排序、再由ID排序
    /// </summary>
    public void Sort()
    {
        itemList = itemList.OrderBy(x => (x as EquipmentData).GetCost)
            .ThenBy(x => (x as EquipmentData).GetId)
            .ToList();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif

        MMDebug.DebugLogTime("資料排序完成！");
    }
}
