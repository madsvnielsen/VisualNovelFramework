using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    SerializedProperty senderProp;
    SerializedProperty receiverProp;
    SerializedProperty messageProp;
    SerializedProperty nextDialogueProp;
    SerializedProperty choicesProp;
    SerializedProperty backdropProp;
    SerializedProperty soundClipProp;
    SerializedProperty nextSceneProp;

    void OnEnable()
    {
        senderProp = serializedObject.FindProperty("sender");
        receiverProp = serializedObject.FindProperty("receiver");
        messageProp = serializedObject.FindProperty("message");
        nextDialogueProp = serializedObject.FindProperty("nextDialogue");
        choicesProp = serializedObject.FindProperty("choices");
        backdropProp = serializedObject.FindProperty("backdrop");
        soundClipProp = serializedObject.FindProperty("soundClip");
        nextSceneProp = serializedObject.FindProperty("nextScene");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(senderProp);
        EditorGUILayout.PropertyField(receiverProp);
        EditorGUILayout.PropertyField(messageProp);
        EditorGUILayout.PropertyField(nextDialogueProp);

        EditorGUILayout.LabelField("Choices");
        for (int i = 0; i < choicesProp.arraySize; i++)
        {
            SerializedProperty choice = choicesProp.GetArrayElementAtIndex(i);
            SerializedProperty textProp = choice.FindPropertyRelative("text");
            SerializedProperty nextDialogueChoiceProp = choice.FindPropertyRelative("nextDialogue");
            SerializedProperty nextSceneChoiceProp = choice.FindPropertyRelative("nextScene");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(textProp);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(nextDialogueChoiceProp, GUIContent.none);
            EditorGUILayout.PropertyField(nextSceneChoiceProp, GUIContent.none);
            if (GUILayout.Button("Remove"))
            {
                choicesProp.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Choice"))
        {
            choicesProp.InsertArrayElementAtIndex(choicesProp.arraySize);
        }

        EditorGUILayout.PropertyField(backdropProp);
        EditorGUILayout.PropertyField(soundClipProp);
        EditorGUILayout.PropertyField(nextSceneProp);

        serializedObject.ApplyModifiedProperties();
    }
}
