using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

var dir = Path.GetDirectoryName(Path.GetFullPath("a"));

using (var sw = new StreamWriter(Path.Combine(dir, "../Shipwreck.Decompiler/Expressions/Generated Codes/ExpressionTypes.cs")))
{
    var types = new[]
    {
        "AssignmentExpression",
        "AwaitExpression",
        "BaseExpression",
        "BinaryExpression",
        "ConditionalExpression",
        "ConstantExpression",
        "DefaultExpression",
        "IndexExpression",
        "MemberExpression",
        "MemberInitExpression",
        "MethodCallExpression",
        "NewArrayExpression",
        "NewExpression",
        "ParameterExpression",
        "ThisExpression",
        "TypeBinaryExpression",
        "UnaryExpression",
        "VariableExpression",
    };

    sw.WriteLine("namespace Shipwreck.Decompiler.Expressions");
    sw.WriteLine("{");

    foreach (var t in types)
    {
        sw.WriteLine("    partial class " + t);
        sw.WriteLine("    {");
        sw.WriteLine("        public override void AcceptVisitor(IExpressionVisitor visitor)");
        sw.WriteLine("            => visitor.Visit" + t + "(this);");
        sw.WriteLine();
        sw.WriteLine("        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)");
        sw.WriteLine("            => visitor.Visit" + t + "(this);");
        sw.WriteLine();
        sw.WriteLine("        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)");
        sw.WriteLine("            => visitor.Visit" + t + "(this, parameter);");
        sw.WriteLine();
        sw.WriteLine("        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)");
        sw.WriteLine("            => visitor.Visit" + t + "(this, parameter);");
        sw.WriteLine("    }");
        sw.WriteLine();
    }

    string camel(string pascal) => char.ToLower(pascal[0]) + pascal.Substring(1);
        

    sw.WriteLine("    partial interface IExpressionVisitor");
    sw.WriteLine("    {");
    foreach (var t in types)
    {
        sw.WriteLine($"        void Visit{t}({t} {camel(t)});");
    }
    sw.WriteLine("    }");

    sw.WriteLine();
    sw.WriteLine("    partial interface IExpressionVisitor<TResult>");
    sw.WriteLine("    {");
    foreach (var t in types)
    {
        sw.WriteLine($"        TResult Visit{t}({t} {camel(t)});");
    }
    sw.WriteLine("    }");

    sw.WriteLine();

    sw.WriteLine("    partial interface IParameteredExpressionVisitor<TParameter>");
    sw.WriteLine("    {");
    foreach (var t in types)
    {
        sw.WriteLine($"        void Visit{t}({t} {camel(t)}, TParameter parameter);");
    }
    sw.WriteLine("    }");

    sw.WriteLine();
    sw.WriteLine("    partial interface IParameteredExpressionVisitor<TParameter, TResult>");
    sw.WriteLine("    {");
    foreach (var t in types)
    {
        sw.WriteLine($"        TResult Visit{t}({t} {camel(t)}, TParameter parameter);");
    }
    sw.WriteLine("    }");

    sw.WriteLine("}");
}