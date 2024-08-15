using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public class DialogueManager : MonoBehaviour
{
    [Header("General")]

    [SerializeField] private Image actorImage;
    [SerializeField] private TextMeshProUGUI actorName;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject continueIcon;
    public bool isActive = false;
    public DialogueTrigger currentNpc;
    public DialogueRoom dialogueRoom;
    public Volume volume;

    [SerializeField] private float typingSpeed = 0.04f;

    [SerializeField] private InputActionReference inputAction;
    private InputAction action;

    [HideInInspector] public Message[] currentMessages;
    [HideInInspector] public Actor[] currentActors;
    [HideInInspector] public int activeMessage = 0;
    [HideInInspector] public List<AnswerSo> currentAnswers = new List<AnswerSo>();
    public DialogueSo currentDialogue;

    [Header("Manager")]
    [SerializeField] private DayManager dayManager;
    [SerializeField] private IndexManager indexManager;
    public ClueManager clueManager;
    private NpcManager npcManager;
    private RoomManager roomManager;

    [Header("Sound")]

    [SerializeField] private bool stopAudio;
    [SerializeField] private bool makePredictable;

    [HideInInspector] public AudioClip[] nDialogueTypingSounds;
    [HideInInspector] public int nFrequenzyLevel = 3;
    [HideInInspector] public float nMinPitch = 0.5f;
    [HideInInspector] public float nMaxPitch = 3f;

    private AudioSource source;

    [Header("Answers")]
    [SerializeField] private GameObject buttonSpawner;
    [SerializeField] private GameObject buttonPrefab;

    private bool inAnswerScreen = false;

    [SerializeField] private Vector3 npcScale = Vector3.one;

    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    private bool skipToEnd = false;
    private bool canSkip = false;

    public void Start()
    {
        action = inputAction.action;
        source = this.gameObject.AddComponent<AudioSource>();
        volume.weight = 0;
        npcManager = FindObjectOfType<NpcManager>();
        roomManager = FindObjectOfType<RoomManager>();
    }

    public void Update()
    {
        action.started += _ => OnSkip();
    }

    public void OnSkip()
    {
        if (isActive)
        {
            NextMessage();
            skipToEnd = true;
        }     
    }

    public void OpenDialogue(Message[] messages, Actor[] actors, List<AnswerSo> answers, DialogueSo dialogueSo)
    {
        if (!isActive)
        {
            volume.weight = 1f;
            currentMessages = messages;
            currentActors = actors;
            currentAnswers = answers;
            currentDialogue = dialogueSo;
            activeMessage = 0;                    
            DisplayMessage();
            StartCoroutine(StartDialogue());
        }
    }

    public void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        nDialogueTypingSounds = actorToDisplay.dialogueTypingSounds;
        nFrequenzyLevel = actorToDisplay.frequenzyLevel;
        nMinPitch = actorToDisplay.minPitch;
        nMaxPitch = actorToDisplay.maxPitch;

        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }
        displayLineCoroutine = StartCoroutine(DisplayLine(messageToDisplay.message));

        switch (messageToDisplay.emotion)
        {
            case Emotions.Neutral:
                actorImage.sprite = actorToDisplay.sprite;
                break;
            case Emotions.Happy:
                actorImage.sprite = actorToDisplay.happySprite ?? actorToDisplay.sprite;
                break;
            case Emotions.Angry:
                actorImage.sprite = actorToDisplay.angrySprite ?? actorToDisplay.sprite;
                break;
            case Emotions.Sad:
                actorImage.sprite = actorToDisplay.sadSprite ?? actorToDisplay.sprite;
                break;
            case Emotions.Disgust:
                actorImage.sprite = actorToDisplay.disgustSprite ?? actorToDisplay.sprite;
                break;
            case Emotions.Fear:
                actorImage.sprite = actorToDisplay.fearSprite ?? actorToDisplay.sprite;
                break;
            case Emotions.Surprise:
                actorImage.sprite = actorToDisplay.surpriseSprite ?? actorToDisplay.sprite;
                break;
            case Emotions.Evil:
                actorImage.sprite = actorToDisplay.evilSprite ?? actorToDisplay.sprite;
                break;
            case Emotions.Excited:
                actorImage.sprite = actorToDisplay.excitedSprite ?? actorToDisplay.sprite;
                break;
            default:
                actorImage.sprite = actorToDisplay.sprite;
                break;
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        messageText.text = "";
        int currentCharas = 0;

        continueIcon.transform.localScale = Vector3.zero;
        buttonSpawner.transform.localScale = Vector3.zero; 

        canContinueToNextLine = false;
        skipToEnd = false;

        StartCoroutine(CanSkip());

        foreach(char letter in line.ToCharArray())
        {
            if(canSkip && skipToEnd)
            {
                skipToEnd = false;
                messageText.text = line;
                break;
            }
            else if(!canSkip)
            {
                skipToEnd = false;
            }
            PlayDiaSound(currentCharas, letter);
            messageText.text += letter;
            currentCharas++;
            yield return new WaitForSeconds(typingSpeed);
        }

        continueIcon.transform.localScale = Vector3.one;
        buttonSpawner.transform.localScale = Vector3.one;
        
        canContinueToNextLine = true;
        canSkip = false;
    }

    private IEnumerator CanSkip()
    {
        canSkip = false;
        yield return new WaitForSeconds(0.1f);
        canSkip = true;
    }

    public void NextMessage()
    {
        if (isActive && !inAnswerScreen && canContinueToNextLine)
        {
            activeMessage++;
            if (activeMessage < currentMessages.Length)
            {
                DisplayMessage();
            }

            if (currentDialogue.setChoreDone != null)
            {
                foreach (ChoreSo chore in dayManager.chores)
                {
                    if (chore == currentDialogue.setChoreDone)
                    {
                        chore.done = true;
                    }
                }
            }

            if (activeMessage >= currentMessages.Length)
            {
                // Überprüfen, ob gültige Antworten zum Anzeigen vorhanden sind
                bool hasValidAnswers = false;
                foreach (AnswerSo answer in currentAnswers)
                {
                    if (!answer.clicked && 
                        (answer.neededClue == null || 
                        (answer.neededClue.clueNoted && !answer.notIfClueIsThere) || 
                        (!answer.neededClue.clueNoted && answer.notIfClueIsThere)) && 
                        ((answer.isChore != null && dayManager.choresOfToday.Contains(answer.isChore) &&
                        (!answer.isChore.done || answer.isChore.type == ChoreType.InterviewNumber)) || 
                        answer.isChore == null))
                    {
                        hasValidAnswers = true;
                        break;
                    }
                }

                if (currentDialogue.clueToAdd != null && !clueManager.foundClues.Contains(currentDialogue.clueToAdd))
                {
                    clueManager.AddClue(currentDialogue.clueToAdd, null);
                }

                if (currentDialogue.choreToAdd != null && !dayManager.choresOfToday.Contains(currentDialogue.choreToAdd))
                {
                    currentDialogue.choreToAdd.noted = true;
                    dayManager.AddChores();
                }

                if (hasValidAnswers)
                {
                    if (currentNpc != null)
                    {
                        currentNpc.SetKnownNpc();
                    }
                    MakeAnswerButtons();
                    inAnswerScreen = true;
                }
                else
                {
                    if (currentNpc != null)
                    {
                        currentNpc.SetKnownNpc();
                    }
                    StartCoroutine(EndDialogue());
                }

            }
        }
    }

    public void MakeAnswerButtons()
    {
        foreach (AnswerSo answer in currentAnswers)
        {
            if (!answer.clicked &&
                (answer.neededClue == null ||
                (answer.neededClue.clueNoted && !answer.notIfClueIsThere) ||
                (!answer.neededClue.clueNoted && answer.notIfClueIsThere)) &&
                ((answer.isChore != null && dayManager.choresOfToday.Contains(answer.isChore) &&
                (!answer.isChore.done || answer.isChore.type == ChoreType.InterviewNumber)) ||
                answer.isChore == null))
            {
                GameObject currentButton = Instantiate(buttonPrefab, buttonSpawner.transform);
                currentButton.GetComponentInChildren<TextMeshProUGUI>().text = answer.answerText;
                currentButton.GetComponent<AnswerButton>().answer = answer;
                if (currentNpc != null)
                {
                    currentButton.GetComponent<AnswerButton>().currentDialogue = currentNpc.currentDialogue;
                }
                if (dialogueRoom != null)
                {
                    currentButton.GetComponent<AnswerButton>().currentDialogue = dialogueRoom.currentDialogue;
                }
                if (answer.isChore != null)
                {
                    currentButton.GetComponent<Image>().color = Color.green;
                }
            }
        }
    }
    
    public void AnswerButton(Message[] messages, Actor[] actors, List<AnswerSo> answers)
    {
        inAnswerScreen = false;
        currentMessages = messages;
        currentActors = actors;
        currentAnswers = answers;
        activeMessage = 0;
        foreach (Transform child in buttonSpawner.transform)
        {
            Destroy(child.gameObject);
        }
        DisplayMessage();
        StartCoroutine(StartDialogue());
    }
   
    private IEnumerator EndDialogue()
    {
        yield return new WaitForEndOfFrame();
        transform.localScale = Vector3.zero;
        if (currentNpc != null)
        {
            currentNpc.transform.localScale = npcScale;
        }
        volume.weight = 0;
        isActive = false;
        foreach(Transform child in buttonSpawner.transform)
        {
            Destroy(child.gameObject);
        }
        currentActors = null;
        currentMessages = null;
        currentNpc = null;
        dialogueRoom = null;
        skipToEnd = false;

        if(currentDialogue.npcGoesAfter)
        {
            foreach(CharacterSo character in currentDialogue.characters)
            {
                for (int i = 0; i < npcManager.npcObjects.Count; i++)
                {
                    if (character.id == npcManager.npcObjects[i].character.id)
                    {
                        npcManager.npcObjects[i].characterObj.SetActive(false);
                        break;
                    }
                }
            }
        }
        StopCoroutine(EndDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForEndOfFrame();
        transform.localScale = Vector3.one;
        if (currentNpc != null)
        {
            if (currentNpc.transform.localScale.x != 0)
            {
                npcScale = currentNpc.transform.localScale;
                currentNpc.transform.localScale = Vector3.zero;
            }
        }
        foreach (CharacterSo currentCharacter in currentDialogue.characters)
        {
            RoomDefinition currentRoom = roomManager.activeRoom.GetComponent<RoomDefinition>();
            currentCharacter.lastRoom = currentRoom.roomName;
            indexManager.AddIndex(currentCharacter);
        }
        isActive = true;
        StopCoroutine(StartDialogue());
    }

    public void LeaveButton()
    {
        inAnswerScreen = false;
        StartCoroutine(EndDialogue());
    }

    private void PlayDiaSound(int currentCharaCount, char currentChara)
    {
        if(currentCharaCount % nFrequenzyLevel == 0)
        {
            if (stopAudio)
            {
                source.Stop();
            }

            AudioClip soundClip = null;
            if(makePredictable)
            {
                int hashCode = currentChara.GetHashCode();
                //sound clip
                int predictableIndex = hashCode % nDialogueTypingSounds.Length;
                soundClip = nDialogueTypingSounds[predictableIndex];
                //pitch
                int minPitchInt = (int)(nMinPitch * 100);
                int maxPitchInt = (int)(nMaxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;

                if(pitchRangeInt!= 0)
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + maxPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                    source.pitch = predictablePitch;
                }
                else
                {
                    source.pitch = nMinPitch;
                }
            }
            else
            {
                int randomIndex = Random.Range(0, nDialogueTypingSounds.Length);
                soundClip = nDialogueTypingSounds[randomIndex];
                source.pitch = Random.Range(nMinPitch, nMaxPitch);
            }

            source.PlayOneShot(soundClip);
        }
    }
}
