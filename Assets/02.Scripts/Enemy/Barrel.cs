using System.Collections;

using JetBrains.Annotations;

using UnityEngine;

public class Barrel : MonoBehaviour
{
    public int Health = 30;
    public float ExplosionPower = 10f;
    public ParticleSystem EffectPrefab;
    private Rigidbody _rigidbody;
    private bool isDead;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        if(Health <= 0)
        {
            isDead = true;
            StartCoroutine(DistroyBarrel());
        }
    }

    IEnumerator DistroyBarrel()
    {
        _rigidbody.AddForce(Vector3.up * ExplosionPower);
        ParticleSystem effect = Instantiate(EffectPrefab,transform.position,Quaternion.identity);
        effect.Play();
        Collider[] cols =  Physics.OverlapSphere(transform.position, 3f);
        foreach(Collider col in cols)
        {
            if (col.transform.CompareTag("Barrel"))
            {
                Barrel barrel = col.transform.GetComponent<Barrel>();
                if (barrel.isDead == false)
                {
                    Damage damage = new Damage();
                    damage.Value = 300;
                    barrel.TakeDamage(damage);
                }
            }
        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
