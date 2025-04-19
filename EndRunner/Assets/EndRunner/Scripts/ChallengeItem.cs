using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeItem : MonoBehaviour
{
    public UILabel challengeInfoLabel;
    public UISprite boxSp;
    public UISprite hatSp;
    public UISprite eyeSp;
    public UISprite tieSp;
    public GameObject getButtonObj;
    public GameObject getLabel;
    public GameObject lockObj;
    public GameObject clearObj;

    private int challengeId;

    public void Init(int challengeId)
    {
        this.challengeId = challengeId;
        if (challengeId != -1)
        {
            var challengeData = Managers.Data.GetData<Table.ChallengeTable>(challengeId);
            challengeInfoLabel.text = string.Format(Managers.Data.GetText(challengeData.text_id), challengeData.value);
            lockObj.SetActive(true);
            clearObj.SetActive(false);
            getLabel.SetActive(false);

            var clearChallengeData = Managers.Data.GetData<Table.PlayerChallengeTable>(challengeId);
            if (clearChallengeData.clear_challenge == "CLEAR")
            {
                SetNone();
                clearObj.SetActive(true);
            }
            else
            {
                if (clearChallengeData.clear_challenge == "TRUE")
                {
                    lockObj.SetActive(false);
                    getLabel.SetActive(true);
                }
            
                S.RewardInfo info = JsonUtility.FromJson<S.RewardInfo>(challengeData.reward_json);

                getButtonObj.SetActive(true);
                boxSp.spriteName = "Greenbox";
                hatSp.spriteName = "None";
                eyeSp.spriteName = "Eye";
                tieSp.spriteName = "None";

                if (info.type == "character")
                {
                    var data = Managers.Data.GetData<Table.PlayerCharacterTable>(info.value);
                    boxSp.spriteName = data.name;
                }
                else
                {
                    var data = Managers.Data.GetData<Table.PlayerAccessoryTable>(info.value);
                    switch (info.type)
                    {
                        case "hat":
                            hatSp.spriteName = data.name;
                            break;
                        case "eye":
                            eyeSp.spriteName = data.name;
                            break;
                        case "tie":
                            tieSp.spriteName = data.name;
                            break;
                    }
                }
            }
        }
        else {
            challengeInfoLabel.text = "";
            SetNone();
        }
    }

    public void SetNone()
    {
        getButtonObj.SetActive(false);
        clearObj.SetActive(false);
        lockObj.SetActive(false);
        boxSp.spriteName = "None";
        hatSp.spriteName = "None";
        eyeSp.spriteName = "None";
        tieSp.spriteName = "None";
    }

    public void GetCostume()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        var clearChallengeData = Managers.Data.GetData<Table.PlayerChallengeTable>(challengeId);
        clearChallengeData.clear_challenge = "CLEAR";
        Managers.Data.UpdateData(clearChallengeData);
        var challengeData = Managers.Data.GetData<Table.ChallengeTable>(challengeId);
        S.RewardInfo info = JsonUtility.FromJson<S.RewardInfo>(challengeData.reward_json);
        if (info.type == "character")
        {
            var data = Managers.Data.GetData<Table.PlayerCharacterTable>(info.value);
            data.my_character = "TRUE";
            Managers.Data.UpdateData(data);
        }
        else
        {
            var data = Managers.Data.GetData<Table.PlayerAccessoryTable>(info.value);
            data.my_accessories = "TRUE";
            Managers.Data.UpdateData(data);
        }
        SetNone();
        clearObj.SetActive(true);
    }
}
