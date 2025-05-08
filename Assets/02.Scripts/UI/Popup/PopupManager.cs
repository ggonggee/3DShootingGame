using System;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

public enum EPopupType
{
    UI_OptionPopup,
    UI_CreditPopup
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    private List<UI_Popup> _openedPopups = new List<UI_Popup>(); //null은 아니지만 비어 있는 리스트

    [Header("팝업 UI 참조")]
    public List<UI_Popup> Popups; // 모든 팝업을 관리하는데

    private void Awake()
    {
        Instance = this;
    }

    public void Open(EPopupType type, Action closeCallback = null)
    {
        Open(type.ToString(), closeCallback);
    }

    private void Open(string popupName, Action closeCallback)
    {
        foreach (UI_Popup popup in Popups)
        {
            if (popup.gameObject.name == popupName)
            {
                popup.Open(closeCallback);
                //팝업을 열때 마다 담는다.
                _openedPopups.Add(popup);
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_openedPopups.Count > 0)
            {
                _openedPopups[_openedPopups.Count - 1].Close();
                _openedPopups.RemoveAt(_openedPopups.Count - 1);
            }
            //_openedPopups[^1].Close();  //마지막거 하나요 라는 코드
            //가장 최근 열렸던 팝업을.close();

            else
            {
                GameManager.Instance.Pause();
            }
        }
    }

}
