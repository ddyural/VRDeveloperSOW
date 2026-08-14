using UnityEngine;
using System.Collections;

public class MagnetGun : MonoBehaviour
{
    [Header("Точка назначения")]
    [SerializeField] private Transform pointA;

    [Header("Точка начала Raycast")]
    [SerializeField] private Transform rayOrigin;

    [Header("Настройки Raycast")]
    [SerializeField] private float rayDistance = 20f;

    [Header("Настройки движения")]
    [SerializeField] private float moveDuration = 1f;

    private bool isMoving;

    // метод вызывается при нажатии Activate у XR Grab Interactable
    public void ActivateMove()
    {
        // не запускаем новое перемещение пока предыдущее ещё выполняется
        if (isMoving)
            return;

        // проверяем что точка назначения указана в Inspector
        if (pointA == null)
        {
            Debug.LogError("magnetgun: не назначен pointA");
            return;
        }

        // проверяем что точка начала луча указана в Inspector
        if (rayOrigin == null)
        {
            Debug.LogError("magnetgun: не назначен rayOrigin");
            return;
        }

        // ищем объект перед пушкой с помощью Raycast
        GameObject targetObject = FindTarget();

        // прекращаем выполнение если подходящий объект не найден
        if (targetObject == null)
        {
            Debug.Log("magnetgun: подходящий объект не найден");
            return;
        }

        // запускаем плавное перемещение найденного объекта
        StartCoroutine(MoveObject(targetObject));
    }

    private GameObject FindTarget()
    {
        // создаём луч от заданной точки в направлении её локальной оси Z
        Ray ray = new Ray(
            rayOrigin.position,
            rayOrigin.forward
        );

        Debug.Log(
            "magnetgun: raycast из " +
            rayOrigin.position +
            " направление " +
            rayOrigin.forward
        );

        // ищем все Collider которые пересекает луч
        // ~0 позволяет проверять объекты на любых слоях
        RaycastHit[] hits = Physics.RaycastAll(
            ray,
            rayDistance,
            ~0,
            QueryTriggerInteraction.Collide
        );

        Debug.Log(
            "magnetgun: найдено пересечений: " +
            hits.Length
        );

        // сортируем найденные объекты от ближайшего к самому дальнему
        System.Array.Sort(
            hits,
            (a, b) => a.distance.CompareTo(b.distance)
        );

        foreach (RaycastHit hit in hits)
        {
            // получаем объект на котором находится Collider
            GameObject hitObject = hit.collider.gameObject;

            // игнорируем саму пушку и все её дочерние объекты
            if (
                hitObject == gameObject ||
                hitObject.transform.IsChildOf(transform)
            )
            {
                Debug.Log(
                    "magnetgun: пропускаем — это сама пушка: " +
                    hitObject.name
                );

                continue;
            }

            // получаем Rigidbody связанный с найденным Collider
            Rigidbody rb = hit.collider.attachedRigidbody;

            // пропускаем объект если у него нет Rigidbody
            if (rb == null)
            {
                Debug.Log(
                    "magnetgun: пропускаем — у объекта нет Rigidbody: " +
                    hitObject.name
                );

                continue;
            }

            // берём объект которому принадлежит Rigidbody
            GameObject targetObject = rb.gameObject;

            // статические объекты не подходят для перемещения
            if (targetObject.isStatic)
            {
                Debug.Log(
                    "magnetgun: пропускаем — объект Static: " +
                    targetObject.name
                );

                continue;
            }

            // возвращаем первый подходящий объект найденный лучом
            Debug.Log(
                "magnetgun: найден целевой объект: " +
                targetObject.name
            );

            return targetObject;
        }

        // если подходящих объектов нет возвращаем null
        return null;
    }

    private IEnumerator MoveObject(GameObject targetObject)
    {
        isMoving = true;

        // получаем Transform объекта который будем перемещать
        Transform targetTransform = targetObject.transform;

        // сохраняем начальную позицию объекта
        Vector3 pointB = targetTransform.position;

        // получаем конечную позицию из Inspector
        Vector3 targetPosition = pointA.position;

        // получаем Rigidbody найденного объекта
        Rigidbody rb = targetObject.GetComponent<Rigidbody>();

        // запоминаем исходное состояние физики
        bool wasKinematic = false;

        if (rb != null)
        {
            wasKinematic = rb.isKinematic;

            // временно отключаем физическое воздействие во время движения
            rb.isKinematic = true;
        }

        float time = 0f;

        // постепенно увеличиваем время движения до заданной продолжительности
        while (time < moveDuration)
        {
            time += Time.deltaTime;

            // переводим текущее время в диапазон от 0 до 1
            float t = time / moveDuration;

            // делаем начало и конец движения более плавными
            t = Mathf.SmoothStep(0f, 1f, t);

            // вычисляем новую позицию между начальной и конечной точкой
            Vector3 newPosition = Vector3.Lerp(
                pointB,
                targetPosition,
                t
            );

            // перемещаем Rigidbody если он существует
            if (rb != null)
            {
                rb.position = newPosition;
            }
            else
            {
                // запасной вариант для объекта без Rigidbody
                targetTransform.position = newPosition;
            }

            yield return null;
        }

        // устанавливаем точную конечную позицию после завершения движения
        if (rb != null)
        {
            rb.position = targetPosition;

            // возвращаем Rigidbody его исходное состояние
            rb.isKinematic = wasKinematic;
        }
        else
        {
            targetTransform.position = targetPosition;
        }

        // разрешаем следующее перемещение
        isMoving = false;
    }

    private void OnDrawGizmos()
    {
        // не рисуем луч если точка начала не назначена
        if (rayOrigin == null)
            return;

        Gizmos.color = Color.red;

        // показываем направление и длину Raycast в Scene View
        Gizmos.DrawRay(
            rayOrigin.position,
            rayOrigin.forward * rayDistance
        );
    }
}