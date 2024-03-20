# Project should use API Key for Authentication

## Context and Problem Statement
We need a way to authenticate users who are trying to access our API. The method should be simple to deploy and use.

## Considered Options
1. OAuth
2. API Key

## Decision Outcome
Chosen option: "API Key", because it is the simplest to deploy. OAuth, while a robust and secure option, requires setting up a provider which can be a complex process.

### Pros of API Key
- Simplicity: It is easy to implement and use. The client only needs to send the API Key in the header of the HTTP request.
- Deployment: It does not require any additional setup like OAuth does.

### Cons of API Key
- Not suitable for frontend scenarios: API Keys are typically used for server-to-server communication. They can be hard to manage securely in a client-side application, as they must be kept secret.
- Less secure: If the API Key is exposed, anyone can use it to access the API. Unlike OAuth tokens, API Keys do not expire.

## Related ADRs