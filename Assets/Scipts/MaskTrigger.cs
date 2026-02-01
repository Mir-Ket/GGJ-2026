using DG.Tweening.Core.Easing;
using UnityEngine;

public class MaskTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MaskSystem manager = Object.FindAnyObjectByType<MaskSystem>();

            if (manager != null)
            {
                manager.ActivateRandomMask();
            }
            // Destroy(gameObject);
        }
    }
}