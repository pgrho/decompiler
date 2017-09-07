using System;

namespace Shipwreck.CSharpModels.Statements
{
    public sealed class SwitchSectionCollection : OwnedCollection<SwitchStatement, SwitchSection>
    {
        public SwitchSectionCollection()
        {
        }

        internal SwitchSectionCollection(SwitchStatement owner)
            : base(owner)
        {
        }

        protected override void ClearState(SwitchSection item)
        {
            item.SwitchStatement = null;
        }

        protected override void SetState(SwitchSection item)
        {
            if (item.SwitchStatement != null
                && (item.SwitchStatement != Owner || item.SwitchStatement.Sections.Contains(item)))
            {
                throw new InvalidOperationException();
            }
            item.SwitchStatement = Owner;
        }
    }
}