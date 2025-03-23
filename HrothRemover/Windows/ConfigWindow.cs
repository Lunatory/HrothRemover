using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Numerics;
using HrothRemover.Utils;
using static HrothRemover.Utils.Constant;

namespace HrothRemover.Windows;

internal class ConfigWindow : Window
{
    private readonly Configuration configuration;
    private readonly string[] race = ["Lalafell", "Hyur", "Elezen", "Miqo'te", "Roegadyn", "Au Ra", "Hrothgar", "Viera"];
    private int selectedRaceIndex = 3;
    public event Action? OnConfigChanged;

    public ConfigWindow(Plugin plugin) : base(
        "HrothRemoverConfiguration Window",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        Size = new Vector2(285, 250);
        SizeCondition = ImGuiCond.Always;

        configuration = Service.configuration;
    }

    public override void Draw()
    {
        // select race
        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted("Target Race");
        ImGui.SameLine();
        if (ImGui.Combo("###Race", ref selectedRaceIndex, race, race.Length))
        {
            configuration.SelectedRace = MapIndexToRace(selectedRaceIndex);
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        // Enabled
        bool _Enabled = configuration.enabled;
        if (ImGui.Checkbox("Enable", ref _Enabled))
        {
            configuration.enabled = _Enabled;
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        bool _StayOn = configuration.stayOn;
        if (ImGui.Checkbox("Stay on", ref _StayOn))
        {
            configuration.stayOn = _StayOn;
            configuration.Save();
        }



        ImGui.Separator();
        bool _NameHQ = configuration.nameHQ;
        if (ImGui.Checkbox("Add  symbol to native Hrothgars", ref _NameHQ))
        {
            configuration.nameHQ = _NameHQ;
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        ImGui.Separator();
        bool _IgnoreMale = configuration.ignoreMale;
        if (ImGui.Checkbox("Don't transform Male PCs", ref _IgnoreMale))
        {
            configuration.ignoreMale = _IgnoreMale;
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        bool _IgnoreFemale = configuration.ignoreFemale;
        if (ImGui.Checkbox("Don't transform Female PCs", ref _IgnoreFemale))
        {
            configuration.ignoreFemale = _IgnoreFemale;
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        bool _IgnoreMaleNPC = configuration.ignoreMaleNPC;
        if (ImGui.Checkbox("Don't transform Male NPCs", ref _IgnoreMaleNPC))
        {
            configuration.ignoreMaleNPC = _IgnoreMaleNPC;
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        bool _IgnoreFemaleNPC = configuration.ignoreFemaleNPC;
        if (ImGui.Checkbox("Don't transform Female NPCs", ref _IgnoreFemaleNPC))
        {
            configuration.ignoreFemaleNPC = _IgnoreFemaleNPC;
            configuration.Save();
            OnConfigChanged?.Invoke();
        }
    }

    private static Constant.Race MapIndexToRace(int index)
    {
        return index switch
        {
            0 => Constant.Race.LALAFELL,
            1 => Constant.Race.HYUR,
            2 => Constant.Race.ELEZEN,
            3 => Constant.Race.MIQOTE,
            4 => Constant.Race.ROEGADYN,
            5 => Constant.Race.AU_RA,
            6 => Constant.Race.HROTHGAR,
            7 => Constant.Race.VIERA,
            _ => Constant.Race.LALAFELL,
        };
    }

    public void InvokeConfigChanged()
    {
        OnConfigChanged?.Invoke();
    }
}
