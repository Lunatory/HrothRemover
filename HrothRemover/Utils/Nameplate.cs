using Dalamud.Game.Gui.NamePlate;

namespace HrothRemover.Utils
{
    internal class Nameplate
    {
        public Nameplate()
        {
            Service.namePlateGui.OnNamePlateUpdate += (context, handlers) =>
            {
                if (!Service.configuration.enabled || !Service.configuration.nameHQ)
                    return;

                foreach (var handler in handlers)
                {
                    if (handler.NamePlateKind == NamePlateKind.PlayerCharacter)
                    {
                        unsafe
                        {
                            if (handler.PlayerCharacter == null) return;

                            // if native hrothgar
                            if (Drawer.NonNativeID.Contains(handler.PlayerCharacter.Name.TextValue))
                            {
                                // Plugin.OutputChatLine($"Adding  to {handler.PlayerCharacter.Name.TextValue}");
                                handler.NameParts.Text = $"{handler.Name} ";
                            }
                        }
                    }
                }
            };
        }

        public void Dispose() { }
    }
}
