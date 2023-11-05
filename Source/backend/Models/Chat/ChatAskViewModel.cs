using System.Collections.Generic;

namespace IASquad.Poc.AzureOpenAi.Models.Chat;

public class ChatAskViewModel
{
    public IEnumerable<string> SystemPrompts { get; set; }
    public string Question { get; set; }
}
