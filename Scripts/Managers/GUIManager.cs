using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    public class GUIManager : MonoBehaviour
    {
        /// Singleton Pattern
        public static GUIManager Instance;

        [Header("HUD")]
        public GameObject HUD;
        public UIProgressBar healthBar;
        public UIProgressBar stockBar;
        public Text textLevel;
        public Text textScore;

        [Header("UI")]
        public GameObject PauseScreen;
        public GameObject UpgradeScreen;
        public GameObject FailSettlementScreen;
        public GameObject WonSettlementScreen;
        public GameObject CompleteSettlementScreen;
        public GameObject YouAreAbountToLoseScreen;

        protected void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Initialization()
        {
            if(Instance == null)
                Instance = this;
                
            Time.timeScale = 1f;
        }

        /// <summary>
        /// 更新血條
        /// </summary>
        public void UpdateHealthBar(int currentHealth, int minHealth, int maxHealth)
        {
            if(healthBar != null)
            {
                healthBar.SetProgressBar(currentHealth, minHealth, maxHealth);
            }
        }

        /// <summary>
        /// 更新資源
        /// </summary>
        public void UpdateStock(int currentStock, int minStock, int maxStock)
        {
            if (healthBar != null)
            {
                stockBar.SetProgressBar(currentStock, minStock, maxStock);
            }
        }

        /// <summary>
        /// 更新分數
        /// </summary>
        public void UpdateScore(int score)
        {
            if(textScore != null)
            {
                textScore.text = score.ToString();
            }
        }

        /// <summary>
        /// 更新關卡
        /// </summary>
        /// <param name="level"></param>
        public void UpdateLevel(int level)
        {
            if(textLevel != null)
            {
                textLevel.text = level.ToString();
            }
        }

        /// <summary>
        /// 設定HUD Active
        /// </summary>
        public void SetHUDActive(bool state)
        {
            if (HUD != null)
            {
                HUD.SetActive(state);
            }
        }

        /// <summary>
        /// 暫停介面
        /// </summary>
        public void SetPauseScreen(bool state)
        {
            if (state)
            {
                Time.timeScale = 0f;
                if (PauseScreen != null)
                {
                    PauseScreen.SetActive(state);
                    LevelManager.Instance.playerCharacter.characterState = Character.CharacterConditions.Paused;
                }
            }
            else
            {
                Time.timeScale = 1f;
                if (PauseScreen != null)
                {
                    PauseScreen.SetActive(state);
                    LevelManager.Instance.playerCharacter.characterState = Character.CharacterConditions.ControlledMovement;
                }
            }
        }

        /// <summary>
        /// 升級介面
        /// </summary>
        public void SetUpgradeScreen(bool state)
        {	
            if (state)
            {
                Time.timeScale = 0f;
                if (UpgradeScreen != null)
                {
                    UpgradeScreen.SetActive(state);
                    LevelManager.Instance.playerCharacter.characterState = Character.CharacterConditions.UpgradeAbilities;
                }

            }
            else
            {
                Time.timeScale = 1f;
                if (UpgradeScreen != null)
                {
                    UpgradeScreen.SetActive(state);
                    LevelManager.Instance.playerCharacter.characterState = Character.CharacterConditions.ControlledMovement;
                }
            }
        }

        /// <summary>
        /// 顯示看廣告復活頁面
        /// </summary>
        public void SetYouAreAbountToLoseScreen(bool state)
        {
            if (state)
            {
                Time.timeScale = 0f;
                if (YouAreAbountToLoseScreen != null)
                {
                    YouAreAbountToLoseScreen.SetActive(state);
                }

                SetHUDActive(false);
            }
            else
            {
                Time.timeScale = 1f;
                if (YouAreAbountToLoseScreen != null)
                {
                    YouAreAbountToLoseScreen.SetActive(state);
                }

                SetHUDActive(true);
            }
        }

        /// <summary>
        /// 結算失敗介面
        /// </summary>
        public void SetFailSettlementScreen(bool state)
        {
            if (state)
            {
                if (FailSettlementScreen != null)
                {
                    UISettlementBase SettlementSplash = FailSettlementScreen.GetComponent<UISettlementBase>();
                    SettlementSplash.SetupSettlement(GameManager.Instance);
                    FailSettlementScreen.SetActive(state);
                }
            }
            else
            {
                if (FailSettlementScreen != null)
                {
                    FailSettlementScreen.SetActive(state);
                }
            }
        }

        /// <summary>
        /// 結算勝利介面
        /// </summary>
        public void SetWonSettlementScreen(bool state)
        {
            if (state)
            {
                if (WonSettlementScreen != null)
                {
                    UISettlementBase SettlementSplash = WonSettlementScreen.GetComponent<UISettlementBase>();
                    SettlementSplash.SetupSettlement(GameManager.Instance);
                    WonSettlementScreen.SetActive(state);
                }
            }
            else
            {
                if (WonSettlementScreen != null)
                {
                    WonSettlementScreen.SetActive(state);
                }
            }
        }

        /// <summary>
        /// 結算勝利介面
        /// </summary>
        public void SetCompleteSettlementScreen(bool state)
        {
            if (state)
            {
                if (CompleteSettlementScreen != null)
                {
                    UISettlementBase SettlementSplash = CompleteSettlementScreen.GetComponent<UISettlementBase>();
                    SettlementSplash.SetupSettlement(GameManager.Instance);
                    CompleteSettlementScreen.SetActive(state);
                }
            }
            else
            {
                if (CompleteSettlementScreen != null)
                {
                    CompleteSettlementScreen.SetActive(state);
                }
            }
        }
    }
}
