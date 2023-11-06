namespace IASquad.Poc.AzureOpenAi.Data.Entities;

public static class MessageRole
{
    public const int SYSTEM = 0;
    public const int USER = 1;
    public const int ASSISTANT = 2;
}

public class Message
{
    public string Value { get; set; }
    public int Role { get; set; }
}
