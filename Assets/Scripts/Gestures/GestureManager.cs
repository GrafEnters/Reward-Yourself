using System;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour {
    [SerializeField]
    private GestureRecognition _gestureRecognition;

    [SerializeField]
    private GameObject _star, _triangle, _square;

    private Dictionary<Gesture, GameObject> _map;

    private void Start() {
        _gestureRecognition.OnRecognised += LogGesture;
        _map = new Dictionary<Gesture, GameObject>() {
            { Gesture.Star, _star },
            { Gesture.Triangle, _triangle },
            { Gesture.Square, _square },
        };
    }

    private void LogGesture(Gesture g, Vector2 center) {
        Debug.Log($"{g} in {center}");
        var gG = Instantiate(_map[g], center, Quaternion.identity, transform);
        Destroy(gG, 3);
    }
}

[Serializable]
public enum Gesture {
    Square,
    Star,
    Triangle
}