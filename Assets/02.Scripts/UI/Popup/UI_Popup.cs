﻿using System;

using UnityEngine;

public class UI_Popup : MonoBehaviour
{
    // 콜백 함수 : 어떤 함수를 기억해 놨다가 특정 시점(작업이 완료된 후 )에 호출하는 함수
    private Action _closeCallback;

    public void Open(Action closeCallback = null) // 1. 야! 팝업열려! 근데! 너 닫힐때 호출할 함수좀 등록할게
    {
        _closeCallback = closeCallback;
        gameObject.SetActive(true);
    }
    public void Close() //2. 나 닫힌다! 어?! 닫힐때 호출할 함수가 등록되어 있네? 실행해야지!
    {
        _closeCallback?.Invoke();
        gameObject.SetActive(false);

        //옵션 팝업일 경우 콘티뉴도 상속 해 줘야 한다.
    }
}
