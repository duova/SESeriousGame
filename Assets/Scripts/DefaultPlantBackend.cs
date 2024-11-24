public class DefaultPlantBackend : IPlantBackend
{
    private PlantLibraryScriptableObject _plantLibrary;

    private QuestionLibraryScriptableObject _questionLibrary;

    public DefaultPlantBackend(PlantLibraryScriptableObject plantLibrary,
        QuestionLibraryScriptableObject questionLibrary)
    {
        _plantLibrary = plantLibrary;
        _questionLibrary = questionLibrary;
    }

    public QuestionSet GetQuestion(QuestionLocation questionLocation)
    {
        throw new System.NotImplementedException();
    }

    public bool AttemptQuestion(AnswerHandle handle)
    {
        // 回傳答案是正確還是錯誤
        throw new System.NotImplementedException();
    }

    public int GetStage(PlantEntryScriptableObject plant)
    {
        throw new System.NotImplementedException();
    }
}