using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChoreSo))]
public class ChoreSoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChoreSo chore = (ChoreSo)target;

        chore.possibleDays = (DaysSo)EditorGUILayout.ObjectField("Possible Days", chore.possibleDays, typeof(DaysSo), false);
        if (chore.possibleDays != null && chore.possibleDays.days != null && chore.possibleDays.days.Count > 0)
        {
            int selectedIndex = Mathf.Max(0, chore.possibleDays.days.IndexOf(chore.day));
            selectedIndex = EditorGUILayout.Popup("Day", selectedIndex, chore.possibleDays.days.ConvertAll(d => d.ToString()).ToArray());
            chore.day = chore.possibleDays.days[selectedIndex];
        }
        else
        {
            EditorGUILayout.HelpBox("Please assign a valid DaysSo with at least one day.", MessageType.Warning);
        }
        chore.done = EditorGUILayout.Toggle("Done", chore.done);
        chore.description = EditorGUILayout.TextField("Description", chore.description);
        chore.interviewNpc = (CharacterSo)EditorGUILayout.ObjectField("Interview NPC", chore.interviewNpc, typeof(CharacterSo), false);
        chore.additionalAnswer = (AnswerSo)EditorGUILayout.ObjectField("Additional AnswerSo", chore.additionalAnswer, typeof(AnswerSo), false);
        chore.id = EditorGUILayout.TextField("Id", chore.id);
        //chore.choreHolder = (GameObject)EditorGUILayout.ObjectField("Chore Holder", chore.choreHolder, typeof(GameObject), true);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(chore);
        }
    }
}
