# BlackSpammerXS — recovered source

Source tree of **BlackSpammerXS** (a.k.a. *Black*, *BlackSpammerXLite*), recovered by decompiling the shipped
`Black.exe`. The original sources were lost; this tree is a reconstruction, not a backup, so read
[RECOVERY.md](RECOVERY.md) before trusting any single line.

## What the binary was

| | |
|---|---|
| Kind | .NET Framework 4.7.2 WinForms application, x64 |
| Assembly / namespace | `BlackSpammerXS` |
| Original project folder | `source\repos\BlackSpammerXLite\BlackSpammerXLite` (leaked in the embedded PDB path) |
| Packing | **Costura.Fody 4.1.0** — every dependency DLL is embedded as a deflate-compressed resource; there is no external runtime to unpack |
| Obfuscation | None. A handful of helper classes (`SecurityMT`, `SECBOOL`, `SecLG`) look obfuscated but their logic is plain C# |

## Layout

```
src/BlackSpammerXLite/          the application project (37 classes, 23 form resource files)
  BlackSpammerXS.csproj         reconstructed project file
  Program.cs                    entry point
  *.cs                          forms, managers, websocket + audit + spam logic
  *.resx                        per-form resources, images included (base64 in the resx)
  Properties/                   AssemblyInfo + generated Resources/Settings designers
  app.ico                       application icon
lib/                            place the non-NuGet assemblies here (see DEPENDENCIES.md)
```

## Building

Requires Visual Studio 2022 (or MSBuild with the .NET Framework 4.7.2 targeting pack) on Windows.

1. Drop the four assemblies listed in [DEPENDENCIES.md](DEPENDENCIES.md) into `lib/`.
2. `msbuild src\BlackSpammerXLite\BlackSpammerXS.csproj -p:Configuration=Release -p:Platform=x64`
3. NuGet restores the open-source dependencies and Costura re-embeds them into a single exe.

The build is not verified to succeed yet: `lib/` is empty, because the commercial DLLs could not be
distributed with the recovery. Everything the compiler needs beyond those four assemblies is in this tree.
