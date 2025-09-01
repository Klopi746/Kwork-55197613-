using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    // Статическая переменная для хранения экземпляра
    public static GameManager Instance { get; private set; }


    [SerializeField] public UiManager uiManager;
    [SerializeField] public LevelManager levelManager;
    [SerializeField] public Data data;

    private void Awake()
    {
        // Проверяем, если уже есть другой экземпляр
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Уничтожаем дубликат
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Сохраняем объект при смене сцены
    }

    private void Start()
    {
        if (YG2.saves.NOTads)
        {
            YG2.StickyAdActivity(false);
        }
        if (YG2.saves.currentIndexLvl <= 0)
        {
            Debug.Log("YG2.saves.currentIndexLvl <= 0 " + YG2.saves.currentIndexLvl);
            levelManager.LoadLevel(YG2.saves.currentIndexLvl);
        }
        else
        {
            Debug.Log("elseelse YG2.saves.currentIndexLvl <= 0 elseelse");
            levelManager.LoadLevel(YG2.saves.currentIndexLvl);
        }
        
    }



    // Вызывается при уничтожении объекта
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
