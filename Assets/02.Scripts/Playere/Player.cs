using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int Health = 30;

    public void TakeDamage(Damage damage)
    {
        
        Health -= damage.Value;
        UIManager.Instance.BooldEffect();
    }

}
