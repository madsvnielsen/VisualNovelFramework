using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class VisualNovelEditor : EditorWindow
{
    // NPC Fields
    string npcID;
    string npcName;
    Sprite npcPortrait;

    // Dialogue Fields
    string dialogueID;
    string dialogueMessage;
    NPC dialogueSender;
    NPC dialogueReceiver;
    Sprite dialogueBackdrop;
    AudioClip dialogueSoundClip;
    string nextScene;

    // Choice Fields
    List<Choice> newChoices = new List<Choice>();

    // Search Fields
    string npcSearchQuery = "";
    string dialogueSearchQuery = "";

    // Lists
    List<NPC> npcs = new List<NPC>();
    List<Dialogue> dialogues = new List<Dialogue>();

    // Expanded States
    Dictionary<NPC, bool> npcExpandedStates = new Dictionary<NPC, bool>();
    Dictionary<Dialogue, bool> dialogueExpandedStates = new Dictionary<Dialogue, bool>();

    // Collapsible sections
    bool createNPCSectionExpanded = false;
    bool createDialogueSectionExpanded = false;
    bool searchNPCSectionExpanded = false;
    bool searchDialogueSectionExpanded = false;

    // Scroll position
    Vector2 scrollPosition;

    [MenuItem("Tools/Visual Novel Editor")]
    public static void ShowWindow()
    {
        GetWindow<VisualNovelEditor>("Visual Novel Editor");
    }

    void OnEnable()
    {
        RefreshNPCs();
        RefreshDialogues();
    }

    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        GUILayout.Space(10);
        GUI.backgroundColor = Color.cyan;
        createNPCSectionExpanded = EditorGUILayout.Foldout(createNPCSectionExpanded, "Create NPC", true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
        GUI.backgroundColor = Color.white;

        if (createNPCSectionExpanded)
        {
            EditorGUILayout.BeginVertical("box");
            npcID = EditorGUILayout.TextField(new GUIContent("ID", "Unique identifier for the NPC"), npcID);
            npcName = EditorGUILayout.TextField(new GUIContent("Name", "Name of the NPC"), npcName);
            npcPortrait = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Portrait", "Portrait image of the NPC"), npcPortrait, typeof(Sprite), false);

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Create NPC"))
            {
                CreateNPC();
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);
        DrawHorizontalLine();

        GUI.backgroundColor = Color.magenta;
        createDialogueSectionExpanded = EditorGUILayout.Foldout(createDialogueSectionExpanded, "Create Dialogue", true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
        GUI.backgroundColor = Color.white;

        if (createDialogueSectionExpanded)
        {
            EditorGUILayout.BeginVertical("box");
            dialogueID = EditorGUILayout.TextField(new GUIContent("ID", "Unique identifier for the dialogue"), dialogueID);
            dialogueMessage = EditorGUILayout.TextField(new GUIContent("Message", "Dialogue message text"), dialogueMessage);
            dialogueSender = (NPC)EditorGUILayout.ObjectField(new GUIContent("Sender", "NPC who sends the dialogue"), dialogueSender, typeof(NPC), false);
            dialogueReceiver = (NPC)EditorGUILayout.ObjectField(new GUIContent("Receiver", "NPC who receives the dialogue"), dialogueReceiver, typeof(NPC), false);
            dialogueBackdrop = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Backdrop", "Background image for the dialogue"), dialogueBackdrop, typeof(Sprite), false);
            dialogueSoundClip = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Sound Clip", "Sound clip to play with the dialogue"), dialogueSoundClip, typeof(AudioClip), false);
            nextScene = EditorGUILayout.TextField(new GUIContent("Next Scene", "Scene to load after this dialogue"), nextScene);

            GUILayout.Label("Choices:");
            DisplayChoices(newChoices);

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Create Dialogue"))
            {
                CreateDialogue();
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);
        DrawHorizontalLine();

        GUI.backgroundColor = Color.yellow;
        searchNPCSectionExpanded = EditorGUILayout.Foldout(searchNPCSectionExpanded, "Search NPCs", true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
        GUI.backgroundColor = Color.white;

        if (searchNPCSectionExpanded)
        {
            EditorGUILayout.BeginVertical("box");
            npcSearchQuery = EditorGUILayout.TextField("Search", npcSearchQuery);
            DisplayNPCs();
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);
        DrawHorizontalLine();

        GUI.backgroundColor = Color.yellow;
        searchDialogueSectionExpanded = EditorGUILayout.Foldout(searchDialogueSectionExpanded, "Search Dialogues", true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
        GUI.backgroundColor = Color.white;

        if (searchDialogueSectionExpanded)
        {
            EditorGUILayout.BeginVertical("box");
            dialogueSearchQuery = EditorGUILayout.TextField("Search", dialogueSearchQuery);
            DisplayDialogues();
            EditorGUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
    }

    void DrawHorizontalLine()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);
    }

    void CreateNPC()
    {
        NPC newNPC = ScriptableObject.CreateInstance<NPC>();
        newNPC.npcName = npcName;
        newNPC.portrait = npcPortrait;

        string path = "Assets/ScriptableObjects/NPCs/" + npcID + ".asset";
        AssetDatabase.CreateAsset(newNPC, path);
        AssetDatabase.SaveAssets();

        RefreshNPCs();
    }

    void CreateDialogue()
    {
        Dialogue newDialogue = ScriptableObject.CreateInstance<Dialogue>();
        newDialogue.message = dialogueMessage;
        newDialogue.sender = dialogueSender;
        newDialogue.receiver = dialogueReceiver;
        newDialogue.backdrop = dialogueBackdrop;
        newDialogue.soundClip = dialogueSoundClip;
        newDialogue.nextScene = nextScene;
        newDialogue.choices = new List<Choice>(newChoices);

        string path = "Assets/ScriptableObjects/Dialogues/" + dialogueID + ".asset";
        AssetDatabase.CreateAsset(newDialogue, path);
        AssetDatabase.SaveAssets();

        newChoices.Clear(); // Clear the temporary choices list after creation

        RefreshDialogues();
    }

    void RefreshNPCs()
    {
        npcs.Clear();
        string[] guids = AssetDatabase.FindAssets("t:NPC");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            NPC npc = AssetDatabase.LoadAssetAtPath<NPC>(path);
            npcs.Add(npc);
        }
    }

    void RefreshDialogues()
    {
        dialogues.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Dialogue");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Dialogue dialogue = AssetDatabase.LoadAssetAtPath<Dialogue>(path);
            dialogues.Add(dialogue);
        }
    }

    void DisplayNPCs()
    {
        var filteredNPCs = npcs.Where(n => AssetDatabase.GetAssetPath(n).ToLower().Contains(npcSearchQuery.ToLower())).ToList();

        foreach (var npc in filteredNPCs)
        {
            if (!npcExpandedStates.ContainsKey(npc))
            {
                npcExpandedStates[npc] = false;
            }

            GUILayout.BeginHorizontal();
            string npcID = System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(npc));
            npcExpandedStates[npc] = EditorGUILayout.Foldout(npcExpandedStates[npc], npcID);
            if (GUILayout.Button("Select", GUILayout.Width(60)))
            {
                Selection.activeObject = npc;
            }
            GUILayout.EndHorizontal();

            if (npcExpandedStates[npc])
            {
                EditorGUI.indentLevel++;
                npc.npcName = EditorGUILayout.TextField("Name", npc.npcName);
                npc.portrait = (Sprite)EditorGUILayout.ObjectField("Portrait", npc.portrait, typeof(Sprite), false);
                EditorUtility.SetDirty(npc);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Delete NPC"))
                {
                    if (EditorUtility.DisplayDialog("Delete NPC", "Are you sure you want to delete this NPC?", "Yes", "No"))
                    {
                        DeleteNPC(npc);
                    }
                }
                GUI.backgroundColor = Color.white;
                GUILayout.Space(10);
                EditorGUI.indentLevel--;
            }
        }
    }

    void DisplayDialogues()
    {
        var filteredDialogues = dialogues.Where(d => AssetDatabase.GetAssetPath(d).ToLower().Contains(dialogueSearchQuery.ToLower())).ToList();

        foreach (var dialogue in filteredDialogues)
        {
            if (!dialogueExpandedStates.ContainsKey(dialogue))
            {
                dialogueExpandedStates[dialogue] = false;
            }

            GUILayout.BeginHorizontal();
            string dialogueID = System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(dialogue));
            dialogueExpandedStates[dialogue] = EditorGUILayout.Foldout(dialogueExpandedStates[dialogue], dialogueID);
            if (GUILayout.Button("Select", GUILayout.Width(60)))
            {
                Selection.activeObject = dialogue;
            }
            GUILayout.EndHorizontal();

            if (dialogueExpandedStates[dialogue])
            {
                EditorGUI.indentLevel++;
                dialogue.message = EditorGUILayout.TextField("Message", dialogue.message);
                dialogue.sender = (NPC)EditorGUILayout.ObjectField("Sender", dialogue.sender, typeof(NPC), false);
                dialogue.receiver = (NPC)EditorGUILayout.ObjectField("Receiver", dialogue.receiver, typeof(NPC), false);
                dialogue.backdrop = (Sprite)EditorGUILayout.ObjectField("Backdrop", dialogue.backdrop, typeof(Sprite), false);
                dialogue.soundClip = (AudioClip)EditorGUILayout.ObjectField("Sound Clip", dialogue.soundClip, typeof(AudioClip), false);
                dialogue.nextScene = EditorGUILayout.TextField("Next Scene", dialogue.nextScene);

                GUILayout.Label("Choices:");
                DisplayChoices(dialogue.choices);

                EditorUtility.SetDirty(dialogue);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Delete Dialogue"))
                {
                    if (EditorUtility.DisplayDialog("Delete Dialogue", "Are you sure you want to delete this dialogue?", "Yes", "No"))
                    {
                        DeleteDialogue(dialogue);
                    }
                }
                GUI.backgroundColor = Color.white;
                GUILayout.Space(10);
                EditorGUI.indentLevel--;
            }
        }
    }

    void DisplayChoices(List<Choice> choicesList)
    {
        for (int i = 0; i < choicesList.Count; i++)
        {
            GUILayout.BeginVertical("box");
            choicesList[i].text = EditorGUILayout.TextField("Text", choicesList[i].text);
            choicesList[i].nextDialogue = (Dialogue)EditorGUILayout.ObjectField("Next Dialogue", choicesList[i].nextDialogue, typeof(Dialogue), false);
            choicesList[i].nextScene = EditorGUILayout.TextField("Next Scene", choicesList[i].nextScene);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("↑", GUILayout.Width(25)) && i > 0)
            {
                var temp = choicesList[i];
                choicesList[i] = choicesList[i - 1];
                choicesList[i - 1] = temp;
            }
            if (GUILayout.Button("↓", GUILayout.Width(25)) && i < choicesList.Count - 1)
            {
                var temp = choicesList[i];
                choicesList[i] = choicesList[i + 1];
                choicesList[i + 1] = temp;
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Remove"))
            {
                choicesList.RemoveAt(i);
            }
            GUILayout.EndVertical();
        }
        if (GUILayout.Button("Add Choice"))
        {
            choicesList.Add(new Choice());
        }
    }

    void DeleteNPC(NPC npc)
    {
        string path = AssetDatabase.GetAssetPath(npc);
        AssetDatabase.DeleteAsset(path);
        RefreshNPCs();
    }

    void DeleteDialogue(Dialogue dialogue)
    {
        string path = AssetDatabase.GetAssetPath(dialogue);
        AssetDatabase.DeleteAsset(path);
        RefreshDialogues();
    }
}
