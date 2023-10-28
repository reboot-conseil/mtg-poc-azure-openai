# Démo Azure OpenAI

## Objectives

Montrer comment utiliser Azure OpenAI en .NET avec une interface web pour intéragir avec.
Il faut utiliser le SDK officiel et faire un scénario de démo pour une présentation (session MTG, Let's Talk)
Bonus:
- Voir si on peut utiliser DALL-E
- Faire un bot qui répond à partir de document qu'on lui entre
- Voir si on peut entrainer un modèle sur Azure OpenAI avec nos propres documents
- Gérer le contexte de la conversation

Date Limite: décembre 2023

## Who

Yohann

## Did we reach the goal ?

## Conclusion


# Infos générales

Ce projet utilise les secrets de .NET.
Pour les mettre, faire un clic droit dans Visual Studio sur le projet `IASquad.Poc.AzureOpenAi` et cliquer sur __Manage User Secrets__
La structure a rentrer pour que le projet fonctionne est comme suit:
```json
{
  "AzureOpenAi": {
    "Url": "<OpenAI Url>",
    "Key": "<OpenAI Secret>"
  }
}
```

Les infos peuvent être trouvées sur l'instance Azure OpenAI dans la section `Keys and Endpoint`

On peut aussi mettre ces infos dans l'appsettings.json, cependant, ce faisant, les endpoints et clés seront alors public dans le repository.