using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumePopup : BasePopup
{
    public TypeInfo type;

    private NextImage nextImage;
    private CostumeInfo costumeInfo;
    private int index;


    public override void Init(int id = -1)
    {
        base.Init(id);

        costumeInfo = gameObject.GetComponent<CostumeInfo>();
        int picks = costumeInfo.Init();

        nextImage = gameObject.GetComponent<NextImage>();
        nextImage.Init(picks % 3);
    }

    public void ChangePopup()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        Close();
        switch (type) {
            case TypeInfo.hat:
                PopupContainer.CreatePopup(PopupType.EyePopup).Init();
                break;
            case TypeInfo.eye:
                PopupContainer.CreatePopup(PopupType.TiePopup).Init();
                break;
            case TypeInfo.tie:
                PopupContainer.CreatePopup(PopupType.HatPopup).Init();
                break;
            default:
                break;
        }
    }
}
