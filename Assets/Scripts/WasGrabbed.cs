using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WasGrabbed : MonoBehaviour
{
    [SerializeField]
    private string itemId;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(Grabbed);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(Grabbed);
    }

    public void Grabbed(SelectEnterEventArgs args)
    {
        Debug.Log("взят в руки");

        bool completed = ScenarioManager.Instance.CompleteCurrentStep(itemId);

        if (completed)
        {
            Debug.Log($"Взятие выполнено: {itemId}");
        }
    }
}
