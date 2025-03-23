using System;
using TMPro;
using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public TMP_Text coinText;

    public void FixedUpdate()
    {
        Debug.Log("mo");
        coinText.text = PlayerPrefs.GetInt("Money").ToString();
    }
}
