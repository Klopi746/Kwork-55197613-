using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace YG.Example
{
    public class ReceivingPurchaseExample : MonoBehaviour
    {
        public Text textExample;
        public static Action ActionSuccessPurchased;
        public static Action FailedPur;
        // Пример Unity событий, на которые можно подписать,
        // например, открытие уведомление о успешности совершения покупки
        public UnityEvent successPurchased;
        public static UnityEvent failedPurchased;

        private void OnEnable()
        {
            YG2.onPurchaseSuccess += SuccessPurchased;
            YG2.onPurchaseFailed += FailedPurchased;
        }

        private void OnDisable()
        {
            YG2.onPurchaseSuccess -= SuccessPurchased;
            YG2.onPurchaseFailed -= FailedPurchased;
        }

        private void SuccessPurchased(string id)
        {
            //successPurchased?.Invoke();
            
            //textExample.text = "Success purchase - " + id;

            // Ваш код для обработки покупки. Например:

            //string coinsKey = "coins";
            //int coins = YG2.GetState(coinsKey);

            if (id == "NOTads")
            {
                YG2.saves.NOTads = true;
                GameManager.Instance.data.Coins += 400;
                YG2.StickyAdActivity(false);
                ActionSuccessPurchased?.Invoke();
              
            }

            if (id == "ADDmoney")
            {
                GameManager.Instance.data.Coins += 150;
            }


        }

        private void FailedPurchased(string id)
        {
            failedPurchased?.Invoke();

            //textExample.text = "Failed purchase - " + id;
        }
    }
}