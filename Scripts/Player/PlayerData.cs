using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HyperCasualFramework
{

    [System.Serializable]
    public class PlayerData
    {
        /// <summary>
        /// 玩家 ID
        /// </summary>
        [SerializeField]
        private string id;

        /// <summary>
        /// 遊戲幣
        /// </summary>
        [SerializeField]
        private int coins;

        /// <summary>
        /// 遊戲累計分數
        /// </summary>
        [SerializeField]
        private int points;

        /// <summary>
        /// 遊戲累計時間(秒)
        /// </summary>
        [SerializeField]
        private float totalPlayTime;

        /// <summary>
        /// 遊戲累計失敗數
        /// </summary>
        [SerializeField]
        private int totalFails;

        /// <summary>
        /// 裝備
        /// </summary>
        [SerializeField]
        private string equipmentId;

        /// <summary>
        /// 到達的關卡 ID
        /// </summary>
        [SerializeField]
        private string arrivalLevelId;

        /// <summary>
        /// 關卡紀錄
        /// </summary>
        [SerializeField]
        private Dictionary<string, PlayerLevelData> levelRecord = new Dictionary<string, PlayerLevelData>();

        /// <summary>
        /// 購買紀錄，key=物品ID、value=購買次數
        /// </summary>
        [SerializeField]
        private Dictionary<string, int> purchaseRecord = new Dictionary<string, int>();

        public PlayerData() : this("000000", 0, "ie00", null) { }

        public PlayerData(string id, int coins, string equipmentId, string arrivalLevelId)
        {
            this.id = id;
            this.coins = coins;
            this.equipmentId = equipmentId;
            this.arrivalLevelId = arrivalLevelId;
        }

        public string GetId { get { return id; } }

        public int GetCoins { get { return coins; } }

        public int GetPoints { get { return levelRecord.Sum(x => x.Value.GetPoints); } }

        public float GetTotalPlayTime { get { return totalPlayTime; } }

        public int GetTotalFails { get { return totalFails; } }

        public string GetArrivalLevelId { get { return arrivalLevelId; } }

        public string EquipmentId { get { return equipmentId; } set { equipmentId = value; } }

        public Dictionary<string, PlayerLevelData> GetLevelRecord { get { return levelRecord; } }

        public Dictionary<string, int> GetPurchaseRecord { get { return purchaseRecord; } }

        public void UpdatePlayerLevelData(string id, float playTime, PlayerLevelData newRecord)
        {
            // 紀錄總遊玩時間
            totalPlayTime += playTime;
            // 紀錄總死亡次數
            totalFails += newRecord.GetTotalDeaths;

            // 是第一次通關、紀錄獲得的分數
            if(IsFirstTimeArrivalLevel(id, newRecord))
            {
                arrivalLevelId = id;
                points += newRecord.GetPoints;
            }

            // 增加或更新紀錄
            if (levelRecord.TryGetValue(id, out PlayerLevelData oldRecord))
                oldRecord.UpdateLevelData(newRecord);
            else
                levelRecord.Add(id, newRecord);
        }

        public void AddCoins(int coins)
        {
            if (coins < 0)
                return;

            this.coins += coins;
        }

        public void AddPlayTime(long playTime)
        {
            if (playTime < 0)
                return;

            this.totalPlayTime += playTime;
        }

        public void AddFailRecord()
        {
            totalFails++;
        }

        public bool AddPurchaseRecord(ItemBase itemBase, int itemCost)
        {
            if (HasEnoughCoins(itemCost) == false)
                return false;

            coins -= itemCost;
            if (purchaseRecord.TryGetValue(itemBase.GetId, out int numOfPurchases))
            {
                purchaseRecord[itemBase.GetId] = numOfPurchases + itemCost;
            }
            else
            {
                purchaseRecord.Add(itemBase.GetId, itemCost);
            }

            return true;
        }

        public bool IsFirstTimeArrivalLevel(string id, PlayerLevelData newRecord)
        {
            if (levelRecord.TryGetValue(id, out PlayerLevelData oldRecord))
            {
                if (newRecord.IsSuccess && oldRecord.IsSuccess == false)
                    return true;
                else
                    return false;
            }
            else
            {
                if (newRecord.IsSuccess)
                    return true;
                else
                    return false;
            }
        }

        public bool HasEnoughCoins(int itemCost)
        {
            //Debug.Log($"Current Coins:{coins} Item Cost:{itemCost}");
            return coins >= itemCost;
        }

        public override string ToString()
        {
            return $"ID:{id} Coins:{coins} ArrivalLevelId:{arrivalLevelId} TotalPlayTime:{totalPlayTime} TotalFail:{totalFails}";
        }
    }
}
