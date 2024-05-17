using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Visual Novel/Dialogue")]
public class Dialogue : ScriptableObject
{
    public NPC sender;
    public NPC receiver;
    public string message;
    public Dialogue nextDialogue;
    public List<Choice> choices;
    public Sprite backdrop;
    public AudioClip soundClip;
    public string nextScene;
}
