using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startBt;
    [SerializeField]
    private GameObject perchaseTicket;
    [SerializeField]
    private GameObject ticketNum;
    [SerializeField]
    private GameObject choicePayment;
    [SerializeField]
    private GameObject cashAccount;
    [SerializeField]
    private GameObject cashFinish;
    [SerializeField]
    private GameObject electlicFinish;

    [SerializeField] List<Text> walletCashText = new List<Text>();

    //- choiceTicket関連 -
    [SerializeField]
    private Text cashDenomiText, electDenomiText;
    //-----------------------

    //- choicePayment関連 -
    [SerializeField]
    private GameObject errorPaymentText;
    [SerializeField]
    private Button toCashBt, toElectBt;
    //-----------------------

    //- cashAccount関連 -
    [SerializeField]
    private Text ticketAmount, shotageNum, walletTotalNum;
    //-----------------------

    //- finish関連 -
    [SerializeField]
    private Text purchaseNum, paymentNum, changeNum, walletNum, paymentTotalNum, changeTotalNum, c_walletTotalNum;
    [SerializeField]
    private Text e_purchaseNum, e_paymentNum, e_changeNum;
    //-----------------------

    [SerializeField] 
    private MachineFaze machineFaze = MachineFaze.Start;
    private Payment payment;

    public Button accountBt;

    enum MachineFaze
    {
        Start,
        ChoiceTicket,
        TicketNum,
        ChoicePayment,
        CashAccount,
        CashFinish,
        ElectlicFinish
    }

    enum Payment
    {
        Cash,
        ElectricMoney
    }


    private void Update()
    {
        PerChaseMode();
    }

    private void PerChaseMode()
    {
        switch(machineFaze)
        {
            case MachineFaze.Start:
                cashFinish.SetActive(false);
                electlicFinish.SetActive(false);
                Init();
                startBt.SetActive(true);

                break;

            case MachineFaze.ChoiceTicket:
                startBt.SetActive(false);
                perchaseTicket.SetActive(true);

                break;

            case MachineFaze.TicketNum:
                perchaseTicket.SetActive(false);
                ticketNum.SetActive(true);

                break;

            case MachineFaze.ChoicePayment:
                perchaseTicket.SetActive(false);
                ticketNum.SetActive(false);
                choicePayment.SetActive(true);

                break;

            case MachineFaze.CashAccount:
                choicePayment.SetActive(false);
                cashAccount.SetActive(true);

                break;

            case MachineFaze.CashFinish:
                cashAccount.SetActive(false);
                cashFinish.SetActive(true);

                break;

            case MachineFaze.ElectlicFinish:
                choicePayment.SetActive(false);
                electlicFinish.SetActive(true);

                break;

        }

    }

    public void Init()
    {
        perchaseTicket.SetActive(false);
        ticketNum.SetActive(false);
        choicePayment.SetActive(false);
        cashAccount.SetActive(false);
        cashFinish.SetActive(false);
        electlicFinish.SetActive(false);
        paymentNum.text = "";
        changeNum.text = "";
        walletNum.text = "";
        errorPaymentText.SetActive(false);
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

    public void OnClickElectlicChoicePayment()
    {
        machineFaze = MachineFaze.ElectlicFinish;
    }

    public void ReturnTopFaze()
    {
        machineFaze = MachineFaze.Start;
    }

    public void UpdateNumView(int num, Text text)
    {
        text.text = num.ToString();
    }

    public void UpdateAmountView(int num, Text text)
    {
        text.text = num.ToString() + "円";
    }


    public void ArrowClickAccountBt(int totalDenomi, int ticketAmount)
    {
        if (totalDenomi >= ticketAmount)
            accountBt.interactable = true;
        else
            accountBt.interactable = false;
    }

    public void TicketAmountText(int ticketNum)
    {
        purchaseNum.text = ticketNum.ToString() + "円";
    }

    public void PaymentText(List<CashStatus> cashes, int totalNum)
    {
        int i = 0;
        foreach (CashStatus c in cashes)
        {
            if(c.count != 0)
            {
                paymentNum.text += c.denomi.ToString() + "円 × " + c.count.ToString() + "  ";
                i++;
                if (i == 4)
                    paymentNum.text += "\n";
            }
        }
        paymentTotalNum.text = totalNum.ToString() + "円";
    }

    public void ChangeText(List<CashStatus> cashes, int balance)
    {
        int i = 0;
        foreach (CashStatus c in cashes)
        {
            if (c.count != 0)
            {
                changeNum.text += c.denomi.ToString() + "円 × " + c.count.ToString() + "  ";
                i++;
                if (i == 4)
                    changeNum.text += "\n";
            }
        }
        if (balance == 0)
            changeNum.text = "なし";

        changeTotalNum.text = balance.ToString() + "円";
    }

    public void WalletText(List<CashStatus> cashes, int wallet)
    {
        int i = 0;
        foreach (CashStatus c in cashes)
        {
            if (c.count != 0)
            {
                walletNum.text += c.denomi.ToString() + "円 × " + c.count.ToString() + "  ";
                i++;
                if (i == 4)
                    walletNum.text += "\n";
            }
        }
        if (wallet == 0)
            walletNum.text = "なし";

        c_walletTotalNum.text = wallet.ToString() + "円";
    }

    public void ChangeElectText(int amount, int wallet, int balance)
    {
        e_purchaseNum.text = amount.ToString() + "円";
        e_paymentNum.text = wallet.ToString() + "円";
        e_changeNum.text = balance.ToString() + "円";
    }

    public void NotArrowPurchase(Button button)
    {
        button.interactable = false;
        errorPaymentText.SetActive(true);
    }

    public void UpdateDenomiText(int cashNum, int electNum)
    {
        cashDenomiText.text = "残高：" + cashNum.ToString() + "円";
        electDenomiText.text = "残高：" + electNum.ToString() + "円";
    }

    public void UpdateCurrentWalletText(int index, int value, int[] totalWallet, List<CashStatus> cashStatuses)
    {
        int total = 0;
        walletCashText[index].text = value.ToString();
        for (int i = 0; i < totalWallet.Length; i++)
        {
            Debug.Log(i);
            total += totalWallet[i] * cashStatuses[i].denomi;
        }
        walletTotalNum.text = total.ToString() + "円";
    }

    //不足金額の更新
    public void UpdateShotageAmountText(int ticketNum, int totalNum)
    {
        int shotage = ticketNum - totalNum;
        if (shotage <= 0)
            shotageNum.text = "0円";
        else
            shotageNum.text = shotage.ToString() + "円";
    }

    public void TicketNumText(int num)
    {
        ticketAmount.text = num.ToString() + "円";
    }
}
