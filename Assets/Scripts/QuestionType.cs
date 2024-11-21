using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestionType")]
public class QuestionTypeScriptableObject : ScriptableObject
{
    [TextArea, Tooltip("Use [H] for hint, [P] for plant, [O] for feature/object with the square brackets.")]
    public string questionTypeEntries;
    
    public bool useFeaturePluralName;
}