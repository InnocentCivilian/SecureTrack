# SecureTrack:Data Integrity and Change Data Capture Library

## Project Overview
This C# library provides mechanisms to maintain data integrity and implement change data capture (CDC). It employs digest-based integrity checks and a CDC system to ensure data authenticity and track changes to records. The library is designed to detect unauthorized modifications and maintain a comprehensive audit trail.

## Features
### 1. Digest-Based Data Integrity
- **Row Digest**: For each record in a table/collection, create a unique digest using a combination of the record's data and a secret salt. This salt is a unique secret known only to the application and serves as a critical component in calculating the digest. The digest is stored alongside the data in the table, typically in a separate column designated for this purpose.The digest can either be resulf of a digesting function or an asymetric encryption algorithm signing result
  
- **Integrity Validation**: The integrity of the record is checked by recalculating the digest when the record is accessed or modified. If the calculated digest does not match the stored digest, this indicates possible tampering or unauthorized modifications. When a mismatch occurs, the system can throw a custom exception, allowing you to handle this scenario in your application logic.

### 2. Change Data Capture (CDC)
- **CDC Table**: A dedicated table captures information about all changes to records. This table typically contains fields like record ID, timestamp, old and new values, user information, and other metadata useful for auditing and tracking changes. By logging these changes, you can maintain a comprehensive history of all modifications to your records.

- **Event-Based Triggers**: SecureTrack is able to capture changes as they occur. This approach ensures that every insertion, update, or deletion is recorded in the CDC table, allowing for a thorough audit trail. The captured data can be used for auditing, debugging, or compliance purposes.

### 3. Exception Handling
- **Data Tampering Exception**: A custom exception is thrown when data integrity checks fail, signaling that the record may have been modified outside the expected application workflow. This exception allows you to define how the application should respond when data integrity is compromised.

- **Logging**: The library includes a logging mechanism that records detailed information when exceptions occur. This is useful for troubleshooting, audit purposes, or investigating potential data integrity issues.

### 4. Secure Configuration
- **Secure Salt Storage**: Since the secret salt/privatekey is crucial to the digesting process, it must be stored securely. Consider using a secure configuration management system that restricts access to authorized personnel. This helps ensure that the secret remains confidential and cannot be easily compromised.

## Service Registration (Using Fluent Builder)

To set up the SecureTrack data integrity system, use the fluent builder provided.

### Basic Setup

```csharp
services.AddDataIntegrityService<MyEntity>()
    .UseSha512()
    .UseJsonSerialization();

services.AddDataIntegrityService()
    .UseSha256()
    .UseRawStringSerialization();
```

## 🚀 SecureTrack: Completion Checklist

This checklist tracks both completed and remaining work to bring SecureTrack to full release.



### 🔄 Core Features

- [x] Digest-based integrity system with pluggable hashing (`SHA256`, `SHA512`)
- [x] Pluggable serialization strategies (`JSON`, `RawString`)
- [x] Generic and non-generic `IDataIntegrityService` interfaces
- [x] Fluent DI registration with `DataIntegrityServiceBuilder`
- [x] Custom exception handling (`IntegrityViolationException`)
- [x] Logging integration via `ILogger`
- [x] Secure salt usage (passed in methods)

- [ ] **Implement Change Data Capture (CDC)**
  - [ ] Define `CdcEntry` model (record ID, operation type, old/new values, timestamp, user)
  - [ ] Create `ICdcRepository` interface for persisting CDC logs
  - [ ] Provide default repository implementation (e.g., in-memory, EF Core)
  - [ ] Hook CDC logging into `DataIntegrityService` after integrity operations

- [ ] **Add Event-Based Triggers**
  - [ ] Design `IDataChangeNotifier` or interceptor interface
  - [ ] Provide decorators/interceptors for insert, update, delete events
  - [ ] Ensure CDC system is triggered automatically on changes



### 🔒 Security & Configuration

- [ ] **Secure Salt/Key Storage**
  - [ ] Create `ISecretProvider` abstraction for secure salt/key retrieval
  - [ ] Provide default implementation using `IConfiguration`
  - [ ] Allow extension to external secret managers (e.g., Azure, AWS)



### 🛡️ Logging & Exception Handling

- [x] Basic logging via `ILogger`
- [ ] Add structured logging with rich context (record ID, type, operation, failure reason)
- [ ] Integrate with structured log sinks (e.g., Serilog)
- [ ] Create middleware or filters for global integrity exception capture
- [ ] Provide hooks or callbacks for custom alerting/auditing



### 🧪 Testing

- [ ] Add unit tests:
  - [ ] Hashing algorithms
  - [ ] Serialization strategies
  - [ ] Data integrity service methods

- [ ] Add integration tests:
  - [ ] Full integrity + CDC flow
  - [ ] Edge cases (tampering, mismatched digests)

- [ ] Add performance/load tests:
  - [ ] High-volume change tracking
  - [ ] Large record handling


### 📦 Packaging & Release

- [ ] **GitHub Actions Build**
  - [ ] Add CI workflow (build, test, validate) on push/pull to `main`/`dev`

- [ ] **NuGet Packaging**
  - [ ] Configure `.csproj` or `.nuspec` for NuGet metadata
  - [ ] Set up GitHub Actions release workflow
  - [ ] Automate NuGet publish on tag or release event



### 📖 Documentation & Examples

- [x] Service registration examples (`AddDataIntegrityService<T>`, `AddDataIntegrityService()`)
- [ ] Expand README:
  - [ ] Code samples for digest + validation
  - [ ] CDC usage examples
  - [ ] Salt/key handling explanation
  - [ ] Custom implementation guides

- [ ] Provide demo app:
  - [ ] Example project showing SecureTrack in action


