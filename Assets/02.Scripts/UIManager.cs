using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("ΩΩ∂Û¿Ã¥ı")]
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Slider _reloadSlider;


    public TextMeshProUGUI BombCountText;
    public TextMeshProUGUI BulletCountText;

    private void Awake()
    {
        // ΩÃ±€≈œ √ ±‚»≠
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetStamina(float current, float max)
    {
        if (_staminaSlider != null)
        {
            _staminaSlider.maxValue = max;
            _staminaSlider.value = current;
        }
    }

    public void SetBomb(int current, int max)
    {
        if(BombCountText != null)
        {
            BombCountText.text = $"∆¯≈∫ ∞≥ºˆ: {current}/{max}";
        }
    }

    public void SetBullet(int bullet, int max)
    {
        BulletCountText.text = $"≈∫»Ø : {bullet}/{max}";
    }

    public void SetReload(float value, float max, bool isReloading)
    {
        _reloadSlider.value = value;
        _reloadSlider.gameObject.SetActive(isReloading);
        //if (max <= value)
        //{
        //    _reloadslider.gameobject.setactive(false);
        //}
    }
}
