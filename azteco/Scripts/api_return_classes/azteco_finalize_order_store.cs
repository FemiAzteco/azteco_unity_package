using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class azteco_finalize_order_store
{
    public string status;
    public string message;
    
    public int order_id;
    public string currency;
    public float bitcoin_price;
    public float total;
    public float purchase_amount;
    public float commission;
    public float fx_rate;
    public float network_fee;
    public float bitcoin;

    public string voucher_code;
    public string reference_code;
}
