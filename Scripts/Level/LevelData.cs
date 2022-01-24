using MoreMountains.Tools;
using UnityEngine;

namespace HyperCasualFramework
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "HyperCasual/Level/Create Level Data", order = 50)]
    public class LevelData : ScriptableObject
    {
        [SerializeField, Header("關卡ID(快速設定)"), Tooltip("欄位保留")]
        protected string id;

        [SerializeField, Header("關卡編號"), Tooltip("目前的關卡是第幾關，用於顯示於介面上")]
        protected int levelNumber;

        [SerializeField, Header("關卡資訊(快速設定)"), Tooltip("關卡物件需要有 LevelDetail Component 才可以設定")]
        protected LevelDetail levelDetail;

        [SerializeField, Header("關卡物件")]
        protected GameObject levelGOPrefab;

        [SerializeField, Header("關卡玩家顏色")]
        protected Color levelPlayerColor = new Color32(50, 50, 50, 255);

        [SerializeField, Header("關卡首次通關獎勵")]
        protected int rewardCoins = 3;

        [Header("快速設定")]
        [MMInspectorButton("QuickSetup", order = 0)]
        public bool autoSort;

        /// <summary>
        /// 取得關卡ID
        /// </summary>
        public string GetId { get { return id; } }

        /// <summary>
        /// 取的關卡編號
        /// </summary>
        public int GetLevelNumber { get { return levelNumber; } }

        /// <summary>
        /// 取得關卡 Prefab
        /// </summary>
        public GameObject GetLevelPrefab { get { return levelDetail.gameObject; } }

        /// <summary>
        /// 取得關卡玩家出生位置
        /// </summary>
        public Vector3 GetLevelPlayerSpawnPosition { get { return levelDetail.GetPlaySpawnTile; } }

        /// <summary>
        /// 取的關卡玩家顏色
        /// </summary>
        public Color GetLevelPlayerColor { get { return levelPlayerColor; } }

        /// <summary>
        /// 取得關卡首次通關獎勵
        /// </summary>
        public int GetLevelRewardCoins { get { return rewardCoins; } }

#if UNITY_EDITOR

        /// <summary>
        /// 快速設定
        /// </summary>
        public void QuickSetup()
        {
            Object[] selectObject = UnityEditor.Selection.objects;
            MMDebug.DebugLogTime($"選擇{selectObject.Length}個關卡物件！");

            int success = 0;
            int fail = 0;
            foreach (var item in selectObject)
            {
                LevelData levelData = (item as LevelData);

                if (levelData)
                {
                    // 檢查關卡編號設定錯誤
                    if (levelData.levelNumber > 0)
                    {
                        levelData.id = (levelData.levelNumber - 1).ToString("000");
                    }
                    else
                    {
                        MMDebug.DebugLogTime($"失敗資料檔名：{item.name}　原因：LevelNumber 小於零！", "#FF2800");
                        fail++;
                        continue;
                    }

                    // 檢查關卡Prefab 有無設定
                    if (levelData.levelGOPrefab)
                    {
                        levelData.levelDetail = levelData.levelGOPrefab.GetComponent<LevelDetail>();
                    }
                    else
                    {
                        MMDebug.DebugLogTime($"失敗資料檔名：{item.name}　原因：Prefab 未設定！", "#FF2800");
                        fail++;
                        continue;
                    }

                    UnityEditor.EditorUtility.SetDirty(levelData);
                    success++;
                }
                else
                {
                    MMDebug.DebugLogTime($"失敗資料檔名：{item.name}　原因：選取到其它物件！", "#FF2800");
                    fail++;
                }
            }

            MMDebug.DebugLogTime($"設定成功{success}個，失敗{fail}個！");
            MMDebug.DebugLogTime("快速設定完成！");
        }

#endif

    }
}
