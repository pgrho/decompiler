namespace Shipwreck.Decompiler.Statements
{
    internal sealed class TemporalGoToStatement : Statement
    {
        public TemporalGoToStatement(int target)
        {
            Target = target;
        }

        public int Target { get; }

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is TemporalGoToStatement gt && Target == gt.Target);

        public override Statement Clone()
            => new TemporalGoToStatement(Target);

        #region AcceptVisitor

        public override void AcceptVisitor(IStatementVisitor visitor)
    => visitor.VisitGoToStatement(ToGoToStatement());

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitGoToStatement(ToGoToStatement());

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitGoToStatement(ToGoToStatement(), parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitGoToStatement(ToGoToStatement(), parameter);

        private GoToStatement ToGoToStatement()
            => new GoToStatement(new LabelTarget($"L_{Target:x4}"));

        #endregion AcceptVisitor
    }
}