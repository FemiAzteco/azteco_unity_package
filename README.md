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
