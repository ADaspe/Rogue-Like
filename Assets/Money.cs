using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public Text moneycount;
    public int moneynumber = 0;
    public int moneyvalue;

    // Update is called once per frame
    void Update()
    {
        moneycount.text = moneynumber.ToString();

        if (Input.GetKeyDown(KeyCode.M))
        {
            moneynumber += moneyvalue;
        }
    }
}
