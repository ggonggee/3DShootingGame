using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 10f;
    public Damage Damage;

    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider Col)
    {
        if (Col.transform.TryGetComponent<IDamageable>(out IDamageable idamageable))
        {
            idamageable.TakeDamage(Damage);
        }
    }
}
