using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class TryCatchTest : ILDecompilerTestBase
    {
        public TryCatchTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        #region Try-Catch

        private static int TryCatch(int a, int b)
        {
            try
            {
                return checked(a + b);
            }
            catch (InvalidOperationException)
            {
                return -3;
            }
            catch
            {
                try
                {
                    return checked(a - b);
                }
                catch (InvalidOperationException)
                {
                    return -2;
                }
                catch
                {
                    return -1;
                }
            }
            finally
            {
                Console.WriteLine("{0} {1}", a.ToString(), b.ToString());
            }
        }

        #endregion Try-Catch

        private static void Lock(IDisposable d)
        {
            lock (d)
            {
                d.Dispose();
            }
        }

        private static void Using()
        {
            using (var ms = new MemoryStream())
            {
                ms.Flush();
            }
        }

        [Theory]
        [InlineData(nameof(TryCatch))]
        [InlineData(nameof(Lock))]
        [InlineData(nameof(Using))]
        public void NotImplementedTest(string m)
            => AssertMethod(GetMethod(m));
    }
}