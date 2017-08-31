using System;
using System.Collections.Generic;
using System.Reflection;
using Shipwreck.Decompiler.Instructions;

namespace Shipwreck.Decompiler
{
    public static class ILDecompiler
    {
        public static unsafe List<Syntax> Decompile(MethodBase method)
        {
            var bytes = method.GetMethodBody().GetILAsByteArray();

            var ret = new List<Syntax>(Math.Min(bytes.Length, Math.Max(4, bytes.Length >> 2)));
            fixed (byte* bp = bytes)
            {
                for (var i = 0; i < bytes.Length; i++)
                {
                    var b = bytes[i];
                    switch (b)
                    {
                        case 0x00: // nop
                            continue;

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
                            ret.Add(new LoadInt32Instruction(*(int*)(bp + i + 1)));
                            i += 4;
                            break;

                        case 0x2a: // ret
                            ret.Add(new ReturnInstruction());
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            bool transformed;
            do
            {
                transformed = false;
                for (var i = 0; i < ret.Count; i++)
                {
                    var il = ret[i] as Instruction;
                    if (il != null)
                    {
                        int s = i, e = i;

                        if (il.TryCreateStatement(method, ret, ref s, ref e, out var st))
                        {
                            ret.RemoveRange(s, e - s + 1);
                            ret.Insert(s, st);
                            transformed = true;
                            break;
                        }
                    }
                }
            } while (transformed);

            return ret;
        }
    }
}