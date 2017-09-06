using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class LocalVariableTest : ILDecompilerTestBase
    {
        public LocalVariableTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        #region Store

        public static int StoreLocal(int a)
        {
            var b = a;
            return b + 30;
        }

        public static int StoreLocal_Expression(int a)
        {
            var b = a;
            return (b += 30) * b;
        }

        #endregion Store

        private static int LoadLocalAddressShort()
        {
            if (int.TryParse("0", out var v))
            {
                return v;
            }
            return 0;
        }

        private static int LoadLocalAddress()
        {
            var l0 = int.MinValue; var l1 = l0 + 1; var l2 = l1 + 2; var l3 = l2 + 3; var l4 = l3 + 4; var l5 = l4 + 5; var l6 = l5 + 6; var l7 = l6 + 7; var l8 = l7 + 8; var l9 = l8 + 9; var l10 = l9 + 10; var l11 = l10 + 11; var l12 = l11 + 12; var l13 = l12 + 13; var l14 = l13 + 14; var l15 = l14 + 15; var l16 = l15 + 16; var l17 = l16 + 17; var l18 = l17 + 18; var l19 = l18 + 19; var l20 = l19 + 20; var l21 = l20 + 21; var l22 = l21 + 22; var l23 = l22 + 23; var l24 = l23 + 24; var l25 = l24 + 25; var l26 = l25 + 26; var l27 = l26 + 27; var l28 = l27 + 28; var l29 = l28 + 29; var l30 = l29 + 30; var l31 = l30 + 31; var l32 = l31 + 32; var l33 = l32 + 33; var l34 = l33 + 34; var l35 = l34 + 35; var l36 = l35 + 36; var l37 = l36 + 37; var l38 = l37 + 38; var l39 = l38 + 39; var l40 = l39 + 40; var l41 = l40 + 41; var l42 = l41 + 42; var l43 = l42 + 43; var l44 = l43 + 44; var l45 = l44 + 45; var l46 = l45 + 46; var l47 = l46 + 47; var l48 = l47 + 48; var l49 = l48 + 49; var l50 = l49 + 50; var l51 = l50 + 51; var l52 = l51 + 52; var l53 = l52 + 53; var l54 = l53 + 54; var l55 = l54 + 55; var l56 = l55 + 56; var l57 = l56 + 57; var l58 = l57 + 58; var l59 = l58 + 59; var l60 = l59 + 60; var l61 = l60 + 61; var l62 = l61 + 62; var l63 = l62 + 63; var l64 = l63 + 64; var l65 = l64 + 65; var l66 = l65 + 66; var l67 = l66 + 67; var l68 = l67 + 68; var l69 = l68 + 69; var l70 = l69 + 70; var l71 = l70 + 71; var l72 = l71 + 72; var l73 = l72 + 73; var l74 = l73 + 74; var l75 = l74 + 75; var l76 = l75 + 76; var l77 = l76 + 77; var l78 = l77 + 78; var l79 = l78 + 79; var l80 = l79 + 80; var l81 = l80 + 81; var l82 = l81 + 82; var l83 = l82 + 83; var l84 = l83 + 84; var l85 = l84 + 85; var l86 = l85 + 86; var l87 = l86 + 87; var l88 = l87 + 88; var l89 = l88 + 89; var l90 = l89 + 90; var l91 = l90 + 91; var l92 = l91 + 92; var l93 = l92 + 93; var l94 = l93 + 94; var l95 = l94 + 95; var l96 = l95 + 96; var l97 = l96 + 97; var l98 = l97 + 98; var l99 = l98 + 99; var l100 = l99 + 100; var l101 = l100 + 101; var l102 = l101 + 102; var l103 = l102 + 103; var l104 = l103 + 104; var l105 = l104 + 105; var l106 = l105 + 106; var l107 = l106 + 107; var l108 = l107 + 108; var l109 = l108 + 109; var l110 = l109 + 110; var l111 = l110 + 111; var l112 = l111 + 112; var l113 = l112 + 113; var l114 = l113 + 114; var l115 = l114 + 115; var l116 = l115 + 116; var l117 = l116 + 117; var l118 = l117 + 118; var l119 = l118 + 119; var l120 = l119 + 120; var l121 = l120 + 121; var l122 = l121 + 122; var l123 = l122 + 123; var l124 = l123 + 124; var l125 = l124 + 125; var l126 = l125 + 126; var l127 = l126 + 127; var l128 = l127 + 128; var l129 = l128 + 129; var l130 = l129 + 130; var l131 = l130 + 131; var l132 = l131 + 132; var l133 = l132 + 133; var l134 = l133 + 134; var l135 = l134 + 135; var l136 = l135 + 136; var l137 = l136 + 137; var l138 = l137 + 138; var l139 = l138 + 139; var l140 = l139 + 140; var l141 = l140 + 141; var l142 = l141 + 142; var l143 = l142 + 143; var l144 = l143 + 144; var l145 = l144 + 145; var l146 = l145 + 146; var l147 = l146 + 147; var l148 = l147 + 148; var l149 = l148 + 149; var l150 = l149 + 150; var l151 = l150 + 151; var l152 = l151 + 152; var l153 = l152 + 153; var l154 = l153 + 154; var l155 = l154 + 155; var l156 = l155 + 156; var l157 = l156 + 157; var l158 = l157 + 158; var l159 = l158 + 159; var l160 = l159 + 160; var l161 = l160 + 161; var l162 = l161 + 162; var l163 = l162 + 163; var l164 = l163 + 164; var l165 = l164 + 165; var l166 = l165 + 166; var l167 = l166 + 167; var l168 = l167 + 168; var l169 = l168 + 169; var l170 = l169 + 170; var l171 = l170 + 171; var l172 = l171 + 172; var l173 = l172 + 173; var l174 = l173 + 174; var l175 = l174 + 175; var l176 = l175 + 176; var l177 = l176 + 177; var l178 = l177 + 178; var l179 = l178 + 179; var l180 = l179 + 180; var l181 = l180 + 181; var l182 = l181 + 182; var l183 = l182 + 183; var l184 = l183 + 184; var l185 = l184 + 185; var l186 = l185 + 186; var l187 = l186 + 187; var l188 = l187 + 188; var l189 = l188 + 189; var l190 = l189 + 190; var l191 = l190 + 191; var l192 = l191 + 192; var l193 = l192 + 193; var l194 = l193 + 194; var l195 = l194 + 195; var l196 = l195 + 196; var l197 = l196 + 197; var l198 = l197 + 198; var l199 = l198 + 199; var l200 = l199 + 200; var l201 = l200 + 201; var l202 = l201 + 202; var l203 = l202 + 203; var l204 = l203 + 204; var l205 = l204 + 205; var l206 = l205 + 206; var l207 = l206 + 207; var l208 = l207 + 208; var l209 = l208 + 209; var l210 = l209 + 210; var l211 = l210 + 211; var l212 = l211 + 212; var l213 = l212 + 213; var l214 = l213 + 214; var l215 = l214 + 215; var l216 = l215 + 216; var l217 = l216 + 217; var l218 = l217 + 218; var l219 = l218 + 219; var l220 = l219 + 220; var l221 = l220 + 221; var l222 = l221 + 222; var l223 = l222 + 223; var l224 = l223 + 224; var l225 = l224 + 225; var l226 = l225 + 226; var l227 = l226 + 227; var l228 = l227 + 228; var l229 = l228 + 229; var l230 = l229 + 230; var l231 = l230 + 231; var l232 = l231 + 232; var l233 = l232 + 233; var l234 = l233 + 234; var l235 = l234 + 235; var l236 = l235 + 236; var l237 = l236 + 237; var l238 = l237 + 238; var l239 = l238 + 239; var l240 = l239 + 240; var l241 = l240 + 241; var l242 = l241 + 242; var l243 = l242 + 243; var l244 = l243 + 244; var l245 = l244 + 245; var l246 = l245 + 246; var l247 = l246 + 247; var l248 = l247 + 248; var l249 = l248 + 249; var l250 = l249 + 250; var l251 = l250 + 251; var l252 = l251 + 252; var l253 = l252 + 253; var l254 = l253 + 254; var l255 = l254 + 255; var l256 = l255 + 256;

            if (int.TryParse("0", out l256))
            {
                return l256;
            }
            return 0;
        }

        [Theory]
        [InlineData(nameof(StoreLocal))]
        [InlineData(nameof(StoreLocal_Expression))]
        [InlineData(nameof(LoadLocalAddressShort))]
        [InlineData(nameof(LoadLocalAddress))]
        public void NotImplementedTest(string m)
            => AssertMethod(GetMethod(m));
    }
}