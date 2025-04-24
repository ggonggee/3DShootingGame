using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("½½¶óÀÌ´õ")]
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Slider _reloadSlider;


    public TextMeshProUGUI BombCountText;
    public TextMeshProUGUI BulletCountText;

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
            BombCountText.text = $"ÆøÅº °³¼ö: {current}/{max}";
        }
    }

    public void SetBullet(int bullet, int max)
    {
        BulletCountText.text = $"ÅºÈ¯ : {bullet}/{max}";
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
