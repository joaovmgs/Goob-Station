using Content.Server.Botany.Components;
using Content.Server.Botany.DnaManipulator;
using Content.Shared.Botany;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Popups;
using Robust.Server.GameObjects;

namespace Content.Server.Botany.Systems;

public sealed class PlantDnaManipulatorSystem : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;
    [Dependency] private readonly BotanySystem _botanySystem = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlantDnaManipulatorComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<PlantDnaManipulatorComponent, BoundUIOpenedEvent>(OnUiOpened);
        SubscribeLocalEvent<PlantDnaManipulatorComponent, EntInsertedIntoContainerMessage>(OnContainerModified);
        SubscribeLocalEvent<PlantDnaManipulatorComponent, EntRemovedFromContainerMessage>(OnContainerModified);
    }

    private void OnStartup(Entity<PlantDnaManipulatorComponent> ent, ref ComponentStartup args)
    {
        _itemSlots.AddItemSlot(ent.Owner, SharedPlantDnaManipulator.SeedSlotName, ent.Comp.SeedSlot);
    }

    private void OnUiOpened(Entity<PlantDnaManipulatorComponent> ent, ref BoundUIOpenedEvent args)
    {
        UpdateUi(ent);
    }

    private void OnContainerModified(Entity<PlantDnaManipulatorComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        UpdateUi(ent);
    }

    private void OnContainerModified(Entity<PlantDnaManipulatorComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        UpdateUi(ent);
    }

    private void UpdateUi(Entity<PlantDnaManipulatorComponent> ent)
    {
        var (uid, comp) = ent;
        var seed = _itemSlots.GetItemOrNull(uid, SharedPlantDnaManipulator.SeedSlotName, comp);
        var hasSeed = seed != null;
        string? name = null;
        Dictionary<string, float>? attributes = null;
        if (hasSeed && TryComp<SeedComponent>(seed.Value, out var seedComp) && _botanySystem.TryGetSeed(seedComp, out var seedData))
        {
            name = Loc.GetString(seedData.Name);
            attributes = new Dictionary<string, float>
            {
                {"Potency", seedData.Potency},
                {"Yield", seedData.Yield}
            };
        }

        var state = new PlantDnaManipulatorBoundUserInterfaceState(hasSeed, name, attributes);
        _ui.SetUiState(uid, PlantDnaManipulatorUiKey.Key, state);
    }
}
