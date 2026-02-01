using UnityEngine;

public class GrapplingHookScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject grapplingHook;

    private void OnTriggerEnter(Collider other)
    {
        grapplingHook.SetActive(true);
        other.GetComponentInParent<Grappling>().enabled = true;
    }
}
