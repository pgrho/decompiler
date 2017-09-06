using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler
{
    public class CSharpSyntaxWriter : IParameteredExpressionVisitor<TextWriter>
    {
        public static readonly CSharpSyntaxWriter Default = new CSharpSyntaxWriter();

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

        public void VisitParameterExpression(ParameterExpression parameterExpression, TextWriter writer)
            => writer.Write(parameterExpression.Name);

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

        #endregion IParameteredExpressionVisitor<TextWriter>

        private void WriteParametersTo(TextWriter writer, IList<Expression> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }
                parameters[i].AcceptVisitor(this, writer);
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
    }
}