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

                        case 0x02: // ldarg.0
                        case 0x03: // ldarg.1
                        case 0x04: // ldarg.2
                        case 0x05: // ldarg.3
                            ret.Add(new LoadArgumentInstruction(b - 0x02));
                            break;

                        case 0x0e: // ldarg.s {index}
                            ret.Add(new LoadArgumentInstruction(bytes[++i]));
                            break;

                        case 0x14: // ldnull
                            ret.Add(new LoadNullInstruction());
                            break;

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

                        case 0x21: //ldc.i8 {num}
                            ret.Add(new LoadInt64Instruction(*(long*)(bp + i + 1)));
                            i += 8;
                            break;

                        case 0x22: //ldc.r4 {num}
                            ret.Add(new LoadSingleInstruction(*(float*)(bp + i + 1)));
                            i += 4;
                            break;

                        case 0x23: //ldc.r8 {num}
                            ret.Add(new LoadDoubleInstruction(*(double*)(bp + i + 1)));
                            i += 8;
                            break;

                        case 0x2a: // ret
                            ret.Add(new ReturnInstruction());
                            break;

                        case 0x66: // not
                            ret.Add(new NotInstruction());
                            break;

                        case 0xfe:
                            var b2 = bytes[++i];
                            switch (b2)
                            {
                                case 0x09: //ldarg {index}
                                    ret.Add(new LoadArgumentInstruction(*(int*)(bp + i + 1)));
                                    i += 4;
                                    continue;

                                default:
                                    throw new NotImplementedException();
                            }

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

        #region Delegates

        public static List<Syntax> Decompile(Delegate @delegate)
            => Decompile(@delegate.GetMethodInfo());

        #region Action

        public static List<Syntax> Decompile(Action action)
            => Decompile(action);

        public static List<Syntax> Decompile<T>(Action<T> action)
            => Decompile(action);

        public static List<Syntax> Decompile<T1, T2>(Action<T1, T2> action)
            => Decompile(action);

        public static List<Syntax> Decompile<T1, T2, T3>(Action<T1, T2, T3> action)
            => Decompile(action);

        #endregion Action

        #region Func

        public static List<Syntax> Decompile<T>(Func<T> func)
            => Decompile(func);

        public static List<Syntax> Decompile<T1, T2>(Func<T1, T2> func)
            => Decompile(func);

        public static List<Syntax> Decompile<T1, T2, T3>(Func<T1, T2, T3> func)
            => Decompile(func);

        #endregion Func

        #endregion Delegates
    }
}