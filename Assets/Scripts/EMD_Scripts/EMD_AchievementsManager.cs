using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EMD_AchievementsManager : MonoBehaviour
{
    //public List<AXD_AchievementSO> ListAchievements;
    public List<AXD_AchievementSO> ListAchievements = new List<AXD_AchievementSO>();
    public AXD_AchievementSO SelectedAchievement;
    public GameObject ContinueButton;
    public GameObject AchivementsContinueButton;
    public GameObject AchievementsButton;
    public GameObject AchievementsCanvas;
    public GameObject DialogueCanvas;
    public GameObject QuittButton;
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;
    public Image image5;
    public Image image6;
    int LimitePage = 6;
    private EMD_DialogueManager DialogueManagerScript;
    public int NumeroPage = 0;

    private void Start()
    {
        DialogueManagerScript = FindObjectOfType<EMD_DialogueManager>();
    }
    // Update is called once per frame
    public void AfficherAchievements()
    {
        image1.sprite = ListAchievements[0 + NumeroPage * 6].HUDSprite;
        image2.sprite = ListAchievements[1 + NumeroPage * 6].HUDSprite;
        image3.sprite = ListAchievements[2 + NumeroPage * 6].HUDSprite;
        image4.sprite = ListAchievements[3 + NumeroPage * 6].HUDSprite;
        image5.sprite = ListAchievements[4 + NumeroPage * 6].HUDSprite;
        image6.sprite = ListAchievements[5 + NumeroPage * 6].HUDSprite;
        AchievementsButton.SetActive(false);
        ContinueButton.SetActive(false);
        AchivementsContinueButton.SetActive(true);
        AchievementsCanvas.SetActive(true);
    }
    public void Selected1()
    {
        if (DialogueManagerScript.IsWriting == false)
        {
            SelectedAchievement = ListAchievements[0 + NumeroPage * 6];
        }        
    }
    public void Selected2()
    {
        if (DialogueManagerScript.IsWriting == false)
        {
            SelectedAchievement = ListAchievements[1 + NumeroPage * 6];
        }
    }
    public void Selected3()
    {
        if (DialogueManagerScript.IsWriting == false)
        {
            SelectedAchievement = ListAchievements[2 + NumeroPage * 6];
        }
    }
    public void Selected4()
    {
        if (DialogueManagerScript.IsWriting == false)
        {
            SelectedAchievement = ListAchievements[3 + NumeroPage * 6];
        }
    }
    public void Selected5()
    {
        if (DialogueManagerScript.IsWriting == false)
        {
            SelectedAchievement = ListAchievements[4 + NumeroPage * 6];
        }
    }
    public void Selected6()
    {
        if (DialogueManagerScript.IsWriting == false)
        {
            SelectedAchievement = ListAchievements[5 + NumeroPage * 6];
        }
    }
    public void PageSuivante()
    {
        if (NumeroPage < 3)
        {
            NumeroPage++;
            AfficherAchievements();
        }
    }
    public void PagePrecedente()
    {
        if (NumeroPage > 0)
        {
            NumeroPage--;
            AfficherAchievements();
        }
    }
}
