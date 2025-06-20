// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Audio;

namespace Content.Server.Botany.Components;

/// <summary>
/// Temporary plant analyzer component.
/// </summary>
[RegisterComponent]
public sealed partial class PlantAnalyzerComponent : Component
{
    [DataField("scanDelay")]
    public float ScanDelay = 1f;

    [DataField("scanSound")] 
    public SoundSpecifier? ScanSound = new SoundPathSpecifier("/Audio/Items/Medical/healthscanner.ogg");
}
