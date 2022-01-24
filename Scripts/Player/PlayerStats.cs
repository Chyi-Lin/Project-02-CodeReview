using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class PlayerStats : MonoBehaviour
    {
        /// <summary>
        /// Singleton Pattern
        /// </summary>
        public static PlayerStats Instance;

        [SerializeField, Header("玩家紀錄")]
        protected PlayerData playerData;

        [SerializeField, Header("基礎造型")]
        protected EquipmentData playerEquipment;

        [SerializeField, Header("選擇關卡")]
        protected LevelData playerSelectLevel;

        protected ILoadSave loadSave;

        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            Initialization();
            Load();
        }

        protected void Initialization()
        {
            loadSave = new LocalLoadSave("playerData" + ".bin");
        }

        /// <summary>
        /// 取得玩家資料
        /// </summary>
        public PlayerData GetPlayerData() => playerData;

        /// <summary>
        /// 玩家選擇的關卡
        /// </summary>
        public LevelData PlayerSelectLevel { get{ return playerSelectLevel; } set { playerSelectLevel = value; } }

        /// <summary>
        /// 載入紀錄
        /// </summary>
        public void Load()
        {
            PlayerData loadPlayerData = loadSave.Load<PlayerData>();

            if (loadPlayerData != null)
            {
                // 發現記錄檔
                playerData = loadPlayerData;
#if UNITY_EDITOR
                Debug.Log("載入玩家記錄檔: playerData" + ".bin");
#endif
            }
            else
            {
                // 第一次執行遊戲
                playerData = new PlayerData();
                playerData.AddPurchaseRecord(playerEquipment, 0);
                Save();
            }
        }

        /// <summary>
        /// 存檔
        /// </summary>
        public void Save()
        {
            loadSave.Save(playerData);
#if UNITY_EDITOR
            Debug.Log("保存玩家記錄檔: playerData" + ".bin");
#endif
        }

#if UNITY_EDITOR

        [ContextMenu("Show Player Record")]
        public void ShowPlayerRecord()
        {
            if (playerData == null)
                return;

            Debug.Log("Show Player Record ===============");
            foreach (string recode in playerData.ToString().Split(' '))
            {
                Debug.Log(recode);
            }
            Debug.Log("End ===============");
        }

        [ContextMenu("Show Purchas Record")]
        public void ShowPurchasRecord()
        {
            if (playerData == null)
                return;

            Debug.Log("Show Purchas Record ===============");
            foreach (KeyValuePair<string, int> recode in playerData.GetPurchaseRecord)
            {
                Debug.Log($"ID:{recode.Key} Num Of Purchases:{recode.Value}");
            }
            Debug.Log("End ===============");
        }

        [ContextMenu("Show Levels Record")]
        public void ShowLevelsRecord()
        {
            if (playerData == null)
                return;

            Debug.Log("Show Levels Record ===============");
            foreach (KeyValuePair<string, PlayerLevelData> recode in playerData.GetLevelRecord)
            {
                Debug.Log($"Level:{recode.Key} {recode.Value}");
            }
            Debug.Log("End ===============");
        }

#endif

    }
}
