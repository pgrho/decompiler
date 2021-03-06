﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Shipwreck.CSharpModels.Expressions;
using Shipwreck.Decompiler.Instructions;
using Shipwreck.CSharpModels.Statements;

namespace Shipwreck.Decompiler
{
    public static class ILDecompiler
    {
        public static unsafe List<Instruction> DecompileToInstructions(MethodBase method)
        {
            var ctx = new DecompilationContext(method);
            var l = new List<Instruction>();
            var bytes = method.GetMethodBody().GetILAsByteArray();
            fixed (byte* bp = bytes)
            {
                for (var i = 0; i < bytes.Length; i++)
                {
                    l.Add(GetInstruction(ctx, bp, ref i));
                }
            }

            return l;
        }

        // TODO: change return type to DecompiledMethod
        public static unsafe List<Statement> Decompile(MethodBase method)
        {
            var ctx = new DecompilationContext(method);

            // Parse IL instructions
            var bytes = method.GetMethodBody().GetILAsByteArray();
            fixed (byte* bp = bytes)
            {
                for (var i = 0; i < bytes.Length; i++)
                {
                    var offset = i;
                    var il = GetInstruction(ctx, bp, ref i);

                    if (il != null && !(il is IIgnorableInstruction))
                    {
                        ctx.RootStatements.Add(il);
                        ctx.SetOffset(il, offset);
                    }
                }
            }

            // Collect flow information
            for (var i = 0; i < ctx.RootStatements.Count - 1; i++)
            {
                ((Instruction)ctx.RootStatements[i]).SetTo(ctx, i);
            }

            // Translate Instruction to Statement.
            bool transformed;
            do
            {
                transformed = false;
                for (var i = 0; i < ctx.RootStatements.Count; i++)
                {
                    var il = ctx.RootStatements[i] as Instruction;
                    if (il != null)
                    {
                        int s = i, e = i;
                        if (il.TryCreateStatement(ctx, ref s, ref e, out var st))
                        {
                            Debug.Assert(s <= i);
                            Debug.Assert(e >= i);

                            var offset = ctx.GetOffset(s);
                            ctx.ReplaceInstructionFlow(st, ctx.RootStatements.Skip(s).Take(e - s + 1));
                            if (st != null)
                            {
                                ctx.RootStatements.RemoveRange(s + 1, e - s);
                                ctx.RootStatements[s] = st;
                                ctx.SetOffset(st, offset);
                            }
                            else
                            {
                                ctx.RootStatements.RemoveRange(s, e - s + 1);
                            }

                            transformed = true;
                            break;
                        }
                    }
                }
            } while (transformed);

            // insert labels
            foreach (var s in ctx.RootStatements.ToArray())
            {
                if (s is TemporalGoToStatement g)
                {
                    var gg = ReplaceGoToStatement(ctx, g);
                    ctx.RootStatements[ctx.RootStatements.IndexOf(g)] = gg;
                }
                else if (s is IfStatement ib)
                {
                    for (int i = 0; i < ib.TruePart.Count; i++)
                    {
                        if (ib.TruePart[i] is TemporalGoToStatement cg)
                        {
                            var gg = ReplaceGoToStatement(ctx, cg);
                            ib.TruePart[i] = gg;
                        }
                    }
                }
                else if (s is SwitchStatement ss)
                {
                    foreach (var sec in ss.Sections)
                    {
                        for (int i = 0; i < sec.Statements.Count; i++)
                        {
                            if (sec.Statements[i] is TemporalGoToStatement cg)
                            {
                                var gg = ReplaceGoToStatement(ctx, cg);
                                sec.Statements[i] = gg;
                            }
                        }
                    }
                }
            }

            InsertTryBlocks(ctx);

            if (ctx.RootStatements.OfType<Instruction>().Any())
            {
                throw new InvalidOperationException("Cannot translate il instructions to statements");
            }

            var dm = new DecompiledMethod();
            dm.RootStatements.AddRange(ctx.RootStatements.Cast<Statement>());

            do
            {
                transformed = dm.RootStatements.ReduceBlock();
            } while (transformed);

            // TODO: reduce variable scope

            return dm.RootStatements.ToList();
        }

