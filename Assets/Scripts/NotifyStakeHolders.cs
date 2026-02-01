using System.Collections.Generic;
using UnityEngine;

public class NotifyStakeholders : MonoBehaviour
{
    [SerializeField]
    private List<StakeHolders> stakeholders = new List<StakeHolders>();

    //     public void NotifyAll()
    //     {
    //         foreach (var stakeholder in stakeholders)
    //         {
    //             stakeholder?.Signal();
    //         }
    //     }

    public void NotifyStakeholder(int index)
    {
        if (index < 0 || index >= stakeholders.Count)
        {
            Debug.LogWarning($"NotifyStakeholder: Index {index} is out of range.");
            return;
        }

        stakeholders[index]?.Signal();
    }

    public bool IsStakeholderNotified(int index)
    {
        if (index < 0 || index >= stakeholders.Count)
            return false;

        return stakeholders[index] != null && stakeholders[index].IsNotified;
    }
}
