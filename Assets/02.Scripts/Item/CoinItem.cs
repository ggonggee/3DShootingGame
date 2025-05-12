using DG.Tweening;

using UnityEngine;

public class CoinItem : MonoBehaviour
{
    public float RotationSpeed = 100f;
    private float _value = 3f;

    void Start()
    {
        //Vector3 ran = Random.insideUnitSphere * _value;
        //Vector3 endPos = transform.position + ran;
        //endPos.y = transform.position.y;
        //transform.DOJump(endPos, 2, 1, 1f).SetEase(Ease.OutBounce);
    }

    void Update()
    {
        transform.Rotate(0f, RotationSpeed * Time.deltaTime, 0f);
    }
}
