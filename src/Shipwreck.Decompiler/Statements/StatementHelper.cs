using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.Decompiler.Statements
{
    public static class StatementHelper
    {
        public static bool IsEquivalentTo(this StatementCollection collection, StatementCollection other)
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
                if (!collection[i].IsEquivalentTo(other[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static IEnumerable<Statement> TreeStatements(this IStatementNode statement)
            => statement.Collection?.Owner?.TreeStatements()
                ?? statement.Collection?.Descendants()
                ?? statement.SelfAndDescendants();

        public static IEnumerable<Statement> SelfAndDescendants(this IStatementNode statements)
        {
            if (statements is Statement s)
            {
                yield return s;
            }

            foreach (var c in statements.GetChildCollections())
            {
                foreach (var cs in c.Descendants())
                {
                    yield return cs;
                }
            }
        }

        public static IEnumerable<Statement> Descendants(this StatementCollection collection)
            => collection.SelectMany(s => s.SelfAndDescendants());

        public static IEnumerable<Statement> Ancestors(this Statement statement)
        {
            var c = statement.Collection;
            while (c != null)
            {
                var s = c.Owner as Statement;
                if (s == null)
                {
                    yield break;
                }
                yield return s;

                c = s.Collection;
            }
        }
    }
}