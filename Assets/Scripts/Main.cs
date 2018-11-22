using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    private int ticketNum = 0;
    private int totalWallet = 0;
    private int totalAmont = 0, totalBalanceAmount = 0;
    private int remainElect;
    private int remainCash;

    [SerializeField] private Text ticketNumText, totalNumText;
    [SerializeField] List<Text> denomiCashText = new List<Text>();// 投入額の金種のテキストの管理（10,000 -> 10の順）

    // 電子マネー
    private ElectricMoney electricMoney = new ElectricMoney(1000);
    // 投入額の金種の管理
    private List<CashStatus> denomiCash = new List<CashStatus>
    {
        new CashStatus(0, 10000, 0),
        new CashStatus(1, 5000, 0),
        new CashStatus(2, 1000, 0),
        new CashStatus(3, 500, 0),
        new CashStatus(4, 100, 0),
        new CashStatus(5, 50, 0),
        new CashStatus(6, 10, 0),
    };

    //ウォレットの金種の管理
    private List<CashStatus> walletCash = new List<CashStatus>
    {
        new CashStatus(0, 10000, 1),
        new CashStatus(1, 5000, 1),
        new CashStatus(2, 1000, 1),
        new CashStatus(3, 500, 1),
        new CashStatus(4, 100, 2),
        new CashStatus(5, 50, 3),
        new CashStatus(6, 10, 15),
    };
    private int[] tempWalletNum = new int[7];

    // お釣りの金種の枚数
    private List<CashStatus> balanceCash = new List<CashStatus>
    {
        new CashStatus(0, 10000, 0),
        new CashStatus(1, 5000, 0),
        new CashStatus(2, 1000, 0),
        new CashStatus(3, 500, 0),
        new CashStatus(4, 100, 0),
        new CashStatus(5, 50, 0),
        new CashStatus(6, 10, 0),
    };


    [SerializeField]
    private UIManager uiManager;
    private Ticket ticket = new Ticket(1, 130, 124);
    //private Cash cash = new Cash();

    // Use this for initialization
    void Start () 
    {
        ticketNumText.text = ticketNum.ToString();
        InitWalletNum();

        InitData();

        //foreach (CashStatus amount in walletCash)
        //{
        //    totalWallet += amount.denomi * amount.count;
        //}
    }

    // Update is called once per frame
    void Update () 
    {
        uiManager.ArrowClickAccountBt(totalAmont, ticket.cashAmount);
	}

    public void InitData()
    {
        totalWallet = 0;
        totalAmont = 0;
        uiManager.UpdateNumView(totalAmont, totalNumText);
        uiManager.UpdateShotageAmountText(ticket.cashAmount, totalAmont);
        foreach (CashStatus wallet in walletCash)
        {
            totalWallet += wallet.denomi * wallet.count;
            denomiCash[wallet.index].count = 0;
            tempWalletNum[wallet.index] = wallet.count;
            uiManager.UpdateNumView(denomiCash[wallet.index].count, denomiCashText[denomiCash[wallet.index].index]);
            uiManager.UpdateDenomiText(wallet.index, wallet.count);
            uiManager.UpdateCurrentWalletText(wallet.index, tempWalletNum[wallet.index], tempWalletNum, walletCash);
        }
    }

    public void ResetData()
    {

    }

    public void PlusNum()
    {
        ticketNum++;
        uiManager.UpdateNumView(ticketNum, ticketNumText);

    }

    public void MinusNum()
    {
        if(ticketNum > 0)
            ticketNum--;
        uiManager.UpdateNumView(ticketNum, ticketNumText);

    }

    public void CashAccountInit()
    {

    }

    public void CashPlusNum(int price)
    {
        foreach(CashStatus c in denomiCash)
        {
            if (c.denomi == price)
            {
                if (c.count >= walletCash[c.index].count)
                    return;

                c.count++;
                tempWalletNum[c.index]--;
                uiManager.UpdateCurrentWalletText(c.index, tempWalletNum[c.index], tempWalletNum, walletCash);
                totalAmont += c.denomi;
                uiManager.UpdateShotageAmountText(ticket.cashAmount, totalAmont);
                uiManager.UpdateNumView(c.count, denomiCashText[c.index]);
                uiManager.UpdateAmountView(totalAmont, totalNumText);
            }
        }

    }

    public void CashMinusNum(int price)
    {
        foreach (CashStatus c in denomiCash)
        {
            if (c.denomi == price)
            {
                if (c.count <= 0)
                    return;

                c.count--;
                tempWalletNum[c.index]++;
                uiManager.UpdateCurrentWalletText(c.index, tempWalletNum[c.index], tempWalletNum, walletCash);
                totalAmont -= c.denomi;
                uiManager.UpdateShotageAmountText(ticket.cashAmount, totalAmont);
                uiManager.UpdateNumView(c.count, denomiCashText[c.index]);
                uiManager.UpdateAmountView(totalAmont, totalNumText);

            }
        }

    }

   
    public void TakeWallet()
    {
        // お釣りを出す
        totalBalanceAmount = totalAmont - ticket.cashAmount;

        // 清算後財布から投入した金種を引く
        foreach (CashStatus c in denomiCash)
        {
            walletCash[c.index].count -= c.count;
        }

        //お釣りの金種と枚数を計算
        foreach (CashStatus c in balanceCash)
        {
            c.count = totalBalanceAmount / c.denomi;
            totalBalanceAmount = totalBalanceAmount % c.denomi;
        }

        //お財布にお釣りを加算
        foreach (CashStatus wallet in walletCash)
        {
            wallet.count += balanceCash[wallet.index].count;
        }
    }

    private void InitWalletNum()
    {
        foreach(CashStatus c in walletCash)
        {
            tempWalletNum[c.index] = c.count;
        }
    }

    public void JudgeCashPurchase(Button button)
    {
        if (totalWallet >= ticket.electAmount)
        {
            uiManager.TicketNumText(ticket.cashAmount);
            uiManager.OnClickCashChoicePayment();
        }
        else
        {
            uiManager.NotArrowPurchase(button);
        }
    }


    // 電子マネーで払えるかどうか
    public void JudgeElectPurchase(Button button)
    {
        if(electricMoney.initBalance >= ticket.electAmount)
        {
            FinishElectlicMoney();
            uiManager.OnClickElectlicChoicePayment();

        } else {
            uiManager.NotArrowPurchase(button);
        }
    }

    // 電子マネー残高計算
    public int CalcBalance()
    {
        return electricMoney.initBalance -= ticket.electAmount;
    }

    public void FinishCash()
    {
        int balance = 0;
        foreach(CashStatus balanceCashes in balanceCash)
        {
            balance += balanceCashes.denomi * balanceCashes.count;
        }

        totalWallet = 0;
        foreach (CashStatus wallet in walletCash)
        {
            totalWallet += wallet.denomi * wallet.count;
        }

        uiManager.TicketAmountText(ticket.cashAmount);
        uiManager.PaymentText(denomiCash, totalAmont);
        uiManager.ChangeText(balanceCash, balance);
        uiManager.WalletText(walletCash, totalWallet);
    }

    public void FinishElectlicMoney()
    {
        uiManager.ChangeElectText(ticket.electAmount, electricMoney.initBalance, CalcBalance());

    }

    public void UpdateDenomiText()
    {
        uiManager.UpdateDenomiText(totalWallet, electricMoney.initBalance);
    }

}
