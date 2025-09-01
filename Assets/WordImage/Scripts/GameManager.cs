using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    // ����������� ���������� ��� �������� ����������
    public static GameManager Instance { get; private set; }


    [SerializeField] public UiManager uiManager;
    [SerializeField] public LevelManager levelManager;
    [SerializeField] public Data data;

    private void Awake()
    {
        // ���������, ���� ��� ���� ������ ���������
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ���������� ��������
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ��������� ������ ��� ����� �����
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



    // ���������� ��� ����������� �������
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
