using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GestureRecognition : MonoBehaviour {
    private List<Vector2> recordedPath = new List<Vector2>(); // записанные точки пути

    public Action<Gesture, Vector2> OnRecognised;
    public static GestureRecognition Instance;

    [SerializeField]
    private float _minVecDistance = 0.05f;

    [SerializeField]
    private float _roundingVectorClamp = 15f;

    [SerializeField]
    private float _threshold = 0.5f; // допустимое отклонение между векторами

    [SerializeField]
    private float _sameDirThreshold = 10f; // допустимое отклонение между векторами

    [SerializeField]
    private List<Vector2> _circle, _star, _triangle;

    private void Awake() {
        Instance = this;
    }

    void Update() {
        // Отслеживаем движение мыши
        if (Input.GetMouseButton(0)) // если нажата левая кнопка мыши
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (recordedPath.Count == 0 || Vector2.Distance(mousePosition, recordedPath[^1]) > _minVecDistance) {
                recordedPath.Add(mousePosition);
            }
        }

        // Когда кнопка отпущена, проверяем путь
        if (Input.GetMouseButtonUp(0)) {
            List<Vector2> normalizedPath = NormalizeGesture(recordedPath);
            Vector2 center = FindMiddlePoint(recordedPath);
            if (CompareGestures(normalizedPath, _circle)) {
                OnRecognised?.Invoke(Gesture.Square, center);
            } else if (CompareGestures(normalizedPath, _star)) {
                OnRecognised?.Invoke(Gesture.Star, center);
            } else if (CompareGestures(normalizedPath, _triangle)) {
                OnRecognised?.Invoke(Gesture.Triangle, center);
            } else {
                Debug.Log("Unknown gesture.\n" + normalizedPath.Select(v => $"[{v.x},{v.y}]").Aggregate("", (total, vs) => total + vs + "\n"));
            }

            recordedPath.Clear(); // очищаем путь для следующего жеста
        }
    }

    List<Vector2> NormalizeGesture(List<Vector2> path) {
        List<Vector2> normalized = new List<Vector2>();

        if (path.Count < 2) return normalized;

        // Первый вектор всегда добавляется
        Vector2 previousDirection = (path[1] - path[0]).normalized;
        normalized.Add(RoundVector(previousDirection));

        for (int i = 2; i < path.Count; i++) {
            Vector2 currentDirection = (path[i] - path[i - 1]).normalized;
            currentDirection = RoundVector(currentDirection);
            // Проверяем угол между текущим и предыдущим вектором
            if (Vector2.Angle(previousDirection, currentDirection) > _sameDirThreshold) // Порог в 10 градусов
            {
                normalized.Add(currentDirection);
                previousDirection = currentDirection; // Обновляем предыдущий вектор
            }
        }

        return normalized;
    }

    // Сравнение двух жестов
    bool CompareGestures(List<Vector2> path, List<Vector2> gesture) {
        if (path.Count != gesture.Count) return false;

        for (int i = 0; i < path.Count; i++) {
            if (Vector2.Angle(path[i], gesture[i]) > _threshold)
                return false;
        }

        return true;
    }

    // Округление вектора к ближайшему направлению, кратному 15 градусам
    Vector2 RoundVector(Vector2 vector) {
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg; // Угол в градусах
        float roundedAngle = Mathf.Round(angle / _roundingVectorClamp) * _roundingVectorClamp; // Округляем к ближайшему

        float radians = roundedAngle * Mathf.Deg2Rad; // Конвертируем обратно в радианы
        Vector2 res = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
        if (Mathf.Abs(res.x) < 0.01f) {
            res.x = 0;
        }

        if (Mathf.Abs(res.y) < 0.01f) {
            res.y = 0;
        }

        if (Mathf.Approximately(Mathf.Abs(res.x), Mathf.Abs(res.y)) && res.x != 0) {
            res.x = Mathf.Sign(res.x);
            res.y = Mathf.Sign(res.y);
        }

        return res; // Вектор направления
    }

    Vector2 FindMiddlePoint(List<Vector2> path) {
        if (path == null || path.Count == 0) return Vector2.zero;

        Vector2 sum = Vector2.zero;
        foreach (var point in path) {
            sum += point;
        }

        return sum / path.Count;
    }
}