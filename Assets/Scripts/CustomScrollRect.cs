using UnityEngine;
using UnityEngine.UI;

public class CustomScrollRect : ScrollRect {
    // Отключаем все вводы, кроме взаимодействия со скроллбаром
    public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData) {
        // Не реагируем на drag
    }

    public override void OnScroll(UnityEngine.EventSystems.PointerEventData data) {
        // Не реагируем на scroll
    }
}