using HyperCasualFramework;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelGenerator : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField]
    protected Transform parentTransform;

    [SerializeField]
    protected Texture2D map;

    [SerializeField]
    [Min(0)]
    protected int limitSize = 1000;

    [SerializeField]
    protected LevelGenerateTile[] colorMappings;

    protected GameObject currentLevelParent;
    protected LevelDetail currentLevelDetail;

    [ContextMenu("Generater Level")]
    protected void GeneraterLevel()
    {
        if (parentTransform == null)
        {
            Debug.LogWarning("Generater parent transform not setup!");
            return;
        }

        if (map.width * map.height > limitSize)
        {
            Debug.LogWarning("Map pixels are too large");
            return;
        }

        ClearLevel();
        CreateLevel();
    }

    [ContextMenu("Clear Level")]
    protected void ClearLevel()
    {
        if(currentLevelParent != null)
            DestroyImmediate(currentLevelParent); 
    }

    protected void CreateLevel()
    {
        currentLevelParent = new GameObject("LevelDetail");
        currentLevelParent.transform.SetParent(parentTransform, false);
        currentLevelDetail = currentLevelParent.AddComponent<LevelDetail>();

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    protected void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            return;
        }

        Debug.Log(pixelColor);

        Vector3 position = new Vector3(-map.width / 2, 0f, -map.height / 2);
        foreach (var colorMapping in colorMappings)
        {
            if (colorMapping.GetColor == pixelColor)
            {
                Vector3 calculatePos = position + new Vector3(x * colorMapping.GetTileSize.x + 1f, 0, y * colorMapping.GetTileSize.x + 1f);
                GameObject tileObj = (GameObject)PrefabUtility.InstantiatePrefab(colorMapping.GetPrefab, currentLevelParent.transform);
                tileObj.transform.localPosition = calculatePos;

                // 地板物件要有 TileBase Component 才算是地板
                currentLevelDetail.AddLevelTileData(colorMapping.GetTileType, tileObj.GetComponent<TileBase>());
            }
        }
    }
#endif
}