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
    [SerializeField] bool _shooting;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
            ShootControl();
            if (Physics.Raycast(_camRaycast, out hit, _raycastDistance, _layerMask))
            {
                
                if (hit.collider.TryGetComponent(out HealthSystem healthSystem))
                {
                    if (_shooting==true)
                    {
                        healthSystem.HealthDecrease(10);
                    }
                    
                }
                Debug.Log("Enemy");
            }
       
        }

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
            Debug.Log("Reload Atýldý");
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
