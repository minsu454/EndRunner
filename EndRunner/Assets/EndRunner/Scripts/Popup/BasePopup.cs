using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    [SerializeField]
    private UIPanel panel;

    public virtual void Init(int id = -1) {
        Pop();
    }

    public void Pop() {
        int depth = PopupContainer.Pop(this);
        panel.depth = depth * 5;
        panel.transform.localPosition = new Vector3(0, 0, -depth * 100);
    }

    public virtual void Close() {
        Managers.Sound.PlaySFX(SfxType.Button);
        PopupContainer.Close();
        Destroy(gameObject);
    }

}

