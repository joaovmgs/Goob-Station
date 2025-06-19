// SPDX-FileCopyrightText: 2025 Codex
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;
using Content.Shared.DoAfter;

namespace Content.Shared.Botany;

[Serializable, NetSerializable]
public enum PlantScannerUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class PlantScannerDataMessage(string info) : BoundUserInterfaceMessage
{
    public string Info = info;
}

[Serializable, NetSerializable]
public sealed partial class PlantScannerDoAfterEvent : SimpleDoAfterEvent
{
}
