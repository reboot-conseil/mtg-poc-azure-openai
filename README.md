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

Le but a été atteint, on a utilisé le SDK d'Azure OpenAI et le service d'Azure pour créer un Chatbot. Le code est disponible dans les services du backend.
Le repository inclut le PowerPoint qui a été présenté à la session du 30 novembre au Microsoft Tech Group de Strasbourg.
La démo consiste à faire tourner les deux applications, s'assurer que le backend a bien accès à une instance d'Azure OpenAI.

Une piste possible d'amélioration est de tester si le SDK .NET d'Azure OpenAI est compatible avec la plateforme d'OpenAI

Aussi un Let's Talk public est prévu le 18 janvier 2024, reprennant la session qui a été faite au MTG.

Sur les 4 objectifs bonus, 3 ont été remplis, il n'est à priori pas possible d'entrainer un modèle de LLM avec notre base de connaissance, mais Azure prévoit un mécanisme similaire basé sur Azure AI Search (précédemment Azure Cognitives Search). La base de connaissance est ensuite déployé sur une instance du chat

## Conclusion

Date de réaction de la conclusion : 13/12/2023

Nous avons maintenant une bonne base de code pour intégrer Azure OpenAI dans une application en .NET et de quoi discuter de ce service d'Azure et de quoi montrer ce qu'on sait faire avec.

Le repo est public sur le GitHub de Reboot et il y a déjà 2 talks qui ont été planifiés / faits. 


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