using UnityEngine;

public class UI_OptionPopup : MonoBehaviour
{

    public void Open()
    {
        gameObject.SetActive(true);
    }

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
//#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
//#else
        Application.Quit();


    }

    public void OnClickCreditButton()
    {
        GameManager.Instance.Credit();
    }


}
