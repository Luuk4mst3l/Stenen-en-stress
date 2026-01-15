using UnityEngine;

public class NotifyStakeholders : MonoBehaviour
{
    [SerializeField] private StakeHolders stakeHolder1;
    [SerializeField] private StakeHolders stakeHolder2;
    [SerializeField] private StakeHolders stakeHolder3;
    [SerializeField] private StakeHolders stakeHolder4;


    public void notifyAll()
    {
        stakeHolder1?.Signal();
        stakeHolder2?.Signal();
        stakeHolder3?.Signal();
        stakeHolder4?.Signal();
    }

    public void notifyStakeHolder1()
    {
        stakeHolder1?.Signal();
    }

    public void notifyStakeHolder2()
    {
        stakeHolder2?.Signal();
    }

    public void notifyStakeHolder3()
    {
        stakeHolder3?.Signal();
    }

    public void notifyStakeHolder4()
    {
        stakeHolder4?.Signal();
    }



}