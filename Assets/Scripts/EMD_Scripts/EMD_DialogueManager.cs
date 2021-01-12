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
    // récuperer les données du npc (phrase et nom)
    public float DelayTime;
    // récuperer la variable DialogueOn du script trigger
    private bool IsWriting;
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
    public GameObject ActualNPC;
    public ELC_PassiveSO SelectedPassive; //le ramener du passifmanager
    public string PNJName;
    private EMD_PassifManager PassiveManagerScript;
    private EMD_AchievementsManager AchievementManagerScript;
    private ELC_PlayerMoves PlayerMovesScript;
    bool IsAchievement = false;


    private void Start()
    {
        PassiveManagerScript = FindObjectOfType<EMD_PassifManager>();
        AchievementManagerScript = FindObjectOfType<EMD_AchievementsManager>();
        PlayerMovesScript = FindObjectOfType<ELC_PlayerMoves>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Dash"))
        {
            QuitDialogue();
        }
        if ( DialogueIsActive && Input.GetButtonDown("Interact"))
        {
            //Debug.Log("yes");
            NextSentence();
        }
    }


    IEnumerator Type()
    {
        textDisplay.text = null;
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
            foreach (char letter in sentences[2].ToCharArray())
            {
                PassiveToUnlock.text += letter;
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
        //besoin de remettre les inputs du joueur
    }

    public void NextSentence()
    {
        if (!IsWriting)
        {
            if (index < sentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
                StartCoroutine("StartDialogue");
            }
            else
            {
                //textDisplay.text = "";
                //ChangeButton();
                QuitDialogue();
            }
        }
    }
    public void ChangePassive()
    {
        sentences[0] = PassiveManagerScript.SelectedPassive.PassiveDescription;
        StartCoroutine("Type");
        //IsWriting = true;
    }
    public void ChangeAchievements()
    {
        IsAchievement = true;
        sentences[0] = "Nom : " + AchievementManagerScript.SelectedAchievement.achievementName;
        Debug.Log("Phrase 1 : "+ sentences[0]);
        sentences[1] = AchievementManagerScript.SelectedAchievement.ennemyToDefeat.name + " : " + AchievementManagerScript.SelectedAchievement.numberDefeated + " / " + AchievementManagerScript.SelectedAchievement.numberToDefeat;
        Debug.Log("Phrase 2 : " + sentences[1]);
        Debug.Log(AchievementManagerScript.SelectedAchievement.passifToUnlock.name);
        sentences[2] = "Nom Passif : " + AchievementManagerScript.SelectedAchievement.passifToUnlock.name;
        Debug.Log("Phrase 1 : " + sentences[2]);
        StartCoroutine("Type");
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
        PNJName = ActualNPC.GetComponent<EMD_Sentences>().PNJName;
        SentencesUpdate(ActualNPC.GetComponent<EMD_Sentences>().sentences); //Ne se remet pas quand on quitte et rejoin une conv
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
        IsWriting = true;
        //yield return new WaitWhile(() => IsWriting == true);
        yield return null;
    }
    public void SentencesUpdate(string[] sentencesToAdd)
    {
        for (int i = 0; i < sentencesToAdd.Length; i++)
        {
            sentences[i] = sentencesToAdd[i];
        }
    }
}
