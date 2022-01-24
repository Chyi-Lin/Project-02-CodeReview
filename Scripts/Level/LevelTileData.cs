using System.Collections.Generic;

namespace HyperCasualFramework
{
    /// <summary>
    /// 關卡地板資訊
    /// </summary>
    [System.Serializable]
    public class LevelTileData
    {
        public TileType type;
        public List<TileBase> tiles;

        public LevelTileData(TileType type)
        {
            this.type = type;
            this.tiles = new List<TileBase>();
        }
    }
}
