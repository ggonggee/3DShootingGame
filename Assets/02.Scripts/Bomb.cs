using UnityEngine;

public class Bomb
    : MonoBehaviour
{
    // ��ǥ: ���콺 ������ ��ư�� ������ ī�޶� �ٶ󺸴� �������� ����ź�� 
    // 1. ����ź ������Ʈ �����
    // 2. ������ ���콺 Ŭ��
    // 3. �߻� ��ġ�� ����ź �����ϱ�
    // 4. ������ ����ź�� ī�޶� �������� �������� �� ���ϱ�

    public GameObject ExplosionEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;
        gameObject.SetActive(false);
    }
}
