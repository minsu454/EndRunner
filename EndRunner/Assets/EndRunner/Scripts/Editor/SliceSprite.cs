using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SliceSprite
{
    [MenuItem("Assets/Reset PlayerPrefs")]
    static void ResetPlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Assets/Slice Sprite/Center Pivot")]
    static void SliceSpriteCenterPivot() {
        var folderGUIDs = Selection.assetGUIDs;
        for (int i = 0; i < folderGUIDs.Length; i++) {
            Slice(AssetDatabase.GUIDToAssetPath(folderGUIDs[i]), new Vector2(0.5f, 0.5f));
        }
    }

    [MenuItem("Assets/Slice Sprite/Top Pivot")]
    static void SliceSpriteTopPivot()
    {
        var folderGUIDs = Selection.assetGUIDs;
        for (int i = 0; i < folderGUIDs.Length; i++)
        {
            Slice(AssetDatabase.GUIDToAssetPath(folderGUIDs[i]), new Vector2(0.5f, 1));
        }
    }

    static void Slice(string imageFilePath, Vector2 pivot) {
        TextureImporter importer = null;
        string assetFilePath = string.Format("{0}.asset", imageFilePath.Remove(imageFilePath.LastIndexOf('.')));

        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imageFilePath);
        var nguiAsset = AssetDatabase.LoadAssetAtPath<NGUIAtlas>(assetFilePath);

        importer = (TextureImporter)TextureImporter.GetAtPath(imageFilePath);
        importer.spriteImportMode = SpriteImportMode.Single;
        AssetDatabase.ImportAsset(imageFilePath, ImportAssetOptions.ForceUpdate);
        importer.spriteImportMode = SpriteImportMode.Multiple;

        List<SpriteMetaData> metaList = new List<SpriteMetaData>();
        for (int i = 0; i < nguiAsset.spriteList.Count; i++) {
            SpriteMetaData meta = new SpriteMetaData();
            meta.name = nguiAsset.spriteList[i].name;
            meta.border = new Vector4(
                    nguiAsset.spriteList[i].borderLeft,
                    nguiAsset.spriteList[i].borderBottom,
                    nguiAsset.spriteList[i].borderRight,
                    nguiAsset.spriteList[i].borderTop
            );

            meta.rect = new Rect(
                nguiAsset.spriteList[i].x,
                texture.height - (nguiAsset.spriteList[i].y + nguiAsset.spriteList[i].height),
                nguiAsset.spriteList[i].width,
                nguiAsset.spriteList[i].height
            );

            meta.alignment = 9;
            meta.pivot = pivot;
            metaList.Add(meta);
        }

        importer.spritesheet = metaList.ToArray();
        AssetDatabase.ImportAsset(imageFilePath, ImportAssetOptions.ForceUpdate);
    }
}
