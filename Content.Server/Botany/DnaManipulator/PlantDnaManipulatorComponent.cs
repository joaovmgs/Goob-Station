using Content.Server.Botany.Systems;
using Content.Shared.Botany;
using Content.Shared.Containers.ItemSlots;

namespace Content.Server.Botany.DnaManipulator;

[RegisterComponent]
[Access(typeof(PlantDnaManipulatorSystem))]
public sealed partial class PlantDnaManipulatorComponent : Component
{
    [DataField]
    public ItemSlot SeedSlot = new();
}
