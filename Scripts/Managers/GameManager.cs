using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class GameManager : MMPersistentSingleton<GameManager>,
                               MMEventListener<TopDownEnginePointEvent>
    {
        public int targetFrameRate = 300;

        [Header("Record")]
        public int points;
        public int coins;
        public float time;
        public int totalDeaths;
        public int recoverys;

        protected float gameStartTime;
        protected float gameOverTime;
        protected bool isPause = false;

        protected override void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialization()
        {
            Application.targetFrameRate = targetFrameRate;
        }

        protected virtual void Start()
        {
            NewGame();
        }

        /// <summary>
        /// 新遊戲
        /// </summary>
        public virtual void NewGame()
        {
            points = 0;
            coins = 0;
            recoverys = 0;
            totalDeaths = 0;
            gameStartTime = Time.time;
            GUIManager.Instance.UpdateScore(points);
            LevelManager.Instance.NewLevel();
        }

        /// <summary>
        /// 暫停
        /// </summary>
        public virtual void Pause()
        {
            isPause = !isPause;
            GUIManager.Instance.SetHUDActive(!isPause);
            GUIManager.Instance.SetPauseScreen(isPause);
        }

        /// <summary>
        /// 結算
        /// </summary>
        public virtual void Setllement(bool isWon)
        {
            gameOverTime = Time.time;
            time = gameOverTime - gameStartTime;

            // 建立關卡紀錄、分數、最佳時間、是否獲勝
            PlayerLevelData levelData = new PlayerLevelData(points, time, totalDeaths, isWon);

            // 獲勝獲得三枚硬幣
            if (PlayerStats.Instance != null && isWon)
            {
                // 第一次通關獲得獎勵金幣
                if (PlayerStats.Instance.GetPlayerData().IsFirstTimeArrivalLevel(LevelManager.Instance.levelData.GetId, levelData))
                {
                    coins += LevelManager.Instance.levelData.GetLevelRewardCoins;
                    PlayerStats.Instance.GetPlayerData().AddCoins(coins);
                }
            }
            
            // 增加關卡紀錄、挑戰時間
            PlayerStats.Instance.GetPlayerData().UpdatePlayerLevelData(LevelManager.Instance.levelData.GetId, time, levelData);

            // 保存紀錄
            PlayerStats.Instance.Save();

            // Google Play Games 更新所有進度
            if (PlayGameManager.instance != null && isWon)
            {
                PlayGameManager.instance.ReportAllProgress();
            }
        }

        /// <summary>
        /// 復活
        /// </summary>
        public virtual bool Recovery()
        {
            if (recoverys > 0)
                return false;

            GUIManager.Instance.SetYouAreAbountToLoseScreen(true);
            recoverys++;
            return true;
        }

        /// <summary>
        /// 分數事件
        /// </summary>
        public void OnMMEvent(TopDownEnginePointEvent eventType)
        {
            switch (eventType.PointsMethod)
            {
                case PointsMethods.Add:
                    points += eventType.Points;
                    GUIManager.Instance.UpdateScore(points);
                    break;
                case PointsMethods.Set:
                    points += eventType.Points;
                    GUIManager.Instance.UpdateScore(points);
                    break;
                default:
                    break;
            }
        }

        protected void OnEnable()
        {
            this.MMEventStartListening<TopDownEnginePointEvent>();
        }

        protected void OnDisable()
        {
            this.MMEventStopListening<TopDownEnginePointEvent>();
        } 
    }
}
