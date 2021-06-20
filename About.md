
# About
This is a sample application for a payment system like Revolut, where customers can get a money account on-line, can top-up their account, and then pay other people (e.g. friends, local businesses) instantly.

I'm using this as a playground to learn about:
- Event Storming technique for translating high level requirements into a model that is easy to understand, discuss with all stakeholders, implement and extend
- Domain Driven Design
- Event Sourcing
- Authentication & Authorization, both user to machine and machine to machine access.

# High level flows
- A customer registers using an email and password.
- The customer then goes through a verification process by providing personal details and a proof of address and identity (e.g. 2 separate images)
- An internal user reviews the verification details and can enable the customer to use the service, or ask for more information.
- If approved, the customer can access the service, and can see the balance of their money account to be 0.
- The customer can top-up their money account using a debit/credit card
- The customer can pay other customers who are using this service, up to the amount in their account.

# High level architecture
In the first version, customers use a web-based (MVC) application. In a future version, a mobile app (e.g. Android) will provide the same functionality.
Internal users will use a similar web-based (MVC) application to validate customers.

For identity, a separate MVC application is provided that allows both external (customers) and internal (employees) users to login. Customers can also register using an email address and password. Authorization will be based on the RBAC model, where customers have the "customer" role and internal users have the "employee" role.

The execution of commands results in events being created into an EventStore instance.
The events are projected to MongoDB, which provides the read-only (used by queries).
This allows for an audit trail to be created for all changes in the system, and thus provides the ultimate auditing capabilities.

# Tooling

- C# Net Core 5.0 (backend, front-end)
- EventStoreDB for storing the events
- MongoDB for projections
- Eventuous (https://github.com/Eventuous/eventuous) which provides the Domain Driven Design capabilities with a very small footprint.
- IdentityServer4 for creating my own authentication and authorization UI/API (OpenID Connect and oAuth2 capabilities)





