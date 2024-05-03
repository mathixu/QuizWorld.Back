### Table des Matières
1. [Introduction](#introduction)
2. [Configuration](#configuration)
   - [Variables d'environnement](#environment)
   - [Secrets](#secrets)
4. [Architecture de l'API](#architecture)
5. [Démarrage de l'API](#startup)
6. [Utilisation des Technologies](#technologies)
   - [AutoMapper](#automapper)
   - [FluentValidation](#fluentvalidation)
   - [MediatR](#mediatr)
   - [Contrôleurs](#controllers)
7. [Conclusion](#conclusion)

---

### 1. Introduction <a name="introduction"></a>
Ce document décrit la mise en place et la configuration de l'API pour le projet QuizWorld. Il couvre les aspects fondamentaux de l'architecture, les configurations nécessaires et la manière d'interagir avec les différentes couches et services utilisés.

### 2. Configuration <a name="configuration"></a>
#### Configuration des variables d'environnement <a name="environment"></a>
Pour configurer l'URL du Key Vault, vous devez définir la variable d'environnement `KEY_VAULT_URL`. 

- **Windows**
  ```cmd
  setx KEY_VAULT_URL "VotreURLDuKeyVault"
  ```

- **Linux/Mac**
  ```bash
  export KEY_VAULT_URL="VotreURLDuKeyVault"
  ```

#### Configuration des Secrets <a name="secrets"></a>

Cette API utilise une Azure Blob Storage ainsi qu'une base de donnée CosmosDb.

Vous trouverez le nom des secrets dans le fichier `.\src\QuizWorld.Application\Common\Helpers\Constants.cs`.

Configurez les secrets dans Azure Key Vault au format JSON suivant :

- **Blob Storage** :
  ```json
  {
    "ConnectionString": "votre_chaine_de_connexion",
    "ContainerName": "nom_du_conteneur"
  }
  ```

- **MongoDb** :
  ```json
  {
    "ConnectionString": "votre_chaine_de_connexion",
    "DatabaseName": "nom_de_la_base"
  }
  ```

#### Authentification Azure Key Vault
- **En local**: Utilisez Azure CLI pour vous authentifier avec `az login`. Ensuite, le `DefaultAzureCredential` gérera l'authentification automatiquement.
- **Azure Web App**: Configurez l'identité managée pour votre application et `DefaultAzureCredential` utilisera cette identité pour accéder au Key Vault.

### 3. Architecture de l'API <a name="architecture"></a>
L'API est structurée en quatre couches principales :
- **QuizWorld.Presentation** : Le projet de démarrage responsable de la présentation des données.
- **QuizWorld.Domain** : Contient la logique métier et les modèles de domaine.
- **QuizWorld.Infrastructure** : Gère la persistance des données et les interactions avec les services externes.
- **QuizWorld.Application** : Coordonne les opérations entre la présentation et l'infrastructure.

### 4. Démarrage de l'API <a name="startup"></a>
Pour démarrer l'API::
```bash
dotnet run --project .\src\QuizWorld.Presentation\
```
Pour accéder à la documentation des endpoints: http://localhost:5243/swagger/index.html

### 5. Utilisation des Technologies <a name="technologies"></a>

#### AutoMapper <a name="automapper"></a>
**AutoMapper** est une bibliothèque qui permet de mapper automatiquement les propriétés d'un objet à un autre, facilitant ainsi la transformation des données entre les couches de l'application.

Exemple de mapping :
```csharp
CreateMap<CreateQuizCommand, Quiz>()
    .ForMember(dest => dest.NameNormalized, opt => opt.MapFrom(src => src.Name.ToNormalizedFormat()))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.Pending))
    .ForMember(dest => dest.SkillWeights, opt => opt.Ignore())
    .ForMember(dest => dest.Attachment, opt => opt.MapFrom(src => src.HasFile ? new QuizFile { Status = QuizFileStatus.Uploading } : null));
```
Ce code configure le mapping pour la commande `CreateQuizCommand` vers l'entité `Quiz`, avec des règles spécifiques pour certaines propriétés.

#### FluentValidation <a name="fluentvalidation"></a>
**FluentValidation** est une bibliothèque pour .NET qui permet de construire des règles de validation fortes, expressives et faciles à lire. Elle est utilisée ici pour valider les commandes avant leur traitement. 

Exemple de règles de validation pour `CreateQuizCommand` :
```csharp
RuleFor(x => x.Name)
    .NotEmpty()
    .WithMessage("The name of the quiz is required.");

RuleFor(x => x.TotalQuestions)
    .GreaterThan(0)
    .WithMessage("The total number of questions must be greater than 0.");

RuleFor(x => x.SkillWeights)
    .NotEmpty()
    .WithMessage("At least one skill weight is required.")
    .Must(x => x.Values.Sum() == 100)
    .WithMessage("The sum of all skill weights must be 100.");
```

#### MediatR <a name="mediatr"></a>
**MediatR** facilite l'implémentation du design pattern basé sur les médiateurs et est souvent utilisé pour mettre en œuvre le principe de CQRS (Command Query Responsibility Segregation). 

**IPipelineBehavior** dans MediatR permet de définir des comportements qui sont exécutés avant ou après les gestionnaires de requête (comme un Middleware). Nous en utilisons 2 :
- **ExceptionHandlingBehavior**: Intercepte les exceptions pour uniformiser les réponses d'erreur.
- **ValidationBehavior**: Exécute les validations définies avec FluentValidation avant que la requête ne soit traitée par son gestionnaire.

##### IQuizWorldRequest
L'interface `IQuizWorldRequest<TResponse>` est une abstraction personnalisée qui étend `IRequest` de MediatR. Elle est paramétrée avec `QuizWorldResponse<TResponse>`, une enveloppe générique qui peut contenir n'importe quelle réponse avec un statut de réussite, un message d'erreur et un code d'état HTTP. Cette structuration permet de normaliser les réponses à travers toutes les commandes et requêtes traitées dans l'API.

Voici l'interface :
```csharp
public interface IQuizWorldRequest<TResponse> : IRequest<QuizWorldResponse<TResponse>> { }
```

##### Command
Une commande est une classe qui implémente `IQuizWorldRequest<Quiz>`. Cela indique que cette commande, lorsqu'elle est traitée, doit retourner une instance de `QuizWorldResponse<Quiz>`. La commande encapsule toutes les données nécessaires pour créer un quiz, agissant comme un DTO.

Voici la définition de la classe :
```csharp
public class CreateQuizCommand : IQuizWorldRequest<Quiz> { 
    // Propriétés du DTO
}
```

##### CommandHandler
Un command handler est le gestionnaire pour sa command correspondante et implémente `IRequestHandler<Command, QuizWorldResponse<Response>>`. Ce gestionnaire est responsable de traiter la commande, c'est-à-dire de prendre les données de la commande, les traiter, et enfin de retourner le résultat dans une `QuizWorldResponse<Response>`.

La définition du handler est la suivante :
```csharp
public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, QuizWorldResponse<Quiz>> {

    public async Task<QuizWorldResponse<Quiz>> Handle(CreateQuizCommand request, CancellationToken cancellationToken) {
        // Logique pour créer un quiz
    }
}
```

Ce pattern permet une séparation claire des responsabilités, où la **commande** définit les données nécessaires, le **command handler** effectue la logique métier, et **`IQuizWorldRequest<TResponse>`** assure que toutes les requêtes retournent des réponses standardisées. Cette structure facilite la maintenance et le développement futur de l'API.

#### Contrôleurs <a name="controllers"></a>
Tous les contrôleurs héritent de `BaseApiController`, qui encapsule l'utilisation de MediatR pour envoyer des requêtes et traiter les réponses. Les méthodes `HandleCommand` et `HandleResult` sont utilisées pour traiter les commandes et les réponses de manière standardisée :

```csharp
public abstract class BaseApiController : ControllerBase
{
    protected readonly ISender _sender;

    protected IActionResult HandleResult<TResponse>(QuizWorldResponse<TResponse> response)
    {
        if (response.IsSuccessful)
            return StatusCode(response.StatusCode, response.StatusCode != 204 ? response.Data : null);
        return StatusCode(response.StatusCode, new { message = response.ErrorMessage });
    }

    protected async Task<IActionResult> HandleCommand<TResponse>(IQuizWorldRequest<TResponse> request)
    {
        var response = await _sender.Send(request);
        return HandleResult(response);
    }
}
```
Ces abstractions aident à maintenir la cohérence et la réutilisabilité du code à travers tous les contrôleurs de l'API.

Exemple de mise en œuvre dans les contrôleurs :
```csharp
[HttpPost]
public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizCommand command)
    => await HandleCommand(command);
```

### 7. Conclusion <a name="conclusion"></a>
Cette documentation fournit les lignes directrices pour configurer, démarrer et utiliser l'API QuizWorld. Assurez-vous de suivre les étapes de configuration pour éviter les problèmes lors du déploiement et du développement.