        private static unsafe void InsertTryBlocks(DecompilationContext context)
        {
            foreach (var c in context.Method.GetMethodBody()
                                    .ExceptionHandlingClauses
                                    .OrderByDescending(cc => cc.TryOffset)
                                    .ThenBy(cc => cc.TryLength)
                                    .ThenBy(cc => cc.HandlerOffset))
            {
                var first = context.GetSyntaxAt(c.TryOffset);
                var tryIndex = context.RootStatements.IndexOf(first);
                var nextIndex = context.RootStatements.IndexOf(context.GetSyntaxAt(c.TryOffset + c.TryLength));
                var tb = first as TryStatement;
                if (tb == null || tryIndex + 1 < nextIndex)
                {
                    //var sts = ctx.RootStatements.Skip(fi).Take(li - fi).SkipWhile(s => s is PopInstruction).ToArray();
                    var sts = context.RootStatements.Skip(tryIndex).Take(nextIndex - tryIndex).ToArray();

                    tb = new TryStatement();
                    tb.Block.AddRange(sts.Cast<Statement>());

                    context.RootStatements.RemoveRange(tryIndex, nextIndex - tryIndex);
                    context.RootStatements[tryIndex] = tb;
                    context.SetOffset(tb, c.TryOffset);
                }

                {
                    var fi = context.RootStatements.IndexOf(context.GetSyntaxAt(c.HandlerOffset));
                    var li = context.RootStatements.IndexOf(context.GetSyntaxAt(c.HandlerOffset + c.HandlerLength));

                    var sts = context.RootStatements.Skip(fi).Take(li - fi).SkipWhile(s => s is PopInstruction).Cast<Statement>().ToArray();
                    context.RootStatements.RemoveRange(fi, li - fi);

                    if (c.Flags == ExceptionHandlingClauseOptions.Finally)
                    {
                        tb.Finally.AddRange(sts);
                    }
                    else
                    {
                        var cc = new CatchClause(tb, c.CatchType);
                        cc.Statements.AddRange(sts);

                        tb.CatchClauses.Add(cc);
                    }
                }
            }
        }

        private static GoToStatement ReplaceGoToStatement(DecompilationContext ctx, TemporalGoToStatement g)
        {
            var tg = ctx.GetSyntaxAt(g.Target);

            var lb = tg as LabelTarget;
            if (lb == null)
            {
                lb = new LabelTarget($"L_{g.Target:x4}");

                var i = tg == null ? -1 : ctx.RootStatements.IndexOf(tg);
                if (i < 0)
                {
                    ctx.RootStatements.Add(lb);
                }
                else
                {
                    ctx.RootStatements.Insert(i, lb);
                }

                ctx.SetOffset(lb, g.Target);
            }

            return new GoToStatement(lb);
        }

