# Hms.API.Tests — Controller Unit Tests

## Overview

| Controller              | Tests | Positive | Negative |
|-------------------------|-------|----------|----------|
| AuthController          |   8   |    4     |    4     |
| AppointmentController   |   8   |    4     |    4     |
| MedicationController    |   8   |    4     |    4     |
| NurseController         |   8   |    4     |    4     |
| PatientController       |   8   |    4     |    4     |
| PhysicianController     |   8   |    4     |    4     |
| PrescriptionController  |   8   |    4     |    4     |
| ProcedureController     |   8   |    4     |    4     |
| RoomController          |   8   |    4     |    4     |
| StayController          |   8   |    4     |    4     |
| **TOTAL**               | **80**| **40**   | **40**   |

---

## Folder Structure

```
Hospital_Management/
├── API/
│   └── Hms.API/            ← your existing project
└── Tests/
    └── Hms.API.Tests/      ← this test project
        ├── Hms.API.Tests.csproj
        └── Controllers/
            ├── AuthControllerTests.cs
            ├── AppointmentControllerTests.cs
            ├── MedicationControllerTests.cs
            ├── NurseControllerTests.cs
            ├── PatientControllerTests.cs
            ├── PhysicianControllerTests.cs
            ├── PrescriptionControllerTests.cs
            ├── ProcedureControllerTests.cs
            ├── RoomControllerTests.cs
            └── StayControllerTests.cs
```

---

## Step 1 — Create the test project folder

```bash
# inside your solution root (where Hms.API folder lives)
mkdir -p Tests/Hms.API.Tests/Controllers
```

Copy all files into that folder.

---

## Step 2 — Install NuGet packages

```bash
cd Tests/Hms.API.Tests

dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Moq
dotnet add package FluentAssertions
```

---

## Step 3 — Add project reference

```bash
dotnet add reference ../../API/Hms.API/Hms.API.csproj
```

> Adjust the relative path if your folder layout is different.

---

## Step 4 — Run the tests

```bash
dotnet test
```

To see detailed output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

To run tests for a single controller:
```bash
dotnet test --filter "FullyQualifiedName~AuthControllerTests"
```

---

## How the tests work

### Tools used

| Tool | Purpose |
|------|---------|
| **xUnit** | Test framework — `[Fact]` attributes mark each test |
| **Moq** | Creates a fake (mock) service so real DB is never needed |
| **FluentAssertions** | Readable assertions — `result.Should().Be(...)` |

### Key pattern

```
Controller = REAL (what we test)
Service    = FAKE (Mock<IXxxService>)
```

```csharp
// 1. Create fake service
_mockService = new Mock<IAuthService>();

// 2. Inject it into the real controller
_controller = new AuthController(_mockService.Object);

// 3. Tell the fake what to return
_mockService.Setup(s => s.LoginAsync(request)).ReturnsAsync(response);

// 4. Call the controller directly
var result = await _controller.Login(request);

// 5. Assert the HTTP result
result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
```

### Positive vs Negative

| Type | What it verifies |
|------|-----------------|
| **Positive** | Happy path — service returns data, controller returns correct 200/201/204 |
| **Negative** | Sad path — service throws exception OR controller guard fires (invalid ID, bad date range), resulting in correct error type or status code |

### Two negative patterns in this project

**Pattern A — Controller catches internally (Appointment, Nurse):**
Service throws `NotFoundException` → controller catches it → returns `404 NotFoundObjectResult`.
Test asserts on the HTTP status code.

**Pattern B — Controller lets exceptions bubble (all others):**
Service throws `NotFoundException` / `ConflictException` / etc. → exception propagates to
`ExceptionMiddleware`.  Test asserts `ThrowAsync<TException>()`.
