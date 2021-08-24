using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//nuget packages
using QRCoder;
using QRCoder.Unity;

public class UI_manager : MonoBehaviour
{

    //purchase button vouchers
    Button voucher_5_dollar;
    Button voucher_10_dollar;
    Button voucher_5_dollar_onchain;
    Button voucher_10_dollar_onchain;

    //api test script, contains methods that mimic API calls. 
    public azteco_api_tests api_test;


    //confirmation object and buttons
    // has an array to store all the text objects to write data
    GameObject confirmation;
    Text[] confirmation_texts = new Text[6];
    Button confirmation_complete;
    Button confirmation_cancel;

    //success object and buttons
    //contains text objects to display 16 digit code and reference number
    //as well as a cube to display lightning qr code. 
    GameObject success;
    Button return_btn;
    GameObject qrcube;
    Text voucher_code;
    Text reference_code;
    Text voucher_decor_text;
    Text reference_decor_text;

    //variabes used to assist scripts. 
    bool islightning;
    int order_id = 0;

    // Start is called before the first frame update
    void Start()
    {
        //setup confirmation 
        confirmation = GameObject.Find("confirmation");
        confirmation_setup();

        //setup success
        success = GameObject.Find("success");
        success_setup();

        //setup example purchase buttons
        voucher_buttons();

        //disable confirmation and success objects to only show purchase buttons.
        confirmation.SetActive(false);
        success.SetActive(false);
    }

    void confirmation_setup()
    {
        confirmation_texts[0] = GameObject.Find("btc_amount_txt").GetComponent<Text>();
        confirmation_texts[1] = GameObject.Find("voucher_type_txt").GetComponent<Text>();
        confirmation_texts[2] = GameObject.Find("network_fee_txt").GetComponent<Text>();
        confirmation_texts[3] = GameObject.Find("commission_txt").GetComponent<Text>();
        confirmation_texts[4] = GameObject.Find("amount_txt").GetComponent<Text>();
        confirmation_texts[5] = GameObject.Find("total_txt").GetComponent<Text>();

        confirmation_cancel = GameObject.Find("confirm_cancel_btn").GetComponent<Button>();
        confirmation_cancel.onClick.AddListener(cancel);

        confirmation_complete = GameObject.Find("complete_sale_btn").GetComponent<Button>();
        confirmation_complete.onClick.AddListener(lightning_or_chain);
    }

    void voucher_buttons()
    {
        //setup button listeners
        voucher_5_dollar = GameObject.Find("stage_5_dollar").GetComponent<Button>();
        voucher_5_dollar.onClick.AddListener(stage_5_dollar_wrapper);

        voucher_10_dollar = GameObject.Find("stage_10_dollar").GetComponent<Button>();
        voucher_10_dollar.onClick.AddListener(stage_10_dollar_wrapper);


        voucher_5_dollar_onchain = GameObject.Find("stage_5_dollar_onchain").GetComponent<Button>();
        voucher_5_dollar_onchain.onClick.AddListener(stage_5_dollar_onchain_wrapper);

        voucher_10_dollar_onchain = GameObject.Find("stage_10_dollar_onchain").GetComponent<Button>();
        voucher_10_dollar_onchain.onClick.AddListener(stage_10_dollar_onchain_wrapper);
    }

    void success_setup()
    {
        qrcube = GameObject.Find("Cube");
        voucher_code = GameObject.Find("voucher_code_txt").GetComponent<Text>();
        reference_code= GameObject.Find("reference_code_txt").GetComponent<Text>();
        voucher_decor_text = GameObject.Find("code_txt").GetComponent<Text>();
        reference_decor_text = GameObject.Find("reference_code_show_txt").GetComponent<Text>();

        return_btn = GameObject.Find("return_btn").GetComponent<Button>();
        return_btn.onClick.AddListener(cancel);
    }

    //disables confirmation and success objects 
    public void cancel()
    {
        confirmation.SetActive(false);
        success.SetActive(false);
        order_id = 0;
    }

    //used to determine what is shown on success object
    public void lightning_or_chain()
    {
        if (islightning)
        {
            finalize_lightning_wrapper();
        } else 
        {
            finalize_onchain_wrapper();
        }
    }

//************************************************************************
//                              LIGHTNING
//************************************************************************

    public void stage_5_dollar_wrapper()
    {
        StartCoroutine(stage_lightning(5.0f));
        islightning = true;
    }


