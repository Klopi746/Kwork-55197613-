using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YG;



public class UiManager : MonoBehaviour
{

    public List<LevelDataSO> allLevels = new List<LevelDataSO>();
    private Dictionary<string, Sprite> imageCache = new Dictionary<string, Sprite>();
    [SerializeField] private GameObject parentPrefabLetterUnswer;
    [SerializeField] private GameObject prefabLetterUnswer;
    public Image mainImage;
    private int countLettersUnswer;
    public string answer;
    public string hint;
    public CoinAnimation coinAnimation;
    public List<GameObject> unswerPrefabs;
    public List<string> currentAnswer;
    public GameObject[] virtualKeys;
    public TextMeshProUGUI textUiMoney;
    public ListChecker checkerList;
    public AudioClip keyClickSound; // Аудиоклип для звука клика
    private AudioSource audioSource; // Компонент для проигрывания звука
    private bool isProcessingInput = false; // Флаг для предотвращения спама ввода
    

    //private List<string> imageUrls; // Список URL-адресов изображений
    //private int currentImageIndex = 0; // Индекс текущего изображения
    //private Sprite nextSprite; // Предварительно загруженный спрайт для следующего изображения
    public int currentLetterIndex = -1;

    private void Start()
    {
        allLevels = GameManager.Instance.levelManager.levels;
        PreloadNextLevels(allLevels);
        // Получаем или добавляем AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        Data.OnMoneyUpdate += PurchaseUpdateUiMoney;
        textUiMoney.text = GameManager.Instance.data.Coins.ToString();
        //Debug.Log(GameManager.Instance.data.Coins.ToString() + " GameManager.Instance.data.Coins.ToString();");
   
    }

    private void Update()
    {
        
        if (!isProcessingInput)
        {
            if (Input.GetKeyDown(KeyCode.Backspace) )
            {
                Debug.Log("<");
                HighlightVirtualKey("<");
                onBackspacePressed();
            }

            foreach (char c in Input.inputString)
            {
                if (char.IsLetter(c) && currentLetterIndex < unswerPrefabs.Count)
                {
                    // Добавляем букву, если это буква и есть место
                    Debug.Log(c.ToString());
                    onKeyPressed(c.ToString().ToUpper());
                    HighlightVirtualKey(c.ToString().ToUpper());
                }
            }

            
        }

       

    }

    private void OnDestroy()
    {
        Data.OnMoneyUpdate -= PurchaseUpdateUiMoney;
    }
 
    private void PurchaseUpdateUiMoney(int value)
    {
        textUiMoney.text = value.ToString();
    }

    public void InitUiLvl(LevelDataSO level)
    {
        // mainImage.sprite = null;
        // Отписка от событий
        KeyboardKey.OnKeyPressed -= onKeyPressed;
        KeyboardKey.OnBackspacePressed -= onBackspacePressed;
        UnswerUI.OnKeyPressed -= onKeyPressedRemoveUnswerUi;

        // Загрузка изображения
        if (level.image != null)
        {
            Debug.Log("Установка изображения из проекта");
            mainImage.sprite = level.image;
            //mainImage.SetNativeSize();
        }
        else if (!string.IsNullOrEmpty(level.imageUrl))
        {
            if (imageCache.ContainsKey(level.imageUrl) && imageCache[level.imageUrl] != null)
            {
                Debug.Log($"Изображение найдено в кэше: {level.imageUrl}");
                mainImage.sprite = imageCache[level.imageUrl];
                //mainImage.SetNativeSize();
            }
            else
            {
                Debug.Log($"Запуск загрузки изображения из URL: {level.imageUrl}");
                StartCoroutine(LoadImageFromUrl(level.imageUrl, (sprite) =>
                {
                    if (sprite != null)
                    {
                        mainImage.sprite = sprite;
                        //mainImage.SetNativeSize();
                        Debug.Log($"Изображение успешно установлено: {sprite.name}");
                    }
                    else
                    {
                        Debug.LogError("Не удалось загрузить изображение из URL");
                    }
                }));
            }
        }
        else
        {
            Debug.LogWarning("URL изображения пустой, и изображение в LevelDataSO отсутствует");
        }

        countLettersUnswer = level.correctWord.Length;
        answer = level.correctWord;
        currentLetterIndex = -1;
        RemoveAllAnswerUI();
        if (unswerPrefabs.Count > 0)
        {
            foreach (var item in unswerPrefabs)
            {
                Destroy(item.gameObject);
            }
        }

        unswerPrefabs = new List<GameObject>(countLettersUnswer);
        currentAnswer = new List<string>(new string[countLettersUnswer]);

        for (int i = 0; i < currentAnswer.Count; i++)
        {
            currentAnswer[i] = "";
        }

        InitUiUnswerLetters();

        KeyboardKey.OnKeyPressed += onKeyPressed;
        KeyboardKey.OnBackspacePressed += onBackspacePressed;
        UnswerUI.OnKeyPressed += onKeyPressedRemoveUnswerUi;

        // Предзагрузка следующих уровней
        List<LevelDataSO> nextLevels = GetNextLevels();
        if (nextLevels != null && nextLevels.Count > 0)
        {
            PreloadNextLevels(nextLevels);
        }
    }

