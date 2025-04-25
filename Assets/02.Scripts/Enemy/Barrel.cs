using System.Collections;

using JetBrains.Annotations;

using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    public int Health = 30;
    public float ExplosionPower = 10f;
    public ParticleSystem EffectPrefab;
    private Rigidbody _rigidbody;
    private bool isDead;

    private float ExplodeRange = 3f;
    private int ExplosionDamage = 300;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        if(Health <= 0)
        {
            if (isDead) { return; }
            
            StartCoroutine(DistroyBarrel());
        }
    }

    IEnumerator DistroyBarrel()
    {
        isDead = true;
        ParticleSystem effect = Instantiate(EffectPrefab,transform.position,Quaternion.identity);
        effect.Play();
        _rigidbody.AddForce(Vector3.up * ExplosionPower);
        // 유니티는 레이어를 넘버링하게 아니라 비트로 관리
        // 2진수 -> 0000 0000
        //      1 : 0000 0001
        //      2 : 0000 0010
        //      3 : 0000 0011
        //     17 : 0001 0001
        //    255 : 1111 1111
        // 비트 단위로 on/off를 관리할 수 있다.

        // 드럼통을 감지 안하고 싶고
        //Collider[] cols =  Physics.OverlapSphere(transform.position, ExplodeRange, ~(1<<9));
        //Collider[] cols = Physics.OverlapSphere(transform.position, ExplodeRange, ~LayerMask.NameToLayer("Barrel"));
        Collider[] cols = Physics.OverlapSphere(transform.position, ExplodeRange);
        foreach (Collider col in cols)
        {
            if (col.transform.TryGetComponent<IDamageable>(out IDamageable damageable)  &&
                col.transform.CompareTag("Barrel"))
            {
                    Damage damage = new Damage();
                    damage.Value = ExplosionDamage;
                    damageable.TakeDamage(damage);   
            }

            //if (col.transform.CompareTag("Enemy"))
            //{
            //    Enemy enemy = col.transform.GetComponent<Enemy>();
            //    Damage damage = new Damage();
            //    damage.Value = ExplosionDamage;
            //    enemy.TakeDamage(damage);
            //}
        }

        //cols = Physics.OverlapSphere(transform.position, ExplodeRange, ~LayerMask.NameToLayer("Barrel"));
        //foreach (Collider col in cols)
        //{
        //    if (col.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
        //    {
        //        Damage damage = new Damage();
        //        damage.Value = ExplosionDamage;
        //        damageable.TakeDamage(damage);
        //    }
        //}

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
