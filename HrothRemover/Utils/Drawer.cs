using FFXIVClientStructs.FFXIV.Client.Game.Object;
using Penumbra.Api.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static HrothRemover.Utils.Constant;

namespace HrothRemover.Utils
{
    internal class Drawer : IDisposable
    {
        public static HashSet<string> NonNativeID = [];

        public Drawer()
        {
            Service.configWindow.OnConfigChanged += RefreshAllPlayers;
            if (Service.configuration.enabled)
            {
                Plugin.OutputChatLine("HrothRemoverstarting...");
                RefreshAllPlayers();
            }
        }

        private static void RefreshAllPlayers()
        {
            NonNativeID.Clear();
            Plugin.OutputChatLine("Refreshing all players");
            Service.penumbraApi.RedrawAll(RedrawType.Redraw);
            Service.namePlateGui.RequestRedraw();
        }

        public static unsafe void OnCreatingCharacterBase(nint gameObjectAddress, Guid _1, nint _2, nint customizePtr, nint _3)
        {
            if (!Service.configuration.enabled) return;

            // return if not player character
            var gameObj = (GameObject*)gameObjectAddress;
            //if (gameObj->ObjectKind != ObjectKind.Pc) return;

            var customData = Marshal.PtrToStructure<Constant.CharaCustomizeData>(customizePtr);
            if (customData.Race is not Constant.Race.HROTHGAR or Constant.Race.UNKNOWN) return;
            if (Service.configuration.ignoreFemale && customData.Gender == Constant.Gender.FEMALE && gameObj->ObjectKind == ObjectKind.Pc) return;
            if (Service.configuration.ignoreMale && customData.Gender == Constant.Gender.MALE && gameObj->ObjectKind == ObjectKind.Pc) return;
            if (Service.configuration.ignoreMaleNPC && customData.Gender == Constant.Gender.MALE && gameObj->ObjectKind != ObjectKind.Pc) return;
            if (Service.configuration.ignoreFemaleNPC && customData.Gender == Constant.Gender.FEMALE && gameObj->ObjectKind != ObjectKind.Pc) return;

            NonNativeID.Add(gameObj->NameString);
            ChangeRace(customData, customizePtr, Service.configuration.SelectedRace);
        }

        private static unsafe void ChangeRace(Constant.CharaCustomizeData customData, nint customizePtr, Constant.Race selectedRace)
        {
            customData.Race = selectedRace;
            customData.RaceFeatureType %= 4;
            if (Service.configuration.forceTribe)
                customData.Tribe = (byte)(Service.configuration.selectedTribe+1);
            else
                customData.Tribe = (byte)(((byte)selectedRace * 2) - (customData.Tribe % 2));
            customData.FaceType %= 4;
            customData.ModelType %= 2;
            customData.HairStyle = (byte)((customData.HairStyle % Constant.RaceMappings.RaceHairs[selectedRace]) + 1);
            Marshal.StructureToPtr(customData, customizePtr, true);
        }

        public void Dispose()
        {
            Service.configWindow.OnConfigChanged -= RefreshAllPlayers;
        }
    }
}
