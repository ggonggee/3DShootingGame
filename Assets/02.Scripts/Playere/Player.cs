using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int CurrentHealth ;
    public int MaxHealth = 100;
    private PlayerMove _playerMove;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerMove =GetComponent<PlayerMove>();
        CurrentHealth = MaxHealth;
    }

    private void Start()
    {
        
    }

    public void TakeDamage(Damage damage)
    {
        if (GameManager.Instance.CurrentGameMove == GameMode.Run)
        {
            CurrentHealth -= damage.Value;
            if(CurrentHealth<= 0)
            {
                GameManager.Instance.CurrentGameMove = GameMode.Over;
            }
            UIManager.Instance.SetPlayerHP((float)CurrentHealth/MaxHealth);
            
            _animator.SetLayerWeight(2, 1 - CurrentHealth/(float)MaxHealth);
            UIManager.Instance.BooldEffect();
        }
    }

}
