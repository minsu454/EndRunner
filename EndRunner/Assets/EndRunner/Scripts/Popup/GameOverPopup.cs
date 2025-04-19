using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameOverPopup : BasePopup
{
    public UILabel totalScoreLabel;
    public UILabel highScoreLabel;
    public UIWidget newWidget;
    public UILabel coinLabel;
    public UILabel coinplusLabel;
    public GameObject[] victoryParticle;

    public override void Init(int id = -1)
    {
        base.Init(id);

        Table.SaveTable saveData = Managers.Data.GetData<Table.SaveTable>(0);
        coinLabel.text = saveData.coin.ToString();
        int coin = 0;
        if (PlayerPrefs.GetString("Dif") == "easy")
        {
            if (saveData.high_score_easy < GameManager.instance.Score)
            {
                saveData.high_score_easy = GameManager.instance.Score;

                DOTween.To(() => 0f, x => newWidget.alpha = x, 1f, 1.5f);
                StartCoroutine("CoParticleOn");
            }
            coin = GameManager.instance.Score / 60;
            highScoreLabel.text = saveData.high_score_easy.ToString();
        }
        else
        {
            if (saveData.high_score_hard < GameManager.instance.Score)
            {
                saveData.high_score_hard = GameManager.instance.Score;

                DOTween.To(() => 0f, x => newWidget.alpha = x, 1f, 1.5f);
                StartCoroutine("CoParticleOn");
            }
            coin = GameManager.instance.Score / 30;

            var challengeDataList_1 = Managers.Data.GetDataList<Table.ChallengeTable>().FindAll(x => x.text_id == 5);
            Table.PlayerChallengeTable playerData;
            for (int i = 0; i < challengeDataList_1.Count; i++)
            {
                playerData = Managers.Data.GetData<Table.PlayerChallengeTable>(challengeDataList_1[i].id);
                if (playerData.clear_challenge == "FALSE")
                {
                    if (challengeDataList_1[i].value <= saveData.play_count)
                    {
                        playerData.clear_challenge = "TRUE";
                        Managers.Data.UpdateData(playerData);
                    }
                    break;
                }
            }

            var challengeDataList_2 = Managers.Data.GetDataList<Table.ChallengeTable>().FindAll(x => x.text_id == 7);
            if (!GameManager.instance.isNotBombNotItem)
            {
                for (int i = 0; i < challengeDataList_2.Count; i++)
                {
                    playerData = Managers.Data.GetData<Table.PlayerChallengeTable>(challengeDataList_2[i].id);
                    if (playerData.clear_challenge == "FALSE")
                    {
                        if (challengeDataList_2[i].value <= GameManager.instance.Score)
                        {
                            playerData.clear_challenge = "TRUE";
                            Managers.Data.UpdateData(playerData);
                        }
                        break;
                    }
                }
            }

            highScoreLabel.text = saveData.high_score_hard.ToString();
        }
        if (coin <= 0)
        {
            coin = 0;
        }
        saveData.coin += coin;
        coinplusLabel.text = string.Format("+{0}", coin.ToString());

        saveData.play_count++;
        totalScoreLabel.text = GameManager.instance.Score.ToString();
        Managers.Data.UpdateData(saveData);
        StartCoroutine("CoCoinAnim");
    }

    IEnumerator CoCoinAnim() {
        yield return new WaitForSeconds(1f);
        Table.SaveTable saveTable = Managers.Data.GetData<Table.SaveTable>(0);
        coinplusLabel.transform.DOLocalMoveY(300f, 1.5f).OnComplete(() => {
            coinLabel.text = saveTable.coin.ToString();
        });
        Color color = Color.white;
        DOTween.To(() => color.a, x => coinplusLabel.alpha = x, 0f, 1f);
        
        StopCoroutine("CoCoinAnim");
    }

    IEnumerator CoParticleOn() {
        int i = 0;
        while (victoryParticle[i] == true) {
            yield return new WaitForSeconds(0.4f);
            victoryParticle[i].SetActive(true);
            i++;
        }
        StopCoroutine("CoParticleOn");
    }

    public void Retry() {
        Managers.Sound.PlaySFX(SfxType.Button);
        GameManager.instance.Retry();
    }

    public void GoTitle() {
        Managers.Sound.PlaySFX(SfxType.Button);
        GameManager.instance.GoTitle();
    }
}
