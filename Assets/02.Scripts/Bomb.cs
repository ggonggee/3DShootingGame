using UnityEngine;

public class Bomb
    : MonoBehaviour
{
    // 목표: 마우스 오른쪽 버튼을 누르면 카메라가 바라보는 방향으로 수류탄을 
    // 1. 수류탄 오브젝트 만들기
    // 2. 오른쪽 마우스 클릭
    // 3. 발사 위치에 수류탄 생성하기
    // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기

    public GameObject ExplosionEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;
        gameObject.SetActive(false);
    }
}
