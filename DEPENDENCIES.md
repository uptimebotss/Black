# Dependencies

Read out of the Costura manifest of the original `Black.exe` and the file versions of the embedded DLLs.

## Not on NuGet — put the DLL in `lib/`

| Assembly | Version | Note |
|---|---|---|
| `Siticone.UI.dll` | 1.0.0.6 | commercial UI kit, vendor-installed |
| `Bunifu_UI_v1.52.dll` | 1.3.0.0 | commercial UI kit |
| `WindowsInput.dll` | 1.0.4.0 | keyboard/mouse simulation (`InputSimulator`) |
| `BlackVerifyNumber.dll` | 1.0.0.0 | phone-verification plugin; assembly copyright says *BlackGriefing 2020* |

## NuGet

| Package | Version |
|---|---|
| `Newtonsoft.Json` | 12.0.3 |
| `System.Reactive` | 4.3.2 |
| `System.Interactive.Async` | 3.1.1 |
| `Websocket.Client` | 4.3.0 |
| `BrotliSharpLib` | 0.3.3 |
| `System.Threading.Channels` | 4.7.0 |
| `Microsoft.Extensions.DependencyInjection` | 1.1.1 |
| `Costura.Fody` | 4.1.0 |
| `Fody` | 6.8.1 |

`System.Collections.Immutable`, `System.Diagnostics.DiagnosticSource`, `System.Runtime.CompilerServices.Unsafe`,
`System.Threading.Tasks.Extensions` and `System.ValueTuple` were embedded too, but they arrive transitively.
