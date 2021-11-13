using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundSetting : MonoBehaviour
{
    [SerializeField]
    private Text roundText;

    private int roundValue;
    public void Start()
    {
        roundValue = PlayerPrefs.GetInt("RoundValue", 1);
        roundText.text = roundValue.ToString();
    }

    public void roundUp()
    {
        if(roundValue != 10)
        {
            roundValue++;
            saveValue();
        }
    }

    public void roundDown()
    {
        if (roundValue != 1)
        {
            roundValue--;
            saveValue();
        }
    }

    public void saveValue()
    {
        PlayerPrefs.SetInt("RoundValue", roundValue);
        roundText.text = roundValue.ToString();
    }
}
