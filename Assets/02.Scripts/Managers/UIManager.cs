using System.Collections;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("슬라이더")]
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Slider _reloadSlider;
    public Slider PlayerHpSlider;

    public TextMeshProUGUI BombCountText;
    public TextMeshProUGUI BulletCountText;
    public Image BloodIamge;
    private float BloodIDuration = 0.05f;

    public GameObject ReadyPanel;
    public TextMeshProUGUI ReadyText;
    public GameObject GameOverPanel;

    public Sprite[] WeaponSprites;
    public Image CurrentWeaponImage;

    public void SetCurrentWeapon(WeaponMode weaponMode)
    {
       CurrentWeaponImage.sprite = WeaponSprites[(int)weaponMode];
    }

    public void SetPlayerHP(float hp)
    {
        PlayerHpSlider.value = hp;
    }

    public void BooldEffect()
    {
        BloodIamge.color = new Color(1f, 1f, 1f, 1f);
        StartCoroutine(BloodEffectDuation());
    
    }    IEnumerator BloodEffectDuation()

    {
        float alpha = BloodIamge.color.a;
        while (BloodIamge.color.a >= 0)
        {
            yield return new WaitForSeconds(BloodIDuration);
            alpha -= 0.1f;
            BloodIamge.color = new Color(1f, 1f, 1f, alpha);
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
            BombCountText.text = $"폭탄 개수: {current}/{max}";
        }
    }

    public void SetBullet(int bullet, int max)
    {
        BulletCountText.text = $"탄환 : {bullet}/{max}";
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
