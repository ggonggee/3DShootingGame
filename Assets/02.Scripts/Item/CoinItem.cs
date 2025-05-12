using DG.Tweening;

using UnityEngine;

public class CoinItem : MonoBehaviour
{
    public Transform player;
    public float RotationSpeed = 100f;
    public float speed = 3f;
    public float CollectSpeed = 10f;
    public bool isCollecting = false;
    public float colletRange = 5f;

    void Start()
    {
        Vector3 ran = Random.insideUnitSphere * speed;
        Vector3 endPos = transform.position + ran;
        endPos.y = transform.position.y;
        transform.DOJump(endPos, 2, 1, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            isCollecting = true;
        });

        //spawnTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        transform.Rotate(0f, RotationSpeed * Time.deltaTime, 0f);

        if (player == null) return;
       Vector3 direction  = player.position - transform.position;
       float distance = direction.magnitude;
       direction = direction.normalized;


        //if (isCollecting && distance < colletRange)
        //{
        //    transform.position = Vector3.MoveTowards(
        //        transform.position,
        //        player.position,
        //        CollectSpeed * Time.deltaTime
        //    );

        //    if (distance < 0.2f)
        //    {
        //        Collect();
        //    }
        //}
        if (isCollecting && distance < colletRange)
        {
            isCollecting = false;
            //Sequence mySequence = DOTween.Sequence();
            //mySequence.Append(transform.DOMove(player.position, 0.8f).SetEase(Ease.InBack));
            //mySequence.Append(transform.DOScale(Vector3.zero, 0.2f).OnComplete(Collect));
            //mySequence.Play();
            transform.DOMove(player.position, 0.5f).SetEase(Ease.InBack);
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(Collect);

        }
    }

    //public float collectRange = 3f;       // 수집 반경
    //public float moveSpeed = 10f;         // 수집 속도
    //public float delayBeforeCollect = 1f; // 수집 전 딜레이


    //private bool isCollecting = false;
    //private float spawnTime;


    //void Update()
    //{
    //    if (player == null) return;

    //    float distance = Vector3.Distance(transform.position, player.position);

    //    // 생성 직후 잠깐은 수집 안 되게 (튕기는 효과용)
    //    if (!isCollecting && Time.time - spawnTime >= delayBeforeCollect && distance < collectRange)
    //    {
    //        isCollecting = true;
    //        // 중력 끄고 회전 멈춤
    //        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
    //        {
    //            rb.useGravity = false;
    //            rb.velocity = Vector3.zero;
    //            rb.angularVelocity = Vector3.zero;
    //        }
    //    }

    //    if (isCollecting)
    //    {
    //        Vector3 direction = (player.position - transform.position).normalized;
    //        transform.position += direction * moveSpeed * Time.deltaTime;

    //        // 도착하면 수집
    //        if (distance < 0.5f)
    //        {
    //            Collect();
    //        }
    //    }
    //}

    private void Collect()
    {
        Debug.Log("💰 코인 획득!");

        // 여기서 플레이어에 코인 추가 로직 등 처리
        // 예: player.GetComponent<PlayerCoin>().AddCoin(1);

        Destroy(gameObject);
    }
}
