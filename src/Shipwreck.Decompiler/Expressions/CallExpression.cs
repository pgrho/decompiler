using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Shipwreck.Decompiler.Expressions
{
    public abstract class CallExpression : Expression
    {
        internal CallExpression(IEnumerable<Expression> parameters, bool shouldCopy = true)
        {
            Parameters = (shouldCopy ? null
                            : (parameters as ReadOnlyCollection<Expression>
                                ?? (parameters is IList<Expression> l ? new ReadOnlyCollection<Expression>(l) : null)))
                        ?? Array.AsReadOnly(parameters?.ToArray() ?? new Expression[0]);
        }

        public ReadOnlyCollection<Expression> Parameters { get; }

        protected void WriteParametersTo(TextWriter writer)
        {
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }
                Parameters[i].WriteTo(writer);
            }
        }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is CallExpression ne
                && (Parameters.Count == ne.Parameters.Count
                && Enumerable.Range(0, Parameters.Count).All(i => Parameters[i].IsEquivalentTo(ne.Parameters[i]))));

        protected bool TryReduceParameters(out Expression[] parameters)
        {
            parameters = null;
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (Parameters[i].TryReduce(out var e))
                {
                    (parameters ?? (parameters = new Expression[Parameters.Count]))[i] = e;
                }
            }
            return SetCurrentParametersIfNull(parameters);
        }

        protected bool TryReplaceParameters(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional, out Expression[] parameters)
        {
            parameters = null;
            for (int i = 0; i < Parameters.Count; i++)
            {
                var cp = Parameters[i];
                var np = cp.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);
                if (cp != np)
                {
                    (parameters ?? (parameters = new Expression[Parameters.Count]))[i] = np;
                }
            }
            return SetCurrentParametersIfNull(parameters);
        }

        private bool SetCurrentParametersIfNull(Expression[] parameters)
        {
            if (parameters == null)
            {
                return false;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == null)
                {
                    parameters[i] = parameters[i] ?? Parameters[i];
                }
            }

            return true;
        }
    }
}