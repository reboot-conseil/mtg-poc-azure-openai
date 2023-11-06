using IASquad.Poc.AzureOpenAi.Data.Entities;
using System.Collections.Generic;

namespace IASquad.Poc.AzureOpenAi.Models.Chat;

public class ChatContextViewModel
{
    public List<Message> Messages { get; set; }
}
