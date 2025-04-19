using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingPopup : BasePopup
{
    [Header("Sound")]
    public UILabel bgmLabel;
    public UILabel sfxLabel;

    public UISlider bgmSlider;
    public UISlider sfxSlider;

    public UISprite bgmMute;
    public UISprite sfxMute;

    private float saveBgmScale;
    private float saveSfxScale;

    [Header("Language")]
    public UILabel languageLabel;
    public UILabel showLabel;
    public UIPopupList languagePopupList;

    public override void Init(int id = -1)
    {
        base.Init(id);

        bgmSlider.value = PlayerPrefs.GetFloat("BGM", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1);
        SetVolume("BGM");
        SetVolume("SFX");
        MuteSpriteChange("BGM", bgmSlider.value);
        MuteSpriteChange("SFX", sfxSlider.value);
        
        showLabel.text = Managers.Data.GetText(8);
        bgmLabel.text = Managers.Data.GetText(10);
        sfxLabel.text = Managers.Data.GetText(11);
        languageLabel.text = Managers.Data.GetText(12);
    }

    public void SetVolume(string name) {
        if (name == "BGM") {
            Managers.Sound.SetVolume(name, bgmSlider.value);
            MuteSpriteChange(name, bgmSlider.value);
        }
        else if (name == "SFX") {
            Managers.Sound.SetVolume(name, sfxSlider.value);
            MuteSpriteChange(name, sfxSlider.value);
        }
    }

    public void MuteSpriteChange(string name, float value) {
        if (value == 0)
        {
            if (name == "BGM")
            {
                bgmMute.spriteName = "mute";
            }
            else if (name == "SFX") {
                sfxMute.spriteName = "mute";
            }
        }
        else {
            if (name == "BGM")
            {
                bgmMute.spriteName = "nonmute";
            }
            else if (name == "SFX")
            {
                sfxMute.spriteName = "nonmute";
            }

        }
    }

    public void Mute(string name) {
        if (name == "BGM")
        {
            if (bgmSlider.value != 0)
            {
                saveBgmScale = bgmSlider.value;
                bgmSlider.value = 0;
            }
            else {
                if (saveBgmScale != 0) {
                    bgmSlider.value = saveBgmScale;
                }
            }
            Managers.Sound.SetVolume(name, bgmSlider.value);
            MuteSpriteChange(name, bgmSlider.value);
        }
        else if (name == "SFX")
        {
            if (sfxSlider.value != 0)
            {
                saveSfxScale = sfxSlider.value;
                sfxSlider.value = 0;
            }
            else
            {
                if (saveSfxScale != 0)
                {
                    sfxSlider.value = saveSfxScale;
                }
            }
            Managers.Sound.SetVolume(name, sfxSlider.value);
            MuteSpriteChange(name, sfxSlider.value);
        }
    }

    public void ExplanationButton() {
        Managers.Sound.PlaySFX(SfxType.Button);
        PopupContainer.CreatePopup(PopupType.ExpPopup).Init();
    }

    public void OnPopupListValueChanged()
    {
        showLabel.text = languagePopupList.value;
        OnReloadScene();
    }

    public void OnReloadScene() {
        BasePopup basePopup = PopupContainer.CreatePopup(PopupType.ConfirmationPopup);
        ((ConfirmationPopup)basePopup).SetCallback(() =>
        {
            var saveData = Managers.Data.GetData<Table.SaveTable>(0);
            switch (showLabel.text)
            {
                case "한국어":
                    saveData.language = "kor";
                    //Debug.Log("한국어");
                    break;
                case "English":
                    saveData.language = "eng";
                    //Debug.Log("English");
                    break;
            }
            Managers.Data.UpdateData(saveData);
            SceneManager.LoadScene("Title");
        });
        ((ConfirmationPopup)basePopup).SetText(Managers.Data.GetText(9));
        basePopup.Init();
    }

    public override void Close()
    {
        PlayerPrefs.SetFloat("BGM", bgmSlider.value);
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        base.Close();
    }

}
