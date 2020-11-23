using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class comboCounter : MonoBehaviour
{
    public Text combocount;
    public ELC_PlayerStatManager statManager;
    public int CurComb = 0;

    // Update is called once per frame
    void Update()
    {
        combocount.text = statManager.CurrentCombo.ToString();
    }
}
