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
    private GameObject choicePayment;
    [SerializeField]
    private GameObject cashAccount;
    [SerializeField]
    private GameObject cashFinish;
    [SerializeField]
    private GameObject electlicFinish;

    // 手持ちの金種のテキストの管理（10,000 -> 10の順）
    [SerializeField]
    List<Text> walletCashText = new List<Text>();

    // 投入額の金種のテキストの管理（10,000 -> 10の順）
    [SerializeField]
    private List<Text> denomiCashText = new List<Text>();


    //- choiceTicket関連 -
    [SerializeField]
    private Text cashDenomiText, electDenomiText;
    //-----------------------

    //- choicePayment関連 -
    [SerializeField]
    private GameObject errorPaymentText;
    //-----------------------

    //- cashAccount関連 -
    [SerializeField]
    private Text ticketAmount, totalNumText, shotageNum, walletTotalNum;
    //-----------------------

    //- finish関連 -
    [SerializeField]
    private Text purchaseNum, paymentNum, changeNum, walletNum, paymentTotalNum, changeTotalNum, c_walletTotalNum;
    [SerializeField]
    private Text e_purchaseNum, e_paymentNum, e_changeNum;
    //-----------------------

    public Button accountBt;


    public void Init()
    {
        perchaseTicket.SetActive(false);
        choicePayment.SetActive(false);
        cashAccount.SetActive(false);
        cashFinish.SetActive(false);
        electlicFinish.SetActive(false);
        errorPaymentText.SetActive(false);

        paymentNum.text = "";
        changeNum.text = "";
        walletNum.text = "";
    }

    public void InitStart()
    {
        startBt.SetActive(true);
    }

    public void InitChoiceTicket()
    {
        startBt.SetActive(false);
        perchaseTicket.SetActive(true);
    }

    public void InitChoicePayment()
    {
        perchaseTicket.SetActive(false);
        choicePayment.SetActive(true);
    }

    public void InitCashAccount()
    {
        choicePayment.SetActive(false);
        cashAccount.SetActive(true);
    }

    public void InitCashFinish()
    {
        cashAccount.SetActive(false);
        cashFinish.SetActive(true);
    }

    public void InitElectronicFinish()
    {
        choicePayment.SetActive(false);
        electlicFinish.SetActive(true);
    }

    public void UpdateInputText(int num, int index)
    {
        denomiCashText[index].text = num.ToString();
    }

    public void UpdateTotalInputText(int num)
    {
        totalNumText.text = num.ToString() + "円";
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

    public void TicketNumText(int num)
    {
        ticketAmount.text = num.ToString() + "円";
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

}