        private unsafe static Instruction GetInstruction(DecompilationContext context, byte* bp, ref int i)
        {
            var b = bp[i];
            switch (b)
            {
                case 0x00: // nop
                    return IgnorableInstruction.Nop;

                case 0x01: // break
                    return BreakInstruction.Default;

                case 0x02: // ldarg.0
                case 0x03: // ldarg.1
                case 0x04: // ldarg.2
                case 0x05: // ldarg.3
                    return LoadArgumentInstruction.GetMacro(b - 0x02);

                case 0x06: // ldloc.0
                case 0x07: // ldloc.1
                case 0x08: // ldloc.2
                case 0x09: // ldloc.3
                    return new LoadLocalInstruction(b - 0x06);

                case 0x0a: // stloc.0
                case 0x0b: // stloc.1
                case 0x0c: // stloc.2
                case 0x0d: // stloc.3
                    return new StoreLocalInstruction(b - 0x0a);

                case 0x0e: // ldarg.s {index}
                    return new LoadArgumentInstruction(bp[++i], 1);

                case 0x0f: // ldarga.s {index}
                    return new LoadArgumentAddressInstruction(bp[++i]);

                case 0x10: // starg.s {index}
                    return new StoreArgumentInstruction(bp[++i]);

                case 0x11: // ldloc.s {index}
                    return new LoadLocalInstruction(bp[++i]);

                case 0x12: // ldloca.s {index}
                    return new LoadLocalAddressInstruction(bp[++i]);

                case 0x13: // stloc.s {index}
                    return new StoreLocalInstruction(bp[++i]);

                case 0x14: // ldnull
                    return new LoadNullInstruction();

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
                    return new LoadInt32Instruction(b - 0x16);

                case 0x1f: //ldc.i4.s {num}
                    return new LoadInt32Instruction((sbyte)bp[++i]);

                case 0x20: //ldc.i4 {num}
                    i += 4;
                    return new LoadInt32Instruction(*(int*)(bp + i - 3));

                case 0x21: //ldc.i8 {num}
                    i += 8;
                    return new LoadInt64Instruction(*(long*)(bp + i - 7));

                case 0x22: //ldc.r4 {num}
                    i += 4;
                    return new LoadSingleInstruction(*(float*)(bp + i - 3));

                case 0x23: //ldc.r8 {num}
                    i += 8;
                    return new LoadDoubleInstruction(*(double*)(bp + i - 7));

                case 0x25: // dup
                    return new DuplicateInstruction();

                case 0x26: // pop
                    return new PopInstruction();

                case 0x28: // call
                case 0x29: // calli
                    i += 4;
                    return new CallInstruction(context.Method.Module.ResolveMethod(*(int*)(bp + i - 3)), false);

                case 0x2a: // ret
                    return new ReturnInstruction();

                case 0x2b: // br.s {num}
                case 0x2c: // br.false.s {num}
                case 0x2d: // br.true.s {num}
                    return new BranchInstruction(i + 2 + (sbyte)bp[++i], b == 0x2b ? (bool?)null : b != 0x2c);

                case 0x2e: // beq
                case 0x2f: // bge
                case 0x30: // bgt
                case 0x31: // ble
                case 0x32: // blt
                case 0x33: // bne.un
                case 0x34: // bge.un
                case 0x35: // bgt.un
                case 0x36: // ble.un
                case 0x37: // blt.un
                    {
                        var op = b == 0x2e ? BinaryOperator.Equal
                                : b == 0x33 ? BinaryOperator.NotEqual
                                : b == 0x2f || b == 0x34 ? BinaryOperator.GreaterThanOrEqual
                                : b == 0x30 || b == 0x35 ? BinaryOperator.GreaterThan
                                : b == 0x31 || b == 0x36 ? BinaryOperator.LessThanOrEqual
                                : BinaryOperator.LessThan;

                        var u = b >= 0x33;

                        return new BranchBinaryInstruction(bp[++i], op, u);
                    }

                case 0x38: // br {num}
                case 0x39: // br.false {num}
                case 0x3a: // br.true {num}
                    i += 4;
                    return new BranchInstruction(i + 1 + *(int*)(bp + i - 3), b == 0x38 ? (bool?)null : b != 0x39);

                case 0x3b: // beq
                case 0x3c: // bge
                case 0x3d: // bgt
                case 0x3e: // ble
                case 0x3f: // blt
                case 0x40: // bne.un
                case 0x41: // bge.un
                case 0x42: // bgt.un
                case 0x43: // ble.un
                case 0x44: // blt.un
                    {
                        i += 4;
                        var t = i + 1 + *(int*)(bp + i - 3);

                        var op = b == 0x3b ? BinaryOperator.Equal
                                : b == 0x40 ? BinaryOperator.NotEqual
                                : b == 0x3c || b == 0x41 ? BinaryOperator.GreaterThanOrEqual
                                : b == 0x3d || b == 0x42 ? BinaryOperator.GreaterThan
                                : b == 0x3e || b == 0x43 ? BinaryOperator.LessThanOrEqual
                                : BinaryOperator.LessThan;

                        var u = b >= 0x40;

                        return new BranchBinaryInstruction(t, op, u);
                    }
                case 0x45: // switch {N} {t1} {t2}...{tN}
                    {
                        i++;
                        var n = *(int*)(bp + i);

                        var ba = i + (n + 1) * 4;

                        var targets = new int[n];

                        for (var j = 0; j < n; j++)
                        {
                            i += 4;
                            targets[j] = ba + *(int*)(bp + i);
                        }
                        i += 3;

                        return new SwitchInstruction(targets);
                    }

                case 0x58: // add
                case 0xd6: // add.ovf
                case 0xd7: // add.ovf.un
                    return new BinaryInstruction(b == 0x58 ? BinaryOperator.Add : BinaryOperator.AddChecked, b == 0xd7);

                case 0x59: // sub
                case 0xda: // sub.ovf
                case 0xdb: // sub.ovf.un
                    return new BinaryInstruction(b == 0x59 ? BinaryOperator.Subtract : BinaryOperator.SubtractChecked, b == 0xdb);

                case 0x5a: // mul
                case 0xd8: // mul.ovf
                case 0xd9: // mul.ovf.un
                    return new BinaryInstruction(b == 0x5a ? BinaryOperator.Multiply : BinaryOperator.MultiplyChecked, b == 0xd9);

                case 0x5b: // div
                case 0x5c: // div.un
                    return new BinaryInstruction(BinaryOperator.Divide, b == 0x5c);

                case 0x5d: // rem
                case 0x5e: // rem.un
                    return new BinaryInstruction(BinaryOperator.Modulo, b == 0x5e);

                case 0x5f: // and
                    return new BinaryInstruction(BinaryOperator.And, false);

                case 0x60: // or
                    return new BinaryInstruction(BinaryOperator.Or, false);

                case 0x61: // xor
                    return new BinaryInstruction(BinaryOperator.ExclusiveOr, false);

                case 0x62: // shl
                    return new BinaryInstruction(BinaryOperator.LeftShift, false);

                case 0x63: // shr
                case 0x64: // shr.un
                    return new BinaryInstruction(BinaryOperator.RightShift, b == 0x64);

                case 0x65: // neg
                case 0x66: // not
                    return new UnaryInstruction(b == 0x65 ? UnaryOperator.UnaryNegation : UnaryOperator.OnesComplement);

                case 0x67: // conv.i1
                    return new ConvertInstruction(typeof(sbyte), false);

                case 0x68: // conv.i2
                    return new ConvertInstruction(typeof(short), false);

                case 0x69: // conv.i4
                    return new ConvertInstruction(typeof(int), false);

                case 0x6a: // conv.i8
                    return new ConvertInstruction(typeof(long), false);

                case 0x6b: // conv.r4
                    return new ConvertInstruction(typeof(float), false);

                case 0x6c: // conv.r8
                    return new ConvertInstruction(typeof(double), false);

                case 0x6d: // conv.u4
                    return new ConvertInstruction(typeof(uint), false);

                case 0x6e: // conv.u8
                    return new ConvertInstruction(typeof(ulong), false);

                case 0x6f: // callvirt
                    i += 4;
                    return new CallInstruction(context.Method.Module.ResolveMethod(*(int*)(bp + i - 3)), true);

                case 0x72: //ldstr {value}
                    i += 4;
                    return new LoadStringInstruction(context.Method.Module.ResolveString(*(int*)(bp + i - 3)));

                case 0x73: // newobj {.ctor}
                    i += 4;
                    return new NewObjectInstruction((ConstructorInfo)context.Method.Module.ResolveMethod(*(int*)(bp + i - 3)));

                case 0x76: // conv.r.un
                    return new ConvertInstruction(typeof(double), false, true);

                case 0x7b: // ldfld
                    i += 4;
                    return new LoadFieldInstruction(context.Method.Module.ResolveField(*(int*)(bp + i - 3)));

                case 0x7c: // ldflda
                    i += 4;
                    return new LoadFieldAddressInstruction(context.Method.Module.ResolveField(*(int*)(bp + i - 3)));

                case 0x7e: // ldsfld
                    i += 4;
                    return new LoadStaticFieldInstruction(context.Method.Module.ResolveField(*(int*)(bp + i - 3)));

                case 0x7d: // stfld
                    i += 4;
                    return new StoreFieldInstruction(context.Method.Module.ResolveField(*(int*)(bp + i - 3)));

                case 0x7f: // ldsflda
                    i += 4;
                    return new LoadStaticFieldAddressInstruction(context.Method.Module.ResolveField(*(int*)(bp + i - 3)));

                case 0x80: // stsfld
                    i += 4;
                    return new StoreStaticFieldInstruction(context.Method.Module.ResolveField(*(int*)(bp + i - 3)));

                case 0x82: // conv.ovf.i1.un
                    return new ConvertInstruction(typeof(sbyte), true, true);

                case 0x83: // conv.ovf.i2.un
                    return new ConvertInstruction(typeof(short), true, true);

                case 0x84: // conv.ovf.i4.un
                    return new ConvertInstruction(typeof(int), true, true);

                case 0x85: // conv.ovf.i8.un
                    return new ConvertInstruction(typeof(long), true, true);

                case 0x86: // conv.ovf.u1.un
                    return new ConvertInstruction(typeof(byte), true, true);

                case 0x87: // conv.ovf.u2.un
                    return new ConvertInstruction(typeof(ushort), true, true);

                case 0x88: // conv.ovf.u4.un
                    return new ConvertInstruction(typeof(uint), true, true);

                case 0x89: // conv.ovf.u8.un
                    return new ConvertInstruction(typeof(ulong), true, true);

                case 0x8a: // conv.ovf.i.un
                    return new ConvertInstruction(typeof(IntPtr), true, true);

                case 0x8b: // conv.ovf.u.un
                    return new ConvertInstruction(typeof(UIntPtr), true, true);

                case 0x8d: // newarr {type}
                    i += 4;
                    return new NewArrayInstruction(context.Method.Module.ResolveType(*(int*)(bp + i - 3)));

                case 0x8e: // ldlen
                    return new LoadLengthInstruction();

                case 0x8f:// ldelema {type}
                    i += 4;
                    return new LoadElementAddressInstruction();

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
                    return new LoadElementInstruction();

                case 0x9b: // stelem.i
                case 0x9c: // stelem.i1
                case 0x9d: // stelem.i2
                case 0x9e: // stelem.i4
                case 0x9f: // stelem.i8
                case 0xa0: // stelem.r4
                case 0xa1: // stelem.r8
                case 0xa2: // stelem.ref
                    return new StoreElementInstruction();

                case 0xa3: // ldelem {type}
                           // method.Module.ResolveType( *(int*)(bp + i + 1))
                    i += 4;
                    return new LoadElementInstruction();

                case 0xa4: // stelem {type}
                           // method.Module.ResolveType( *(int*)(bp + i + 1))
                    i += 4;
                    return new StoreElementInstruction();

                case 0xb3: // conv.ovf.i1
                    return new ConvertInstruction(typeof(sbyte), true);

                case 0xb4: // conv.ovf.u1
                    return new ConvertInstruction(typeof(byte), true);

                case 0xb5: // conv.ovf.i2
                    return new ConvertInstruction(typeof(short), true);

                case 0xb6: // conv.ovf.u2
                    return new ConvertInstruction(typeof(ushort), true);

                case 0xb7: // conv.ovf.14
                    return new ConvertInstruction(typeof(int), true);

                case 0xb8: // conv.ovf.u4
                    return new ConvertInstruction(typeof(uint), true);

                case 0xb9: // conv.ovf.i8
                    return new ConvertInstruction(typeof(long), true);

                case 0xba: // conv.ovf.u8
                    return new ConvertInstruction(typeof(ulong), true);

                case 0xd1: // conv.u2
                    return new ConvertInstruction(typeof(ushort), false);

                case 0xd2: // conv.u1
                    return new ConvertInstruction(typeof(byte), false);

                case 0xd3: // conv.i
                case 0xd4: // conv.ovf.i
                case 0xd5: // conv.ovf.u
                    return new ConvertInstruction(b == 0xd5 ? typeof(UIntPtr) : typeof(IntPtr), b != 0xd3);

                case 0xdc: // endfinally
                    return IgnorableInstruction.EndFinally;

                case 0xdd: // leave {num}
                    i += 4;
                    return new LeaveInstruction(i + 1 + *(int*)(bp + i - 3));

                case 0xde: // leave.s {num}
                    return new LeaveInstruction(i + 2 + (sbyte)bp[++i]);

                case 0xe0: // conv.u
                    return new ConvertInstruction(typeof(ulong), false);

                case 0xfe:
                    var b2 = bp[++i];
                    switch (b2)
                    {
                        case 0x01: // ceq
                            return new BinaryInstruction(BinaryOperator.Equal);

                        case 0x02: // cgt
                        case 0x03: // cgt.un
                            return new BinaryInstruction(BinaryOperator.GreaterThan, b2 == 0x03);

                        case 0x04: // clt
                        case 0x05: // clt.un
                            return new BinaryInstruction(BinaryOperator.LessThan, b2 == 0x05);

                        case 0x09: //ldarg {index}
                            i += 2;
                            return new LoadArgumentInstruction(*(ushort*)(bp + i - 1));

                        case 0x0a: //ldarga {index}
                            i += 2;
                            return new LoadArgumentAddressInstruction(*(ushort*)(bp + i - 1));

                        case 0x0b: //starg {index}
                            i += 2;
                            return new StoreArgumentInstruction(*(ushort*)(bp + i - 1));

                        case 0x0c: //ldloc {index}
                            i += 2;
                            return new LoadLocalInstruction(*(ushort*)(bp + i - 1));

                        case 0x0d: //ldloca {index}
                            i += 2;
                            return new LoadLocalAddressInstruction(*(ushort*)(bp + i - 1));

                        case 0x0e: // stloc {index}
                            i += 2;
                            return new StoreLocalInstruction(*(ushort*)(bp + i - 1));

                        case 0x15: // initobj {type}
                            i += 4;
                            return new InitObjectInstruction(context.Method.Module.ResolveType(*(int*)(bp + i - 3)));

                        case 0x16: // constrained {type}
                            i += 4;
                            return new IgnorableTypeInstruction(0xfe16, "constrained", context.Method.Module.ResolveType(*(int*)(bp + i - 3)));

                        default:
                            throw new NotImplementedException($"Invalid IL '{b:x2} {b2:x2}'");
                    }

                // TODO: OpCodes.Arglist
                // TODO: OpCodes.Box
                // TODO: OpCodes.Break
                // TODO: OpCodes.Castclass
                // TODO: OpCodes.Ckfinite
                // TODO: OpCodes.Cpblk
                // TODO: OpCodes.Cpobj
                // TODO: OpCodes.Endfilter
                // TODO: OpCodes.Initblk
                // TODO: OpCodes.Isinst
                // TODO: OpCodes.Jmp
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
                // TODO: OpCodes.Ldobj
                // TODO: OpCodes.Ldtoken
                // TODO: OpCodes.Ldvirtftn
                // TODO: OpCodes.Localloc
                // TODO: OpCodes.Mkrefany
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
                // TODO: OpCodes.Rethrow
                // TODO: OpCodes.Sizeof
                // TODO: OpCodes.Stind_I
                // TODO: OpCodes.Stind_I1
                // TODO: OpCodes.Stind_I2
                // TODO: OpCodes.Stind_I4
                // TODO: OpCodes.Stind_I8
                // TODO: OpCodes.Stind_R4
                // TODO: OpCodes.Stind_R8
                // TODO: OpCodes.Stind_Ref
                // TODO: OpCodes.Stobj
                // TODO: OpCodes.Tailcall
                // TODO: OpCodes.Throw
                // TODO: OpCodes.Unaligned
                // TODO: OpCodes.Unbox
                // TODO: OpCodes.Unbox_Any
                // TODO: OpCodes.Volatile

                default:
                    throw new NotImplementedException($"Invalid IL '{b:x2}'");
            }
        }
    }
}