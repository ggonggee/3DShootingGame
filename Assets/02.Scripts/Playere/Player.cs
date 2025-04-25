using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int CurrentHealth ;
    public int MaxHealth = 100;
    private PlayerMove _playerMove;

    private void Awake()
    {
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
            UIManager.Instance.BooldEffect();
        }
    }

}
