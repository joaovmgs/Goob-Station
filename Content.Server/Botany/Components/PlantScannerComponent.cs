// SPDX-FileCopyrightText: 2025 Codex
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Botany.Systems;
using System;

namespace Content.Server.Botany.Components;

[RegisterComponent]
[Access(typeof(PlantScannerSystem))]
public sealed partial class PlantScannerComponent : Component
{
    [DataField]
    public TimeSpan ScanDelay = TimeSpan.FromSeconds(1.0);
}
