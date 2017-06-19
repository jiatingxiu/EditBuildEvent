using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Test
{
    public class DatePickerTextBoxBehavior : Behavior<FrameworkElement>
    {
        #region Fields

        private bool _IsBack = false;

        #endregion

        #region Property


        #endregion

        #region DependencyProperty

        public bool IsValidation
        {
            get { return (bool)GetValue(IsValidationProperty); }
            set { SetValue(IsValidationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsValidation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsValidationProperty =
            DependencyProperty.Register("IsValidation", typeof(bool), typeof(DatePickerTextBoxBehavior), new PropertyMetadata(false));

        #endregion

        #region Constroctors

        public DatePickerTextBoxBehavior()
        {

        }

        #endregion

        #region PrivateMethod

        private static Boolean IsDataValid(IDataObject data)
        {
            Boolean isValid = false;
            if (data != null)
            {
                String text = data.GetData(DataFormats.Text) as String;
                if (!String.IsNullOrEmpty(text == null ? null : text.Trim()))
                {
                    float result = -1;
                    if (float.TryParse(text, out result))
                    {
                        if (result > 0)
                        {
                            isValid = true;
                        }
                    }
                }
            }

            return isValid;
        }

        #endregion

        #region ProtectedMethod

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                InputMethod.SetIsInputMethodEnabled(AssociatedObject as DatePickerTextBox, false);
                (AssociatedObject as DatePickerTextBox).MaxLength = 10;
                (AssociatedObject as DatePickerTextBox).TextChanged += AssociatedObject_TextChanged;
                (AssociatedObject as DatePickerTextBox).LostFocus += DatePickerTextBoxBehavior_LostFocus;
                (AssociatedObject as DatePickerTextBox).DragOver += DatePickerTextBoxBehavior_DragOver;
                (AssociatedObject as DatePickerTextBox).Drop += DatePickerTextBoxBehavior_Drop;
                (AssociatedObject as DatePickerTextBox).PreviewKeyDown += DatePickerTextBoxBehavior_PreviewKeyDown;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        #endregion

        #region PublicMethod

        #endregion

        #region Event

        private void DatePickerTextBoxBehavior_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsValidation)
            {
                (sender as DatePickerTextBox).CaretIndex = 0;
                (sender as DatePickerTextBox).Text = string.Empty;
            }
        }

        private void AssociatedObject_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string text = (sender as DatePickerTextBox).Text;
            if (_IsBack)
            {
                (sender as DatePickerTextBox).CaretIndex = text.Length;
                e.Handled = true;
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(text)) return;
                string[] ymd = text.Split('-');
                if (ymd.Length == 1)
                {
                    if (ymd[0].Length == 4)
                    {
                        text += "-";
                    }
                    else
                    {

                    }
                }
                else if (ymd.Length == 2)
                {
                    if (ymd[1].Length == 2)
                    {
                        text += "-";
                    }
                }
                else if (ymd.Length == 3)
                {
                    if (ymd[2].Length == 2)
                    {
                        e.Handled = true;
                    }
                }
                (sender as DatePickerTextBox).CaretIndex = text.Length;
                (sender as DatePickerTextBox).Text = text;
            }

        }

        private void DatePickerTextBoxBehavior_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                string text = (sender as DatePickerTextBox).Text;
                if (string.IsNullOrEmpty(text)) return;
                _IsBack = true;
                string[] ymd = text.Split('-');
                if (ymd.Length == 1)
                {

                }
                else if (ymd.Length == 2)
                {
                    if (ymd[1].Length == 0)
                    {
                        text = text.Substring(0, text.Length - 2);
                        e.Handled = true;
                    }
                }
                else if (ymd.Length == 3)
                {
                    if (ymd[2].Length == 0)
                    {
                        text = text.Substring(0, text.Length - 2);
                        e.Handled = true;
                    }
                }
                (sender as DatePickerTextBox).Text = text;
                _IsBack = false;
                return;
            }
            if (Keyboard.Modifiers == ModifierKeys.Shift || e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            // 如果输入的字符不在D0到D9之间 并且不再 NumPad0到NumPad9之间 并且 不是BackSpace   按键无效
            if ((e.Key < Key.D0 || e.Key > Key.D9) &&
                (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) &&
                (e.Key != Key.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void DatePickerTextBoxBehavior_Drop(object sender, DragEventArgs e)
        {
            e.Handled = !IsDataValid(e.Data);
        }

        private void DatePickerTextBoxBehavior_DragOver(object sender, DragEventArgs e)
        {
            if (!IsDataValid(e.Data))
            {
                e.Handled = true;
                e.Effects = DragDropEffects.None;
            }
        }

        #endregion
    }
}
