using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;
using System.Xml.Linq;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataGridSource source = new DataGridSource();

        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
        public MainWindow()
        {
            InitializeComponent();



            DataView dv = new DataView();

            var empTbl = new DataTable("1");
            dv.Table = empTbl;

            var dc = new DataColumn("Test1");
            dv.Table.Columns.Add(dc);
            dc = new DataColumn("Test2");
            dv.Table.Columns.Add(dc);
            dc = new DataColumn("Test3");
            dv.Table.Columns.Add(dc);

            if (dv.Table.Rows.Count == 0)
            {
                DataRow dr = dv.Table.NewRow();
                for (int i = 0; i < dv.Table.Columns.Count; i++)
                {
                    object oo = GetDefault(dv.Table.Columns[i].DataType);
                    dr[i] = oo;

                }
                dv.Table.Rows.Add(dr);
            }
            source.MyProperty = dv;

            this.DataContext = source;


            this.Loaded += MainWindow_Loaded;

            return;
            //RichTextBox
            //Assembly assembly = Assembly.LoadFrom("Test.exe");
            //Version ver = assembly.GetName().Version;

            //Load xml
            XDocument xdoc = XDocument.Load("CommonSettings.xml");

            //Run query
            var lv1s = from lv1 in xdoc.Descendants("SoftwareVersions")
                       select lv1;
            var item = lv1s.FirstOrDefault();

            //GridViewColumnHeader
            //GridViewColumn

            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.MouseEnterEvent, new RoutedEventHandler(SelectivelyIgnoreMouseButton));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //source.MyProperty.Table.Rows.RemoveAt(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            source.MyProperty.Table.Rows.RemoveAt(0);
        }

        private void SelectivelyIgnoreMouseButton(object sender, RoutedEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent == null) return;
            var textBox = (TextBox)parent;
            if (textBox.IsKeyboardFocusWithin) return;
            // If the text box is not yet focused, give it the focus and
            // stop further processing of this click event.
            textBox.Focus();
            e.Handled = true;
        }

        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("234234");
        }

        private void datepicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datepicker.SelectedDate > DateTime.Now)
                datepicker.SelectedDate = null;
        }


    }
}
