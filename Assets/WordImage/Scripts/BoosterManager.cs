using System;
using System.Linq;
using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static BoosterDataSO;

public class BoosterManager : MonoBehaviour
{
    [SerializeField] private BoosterDataSO boosterData; // Ссылка на ScriptableObject
    private PopupOpener popupOpener;
    public Popup popupHint;
    public TextMeshProUGUI text;

    private bool hintUsed = false;
    public BoosterUi boosterUi;

    // Ссылки на UI-элементы (заранее свёрстанные кнопки и тексты)
    [System.Serializable]
    public struct BoosterUI
    {
        public BoosterDataSO.BoosterType type;
        public Toggle button; // Кнопка бустера
        public TextMeshProUGUI costText; // Текстовое поле для стоимости
    }

    [SerializeField] private BoosterUI[] boosterUIs; // Массив UI-элементов для бустеров

    private void Start()
    {
        popupOpener = GetComponent<PopupOpener>();
        


    }

   

    // Получение данных о бустере из ScriptableObject
    private BoosterDataSO.BoosterInfo GetBoosterInfo(BoosterDataSO.BoosterType type)
    {
        foreach (var booster in boosterData.boosters)
        {
            if (booster.type == type)
                return booster;
        }
        return new BoosterDataSO.BoosterInfo(); // Возвращаем пустую структуру, если не найдено
    }

    // Обновление UI для всех бустеров
    public void UpdateBoosterUI()
    {
        foreach (var boosterUI in boosterUIs)
        {
           
            // Находим данные о бустере по типу
            var boosterInfo = GetBoosterInfo(boosterUI.type);

            // Обновляем текст стоимости
            boosterUI.costText.text = boosterInfo.cost.ToString();
            // Настраиваем кнопку
            //boosterUI.button.interactable = playerCoins >= boosterInfo.cost;
            // Привязываем событие клика
            boosterUI.button.onValueChanged.RemoveAllListeners();
            boosterUI.button.onValueChanged.AddListener((a) => ActivateBooster(boosterUI.type));

            if (boosterUI.type == BoosterType.Hint)
            {
                Data.OnChangeCurrentIndexLvl += OnChangeCurrentIndexLvl;
            }

        }
    }

    

    // Активация бустера
    private void ActivateBooster(BoosterDataSO.BoosterType type)
    {
        var boosterInfo = GetBoosterInfo(type);
        //if (playerCoins < boosterInfo.cost)
        //{
        //    Debug.Log("Недостаточно монет!");
        //    return;
        //}
        Debug.Log("BoosterType.Loopa 0");
        //playerCoins -= boosterInfo.cost;
        switch (type)
        {
            case BoosterDataSO.BoosterType.Loopa:

                if (GameManager.Instance.data.Coins >= boosterInfo.cost)
                {
                    GameManager.Instance.data.Coins -= boosterInfo.cost;
                    Debug.Log("BoosterType.Loopa");
                }
                else
                {
                    Debug.Log("qqqqqqqqqqqqq");
                    popupOpener.OpenPopup();
                    //Попап в котором предлагается за яны купить игровые монеты + акации отключение рекламы!
                    return;
                }
                
                
                RevealLetter();
                break;
            case BoosterDataSO.BoosterType.Hint:
                if (GameManager.Instance.data.Coins >= boosterInfo.cost)
                {
                    GameManager.Instance.data.Coins -= boosterInfo.cost;
                    Debug.Log("BoosterType.Loopa");
                }
                else
                {
                    Debug.Log("qqqqqqqqqqqqq");
                    popupOpener.OpenPopup();
                    //Попап в котором предлагается за яны купить игровые монеты + акации отключение рекламы!
                    return;
                }

                HintLvl();
                break;
                //case BoosterDataSO.BoosterType.RemoveLetters:
                //    //RemoveWrongLetters();
                //    break;
        }

        // Обновляем UI после использования бустера
        UpdateBoosterUI();
    }

    private void RevealLetter()
    {
        
        var unswerPrefabs = GameManager.Instance.uiManager.unswerPrefabs;
        string answer = GameManager.Instance.uiManager.answer;
        string[] answers = answer.Select(c => c.ToString()).ToArray();
      
        var index = 0;
        while(unswerPrefabs[index].GetComponent<UnswerUI>().LetterText.text != "" 
            && index < answers.Length - 1)
        {
            index++;
        }
       
        GameManager.Instance.uiManager.currentLetterIndex = index;
        GameManager.Instance.uiManager.currentAnswer[index] = answers[index];
        unswerPrefabs[index].GetComponent<UnswerUI>().LetterText.text = answers[index];

        GameManager.Instance.uiManager.СheckWin();

    }

    private void HintLvl()
    {
        if (!hintUsed)
        {
            Debug.Log(GameManager.Instance.data.CurrentIndexLvl + " GameManager.Instance.data.CurrentIndexLvl");
            text.text = boosterData.hints[GameManager.Instance.data.CurrentIndexLvl];
            popupOpener.ShowPopup(popupHint);
            hintUsed = true;
            boosterUi.blockUsed.SetActive(true);
            boosterUi.togle.interactable = false;
        }

    }
    private void OnChangeCurrentIndexLvl()
    {
        hintUsed = false;
        boosterUi.blockUsed.SetActive(false);
        boosterUi.togle.interactable = true;
        popupOpener.HidePopup(popupHint);
    }
}
