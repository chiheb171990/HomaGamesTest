using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Panels")]
    public GameObject ConnectionCheckPanel;
    public GameObject CurrencyPanel;
    public GameObject DatePanel;

    [Header("UI Elements")]
    public Dropdown DropDownFrom;
    public Dropdown DropDownTo;
    public InputField CuerrencyValue;
    public InputField CurrencyResult;
    public Dropdown YearDropDown;
    public Dropdown MonthDropDown;
    public Dropdown DayDropDown;

    //Private Variables
    private bool IsDateNow = false;

    void Start()
    {
        InitScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitScene()
    {
        StartCoroutine(_InitScene());
    }

    public IEnumerator _InitScene()
    {
        //Check for internet Connection
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Activate the check connection panel
            ConnectionCheckPanel.SetActive(true);
            CurrencyPanel.SetActive(false);
        }

        //If there is connection Begin the scene initiation
        else
        {

            //Activate the Currency panel
            ConnectionCheckPanel.SetActive(false);
            CurrencyPanel.SetActive(true);

            //Get the currency IDs from the Currency API
            yield return JsonManager.Instance.GetCurrencyIDs();

            //Clear the DropDown options
            DropDownFrom.options.Clear();
            DropDownTo.options.Clear();

            //Fill the DropDown options with the Currency IDs
            DropDownFrom.AddOptions(CurrencyDataManager.Instance.CurrencyIDs);
            DropDownTo.AddOptions(CurrencyDataManager.Instance.CurrencyIDs);
        }
    }

    public void CheckIsDateNow()
    {
        if (IsDateNow)
        {
            DatePanel.SetActive(true);
        }
        else
        {
            DatePanel.SetActive(false);
        }

        IsDateNow = !IsDateNow;
    }

    public void GetCurrencyResult()
    {
        StartCoroutine(_GetCurrencyResult());
    }

    public IEnumerator _GetCurrencyResult()
    {
        // Set the DropDown Values intp string variables
        string date = YearDropDown.options[YearDropDown.value].text+"-"+ MonthDropDown.options[MonthDropDown.value].text + "-"+ DayDropDown.options[DayDropDown.value].text;
        string from = DropDownFrom.options[DropDownFrom.value].text;
        string to = DropDownTo.options[DropDownTo.value].text;

        //Execute the conversion request 
        if (IsDateNow)
        {
            yield return StartCoroutine(JsonManager.Instance.GetCurrencyConversion(from, to));
        }
        else
        {
            yield return StartCoroutine(JsonManager.Instance.GetCurrencyConversion(from, to,date));
        }

        float AmountResult = CurrencyDataManager.Instance.ConversionValue * float.Parse(CuerrencyValue.text);
        CurrencyResult.text = AmountResult.ToString();
    }
}
