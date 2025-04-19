using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndPopup : BasePopup
{
    public void End() {
        Managers.Sound.PlaySFX(SfxType.Button);
        Application.Quit();
    }
}
