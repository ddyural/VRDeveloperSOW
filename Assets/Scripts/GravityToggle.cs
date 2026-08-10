using UnityEngine;

public class GravityToggle : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    private Rigidbody targetRigidbody;

    private void Awake()
    {
        if (targetObject == null)
        {
            Debug.LogError(
                $"[{nameof(GravityToggle)}] Target Object not found",
                this
            );
            return;
        }

        if (!targetObject.TryGetComponent(out targetRigidbody))
        {
            Debug.LogError(
                $"[{nameof(GravityToggle)}] On object '{targetObject.name}' Rigidbody not found",
                targetObject
            );
        }
    }

    public void ToggleGravity()
    {
        if (!CheckRigidbody())
            return;

        targetRigidbody.useGravity = !targetRigidbody.useGravity;
    }

    private bool CheckRigidbody()
    {
        if (targetObject == null)
        {
            Debug.LogWarning(
                $"[{nameof(GravityToggle)}] Target Object not found",
                this
            );
            return false;
        }

        if (targetRigidbody == null)
        {
            Debug.LogWarning(
                $"[{nameof(GravityToggle)}] Rigidbody not found on '{targetObject.name}' ",
                targetObject
            );
            return false;
        }

        return true;
    }

    [ContextMenu("Toggle Gravity (Editor)")]
    private void ContextToggle()
    {
        ToggleGravity();
    }
}
