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
        
    }

    IEnumerator StartGameReady()
    {
        yield return new WaitForSeconds(3f);
        CurrentGameMove = GameMode.Run;
        Debug.Log($"게임 Run {CurrentGameMove}");
    }
}
