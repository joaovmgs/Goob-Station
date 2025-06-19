using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;

namespace Content.Shared.Botany;

public sealed class SharedPlantDnaManipulator
{
    public const string SeedSlotName = "seedSlot";
}

[Serializable, NetSerializable]
public sealed class PlantDnaManipulatorBoundUserInterfaceState : BoundUserInterfaceState
{
    public readonly bool HasSeed;
    public readonly string? SeedName;
    public readonly Dictionary<string, float>? Attributes;

    public PlantDnaManipulatorBoundUserInterfaceState(bool hasSeed, string? seedName, Dictionary<string, float>? attributes)
    {
        HasSeed = hasSeed;
        SeedName = seedName;
        Attributes = attributes;
    }
}

[NetSerializable, Serializable]
public enum PlantDnaManipulatorUiKey : byte
{
    Key
}
