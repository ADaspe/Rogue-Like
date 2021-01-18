using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EMD_DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI AchievementName;
    public TextMeshProUGUI EnnemyToDestroy;
    public TextMeshProUGUI PassiveToUnlock;
    public bool DialogueIsActive;
    [SerializeField]
    private int index;
    public float DelayTime;
    public bool IsWriting;
    public GameObject PassiveCanvas;
    public GameObject DialogueCanvas;
    public GameObject ContinueButton;
    public GameObject QuitButton;
    public GameObject PassiveButton;
    public GameObject AchievementsButton;
    public GameObject ValidateButton;
    public GameObject AchievementCanvas;
    public List<EMD_NPCIsTrigger> NPCsList;
    public string[] sentences = new string[3];
    public string[] Achsentences = new string[3];
    public string[] PasSentences = new string[1];
    public GameObject ActualNPC;
    public ELC_PassiveSO SelectedPassive; 
    public string PNJName;
    private EMD_PassifManager PassiveManagerScript;
    private EMD_AchievementsManager AchievementManagerScript;
    private ELC_PlayerMoves PlayerMovesScript;
    bool IsAchievement = false;
    bool IsPassive = false;


    private void Start()
    {
        PassiveManagerScript = FindObjectOfType<EMD_PassifManager>();
        AchievementManagerScript = FindObjectOfType<EMD_AchievementsManager>();
        PlayerMovesScript = FindObjectOfType<ELC_PlayerMoves>();
    }
    private void Update()
    {
        if (DialogueIsActive && (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Dash")))
        {
            QuitDialogue();
        }
        //if ( DialogueIsActive && Input.GetButtonDown("Interact"))
        //{
        //    NextSentence();
        //}
    }


    /*IEnumerator Type()
    {
        IsWriting = true;
        textDisplay.text = null;
        AchievementName.text = null;
        EnnemyToDestroy.text = null;
        PassiveToUnlock.text = null;
        if (IsAchievement == true)
        {
            foreach (char letter in sentences[0].ToCharArray())
            {
                AchievementName.text += letter;
                yield return new WaitForSeconds(DelayTime);
            }
            foreach (char letter in sentences[1].ToCharArray())
            {
                EnnemyToDestroy.text += letter;
                yield return new WaitForSeconds(DelayTime);
            }
            if (AchievementManagerScript.SelectedAchievement.passifToUnlock != null)
            {
                foreach (char letter in sentences[2].ToCharArray())
                {
                    PassiveToUnlock.text += letter;
                    yield return new WaitForSeconds(DelayTime);
                }
            }
        }
        else
        {
            foreach (char letter in sentences[index].ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(DelayTime);
            }
        }
        IsWriting = false;
    }*/
    IEnumerator Type()
    {
        IsWriting = true;
        textDisplay.text = null;
        AchievementName.text = null;
        EnnemyToDestroy.text = null;
        PassiveToUnlock.text = null;
        if (IsAchievement == true)
        {
            foreach (char letter in Achsentences[0].ToCharArray())
            {
                AchievementName.text += letter;
                yield return new WaitForSeconds(DelayTime);
            }
            foreach (char letter in Achsentences[1].ToCharArray())
            {
                EnnemyToDestroy.text += letter;
                yield return new WaitForSeconds(DelayTime);
            }
            if (AchievementManagerScript.SelectedAchievement.passifToUnlock != null)
            {
                foreach (char letter in Achsentences[2].ToCharArray())
                {
                    PassiveToUnlock.text += letter;
                    yield return new WaitForSeconds(DelayTime);
                }
            }
        }
        else if (IsPassive == true)
        {
            foreach (char letter in PasSentences[0].ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(DelayTime);
            }
        }
        else
        {
            foreach (char letter in sentences[index].ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(DelayTime);
            }
        }
        IsWriting = false;
    }

    public void ChangeButton()
    {
        ContinueButton.SetActive(false);
        QuitButton.SetActive(true);
    }

    public void QuitDialogue()
    {
        DialogueCanvas.SetActive(false);
        index = 0;
        textDisplay.text = "";
        DialogueIsActive = false;
        PlayerMovesScript.ToggleMenu();
        AchievementManagerScript.NumeroPage = 0;
    }

    public void NextSentence()
    {
        if (!IsWriting)
        {
            if (index < sentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
                PlayerMovesScript.ToggleMenu();
                StartCoroutine("StartDialogue");
            }
            else
            {
                QuitDialogue();
            }
        }
    }
    public void ChangePassive()
    {
        if (!IsWriting)
        {
            IsPassive = true;
            PasSentences[0] = PassiveManagerScript.SelectedPassive.PassiveDescription;
            StartCoroutine("Type");
            
        }
    }
    public void ChangeAchievements()
    {
        if (!IsWriting)
        {
            IsAchievement = true;
            Achsentences[0] = "Nom : " + AchievementManagerScript.SelectedAchievement.achievementName;
            Achsentences[1] = AchievementManagerScript.SelectedAchievement.ennemyToDefeatName + " : " + AchievementManagerScript.SelectedAchievement.numberDefeated + " / " + AchievementManagerScript.SelectedAchievement.numberToDefeat;
            if (AchievementManagerScript.SelectedAchievement.passifToUnlock != null)
            {
                Achsentences[2] = "Nom Passif : " + AchievementManagerScript.SelectedAchievement.passifToUnlock.name;
            }
            StartCoroutine("Type");
        }
    } 

    public IEnumerator StartDialogue()
    {
        AchievementCanvas.SetActive(false);
        PassiveCanvas.SetActive(false);
        AchievementsButton.SetActive(false);
        PassiveButton.SetActive(false);
        ValidateButton.SetActive(false);
        DialogueCanvas.SetActive(true);
        PlayerMovesScript.ToggleMenu();
        IsAchievement = false;
        IsPassive = false;
        PNJName = ActualNPC.GetComponent<EMD_Sentences>().PNJName;
        sentences = ActualNPC.GetComponent<EMD_Sentences>().sentences;
        //SentencesUpdate(ActualNPC.GetComponent<EMD_Sentences>().sentences); //Ne se remet pas quand on quitte et rejoin une conv
        ContinueButton.SetActive(true);
        if (PNJName == "PassiveMerchant") 
        {
            PassiveButton.SetActive(true);
        }
        else if (PNJName == "AchievementsMerchant") 
        {
            AchievementsButton.SetActive(true);
        }
        DialogueIsActive = true;
        StartCoroutine("Type");
        //yield return new WaitWhile(() => IsWriting == true);
        yield return null;
    }
    /*public void SentencesUpdate(string[] sentencesToAdd)
    {
        for (int i = 0; i < sentencesToAdd.Length - 1; i++)
        {
            sentences[i] = sentencesToAdd[i];
        }
    }*/
}
