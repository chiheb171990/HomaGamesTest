using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class JsonManager : SingletonMB<JsonManager>
{

    public string Result; //The request will be saved in the Result variable

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetCurrencyConversion("EUR","TND","2018-11-18"));
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
        }
        else
        {
            // Show results as text
            Result = www.downloadHandler.text;
        }
    }

    public IEnumerator GetCurrencyConversion(string from,string to,string date)
    {
        yield return LoadFromURL("https://free.currconv.com/api/v7/convert?apiKey=3a33892339936b3e46e4&q=" + from + "_" + to + "&date=" + date);
        print("https://free.currconv.com/api/v7/convert?apiKey=3a33892339936b3e46e4&q=" + from + "_" + to + "&date=" + date);
        print(Result);
        JsonData JsonResult = JsonMapper.ToObject(Result);
        print(JsonResult["results"][0]["val"][0]);
    }
    
}
