using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum CheckResult
{
    Identical,              // ������ ��������� ���������
    OneLetterDifference,    // ���������� ����� ���� ����� � ����� �������
    MultipleDifferences,    // ����� ������ �������� ��� ������ �����
    InvalidInput            // ���� �� ������� null ��� �������� �� �������� ���������� �������
}

public struct CheckOutcome
{
    public CheckResult Result { get; set; }
    public int Index { get; set; } // ������ ������� � ���������

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
        // �������� �� null
        if (list1 == null || list2 == null)
        {
            return new CheckOutcome(list1 == null && list2 == null ? CheckResult.Identical : CheckResult.InvalidInput);
        }

        // �������� �� ���������� ����� �������
        if (list1.Count != list2.Count)
        {
            return new CheckOutcome(CheckResult.MultipleDifferences);
        }

        // ��������, ��� ��� �������� � ������ �� ����� �����
        foreach (var item in list1.Concat(list2))
        {
            if (string.IsNullOrEmpty(item) || item.Length != 1)
            {
                return new CheckOutcome(CheckResult.InvalidInput);
            }
        }

        // �������� �� ������ ����������
        if (list1.SequenceEqual(list2))
        {
            return new CheckOutcome(CheckResult.Identical);
        }

        // ������� ��������
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
