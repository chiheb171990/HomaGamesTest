using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System.Linq;

public class JsonManager : SingletonMB<JsonManager>
{

    public string Result; //The request will be saved in the Result variable
    private bool isRequestError;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LoadFromURL(string Url)
    {
        UnityWebRequest www = UnityWebRequest.Get(Url);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);

            //The is error in the request
            isRequestError = true;

            //Enable the notification Panel to show the error
            MainController.Instance.EnableNotificationPanel("Current version allows up to 1 year earlier. \n Please choose another date. ");
        }
        else
        {
            // There is no error in the request
            isRequestError = false;

            // Save results as text
            Result = www.downloadHandler.text;
        }
    }

    //Get Currency conversion in a specific day
    public IEnumerator GetCurrencyConversion(string from,string to,string date)
    {
        yield return LoadFromURL("https://free.currconv.com/api/v7/convert?apiKey=3a33892339936b3e46e4&q=" + from + "_" + to + "&date=" + date);

        //If there is no error in the request
        if (!isRequestError)
        {
            //convert the json text to jsonData object
            JsonData JsonResult = JsonMapper.ToObject(Result);

            //Get the conversion value from the jsonResult and save it into a Variable in the CurrencyDataManager script
            CurrencyDataManager.Instance.ConversionValue = float.Parse(JsonResult["results"][0]["val"][0].ToString());
        }
    }

    //Get currency conversion Now
    public IEnumerator GetCurrencyConversion(string from, string to)
    {
        yield return LoadFromURL("https://free.currconv.com/api/v7/convert?apiKey=3a33892339936b3e46e4&q=" + from + "_" + to);

        //If there is no error in the request
        if (!isRequestError)
        {
            //convert the json text to jsonData object
            JsonData JsonResult = JsonMapper.ToObject(Result);

            //Get the conversion value from the jsonResult and save it into a Variable in the CurrencyDataManager script
            CurrencyDataManager.Instance.ConversionValue = float.Parse(JsonResult["results"][0]["val"].ToString());
        }
    }

    //Get the currency IDs
    public IEnumerator GetCurrencyIDs()
    {
        yield return LoadFromURL("https://free.currconv.com/api/v7/countries?apiKey=3a33892339936b3e46e4");

        //convert the json text to jsonData object
        JsonData JsonResult = JsonMapper.ToObject(Result);

        CurrencyDataManager.Instance.CurrencyIDs = new List<string>();
        for(int i = 0; i < JsonResult["results"].Count; i++)
        {
            CurrencyDataManager.Instance.CurrencyIDs.Add(JsonResult["results"][i]["currencyId"].ToString());
        }

        //delete redundancy from the list
        CurrencyDataManager.Instance.CurrencyIDs = CurrencyDataManager.Instance.CurrencyIDs.Distinct().ToList();

        //sort the list alphabetically
        CurrencyDataManager.Instance.CurrencyIDs.Sort();
    }

}
