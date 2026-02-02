using TMPro;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    [Header("Raycast System")]
    [SerializeField] float _raycastDistance;
    [SerializeField] LayerMask _layerMask;
    private Camera _camera;

    [Header("Shooting System")]
    [SerializeField] TextMeshProUGUI _ammoText;
    [SerializeField] float _currentAmmo;
    [SerializeField] float _MinAmmo;
    [SerializeField] float _maxAmmo;

    [Header("Explode System")]
    [SerializeField] float _explodRange;
    [SerializeField] float _explodDamage;
    [SerializeField] float _rotationSpeed;

    public bool _shooting;
    public bool _attacked;
    public bool _reload;

    [SerializeField] float _Damage;

    private void Awake()
    {
        _currentAmmo = _maxAmmo;
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastControl();
        Reload();

    }
    private void RaycastControl()
    {
        Ray _camRaycast = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            _attacked = true;
            ShootControl();
            if (Physics.Raycast(_camRaycast, out hit, _raycastDistance, _layerMask))
            {
                if (hit.collider.TryGetComponent(out HealthSystem healthSystem))
                {
                    if (_shooting == true)
                    {
                        healthSystem.HealthDecrease(_Damage);
                    }

                }
                Debug.Log("Enemy");
            }
        }
        else { _attacked = false; _reload = false; }
    }
    private void ShootControl()
    {

        if (_currentAmmo<=_MinAmmo)
        {
            _currentAmmo = _MinAmmo;
            _shooting = false;
            Debug.Log(_shooting);

        }

        if (_currentAmmo>=_maxAmmo)
        {
            _currentAmmo = _maxAmmo;
        }
        if (_currentAmmo>_MinAmmo && _currentAmmo<=_maxAmmo)
        {
            _shooting=true;
            Debug.Log(_shooting);
        }
        if(_shooting==true)
        {
            _currentAmmo -= 1;
        }
    }
    private void Reload()
    {
        _ammoText.text = _currentAmmo.ToString();
        if (Input.GetKey(KeyCode.R)&&_currentAmmo<_maxAmmo)
        {
            _currentAmmo = _maxAmmo;
            _reload=true;
            Debug.Log("Reload Atýldý");
        }
    }

    public void Explod()
    {
        if (Physics.CheckSphere(transform.position,_explodRange,_layerMask))
        {
            transform.Rotate(Vector3.up*_rotationSpeed*Time.deltaTime);
        }
    }
    private void OnDrawGizmos()
       {
        Gizmos.color = Color.green;

        if (_camera != null)
        {
            Gizmos.color = Color.green;

            // raycastin nerden baþlayýp nereye vardýðýný kontrol ediyoruz
            Vector3 _endPoint = _camera.transform.position + _camera.transform.forward * _raycastDistance;
            Gizmos.DrawLine(_camera.transform.position, _endPoint);
        }
       }
    }
