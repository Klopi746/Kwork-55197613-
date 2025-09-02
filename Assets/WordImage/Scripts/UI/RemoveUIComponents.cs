using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveUIComponents : MonoBehaviour
{
    public void RemoveUIElement()
    {
        // �������� ����������
        Image image = GetComponent<Image>();
        LayoutElement layoutElement = GetComponent<LayoutElement>();

        // ������� ��������� ����������
        if (image != null)
            Destroy(image);
        if (layoutElement != null)
            Destroy(layoutElement);

        // ������ ����� ������� RectTransform
        Destroy(this.gameObject);
    }
}
