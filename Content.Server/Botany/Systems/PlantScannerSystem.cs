using System.Text;
using Content.Server.Botany.Components;
using Content.Shared.Botany;
using Content.Shared.DoAfter;
using Content.Shared.Popups;
using Robust.Server.GameObjects;
using Content.Shared.Interaction;

namespace Content.Server.Botany.Systems;

public sealed class PlantScannerSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<PlantScannerComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<PlantScannerComponent, PlantScannerDoAfterEvent>(OnDoAfter);
    }

    private void OnAfterInteract(Entity<PlantScannerComponent> uid, ref AfterInteractEvent args)
    {
        if (args.Target == null || !args.CanReach)
            return;

        if (!TryComp<PlantHolderComponent>(args.Target.Value, out var plantHolder) || plantHolder.Seed == null)
        {
            _popup.PopupClient(Loc.GetString("plant-scanner-no-plant"), args.User, args.User);
            return;
        }

        _doAfter.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, uid.Comp.ScanDelay, new PlantScannerDoAfterEvent(), uid, target: args.Target, used: uid.Owner)
        {
            BreakOnMove = true,
            NeedHand = true,
        });
    }

    private void OnDoAfter(Entity<PlantScannerComponent> uid, ref PlantScannerDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled || args.Target == null)
            return;

        if (!TryComp<PlantHolderComponent>(args.Target.Value, out var plantHolder) || plantHolder.Seed == null)
            return;

        var seed = plantHolder.Seed;
        var sb = new StringBuilder();
        var name = Loc.GetString(seed.DisplayName);
        sb.AppendLine(Loc.GetString("plant-scanner-plant-name", ("name", name)));

        var mutationNames = new HashSet<string>(plantHolder.ActiveMutations);
        foreach (var mut in seed.Mutations)
            mutationNames.Add(mut.Name);

        if (mutationNames.Count > 0)
        {
            sb.AppendLine(Loc.GetString("plant-scanner-mutations"));
            foreach (var name in mutationNames)
            {
                sb.Append(" - ").AppendLine(name);
            }
        }
        else
        {
            sb.AppendLine(Loc.GetString("plant-scanner-no-mutations"));
        }

        if (seed.Chemicals.Count > 0)
        {
            sb.AppendLine(Loc.GetString("plant-scanner-chemicals"));
            foreach (var chem in seed.Chemicals.Keys)
            {
                sb.Append(" - ").AppendLine(chem);
            }
        }
        else
        {
            sb.AppendLine(Loc.GetString("plant-scanner-no-chemicals"));
        }

        OpenUi(args.User, uid.Owner, sb.ToString());
        args.Handled = true;
    }

    private void OpenUi(EntityUid user, EntityUid scanner, string info)
    {
        if (!_ui.HasUi(scanner, PlantScannerUiKey.Key))
            return;

        _ui.OpenUi(scanner, PlantScannerUiKey.Key, user);
        _ui.ServerSendUiMessage(scanner, PlantScannerUiKey.Key, new PlantScannerDataMessage(info), user);
    }
}
