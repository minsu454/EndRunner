using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CharacterContainer : MonoBehaviour
{
    public static CharacterContainer instance;

    public Dictionary<CharacterType, CharacterColor> characterGraphicDic = new Dictionary<CharacterType, CharacterColor>();
    public Dictionary<AccessoryType, Dictionary<Enum, CharacterAccessories>> characterAccessoryDic = new Dictionary<AccessoryType, Dictionary<Enum, CharacterAccessories>>();

    private string[] accessoriesPathArray = new string[] {
        "Accessories/Hat/",
        "Accessories/Eye/",
        "Accessories/Tie/",
    };

    public CharacterColor GetCharacter(CharacterType type)
    {
        return characterGraphicDic[type];
    }

    public CharacterAccessories GetAccessory(AccessoryType type1, Enum type2)
    {
        return characterAccessoryDic[type1][type2];
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitCharacterData()
    {
        GameObject characterContainerObj = new GameObject("CharacterContainer");
        instance = characterContainerObj.AddComponent<CharacterContainer>();
        DontDestroyOnLoad(characterContainerObj);

        CharacterContainer.instance.Init();

    }

    public void Init()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Characters/Sprites");
        var runtimeAnimatorControllers = Resources.LoadAll<RuntimeAnimatorController>("Characters/AnimatorControllers");
        if (sprites.Length == runtimeAnimatorControllers.Length)
        {
            var runtimeAnimatorControllerList = runtimeAnimatorControllers.ToList();
            for (int i = 0; i < sprites.Length; i++)
            {
                CharacterType characterType = (CharacterType)Enum.Parse(typeof(CharacterType), sprites[i].name);
                var runtimeAnimatorController = runtimeAnimatorControllerList.Find(x => x.name == characterType.ToString());
                CharacterColor characterColor;
                characterColor.sprite = sprites[i];
                characterColor.runtimeAnimatorController = runtimeAnimatorController;
                CharacterContainer.instance.characterGraphicDic.Add(characterType, characterColor);
            }
        }
        else
        {
            Debug.LogWarning("CharacterColor Count : " + sprites.Length + ", " + runtimeAnimatorControllers.Length);
        }
        for (int i = 0; i < accessoriesPathArray.Length; i++)
        {
            AccessoryType type = (AccessoryType)i;
            CharacterContainer.instance.InitAccessories(type, accessoriesPathArray[i]);
        }
    }

    public void InitAccessories(AccessoryType type, string path)
    {
        Sprite[] idleSprites = Resources.LoadAll<Sprite>(string.Format("{0}Idle", path));
        Sprite[] downMoveSprites = Resources.LoadAll<Sprite>(string.Format("{0}Move/Down", path));
        Sprite[] upMoveSprites = Resources.LoadAll<Sprite>(string.Format("{0}Move/Up", path));
        if (idleSprites.Length == downMoveSprites.Length && idleSprites.Length == upMoveSprites.Length)
        {
            var downMoveSpritesList = downMoveSprites.ToList();
            var upMoveSpritesList = upMoveSprites.ToList();
            CharacterContainer.instance.characterAccessoryDic.Add(type, new Dictionary<Enum, CharacterAccessories>());
            HatType hatType;
            EyeType eyeType;
            TieType tieType;
            for (int i = 0; i < idleSprites.Length; i++)
            {
                CharacterAccessories characterAccessories;
                Sprite downMoveSprite;
                Sprite upMoveSprite;
                switch (type)
                {
                    case AccessoryType.HatType:
                        hatType = (HatType)Enum.Parse(typeof(HatType), idleSprites[i].name);
                        downMoveSprite = downMoveSpritesList.Find(x => x.name == hatType.ToString());
                        upMoveSprite = upMoveSpritesList.Find(x => x.name == hatType.ToString());
                        characterAccessories.idleSprite = idleSprites[i];
                        characterAccessories.downMoveSprite = downMoveSprite;
                        characterAccessories.upMoveSprite = upMoveSprite;
                        CharacterContainer.instance.characterAccessoryDic[type].Add(hatType, characterAccessories);
                        break;
                    case AccessoryType.EyeType:
                        eyeType = (EyeType)Enum.Parse(typeof(EyeType), idleSprites[i].name);
                        downMoveSprite = downMoveSpritesList.Find(x => x.name == eyeType.ToString());
                        upMoveSprite = upMoveSpritesList.Find(x => x.name == eyeType.ToString());
                        characterAccessories.idleSprite = idleSprites[i];
                        characterAccessories.downMoveSprite = downMoveSprite;
                        characterAccessories.upMoveSprite = upMoveSprite;
                        CharacterContainer.instance.characterAccessoryDic[type].Add(eyeType, characterAccessories);
                        break;
                    case AccessoryType.TieType:
                        tieType = (TieType)Enum.Parse(typeof(TieType), idleSprites[i].name);
                        downMoveSprite = downMoveSpritesList.Find(x => x.name == tieType.ToString());
                        upMoveSprite = upMoveSpritesList.Find(x => x.name == tieType.ToString());
                        characterAccessories.idleSprite = idleSprites[i];
                        characterAccessories.downMoveSprite = downMoveSprite;
                        characterAccessories.upMoveSprite = upMoveSprite;
                        CharacterContainer.instance.characterAccessoryDic[type].Add(tieType, characterAccessories);
                        break;
                }
            }
        }
        else
        {
            Debug.LogWarning("CharacterAccessories Count : " + idleSprites.Length + ", " + downMoveSprites.Length + ", " + upMoveSprites.Length);
        }
    }
}

public enum CharacterType
{
    Greenbox,
    Purplebox,
    Redbox,
    Yellowbox,
    Pinkbox,
    Bluebox,
    Emeraldbox,
    DarkBluebox,
    Greybox,
    Orangebox,

}

public enum AccessoryType
{
    HatType = 0,
    EyeType,
    TieType,

}

public enum HatType
{
    None,
    Hat,
    Cowboy,
    Angel,
    Ribbon,
    Zelda,

}

public enum EyeType
{
    Eye,
    Star,
    Skull,

}

public enum TieType
{
    None,
    Mustache,
    Bell,
    Ghost,
}

public struct CharacterColor
{
    public Sprite sprite;
    public RuntimeAnimatorController runtimeAnimatorController;
}

public struct CharacterAccessories
{
    public Sprite idleSprite;
    public Sprite downMoveSprite;
    public Sprite upMoveSprite;
}
