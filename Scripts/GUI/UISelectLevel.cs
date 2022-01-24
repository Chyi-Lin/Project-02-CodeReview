using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class UISelectLevel : MonoBehaviour
    {
        [SerializeField, Header("選擇世界介面"), Tooltip("從選擇介面取得世界清單")]
        protected UISelectWorld uiSelectWorld;

        [SerializeField, Header("按鈕 Prefab")]
        protected UIButtonHandler buttonPrefab;
        [SerializeField]
        protected Transform buttonsContent;

        [SerializeField, Header("建立的按鈕")]
        protected List<UIButtonHandler> buttonLists;

        [SerializeField, Header("關卡資料")]
        protected LevelList levelDatas;

        [SerializeField, Header("關卡轉跳 Feedbacks")]
        protected MMFeedbacks loadSceneFeedbacks;

        protected Dictionary<GameObject, LevelData> buttonItems = new Dictionary<GameObject, LevelData>();

        /// <summary>
        /// 設定關卡清單
        /// </summary>
        public LevelList SetLevelList
        {
            set
            {
                levelDatas = value;
            }
        }

        protected void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化介面
        /// </summary>
        protected virtual void Initialization()
        {
            buttonItems.Clear();

            int itemLength = levelDatas.GetLength();
            for (int i = 0; i < itemLength; i++)
            {
                UIButtonHandler touchButton;
                if (i > buttonLists.Count - 1)
                {
                    touchButton = Instantiate(buttonPrefab, buttonsContent);
                    buttonLists.Add(touchButton);
                }
                else
                {
                    touchButton = buttonLists[i];
                }
                InitializationDefaultButton((UIToggleButtonHandler)touchButton, levelDatas.GetData(i));

                // 加入至按鈕快取
                buttonItems.Add(touchButton.gameObject, levelDatas.GetData(i));
            }

            // 影藏未使用到的按鈕
            for (int i = itemLength; i < buttonLists.Count; i++)
            {
                buttonLists[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 初始化介面按鈕與事件
        /// </summary>
        /// <param name="button">按鈕處理器</param>
        /// <param name="levelData">關卡資料</param>
        protected void InitializationDefaultButton(UIToggleButtonHandler button, LevelData levelData)
        {
            // 設定按鈕圖片、文字
            button.isOn = true;
            button.SetText(levelData.GetLevelNumber.ToString());

            // 註冊更換事件
            button.SetButtonEvent(OnClick);
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

            int arrivalLevelByPlayer = 0;
            // 如果玩家已有完成關卡
            if (!string.IsNullOrEmpty(playerData.GetArrivalLevelId))
            {
                // 透過關卡ID 取得目前的關卡
                LevelData arrivalLevel = levelDatas.GetData(playerData.GetArrivalLevelId);

                // 針對選擇世界做的判斷
                if (arrivalLevel != null)
                    arrivalLevelByPlayer = arrivalLevel.GetLevelNumber;
                else
                    arrivalLevelByPlayer = uiSelectWorld.GetWorldDatas.GetDataByLevelId(playerData.GetArrivalLevelId).GetLevelNumber;
            }

            foreach (KeyValuePair<GameObject, LevelData> item in buttonItems)
            {
                UIButtonHandler buttonItem = item.Key.GetComponent<UIButtonHandler>();

                // 可以挑戰的關卡
                if (item.Value.GetLevelNumber <= arrivalLevelByPlayer + 1)
                    ReflashButton((UIToggleButtonHandler)buttonItem, true, item.Value.GetLevelNumber.ToString());
                // 無法挑戰的關卡
                else
                    ReflashButton((UIToggleButtonHandler)buttonItem, false, "");
            }
        }

        /// <summary>
        /// 刷新按鈕
        /// </summary>
        protected void ReflashButton(UIToggleButtonHandler button, bool isOn, string showText)
        {
            if (isOn)
            {
                // 可挑戰關卡，增加選關事件
                button.SetButtonEvent(OnClick);
            }
            else
            {
                // 不可挑戰關卡，移除選關事件
                button.SetButtonEvent(null);
            }

            // 更換按鈕介面
            button.isOn = isOn;
            button.SetText(showText);
        }

        /// <summary>
        /// 選擇關卡事件
        /// </summary>
        protected void OnClick(GameObject go)
        {
            // 設定選擇的關卡
            PlayerStats.Instance.PlayerSelectLevel = buttonItems[go.gameObject];
            // 切換場景
            loadSceneFeedbacks?.PlayFeedbacks(this.transform.position);
        }

        /// <summary>
        /// 顯示
        /// </summary>
        public void Show()
        {
            // 顯示物件
            gameObject.SetActive(true);

            // 初始化界面
            Initialization();

            // 顯示世界物件後初始化介面
            InitializationViewByPlayerStats();
        }
    }
}
