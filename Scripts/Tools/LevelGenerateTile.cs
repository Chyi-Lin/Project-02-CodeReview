using UnityEngine;

namespace HyperCasualFramework
{
    [CreateAssetMenu(fileName = "New Tile Data", menuName = "HyperCasual/LevelGenerate/Create Tile Data", order = 50)]
    public class LevelGenerateTile : ScriptableObject
    {
        [SerializeField, Header("用途")]
        protected string description;

        [SerializeField, Header("種類")]
        protected TileType tileType;

        [SerializeField, Header("顏色")]
        protected Color color = Color.white;

        [SerializeField, Header("地板物件")]
        protected GameObject prefab;

        [SerializeField, Header("地板大小")]
        protected Vector2 tileSize = Vector2.one;

        public string GetDescription { get { return description; } }

        public TileType GetTileType { get { return tileType; } }

        public Color GetColor { get { return color; } }

        public GameObject GetPrefab { get { return prefab; } }

        public Vector2 GetTileSize { get { return tileSize; } }
    }
}
