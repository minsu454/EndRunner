using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPopup : BasePopup
{
    [Header("Page")]
    public UILabel nowPagenumLabel;
    public UILabel maxPagenumLabel;
    public GameObject[] expGameObj;

    [Header("Image")]
    public UISprite sprite;

    private int nowPagenum;
    private int maxPagenum;

    public override void Init(int id = -1)
    {
        base.Init(id);
        Managers.Sound.SetBGM(0.2f);
        nowPagenum = 1;
        maxPagenum = expGameObj.Length;
        for (int i = 0; i < maxPagenum; i++) {
            expGameObj[i].SetActive(false);
        }
        ImageChange();
        maxPagenumLabel.text = string.Format("/" + maxPagenum.ToString());
    }

    public void PageUp() {
        Managers.Sound.PlaySFX(SfxType.Button);
        expGameObj[nowPagenum - 1].SetActive(false);
        nowPagenum++;
        if (nowPagenum > maxPagenum) {
            nowPagenum = 1;
        }
        ImageChange();
    }

    public void PageDown()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        expGameObj[nowPagenum - 1].SetActive(false);
        nowPagenum--;
        if (nowPagenum < 1) {
            nowPagenum = maxPagenum;
        }
        ImageChange();
    }

    public void ImageChange() {
        nowPagenumLabel.text = nowPagenum.ToString();
        ExpTypeInfo info = expGameObj[nowPagenum - 1].GetComponent<ExpTypeInfo>();
        sprite.spriteName = info.expType.ToString();
        expGameObj[nowPagenum - 1].SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        Managers.Sound.SetBGM(1f);
    }

}
