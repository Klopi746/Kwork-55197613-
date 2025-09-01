using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterData", menuName = "Game/BoosterData")]
public class BoosterDataSO : ScriptableObject   
{
    public enum BoosterType
    {
        Loopa,
        Hint
    }

    [System.Serializable]
    public struct BoosterInfo
    {
        public BoosterType type;
        public int cost;           
    }

    public BoosterInfo[] boosters; // Массив данных о бустерах
    public string[] hints;// Массив с подсказками под каждый уровень
}
