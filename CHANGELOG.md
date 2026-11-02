# Release notes

## v3.1.0

Platform 2026.7. Minor: additive, no public member removed or renamed. What the SDK's procedures need from the task
engine: plain hint and feedback texts on a task node, a hook that decides whether a task in its status advances the
flow (`AdvancesFlow`), the step setter's skip state, and an opt-out from `TaskReactor`. "Start from step N" and the
networked late-join now complete the steps before it, and the networked sample reads its room key right on a rejoin.

### Added
- **`TaskSystem.AdvancesFlow(TaskNode)`** (protected virtual): whether a task, in its status, unlocks the next task and
  counts as done for the tasks depending on it. Default: Completed only, as before; a subclass can let a failed task
  advance the flow.
- **`TaskNode.Hint`, `RightFeedback`, `WrongFeedback`**: plain texts beside the description, for procedures. Graphs
  saved before they existed load them empty.
- **`TaskStepSetter.IsSkipping`**: true while a step setter completes the tasks before the step it starts from, so a
  procedure can leave them out of the score.
- **`ITaskReactorExempt`**: a component that implements it is not enabled and disabled by a `TaskReactor` on its object.

### Fixed
- **Networked sample `TasksRPCManager`: the room property key is built once** (`tid` + view id). `OnJoinedRoom` runs
  again for every `AddJoinRoomEvent` and appended the view id each time, so a rejoin read the wrong key. The normal
  flow no longer logs errors ("Exist", "Not Exist, add it"). Same fix in the copy imported under
  `Assets/Samples` of a project.
- **"Start from step N" and the networked late-join now complete the steps before it.**
  `TaskStepSetter.CompleteTaskRecursive` looked for the next dependency with `Previous == null`,
  which is never true (`Previous` is a list): no dependency was ever completed. It now checks
  `Previous.Count == 0`, as `TaskSystem` does.
- `RaycastDetector` raises `onRaycastExit` as soon as the ray stops hitting a target, also when it
  hits something else (it only did when it hit nothing at all). The cast no longer allocates every
  frame, and a missing `castPivot` (no main camera yet) no longer throws.
- `DetectorsSystem` returns after destroying a duplicate, so a second instance no longer registers
  every detector in the scene again.
- The graph panel no longer adds a status listener to a task node on every redraw in Play mode.
- `TaskUIPanel` and `TaskUIElement` no longer reference assets the package never shipped (two TMP fonts and
  TMP Essentials' LiberationSans, the Trapezoid and BGImageGradient sprites, the two video render textures): 24
  missing references in every project that installs the package. They are empty, which is what a missing reference
  already was at runtime: the texts take TMP's default font, the images draw plain, `TasksUIVideoController` makes
  its render texture as before.

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
- The "XR Tasks sample scene" sample referenced the graph types as `Virtuademy.SDK.Graphs`, an
  assembly that no longer exists: every graph field deserialized to null. The 204 references now
  name `SPACS.Graphs`. The "XR Tasks" asmdef no longer references `Reflectis.SDK.Utilities`,
  deleted in 2025, by GUID.

### Removed
- The "XR Tasks" and "XR Tasks sample scene" samples are no longer listed in `package.json`. They
  need `Virtuademy.SDK.XRKit` and Autohand, which exist only in the Virtuademy application, so they
  could not compile in any other project; the scene also carried six components of a script deleted
  in 2022. The files stay under `Samples~/` until the samples are rewritten on the XR Interaction
  Toolkit alone.

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
