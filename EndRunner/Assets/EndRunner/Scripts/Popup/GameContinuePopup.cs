using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameContinuePopup : BasePopup
{
    public UILabel Count;

    public override void Init(int id = -1)
    {
        base.Init(id);
        StartCoroutine("CoContinueCountdown");
    }

    IEnumerator CoContinueCountdown() {
        for (int i = 4; i >= 0; i--) {
            yield return new WaitForSeconds(1f);
            Count.text = i.ToString();
        }
        GameOver();
    }

    public void AdStart() {
        Managers.Sound.PlaySFX(SfxType.Button);
        StopCoroutine("CoContinueCountdown");
        Managers.Ad.ShowRewardedAd();
    }

    public void GameOver()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        StopCoroutine("CoContinueCountdown");
        Close();
        GameManager.instance.GameOver();
    }


}
