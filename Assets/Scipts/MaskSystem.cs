using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MaskSystem : MonoBehaviour
{
    [Header("Maske Ayarlarý")]
    public GameObject[] _mask3DObjects; 
    public GameObject[] _maskUIs;       

    private int lastActiveIndex = -1;

    private Grappling _grapplingScript;
    private PlayerMovement _playerMovement;

    void Start()
    {
        DeactivateAll();
        _grapplingScript = Object.FindAnyObjectByType<Grappling>();
        _grapplingScript.enabled = false;

    }

    public void ActivateRandomMask()
    {
        if (lastActiveIndex != -1)
        {
            _mask3DObjects[lastActiveIndex].SetActive(false);
            _maskUIs[lastActiveIndex].SetActive(false);
        }

        int randomIndex = Random.Range(0, _mask3DObjects.Length);

        while(randomIndex == lastActiveIndex) { randomIndex = Random.Range(0, 5); }

        _mask3DObjects[randomIndex].SetActive(true);
        _maskUIs[randomIndex].SetActive(true);

        lastActiveIndex = randomIndex;


        GameObject checkMaskName = _mask3DObjects[randomIndex];
        string maskName = checkMaskName.name;

        switch (maskName)
        {
            case "Grapple Gun":
                Debug.Log("GrableGun");
                _grapplingScript.enabled = true;

                break;
            case "Desert Eagle":
                Debug.Log("Desert Eagle");
                _grapplingScript.enabled = false;
                break;
            case "Katana":
                Debug.Log("Katana");
                _grapplingScript.enabled = false;

                break;
            case "Mic":
                Debug.Log("Mic");
                _grapplingScript.enabled = false;

                break;
            case "Speed":
                Debug.Log("Speed");
                _grapplingScript.enabled = false;
                _playerMovement.limitSpeed = false;
                Debug.Log("Hýzladý");
                break;
                
        }
        Debug.Log("Yeni maske aktif edildi: " + randomIndex);
    }

    private void DeactivateAll()
    {
        foreach (var obj in _mask3DObjects) obj.SetActive(false);
        foreach (var ui in _maskUIs) ui.SetActive(false);
    }
}