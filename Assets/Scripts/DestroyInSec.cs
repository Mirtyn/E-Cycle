using UnityEngine;

public class DestroyInSec : MonoBehaviour
{
    [SerializeField] private float seconds = 1f;

    private void OnEnable()
    {
        Destroy(gameObject, seconds);
    }
}
