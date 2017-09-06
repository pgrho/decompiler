using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public abstract class ILDecompilerTestBase
    {
        protected readonly ITestOutputHelper Output;

        protected ILDecompilerTestBase(ITestOutputHelper output)
        {
            Output = output;
        }

        [DebuggerStepThrough]
        protected MethodInfo GetMethod(string name)
            => GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

        protected void AssertMethod(MethodInfo method, params Statement[] expectedStatement)
        {
            List<Statement> dm;
            try
            {
                dm = ILDecompiler.Decompile(method);
            }
            catch
            {
                if (Output != null)
                {
                    try
                    {
                        var ils = ILDecompiler.DecompileToInstructions(method);

                        Output.WriteLine(string.Join(Environment.NewLine, ils));
                    }
                    catch { }
                }
                throw;
            }

            if (expectedStatement.Any())
            {
                try
                {
                    Assert.Equal(expectedStatement.Length, dm.Count);
                    for (int i = 0; i < dm.Count; i++)
                    {
                        Assert.True(expectedStatement[i].IsEquivalentTo(dm[i])
                                    || expectedStatement[i].ToString() == dm[i].ToString());
                    }
                }
                catch
                {
                    WriteStatements(dm);
                    throw;
                }
            }
            else
            {
                WriteStatements(dm);
            }
        }

        protected void WriteStatements(List<Statement> dm)
        {
            if (Output != null)
            {
                using (var sw = new StringWriter())
                using (var tw = new IndentedTextWriter(sw))
                {
                    foreach (var s in dm)
                    {
                        s.WriteTo(tw);
                    }

                    tw.Flush();

                    Output.WriteLine(sw.ToString());
                }
            }
        }
    }

}