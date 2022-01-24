using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class LevelManager : MonoBehaviour
    {
        /// Singleton Pattern
        public static LevelManager Instance;

        [Header("Player")]
        public Character playerPrefab;
        public Transform playerSpawnPoint;

        [HideInInspector]
        public Character playerCharacter;

        [Header("Player MoveController")]
        public MovementController movementController;

        [Header("Level")]
        public LevelData levelData;
        public Transform levelSpawnPoint;

        [Header("LevelWonFeedbacks")]
        public LevelList levelList;
        public MMFeedbacks levelWonFeedbacks;
        public MMFeedbacks levelCompleteFeedbacks;
        public MMFeedbacks levelFailFeedbacks;

        protected LevelDetail currentLevelDetail;

        protected List<Character> enemyList = new List<Character>();
        protected int amount;

        protected virtual void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialization()
        {
            if (Instance == null)
                Instance = this;
        }

        /// <summary>
        /// 新關卡
        /// </summary>
        public virtual void NewLevel()
        {
            SpawnLevel();
            SpawnPlayer();
            SetupSetllementEvent();
        }

        /// <summary>
        /// 下一關
        /// </summary>
        public virtual bool NextLevel()
        {
            int nextLevelNum = levelData.GetLevelNumber + 1;
            LevelData nextLevelData = levelList.GetDataByNumber(nextLevelNum);

            if (nextLevelData != null)
            {
                // 有下一關
                if (PlayerStats.Instance != null)
                    PlayerStats.Instance.PlayerSelectLevel = nextLevelData;
                else
                    Debug.LogWarning("Player stats is not found!");
                return true;
            }
            else
            {
                // 沒有下一關
                return false;
            }
        }

        /// <summary>
        /// 是否有下一關
        /// </summary>
        /// <returns></returns>
        public virtual bool HasNextLevel()
        {
            int nextLevelNum = levelData.GetLevelNumber + 1;
            LevelData nextLevelData = levelList.GetDataByNumber(nextLevelNum);

            return nextLevelData != null;
        }

        /// <summary>
        /// 生產玩家
        /// </summary>
        protected virtual void SpawnPlayer()
        {
            if (playerPrefab == null)
                return;

            playerCharacter = Instantiate(playerPrefab);
            movementController = playerCharacter.GetComponent<MovementController>();

            if (levelData != null)
            {
                // 以關卡資訊設定玩家出生位置
                if (movementController != null)
                    movementController.SetPosition(levelData.GetLevelPlayerSpawnPosition);

                // 設定玩家模型顏色
                ChangeSelfColor selfColor = playerCharacter.GetComponent<ChangeSelfColor>();
                if (selfColor != null)
                    selfColor.SetColor(levelData.GetLevelPlayerColor);

                // 設定玩家染色顏色
                ChangeTargetColor targetColor = playerCharacter.GetComponent<ChangeTargetColor>();
                if (targetColor != null)
                    targetColor.SetColor(levelData.GetLevelPlayerColor);
            }
            else
            {
                // 以玩家出生點來設定玩家出生位置
                if (movementController != null)
                    movementController.SetPosition(playerSpawnPoint.position);
            }

            playerCharacter.characterState = Character.CharacterConditions.ControlledMovement;
        }

        /// <summary>
        /// 復活玩家
        /// </summary>
        public virtual void RecoveryPlayer()
        {
            if(playerCharacter != null)
            {
                if (levelData != null)
                {
                    // 以關卡資訊設定玩家出生位置
                    movementController.SetPosition(levelData.GetLevelPlayerSpawnPosition);
                }
                else
                {
                    // 以玩家出生點來設定玩家出生位置
                    movementController.SetPosition(playerSpawnPoint.position);
                }

                Bouncy characterBouny = playerCharacter.GetComponent<Bouncy>();
                if (characterBouny != null)
                    characterBouny.ResetGravity();

                CharacterHealth characterHealth = playerCharacter.GetComponent<CharacterHealth>();
                if(characterHealth != null)
                    characterHealth.OnRecovery();
            }
        }

        /// <summary>
        /// 產生關卡
        /// </summary>
        protected virtual void SpawnLevel()
        {
            if(PlayerStats.Instance != null)
            {
                levelData = PlayerStats.Instance.PlayerSelectLevel;
#if UNITY_EDITOR
                Debug.Log($"Level select: {levelData.GetLevelNumber}");
#endif
            }

            if(levelData == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Level data is null!");
#endif
                return;
            }

            if(levelSpawnPoint == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Level spawn point is null!");
#endif
                return;
            }

            GameObject levelObject = Instantiate(levelData.GetLevelPrefab, levelSpawnPoint);
            currentLevelDetail = levelObject.GetComponent<LevelDetail>();

            // 設定 HUD 顯示目前關卡
            GUIManager.Instance.textLevel.text = levelData.GetLevelNumber.ToString();
        }

        /// <summary>
        /// 設定結算條件
        /// </summary>
        protected virtual void SetupSetllementEvent()
        {
            // 成功事件註冊
            if(currentLevelDetail != null)
            {
                currentLevelDetail.OnLevelCompleteEvent.AddListener(LevelWonEvent);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("Setup won setllement event: Level data is null!");
#endif
            }

            // 失敗事件註冊
            if(playerCharacter != null)
            {
                CharacterHealth health = playerCharacter.GetComponent<CharacterHealth>();
                health.OnDeath.AddListener(LevelFailEvent);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("Setup fail setllement event: Player Character is null!");
#endif
            }

        }

        /// <summary>
        /// 關卡成功事件
        /// </summary>
        protected virtual void LevelWonEvent()
        {
            // 移除失敗條件
            CharacterHealth health = playerCharacter.GetComponent<CharacterHealth>();
            if (health != null)
                health.OnDeath.RemoveListener(LevelFailEvent);

            // 影藏玩家
            playerCharacter.characterState = Character.CharacterConditions.Hide;
            playerCharacter.gameObject.SetActive(false);

            // 成功結算
            GameManager.Instance.Setllement(true);

            // 顯示成功結算介面
            GUIManager.Instance.SetHUDActive(false);
            if (HasNextLevel())
                levelWonFeedbacks?.PlayFeedbacks(this.transform.position);
            else
                levelCompleteFeedbacks?.PlayFeedbacks(this.transform.position);
        }

        /// <summary>
        /// 關卡失敗事件，會檢查玩家有沒有復活機會
        /// </summary>
        /// <param name="character">玩家</param>
        protected virtual void LevelFailEvent(Character character)
        {
            // 紀錄玩家死亡次數
            GameManager.Instance.totalDeaths++;

            // 檢查是否有復活機會
            if (GameManager.Instance.Recovery())
                return;

            CharacterHealth health = playerCharacter.GetComponent<CharacterHealth>();
            if (health != null)
                health.OnDeath.RemoveListener(LevelFailEvent);

            LevelFail();
        }

        /// <summary>
        /// 關卡失敗
        /// </summary>
        public void LevelFail()
        {
            // 失敗結算
            GameManager.Instance.Setllement(false);

            // 顯示失敗結算介面
            GUIManager.Instance.SetHUDActive(false);
            levelFailFeedbacks?.PlayFeedbacks(this.transform.position);
        }

        /// <summary>
        /// 敵人死亡事件
        /// </summary>
        protected virtual void EnemyDeathEvent(Character character)
        {
            GameManager.Instance.points++;

            enemyList.Remove(character);
            if (enemyList.Count == 0)
            {
                //GameManager.Instance.round++;
            }

            CharacterHealth health = character.GetComponent<CharacterHealth>();
            health.OnDeath.RemoveListener(EnemyDeathEvent);
        }
    }
}