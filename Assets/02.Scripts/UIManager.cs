using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("�����̴�")]
    [SerializeField] private Slider staminaSlider;
    public TextMeshProUGUI BombCountText;

    private void Awake()
    {
        // �̱��� �ʱ�ȭ
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
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = max;
            staminaSlider.value = current;
        }
    }

    public void SetBomb(int current, int max)
    {
        if(BombCountText != null)
        {
            BombCountText.text = $"��ź ����: {current}/{max}";
        }
    }
}
