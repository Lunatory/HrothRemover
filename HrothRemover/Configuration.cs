using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using HrothRemover.Utils;
using static HrothRemover.Utils.Constant;

namespace HrothRemover
{
    [Serializable]
    internal class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;
        public Constant.Race SelectedRace { get; set; } = Constant.Race.MIQOTE;
        public bool enabled { get; set; } = false;
        public bool stayOn { get; set; } = false;
        public bool nameHQ { get; set; } = true;
        public bool forceTribe { get; set; } = false;
        public Constant.SelectedTribe selectedTribe { get; set; } = Constant.SelectedTribe.ZERO;
        public bool ignoreFemale { get; set; } = false;
        public bool ignoreMale { get; set; } = false;
        public bool ignoreMaleNPC { get; set; } = true;
        public bool ignoreFemaleNPC { get; set; } = true;

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private IDalamudPluginInterface? pluginInterface;

        public void Initialize(IDalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            pluginInterface!.SavePluginConfig(this);
        }
    }
}
