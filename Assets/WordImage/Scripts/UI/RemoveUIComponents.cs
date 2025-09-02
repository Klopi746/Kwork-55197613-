using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveUIComponents : MonoBehaviour
{
    public void RemoveUIElement()
    {
        // Получаем компоненты
        Image image = GetComponent<Image>();
        LayoutElement layoutElement = GetComponent<LayoutElement>();

        // Удаляем зависимые компоненты
        if (image != null)
            Destroy(image);
        if (layoutElement != null)
            Destroy(layoutElement);

        // Теперь можно удалить RectTransform
        Destroy(this.gameObject);
    }
}
