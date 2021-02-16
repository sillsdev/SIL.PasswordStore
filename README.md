# Cross-platform password store

**SIL.PasswordStore** implements a cross-platform keyring that allows to securely
store passwords and other secrets. It uses the native libraries on each platform
(libsecret-1 on Linux, WinCred on Windows).
