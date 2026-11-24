# Everything else that came out of the binary

Two things that are decompiled output but do not belong in the compilable project tree.

## fody-generated/

`Costura/AssemblyLoader.cs` and `ProcessedByFody.cs` are written by Fody/Costura at build time into
`obj/`, not authored by hand. They are here for completeness only: adding them to the project would
collide with the copies Fody regenerates. `AssemblyLoader` is the runtime hook that inflates the embedded
DLLs; `ProcessedByFody` is a marker type that tells Costura the assembly has already been processed.

## BlackVerifyNumber/

Decompiled source of the `BlackVerifyNumber.dll` that was embedded inside `Black.exe`. It is **not** part of
this project: its assembly metadata carries `Copyright © BlackGriefing 2020` and describes it as a
"Number verification plugin for BlackGenerator v2". It was recovered because it was shipped inside the
binary, and it is kept outside `src/` so the built application still references it as an external DLL.
Treat the code as third-party, not as yours.

## manifest.txt

SHA-256 of every file in this tree that was produced by the decompiler, one per line as `<sha256>  <path>`.
Regenerate it from the repository root with `sha256sum -c recovered/manifest.txt` to confirm nothing has
been altered since recovery.
