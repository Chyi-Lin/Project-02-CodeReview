using UnityEngine;

namespace HyperCasualFramework
{
    /// <summary>
    /// 玩家關卡紀錄
    /// </summary>
    [System.Serializable]
    public class PlayerLevelData
    {
        /// <summary>
        /// 關卡分數
        /// </summary>
        [SerializeField]
        private int points;

        /// <summary>
        /// 最佳時間
        /// </summary>
        [SerializeField]
        private float bestTime;

        /// <summary>
        /// 關卡總死亡數
        /// </summary>
        [SerializeField]
        private int totalDeaths;

        /// <summary>
        /// 關卡是否通關
        /// </summary>
        [SerializeField]
        private bool isSuccess;

        /// <summary>
        /// 設定關卡紀錄，通關才會保存分數、最佳時間
        /// </summary>
        public PlayerLevelData(int points, float bestTime, int totalDeaths, bool isSuccess)
        {
            // 有通關才保存分數、最佳時間
            if (isSuccess)
            {
                this.points = points;
                this.bestTime = bestTime;
            }

            this.totalDeaths = totalDeaths;
            this.isSuccess = isSuccess;
        }

        /// <summary>
        /// 更新關卡紀錄
        /// </summary>
        public void UpdateLevelData(PlayerLevelData newLevelData)
        {
            // 有紀錄未通關，玩家這場通關則直接保存分數、最佳時間
            if (!isSuccess && newLevelData.isSuccess)
            {
                this.points = newLevelData.points;
                this.bestTime = newLevelData.bestTime;
                this.isSuccess = newLevelData.isSuccess;
            }
            // 有紀錄且通關，玩家最新紀錄比較原始紀錄的分數、最佳時間
            else if(newLevelData.isSuccess)
            {
                this.points = (this.points >= newLevelData.points) ? this.points : newLevelData.points;
                this.bestTime = (this.bestTime < newLevelData.bestTime) ? this.bestTime : newLevelData.bestTime;
            }
            // 有紀錄未通關，不做紀錄保存
            else
            {
                
            }
            
            this.totalDeaths += newLevelData.totalDeaths;
        }

        /// <summary>
        /// 最高分數
        /// </summary>
        public int GetPoints { get { return this.points; } }

        /// <summary>
        /// 最佳時間
        /// </summary>
        public float GetBestTime { get { return this.bestTime; } }

        /// <summary>
        /// 累計死亡數
        /// </summary>
        public int GetTotalDeaths { get { return this.totalDeaths; } }

        /// <summary>
        /// 是否勝利
        /// </summary>
        public bool IsSuccess { get { return this.isSuccess; } }

        public override string ToString()
        {
            return $"Points:{this.points} BestTime:{this.bestTime} TotalDeaths:{this.totalDeaths} IsSuccess:{this.isSuccess}";
        }
    }
}
