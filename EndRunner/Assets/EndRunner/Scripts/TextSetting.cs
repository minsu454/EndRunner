using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSetting : MonoBehaviour
{
    public UILabel label;
    public int text_id;

    private void OnEnable()
    {
        label.text = Managers.Data.GetText(text_id);
    }
}
