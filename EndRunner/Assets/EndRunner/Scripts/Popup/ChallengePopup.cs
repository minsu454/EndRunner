using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengePopup : BasePopup
{
    public ChallengeItem[] challengeItems;

    private int curPage;
    private int maxPage;

    public override void Init(int id = -1)
    {
        base.Init(id);
        SetPage();
    }

    public void BeforeButton() {
        Managers.Sound.PlaySFX(SfxType.Button);
        curPage--;
        if (curPage <= 0) {
            curPage = maxPage;
        }
        SetItem();
    }

    public void AfterButton() {
        Managers.Sound.PlaySFX(SfxType.Button);
        curPage++;
        if (curPage > maxPage)
        {
            curPage = 1;
        }
        SetItem();
    }

    public void SetPage(int curPage = 1) {
        this.curPage = curPage;
        SetItem();
    }

    public void SetItem() {
        var dataList = Managers.Data.GetDataList<Table.ChallengeTable>();
        maxPage = dataList.Count / 6 + 1;

        int startIndex = (curPage - 1) * 6;
        int itemCount = 0;

        for (int i = startIndex; i < dataList.Count; i++)
        {
            if (itemCount > 5)
            {
                break;
            }
            challengeItems[itemCount].Init(dataList[i].id);
            itemCount++;
        }
        
        for (int i = itemCount; i < 6; i++)
        {
            challengeItems[i].Init(-1);
        }
    }
}
