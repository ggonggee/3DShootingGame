using System.Collections;

using UnityEngine;

public enum GameMode
{
    Ready,
    Run,
    Over,
}

public class GameManager : Singleton<GameManager>
{
    public GameMode CurrentGameMove;
    void Start()
    {
        StartCoroutine(StartGameReady());
    }


    void Update()
    {
        if(CurrentGameMove == GameMode.Over)
        {
            UIManager.Instance.GameOverPanel.SetActive(true);
        }
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
