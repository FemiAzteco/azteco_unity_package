# Azteco Bitcoin API Unity package
This unity package is a simple framework to allow you to make Azteco API calls, and store the data returned. 

## azteco_store classes
Inside the api_return_classes folder are a set of serialized classes that are instantiated and populated when its api command is called. **do not modify these files**. If their structure and variable names change, the data will not be populated correctly. 

## azteco_api.cs
azteco_api.cs contains methods to run all possible api calls, as well as storing your api key and the base url of the api.
To make an api call, simply call the public method that relates to the call you wish to make. For the methods:
 - api_get_price
 - api_get_balance
 - api_stage_order
 - api_stage_lightning_order

The default currency is "USD". You can add a variable to add any currency supported by [fixer.io](https://fixer.io/).

## Process_and_format
The process and format classes are used to initiate the api call, and format the json that is returned. **it is advised to not modify these unless necessary**. Modifying these methods is likely to break the methods they are used in. 


## why?

You can read more about integrating bitcoin into gaming and its potential [Here](https://docs.google.com/presentation/d/1u5sgRh3uy5l-4slkelrJf392f83tUFSAwSh3lXH1IiQ/edit#slide=id.p).
# how to run tests with an API key 
The only prerequisite you must fulfil before being able to run tests using an API key is to set the *api_key* string in the *azteco_api* class. 

To make an API call, run one of the following scripts and pass the correct data into the method.
|Method to Run| Variables Required |
|--|--|
| run_api_get_price | currency *(string)* |
|run_api_get_balance| currency *(string)* |
|run_api_get_statement||
|run_api_stage_order|currency *(string)*<br /> amount *(float)*|
|run_api_finalize_order| order_id *(int)* |
|run_api_stage_lightning_order|currency *(string)* <br /> amount *(float)*|
run_api_finalize_lightning_order| order_id *(int)* |
|run_api_get_topup_address||
|run_api_get_voucher_status| reference_code *(string)*|
 
 The data is stored in serialized classes contained within the script.

# how to run tests without an API key
To run tests with this package without an API key, you can use the methods in azteco_api_tests. These methods are coroutines, that run after a 3 second delay, to mimic the delay when using the actual api, and they return the same data that a real api call would return. 

| Storage Class | Data |
|--|--|
| azteco_get_price_store | status = "success" <br /> currency = "USD" <br /> bitcoin_price = 59895.03 |
| azteco_get_balance_store | status = "success" <br /> currency = "USD" <br /> balance = 23210.10 <br /> fx_rate = 1.0|
| azteco_stage_order_store | status = "success" <br /> message = "" <br />  order_id = 1549379 <br />  currency = "USD" <br />  bitcoin_price = 57894.03 <br />  total = 500.0 <br /> purchase_amount - 478.85 <br /> comission = 20.0 <br /> fx_rate = 1.0 <br />  network_fee = 0.000012483 <br /> bitcoin = 0.08338755 <br /> ttl = 300|
| azteco_finalize_order_store | status = "success"<br />  message = ""<br /> order_id = 1549379 <br />  currency = "USD" <br />  bitcoin_price = 57894.03 <br />  total = 500.0 <br />  purchase_amount = 478.50 <br />  comission = 20.0 <br />  fx_rate = 1.0 <br />  network_fee = 0.000012483 <br />  bitcoin = 0.08338755 <br /> voucher_code = "1234123412341234" <br />  reference_code = "0000000000000000"|
|azteco_finalize_lightning_order_store| status = "success" <br /> order_id = 1549379 <br /> currency = "USD" <br /> bitcoin_price = 57894.03 <br />  total = 500.0 <br /> purchase_amount = 480.0 <br />  comission = 20.0 <br /> fx_rate = 1.0 <br />  network_fee = 0.0 <br /> bitcoin = 0.08438755 <br />  lnurl = "LNURL1DP68GURN8GHJ7CT6W3JJUCM09ACXZ72LD9H8VMMFVDJJUURGWQLKKVFAXGENGVF5XC6RZWF48YUNWDEEXVN8GCT884MKJARGV3EXZACGNU6UZ" <br /> reference_code = "0000000000000000" |
|azteco_get_topup_address_store| status = "success" <br /> address = "bc1qjlzm9n4zxel8ykluyylqnxq72ajpjuyaj78p6w"|

### Get statement
Get statement returns an array of classes. The first entry in the array contains the success status, total bitcoin sold, totalsales price as well as the total comission earned. The rest of the entries will be information about every voucher you have sold.  below is an example of the first entry in the array, as well as 3 example voucher statements. 
| entry # |  data|
|--|--|
| data in array[0] | status = "success" <br /> bitcoin_total = 2.49383839 <br /> sales_total = 204483.49 <br /> comission_total = 7849.34|
|data in rest of array. |index = 1 <br /> timestamp = 1589483925 <br />  currency = "USD" <br />  total = 300.0 <br /> comission = 12.0 <br /> fx_rate = 1.0 <br />  bitcoin = 0.00178559 <br />  unique_id = "0000000000000000"<br /><br /> index = 2 <br /> timestamp = 1589483926 <br />  currency = "GBP" <br />  total = 750.0<br /> comission = 26.0 <br /> fx_rate = 0.85838 <br />  bitcoin = 0.00188559 <br />  unique_id = "0000000000000000" <br /><br /> index = 3 <br /> timestamp = 1591485929 <br />  currency = "SEK" <br />  total = 1500.0 <br /> comission = 60.0<br /> fx_rate = 10.91282 <br />  bitcoin = 0.00179559 <br />  unique_id = "0000000000000000"|


### Verify voucher status 
Verify voucher status can return one of four responses, shown below. 
| voucher status | Data |
|--|--|
|if valid and unredeemed | status = "success" <br />  genuine = "GENUINE-UNREDEEMED" <br /> redeem_status = 0 <br />  redeem_date = "NULL" <br />  sale_date = 1595861859|
|if valid and redeemed | status = "success" <br /> genuine = "GENUINE-REDEEMED" <br />  redeem_status = 1 <br /> redeem_date = 1595810931 <br />  sale_date = 1595810801|
| if expired | status = "success" <br />  genuine = "GENUINE-EXPIRED" <br />  redeem_status = 2 <br />  reddem_date = "NULL" <br />  sale_date = 1570794913 |
| if bad reference code | status = "success" <br /> genuine = "BAD_REFERENCE_CODE" <br /> redeem_status = "NULL" <br />  redeem_date = "NULL" <br />  sale_date = "NULL"|

### API Failure Responses
If a request to the server is invalid, the API will return a failure response. All posible failure responses are shown below. 
#### stage order
| Failure response | Data |
|--|--|
| if the amount is less than minimum sale | status = "failure" <br /> message = "amount requested less than minimum sale of $20"|
|if the vendor requested 'amount' is greater than avaliable topup balance| status = "failure" <br /> message = "insufficient balance" |
| currency not supported |status = "failure" <br /> message = "currency not supported"|

#### finalize order
|failure response| Data  |
|--|--|
| if ttl is exceeded (5 minutes) | status = "failure" <br /> message = "ttl exceeded" |
|if the order has already been finalized|status = "failure" <br /> message = "order previously fulfilled"|
|if the order id was not found| status = "failure" <br /> message = "order id not found"|

#### stage lightning
|failure response| data |
|--|--|
| if the 'amount' is less than the minimum sale | status = "failure" <br /> message = "amount requested is less than minimum sale of $1" |
|if the vendore requested 'amount' is greater than avaliable balance|status = "failure" <br /> message = "insufficient balance"|
|currency not supported| status = "failure" <br /> message = "currency not supported"|

#### finalize lightning order
|failure response| data |
|--|--|
| if the ttl is exceeded | status = "failure" <br /> message = "ttl exceeded"|
|if the order has already been finalized|status = "failure" <br /> message = "order previously fulfilled"|
|if the order id was not found|status = "failure" <br /> message = "order id not found"|




### Testing & Data specifics

- On average, an API call takes 3 seconds. Make sure you **set a manual delay** when setting the data using a coroutine. 
- The "NULL" values returned in *Verify voucher status* are returned **as strings, not null**. 
- unique_id, voucher_code and reference_code must be stored as **Strings**.
