public class ElectricMoney 
{
    public int initBalance; //初期残高

    public ElectricMoney(int _initBalance)
    {
        initBalance = _initBalance;
    }

    // 払えるかどうか
    public bool JudgePurchase(int totalAmont, int remainElect)
    {
        if(remainElect >= totalAmont)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // 残高計算
    public int CalcBalance(int totalAmont, int remainElect)
    {
        return 1;
    }

}
