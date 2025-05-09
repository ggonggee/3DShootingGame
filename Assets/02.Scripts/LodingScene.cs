using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LodingScene : MonoBehaviour
{
    // 목표: 다음 씬을 '비동기 방식'으로 로드 하고 싶다.
    //       또한 로딩 진행륭을 시각적으로 표현하고 싶다.
    //                              - % 프로그래스 바와 %별 텍스트

    // 속성:
    // - 다음 씬 번호 (인덱스)
    public int NextSceneIndex = 2;
         
    // - 프로그래스 슬라이더바
    public Slider ProgressSlider;

    // - 프로그래스 텍스트
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        //동기적 방식
        //SceneManager.LoadScene(NextSceneIndex);
        StartCoroutine(LoadNextSene_Coroutine());
    }

    private IEnumerator LoadNextSene_Coroutine()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false; // 비동기로 로드되는 씬 모습이 화면에 보이지 않게 한다.

        // 로딩이 되는 동안 계속 해서 반복문
        while (ao.isDone == false)
        {
            // 비동기로 실행할 코드를 여기에 넣으면 된다.
            Debug.Log(ao.progress); // 0~1
            ProgressSlider.value = ao.progress;
            //ProgressText.text = $"{ao.progress * 100}%";
            //여기까지만 살짝 바뀐다.
            //서버 통신해서 유저 데이터나 기획 데이터를 받아오면 된다.

            if(ao.progress >= 0.1f)
            {
                ProgressText.text = $"({ao.progress * 100})임무 수신 중... 좀비 감염 지역에 접근 중입니다.";
            }


            if (ao.progress >= 0.3f)
            {
                ProgressText.text = $"({ao.progress * 100})바이러스 확산 경로 분석 완료. 생존자 없음. 전면 소탕 지시.";
            }

            if (ao.progress >= 0.5f)
            {
                ProgressText.text = $"({ao.progress * 100})장비 확인 중… 스나이퍼 라이플, 탄창, 전술 키트 이상 없음.";
            }

            if (ao.progress >= 0.7f)
            {
                ProgressText.text = $"({ao.progress * 100})열 감지기 활성화. 적 다수 포착… 접근 중!";
            }



            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
                ProgressText.text = $"({ao.progress * 100}) 투입 준비 완료. 작전명: Z.MARINE 개시!";
            }
                        
            yield return null;  // 1프레임 대기
        }
     
    }

}
