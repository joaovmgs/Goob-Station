using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Content.Server.Botany.Systems;

namespace Content.Server.Botany.Components;

[RegisterComponent]
[Access(typeof(PlantScannerSystem))]
public sealed partial class PlantScannerComponent : Component
{
    [DataField]
    public TimeSpan ScanDelay = TimeSpan.FromSeconds(1.0);
}
