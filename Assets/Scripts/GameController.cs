using UnityEngine;

public class GameController : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Dialogue startingDialogue;

    void Start()
    {
        dialogueManager.StartDialogue(startingDialogue);
    }
}
