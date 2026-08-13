# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

<!-- Available types of changes:
### Added
### Changed
### Fixed
### Deprecated
### Removed
### Security
-->

## [Unreleased]

### Removed

- removed net461 and net6.0 build targets

### Added

- Initial release
- build for .NET 10.0 and net462

### Changed

- on Windows passwords are now stored without terminating nulls.
  Earlier versions of the library wrote the terminating null plus
  an incorrect blob length.
- on Windows an empty password now returns an empty string instead
  of `null` (as it did previously). This now matches the behavior
  on the other platforms. `null` is returned if the credentials
  can't be found.
