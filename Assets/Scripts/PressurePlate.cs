using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Box")]
    public string boxTag = "Box";

    [Header("Plate")]
    public Transform plate;
    public float plateDownDistance = 0.2f;

    [Header("Materials")]
    public Renderer plateRenderer;
    public Material passiveMaterial;
    public Material activeMaterial;

    [Header("Doors")]
    public Transform doorLeft;
    public Transform doorRight;
    public float doorOpenDistance = 2f;

    [Header("Animation")]
    public float speed = 3f;

    private Vector3 plateStartPosition;
    private Vector3 plateDownPosition;

    private Vector3 leftDoorClosedPosition;
    private Vector3 leftDoorOpenPosition;

    private Vector3 rightDoorClosedPosition;
    private Vector3 rightDoorOpenPosition;

    private bool activated;

    private void Start()
    {
        //  remember position on start
        plateStartPosition = plate.position;
        plateDownPosition = plateStartPosition + Vector3.down * plateDownDistance;

        leftDoorClosedPosition = doorLeft.position;
        leftDoorOpenPosition = leftDoorClosedPosition + Vector3.left * doorOpenDistance;

        rightDoorClosedPosition = doorRight.position;
        rightDoorOpenPosition = rightDoorClosedPosition + Vector3.right * doorOpenDistance;

        // initial state
        plateRenderer.material = passiveMaterial;
    }

    private void Update()
    {
        // smooth plate movement
        Vector3 targetPlatePosition = activated
            ? plateDownPosition
            : plateStartPosition;

        plate.position = Vector3.Lerp(
            plate.position,
            targetPlatePosition,
            speed * Time.deltaTime
        );

        // smooth left door movement
        Vector3 targetLeftDoorPosition = activated
            ? leftDoorOpenPosition
            : leftDoorClosedPosition;

        doorLeft.position = Vector3.Lerp(
            doorLeft.position,
            targetLeftDoorPosition,
            speed * Time.deltaTime
        );

        // smooth right door movement
        Vector3 targetRightDoorPosition = activated
            ? rightDoorOpenPosition
            : rightDoorClosedPosition;

        doorRight.position = Vector3.Lerp(
            doorRight.position,
            targetRightDoorPosition,
            speed * Time.deltaTime
        );
    }

    // start contact with box
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter = true");
        if (other.CompareTag(boxTag))
        {
            Debug.Log("activated = true");
            activated = true;

            // change material
            plateRenderer.material = activeMaterial;
        }
    }

    // end contact with box
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit = true");
        if (other.CompareTag(boxTag))
        {
            Debug.Log("activated = false");
            activated = false;

            // return material
            plateRenderer.material = passiveMaterial;
        }
    }
}