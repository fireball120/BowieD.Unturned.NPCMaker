﻿using BowieD.Unturned.NPCMaker.Localization;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsTree : Condition
    {
        public ushort ID { get; set; }
        public short Value { get; set; }
        public string Tree { get; set; }
        public override Condition_Type Type => Condition_Type.Kills_Tree;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{ID}] {Tree} x{Value}");
                return sb.ToString();
            }
        }

        public override bool Check(Simulation simulation)
        {
            if (simulation.Flags.TryGetValue(ID, out short flag))
            {
                return flag >= Value;
            }
            return false;
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                simulation.Flags.Remove(ID);
            }
        }
        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"]["Default_Condition_TreeKills"];
            }

            if (!simulation.Flags.TryGetValue(ID, out short value))
            {
                value = 0;
            }

            return string.Format(text, value, Value, Tree);
        }
    }
}
