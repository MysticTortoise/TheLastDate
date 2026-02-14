
[System.Serializable]
public class DialogueMessage
{
    public string Message;
    public string SpeakerTitle;

    public int GetLength()
    {
        return Message.Length;
    }
}
