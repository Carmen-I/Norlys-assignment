# 📌 Norlys Assignment — Backend API (C# / .NET)

This project implements a small backend system for managing persons and their offices.
It focuses on clean architecture, separation of concerns, and scalable design with room for future expansion.

## 🏗️ Architecture Overview

The solution is structured in three layers:

1️⃣ API Layer

ASP.NET Core Web API controllers (REST)

Uses DTOs + manual mapping to protect domain models

Global Exception Middleware for consistent error responses

Logging through ILogger

2️⃣ Business Logic Layer

Encapsulates validation and business rules:

Person data validation (name, age rules)

Office rules (checking max capacity before assigning)

Existence checks

No dependency on external libraries or database framework

3️⃣ Data Access Layer

Repository using Dapper

Fully async database operations

Only SQL logic in this layer (separation of concerns)

This architecture allows the system to evolve without affecting higher layers.

## 🧱 Design Principles (SOLID)
✔ Single Responsibility Principle

Each layer and class has one responsibility:

Controllers → HTTP & routing

Business Logic → validation + rules

Repository → data access

DTOs → request/response models

Converters → mapping only

This prevents mixed responsibilities and makes testing easier.

✔ Open/Closed Principle

All repositories (IPersonRepository, IOfficeRepository) are abstractions.
The system can be extended by adding new implementations:

PersonRepositorySqlServer

PersonRepositoryPostgres

PersonRepositoryInMemory (for testing)

Business logic and controllers never change — they depend only on interfaces.
This is a clean application of OCP.

✔ Liskov Substitution Principle

Any repository implementation can replace another as long as it implements the same interface.
Business logic does not need to know what database engine is used.

✔ Interface Segregation Principle

Small, focused interfaces:

IPersonRepository

IOfficeRepository

Each exposes only what is needed.
No “fat” interfaces that force unused methods.

✔ Dependency Inversion Principle

High-level modules (API, Business Logic) depend on abstractions, not concrete classes.

Registered via DI:

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
builder.Services.AddScoped<IPersonLogic, PersonLogic>();


This allows:

easier testing (Moq)

flexible data storage choice

cleaner architecture

##  🧪 Testing Approach
Unit Tests (Moq)

Business logic is fully tested using mocked repositories:

Valid person creation

Validation failures

Office full rule

Updating person

Deleting person
Ensures rules behave consistently without a real DB.

Integration Tests (WebApplicationFactory)

Tests API endpoints.
Covers:

POST create person

PUT update person/ maybe in future would be a great idea to add pr replace it with a PATCH that modifies just the offiec(the only logic is to move a person from an office to another) 

DELETE person

GET endpoints return proper status codes
Ensures transport layer + business layer work correctly together.

## 🧰 Technologies Used

C# / .NET 8

ASP.NET Core Web API

Dapper (lightweight ORM)

SQL Server

Dependency Injection (built-in DI)

xUnit + Moq

Swagger for endpoint exploration

Middleware for global exception handling

Docker support (API can run in container)

The goal was to use technologies that fit a clean and maintainable architecture.

## 🚀 Possible Improvements

These features could extend the project:

1. Authentication / Authorization

Implement:

API Key (simple admin-only API)

or JWT Authentication

Controllers would receive:
 
[Authorize]

2. Office Concurrency with rowversion and an extra column named TotalEmployers or something.

Add RowVersion to Office and implement optimistic concurrency to prevent:

two people being assigned to the last available seat at the same time
