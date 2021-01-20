using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ELC_Eclair : MonoBehaviour
{
    public GameObject BarreVie;
    public List<int> percentagesTriggerBlue;
    public List<int> percentagesTriggerRed;


    private void Start()
    {
        this.GetComponent<Image>().enabled = false;
    }

    public void LaunchEclair(string color)
    {
        Animator anim = this.GetComponent<Animator>();
        this.GetComponent<Image>().enabled = true ;
        if (color == "Blue")
        {
            anim.SetBool("Blue", true);
            anim.SetBool("Red", false);
        }
        else if (color == "Red")
        {
            anim.SetBool("Blue", false);
            anim.SetBool("Red", true);
        }

        anim.SetBool("IsActive", true);
        StartCoroutine("PlayAnim");
    }

    private IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<Animator>().SetBool("IsActive", false);
        this.GetComponent<Image>().enabled = false;
    }
}
