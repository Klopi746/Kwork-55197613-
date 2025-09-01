using UnityEngine;
using YG;

public class clearData : MonoBehaviour
{
    
    public void ClearData()
    {
        YG2.SetDefaultSaves();
        YG2.SaveProgress();


    }

    
}
