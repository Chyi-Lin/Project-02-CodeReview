using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "HyperCasual/Item/Create Equipment Data", order = 50)]
public class EquipmentData : ItemBase
{
    [SerializeField, Header("模型")]
    protected GameObject modelPrefab;

    [SerializeField, Header("投影材質球")]
    protected Material projectorMaterial;

    [SerializeField, Header("花費")]
    protected int cost;

    public GameObject GetModel { get { return this.modelPrefab; } }

    public Material GetProjectorMaterial { get { return this.projectorMaterial; } }

    public int GetCost { get { return this.cost; } }
}
