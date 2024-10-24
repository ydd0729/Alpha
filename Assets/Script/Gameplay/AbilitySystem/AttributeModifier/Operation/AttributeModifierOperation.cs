using System;

namespace Yd.Gameplay.AbilitySystem
{
    [Serializable]
    public struct AttributeModifierOperation
    {
        public AttributeModifierOperator @operator;
        public AttributeModifierOperand operand;

        public void Deconstruct(out AttributeModifierOperator @operator, out AttributeModifierOperand operand)
        {
            @operator = this.@operator;
            operand = this.operand;
        }
    }
}