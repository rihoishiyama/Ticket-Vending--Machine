using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    private int totalWallet = 0, totalAmount = 0, totalBalanceAmount = 0;

    // 切符のインスタンス
    private Ticket ticket = new Ticket(1, 130, 124);

    // 電子マネーのインスタンス
    private ElectronicMoney electronicMoney = new ElectronicMoney(1000);

    // 現金のインスタンス
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

    //手持ちの金種の管理
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

    enum MachineFaze
    {
        Start,
        ChoiceTicket,
        TicketNum,
        ChoicePayment,
        CashAccount,
        CashFinish,
        ElectronicFinish
    }

    [SerializeField]
    private MachineFaze machineFaze = MachineFaze.Start;


    void Update () 
    {
        MachineMode();

        if(machineFaze == MachineFaze.CashAccount)
            uiManager.ArrowClickAccountBt(totalAmount, ticket.cashAmount);
	}

    private void MachineMode()
    {
        switch (machineFaze)
        {
            case MachineFaze.Start:
                InitData();
                uiManager.Init();
                uiManager.InitStart();
                break;

            case MachineFaze.ChoiceTicket:
                uiManager.InitChoiceTicket();

                break;

            case MachineFaze.ChoicePayment:
                uiManager.InitChoicePayment();

                break;

            case MachineFaze.CashAccount:
                uiManager.InitCashAccount();
                break;

            case MachineFaze.CashFinish:
                uiManager.InitCashFinish();
                break;

            case MachineFaze.ElectronicFinish:
                uiManager.InitElectronicFinish();

                break;

        }

    }
    public void OnClickStartBt()
    {
        machineFaze = MachineFaze.ChoiceTicket;
    }

    public void OnClickPerchaseTicket()
    {
        machineFaze = MachineFaze.ChoicePayment;
    }

    public void OnClickCashChoicePayment()
    {
        machineFaze = MachineFaze.CashAccount;
    }

    public void OnClickCashAccount()
    {
        machineFaze = MachineFaze.CashFinish;
    }

    public void OnClickElectronicChoicePayment()
    {
        machineFaze = MachineFaze.ElectronicFinish;
    }

    public void ReturnTopFaze()
    {
        machineFaze = MachineFaze.Start;
    }


    public void InitData()
    {
        totalWallet = 0;
        totalAmount = 0;
        InitWalletNum();
        uiManager.UpdateTotalInputText(totalAmount);
        uiManager.UpdateShotageAmountText(ticket.cashAmount, totalAmount);
        foreach (CashStatus wallet in walletCash)
        {
            totalWallet += wallet.denomi * wallet.count;
            denomiCash[wallet.index].count = 0;
            tempWalletNum[wallet.index] = wallet.count;
            uiManager.UpdateInputText(denomiCash[wallet.index].count, denomiCash[wallet.index].index);
            uiManager.UpdateDenomiText(wallet.index, wallet.count);
            uiManager.UpdateCurrentWalletText(wallet.index, tempWalletNum[wallet.index], tempWalletNum, walletCash);
        }
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
                totalAmount += c.denomi;
                uiManager.UpdateShotageAmountText(ticket.cashAmount, totalAmount);
                uiManager.UpdateInputText(c.count, c.index);
                uiManager.UpdateTotalInputText(totalAmount);
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
                totalAmount -= c.denomi;
                uiManager.UpdateShotageAmountText(ticket.cashAmount, totalAmount);
                uiManager.UpdateInputText(c.count, c.index);
                uiManager.UpdateTotalInputText(totalAmount);

            }
        }
    }

    public void UpdateDenomiText()
    {
        uiManager.UpdateDenomiText(totalWallet, electronicMoney.e_balance);
    }


    public void TakeWallet()
    {
        // お釣りを出す
        totalBalanceAmount = totalAmount - ticket.cashAmount;

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

    // 指定した支払い方法で払えるのか
    public void JudgePurchase(Button button)
    {
        if (button.tag == "Cash" && totalWallet >= ticket.electAmount)
        {
            uiManager.TicketNumText(ticket.cashAmount);
            OnClickCashChoicePayment();
        }
        else if (button.tag == "ElectronicMoney" && electronicMoney.e_balance >= ticket.electAmount)
        {
            FinishElectronicMoney();
            OnClickElectronicChoicePayment();
        }
        else
        {
            electronicMoney.e_transaction = false;
            uiManager.NotArrowPurchase(button);
        }
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
        uiManager.PaymentText(denomiCash, totalAmount);
        uiManager.ChangeText(balanceCash, balance);
        uiManager.WalletText(walletCash, totalWallet);
    }

    public void FinishElectronicMoney()
    {
        electronicMoney.e_balance -= ticket.electAmount;
        uiManager.ChangeElectText(ticket.electAmount, electronicMoney.e_balance, electronicMoney.e_balance);

    }

    private void InitWalletNum()
    {
        foreach (CashStatus c in walletCash)
        {
            tempWalletNum[c.index] = c.count;
        }
    }
}
