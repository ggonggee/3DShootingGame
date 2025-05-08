using UnityEngine;

public class UI_OptionPopup : UI_Popup
{

    public void OnClickContinueButton()
    {
        GameManager.Instance.Continue();
        gameObject.SetActive(false);
    }

    public void OnClicketryRetryButton()
    {
        //meObject.SetActive(false);
        GameManager.Instance.Restart();
    }

    public void OnClickeQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif


    }

    public void OnClickCreditButton()
    {
        PopupManager.Instance.Open(EPopupType.UI_CreditPopup);
    }


}
