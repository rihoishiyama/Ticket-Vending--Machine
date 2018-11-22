using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashStatus
{
    public int index;
    public int denomi;
    public int count;

    public CashStatus(int _index, int _denomi, int _count)
    {
        index = _index;
        denomi = _denomi;
        count = _count;
    }

}

//public class Cash 
//{
//    // お釣りの計算
//    public int CalcBalance(int totalAmont, int ticketAmount)
//    {
//        return totalAmont - ticketAmount;
//    }

//    // お釣りの金種の枚数計算と財布の更新
//    public int[] OutMoney(int remainCash) 
//    {
//        int[] tempOutMoney = new int[8];
//        return tempOutMoney;
//    }

//}

