using System;
using System.Collections.Generic;
using System.Reflection;
using Shipwreck.Decompiler.Instructions;

namespace Shipwreck.Decompiler
{
    public static class ILDecompiler
    {
        public static List<Instruction> Decompile(MethodBase method)
        {
            var bytes = method.GetMethodBody().GetILAsByteArray();

            var ret = new List<Instruction>(Math.Min(bytes.Length, Math.Max(4, bytes.Length >> 2)));

            for (var i = 0; i < bytes.Length; i++)
            {
                var b = bytes[i];
                switch (b)
                {
                    case 0x15: // ldc.i4.m1
                    case 0x16: // ldc.i4.0
                    case 0x17: // ldc.i4.1
                    case 0x18: // ldc.i4.2
                    case 0x19: // ldc.i4.3
                    case 0x1A: // ldc.i4.4
                    case 0x1B: // ldc.i4.5
                    case 0x1C: // ldc.i4.6
                    case 0x1D: // ldc.i4.7
                    case 0x1e: // ldc.i4.8
                        ret.Add(new LoadInt32Instruction(b - 0x16));
                        break;

                    case 0x1f: //ldc.i4.s {num}
                        ret.Add(new LoadInt32Instruction(bytes[++i]));
                        break;

                    case 0x20: //ldc.i4 {num}
                        ret.Add(new LoadInt32Instruction(bytes[++i] + (bytes[++i] << 8) + (bytes[++i] << 16) + (bytes[++i] << 24)));
                        break;

                    case 0x2a: // ret
                        ret.Add(new ReturnInstruction());
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return ret;
        }
    }
}