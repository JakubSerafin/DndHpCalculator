# HP Calculation

## Context and Problem Statement
We need a method to calculate a character's current HP in our game. The method should allow for a history of HP modifications during a session and should be compatible with a RESTful API.

## Considered Options
1. Chain of HP Modifiers
2. RPC-style Operations on Current HP

## Decision Outcome
Chosen option: "Chain of HP Modifiers", because it allows for a history of HP modifications and is more compatible with a RESTful API.

### Pros of Chain of HP Modifiers
- History of HP modifications: This approach allows us to retain a history of HP modifications during a session. This can be useful for fixing or undoing mistakes by modifying or removing one of the previously added modifiers.
- RESTful compatibility: Each modification of HP is an object that can be created, read, updated, or deleted (CRUD operations), which aligns well with the principles of a RESTful API.

### Cons of Chain of HP Modifiers
- Complexity: This approach might be less intuitive to users who are used to commands rather than events.

### Pros of RPC-style Operations on Current HP
- Intuitiveness: This approach might be more intuitive to users who are used to commands rather than events.
- Simplicity: It introduces fewer entities to track, as it operates directly on the current HP.

### Cons of RPC-style Operations on Current HP
- Less RESTful compatibility: This approach is less compatible with a RESTful API, as it does not treat HP modifications as objects that can be CRUD-ed.
- Increased complexity in the business logic: It introduces more entities to track (max HP, current HP, and temp HP), which all need to be persisted in the character sheet domain model.

## Related ADRs
- [API Structure](ADRs/05_APIStructure.md)