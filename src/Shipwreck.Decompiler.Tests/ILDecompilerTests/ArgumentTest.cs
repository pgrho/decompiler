using System;
using Shipwreck.Decompiler.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class ArgumentTest : ILDecompilerTestBase
    {
        public ArgumentTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        #region LoadTest

        private static int LoadStatic_0(int a0) => a0;

        private static int LoadStatic_1(int a0, int a1) => a1;

        private static int LoadStatic_2(int a0, int a1, int a2) => a2;

        private static int LoadStatic_3(int a0, int a1, int a2, int a3) => a3;

        private static int LoadStatic_255(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17, int a18, int a19, int a20, int a21, int a22, int a23, int a24, int a25, int a26, int a27, int a28, int a29, int a30, int a31, int a32, int a33, int a34, int a35, int a36, int a37, int a38, int a39, int a40, int a41, int a42, int a43, int a44, int a45, int a46, int a47, int a48, int a49, int a50, int a51, int a52, int a53, int a54, int a55, int a56, int a57, int a58, int a59, int a60, int a61, int a62, int a63, int a64, int a65, int a66, int a67, int a68, int a69, int a70, int a71, int a72, int a73, int a74, int a75, int a76, int a77, int a78, int a79, int a80, int a81, int a82, int a83, int a84, int a85, int a86, int a87, int a88, int a89, int a90, int a91, int a92, int a93, int a94, int a95, int a96, int a97, int a98, int a99, int a100, int a101, int a102, int a103, int a104, int a105, int a106, int a107, int a108, int a109, int a110, int a111, int a112, int a113, int a114, int a115, int a116, int a117, int a118, int a119, int a120, int a121, int a122, int a123, int a124, int a125, int a126, int a127, int a128, int a129, int a130, int a131, int a132, int a133, int a134, int a135, int a136, int a137, int a138, int a139, int a140, int a141, int a142, int a143, int a144, int a145, int a146, int a147, int a148, int a149, int a150, int a151, int a152, int a153, int a154, int a155, int a156, int a157, int a158, int a159, int a160, int a161, int a162, int a163, int a164, int a165, int a166, int a167, int a168, int a169, int a170, int a171, int a172, int a173, int a174, int a175, int a176, int a177, int a178, int a179, int a180, int a181, int a182, int a183, int a184, int a185, int a186, int a187, int a188, int a189, int a190, int a191, int a192, int a193, int a194, int a195, int a196, int a197, int a198, int a199, int a200, int a201, int a202, int a203, int a204, int a205, int a206, int a207, int a208, int a209, int a210, int a211, int a212, int a213, int a214, int a215, int a216, int a217, int a218, int a219, int a220, int a221, int a222, int a223, int a224, int a225, int a226, int a227, int a228, int a229, int a230, int a231, int a232, int a233, int a234, int a235, int a236, int a237, int a238, int a239, int a240, int a241, int a242, int a243, int a244, int a245, int a246, int a247, int a248, int a249, int a250, int a251, int a252, int a253, int a254, int a255) => a255;

        private static int LoadStatic_256(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17, int a18, int a19, int a20, int a21, int a22, int a23, int a24, int a25, int a26, int a27, int a28, int a29, int a30, int a31, int a32, int a33, int a34, int a35, int a36, int a37, int a38, int a39, int a40, int a41, int a42, int a43, int a44, int a45, int a46, int a47, int a48, int a49, int a50, int a51, int a52, int a53, int a54, int a55, int a56, int a57, int a58, int a59, int a60, int a61, int a62, int a63, int a64, int a65, int a66, int a67, int a68, int a69, int a70, int a71, int a72, int a73, int a74, int a75, int a76, int a77, int a78, int a79, int a80, int a81, int a82, int a83, int a84, int a85, int a86, int a87, int a88, int a89, int a90, int a91, int a92, int a93, int a94, int a95, int a96, int a97, int a98, int a99, int a100, int a101, int a102, int a103, int a104, int a105, int a106, int a107, int a108, int a109, int a110, int a111, int a112, int a113, int a114, int a115, int a116, int a117, int a118, int a119, int a120, int a121, int a122, int a123, int a124, int a125, int a126, int a127, int a128, int a129, int a130, int a131, int a132, int a133, int a134, int a135, int a136, int a137, int a138, int a139, int a140, int a141, int a142, int a143, int a144, int a145, int a146, int a147, int a148, int a149, int a150, int a151, int a152, int a153, int a154, int a155, int a156, int a157, int a158, int a159, int a160, int a161, int a162, int a163, int a164, int a165, int a166, int a167, int a168, int a169, int a170, int a171, int a172, int a173, int a174, int a175, int a176, int a177, int a178, int a179, int a180, int a181, int a182, int a183, int a184, int a185, int a186, int a187, int a188, int a189, int a190, int a191, int a192, int a193, int a194, int a195, int a196, int a197, int a198, int a199, int a200, int a201, int a202, int a203, int a204, int a205, int a206, int a207, int a208, int a209, int a210, int a211, int a212, int a213, int a214, int a215, int a216, int a217, int a218, int a219, int a220, int a221, int a222, int a223, int a224, int a225, int a226, int a227, int a228, int a229, int a230, int a231, int a232, int a233, int a234, int a235, int a236, int a237, int a238, int a239, int a240, int a241, int a242, int a243, int a244, int a245, int a246, int a247, int a248, int a249, int a250, int a251, int a252, int a253, int a254, int a255, int a256) => a256;

        private int LoadInstance_0(int a0) => a0;

        private int LoadInstance_1(int a0, int a1) => a1;

        private int LoadInstance_2(int a0, int a1, int a2) => a2;

        private int LoadInstance_3(int a0, int a1, int a2, int a3) => a3;

        [Theory]
        [InlineData(nameof(LoadStatic_0), 0)]
        [InlineData(nameof(LoadStatic_1), 1)]
        [InlineData(nameof(LoadStatic_2), 2)]
        [InlineData(nameof(LoadStatic_3), 3)]
        [InlineData(nameof(LoadStatic_255), 255)]
        [InlineData(nameof(LoadStatic_256), 256)]
        [InlineData(nameof(LoadInstance_0), 0)]
        [InlineData(nameof(LoadInstance_1), 1)]
        [InlineData(nameof(LoadInstance_2), 2)]
        [InlineData(nameof(LoadInstance_3), 3)]
        public void LoadTest(string m, int i)
            => AssertMethod(GetMethod(m), new ParameterExpression("a" + i, typeof(int)).ToReturnStatement());

        private ArgumentTest LoadThis() => this;

        #endregion LoadTest

        #region LoadArgumenAddresstTest

        [Fact]
        public void LoadArgumentTest_This()
            => AssertMethod(GetMethod(nameof(LoadThis)), new ThisExpression(GetType()).ToReturnStatement());

        private static string LoadAddress_255(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17, int a18, int a19, int a20, int a21, int a22, int a23, int a24, int a25, int a26, int a27, int a28, int a29, int a30, int a31, int a32, int a33, int a34, int a35, int a36, int a37, int a38, int a39, int a40, int a41, int a42, int a43, int a44, int a45, int a46, int a47, int a48, int a49, int a50, int a51, int a52, int a53, int a54, int a55, int a56, int a57, int a58, int a59, int a60, int a61, int a62, int a63, int a64, int a65, int a66, int a67, int a68, int a69, int a70, int a71, int a72, int a73, int a74, int a75, int a76, int a77, int a78, int a79, int a80, int a81, int a82, int a83, int a84, int a85, int a86, int a87, int a88, int a89, int a90, int a91, int a92, int a93, int a94, int a95, int a96, int a97, int a98, int a99, int a100, int a101, int a102, int a103, int a104, int a105, int a106, int a107, int a108, int a109, int a110, int a111, int a112, int a113, int a114, int a115, int a116, int a117, int a118, int a119, int a120, int a121, int a122, int a123, int a124, int a125, int a126, int a127, int a128, int a129, int a130, int a131, int a132, int a133, int a134, int a135, int a136, int a137, int a138, int a139, int a140, int a141, int a142, int a143, int a144, int a145, int a146, int a147, int a148, int a149, int a150, int a151, int a152, int a153, int a154, int a155, int a156, int a157, int a158, int a159, int a160, int a161, int a162, int a163, int a164, int a165, int a166, int a167, int a168, int a169, int a170, int a171, int a172, int a173, int a174, int a175, int a176, int a177, int a178, int a179, int a180, int a181, int a182, int a183, int a184, int a185, int a186, int a187, int a188, int a189, int a190, int a191, int a192, int a193, int a194, int a195, int a196, int a197, int a198, int a199, int a200, int a201, int a202, int a203, int a204, int a205, int a206, int a207, int a208, int a209, int a210, int a211, int a212, int a213, int a214, int a215, int a216, int a217, int a218, int a219, int a220, int a221, int a222, int a223, int a224, int a225, int a226, int a227, int a228, int a229, int a230, int a231, int a232, int a233, int a234, int a235, int a236, int a237, int a238, int a239, int a240, int a241, int a242, int a243, int a244, int a245, int a246, int a247, int a248, int a249, int a250, int a251, int a252, int a253, int a254, int a255) => a255.ToString();

        private static string LoadAddress_256(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17, int a18, int a19, int a20, int a21, int a22, int a23, int a24, int a25, int a26, int a27, int a28, int a29, int a30, int a31, int a32, int a33, int a34, int a35, int a36, int a37, int a38, int a39, int a40, int a41, int a42, int a43, int a44, int a45, int a46, int a47, int a48, int a49, int a50, int a51, int a52, int a53, int a54, int a55, int a56, int a57, int a58, int a59, int a60, int a61, int a62, int a63, int a64, int a65, int a66, int a67, int a68, int a69, int a70, int a71, int a72, int a73, int a74, int a75, int a76, int a77, int a78, int a79, int a80, int a81, int a82, int a83, int a84, int a85, int a86, int a87, int a88, int a89, int a90, int a91, int a92, int a93, int a94, int a95, int a96, int a97, int a98, int a99, int a100, int a101, int a102, int a103, int a104, int a105, int a106, int a107, int a108, int a109, int a110, int a111, int a112, int a113, int a114, int a115, int a116, int a117, int a118, int a119, int a120, int a121, int a122, int a123, int a124, int a125, int a126, int a127, int a128, int a129, int a130, int a131, int a132, int a133, int a134, int a135, int a136, int a137, int a138, int a139, int a140, int a141, int a142, int a143, int a144, int a145, int a146, int a147, int a148, int a149, int a150, int a151, int a152, int a153, int a154, int a155, int a156, int a157, int a158, int a159, int a160, int a161, int a162, int a163, int a164, int a165, int a166, int a167, int a168, int a169, int a170, int a171, int a172, int a173, int a174, int a175, int a176, int a177, int a178, int a179, int a180, int a181, int a182, int a183, int a184, int a185, int a186, int a187, int a188, int a189, int a190, int a191, int a192, int a193, int a194, int a195, int a196, int a197, int a198, int a199, int a200, int a201, int a202, int a203, int a204, int a205, int a206, int a207, int a208, int a209, int a210, int a211, int a212, int a213, int a214, int a215, int a216, int a217, int a218, int a219, int a220, int a221, int a222, int a223, int a224, int a225, int a226, int a227, int a228, int a229, int a230, int a231, int a232, int a233, int a234, int a235, int a236, int a237, int a238, int a239, int a240, int a241, int a242, int a243, int a244, int a245, int a246, int a247, int a248, int a249, int a250, int a251, int a252, int a253, int a254, int a255, int a256) => a256.ToString();

        [Theory]
        [InlineData(nameof(LoadAddress_255), 255)]
        [InlineData(nameof(LoadAddress_256), 256)]
        public void LoadAddresstTest(string m, int i)
            => AssertMethod(
                GetMethod(m),
                new ParameterExpression("a" + i, typeof(int))
                    .Call(typeof(int).GetMethod(nameof(ToString), Type.EmptyTypes))
                    .ToReturnStatement());

        #endregion LoadArgumenAddresstTest

        private static string Store_255(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17, int a18, int a19, int a20, int a21, int a22, int a23, int a24, int a25, int a26, int a27, int a28, int a29, int a30, int a31, int a32, int a33, int a34, int a35, int a36, int a37, int a38, int a39, int a40, int a41, int a42, int a43, int a44, int a45, int a46, int a47, int a48, int a49, int a50, int a51, int a52, int a53, int a54, int a55, int a56, int a57, int a58, int a59, int a60, int a61, int a62, int a63, int a64, int a65, int a66, int a67, int a68, int a69, int a70, int a71, int a72, int a73, int a74, int a75, int a76, int a77, int a78, int a79, int a80, int a81, int a82, int a83, int a84, int a85, int a86, int a87, int a88, int a89, int a90, int a91, int a92, int a93, int a94, int a95, int a96, int a97, int a98, int a99, int a100, int a101, int a102, int a103, int a104, int a105, int a106, int a107, int a108, int a109, int a110, int a111, int a112, int a113, int a114, int a115, int a116, int a117, int a118, int a119, int a120, int a121, int a122, int a123, int a124, int a125, int a126, int a127, int a128, int a129, int a130, int a131, int a132, int a133, int a134, int a135, int a136, int a137, int a138, int a139, int a140, int a141, int a142, int a143, int a144, int a145, int a146, int a147, int a148, int a149, int a150, int a151, int a152, int a153, int a154, int a155, int a156, int a157, int a158, int a159, int a160, int a161, int a162, int a163, int a164, int a165, int a166, int a167, int a168, int a169, int a170, int a171, int a172, int a173, int a174, int a175, int a176, int a177, int a178, int a179, int a180, int a181, int a182, int a183, int a184, int a185, int a186, int a187, int a188, int a189, int a190, int a191, int a192, int a193, int a194, int a195, int a196, int a197, int a198, int a199, int a200, int a201, int a202, int a203, int a204, int a205, int a206, int a207, int a208, int a209, int a210, int a211, int a212, int a213, int a214, int a215, int a216, int a217, int a218, int a219, int a220, int a221, int a222, int a223, int a224, int a225, int a226, int a227, int a228, int a229, int a230, int a231, int a232, int a233, int a234, int a235, int a236, int a237, int a238, int a239, int a240, int a241, int a242, int a243, int a244, int a245, int a246, int a247, int a248, int a249, int a250, int a251, int a252, int a253, int a254, int a255) => (a255 += 5).ToString(a255.ToString());

        private static string Store_256(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17, int a18, int a19, int a20, int a21, int a22, int a23, int a24, int a25, int a26, int a27, int a28, int a29, int a30, int a31, int a32, int a33, int a34, int a35, int a36, int a37, int a38, int a39, int a40, int a41, int a42, int a43, int a44, int a45, int a46, int a47, int a48, int a49, int a50, int a51, int a52, int a53, int a54, int a55, int a56, int a57, int a58, int a59, int a60, int a61, int a62, int a63, int a64, int a65, int a66, int a67, int a68, int a69, int a70, int a71, int a72, int a73, int a74, int a75, int a76, int a77, int a78, int a79, int a80, int a81, int a82, int a83, int a84, int a85, int a86, int a87, int a88, int a89, int a90, int a91, int a92, int a93, int a94, int a95, int a96, int a97, int a98, int a99, int a100, int a101, int a102, int a103, int a104, int a105, int a106, int a107, int a108, int a109, int a110, int a111, int a112, int a113, int a114, int a115, int a116, int a117, int a118, int a119, int a120, int a121, int a122, int a123, int a124, int a125, int a126, int a127, int a128, int a129, int a130, int a131, int a132, int a133, int a134, int a135, int a136, int a137, int a138, int a139, int a140, int a141, int a142, int a143, int a144, int a145, int a146, int a147, int a148, int a149, int a150, int a151, int a152, int a153, int a154, int a155, int a156, int a157, int a158, int a159, int a160, int a161, int a162, int a163, int a164, int a165, int a166, int a167, int a168, int a169, int a170, int a171, int a172, int a173, int a174, int a175, int a176, int a177, int a178, int a179, int a180, int a181, int a182, int a183, int a184, int a185, int a186, int a187, int a188, int a189, int a190, int a191, int a192, int a193, int a194, int a195, int a196, int a197, int a198, int a199, int a200, int a201, int a202, int a203, int a204, int a205, int a206, int a207, int a208, int a209, int a210, int a211, int a212, int a213, int a214, int a215, int a216, int a217, int a218, int a219, int a220, int a221, int a222, int a223, int a224, int a225, int a226, int a227, int a228, int a229, int a230, int a231, int a232, int a233, int a234, int a235, int a236, int a237, int a238, int a239, int a240, int a241, int a242, int a243, int a244, int a245, int a246, int a247, int a248, int a249, int a250, int a251, int a252, int a253, int a254, int a255, int a256) => (a256 += 5).ToString(a256.ToString());

        [Theory]
        [InlineData(nameof(Store_255))]
        [InlineData(nameof(Store_256))]
        public void StoreTest(string m)
        {
            var p = new ParameterExpression("a" + m.Split('_')[1], typeof(int));
            var rs = p.AddAssign(5.ToExpression())
                        .Call(typeof(int).GetMethod(nameof(ToString), Type.EmptyTypes))
                        .ToReturnStatement();

            AssertMethod(GetMethod(m), rs);
        }
    }
}