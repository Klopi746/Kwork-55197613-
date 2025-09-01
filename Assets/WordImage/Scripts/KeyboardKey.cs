using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class KeyboardKey : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private bool isBackspace;
    public Image buttonImage;
    public TextMeshProUGUI LetterText => letterText;

    public static Action<string> OnKeyPressed;
    public static Action OnBackspacePressed;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (isBackspace)
        {
            // Вызываем событие удаления
            OnBackspacePressed?.Invoke();
        }
        else
        {
            // Вызываем событие нажатия буквы          
            OnKeyPressed?.Invoke(letterText.text[0].ToString());
        }
    }

    
}
