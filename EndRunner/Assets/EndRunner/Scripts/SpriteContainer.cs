using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteContainer
{
    private static SpriteContainer instance;
    public static SpriteContainer Instance {
        get {
            if (instance == null) {
                instance = new SpriteContainer();
            }
            return instance;
        }
    }

    public enum Category {
        Accessories = 0,
        Characters,
        Enemy,

    }

    private string[] atlasPathArray = new string[] {
        "Atlases/Accessories",
        "Atlases/Characters",
        "Atlases/Enemy",

    };

    Dictionary<Category, Dictionary<string, Sprite>> spriteDic = new Dictionary<Category, Dictionary<string, Sprite>>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitSpriteContainer() {
        SpriteContainer.Instance.Init();
    }

    public void Init()
    {
        for (int i = 0; i < atlasPathArray.Length; i++) {
            Category category = (Category)i;
            Sprite[] sprites = Resources.LoadAll<Sprite>(atlasPathArray[i]);
            spriteDic.Add(category, new Dictionary<string, Sprite>());
            for (int j = 0; j < sprites.Length; j++) {
                spriteDic[category].Add(sprites[j].name, sprites[j]);
            }
        }
    }

    public Sprite GetSprite(Category category, string name) {
        return spriteDic[category][name];
    }

}
