using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeAnchorManager : MonoBehaviour
{

    public List<AnchorTarget> positionTargetList;

    [System.Serializable]
    public struct AnchorTarget
    {
        public UIRect target;
        public RELATIVE leftRelative;
        public RELATIVE rightRelative;
        public RELATIVE topRelative;
        public RELATIVE bottomRelative;
        public int leftOffset;
        public int rightOffset;
        public int topOffset;
        public int bottomOffset;

    }
    public enum RELATIVE
    {
        CENTER,
        LEFT,
        RIGHT,
        TOP,
        BOTTOM,
    }

    //    leftAnchor.relative = 0f;
    //    rightAnchor.relative = 1f;
    //    bottomAnchor.relative = 0f;
    //    topAnchor.relative = 1f;
    void OnEnable()
    {
        for (int i = 0; i < positionTargetList.Count; i++)
        {
            float leftRelative = 0.5f;
            float rightRelative = 0.5f;
            float topRelative = 0.5f;
            float bottomRelative = 0.5f;

            switch (positionTargetList[i].leftRelative)
            {
            case RELATIVE.LEFT: leftRelative = 0; break;
            case RELATIVE.RIGHT: leftRelative = 1; break;
            }

            switch (positionTargetList[i].rightRelative)
            {
            case RELATIVE.LEFT: rightRelative = 0; break;
            case RELATIVE.RIGHT: rightRelative = 1; break;
            }

            switch (positionTargetList[i].topRelative)
            {
            case RELATIVE.BOTTOM: topRelative = 0; break;
            case RELATIVE.TOP: topRelative = 1; break;
            }

            switch (positionTargetList[i].bottomRelative)
            {
            case RELATIVE.BOTTOM: bottomRelative = 0; break;
            case RELATIVE.TOP: bottomRelative = 1; break;
            }

            positionTargetList[i].target.SetAnchor(
                transform.root.gameObject,
                leftRelative,
                positionTargetList[i].leftOffset,
                bottomRelative,
                positionTargetList[i].bottomOffset,
                rightRelative,
                positionTargetList[i].rightOffset,
                topRelative,
                positionTargetList[i].topOffset);
        }
    }
}
