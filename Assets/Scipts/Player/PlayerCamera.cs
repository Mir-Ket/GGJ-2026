using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;
    private float currentTilt;
    private float firstFovValue;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        firstFovValue = getFovValue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * Time.deltaTime * sensX;
        float mouseY = mouseDelta.y * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, currentTilt);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().fieldOfView = endValue;
    }

    public float getFovValue()
    {
        return this.GetComponent<Camera>().fieldOfView;
    }

    public float getFirstFovValue()
    {
        return firstFovValue;
    }

    public void DoTilt(float zTilt)
    {
        //transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);

        DOTween.To(() => currentTilt, x => currentTilt = x, zTilt, 0.25f);
    }


}
