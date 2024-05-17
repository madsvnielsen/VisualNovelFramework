using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "Visual Novel/NPC")]
public class NPC : ScriptableObject
{
    public string npcName;
    public Sprite portrait;
}
