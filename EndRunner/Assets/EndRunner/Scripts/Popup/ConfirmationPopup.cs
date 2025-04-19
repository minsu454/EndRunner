using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationPopup : BasePopup
{
    public UILabel title;

    private System.Action callback;

    public void SetText(string text)
    {
        title.text = text;
    }

    public void SetCallback(System.Action callback)
    {
        this.callback = callback;
    }

    public void Ok()
    {
        if (callback != null)
        {
            callback();
            base.Close();
        }
    }
}
