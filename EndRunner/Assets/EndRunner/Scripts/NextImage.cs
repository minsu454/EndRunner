using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextImage : MonoBehaviour
{
    public TypeInfo typeInfo;

    public bool isUsedSprite = true;
    public UISprite[] sprites_0;
    public UISprite[] sprites_1;
    public UISprite[] sprites_2;
    public UISprite[] sprites_3;

    public GameObject[] pages;

    private int pageNum;
    private int picks;

    public void Init(int picks, int pageNum = 0)
    {
        if (isUsedSprite) {
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        for (int i = 0; i < sprites_0.Length; i++)
            {
                if (typeInfo != TypeInfo.Character)
                    sprites_0[i].spriteName = saveData.pick_character;
                if (typeInfo != TypeInfo.hat)
                    sprites_1[i].spriteName = saveData.pick_hat;
                if (typeInfo != TypeInfo.eye)
                    sprites_2[i].spriteName = saveData.pick_eye;
                if (typeInfo != TypeInfo.tie)
                    sprites_3[i].spriteName = saveData.pick_tie;
            }
        }

        this.pageNum = pageNum;
        this.picks = picks;
        if (pageNum == pages.Length - 1) {
            SpriteOnoff(false);
        }
        pages[pageNum].SetActive(true);
    }

    public void BeforeButton()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        next(false);
    }

    public void AfterButton()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        next(true);
    }

    void next(bool isAfter)
    {
        pages[pageNum].SetActive(false);
        if (isAfter)
        {
            pageNum++;
            if (pageNum == pages.Length)
            {
                pageNum = 0;
            }
        }
        else
        {
            pageNum--;
            if (pageNum == -1)
            {
                pageNum = pages.Length - 1;
            }
        }
        if (pageNum == pages.Length - 1 && isUsedSprite)
        {
            SpriteOnoff(false);
        }
        else {
            SpriteOnoff(true);
        }
        pages[pageNum].SetActive(true);
    }

    void SpriteOnoff(bool isTrue)
    {
        if (picks > 0)
        {
            for (int i = picks; i < 3; i++)
            {
                sprites_0[i].gameObject.SetActive(isTrue);
                sprites_1[i].gameObject.SetActive(isTrue);
                sprites_2[i].gameObject.SetActive(isTrue);
                sprites_3[i].gameObject.SetActive(isTrue);
            }
        }
    }
}
