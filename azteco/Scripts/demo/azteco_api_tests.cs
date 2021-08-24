using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class azteco_api_tests : MonoBehaviour
{
    //classes to store the returned data from the api calls. 
    public azteco_get_price_store get_price;
    public azteco_get_balance_store get_balance;
    public azteco_get_statement_store[] get_statement;
    public azteco_stage_order_store stage_order;
    public azteco_finalize_order_store finalize_order;
    public azteco_finalize_lightning_order_store finalize_lightning_order;
    public azteco_get_topup_address_store topup_address;
    public azteco_verify_voucher_status_store verify_voucher_status;

    //methods for mimicing api calls
    //these are IEnumerators due to the scripts in UI_manager relying on return data from them
//******************************************************************************
//                              MIMIC API INITIALIZATION
//******************************************************************************
    public IEnumerator run_api_get_price_test(string currency)
    {
         yield return StartCoroutine(api_get_price_test(currency));
    }


    public IEnumerator run_api_get_balance_test(string currency)
    {

        yield return StartCoroutine(api_get_balance_test(currency));
    }
/*
    public IEnumerator run_api_get_statement_test()
    {

        yield return StartCoroutine(api_get_statement_test());
    }
*/
    public IEnumerator run_api_stage_order_test(string currency , float amount)
    {

        yield return StartCoroutine(api_stage_order_test(currency, amount));
    }

    public IEnumerator run_api_finalize_order_test(int order_id)
    {

        yield return StartCoroutine(api_finalize_order_test(order_id));
    }

    public IEnumerator run_api_stage_lightning_order_test(string currency , float amount)
    {

        yield return StartCoroutine(api_stage_lightning_order_test(currency, amount));
    }

    public IEnumerator run_api_finalize_lightning_order_test(int order_id)
    {

        yield return StartCoroutine(api_finalize_lightning_order_test(order_id));
    }

    public IEnumerator run_api_get_topup_address_test()
    {

        yield return StartCoroutine(api_get_topup_address_test());
    }
/*
    public IEnumerator run_api_get_voucher_status_test(string reference_code)
    {

        yield return StartCoroutine(api_get_voucher_status_test(reference_code));
    }
*/

    //************************************************************************************
    //                        METHODS FOR API MIMIC CALLS
    //************************************************************************************

    private IEnumerator api_get_price_test(string currency)
    {
        
        yield return new WaitForSeconds(3);

        get_price.status = "success";
        get_price.currency = currency;
        get_price.bitcoin_price = 59895.03f;
    }



    private IEnumerator api_get_balance_test(string currency)
    {
        yield return new WaitForSeconds(3);

        get_balance.status = "success";
        get_balance.currency = currency;
        get_balance.balance = 24000.03f;
        get_balance.fx_rate = 1.0f;
    }
/*
    private IEnumerator api_get_statement_test()
    {
       
    }
*/
    private IEnumerator api_stage_order_test(string currency , float amount)
    {
        yield return new WaitForSeconds(3);

        stage_order.status = "success";
        stage_order.message = "";

        stage_order.order_id = 1647379;
        stage_order.currency = currency;
        stage_order.bitcoin_price = 59895.03f;
        stage_order.total = amount;


        stage_order.network_fee = 0.00001248f;
        stage_order.bitcoin = calcBtcAmount(amount, stage_order.bitcoin_price );
        stage_order.commission = amount * 0.04f;
        stage_order.purchase_amount = amount - stage_order.commission - (stage_order.network_fee * stage_order.bitcoin_price);
        stage_order.fx_rate = 1.0f;


        stage_order.ttl = 300;
    }

    private IEnumerator api_finalize_order_test(int order_id)
    {

        yield return new WaitForSeconds(3);
        
        finalize_order.status = "success";
        finalize_order.message = "";
        finalize_order.order_id = 1549379;
        finalize_order.currency = "USD";
        finalize_order.bitcoin_price = 57894.03f;
        finalize_order.total = 500.00f;
        finalize_order.purchase_amount = 478.85f;
        finalize_order.commission = 20.00f;
        finalize_order.fx_rate = 1.0f;
        finalize_order.network_fee = 0.00012483f;
        finalize_order.bitcoin = 0.08338755f;
        finalize_order.voucher_code = "1234123412341234";
        finalize_order.reference_code = "0000000000000000";

        //Debug.Log( finalize_order.voucher_code);
    }

    private IEnumerator api_stage_lightning_order_test(string currency, float amount)
    {
        yield return new WaitForSeconds(3);

        stage_order.status = "success";
        stage_order.message = "";

        stage_order.order_id = 1647379;
        stage_order.currency = currency;
        stage_order.bitcoin_price = 59895.03f;
        stage_order.total = amount;

        stage_order.commission = amount * 0.04f;
        stage_order.purchase_amount = amount - stage_order.commission;
        stage_order.fx_rate = 1.0f;
        stage_order.network_fee = 0.0f;
        //stage_order.bitcoin = 0.08438755f;
        stage_order.bitcoin = calcBtcAmount(amount, stage_order.bitcoin_price );
        stage_order.ttl = 300;
    }

    private IEnumerator api_finalize_lightning_order_test(int order_id)
    {
        yield return new WaitForSeconds(3);

        finalize_lightning_order.status = "success";
        finalize_lightning_order.message = "";
        finalize_lightning_order.order_id = order_id;
        finalize_lightning_order.currency = stage_order.currency;
        finalize_lightning_order.bitcoin_price = stage_order.bitcoin_price;
        finalize_lightning_order.total =  stage_order.total;
        finalize_lightning_order.purchase_amount = stage_order.purchase_amount;
        finalize_lightning_order.commission = stage_order.commission;
        finalize_lightning_order.fx_rate = stage_order.fx_rate;
        finalize_lightning_order.network_fee = stage_order.network_fee;
        finalize_lightning_order.bitcoin = stage_order.bitcoin ;
        finalize_lightning_order.lnurl = "LNURL1DP68GURN8GHJ7CT6W3JJUCM09ACXZ72LD9H8VMMFVDJJUURGWQLKKVFAXGENGVF5XC6RZWF48YUNWDEEXVN8GCT884MKJARGV3EXZACGNU6UZ";
        finalize_lightning_order.reference_code = "0000000000000000";
    }

    private IEnumerator api_get_topup_address_test()
    {
        yield return new WaitForSeconds(3);
        
        topup_address.status = "success";
        topup_address.address = "bc1qjlzm9n4zxel8ykluyylqnxq72ajpjuyaj78p6w";
    }

/*
    private IEnumerator api_get_voucher_status_test(string reference_code)
    {
        
    }
*/

    private float calcBtcAmount(float amount, float btc)
    {
        decimal amount_dec =  (decimal)amount;
        decimal btc_dec =  (decimal)btc;

        decimal final = amount_dec / btc_dec;

        //Debug.Log(final);
        return (float)final;
    }


}
