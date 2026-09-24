# Release notes

## v3.0.0

### Changed
- **The package is `SPACS-Tasks`, id `com.anotherealitysrl.spacs-tasks`** (was `Virtuademy-SDK-Tasks` /
  `com.anotherealitysrl.virtuademy-sdk-tasks`). The `Virtuademy-SDK-*` prefix is kept for the SDK
  proper — Core, Environments, Library; a package that carries no platform takes the `SPACS-*`
  prefix, as SPACS-Utility did. The assemblies and namespaces were already `SPACS.Tasks*` and are
  unchanged, so built bundles, Visual Scripting graphs and interpreted scripts are unaffected.
- Depends on `com.anotherealitysrl.spacs-graphs` 3.0.0.

### Breaking
- A project that names `com.anotherealitysrl.virtuademy-sdk-tasks` in its `manifest.json` stops
  resolving once it pulls this version: the manifest key must match the id in `package.json`.
  Switch the key to `com.anotherealitysrl.spacs-tasks` and the URL to `SPACS-Tasks.git` (the package
  rename migrator in Virtuademy-SDK-Environments does both). Registry releases up to 2026.5.0 are
  unaffected: each pins the old repository URL at a tag, GitHub redirects that URL, and the tag
  still carries the id it was released with.

### Fixed
- **`WorldSpaceTasksCanvas` no longer carries a missing script in a creator project.** Its `Billboard`
  lived in the application, which a creator project does not have; it now lives in `SPACS-Utility`
  (same GUID), which this package declares as a dependency.

## v2.1.0

### Added
- `startImmediately` option in the task system.
- Logic to add items to the task reactor.

### Fixed
- Null check in `TaskUIManager`.
- Guard against null `RawImage` in `TaskUIVideoController`.
- Fixed handling of the last task in the ask system.

## v2.0.0

### Changed

- Changed package name, from Virtuademy-PLG-Tasks to Reflecits-SDK-Tasks, and updated namespaces according to new package name.
- Replace `Pointer_stringify` with `UTF8ToString` in jslib utilities.
- Added `isMultiplayer` parameter in `PingMyOnlinePresence` method in `IClientModelSystem`.
- Changed type parameter in `UpdateSavedAssets` to string.
- Removed API utilities (`ApiResponse`, `JsonArrayHelper`). They are moved to their own HTTP module.

### Removed

- Removed dependecies to spawned object from `IClientModelSystem`.

### Added

- Added max fov parameter to spawn position data in `SpawnData`.
- Implemented web socket listeners. Added callbacks to `IWebSocketSystem`.

## v1.2.0

### Added

- Add Revert function to the `TaskSystem` used to reset the tasks

### Fixed

- Fixed `ITasksRPCManager` to handle networked tasks

## v1.1.0

### Changed

- Update `TaskStepSetter` to check for null references and to support networking in Reflectis
- Update `ITasksRPCManager` adding events for when a room is joined and on its initialization

## v1.0.0

- Initial release
