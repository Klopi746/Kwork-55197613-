
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Game/Level")]
public class LevelDataSO : ScriptableObject
{
    public int levelNumber; // Номер уровня
    public Sprite image;
    public string correctWord;
    public string imageUrl;
    //public string hint;
}
