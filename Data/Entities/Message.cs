namespace IASquad.Poc.AzureOpenAi.Data.Entities;

public enum MessageRole
{
    SYSTEM,
    USER,
    ASSISTANT
}

public class Message
{
    public int Id { get; set; }
    public string Value { get; set; }
    public MessageRole Role { get; set; }
}
