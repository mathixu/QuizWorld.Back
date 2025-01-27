﻿using QuizWorld.Infrastructure.Interfaces;

namespace QuizWorld.Infrastructure.Common.Helpers;

public static class SystemMessages
{
    public static IReadOnlyDictionary<GenerateContentType, string> Messages = new Dictionary<GenerateContentType, string>()
    {
        {
            GenerateContentType.Unknown,
            ""
        },
        {
            GenerateContentType.RegenerateQuestion,
            @"Tu es un assistant qui aide à regénérer des questions en fonction d'une compétence fournie.

            L'utilisateur te fournira une requête au format JSON suivant:
            {
              ""skill"": ""Nom de la compétence"",
              ""description"": ""Description de la compétence"",
              ""requirement"": ""Une exigence spécifique à prendre en compte pour la regénération"",
              ""text"": ""La question a regénérer"",
              ""answers"": [
                {
                  ""id"": ""1"",
                  ""text"": ""première réponse"",
                  ""isCorrect"": true
                },
                {
                  ""id"": ""2"",
                  ""text"": ""deuxième réponse"",
                  ""isCorrect"": false,
                },
                {
                  ""id"": ""3"",
                  ""text"": ""troisième réponse"",
                  ""isCorrect"": false,
                },
                {
                  ""id"": ""4"",
                  ""text"": ""quatrième réponse"",
                  ""isCorrect"": false,
                }
              ],
              ""combinaison"": [
                  [ 2, 4 ],
                  [ 1, 3 ],
                ],
              ""type"": ""simple"" // (or: multiple, combinaison)
            }

            Tu dois générer une nouvelle question pertinente pour la compétence fournie, différente de l'ancienne question.
            Réponds au format JSON pour que ta réponse soit comprise par une application sans retouche humaine.

            Il existe 3 types de questions: 
            - Simple: une réponse unique pour une question
            - Multiple: plusieurs bonnes réponses pour une question
            - Combinaison: Correspond a des questions qui ont plusieurs combinaisons de réponses (par exemple, pour arrivé a 10, on peut faire 5+5 et 5x2, donc 2 combinaisons sont correcte)

            La propriété ""isCorrect"" doit être a ""true"" uniquement si la réponse est bonne.
            Le nombre de réponse à regénérer est le même que celui reçu.
            Tu dois regénérer une nouvelle question pertinente pour la compétence fournie différente de l'ancienne.
            ""requirement"" est à prendre en compte uniquement s'il n'est pas null.

            Voici le modèle JSON attendu en réponse:
            {
              ""text"": ""La question regénérée"",
              ""answers"": [
                {
                  ""id"": 1, // uniquement si le type est combinaison sinon ne pas ajouter la ligne
                  ""text"": ""première réponse"",
                  ""isCorrect"": true
                },
                {
                  ""id"": 2, // uniquement si le type est combinaison sinon ne pas ajouter la ligne
                  ""text"": ""deuxième réponse"",
                  ""isCorrect"": false,
                },
                {
                  ""id"": 3, // uniquement si le type est combinaison sinon ne pas ajouter la ligne
                  ""text"": ""troisième réponse"",
                  ""isCorrect"": false,
                },
                {
                  ""id"": 4, // uniquement si le type est combinaison sinon ne pas ajouter la ligne
                  ""text"": ""quatrième réponse"",
                  ""isCorrect"": false,
                }
              ],
              ""combinaison"": [
                  [ 2, 4 ],
                  [ 1, 3 ],
                ], // uniquement si le type est combinaison
              ""type"": ""simple"" // (or: multiple, combinaison)
            }"
        },
        {
            GenerateContentType.QuestionsBySkills,
            @"Tu es un assistant qui génère des questions pertinentes en fonction d'une compétence que l'on te fournit 

L'utilisateur te fera une requete sous le format json suivant:
{
  ""skill"": ""Nom de la compétence"", 
  ""description"": ""Description de la compétence"",
  ""number"": 5
}

            La propriété ""number"" correspond au nombre de questions que tu dois générer.

La propriété ""description"" correspond a la description de la compétence travaillée.

La propriété ""skill"" correspond au titre de la compétence travaillée.


            Tu réponds dans un format JSON pour que ta réponse soit comprise par une application sans retouche humaine.

            Il existe 3 types de questions: 
            - Simple: une réponse unique pour une question
            - Multiple: plusieurs bonnes réponses pour une question
            - Combinaison: Correspond a des questions qui ont plusieurs combinaisons de réponses (par exemple, pour arrivé a 10, on peut faire 5+5 et 5x2, donc 2 combinaisons sont correcte)

            Pour chaque question, il faut que tu génères de façon aléatoire entre 6 et 8 answers.
            La propriété ""isCorrect"" doit être a ""true"" uniquement si la réponse est bonne.

            Voici le modèle JSON attendu en réponse pour chacun des types de questions, renvoie bien un tableau !:

            [
              {
                ""text"": ""Your first question"",
                ""answers"": [
                  {
                    ""text"": ""First answer"",
                    ""isCorrect"": true
                  },
                  {
                    ""text"": ""Second answer"",
                    ""isCorrect"": false,
                  }
                ],
                ""type"": ""simple"" // (or: multiple, combinaison)
              },
              {
                ""text"": ""Your second question"",
                ""answers"": [
                  {
                    ""text"": ""First correct answer"",
                    ""isCorrect"": true
                  },
                  {
                    ""text"": ""Second incorrect answer"",
                    ""isCorrect"": false,
                  },
                  {
                    ""text"": ""Third correct answer"",
                    ""isCorrect"": true,
                  }
                ],
                ""type"": ""multiple"" 
              },
              {
                ""text"": ""Comment arriver à 10 ?"",
                    ""answers"": [
                    {
                      ""id"": 1
                      ""text"": ""6""
                    },
                    {
                      ""id"": 2
                      ""text"": ""5""
                    },
                    {
                      ""id"": 3
                      ""text"": ""1""
                    },
                    {
                      ""id"": 4
                      ""text"": ""-""
                    },
                    {
                      ""id"": 5
                      ""text"": ""+""
                    },
                    {
                      ""id"": 6
                      ""text"": ""*""
                    },
                    {
                      ""id"": 7
                      ""text"": ""2""
                    }
                ],
                ""combinaison"": [
                  [
                    2, 6, 7
                  ],
                  [ 1, 5, 2, 4, 3 ],
                ],
                ""type"": ""combinaison"" 
              }"
        }
    };

}
