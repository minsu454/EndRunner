using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopup : BasePopup
{
    [Header("Item")]
    public List<GameObject> pickList = new List<GameObject>();
    private List<bool> itemList = new List<bool>();

    [Header("Coin")]
    public UILabel coinLabel;
    private int itemMoney;

    [Header("Text")]
    public UILabel bombReducedCooldownTime;
    public UILabel barrier;
    public UILabel hyperRun;

    [Header("QuestionMark")]
    public GameObject question;

    [Header("difficulty")]
    public UILabel showLabel;
    public UIPopupList difficultyPopupList;

    private enum ItemType
    {
        BombReducedCooldownTime = 0,
        Barrier,
        HyperRun,

    }

    public override void Init(int id = -1)
    {
        base.Init(id);
        
        for (int i = 0; i < pickList.Count; i++)
        {
            itemList.Add(false);
        }
        ItemUIReset();
        question.SetActive(false);
        bombReducedCooldownTime.text = Managers.Data.GetText(0);
        barrier.text = Managers.Data.GetText(1);
        hyperRun.text = Managers.Data.GetText(2);
        itemMoney = 0;
        if (!PlayerPrefs.HasKey("Dif")) {
            PlayerPrefs.SetString("Dif", "easy");
        }
        showLabel.text = PlayerPrefs.GetString("Dif");
    }

    public void QuestionMark() {
        Managers.Sound.PlaySFX(SfxType.Button);
        question.SetActive(!question.activeSelf);
    }

    public void ItemUIReset()
    {
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        coinLabel.text = saveData.coin.ToString();
        saveData.bombreducedcooldowntime = "FALSE";
        saveData.barrier = "FALSE";
        saveData.hyperrun = "FALSE";
        Managers.Data.UpdateData(saveData);
    }

    public void BombReducedCooldownTime()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        if (!itemList[(int)ItemType.BombReducedCooldownTime])
        {
            itemMoney += 6000;

            if (saveData.coin >= itemMoney)
            {
                pickList[(int)ItemType.BombReducedCooldownTime].SetActive(true);
                itemList[(int)ItemType.BombReducedCooldownTime] = !itemList[(int)ItemType.BombReducedCooldownTime];
                saveData.bombreducedcooldowntime = "TRUE";
            }
            else {
                itemMoney -= 6000;
            }
        }
        else
        {
            pickList[(int)ItemType.BombReducedCooldownTime].SetActive(false);
            itemMoney -= 6000;
            itemList[(int)ItemType.BombReducedCooldownTime] = !itemList[(int)ItemType.BombReducedCooldownTime];
            saveData.bombreducedcooldowntime = "FALSE";
        }
        Managers.Data.UpdateData(saveData);
    }

    public void Barrier()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        if (!itemList[(int)ItemType.Barrier])
        {
            itemMoney += 4500;
            if (saveData.coin >= itemMoney)
            {
                pickList[(int)ItemType.Barrier].SetActive(true);
                itemList[(int)ItemType.Barrier] = !itemList[(int)ItemType.Barrier];
                saveData.barrier = "TRUE";
            }
            else
            {
                itemMoney -= 4500;
            }
        }
        else
        {
            pickList[(int)ItemType.Barrier].SetActive(false);
            itemList[(int)ItemType.Barrier] = !itemList[(int)ItemType.Barrier];
            itemMoney -= 4500;
            saveData.barrier = "FALSE";
        }
        Managers.Data.UpdateData(saveData);
    }

    public void HyperRun()
    {

        Managers.Sound.PlaySFX(SfxType.Button);
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        if (!itemList[(int)ItemType.HyperRun])
        {
            itemMoney += 8500;
            if (saveData.coin >= itemMoney)
            {
                pickList[(int)ItemType.HyperRun].SetActive(true);
                itemList[(int)ItemType.HyperRun] = !itemList[(int)ItemType.HyperRun];
                saveData.hyperrun = "TRUE";
            }
            else
            {
                itemMoney -= 8500;
            }
        }
        else
        {
            pickList[(int)ItemType.HyperRun].SetActive(false);
            itemList[(int)ItemType.HyperRun] = !itemList[(int)ItemType.HyperRun];
            itemMoney -= 8500;
            saveData.hyperrun = "FALSE";
        }
        Managers.Data.UpdateData(saveData);
    }

    public void OnPopupListValueChanged()
    {
        showLabel.text = difficultyPopupList.value;
        PlayerPrefs.SetString("Dif", showLabel.text);
    }

    public override void Close()
    {
        base.Close();
        TitleManager.instance.titleLabel.SetActive(true);
        TitleManager.instance.itemupPanel.depth = 2;
    }
}
