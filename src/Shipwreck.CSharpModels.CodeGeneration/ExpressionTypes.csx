using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

var dir = Path.GetDirectoryName(Path.GetFullPath("a"));

using (var sw = new StreamWriter(Path.Combine(dir, "../Shipwreck.CSharpModels/Statements/Generated Codes/StatementTypes.cs")))
{
    var types = new[]
    {
        "BreakStatement",
        "ConstantDeclarationStatement",
        "ContinueStatement",
        "DoWhileStatement",
        "ExpressionStatement",
        "ForEachStatement",
        "ForStatement",
        "GoToStatement",
        "IfStatement",
        "LabelTarget",
        "LockStatement",
        "ReturnStatement",
        "SwitchStatement",
        "ThrowStatement",
        "TryStatement",
        "UsingStatement",
        "VariableDeclarationStatement",
        "WhileStatement"
    };

    sw.WriteLine("namespace Shipwreck.CSharpModels.Statements");
    sw.WriteLine("{");

    foreach (var t in types)
    {
        sw.WriteLine("    partial class " + t);
        sw.WriteLine("    {");
        sw.WriteLine("        public override void AcceptVisitor(IStatementVisitor visitor)");
        sw.WriteLine("            => visitor.Visit" + t + "(this);");
        sw.WriteLine();
        sw.WriteLine("        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)");
        sw.WriteLine("            => visitor.Visit" + t + "(this);");
        sw.WriteLine();
        sw.WriteLine("        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)");
        sw.WriteLine("            => visitor.Visit" + t + "(this, parameter);");
        sw.WriteLine();
        sw.WriteLine("        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)");
        sw.WriteLine("            => visitor.Visit" + t + "(this, parameter);");
        sw.WriteLine("    }");
        sw.WriteLine();
    }

    string camel(string pascal) => char.ToLower(pascal[0]) + pascal.Substring(1);

    sw.WriteLine("    partial interface IStatementVisitor");
    sw.WriteLine("    {");
    foreach (var t in types)
    {
        sw.WriteLine($"        void Visit{t}({t} {camel(t)});");
    }
    sw.WriteLine("    }");

    sw.WriteLine();
    sw.WriteLine("    partial interface IStatementVisitor<TResult>");
    sw.WriteLine("    {");
    foreach (var t in types)
    {
        sw.WriteLine($"        TResult Visit{t}({t} {camel(t)});");
    }
    sw.WriteLine("    }");

    sw.WriteLine();

    sw.WriteLine("    partial interface IParameteredStatementVisitor<TParameter>");
    sw.WriteLine("    {");
    foreach (var t in types)
    {
        sw.WriteLine($"        void Visit{t}({t} {camel(t)}, TParameter parameter);");
    }
    sw.WriteLine("    }");

    sw.WriteLine();
    sw.WriteLine("    partial interface IParameteredStatementVisitor<TParameter, TResult>");
    sw.WriteLine("    {");
    foreach (var t in types)
    {
        sw.WriteLine($"        TResult Visit{t}({t} {camel(t)}, TParameter parameter);");
    }
    sw.WriteLine("    }");

    sw.WriteLine("}");
}