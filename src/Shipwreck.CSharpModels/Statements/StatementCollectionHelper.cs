using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.CSharpModels.Statements
{
    internal static class StatementCollectionHelper
    {
        public static bool ShouldSerialize(this StatementCollection collection)
            => collection?.Count > 0;

        public static bool ReduceBlock(this StatementCollection block)
        {
            if (block == null)
            {
                return false;
            }

            var reduced = false;
            bool iter;
            do
            {
                iter = false;
                for (var i = 0; i < block.Count; i++)
                {
                    var s = block[i];

                    if (s.Reduce())
                    {
                        reduced = iter = true;
                        break;
                    }
                    else if (s.IsBreaking())
                    {
                        while (i + 1 < block.Count)
                        {
                            var ns = block[i + 1];
                            if (ns.HasLabel())
                            {
                                break;
                            }

                            block.RemoveAt(i + 1);
                            reduced = iter = true;
                        }
                        if (iter)
                        {
                            break;
                        }
                    }
                }

                if (!(block.Owner is TryStatement ts && block == ts.Finally)
                    && !(block.Owner is IContinuableStatement))
                {
                    var ls = block.LastOrDefault();

                    if (!ls.IsBreaking()
                        || (block.Owner is SwitchStatement && ls is BreakStatement))
                    {
                        var ns = block.NextStatement();

                        // TODO: support throw
                        if (ns is ReturnStatement)
                        {
                            if (ls is BreakStatement)
                            {
                                block.RemoveAt(block.Count - 1);
                            }
                            block.Add(ns.Clone());
                            reduced = iter = true;
                        }
                    }
                }

                if (block.NextStatement() == null
                    && (block.Owner as Statement)?.Ancestors().OfType<IContinuableStatement>().Any() != true)
                {
                    if (block.LastOrDefault() is ReturnStatement rs && rs.Value == null)
                    {
                        block.RemoveAt(block.Count - 1);
                        reduced = iter = true;
                    }
                }
            } while (iter);

            return reduced;
        }

        internal static Statement NextStatement(this StatementCollection block)
        {
            if (block.Owner?.Collection == null || block.Owner is IContinuableStatement)
            {
                return null;
            }

            return block.Owner.Collection.GetNextOf(block.Owner as Statement, out _)
                    ?? block.Owner.Collection.NextStatement();
        }

        public static Statement GetPreviousOf(this StatementCollection collection, Statement statement, out int i)
        {
            i = collection?.IndexOf(statement) ?? -1;
            if (i > 0)
            {
                return collection[i - 1];
            }
            return null;
        }

        public static Statement GetNextOf(this StatementCollection collection, Statement statement, out int i)
        {
            i = collection?.IndexOf(statement) ?? -1;
            if (0 <= i && i + 1 < collection.Count)
            {
                return collection[i + 1];
            }
            return null;
        }

        public static bool IsEqualTo(this StatementCollection collection, StatementCollection other)
        {
            if (collection == null)
            {
                return !(other?.Count > 0);
            }
            else if (other?.Count != collection.Count)
            {
                return false;
            }

            for (int i = 0; i < collection.Count; i++)
            {
                if (!collection[i].IsEqualTo(other[i]))
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool HaveLabel(this IEnumerable<Statement> statements)
        {
            foreach (var s in statements)
            {
                if (s.HasLabel())
                {
                    return true;
                }
            }
            return false;
        }
    }
}