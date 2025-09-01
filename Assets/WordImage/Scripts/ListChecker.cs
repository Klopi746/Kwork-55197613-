using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum CheckResult
{
    Identical,              // Списки полностью совпадают
    OneLetterDifference,    // Отличается ровно одна буква в одной позиции
    MultipleDifferences,    // Более одного различия или разные длины
    InvalidInput            // Один из списков null или элементы не являются одиночными буквами
}

public struct CheckOutcome
{
    public CheckResult Result { get; set; }
    public int Index { get; set; } // Индекс позиции с различием

    public CheckOutcome(CheckResult result, int index = -1)
    {
        Result = result;
        Index = index;
    }
}
public class ListChecker : MonoBehaviour
{

    public CheckOutcome CheckWin(List<string> list1, List<string> list2)
    {
        // Проверка на null
        if (list1 == null || list2 == null)
        {
            return new CheckOutcome(list1 == null && list2 == null ? CheckResult.Identical : CheckResult.InvalidInput);
        }

        // Проверка на одинаковую длину списков
        if (list1.Count != list2.Count)
        {
            return new CheckOutcome(CheckResult.MultipleDifferences);
        }

        // Проверка, что все элементы — строки из одной буквы
        foreach (var item in list1.Concat(list2))
        {
            if (string.IsNullOrEmpty(item) || item.Length != 1)
            {
                return new CheckOutcome(CheckResult.InvalidInput);
            }
        }

        // Проверка на полное совпадение
        if (list1.SequenceEqual(list2))
        {
            return new CheckOutcome(CheckResult.Identical);
        }

        // Подсчет различий
        int differences = 0;
        int diffIndex = -1;

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
            {
                differences++;
                diffIndex = i;
            }
        }

        return differences == 1
            ? new CheckOutcome(CheckResult.OneLetterDifference, diffIndex)
            : new CheckOutcome(CheckResult.MultipleDifferences);
    }

}
