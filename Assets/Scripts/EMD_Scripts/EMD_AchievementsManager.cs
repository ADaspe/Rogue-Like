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
    public GameObject ValidateButton;
    public GameObject DialogueCanvas;
    public GameObject QuittButton;
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;
    public Image image5;
    public Image image6;
    int LimitePage = 6;
    private EMD_PassifManager PassiveManagerScript;

    private void Start()
    {
        PassiveManagerScript = FindObjectOfType<EMD_PassifManager>();
    }
    // Update is called once per frame
    public void AfficherAchievements()
    {
        /*image1.sprite = ListAchievements[0].HUDSprite;        Add Images
        image2.sprite = ListAchievements[1].HUDSprite;
        image3.sprite = ListAchievements[2].HUDSprite;
        image1.sprite = ListAchievements[3].HUDSprite;
        image2.sprite = ListAchievements[4].HUDSprite;
        image3.sprite = ListAchievements[5].HUDSprite;*/
        AchievementsButton.SetActive(false);
        ContinueButton.SetActive(false);
        AchivementsContinueButton.SetActive(true);
        AchievementsCanvas.SetActive(true);
    }
    public void Selected1()
    {
        ValidateButton.SetActive(false);
        SelectedAchievement = ListAchievements[0];
        /*if (SelectedAchievement.canUnlockPassif == true)
        {
            AchivementsContinueButton.SetActive(false);
            ValidateButton.SetActive(true);
        }*/
    }
    public void Selected2()
    {
        ValidateButton.SetActive(false);
        SelectedAchievement = ListAchievements[1];
        /*if (SelectedAchievement.canUnlockPassif == true)
        {
            AchivementsContinueButton.SetActive(false);
            ValidateButton.SetActive(true);
        }*/
    }
    public void Selected3()
    {
        ValidateButton.SetActive(false);
        SelectedAchievement = ListAchievements[2];
        /*if (SelectedAchievement.canUnlockPassif == true)
        {
            AchivementsContinueButton.SetActive(false);
            ValidateButton.SetActive(true);
        }*/
    }
    public void Selected4()
    {
        ValidateButton.SetActive(false);
        SelectedAchievement = ListAchievements[3];
        /*if (SelectedAchievement.canUnlockPassif == true)
        {
            AchivementsContinueButton.SetActive(false);
            ValidateButton.SetActive(true);
        }*/
    }
    public void Selected5()
    {
        ValidateButton.SetActive(false);
        ValidateButton.SetActive(false);
        SelectedAchievement = ListAchievements[4];
        /*if (SelectedAchievement.canUnlockPassif == true)
        {
            AchivementsContinueButton.SetActive(false);
            ValidateButton.SetActive(true);
        }*/
    }
    public void Selected6()
    {
        ValidateButton.SetActive(false);
        SelectedAchievement = ListAchievements[5];
        /*if (SelectedAchievement.canUnlockPassif == true)
        {
            AchivementsContinueButton.SetActive(false);
            ValidateButton.SetActive(true);
        }*/
    }
    public void Page2()
    {
        ListAchievements.RemoveRange(1, LimitePage);
        AfficherAchievements();
    }
}
