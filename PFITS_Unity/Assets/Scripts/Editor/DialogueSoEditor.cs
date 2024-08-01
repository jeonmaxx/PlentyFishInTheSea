using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DialogueSo))]
public class DialogueSoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueSo dialogue = (DialogueSo)target;

        EditorGUILayout.LabelField("AFFINITIED AND DAY", EditorStyles.boldLabel);

        SerializedProperty affinityProperty = serializedObject.FindProperty("affinity");
        EditorGUILayout.PropertyField(affinityProperty, new GUIContent("Affinity"), true);
        dialogue.day = EditorGUILayout.IntField("Day", dialogue.day);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("CHARACTERS AND MESSAGES", EditorStyles.boldLabel);

        SerializedProperty charactersProperty = serializedObject.FindProperty("characters");
        EditorGUILayout.PropertyField(charactersProperty, new GUIContent("Characters"), true);

        SerializedProperty messagesProperty = serializedObject.FindProperty("messages");
        EditorGUILayout.PropertyField(messagesProperty, new GUIContent("Messages"), true);

        SerializedProperty answersProperty = serializedObject.FindProperty("answers");
        EditorGUILayout.PropertyField(answersProperty, new GUIContent("Answers"), true);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("PLAYER TALKS TO THEMSELVES", EditorStyles.boldLabel);
        dialogue.selfTalk = EditorGUILayout.Toggle("Self Talk", dialogue.selfTalk);

        if (dialogue.selfTalk)
        {
            dialogue.roomTalk = EditorGUILayout.Toggle("Room Talk", dialogue.roomTalk);
            if(dialogue.roomTalk)
            {
                dialogue.room = (Rooms)EditorGUILayout.EnumPopup("Room", dialogue.room);
                dialogue.knownRoom = EditorGUILayout.Toggle("Known Room", dialogue.knownRoom);
            }       
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("PLAYER MEETS NPC", EditorStyles.boldLabel);
        dialogue.firstTimeNpc = EditorGUILayout.Toggle("First Time NPC", dialogue.firstTimeNpc);

        if (dialogue.firstTimeNpc)
        {
            dialogue.knownNpc = EditorGUILayout.Toggle("Known NPC", dialogue.knownNpc);
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("ADDITIONAL INFO", EditorStyles.boldLabel);
        dialogue.neededClue = (ClueSo)EditorGUILayout.ObjectField("Needed Clue", dialogue.neededClue, typeof(ClueSo), false);
        if(dialogue.neededClue != null)
        {
            dialogue.dialogueIfNoClue = EditorGUILayout.Toggle("Dialogue If No Clue", dialogue.dialogueIfNoClue);
        }

        dialogue.id = EditorGUILayout.TextField("ID", dialogue.id);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(dialogue);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
