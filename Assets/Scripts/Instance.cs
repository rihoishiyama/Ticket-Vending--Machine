public class Ticket
{
    //駅名
    public int ticketNum;
    public int cashAmount;
    public int electAmount;


    public Ticket(int _ticketNum, int _cashAmount, int _electAmount)
    {
        ticketNum = _ticketNum;
        cashAmount = _cashAmount;
        electAmount = _electAmount;
    }
}

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

public class ElectronicMoney
{
    public int e_balance; //初期残高
    public bool e_transaction;//支払い可能な状態か（初期値はtrue）

    public ElectronicMoney(int _balance, bool _transaction = true)
    {
        e_balance = _balance;
        e_transaction = _transaction;
    }
}