    private List<LevelDataSO> GetNextLevels()
    {
        int currentIndex = YG2.saves.currentIndexLvl/* Индекс текущего уровня */;
        List<LevelDataSO> nextLevels = new List<LevelDataSO>();
        for (int i = currentIndex + 1; i < allLevels.Count && i < currentIndex + 3; i++) // Предзагрузка 2-3 уровней
        {
            nextLevels.Add(allLevels[i]);
        }
        return nextLevels;
    }

    private void onKeyPressedRemoveUnswerUi(int index)
    {

        //if ((currentAnswer.Count > 0 && currentAnswer.Count >= currentLetterIndex) && index < currentLetterIndex)
        //{
     
           
            unswerPrefabs[index].GetComponent<UnswerUI>().LetterText.text = "";
            currentAnswer[index] = "";
           
        //}
        
    }

    public void RemoveUnswerUiByIndex(int index)
    {

        //if ((currentAnswer.Count > 0 && currentAnswer.Count >= currentLetterIndex) && index < currentLetterIndex)
        //{


        unswerPrefabs[index].GetComponent<UnswerUI>().LetterText.text = "";
        currentAnswer[index] = "";

        //}

    }

    public void RemoveAllAnswerUI()
    {
        for (int i = 0; i < unswerPrefabs.Count; i++)
        {
            unswerPrefabs[i].GetComponent<UnswerUI>().LetterText.text = "";
            currentAnswer[i] = "";
        }      
    }
    private void onBackspacePressed()
    {
        int indexBackspace = countLettersUnswer - 1;

        while(unswerPrefabs[indexBackspace].GetComponent<UnswerUI>().LetterText.text == "" 
            && indexBackspace != 0)
        {            
            
            indexBackspace--;
        }
        unswerPrefabs[indexBackspace].GetComponent<UnswerUI>().LetterText.text = "";
        currentAnswer[indexBackspace] = "";       
    }

    private void onKeyPressed(string letter)
    {
        // Проигрываем звук клика
        if (audioSource != null && keyClickSound != null)
        {
            audioSource.PlayOneShot(keyClickSound);
        }
        var indexAddLetter = 0;

        while (unswerPrefabs[indexAddLetter].GetComponent<UnswerUI>().LetterText.text != ""
            && indexAddLetter != (countLettersUnswer - 1))
        {
            //Debug.Log(indexAddLetter + " indexAddLetter");
            
            indexAddLetter++;
        }

        

        if (countLettersUnswer - 1 == indexAddLetter && 
            unswerPrefabs[indexAddLetter].GetComponent<UnswerUI>().LetterText.text != "")
        {
            return;
        }

        //Debug.Log(indexAddLetter + " indexAddLetter11111.... " + (countLettersUnswer - 1));
        unswerPrefabs[indexAddLetter].GetComponent<UnswerUI>().LetterText.text = letter;
        currentAnswer[indexAddLetter] = letter;  

        //Debug.Log(unswerPrefabs.Count - 1 + " /////....///// " + countLettersUnswer);
        if (unswerPrefabs.Count  == countLettersUnswer)
        {
            Debug.Log("TUTA");
            List<string> charList = answer.Select(c => c.ToString()).ToList();
            //Debug.Log(checkWin(charList, currentAnswer) + " checkWin(unswerPrefabs, currentAnswer)");
            //Debug.Log(answer.Length + " answer.Length");
            


            CheckOutcome checkOutcome = checkerList.CheckWin(charList, currentAnswer);
            if(checkOutcome.Result == CheckResult.Identical)
            {

              
                coinAnimation.OnVictory(); 
                //foreach (var item in charList)
                //{
                //    Debug.Log(item + " charList");
                //}
                //foreach (var item in currentAnswer)
                //{
                //    Debug.Log(item + " currentAnswer");
                //}
            } else if(checkOutcome.Result == CheckResult.OneLetterDifference)
            {
                   
                UnswerUI unswerUI = unswerPrefabs[checkOutcome.Index].GetComponent<UnswerUI>();
                unswerUI.bgImage.color = Color.red;
                unswerUI.Shake();
                //RemoveUnswerUiByIndex(checkOutcome.Index);

            }
            else if (checkOutcome.Result == CheckResult.MultipleDifferences)
            {
                foreach (var item in unswerPrefabs)
                {
                    UnswerUI unswerUI = item.GetComponent<UnswerUI>();
                   
                    unswerUI.Shake();
                    //RemoveAllAnswerUI();
                }
            }

        }


    }

    public void InitUiUnswerLetters()
    {
        
        for (int i = 0; i < countLettersUnswer; i++)
        {
            GameObject unswerLetterPrefab = Instantiate(prefabLetterUnswer, parentPrefabLetterUnswer.transform);
            UnswerUI unswerUi = unswerLetterPrefab.GetComponent<UnswerUI>();
            unswerUi.index = i;
            //Debug.Log("unswerLetterText " + unswerLetterText);
            unswerPrefabs.Add(unswerLetterPrefab);
        }
        
    }

