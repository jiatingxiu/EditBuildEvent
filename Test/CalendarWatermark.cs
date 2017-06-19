using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Test
{
    public class CalendarWatermark : TextBlock
    {
        public CalendarWatermark()
        {
            this.IsVisibleChanged += CalendarWatermark_IsVisibleChanged;
        }

        private void CalendarWatermark_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                this.Text = "111111111111";
            }
        }
    }
}
