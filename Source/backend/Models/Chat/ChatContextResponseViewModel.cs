using IASquad.Poc.AzureOpenAi.Data.Entities;
using System.Collections.Generic;

namespace IASquad.Poc.AzureOpenAi.Models.Chat;

public class ChatContextResponseViewModel
{
    public IEnumerable<string> SystemPrompts { get; set; }

    public IEnumerable<Message> Messages { get; set; }
}
