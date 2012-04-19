using System;
using System.Windows.Data;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;

namespace ScottIsAFool.WindowsPhone.Behaviours
{
    public class UpdateTextBindingOnPropertyChanged : Behavior<PhoneTextBox>
    {
        // Fields
        private BindingExpression expression;

        // Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            this.expression = base.AssociatedObject.GetBindingExpression(PhoneTextBox.TextProperty);
            base.AssociatedObject.TextChanged += OnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            base.AssociatedObject.TextChanged -= OnTextChanged;
            this.expression = null;
        }

        private void OnTextChanged(object sender, EventArgs args)
        {
            this.expression.UpdateSource();
        }
    }
}