    public void stage_10_dollar_wrapper()
    {
        StartCoroutine(stage_lightning(10.0f));
        islightning = true;
    }

    //wait for the coroutine to return a response and set the confirmation text to display its data
    private IEnumerator stage_lightning(float amount)
    {
        yield return StartCoroutine(api_test.run_api_stage_lightning_order_test("USD", amount));

        confirmation.SetActive(true);

        confirmation_texts[0].text = "Amount of bitcoin: " + api_test.stage_order.bitcoin.ToString("n8") ;
        confirmation_texts[1].text = "Voucher type: Lightning";
        confirmation_texts[2].text = "Network Fee: " + api_test.stage_order.network_fee;
        confirmation_texts[3].text = "Commission @ 4%: $" + api_test.stage_order.commission.ToString("n2");
        confirmation_texts[4].text = "Purchase amount: $" + api_test.stage_order.purchase_amount.ToString("n2");
        confirmation_texts[5].text = "TOTAL: $" + api_test.stage_order.total.ToString("n2");


        order_id = api_test.stage_order.order_id;
    }

    public void finalize_lightning_wrapper()
    {

        StartCoroutine(finalize_lightning());
    }

    //waits for coroutine to return a value and generates a qr code to display the LNUrl. 
    private IEnumerator finalize_lightning()
    {

        yield return StartCoroutine(api_test.run_api_finalize_lightning_order_test(order_id));

        confirmation.SetActive(false);
        success.SetActive(true);
        qrcube.SetActive(true);

        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        string lnurl = api_test.finalize_lightning_order.lnurl;
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(lnurl, QRCodeGenerator.ECCLevel.Q);
        UnityQRCode qrCode = new UnityQRCode(qrCodeData);
        Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(20);

        GameObject.Find("Cube").GetComponent<Renderer>().material.mainTexture = qrCodeAsTexture2D;

        voucher_decor_text.text = "your QR code:";
        reference_decor_text.text = "";

        voucher_code.text = "";     
        reference_code.text = "";
    }

//************************************************************************
//                              ON-CHAIN
//************************************************************************

    public void stage_5_dollar_onchain_wrapper()
    {
        StartCoroutine(stage_onchain(5.0f));
        islightning = false;
    }

    public void stage_10_dollar_onchain_wrapper()
    {
        StartCoroutine(stage_onchain(10.0f));
        islightning = false;
    }

    //wait for the coroutine to return a response and set the confirmation text to display its data
    private IEnumerator stage_onchain(float amount)
    {
        yield return StartCoroutine(api_test.run_api_stage_order_test("USD", amount));

        confirmation.SetActive(true);

        confirmation_texts[0].text = "Amount of bitcoin: " + api_test.stage_order.bitcoin.ToString("n8") ;
        confirmation_texts[1].text = "Voucher type: On-chain";
        confirmation_texts[2].text = "Network Fee: " + api_test.stage_order.network_fee.ToString("n8");
        confirmation_texts[3].text = "Commission @ 4%: $" + api_test.stage_order.commission.ToString("n2");
        confirmation_texts[4].text = "Purchase amount: $" + api_test.stage_order.purchase_amount.ToString("n2");
        confirmation_texts[5].text = "TOTAL: $" + api_test.stage_order.total.ToString("n2");


        order_id = api_test.stage_order.order_id;
    }

    public void finalize_onchain_wrapper()
    {

        StartCoroutine(finalize_onchain());
    }

    //waits for coroutine to return a value and displays the voucher and reference code. 
    private IEnumerator finalize_onchain()
    {

        yield return StartCoroutine(api_test.run_api_finalize_order_test(order_id));

        confirmation.SetActive(false);
        success.SetActive(true);
        qrcube.SetActive(false);


        string voucher_code_formatted = api_test.finalize_order.voucher_code;

        for (int i = 4; i <= voucher_code_formatted.Length; i += 4)
        {
            voucher_code_formatted = voucher_code_formatted.Insert(i, " ");
            i++;
        }

        voucher_code.text = voucher_code_formatted;
        reference_code.text = api_test.finalize_order.reference_code;

        voucher_decor_text.text = "your Voucher code:";
        reference_decor_text.text = "your reference code:";

        Debug.Log(api_test.finalize_order.voucher_code);
        Debug.Log(api_test.finalize_order.reference_code);
    }

}
