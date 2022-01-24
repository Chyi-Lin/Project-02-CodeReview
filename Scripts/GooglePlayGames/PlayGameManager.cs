using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualFramework
{
    /// <summary>
    /// Google Play Games
    /// </summary>
    public class PlayGameManager : MonoBehaviour
    {
        public static PlayGameManager instance;

        [SerializeField]
        private bool authenticating = false;

        // what is the highest score we have posted to the leaderboard?
        private int mHighestPostedScore = 0;
        private int mApproximateCount = 0;
        private int mMaxRowCount = 100;

        public bool Authentication { get { return authenticating; } }

        public bool Authenticated
        {
            get { return Social.Active.localUser.authenticated; }
        }

        protected void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            Initialization();
        }

        protected virtual void Initialization()
        {
            Authenticate();
        }

        public void ReportAllProgress()
        {
            PostToLeaderboard();
        }

        protected void Authenticate()
        {
            if (Authenticated || authenticating)
            {
                Debug.LogWarning("Ignoring repeated call to Authenticate().");
                return;
            }

            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .EnableSavedGames()
                .Build();
            PlayGamesPlatform.InitializeInstance(config);

            // Activate the Play Games platform. This will make it the default
            // implementation of Social.Active
            PlayGamesPlatform.Activate();

            // Set the default leaderboard for the leaderboards UI
            ((PlayGamesPlatform)Social.Active).SetDefaultLeaderboardForUI(PlayGameIds.LeaderboardId);

            // Sign in to Google Play Games
            authenticating = true;
            Social.localUser.Authenticate((bool success) =>
            {
                authenticating = false;
                if (success)
                {
                    // if we signed in successfully, load data from cloud
                    Debug.Log("Login successful!");
                    // 刷新玩家最高分數
                    PlayerHeightPostedScore(null);
                }
                else
                {
                    // no need to show error message (error messages are shown automatically
                    // by plugin)
                    Debug.LogWarning("Failed to sign in with Google Play Games.");
                }
            });
        }

        protected void SignOut()
        {
            ((PlayGamesPlatform)Social.Active).SignOut();
        }

        public void OnSignIn()
        {
            if (authenticating)
                return;

            if (Authenticated)
            {
                SignOut();
            }
            else
            {
                Authenticate();
            }
        }

        public void ShowLeaderboardUI(UnityAction<LeaderboardScoreData> OnLoadScoresEvent)
        {
            if (Authenticated)
            {
                //Social.ShowLeaderboardUI();

                PlayGamesPlatform.Instance.LoadScores(
                    PlayGameIds.LeaderboardId,
                    LeaderboardStart.TopScores,
                    mMaxRowCount >= mApproximateCount? mApproximateCount: mMaxRowCount,
                    LeaderboardCollection.Public,
                    LeaderboardTimeSpan.AllTime,
                    (data) => OnLoadScoresEvent?.Invoke(data));
            }
            else
            {
                OnLoadScoresEvent?.Invoke(null);
            }
        }

        /// <summary>
        /// 取得排行中玩家的最高分數
        /// </summary>
        public void PlayerHeightPostedScore(UnityAction<LeaderboardScoreData> OnLoadScoresEvent)
        {
            if (Authenticated)
            {
                PlayGamesPlatform.Instance.LoadScores(
                    PlayGameIds.LeaderboardId,
                    LeaderboardStart.PlayerCentered,
                    1,
                    LeaderboardCollection.Public,
                    LeaderboardTimeSpan.AllTime,
                    (data) =>
                    {
                        if (data.PlayerScore != null)
                        {
                            mHighestPostedScore = (int)data.PlayerScore.value;
                            mApproximateCount = (int)data.ApproximateCount;
                            OnLoadScoresEvent?.Invoke(data);
                        }
                    });
            }
        }

        /// <summary>
        /// 加入分數至排行榜
        /// </summary>
        protected void PostToLeaderboard()
        {
            int score = PlayerStats.Instance.GetPlayerData().GetPoints;

            if (Authenticated && score > mHighestPostedScore)
            {
                // post score to the leaderboard
                Social.ReportScore(score, PlayGameIds.LeaderboardId, (bool success) => { });
                mHighestPostedScore = score;
            }
            else
            {
                Debug.LogWarning("Not reporting score, auth = " + Authenticated + " " +
                                 score + " <= " + mHighestPostedScore);
            }

        }
    }
}
