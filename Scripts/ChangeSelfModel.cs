using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class ChangeSelfModel : MonoBehaviour
    {
        [SerializeField, Header("Default")]
        protected Transform targetModel;
        [SerializeField]
        protected string layerName = "Player";
        protected Renderer targetRenderer;

        [SerializeField, Header("Equipment List")]
        protected EquipmentList equipmentList;

        protected void Awake()
        {
            Initialization();
            InitializationPlayerModel();
        }

        protected void Initialization()
        {
            targetRenderer = targetModel.GetComponentInChildren<Renderer>();
        }

        protected void InitializationPlayerModel()
        {
            if (PlayerStats.Instance == null)
                return;

            EquipmentData equipmentData = equipmentList.GetData(PlayerStats.Instance.GetPlayerData().EquipmentId);
            SetModel(equipmentData.GetModel);
        }

        /// <summary>
        /// 設定模型
        /// </summary>
        protected void SetModel(GameObject modelPrefab)
        {
            Destroy(targetModel.GetChild(0).gameObject);
            GameObject modelObject = Instantiate(modelPrefab, targetModel);

            foreach (var child in modelObject.GetComponentsInChildren<Transform>(true))
                child.gameObject.layer = LayerMask.NameToLayer(layerName);

            targetRenderer = modelObject.GetComponentInChildren<Renderer>();
        }

        /// <summary>
        /// 取得 Renderer
        /// </summary>
        public Renderer GetRenderer()
        {
            return targetRenderer;
        }
    }
}
