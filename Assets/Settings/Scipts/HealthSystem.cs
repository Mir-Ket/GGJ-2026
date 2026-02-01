
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    [Header("Health UI")]
    [SerializeField] Image _healthBarImage;
    [SerializeField] Image _effectBarImage;

    [SerializeField] float _effectDelayer;
    [SerializeField] TextMeshProUGUI _healtText;

    [Header("Health Amount")]
    [SerializeField] float _currentHealth;
    [SerializeField] float _maxHealth;
    [SerializeField] float _minHealth;


    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (_currentHealth>=_maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        if (_currentHealth <= _minHealth)
        {
            _currentHealth = _minHealth;
            Debug.Log("Karakter öldü sahne yenilendi");
        }

        //can denme 
        /* if (Input.GetKeyDown(KeyCode.E))
        {
            HealthIncrease(10);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        { 
            HealthDecrease(10); 
        }*/
            UIEffect();

    }
    private void UIEffect()
    {
        _healtText.text = _currentHealth.ToString();

        if (_healthBarImage.fillAmount!=_effectBarImage.fillAmount)
        {
            _effectBarImage.fillAmount = Mathf.Lerp(_effectBarImage.fillAmount, _healthBarImage.fillAmount, _effectDelayer);
        }
    }

    public void HealthDecrease (float decreaseHealth)
    {
        _currentHealth-=decreaseHealth;
        _healthBarImage.fillAmount = _currentHealth / _maxHealth;
    }

    public void HealthIncrease(float ýncreaseHealth)
    {
        _currentHealth += ýncreaseHealth;
        _healthBarImage.fillAmount = _currentHealth / _maxHealth;
    }

}
