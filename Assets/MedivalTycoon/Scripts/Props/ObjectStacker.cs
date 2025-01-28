using System;
using UnityEngine;

public class ObjectStacker : MonoBehaviour
{
    public GameObject objectPrefab; // Префаб объекта, который будет складываться
    public int numberOfObjects = 5; // Количество объектов для стека
    public float stackOffset = 1.0f; // Расстояние между объектами в стеке
    public float parabolaHeight = 5.0f; // Высота параболы
    public float moveDuration = 2.0f; // Время движения по параболе

    public Vector3 targetPosition; // Целевая позиция для стека

    private void Start()
    {
        OnPlayerReachedPoint(targetPosition);
    }

    public void OnPlayerReachedPoint(Vector3 point)
    {
        targetPosition = point;
        StartCoroutine(StackObjects());
    }

    private System.Collections.IEnumerator StackObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject newObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            Vector3 startPosition = transform.position;
            Vector3 endPosition = targetPosition + Vector3.up * (i * stackOffset);

            float elapsedTime = 0;

            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / moveDuration;

                // Параболическое движение
                float height = Mathf.Sin(t * Mathf.PI) * parabolaHeight;
                newObject.transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * height;

                yield return null;
            }

            newObject.transform.position = endPosition; // Убедимся, что объект точно на целевой позиции
        }
    }
}