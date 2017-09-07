using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Shipwreck.CSharpModels.Expressions;
using Shipwreck.CSharpModels.Statements;

namespace Shipwreck.CSharpModels
{
    public class CSharpSyntaxWriter : IParameteredExpressionVisitor<TextWriter>, IParameteredStatementVisitor<IndentedTextWriter>
    {
        public static readonly CSharpSyntaxWriter Default = new CSharpSyntaxWriter();

        #region IParameteredStatementVisitor<IndentedTextWriter>

        public void VisitBreakStatement(BreakStatement breakStatement, IndentedTextWriter writer)
            => writer.WriteLine("break;");

        public void VisitConstantDeclarationStatement(ConstantDeclarationStatement constantDeclarationStatement, IndentedTextWriter writer)
        {
            if (constantDeclarationStatement.ShouldSerializeDeclarators())
            {
                writer.Write("const ");
                writer.Write(constantDeclarationStatement.Type.FullName);
                writer.Write(' ');

                WriteDeclaration(constantDeclarationStatement, writer);

                writer.WriteLine(';');
            }
        }

        public void VisitContinueStatement(ContinueStatement continueStatement, IndentedTextWriter writer)
            => writer.WriteLine("continue;");

        public void VisitDoWhileStatement(DoWhileStatement doWhileStatement, IndentedTextWriter writer)
        {
            writer.WriteLine("do");
            writer.WriteLine('{');
            if (doWhileStatement.ShouldSerializeStatements())
            {
                writer.Indent++;
                foreach (var s in doWhileStatement.Statements)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
            writer.Write("} while (");
            doWhileStatement.Condition.AcceptVisitor(this, writer);
            writer.WriteLine(");");
        }

        public void VisitExpressionStatement(ExpressionStatement expressionStatement, IndentedTextWriter writer)
        {
            expressionStatement.Expression.AcceptVisitor(this, writer);
            writer.WriteLine(';');
        }

        public void VisitForEachStatement(ForEachStatement forEachStatement, IndentedTextWriter writer)
        {
            writer.Write("foreach (");

            writer.Write(forEachStatement.Type?.FullName ?? "var");
            writer.Write(' ');

            writer.Write(forEachStatement.Identifier);

            writer.Write(" in ");
            forEachStatement.Expression.AcceptVisitor(this, writer);
            writer.WriteLine(")");
            writer.WriteLine('{');
            if (forEachStatement.ShouldSerializeStatements())
            {
                writer.Indent++;
                foreach (var s in forEachStatement.Statements)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');
        }

        public void VisitForStatement(ForStatement forStatement, IndentedTextWriter writer)
        {
            writer.Write("for (");
            forStatement.Initializer?.AcceptVisitor(this, writer);
            writer.Write("; ");
            forStatement.Condition?.AcceptVisitor(this, writer);
            writer.Write("; ");
            forStatement.Iterator?.AcceptVisitor(this, writer);
            writer.WriteLine(")");
            writer.WriteLine('{');
            if (forStatement.ShouldSerializeStatements())
            {
                writer.Indent++;
                foreach (var s in forStatement.Statements)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');
        }

        public void VisitGoToStatement(GoToStatement goToStatement, IndentedTextWriter writer)
        {
            writer.Write("goto ");
            writer.Write(goToStatement.Target.Name);
            writer.WriteLine(';');
        }

        public void VisitIfStatement(IfStatement ifStatement, IndentedTextWriter writer)
        {
            writer.Write("if (");
            ifStatement.Condition.AcceptVisitor(this, writer);
            writer.WriteLine(')');

            writer.WriteLine('{');
            if (ifStatement.ShouldSerializeTruePart())
            {
                writer.Indent++;
                foreach (var s in ifStatement.TruePart)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');

            if (ifStatement.ShouldSerializeFalsePart())
            {
                if (ifStatement.FalsePart.Count == 1 && ifStatement.FalsePart[0] is IfStatement cib)
                {
                    writer.Write("else ");
                    cib.AcceptVisitor(this, writer);
                }
                else
                {
                    writer.WriteLine("else");
                    writer.WriteLine('{');
                    writer.Indent++;
                    foreach (var s in ifStatement.FalsePart)
                    {
                        s.AcceptVisitor(this, writer);
                    }
                    writer.Indent--;
                    writer.WriteLine('}');
                }
            }
        }

        public void VisitLabelTarget(LabelTarget labelTarget, IndentedTextWriter writer)
        {
            writer.Write(labelTarget.Name);
            writer.WriteLine(':');
        }

        public void VisitLockStatement(LockStatement lockStatement, IndentedTextWriter writer)
        {
            writer.Write("lock (");
            lockStatement.Object.AcceptVisitor(this, writer);
            writer.WriteLine(")");
            writer.WriteLine('{');
            if (lockStatement.ShouldSerializeStatements())
            {
                writer.Indent++;
                foreach (var s in lockStatement.Statements)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');
        }

        public void VisitReturnStatement(ReturnStatement returnStatement, IndentedTextWriter writer)
        {
            writer.Write("return");
            if (returnStatement.Value != null)
            {
                writer.Write(' ');
                returnStatement.Value.AcceptVisitor(this, writer);
            }
            writer.WriteLine(';');
        }

        public void VisitSwitchStatement(SwitchStatement switchStatement, IndentedTextWriter writer)
        {
            writer.Write("switch (");
            switchStatement.Expression.AcceptVisitor(this, writer);
            writer.WriteLine(')');
            writer.WriteLine('{');
            if (switchStatement.ShouldSerializeSections())
            {
                writer.Indent++;
                foreach (var s in switchStatement.Sections)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');
        }

        public void VisitThrowStatement(ThrowStatement throwStatement, IndentedTextWriter writer)
        {
            writer.Write("throw");
            if (throwStatement.Value != null)
            {
                writer.Write(' ');
                throwStatement.Value.AcceptVisitor(this, writer);
            }
            writer.WriteLine(';');
        }

        public void VisitTryStatement(TryStatement tryStatement, IndentedTextWriter writer)
        {
            writer.WriteLine("try");
            writer.WriteLine('{');
            if (tryStatement.ShouldSerializeBlock())
            {
                writer.Indent++;
                foreach (var s in tryStatement.Block)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');

            if (tryStatement.ShouldSerializeCatchClauses())
            {
                foreach (var c in tryStatement.CatchClauses)
                {
                    c.AcceptVisitor(this, writer);
                }
            }

            if (tryStatement.ShouldSerializeFinallyStatements() || !tryStatement.ShouldSerializeCatchClauses())
            {
                writer.WriteLine("finally");
                writer.WriteLine('{');
                writer.Indent++;
                foreach (var s in tryStatement.Finally)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
                writer.WriteLine('}');
            }
        }

        public void VisitUsingStatement(UsingStatement usingStatement, IndentedTextWriter writer)
        {
            writer.Write("using (");

            if (usingStatement.Resource is VariableDeclarationStatement ds)
            {
                if (ds.Declarators.Count == 1)
                {
                    WriteDeclaration(ds, writer);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (usingStatement.Resource is ExpressionStatement es)
            {
                es.Expression.AcceptVisitor(this, writer);
            }
            else
            {
                throw new InvalidOperationException();
            }

            usingStatement.Resource.AcceptVisitor(this, writer);
            writer.WriteLine(")");

            if (usingStatement.Statements?.Count == 1 && usingStatement.Statements[0] is UsingStatement)
            {
                usingStatement.Statements[0].AcceptVisitor(this, writer);
            }
            else
            {
                writer.WriteLine('{');
                if (usingStatement.ShouldSerializeStatements())
                {
                    writer.Indent++;
                    foreach (var s in usingStatement.Statements)
                    {
                        s.AcceptVisitor(this, writer);
                    }
                    writer.Indent--;
                }
                writer.WriteLine('}');
            }
        }

        public void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement, IndentedTextWriter writer)
        {
            if (variableDeclarationStatement.ShouldSerializeDeclarators())
            {
                writer.Write(variableDeclarationStatement.Type?.FullName ?? "var");
                writer.Write(' ');

                WriteDeclaration(variableDeclarationStatement, writer);

                writer.WriteLine(';');
            }
        }

        public void VisitWhileStatement(WhileStatement whileStatement, IndentedTextWriter writer)
        {
            writer.Write("while (");
            whileStatement.Condition.AcceptVisitor(this, writer);
            writer.WriteLine(")");
            WriteStatementsCore(writer, whileStatement);
        }

        public void VisitCatchClause(CatchClause catchClause, IndentedTextWriter writer)
        {
            writer.Write("catch");

            if (catchClause.CatchType != null && catchClause.CatchType != typeof(object))
            {
                writer.Write(" (");
                writer.Write(catchClause.CatchType.FullName);
                writer.Write(')');
            }

            writer.WriteLine();
            writer.WriteLine('{');
            if (catchClause.ShouldSerializeStatements())
            {
                writer.Indent++;
                foreach (var s in catchClause.Statements)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');
        }

        public void VisitSwitchSection(SwitchSection switchSection, IndentedTextWriter writer)
        {
            if (switchSection.ShouldSerializeLabels() && switchSection.ShouldSerializeStatements())
            {
                foreach (var v in switchSection.Labels)
                {
                    if (v == null)
                    {
                        writer.WriteLine("default:");
                    }
                    else
                    {
                        writer.Write("case ");
                        v.AcceptVisitor(this, writer);
                        writer.WriteLine(';');
                    }
                }

                writer.Indent++;
                foreach (var ss in switchSection.Statements)
                {
                    ss.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
        }

        private void WriteStatementsCore(IndentedTextWriter writer, IContinuableStatement iterationStatement)
        {
            writer.WriteLine('{');
            if (iterationStatement.ShouldSerializeStatements())
            {
                writer.Indent++;
                foreach (var s in iterationStatement.Statements)
                {
                    s.AcceptVisitor(this, writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');
        }

        internal void WriteDeclaration(DeclarationStatement declarationStatement, IndentedTextWriter writer)
        {
            for (int i = 0; i < declarationStatement.Declarators.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }
                var d = declarationStatement.Declarators[i];

                writer.Write(d.Identifier);
                if (d.Initializer == null)
                {
                    writer.Write(" = ");
                    d.Initializer.AcceptVisitor(this, writer);
                }
            }
        }

        #endregion IParameteredStatementVisitor<IndentedTextWriter>

        #region IParameteredExpressionVisitor<TextWriter>

        public void VisitAssignmentExpression(AssignmentExpression assignmentExpression, TextWriter writer)
        {
            var isChecked = assignmentExpression.Operator.IsChecked();

            if (isChecked)
            {
                writer.Write("checked(");
            }

            WriteSecondChild(writer, assignmentExpression.Left, assignmentExpression);

            writer.Write(' ');
            if (assignmentExpression.Operator != BinaryOperator.Default)
            {
                writer.Write(assignmentExpression.Operator.GetToken());
            }
            writer.Write("= ");

            WriteFirstChild(writer, assignmentExpression.Right, assignmentExpression);

            if (isChecked)
            {
                writer.Write(")");
            }
        }

        public void VisitAwaitExpression(AwaitExpression awaitExpression, TextWriter writer)
        {
            writer.Write("await ");
            WriteFirstChild(writer, awaitExpression.Expression, awaitExpression);
        }

        public void VisitBaseExpression(BaseExpression baseExpression, TextWriter writer)
            => writer.Write("base");

        public void VisitBinaryExpression(BinaryExpression binaryExpression, TextWriter writer)
        {
            var isChecked = binaryExpression.Operator.IsChecked();

            if (isChecked)
            {
                writer.Write("checked(");
            }

            WriteFirstChild(writer, binaryExpression.Left, binaryExpression);

            writer.Write(' ');
            writer.Write(binaryExpression.Operator.GetToken());
            writer.Write(' ');

            WriteSecondChild(writer, binaryExpression.Right, binaryExpression);

            if (isChecked)
            {
                writer.Write(')');
            }
        }

        public void VisitConditionalExpression(ConditionalExpression conditionalExpression, TextWriter writer)
        {
            WriteSecondChild(writer, conditionalExpression.Condition, conditionalExpression);
            writer.Write(" ? ");
            WriteSecondChild(writer, conditionalExpression.TruePart, conditionalExpression);
            writer.Write(" : ");
            WriteFirstChild(writer, conditionalExpression.FalsePart, conditionalExpression);
        }

        public void VisitConstantExpression(ConstantExpression constantExpression, TextWriter writer)
        {
            var value = constantExpression.Value;
            if (value == null)
            {
                writer.Write("null");
            }
            else if (value is bool b)
            {
                writer.Write(b ? "true" : "false");
            }
            else if (value is double d)
            {
                writer.Write(d.ToString("R"));
            }
            else if (value is float f)
            {
                writer.Write(f.ToString("R"));
            }
            else if (value is string s)
            {
                writer.Write('"');
                foreach (var c in s)
                {
                    switch (c)
                    {
                        case '\0':
                            writer.Write("\\0");
                            break;

                        case '\b':
                            writer.Write("\\b");
                            break;

                        case '\n':
                            writer.Write("\\n");
                            break;

                        case '\r':
                            writer.Write("\\r");
                            break;

                        case '\t':
                            writer.Write("\\t");
                            break;

                        case '"':
                            writer.Write("\\\"");
                            break;

                        default:
                            writer.Write(c);
                            break;
                    }
                }
                writer.Write('"');
            }
            else if (value is IFormattable c)
            {
                writer.Write(c.ToString("D", null));
            }
            else if (value is Type t)
            {
                writer.Write("typeof(");
                writer.Write(t.FullName);
                writer.Write(')');
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void VisitDefaultExpression(DefaultExpression defaultExpression, TextWriter writer)
        {
            writer.Write("default(");
            writer.Write(defaultExpression.Type.FullName);
            writer.Write(')');
        }

        public void VisitIndexExpression(IndexExpression indexExpression, TextWriter writer)
        {
            WriteFirstChild(writer, indexExpression.Object, indexExpression);
            writer.Write('[');
            WriteParametersTo(writer, indexExpression.Parameters);
            writer.Write(']');
        }

        public void VisitMemberExpression(MemberExpression memberExpression, TextWriter writer)
        {
            if (memberExpression.Object == null)
            {
                writer.Write(memberExpression.Member.DeclaringType.FullName);
            }
            else
            {
                WriteFirstChild(writer, memberExpression.Object, memberExpression);
            }
            writer.Write('.');
            writer.Write(memberExpression.Member.Name);
        }

        public void VisitMemberInitExpression(MemberInitExpression memberInitExpression, TextWriter writer)
        {
            memberInitExpression.NewExpression.AcceptVisitor(this, writer);

            var itw = writer as IndentedTextWriter;

            if (itw != null)
            {
                itw.WriteLine();
                itw.WriteLine('{');
                itw.Indent++;
            }
            else
            {
                writer.Write(" { ");
            }

            var f = true;
            foreach (var b in memberInitExpression.Bindings)
            {
                if (f)
                {
                    f = false;
                }
                else
                {
                    if (itw != null)
                    {
                        itw.WriteLine(',');
                    }
                    else
                    {
                        writer.Write(", ");
                    }
                }
                b.AcceptVisitor(this, writer);
            }

            if (itw != null)
            {
                itw.WriteLine();
                itw.Indent--;
                itw.Write('}');
            }
            else
            {
                writer.Write(" }");
            }
        }

        public void VisitMethodCallExpression(MethodCallExpression methodCallExpression, TextWriter writer)
        {
            if (methodCallExpression.Object != null)
            {
                WriteFirstChild(writer, methodCallExpression.Object, methodCallExpression);
            }
            else
            {
                writer.Write(methodCallExpression.Method.DeclaringType.FullName);
            }
            writer.Write('.');
            writer.Write(methodCallExpression.Method.Name);
            writer.Write('(');
            WriteParametersTo(writer, methodCallExpression.Parameters);
            writer.Write(')');
        }

        public void VisitNewArrayExpression(NewArrayExpression newArrayExpression, TextWriter writer)
        {
            writer.Write("new ");
            writer.Write(newArrayExpression.Type.GetElementType().FullName);
            writer.Write('[');
            newArrayExpression.Length.AcceptVisitor(this, writer);
            writer.Write(']');
        }

        public void VisitNewExpression(NewExpression newExpression, TextWriter writer)
        {
            writer.Write("new ");
            writer.Write(newExpression.Constructor.DeclaringType.FullName);
            writer.Write('(');
            WriteParametersTo(writer, newExpression.Parameters);
            writer.Write(')');
        }

        public void VisitParameterExpression(ParameterExpression writerExpression, TextWriter writer)
            => writer.Write(writerExpression.Name);

        public void VisitThisExpression(ThisExpression thisExpression, TextWriter writer)
            => writer.Write("this");

        public void VisitTypeBinaryExpression(TypeBinaryExpression typeBinaryExpression, TextWriter writer)
        {
            WriteFirstChild(writer, typeBinaryExpression.Expression, typeBinaryExpression);

            writer.Write(" is ");

            writer.Write(typeBinaryExpression.TypeOperand.FullName);
        }

        public void VisitUnaryExpression(UnaryExpression unaryExpression, TextWriter writer)
        {
            switch (unaryExpression.Operator)
            {
                case UnaryOperator.UnaryPlus:
                    writer.Write('+');
                    break;

                case UnaryOperator.UnaryNegation:
                    writer.Write('-');
                    break;

                case UnaryOperator.LogicalNot:
                    writer.Write('!');
                    break;

                case UnaryOperator.OnesComplement:
                    writer.Write('~');
                    break;

                case UnaryOperator.PreIncrement:
                    writer.Write("++");
                    break;

                case UnaryOperator.PreDecrement:
                    writer.Write("--");
                    break;

                case UnaryOperator.AddressOf:
                    writer.Write('&');
                    break;

                case UnaryOperator.PostIncrement:
                case UnaryOperator.PostDecrement:
                case UnaryOperator.TypeAs:
                    break;

                case UnaryOperator.Convert:
                case UnaryOperator.ConvertChecked:
                    writer.Write('(');
                    writer.Write(unaryExpression.Type.FullName);
                    writer.Write(')');
                    break;

                default:
                    throw new NotImplementedException();
            }

            WriteFirstChild(writer, unaryExpression.Operand, unaryExpression);

            switch (unaryExpression.Operator)
            {
                case UnaryOperator.PostIncrement:
                    writer.Write("++");
                    break;

                case UnaryOperator.PostDecrement:
                    writer.Write("--");
                    break;

                case UnaryOperator.TypeAs:
                    writer.Write(" as ");
                    writer.Write(unaryExpression.Type.FullName);
                    break;
            }
        }

        public void VisitVariableExpression(VariableExpression variableExpression, TextWriter writer)
        {
            writer.Write("$local");
            writer.Write(variableExpression.Index);
        }

        public void VisitMemberAssignment(MemberAssignment memberAssignment, TextWriter writer)
        {
            writer.Write(memberAssignment.Member.Name);
            writer.Write(" = ");
            memberAssignment.Expression.AcceptVisitor(this, writer);
        }

        #region Helper methods

        private void WriteParametersTo(TextWriter writer, IList<Expression> writers)
        {
            for (int i = 0; i < writers.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }
                writers[i].AcceptVisitor(this, writer);
            }
        }

        private void WriteFirstChild(TextWriter writer, Expression expression, Expression parent)
        {
            if (expression != null)
            {
                var wrap = expression.Precedence > parent.Precedence;

                if (wrap)
                {
                    writer.Write('(');
                }
                expression.AcceptVisitor(this, writer);
                if (wrap)
                {
                    writer.Write(')');
                }
            }
        }

        private void WriteSecondChild(TextWriter writer, Expression expression, Expression parent)
        {
            if (expression != null)
            {
                var wrap = expression.Precedence >= parent.Precedence;

                if (wrap)
                {
                    writer.Write('(');
                }
                expression.AcceptVisitor(this, writer);
                if (wrap)
                {
                    writer.Write(')');
                }
            }
        }

        #endregion Helper methods

        #endregion IParameteredExpressionVisitor<TextWriter>
    }
}