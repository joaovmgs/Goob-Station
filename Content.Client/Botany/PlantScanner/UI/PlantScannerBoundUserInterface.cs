using Content.Shared.Botany;
using Robust.Client.UserInterface;

namespace Content.Client.Botany.PlantScanner.UI;

public sealed class PlantScannerBoundUserInterface : BoundUserInterface
{
    private PlantScannerWindow? _window;

    public PlantScannerBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = new PlantScannerWindow();
        _window.OnClose += Close;
        _window.OpenCentered();
    }

    protected override void ReceiveMessage(BoundUserInterfaceMessage message)
    {
        if (_window == null)
            return;

        if (message is PlantScannerDataMessage data)
            _window.Populate(data.Info);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
            return;
        _window?.Dispose();
    }
}
