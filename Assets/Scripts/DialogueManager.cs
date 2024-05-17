using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image PortraitSender;
    public Image PortraitReceiver;
    public TMP_Text MessageText;
    public GameObject ChoicesPanel;
    public GameObject ChoiceButtonPrefab;
    public Image Backdrop;
    public AudioSource AudioSource;

    private Dialogue currentDialogue;

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        DisplayDialogue();
    }

    private void DisplayDialogue()
    {
        PortraitSender.gameObject.SetActive(currentDialogue.sender != null);
        if (currentDialogue.sender != null)
            PortraitSender.sprite = currentDialogue.sender.portrait;

        PortraitReceiver.gameObject.SetActive(currentDialogue.receiver != null);
        if (currentDialogue.receiver != null)
            PortraitReceiver.sprite = currentDialogue.receiver.portrait;

        MessageText.SetText(currentDialogue.message);
        Backdrop.gameObject.SetActive(currentDialogue.backdrop != null);
        if (currentDialogue.backdrop != null)
            Backdrop.sprite = currentDialogue.backdrop;

        if (currentDialogue.soundClip != null)
        {
            AudioSource.clip = currentDialogue.soundClip;
            AudioSource.Play();
        }

        foreach (Transform child in ChoicesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (currentDialogue.choices != null && currentDialogue.choices.Count > 0)
        {
            foreach (Choice choice in currentDialogue.choices)
            {
                GameObject choiceButton = Instantiate(ChoiceButtonPrefab, ChoicesPanel.transform);
                choiceButton.GetComponentInChildren<TMP_Text>().SetText(choice.text);
                choiceButton.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(choice));
            }
        }
        else if (currentDialogue.nextDialogue != null)
        {
            Invoke(nameof(NextDialogue), 6f); // Auto-advance after 6 seconds
        }
        else if (!string.IsNullOrEmpty(currentDialogue.nextScene))
        {
            // Load next scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentDialogue.nextScene);
        }
    }

    private void OnChoiceSelected(Choice choice)
    {
        if (choice.nextDialogue != null)
        {
            currentDialogue = choice.nextDialogue;
            DisplayDialogue();
        }
        else if (!string.IsNullOrEmpty(choice.nextScene))
        {
            // Load next scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(choice.nextScene);
        }
    }

    private void NextDialogue()
    {
        StartDialogue(currentDialogue.nextDialogue);
    }
}
