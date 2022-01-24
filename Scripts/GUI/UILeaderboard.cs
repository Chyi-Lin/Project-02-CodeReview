using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    public class UILeaderboard : MonoBehaviour
    {
        [SerializeField, Header("累積分數")]
        protected Text TextPoints;

        [SerializeField, Header("關卡資料")]
        protected LevelList levelData;

        [SerializeField, Header("排行榜提示")]
        protected Text TextTip;

        [SerializeField, Header("排行榜")]
        protected ScrollRect recordScrollView;

        [SerializeField, Header("玩家項目")]
        protected UIRecordItem recordItemMe;

        [SerializeField, Header("排行榜紀錄項目")]
        protected List<UIRecordItem> recordItemList;

        [SerializeField, Header("紀錄項目 Prefab")]
        protected UIRecordItem recordItemPrefab;

        protected int recordItemMeIndex;
        protected bool isShowLeaderboard;

        protected void Awake()
        {
            Initialization();
            InitializationView();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Initialization()
        {
            isShowLeaderboard = false;
        }

        /// <summary>
        /// 初始化介面
        /// </summary>
        protected void InitializationView()
        {
            TextTip.text = "";

            // 影藏所有紀錄
            for (int i = 0; i < recordItemList.Count; i++)
                recordItemList[i].gameObject.SetActive(false);
        }

        /// <summary>
        /// 初始化界面資料，由玩家資訊來調整
        /// </summary>
        protected void InitializationViewByPlayerStats()
        {
            // 取得玩家資料
            if (PlayerStats.Instance == null)
                return;
            PlayerData playerData = PlayerStats.Instance.GetPlayerData();

            // 顯示玩家分數
            TextPoints.text = playerData.GetPoints.ToString();
            if (recordItemMe)
                recordItemMe.SetRecordItem(-1, "ME", playerData.GetPoints.ToString());
        }

        /// <summary>
        /// 更新排行榜
        /// </summary>
        protected void ReflashLeaderboard(GooglePlayGames.BasicApi.LeaderboardScoreData data)
        {
            if(data == null)
            {
                TextTip.text = "排行榜讀取失敗";
                return;
            }

            if (data.PlayerScore == null)
            {
                TextTip.text = "排行榜上沒有使用者";
                return;
            }

            //string mStatus;
            //mStatus = "Leaderboard data valid: " + data.Valid;
            //mStatus += "\n approx:" + data.ApproximateCount + " have " + data.Scores.Length;
            //TextTip.text = mStatus;

            List<string> userIds = new List<string>();
            foreach (IScore score in data.Scores)
            {
                userIds.Add(score.userID);
            }

            Social.LoadUsers(userIds.ToArray(), (users) =>
            {
                // 影藏所有紀錄
                for (int i = 0; i < recordItemList.Count; i++)
                    recordItemList[i].gameObject.SetActive(false);

                // 讀取紀錄並顯示
                for (int i = 0; i < data.Scores.Length; i++)
                {
                    IScore scoreData = data.Scores[i];
                    IUserProfile user = FindUser(users, scoreData.userID);

                    UIRecordItem recordItem = null;
                    if (i < recordItemList.Count - 1)
                    {
                        recordItem = recordItemList[i];
                        recordItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        recordItem = Instantiate(recordItemPrefab, recordScrollView.content);
                        recordItemList.Add(recordItem);
                    }

                    if (Social.localUser.userName == user.userName)
                        recordItem.SetRecordItem(scoreData.rank, "ME", scoreData.formattedValue);
                    else
                        recordItem.SetRecordItem(scoreData.rank, user.userName, scoreData.formattedValue);
                }

                TextTip.text = "";
                isShowLeaderboard = true;
            });
        }

        /// <summary>
        /// 更新玩家排名
        /// </summary>
        protected void ReflashPlayerScore(GooglePlayGames.BasicApi.LeaderboardScoreData data)
        {
            if (data == null)
            {
                TextTip.text = "排行榜讀取失敗";
                return;
            }

            if (data.PlayerScore == null)
            {
                TextTip.text = "排行榜上沒有玩家排名";
                return;
            }

            if(recordItemMe)
                recordItemMe.SetRecordItem(data.PlayerScore.rank, "ME", data.PlayerScore.formattedValue);
        }

        /// <summary>
        /// 尋找用戶
        /// </summary>
        protected IUserProfile FindUser(IUserProfile[] users, string userID)
        {
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].id == userID)
                    return users[i];
            }

            return null;
        }

        /// <summary>
        /// 顯示
        /// </summary>
        public void Show()
        {
            // 顯示物件
            gameObject.SetActive(true);

            // 顯示裝備物件後初始化介面
            InitializationViewByPlayerStats();

            // 顯示排行榜
            if (PlayGameManager.instance != null)
            {
                PlayGameManager.instance.ShowLeaderboardUI(ReflashLeaderboard);
                PlayGameManager.instance.PlayerHeightPostedScore(ReflashPlayerScore);
                TextTip.text = "讀取世界紀錄中";
            }
            else
            {
                TextTip.text = "讀取失敗";
            }
        }
    }
}
