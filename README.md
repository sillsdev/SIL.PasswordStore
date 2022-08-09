# Cross-platform password store

**SIL.PasswordStore** implements a cross-platform keyring for Windows and Linux
that allows to securely store passwords and other secrets. It uses the native libraries
on each platform (libsecret-1 on Linux, WinCred on Windows).

[![NuGet version (SIL.PasswordStore)](https://img.shields.io/nuget/v/SIL.PasswordStore.svg?style=flat-square)](https://www.nuget.org/packages/SIL.PasswordStore/)
[![Build, Test and Pack](https://github.com/sillsdev/SIL.PasswordStore/actions/workflows/CI-CD.yml/badge.svg)](https://github.com/sillsdev/SIL.PasswordStore/actions/workflows/CI-CD.yml)

## When to use

- Do **NOT** use this library if you're implementing a service and need to authenticate your users.
  Instead store a salt and a hash of the password.
- However, if you're implementing a tool that makes use of other services like GitHub or NuGet and you
  want to cache the password or token so that the user doesn't have to type the password each time
  he uses the tool, then this library might be for you.

## Usage

Very easy. `service` should identify the application, then simply call one of the
methods:

```csharp
PasswordStore.SetPassword(service, user, password);
password = PasswordStore.GetPassword(service, user);
PasswordStore.DeletePassword(service, user);
```

## Similar projects

There are few other projects that implement cross-platform keyring/password storage. If you know of others
let me know.

- [go-keyring](https://github.com/zalando/go-keyring) - similar project which gave the inspiration to this
  implementation. Implemented in Go.
