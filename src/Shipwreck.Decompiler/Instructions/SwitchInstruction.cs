using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class SwitchInstruction : Instruction
    {
        public SwitchInstruction(IEnumerable<int> targets)
        {
            Targets = Array.AsReadOnly(targets.ToArray());
        }

        internal SwitchInstruction(IList<int> targets)
        {
            Targets = new ReadOnlyCollection<int>(targets);
        }

        public ReadOnlyCollection<int> Targets { get; }

        public override FlowControl FlowControl
            => FlowControl.Cond_Branch;

        public override int PopCount => 1;

        public override int PushCount => 0;

        public override bool IsEqualTo(Syntax other)
            => this == other || (other is SwitchInstruction si && Targets.SequenceEqual(si.Targets));

        internal override void SetTo(DecompilationContext context, int index)
            => context.SetTo(this, new[] { context.RootStatements[index + 1] }.Concat(Targets.Select(t => context.GetSyntaxAt(t))).Distinct());

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            if (context.GetFromCount(this) <= 1 && startIndex >= 1)
            {
                var j = startIndex - 1;

                if (context.TryCreateExpression(ref j, out var e))
                {
                    startIndex = j;

                    var sw = new SwitchStatement(e);

                    foreach (var g in Targets.Select((t, i) => new { t, i }).GroupBy(p => p.t))
                    {
                        var sec = new SwitchSection();
                        foreach (var p in g)
                        {
                            sec.Labels.Add(p.i.ToExpression());
                        }
                        sec.Statements.Add(new TemporalGoToStatement(g.Key));
                        sw.Sections.Add(sec);
                    }

                    statement = sw;

                    return true;
                }
            }
            statement = null;
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(9 + Targets.Count * 8);
            sb.Append("switch (");
            foreach (var t in Targets)
            {
                if (sb.Length > 8)
                {
                    sb.Append(", ");
                }
                sb.Append("L_");
                sb.Append(t.ToString("x4"));
            }
            sb.Append(')');

            return sb.ToString();
        }
    }
}