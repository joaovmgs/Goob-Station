// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Popups;
using Content.Server.Botany.Components;
using Content.Shared.Interaction;
using Content.Shared.DoAfter;
using Content.Shared.Interaction.Events;
using Content.Shared.PlantAnalyzer;
using Robust.Shared.Audio.Systems;

namespace Content.Server.Botany.Systems;

public sealed class PlantAnalyzerSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<PlantAnalyzerComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<PlantAnalyzerComponent, PlantAnalyzerDoAfterEvent>(OnDoAfter);
    }

    private void OnAfterInteract(EntityUid uid, PlantAnalyzerComponent component, ref AfterInteractEvent args)
    {
        if (args.Target == null || !args.CanReach)
            return;

        var doAfter = new DoAfterArgs(EntityManager, args.User, component.ScanDelay, new PlantAnalyzerDoAfterEvent(), uid, target: args.Target, used: uid)
        {
            BreakOnMove = true,
            NeedHand = true,
            Broadcast = true
        };

        _doAfter.TryStartDoAfter(doAfter);
    }

    private void OnDoAfter(EntityUid uid, PlantAnalyzerComponent component, DoAfterEvent args)
    {
        if (args.Cancelled || args.Handled)
            return;

        if (component.ScanSound != null)
            _audio.PlayPvs(component.ScanSound, uid);

        _popup.PopupEntity(Loc.GetString("plant-analyzer-malfunction"), args.Args.User, args.Args.User);
        args.Handled = true;
    }
}
