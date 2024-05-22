using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("General")]

    [SerializeField] private Image actorImage;
    [SerializeField] private TextMeshProUGUI actorName;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject continueIcon;
    public bool isActive = false;
    public DialogueTrigger currentNpc;

    [SerializeField] private float typingSpeed = 0.04f;

    [SerializeField] private InputActionReference inputAction;
    private InputAction action;

    [HideInInspector] public Message[] currentMessages;
    [HideInInspector] public Actor[] currentActors;
    [HideInInspector] public int activeMessage = 0;
    [HideInInspector] public Answer[] currentAnswers;

    [Header("Manager")]
    //noch nicht eingebaut (ToDo)
    [SerializeField] private DayManager dayManager;
    [SerializeField] private IndexManager indexManager;

    [Header("Sound")]

    [SerializeField] private bool stopAudio;
    [SerializeField] private bool makePredictable;

    [HideInInspector] public AudioClip[] nDialogueTypingSounds;
    [HideInInspector] public int nFrequenzyLevel = 3;
    [HideInInspector] public float nMinPitch = 0.5f;
    [HideInInspector] public float nMaxPitch = 3f;

    //public AudioClip openSound;
    //public AudioClip closeSound;

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

    public void OpenDialogue(Message[] messages, Actor[] actors, Answer[] answers)
    {
        if (!isActive)
        {
            //source.clip = openSound;
            //source.Play();
            currentMessages = messages;
            currentActors = actors;
            currentAnswers = answers;
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

        if (messageToDisplay.emotion == Emotions.Neutral)
        {
            actorImage.sprite = actorToDisplay.sprite;
        }
        else if(messageToDisplay.emotion == Emotions.Angry)
        {
            actorImage.sprite = actorToDisplay.angrySprite;
        }
        else if(messageToDisplay.emotion == Emotions.Happy)
        {
            actorImage.sprite = actorToDisplay.happySprite;
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
                if(activeMessage == currentMessages.Length-1 && currentAnswers.Length > 0)
                {
                    MakeAnswerButtons();
                    inAnswerScreen = true;
                }
            }
            else
            {
                //source.clip = closeSound;
                //source.Play();
                StartCoroutine(EndDialogue());
                Debug.Log("dialogue ended");
            }
        }
    }

    public void MakeAnswerButtons()
    {
        for(int i = 0; i < currentAnswers.Length; i++)
        {
            if (!currentAnswers[i].clicked)
            {
                GameObject currentButton = Instantiate(buttonPrefab, buttonSpawner.transform);
                currentButton.GetComponentInChildren<TextMeshProUGUI>().text = currentAnswers[i].answerText;
                currentButton.GetComponent<AnswerButton>().answer = currentAnswers[i];
                currentButton.GetComponent<AnswerButton>().currentDialogue = currentNpc.currentDialogue;
                if (currentAnswers[i].questAnswer)
                {
                    currentButton.GetComponent<Image>().color = Color.yellow;
                }
            }
        }
    }

    public void AnswerButton(Message[] messages, Actor[] actors, Answer[] answers)
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
        currentNpc.transform.localScale = npcScale;
        isActive = false;
        foreach(Transform child in buttonSpawner.transform)
        {
            Destroy(child.gameObject);
        }
        currentActors = null;
        currentMessages = null;
        skipToEnd = false;
        StopCoroutine(EndDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForEndOfFrame();
        if (currentNpc.transform.localScale.x != 0)
        {
            npcScale = currentNpc.transform.localScale;
            currentNpc.transform.localScale = Vector3.zero;
        }
        foreach (CharacterSo characterSo in currentNpc.currentDialogue.characters)
        {
            indexManager.AddIndex(characterSo);
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
