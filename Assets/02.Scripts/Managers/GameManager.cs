using System.Collections;

using UnityEditor;

using UnityEngine;

public enum GameMode
{
    Ready,
    Run,
    Pause,
    Over,
}

public class GameManager : Singleton<GameManager>
{
    public GameMode CurrentGameMove;

    public UI_OptionPopup UI_OptionPopup;

    private GameMode _gameState = GameMode.Ready;
    void Start()
    {
        StartCoroutine(StartGameReady());
    }



    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if(CurrentGameMove == GameMode.Over)
        {
            UIManager.Instance.GameOverPanel.SetActive(true);
        }
    }

    private void Pause()
    {
        _gameState = GameMode.Pause;
        // Todo:
        // 옵션 팝업을 활성화 한다.
        UI_OptionPopup.Open();
    }

    IEnumerator StartGameReady()
    {
        UIManager.Instance.ReadyPanel.SetActive(true);
        UIManager.Instance.ReadyText.text = "READY";
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ReadyText.text = "GO!";
        CurrentGameMove = GameMode.Run;
        Debug.Log($"게임 Run {CurrentGameMove}");
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ReadyPanel.SetActive(false);
    }


}
