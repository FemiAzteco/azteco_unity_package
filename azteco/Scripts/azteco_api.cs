using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class azteco_api : MonoBehaviour
{
    private string api_key = ""; //YOUR KEY HERE
    private string baseURL = "https://api.azte.co/v1/";

    //classes to store the returned data from the api calls. 
    public azteco_get_price_store get_price;
    public azteco_get_balance_store get_balance;
    public azteco_get_statement_store[] get_statement;
    public azteco_stage_order_store stage_order;
    public azteco_finalize_order_store finalize_order;
    public azteco_finalize_lightning_order_store finalize_lightning_order;
    public azteco_get_topup_address_store topup_address;
    public azteco_verify_voucher_status_store verify_voucher_status;
    private string json_string_formatted;

    //methods for initiating api calls
    public IEnumerator run_api_get_price(string currency)
    {
        yield return StartCoroutine(api_get_price(currency));
    }

    public IEnumerator run_api_get_balance(string currency)
    {

        yield return StartCoroutine(api_get_balance(currency));
    }

    public IEnumerator run_api_get_statement()
    {

        yield return StartCoroutine(api_get_statement());
    }

    public IEnumerator run_api_stage_order(string currency , float amount)
    {

        yield return StartCoroutine(api_stage_order(currency, amount));
    }

    public IEnumerator run_api_finalize_order(int order_id)
    {

        yield return StartCoroutine(api_finalize_order(order_id));
    }

    public IEnumerator run_api_stage_lightning_order(string currency , float amount)
    {

        yield return StartCoroutine(api_stage_lightning_order(currency, amount));
    }

    public IEnumerator run_api_finalize_lightning_order(int order_id)
    {

        yield return StartCoroutine(api_finalize_lightning_order(order_id));
    }

    public IEnumerator run_api_get_topup_address()
    {

        yield return StartCoroutine(api_get_topup_address());
    }

    public IEnumerator run_api_get_voucher_status(string reference_code)
    {

        yield return StartCoroutine(api_get_voucher_status(reference_code));
    }


    //************************************************************************************
    //                                  METHODS FOR API CALLS
    //************************************************************************************

    private IEnumerator api_get_price(string currency)
    {
        //create the url to send to the api
        string fullURL = baseURL + "price/" + currency + "/" + api_key;
        
        //process api call and format the json string to populate object
        yield return StartCoroutine(process_and_format(fullURL));

        //set azteco_get_price_store class with the json data
        get_price = JsonUtility.FromJson<azteco_get_price_store>(json_string_formatted);
    }


    private IEnumerator api_get_balance(string currency)
    {
        string fullURL = baseURL + "balance/" + currency + "/" + api_key;

        //process api call and format the json string to populate object
        yield return StartCoroutine(process_and_format(fullURL));
        
        //set public azteco_get_balance_store class with the json data
        get_balance = JsonUtility.FromJson<azteco_get_balance_store>(json_string_formatted); 
    }

    private IEnumerator api_get_statement()
    {
        string fullURL = baseURL + "statement/" + api_key;
        
        yield return StartCoroutine(process_and_format_statement(fullURL));

        //use JsonHelper to create an array of azteco_get_statement_store objects containing api data. 
        get_statement = JsonHelper.FromJson<azteco_get_statement_store>(json_string_formatted); 
    }

    private IEnumerator api_stage_order(string currency , float amount)
    {
        string fullURL = baseURL + "stage/" + currency + "/" + amount + "/" + api_key;
 
        yield return StartCoroutine(process_and_format(fullURL));
        
        //set public azteco_stage_order_store class with the json data, and stores the order_id to be used in finalize order
        stage_order = JsonUtility.FromJson<azteco_stage_order_store>(json_string_formatted);
        //order_id = stage_order.order_id;
    }


    private IEnumerator api_finalize_order(int order_id)
    {
        string fullURL = baseURL + "order/" + order_id + "/" + api_key;

        yield return StartCoroutine(process_and_format(fullURL));
        
        //set public azteco_finalize_order_store class with the json data
        finalize_order = JsonUtility.FromJson<azteco_finalize_order_store>(json_string_formatted);
    }

    private IEnumerator api_stage_lightning_order(string currency, float amount)
    {
        string fullURL = baseURL + "stage_lightning/" + currency + "/" + amount + "/" + api_key;
        
        yield return StartCoroutine(process_and_format(fullURL));
        
        //set public azteco_stage_lightning_order_store class with the json data
        stage_order = JsonUtility.FromJson<azteco_stage_order_store>(json_string_formatted);
        //order_id = stage_order.order_id;
    }

    private IEnumerator api_finalize_lightning_order(int order_id)
    {
        string fullURL = baseURL + "order_lightning/" + order_id + "/" + api_key;
        
        yield return StartCoroutine(process_and_format(fullURL));
        
        //set public azteco_finalize_lightning_order_store class with the json data
        finalize_lightning_order = JsonUtility.FromJson<azteco_finalize_lightning_order_store>(json_string_formatted);
        //reference_code = finalize_lightning_order.reference_code;
    }

    private IEnumerator api_get_topup_address()
    {
        string fullURL = baseURL + "topup/"+ api_key;
        
        yield return StartCoroutine(process_and_format(fullURL));
        
        //set public azteco_stopup_address_store class with the json data
        topup_address = JsonUtility.FromJson<azteco_get_topup_address_store>(json_string_formatted);
    }


    private IEnumerator api_get_voucher_status(string reference_code)
    {
        string fullURL = baseURL + "verify/" + reference_code + "/" + api_key;
        
        yield return StartCoroutine(process_and_format(fullURL));
        
        //set public azteco_stage_verify_voucher_status_store class with the json data
        verify_voucher_status = JsonUtility.FromJson<azteco_verify_voucher_status_store>(json_string_formatted);
    }


    //************************************************************************************
    //                                  PROCESS AND FORMAT
    //************************************************************************************



    private IEnumerator process_and_format(string fullURL)
    {
        //create and send the request to the api
        UnityWebRequest request = UnityWebRequest.Get(fullURL);
        yield return request.SendWebRequest();

        //save the returned value
        string json_string = request.downloadHandler.text; 

        //trim answer to remove right angle brackets from string
        json_string = json_string.Trim('[');
        json_string = json_string.Trim(']');

        //set formatted string
        json_string_formatted = json_string;
    }

    private IEnumerator process_and_format_statement(string fullURL)
    {
        UnityWebRequest request = UnityWebRequest.Get(fullURL);

        //send the request to the api
        yield return request.SendWebRequest();

        //save the returned value
        string json_string = request.downloadHandler.text; 

        //fix formatting of the json so it can be read by unity jsonparser
        json_string = JsonHelper.fixJson(json_string);

        json_string_formatted = json_string;
    }

}
