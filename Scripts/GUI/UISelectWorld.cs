using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualFramework
{
    [System.Serializable]
    public class PassLevelListEvent : UnityEvent<LevelList> { }

    public class UISelectWorld : MonoBehaviour
    {
        [SerializeField, Header("按鈕 Prefab")]
        protected UIButtonHandler buttonPrefab;
        [SerializeField]
        protected Transform buttonsContent;

        [SerializeField, Header("建立的按鈕")]
        protected List<UIButtonHandler> buttonLists;

        [SerializeField, Header("世界資料")]
        protected WorldList worldDatas;

        [SerializeField, Header("選擇世界事件")]
        protected PassLevelListEvent OnClickWorldEvent;

        protected Dictionary<GameObject, LevelList> buttonItems = new Dictionary<GameObject, LevelList>();

        /// <summary>
        /// 取得世界資料
        /// </summary>
        public WorldList GetWorldDatas { get { return worldDatas; } }

        protected void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化介面
        /// </summary>
        protected virtual void Initialization()
        {
            int itemLength = worldDatas.GetLength();
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
                InitializationDefaultButton((UIToggleButtonHandler)touchButton, worldDatas.GetData(i));

                // 加入至按鈕快取
                buttonItems.Add(touchButton.gameObject, worldDatas.GetData(i));
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
        /// <param name="levelList">關卡清單資料</param>
        protected void InitializationDefaultButton(UIToggleButtonHandler button, LevelList levelList)
        {
            // 設定按鈕圖片、文字
            button.isOn = true;
            button.SetText($"<size=32>World</size>\n{levelList.GetListNumber}");

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

            string arrivalLevelByPlayer;
            // 如果玩家已有完成關卡，取得完成關卡的 ID
            if (!string.IsNullOrEmpty(playerData.GetArrivalLevelId))
                arrivalLevelByPlayer = playerData.GetArrivalLevelId;
            // 否則取得第一個世界的第一個關卡 ID
            else
                arrivalLevelByPlayer = worldDatas.GetData(0).GetData(0).GetId;



            foreach (KeyValuePair<GameObject, LevelList> item in buttonItems)
            {
                UIButtonHandler buttonItem = item.Key.GetComponent<UIButtonHandler>();

                // 透過關卡ID 取得目前的關卡清單
                int levelNum = worldDatas.GetDataByLevelId(arrivalLevelByPlayer).GetLevelNumber;

                // 可以挑戰的世界
                if (item.Value.CheckSelectLevelList(levelNum))
                    ReflashButton((UIToggleButtonHandler)buttonItem, true, $"<size=32>World</size>\n{item.Value.GetListNumber}");
                // 無法挑戰的世界
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
                // 可挑戰世界，增加選關事件
                button.SetButtonEvent(OnClick);
            }
            else
            {
                // 不可挑戰世界，移除選關事件
                button.SetButtonEvent(null);
            }

            // 更換按鈕介面
            button.isOn = isOn;
            button.SetText(showText);
        }

        /// <summary>
        /// 選擇世界事件
        /// </summary>
        protected void OnClick(GameObject go)
        {
            // 選擇世界 UnityEvent
            OnClickWorldEvent.Invoke(buttonItems[go.gameObject]);
        }

        /// <summary>
        /// 顯示
        /// </summary>
        public void Show()
        {
            // 顯示物件
            gameObject.SetActive(true);

            // 顯示世界物件後初始化介面
            InitializationViewByPlayerStats();
        }
    }
}
