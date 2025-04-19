using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeInfo : MonoBehaviour
{
    public GameObject[] picks;
    public GameObject[] locks;

    public TypeInfo type;

    private int index;
    private GameObject pickgameObj;

    public int Init()
    {
        pickgameObj = null;

        var saveData = Managers.Data.GetData<Table.SaveTable>(0);

        string saveDataString = "";
        switch (type)
        {
            case TypeInfo.Character:
                saveDataString = saveData.pick_character;
                break;
            case TypeInfo.hat:
                saveDataString = saveData.pick_hat;
                break;
            case TypeInfo.eye:
                saveDataString = saveData.pick_eye;
                break;
            case TypeInfo.tie:
                saveDataString = saveData.pick_tie;
                break;
        }

        for (int i = 0; i < picks.Length; i++)
        {
            if (picks[i].transform.parent.name == saveDataString)
            {
                pickgameObj = picks[i];
                break;
            }
        }

        pickgameObj.SetActive(true);

        if (type != TypeInfo.Character)
        {
            var lockDataList = Managers.Data.GetDataList<Table.PlayerAccessoryTable>().FindAll(x => x.accessorytype == type.ToString());
            for (int i = 0; i < lockDataList.Count; i++)
            {
                if (lockDataList[i].my_accessories == "FALSE")
                {
                    locks[i].SetActive(true);
                }
            }
        }
        else
        {
            var lockDataList = Managers.Data.GetDataList<Table.PlayerCharacterTable>();
            for (int i = 0; i < lockDataList.Count; i++)
            {
                if (lockDataList[i].my_character == "FALSE")
                {
                    locks[i].SetActive(true);
                }
            }
        }

        return picks.Length;
    }

    public void AccessoryPick(string name, string pick)
    {
        for (int i = 0; i < picks.Length; i++)
        {
            if (picks[i].name == pick)
            {
                index = i;
            }
        }
        pickgameObj.SetActive(false);
        pickgameObj = picks[index];
        pickgameObj.SetActive(true);

        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        switch (type)
        {
            case TypeInfo.Character:
                saveData.pick_character = picks[index].transform.parent.name;
                break;
            case TypeInfo.hat:
                saveData.pick_hat = picks[index].transform.parent.name;
                break;
            case TypeInfo.eye:
                saveData.pick_eye = picks[index].transform.parent.name;
                break;
            case TypeInfo.tie:
                saveData.pick_tie = picks[index].transform.parent.name;
                break;
        }
        Managers.Data.UpdateData(saveData);
    }
}

public enum TypeInfo
{
    Character,
    hat,
    eye,
    tie,
}
