using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualFramework
{
    [System.Serializable]
    public class PassCompleteEvent : UnityEvent { }

    public class LevelDetail : MonoBehaviour
    {
        [SerializeField, Header("玩家出生地板")]
        protected TileBase playSpawnTile;

        [SerializeField, Header("所有地板")]
        protected int totalTile;

        [SerializeField, Header("目標地板")]
        protected int targetTotalTile;
        protected int currentTargetTile = 0;

        [SerializeField, Header("關卡地板資料")]
        protected List<LevelTileData> levelTileDatas = new List<LevelTileData>();

        protected Dictionary<TileType, List<TileBase>> tileList = new Dictionary<TileType, List<TileBase>>();
        protected Dictionary<TileType, List<TileBase>> completeTileList = new Dictionary<TileType, List<TileBase>>();

        public PassCompleteEvent OnLevelCompleteEvent;

        /// <summary>
        /// 取得玩家出生地板
        /// </summary>
        public Vector3 GetPlaySpawnTile
        {
            get
            {
                Vector3 spawnPos = new Vector3(0f, 8f, 0f);
                if (playSpawnTile != null)
                {
                    spawnPos.x = playSpawnTile.transform.position.x;
                    spawnPos.z = playSpawnTile.transform.position.z;
                }
                return spawnPos;
            }
        }

        /// <summary>
        /// 取得所有地板數量
        /// </summary>
        public int GetTotalTile { get { return totalTile; } }

        /// <summary>
        /// 取得關卡獲勝的目標地板數量
        /// </summary>
        public int GetLevelTotalTile { get { return targetTotalTile; } }

        protected void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Initialization()
        {
            for (int i = 0; i < levelTileDatas.Count; i++)
            {
                AddTile(levelTileDatas[i].type, levelTileDatas[i].tiles);
                CheckLevelTargetTile(levelTileDatas[i].type, levelTileDatas[i].tiles.Count);
            }
        }

        /// <summary>
        /// 增加地板資訊，透過地形產生器來保存地板資訊
        /// </summary>
        /// <param name="type">地板種類</param>
        /// <param name="tileObject">地板物件</param>
        public void AddLevelTileData(TileType type, TileBase tileObject)
        {
            LevelTileData tiledata;
            if ((tiledata = levelTileDatas.Find(data => data.type == type)) != null)
            {
                tiledata.tiles.Add(tileObject);
            }
            else
            {
                tiledata = new LevelTileData(type);
                tiledata.tiles.Add(tileObject);
                levelTileDatas.Add(tiledata);
            }
        }

        /// <summary>
        /// 加入地板快取清單
        /// </summary>
        protected void AddTile(TileType type, List<TileBase> tileObjects)
        {
            List<TileBase> objectList;
            if (tileList.TryGetValue(type, out objectList))
            {
                objectList.AddRange(tileObjects);
            }
            else
            {
                objectList = new List<TileBase>(tileObjects);
                tileList.Add(type, objectList);
            }
        }

        /// <summary>
        /// 檢查是否是關卡目標地板
        /// </summary>
        protected void CheckLevelTargetTile(TileType type, int amount)
        {
            switch (type)
            {
                case TileType.Normal:
                case TileType.Override:
                    totalTile += amount;
                    targetTotalTile += amount;
                    break;
                default:
                    totalTile += amount;
                    break;
            }
        }

        /// <summary>
        /// 設定地板事件
        /// </summary>
        protected void SetupTileEvent()
        {
            List<TileBase> allObjectList = new List<TileBase>();
            List<TileBase> objectListTmp;
            
            // 取得關卡需要完成的地板
            if(tileList.TryGetValue(TileType.Normal, out objectListTmp))
                allObjectList.AddRange(objectListTmp);
            if (tileList.TryGetValue(TileType.Override, out objectListTmp))
                allObjectList.AddRange(objectListTmp);

            // 增加顏色地板事件
            //Debug.Log(allObjectList.Count);
            for (int i = 0; i < allObjectList.Count; i++)
            {
                // 地板完成事件
                allObjectList[i].OnTileCompleteEvent.AddListener(LevelTileCompleteListener);
                // 地板錯誤事件
                allObjectList[i].OnTileWrongEvent.AddListener(LevelTileWrongListener);
            }

            // 建立顏色地板完成快取清單
            completeTileList.Clear();
            if (completeTileList.ContainsKey(TileType.Normal) == false)
                completeTileList.Add(TileType.Normal, new List<TileBase>());
            if (completeTileList.ContainsKey(TileType.Override) == false)
                completeTileList.Add(TileType.Override, new List<TileBase>());
        }

        /// <summary>
        /// 關卡地板顏色錯誤事件
        /// </summary>
        protected void LevelTileWrongListener(TileType tileType, TileBase tileObject)
        {
            Debug.Log($"<color=white>Wrong Tile Type: {tileType.ToString()}</color>");

            // 地板種類是會被覆蓋顏色的地板
            if (tileType == TileType.Override)
            {
                if (!completeTileList.TryGetValue(TileType.Override, out List<TileBase> objectList))
                    return;

                objectList.Remove(tileObject);
                currentTargetTile--;
            }
        }

        /// <summary>
        /// 關卡地板完成事件
        /// </summary>
        protected void LevelTileCompleteListener(TileType tileType, TileBase tileObject)
        {
            //Debug.Log($"<color=white>Complete Tile Type: {tileType.ToString()}</color>");
            
            List<TileBase> objectListTmp = null;

            if(tileType == TileType.Normal)
            {
                if (!completeTileList.TryGetValue(TileType.Normal, out objectListTmp))
                    return;

                objectListTmp.Add(tileObject);
                currentTargetTile++;
            }

            if (tileType == TileType.Override)
            {
                if (!completeTileList.TryGetValue(TileType.Override, out objectListTmp))
                    return;

                objectListTmp.Add(tileObject);
                currentTargetTile++;
            }

            //Debug.Log("totalObjectTile:" + currentTargetTile);
            // 完成所有地板，即可過關
            if (currentTargetTile == targetTotalTile)
                OnLevelCompleteEvent?.Invoke();
        }

        protected void OnEnable()
        {
            SetupTileEvent();
        }

        protected void OnDisable()
        {

        }
    }
}
