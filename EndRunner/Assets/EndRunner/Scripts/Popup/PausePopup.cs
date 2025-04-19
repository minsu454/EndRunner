using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePopup : BasePopup
{
    public void GoTitle() {
        Managers.Sound.PlaySFX(SfxType.Button);
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        if (saveData.bombreducedcooldowntime == "TRUE" || saveData.hyperrun == "TRUE" || saveData.barrier == "TRUE")
        {
            BasePopup basePopup = PopupContainer.CreatePopup(PopupType.ConfirmationPopup);
            ((ConfirmationPopup)basePopup).SetCallback(() =>
            {
                GameManager.instance.GameOver();
            });
            ((ConfirmationPopup)basePopup).SetText(Managers.Data.GetText(3));
            basePopup.Init();
        }
        else {
            GameManager.instance.SetPause();
            GameManager.instance.GoTitle();
        }
    }

    public void Continue() {
        Managers.Sound.PlaySFX(SfxType.Button);
        GameManager.instance.SetPause();
        Close();
    }

    public void Retry() {
        Managers.Sound.PlaySFX(SfxType.Button);
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        if (saveData.bombreducedcooldowntime == "TRUE" || saveData.hyperrun == "TRUE" || saveData.barrier == "TRUE")
        {
            BasePopup basePopup = PopupContainer.CreatePopup(PopupType.ConfirmationPopup);
            ((ConfirmationPopup)basePopup).SetCallback(() =>
            {
                GameManager.instance.SetPause();
                GameManager.instance.Retry();
            });
            ((ConfirmationPopup)basePopup).SetText(Managers.Data.GetText(3));
            basePopup.Init();
        }
        else
        {
            GameManager.instance.SetPause();
            GameManager.instance.Retry();
        }
    }

}
