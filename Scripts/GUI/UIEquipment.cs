using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    /// <summary>
    /// 更換裝備介面，可更換裝備、購買裝備。
    /// 購買裝備的方式點擊按鈕一次為購買一點，假設裝備花費為三點，就必須點擊三次購買才可獲得裝備。
    /// </summary>
    public class UIEquipment : MonoBehaviour
    {
        [SerializeField, Header("金幣數量")]
        protected UICoin uiCoins;

        [SerializeField, Header("按鈕 Prefab")]
        protected UIButtonHandler buttonPrefab;
        [SerializeField]
        protected Transform buttonsContent;

        [SerializeField, Header("預設按鈕圖片")]
        protected Sprite buttonDefaultImage;
        [SerializeField]
        protected Color imageDefaultColor = Color.white;

        [SerializeField, Header("建立的按鈕")]
        protected List<UIButtonHandler> buttonLists;

        [SerializeField, Header("物品資料")]
        protected EquipmentList equipmentDatas;

        [SerializeField, Header("金幣不足事件")]
        protected UnityEvent OnNotEnoughCoinsEvent;

        protected UIButtonHandler selectedButton;
        protected Dictionary<GameObject, EquipmentData> buttonItems = new Dictionary<GameObject, EquipmentData>();

        protected void Awake()
        {
            InitializationView();
        }

        /// <summary>
        /// 初始化介面
        /// </summary>
        protected void InitializationView()
        {
            if(uiCoins)
                uiCoins.SetCoin("0");

            int itemLength = equipmentDatas.GetLength();
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
                InitializationDefaultButton(touchButton, equipmentDatas.GetData(i));

                // 加入至按鈕快取
                buttonItems.Add(touchButton.gameObject, equipmentDatas.GetData(i));
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
        /// <param name="equipmentData">裝備資料</param>
        protected void InitializationDefaultButton(UIButtonHandler button, EquipmentData equipmentData)
        {
            // 如果是免費的物品，可以直接使用
            if (equipmentData.GetCost <= 0)
            {
                // 設定按鈕圖片、文字
                button.SetImage(equipmentData.GetImage, Color.white);
                button.SetText("");

                // 註冊更換事件
                button.SetButtonEvent(OnSelect);
            }
            // 否則都是須購買的物品
            else
            {
                // 設定按鈕圖片、文字
                button.SetImage(buttonDefaultImage, imageDefaultColor);
                button.SetText(equipmentData.GetCost.ToString());

                // 註冊購買事件
                button.SetButtonEvent(OnBuy);
            }
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
            if (uiCoins)
                uiCoins.SetCoin(playerData.GetCoins.ToString());

            foreach (KeyValuePair<GameObject, EquipmentData> item in buttonItems)
            {
                // 檢查是否有購買紀錄
                if (!playerData.GetPurchaseRecord.TryGetValue(item.Value.GetId, out int numOfPurchased))
                    continue;

                UIButtonHandler buttonItem = item.Key.GetComponent<UIButtonHandler>();
                int itemCost = item.Value.GetCost - numOfPurchased;
                if (itemCost > 0)
                {
                    // 未擁有
                    // 註冊按鈕事件
                    buttonItem.SetButtonEvent(OnBuy);
                }
                else
                {
                    // 已擁有
                    // 註冊按鈕事件
                    buttonItem.SetButtonEvent(OnSelect);

                    // 檢查是否已裝備
                    if (playerData.EquipmentId == item.Value.GetId)
                    {
                        selectedButton = buttonItem;
                        selectedButton.GetComponent<UISelectButtonHandler>().isOn = true;
                    }
                    else
                    {
                        buttonItem.GetComponent<UISelectButtonHandler>().isOn = false;
                    }
                }

                ReflashButton(buttonItem, itemCost);
            }
        }

        /// <summary>
        /// 刷新按鈕
        /// </summary>
        protected void ReflashButton(UIButtonHandler button, int itemCost)
        {
            if (itemCost == 0)
            {
                // 獲得道具
                // 更換事件，從購買跟換成選擇
                button.SetButtonEvent(OnSelect);

                // 更換按鈕介面
                button.SetImage(buttonItems[button.gameObject].GetImage, Color.white);
                button.SetText("");
            }
            else
            {
                // 道具未支付完
                // 更換按鈕介面
                button.SetText(itemCost.ToString());
            }
        }

        /// <summary>
        /// 按鈕點擊事件
        /// </summary>
        protected void OnBuy(GameObject go)
        {
            //Debug.Log("OnBuy", go);

            if (PlayerStats.Instance == null)
                return;
            PlayerData playerData = PlayerStats.Instance.GetPlayerData();

            // 檢查玩家是否有足夠金幣購買一點
            if (playerData.HasEnoughCoins(1) == false)
            {
                OnNotEnoughCoinsEvent?.Invoke();
                return;
            }

            // 取得物品資料檔
            EquipmentData equipmentData = buttonItems[go];

            // 增加購買紀錄，一次只能購買一點
            playerData.AddPurchaseRecord(equipmentData, 1);
            PlayerStats.Instance.Save();

            // 刷新玩家目前金幣
            if (uiCoins)
                uiCoins.SetCoin(playerData.GetCoins.ToString(), true);
            // 當前物品還需要的金額
            int currentCost = equipmentData.GetCost - playerData.GetPurchaseRecord[equipmentData.GetId];
            // 刷新按鈕
            ReflashButton(go.GetComponent<UIButtonHandler>(), currentCost);
        }

        /// <summary>
        /// 按鈕選擇事件
        /// </summary>
        protected void OnSelect(GameObject go)
        {
            //Debug.Log("OnSelect", go);

            // 取得玩家目前的裝備
            if (PlayerStats.Instance == null)
                return;
            string equipmentId = PlayerStats.Instance.GetPlayerData().EquipmentId;

            // 取得物品資料檔
            EquipmentData equipmentData = buttonItems[go];
            if (equipmentData.GetId == equipmentId)
                return;

            // 選取
            PlayerStats.Instance.GetPlayerData().EquipmentId = equipmentData.GetId;
            PlayerStats.Instance.Save();
            selectedButton.GetComponent<UISelectButtonHandler>().isOn = false;

            selectedButton = go.GetComponent<UIButtonHandler>();
            selectedButton.GetComponent<UISelectButtonHandler>().isOn = true;
        }

        /// <summary>
        /// 獲得一枚金幣
        /// </summary>
        public void OnAddCoin()
        {
            if (PlayerStats.Instance == null)
                return;
            PlayerData playerData = PlayerStats.Instance.GetPlayerData();
            playerData.AddCoins(1);
            PlayerStats.Instance.Save();

            // 刷新玩家目前金幣
            if (uiCoins)
                uiCoins.SetCoin(playerData.GetCoins.ToString(), true);
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
        }
    }
}