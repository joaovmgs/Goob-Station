using Content.Shared.Botany;
using Robust.Client.UserInterface;

namespace Content.Client.Botany.PlantScanner.UI;

public sealed class PlantScannerBoundUserInterface : BoundUserInterface
{
    private PlantScannerWindow? _window;
    private string? _pendingInfo;

    public PlantScannerBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = new PlantScannerWindow();
        _window.OnClose += Close;
        _window.OpenCentered();
        if (_pendingInfo != null)
        {
            _window.Populate(_pendingInfo);
            _pendingInfo = null;
        }
    }

    protected override void ReceiveMessage(BoundUserInterfaceMessage message)
    {
        if (message is PlantScannerDataMessage data)
        {
            if (_window != null)
                _window.Populate(data.Info);
            else
                _pendingInfo = data.Info;
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
            return;
        _window?.Dispose();
    }
}
