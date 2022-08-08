# Cross-platform password store

**SIL.PasswordStore** implements a cross-platform keyring that allows to securely
store passwords and other secrets. It uses the native libraries on each platform
(libsecret-1 on Linux, WinCred on Windows).

[![Build, Test and Pack](https://github.com/sillsdev/SIL.PasswordStore/actions/workflows/CI-CD.yml/badge.svg)](https://github.com/sillsdev/SIL.PasswordStore/actions/workflows/CI-CD.yml)

## Installation

Get the [nuget](https://www.nuget.org/packages/SIL.PasswordStore/) package.

## Usage

Very easy. `service` should identify the application, then simply call one of the
methods:

```csharp
PasswordStore.SetPassword(service, user, password);
password = PasswordStore.GetPassword(service, user);
PasswordStore.DeletePassword(service, user);
```
