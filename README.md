
# About
This is a sample application for a payment system like Revolut, where customers can get an online prepaid account they can use to make and receive online payments.

I'm using this as a playground to learn about:
- Event Storming (as a requirements elicitation technique)
- Domain Driven Design (for using an ubiquitous language at all levels, e.g. requirements, implementation, testing, documentation, and for separating the domain logic from the rest of the code)
- Event Sourcing (for storing all changes as events for auditing purposes)
- Authentication & Authorization, both user to machine and machine to machine access.

# High level flows
- A customer registers using an email and password.
- The customer then goes through a verification process by providing personal details and a proof of address and identity (e.g. 2 separate images)
- An employee reviews the verification details and can enable the customer to use the service, or ask for more information.
- If approved, the customer can access the service, and can see the balance of their prepaid account to be 0.
- The customer can top-up their prepaid account using a debit/credit card.
- The customer can pay other customers who are using this service, up to the total amount in their prepaid account.

# Business process
Event Storming is being used to define the domain model for online payments using a prepaid account. 
The output has been captured in the [Business Process Modeling with Event Storming](DomainModel.pdf) document.

This model captures:
- the ubiquitous language for this domain model (in the form of events)
- the personas
- the user flows
- aggregates (derived from events and used for technical implementation)

# High level architecture
In the first version, customers use a web-based (MVC) application. In a future version, a mobile app (e.g. Android) will provide the same functionality.
Internal users will use a similar web-based (MVC) application to validate customers.

For identity, a separate MVC application is provided that allows both external (customers) and internal (employees) users to login. Customers can also register using an email address and password. Authorization will be based on the RBAC model, where customers have the "customer" role and internal users have the "employee" role.

The execution of commands results in events being created into an EventStore instance.
The events are projected to MongoDB, which provides the read-only side for the application.
This allows for an audit trail to be created for all changes in the system, and thus provides the ultimate auditing capabilities.

# Tooling

- C# Net Core 5.0 (backend, front-end)
- EventStoreDB for storing the events
- MongoDB for projections and read-only view of the data
- Eventuous (https://github.com/Eventuous/eventuous) which provides the Domain Driven Design capabilities with a very small footprint & Event Sourcing capabilities.
- IdentityServer4 for authentication and authorization (OpenID Connect and oAuth2 capabilities)

Note:
- to get the browser to recognize the self-signed certificate (and thus to be able to login using the Pay.Identity service), run this command: dotnet dev-certs https --trust
(https://www.thesslstore.com/blog/how-to-make-ssl-certificates-play-nice-with-asp-net-core/)



