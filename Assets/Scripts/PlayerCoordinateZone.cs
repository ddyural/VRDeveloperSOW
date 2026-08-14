using UnityEngine;

public class PlayerCoordinateZone : MonoBehaviour
{
    // камера игрока
    [SerializeField] private Transform playerCamera;

    // первая точка зоны - крайняя правая верхняя задняя
    [SerializeField] private Transform pointA;

    // вторая точка зоны - крайняя левая верхняя передняя
    [SerializeField] private Transform pointB;

    // объект, который будет включаться
    [SerializeField] private GameObject objectToActivate;

    // объект с Mesh Renderer
    [SerializeField] private GameObject meshRendererObject;

    // материал вне зоны
    [SerializeField] private Material normalMaterial;

    // материал внутри зоны
    [SerializeField] private Material activeMaterial;

    // ссылка на Mesh Renderer
    private MeshRenderer meshRenderer;

    // состояние игрока
    private bool playerIsInside;


    private void Start()
    {
        // получаем Mesh Renderer
        if (meshRendererObject != null)
        {
            meshRenderer = meshRendererObject.GetComponent<MeshRenderer>();
        }

        // устанавливаем обычный материал
        if (meshRenderer != null && normalMaterial != null)
        {
            meshRenderer.material = normalMaterial;
        }
    }


    private void Update()
    {
        // проверяем, что камера и две точки назначены
        if (playerCamera == null || pointA == null || pointB == null)
        {
            return;
        }

        // получаем позицию игрока
        Vector3 playerPosition = playerCamera.position;

        // получаем минимальные координаты зоны
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float minY = Mathf.Min(pointA.position.y, pointB.position.y);
        float minZ = Mathf.Min(pointA.position.z, pointB.position.z);

        // получаем максимальные координаты зоны
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
        float maxY = Mathf.Max(pointA.position.y, pointB.position.y);
        float maxZ = Mathf.Max(pointA.position.z, pointB.position.z);

        // проверяем, находится ли игрок внутри зоны
        bool isInside =
            playerPosition.x >= minX &&
            playerPosition.x <= maxX &&
            playerPosition.y >= minY &&
            playerPosition.y <= maxY &&
            playerPosition.z >= minZ &&
            playerPosition.z <= maxZ;

        // если состояние изменилось
        if (isInside != playerIsInside)
        {
            playerIsInside = isInside;

            // игрок вошел в зону
            if (playerIsInside)
            {
                EnterZone();
            }
            // игрок вышел из зоны
            else
            {
                ExitZone();
            }
        }
    }


    private void EnterZone()
    {
        // меняем материал
        if (meshRenderer != null && activeMaterial != null)
        {
            meshRenderer.material = activeMaterial;
        }

        // включаем объект
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }


    private void ExitZone()
    {
        // возвращаем обычный материал
        if (meshRenderer != null && normalMaterial != null)
        {
            meshRenderer.material = normalMaterial;
        }
    }
}