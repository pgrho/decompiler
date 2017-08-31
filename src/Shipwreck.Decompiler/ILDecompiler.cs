﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Shipwreck.Decompiler.Expressions;
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
                            ret.Add(new LoadArgumentInstruction(i, b - 0x02));
                            break;

                        case 0x0e: // ldarg.s {index}
                            ret.Add(new LoadArgumentInstruction(i, bytes[++i]));
                            break;

                        case 0x14: // ldnull
                            ret.Add(new LoadNullInstruction(i));
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
                            ret.Add(new LoadInt32Instruction(i, b - 0x16));
                            break;

                        case 0x1f: //ldc.i4.s {num}
                            ret.Add(new LoadInt32Instruction(i, bytes[++i]));
                            break;

                        case 0x20: //ldc.i4 {num}
                            ret.Add(new LoadInt32Instruction(i, *(int*)(bp + i + 1)));
                            i += 4;
                            break;

                        case 0x21: //ldc.i8 {num}
                            ret.Add(new LoadInt64Instruction(i, *(long*)(bp + i + 1)));
                            i += 8;
                            break;

                        case 0x22: //ldc.r4 {num}
                            ret.Add(new LoadSingleInstruction(i, *(float*)(bp + i + 1)));
                            i += 4;
                            break;

                        case 0x23: //ldc.r8 {num}
                            ret.Add(new LoadDoubleInstruction(i, *(double*)(bp + i + 1)));
                            i += 8;
                            break;

                        case 0x2a: // ret
                            ret.Add(new ReturnInstruction(i));
                            break;

                        case 0x58: // add
                        case 0xd6: // add.ovf
                        case 0xd7: // add.ovf.un
                            ret.Add(new BinaryInstruction(i, b == 0x58 ? BinaryOperator.Add : BinaryOperator.AddChecked, b == 0xd7));
                            break;

                        case 0x59: // sub
                        case 0xda: // sub.ovf
                        case 0xdb: // sub.ovf.un
                            ret.Add(new BinaryInstruction(i, b == 0x59 ? BinaryOperator.Subtract : BinaryOperator.SubtractChecked, b == 0xdb));
                            break;

                        case 0x5a: // mul
                        case 0xd8: // mul.ovf
                        case 0xd9: // mul.ovf.un
                            ret.Add(new BinaryInstruction(i, b == 0x5a ? BinaryOperator.Multiply : BinaryOperator.MultiplyChecked, b == 0xd9));
                            break;

                        case 0x5b: // div
                        case 0x5c: // div.un
                            ret.Add(new BinaryInstruction(i, BinaryOperator.Divide, b == 0x5c));
                            break;

                        case 0x65: // neg
                        case 0x66: // not
                            ret.Add(new UnaryInstruction(i, b == 0x65 ? UnaryOperator.Negate : UnaryOperator.Not));
                            break;

                        case 0x90: // ldelem.i1
                        case 0x91: // ldelem.u1
                        case 0x92: // ldelem.i2
                        case 0x93: // ldelem.u2
                        case 0x94: // ldelem.i4
                        case 0x95: // ldelem.u4
                        case 0x96: // ldelem.i8
                        case 0x97: // ldelem.i
                        case 0x98: // ldelem.r4
                        case 0x99: // ldelem.r8
                        case 0x9a: // ldelem.ref
                            ret.Add(new LoadElementInstruction(i));
                            continue;

                        case 0xfe:
                            var b2 = bytes[++i];
                            switch (b2)
                            {
                                case 0x09: //ldarg {index}
                                    ret.Add(new LoadArgumentInstruction(i, *(int*)(bp + i + 1)));
                                    i += 4;
                                    continue;

                                default:
                                    throw new NotImplementedException();
                            }

                        // TODO: OpCodes.And
                        // TODO: OpCodes.Arglist
                        // TODO: OpCodes.Beq
                        // TODO: OpCodes.Beq_S
                        // TODO: OpCodes.Bge
                        // TODO: OpCodes.Bge_S
                        // TODO: OpCodes.Bge_Un
                        // TODO: OpCodes.Bge_Un_S
                        // TODO: OpCodes.Bgt
                        // TODO: OpCodes.Bgt_S
                        // TODO: OpCodes.Bgt_Un
                        // TODO: OpCodes.Bgt_Un_S
                        // TODO: OpCodes.Ble
                        // TODO: OpCodes.Ble_S
                        // TODO: OpCodes.Ble_Un
                        // TODO: OpCodes.Ble_Un_S
                        // TODO: OpCodes.Blt
                        // TODO: OpCodes.Blt_S
                        // TODO: OpCodes.Blt_Un
                        // TODO: OpCodes.Blt_Un_S
                        // TODO: OpCodes.Bne_Un
                        // TODO: OpCodes.Bne_Un_S
                        // TODO: OpCodes.Box
                        // TODO: OpCodes.Br
                        // TODO: OpCodes.Br_S
                        // TODO: OpCodes.Break
                        // TODO: OpCodes.Brfalse
                        // TODO: OpCodes.Brfalse_S
                        // TODO: OpCodes.Brtrue
                        // TODO: OpCodes.Brtrue_S
                        // TODO: OpCodes.Call
                        // TODO: OpCodes.Calli
                        // TODO: OpCodes.Callvirt
                        // TODO: OpCodes.Castclass
                        // TODO: OpCodes.Ceq
                        // TODO: OpCodes.Cgt
                        // TODO: OpCodes.Cgt_Un
                        // TODO: OpCodes.Ckfinite
                        // TODO: OpCodes.Clt
                        // TODO: OpCodes.Clt_Un
                        // TODO: OpCodes.Constrained
                        // TODO: OpCodes.Conv_I
                        // TODO: OpCodes.Conv_I1
                        // TODO: OpCodes.Conv_I2
                        // TODO: OpCodes.Conv_I4
                        // TODO: OpCodes.Conv_I8
                        // TODO: OpCodes.Conv_Ovf_I
                        // TODO: OpCodes.Conv_Ovf_I_Un
                        // TODO: OpCodes.Conv_Ovf_I1
                        // TODO: OpCodes.Conv_Ovf_I1_Un
                        // TODO: OpCodes.Conv_Ovf_I2
                        // TODO: OpCodes.Conv_Ovf_I2_Un
                        // TODO: OpCodes.Conv_Ovf_I4
                        // TODO: OpCodes.Conv_Ovf_I4_Un
                        // TODO: OpCodes.Conv_Ovf_I8
                        // TODO: OpCodes.Conv_Ovf_I8_Un
                        // TODO: OpCodes.Conv_Ovf_U
                        // TODO: OpCodes.Conv_Ovf_U_Un
                        // TODO: OpCodes.Conv_Ovf_U1
                        // TODO: OpCodes.Conv_Ovf_U1_Un
                        // TODO: OpCodes.Conv_Ovf_U2
                        // TODO: OpCodes.Conv_Ovf_U2_Un
                        // TODO: OpCodes.Conv_Ovf_U4
                        // TODO: OpCodes.Conv_Ovf_U4_Un
                        // TODO: OpCodes.Conv_Ovf_U8
                        // TODO: OpCodes.Conv_Ovf_U8_Un
                        // TODO: OpCodes.Conv_R_Un
                        // TODO: OpCodes.Conv_R4
                        // TODO: OpCodes.Conv_R8
                        // TODO: OpCodes.Conv_U
                        // TODO: OpCodes.Conv_U1
                        // TODO: OpCodes.Conv_U2
                        // TODO: OpCodes.Conv_U4
                        // TODO: OpCodes.Conv_U8
                        // TODO: OpCodes.Cpblk
                        // TODO: OpCodes.Cpobj
                        // TODO: OpCodes.Div
                        // TODO: OpCodes.Div_Un
                        // TODO: OpCodes.Dup
                        // TODO: OpCodes.Endfilter
                        // TODO: OpCodes.Endfinally
                        // TODO: OpCodes.Initblk
                        // TODO: OpCodes.Initobj
                        // TODO: OpCodes.Isinst
                        // TODO: OpCodes.Jmp
                        // TODO: OpCodes.Ldarga
                        // TODO: OpCodes.Ldarga_S
                        // TODO: OpCodes.Ldelem
                        // TODO: OpCodes.Ldelema
                        // TODO: OpCodes.Ldfld
                        // TODO: OpCodes.Ldflda
                        // TODO: OpCodes.Ldftn
                        // TODO: OpCodes.Ldind_I
                        // TODO: OpCodes.Ldind_I1
                        // TODO: OpCodes.Ldind_I2
                        // TODO: OpCodes.Ldind_I4
                        // TODO: OpCodes.Ldind_I8
                        // TODO: OpCodes.Ldind_R4
                        // TODO: OpCodes.Ldind_R8
                        // TODO: OpCodes.Ldind_Ref
                        // TODO: OpCodes.Ldind_U1
                        // TODO: OpCodes.Ldind_U2
                        // TODO: OpCodes.Ldind_U4
                        // TODO: OpCodes.Ldlen
                        // TODO: OpCodes.Ldloc
                        // TODO: OpCodes.Ldloc_0
                        // TODO: OpCodes.Ldloc_1
                        // TODO: OpCodes.Ldloc_2
                        // TODO: OpCodes.Ldloc_3
                        // TODO: OpCodes.Ldloc_S
                        // TODO: OpCodes.Ldloca
                        // TODO: OpCodes.Ldloca_S
                        // TODO: OpCodes.Ldobj
                        // TODO: OpCodes.Ldsfld
                        // TODO: OpCodes.Ldsflda
                        // TODO: OpCodes.Ldstr
                        // TODO: OpCodes.Ldtoken
                        // TODO: OpCodes.Ldvirtftn
                        // TODO: OpCodes.Leave
                        // TODO: OpCodes.Leave_S
                        // TODO: OpCodes.Localloc
                        // TODO: OpCodes.Mkrefany
                        // TODO: OpCodes.Newarr
                        // TODO: OpCodes.Newobj
                        // TODO: OpCodes.Or
                        // TODO: OpCodes.Pop
                        // TODO: OpCodes.Prefix1
                        // TODO: OpCodes.Prefix2
                        // TODO: OpCodes.Prefix3
                        // TODO: OpCodes.Prefix4
                        // TODO: OpCodes.Prefix5
                        // TODO: OpCodes.Prefix6
                        // TODO: OpCodes.Prefix7
                        // TODO: OpCodes.Prefixref
                        // TODO: OpCodes.Readonly
                        // TODO: OpCodes.Refanytype
                        // TODO: OpCodes.Refanyval
                        // TODO: OpCodes.Rem
                        // TODO: OpCodes.Rem_Un
                        // TODO: OpCodes.Ret
                        // TODO: OpCodes.Rethrow
                        // TODO: OpCodes.Shl
                        // TODO: OpCodes.Shr
                        // TODO: OpCodes.Shr_Un
                        // TODO: OpCodes.Sizeof
                        // TODO: OpCodes.Starg
                        // TODO: OpCodes.Starg_S
                        // TODO: OpCodes.Stelem
                        // TODO: OpCodes.Stelem_I
                        // TODO: OpCodes.Stelem_I1
                        // TODO: OpCodes.Stelem_I2
                        // TODO: OpCodes.Stelem_I4
                        // TODO: OpCodes.Stelem_I8
                        // TODO: OpCodes.Stelem_R4
                        // TODO: OpCodes.Stelem_R8
                        // TODO: OpCodes.Stelem_Ref
                        // TODO: OpCodes.Stfld
                        // TODO: OpCodes.Stind_I
                        // TODO: OpCodes.Stind_I1
                        // TODO: OpCodes.Stind_I2
                        // TODO: OpCodes.Stind_I4
                        // TODO: OpCodes.Stind_I8
                        // TODO: OpCodes.Stind_R4
                        // TODO: OpCodes.Stind_R8
                        // TODO: OpCodes.Stind_Ref
                        // TODO: OpCodes.Stloc
                        // TODO: OpCodes.Stloc_0
                        // TODO: OpCodes.Stloc_1
                        // TODO: OpCodes.Stloc_2
                        // TODO: OpCodes.Stloc_3
                        // TODO: OpCodes.Stloc_S
                        // TODO: OpCodes.Stobj
                        // TODO: OpCodes.Stsfld
                        // TODO: OpCodes.Switch
                        // TODO: OpCodes.Tailcall
                        // TODO: OpCodes.Throw
                        // TODO: OpCodes.Unaligned
                        // TODO: OpCodes.Unbox
                        // TODO: OpCodes.Unbox_Any
                        // TODO: OpCodes.Volatile
                        // TODO: OpCodes.Xor

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