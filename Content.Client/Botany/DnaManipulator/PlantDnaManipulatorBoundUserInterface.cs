using Content.Shared.Botany;
using Content.Shared.Containers.ItemSlots;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Botany.DnaManipulator;

[UsedImplicitly]
public sealed class PlantDnaManipulatorBoundUserInterface : BoundUserInterface
{
    private PlantDnaManipulatorWindow? _window;

    public PlantDnaManipulatorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = this.CreateWindow<PlantDnaManipulatorWindow>();
        _window.Title = EntMan.GetComponent<MetaDataComponent>(Owner).EntityName;
        _window.SeedEjectButton.OnPressed += _ => SendMessage(new ItemSlotButtonPressedEvent(SharedPlantDnaManipulator.SeedSlotName));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        _window?.UpdateState((PlantDnaManipulatorBoundUserInterfaceState)state);
    }
}