    public void СheckWin()
    {
        if (unswerPrefabs.Count == countLettersUnswer)
        {
            
            List<string> charList = answer.Select(c => c.ToString()).ToList();
            CheckOutcome checkOutcome = checkerList.CheckWin(charList, currentAnswer);
            if (checkOutcome.Result == CheckResult.Identical)
            {

               
                coinAnimation.OnVictory();
              
            }
        }
    }

    private void HighlightVirtualKey(string letter)
    {
        
        // Предположим, у вас есть массив кнопок виртуальной клавиатуры
        // Например, GameObject[] virtualKeys с компонентом Button и текстом
        foreach (var key in virtualKeys) // virtualKeys нужно определить как поле класса
        {
            if (key.GetComponentInChildren<TextMeshProUGUI>().text == letter)
            {
                // Подсветка или анимация кнопки
                // Например, временно меняем цвет
              
                var buttonImage = key.GetComponent<KeyboardKey>().buttonImage;
                //foreach (var key1 in virtualKeys) // virtualKeys нужно определить как поле класса
                //{

                //    Color originalColor = buttonImage.color;
                //    var test = key1.GetComponent<KeyboardKey>().buttonImage;
                //    test.color = originalColor;


                //}
                // Проигрываем звук клика
                if (audioSource != null && keyClickSound != null)
                {
                    audioSource.PlayOneShot(keyClickSound);
                }
                StartCoroutine(FlashButton(buttonImage));
                break;
            }

            if (key.GetComponentInChildren<TextMeshProUGUI>().text == letter)
            {
                // Подсветка или анимация кнопки
                // Например, временно меняем цвет

                var buttonImage = key.GetComponent<KeyboardKey>().buttonImage;
                //foreach (var key1 in virtualKeys) // virtualKeys нужно определить как поле класса
                //{

                //    Color originalColor = buttonImage.color;
                //    var test = key1.GetComponent<KeyboardKey>().buttonImage;
                //    test.color = originalColor;


                //}
                // Проигрываем звук клика
                if (audioSource != null && keyClickSound != null)
                {
                    audioSource.PlayOneShot(keyClickSound);
                }
                StartCoroutine(FlashButton(buttonImage));
                StartCoroutine(ProcessBackspace());
                break;
            }
        }
    }

    // Корутина для визуального эффекта нажатия кнопки
    private System.Collections.IEnumerator FlashButton(Image buttonImage)
    {      
        isProcessingInput = true;
        Color originalColor = buttonImage.color;
        buttonImage.color = Color.gray; // Подсветка
        yield return new WaitForSeconds(0.2f);
        buttonImage.color = originalColor; // Возврат к исходному цвету
        isProcessingInput = false;
    }
    // Корутина для обработки Backspace
    private IEnumerator ProcessBackspace()
    {
        isProcessingInput = true;
        onBackspacePressed();
        yield return new WaitForSeconds(0.2f);
        isProcessingInput = false;
    }
    private IEnumerator LoadImageFromUrl(string url, Action<Sprite> callback)
    {
        Debug.Log($"Попытка загрузки по URL: {url}");
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                imageCache[url] = sprite; // Сохраняем в кэш
                Debug.Log($"Изображение успешно загружено: {url}");
                callback?.Invoke(sprite); // Вызываем callback с загруженным sprite
            }
            else
            {
                Debug.LogError($"Ошибка загрузки изображения: {www.error}, URL: {url}");
                callback?.Invoke(null); // Вызываем callback с null при ошибке
            }
        }
    }

    // Предзагрузка следующих уровней (опционально)
    private void PreloadNextLevels(List<LevelDataSO> levels)
    {
        int maxPreload = 2; // Ограничение на количество предзагружаемых уровней
        for (int i = 0; i < Mathf.Min(levels.Count, maxPreload); i++)
        {
            var level = levels[i];
            if (!string.IsNullOrEmpty(level.imageUrl) && !imageCache.ContainsKey(level.imageUrl))
            {
                Debug.Log($"Предзагрузка изображения для уровня: {level.imageUrl}");
                StartCoroutine(LoadImageFromUrl(level.imageUrl, (sprite) =>
                {
                    if (sprite != null)
                    {
                        imageCache[level.imageUrl] = sprite;
                    }
                }));
            }
        }
        //StartCoroutine(PreloadNextImagesCoroutine(levels));
    }

    private IEnumerator PreloadNextImagesCoroutine(List<LevelDataSO> levels)
    {
        foreach (var level in levels)
        {
            if (!string.IsNullOrEmpty(level.imageUrl) && !imageCache.ContainsKey(level.imageUrl))
            {
                yield return StartCoroutine(LoadImageFromUrl(level.imageUrl, (sprite) =>
                {
                    if (sprite != null)
                    {
                        Debug.Log($"Предзагружено изображение для уровня: {level.imageUrl}");
                    }
                    else
                    {
                        Debug.LogError($"Не удалось предзагрузить изображение: {level.imageUrl}");
                    }
                }));
            }
        }
        
    }

}
