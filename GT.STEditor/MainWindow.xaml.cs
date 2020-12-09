using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Win32;

using Xceed.Wpf.Toolkit;

using GTSTEditor.Nodes;

namespace GTSTEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SerializedTree Tree { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "Gran Turismo ST File (*.*)|*.*";
            openDialog.CheckFileExists = true;
            openDialog.CheckPathExists = true;

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    Tree = SerializedTree.Read(openDialog.FileName);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error occured while loading file: {ex.Message}", "A not so friendly prompt", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                BuildTree();
                menuItem_Save.IsEnabled = true;
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Gran Turismo SDEF File (*.*)|*.*";

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    Tree.Save(saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error occured while saving file: {ex.Message}", "A not so friendly prompt", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void tv_STListing_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tv_STListing.SelectedItem is null)
                return;

            ResetEntryControls();

            var listItem = (tv_STListing.SelectedItem as STListing);
            var key = listItem.Element as NodeKey;
            if (key is null)
                return;

            switch (key.Child)
            {
                case STByte @byte:
                    param_Byte.IsEnabled = true;
                    param_Byte.Value = @byte.Value; break;
                case STSByte @sbyte:
                    param_SByte.IsEnabled = true;
                    param_SByte.Value = @sbyte.Value; break;
                case STInt @int:
                    param_Integer.IsEnabled = true;
                    param_Integer.Value = @int.Value; break;
                case STUInt @uint:
                    param_UInteger.IsEnabled = true;
                    param_UInteger.Value = @uint.Value; break;
                case STFloat @float:
                    param_Single.IsEnabled = true;
                    param_Single.Value = @float.Value; break;
                case STULong @ulong:
                    param_ULong.IsEnabled = true;
                    param_ULong.Value = @ulong.Value; break;
                case STLong @long:
                    param_Long.IsEnabled = true;
                    param_Long.Value = @long.Value; break;
                case STShort @short:
                    param_Short.IsEnabled = true;
                    param_Short.Value = @short.Value; break;
                case NodeKey @string:
                    param_String.IsEnabled = true;
                    param_String.Text = @string.Name; break;

            }
        }


        private void param_Single_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (param_Single.Value != null)
            {
                var listItem = tv_STListing.SelectedItem as STListing;
                var key = listItem.Element as NodeKey;
                var field = key.Child as STFloat;

                field.Value = param_Single.Value.Value;
            }
        }

        private void param_UInteger_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (param_UInteger.Value != null)
            {
                var listItem = tv_STListing.SelectedItem as STListing;
                var key = listItem.Element as NodeKey;
                var field = key.Child as STUInt;

                field.Value = param_UInteger.Value.Value;
            }
        }

        private void param_SByte_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (param_SByte.Value != null)
            {
                var listItem = tv_STListing.SelectedItem as STListing;
                var key = listItem.Element as NodeKey;
                var field = key.Child as STSByte;

                field.Value = param_SByte.Value.Value;
            }
        }

        private void param_Byte_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (param_Byte.Value != null)
            {
                var listItem = tv_STListing.SelectedItem as STListing;
                var key = listItem.Element as NodeKey;
                var field = key.Child as STByte;

                field.Value = param_Byte.Value.Value;
            }
        }

        private void param_Integer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (param_Integer.Value != null)
            {
                var listItem = tv_STListing.SelectedItem as STListing;
                var key = listItem.Element as NodeKey;
                var field = key.Child as STInt;

                field.Value = param_Integer.Value.Value;
            }
        }

        private void param_ULong_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (param_ULong.Value != null)
            {
                var listItem = tv_STListing.SelectedItem as STListing;
                var key = listItem.Element as NodeKey;
                var field = key.Child as STULong;

                field.Value = param_ULong.Value.Value;
            }
        }

        private void param_Long_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (param_ULong.Value != null)
            {
                var listItem = tv_STListing.SelectedItem as STListing;
                var key = listItem.Element as NodeKey;
                var field = key.Child as STLong;

                field.Value = param_Long.Value.Value;
            }
        }

        private void param_Short_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (param_Short.Value != null)
            {
                var listItem = tv_STListing.SelectedItem as STListing;
                var key = listItem.Element as NodeKey;
                var field = key.Child as STShort;

                field.Value = param_Short.Value.Value;
            }
        }

        private void param_String_TextChanged(object sender, RoutedEventArgs e)
        {
            var listItem = tv_STListing.SelectedItem as STListing;
            var key = listItem.Element as NodeKey;
            if (key is null)
                return;

            var field = key.Child as NodeKey;
            if (field is null)
                return;

            field.Name = param_String.Text;
        }

        public void ResetEntryControls()
        {
            param_Byte.IsEnabled = false;
            param_Byte.Value = null;

            param_SByte.IsEnabled = false;
            param_SByte.Value = null;

            param_Bool.IsEnabled = false;
            param_Bool.IsChecked = null;

            param_Integer.IsEnabled = false;
            param_Integer.Value = null;

            param_UInteger.IsEnabled = false;
            param_UInteger.Value = null;

            param_Single.IsEnabled = false;
            param_Single.Value = null;

            param_Double.IsEnabled = false;
            param_Double.Value = null;

            param_ULong.IsEnabled = false;
            param_ULong.Value = null;

            param_String.IsEnabled = false;
            param_String.Text = null;

            param_Short.IsEnabled = false;
            param_Short.Value = null;

            param_Long.IsEnabled = false;
            param_Long.Value = null;
        }

        public void BuildTree()
        {
            var root = new STListing();
            tv_STListing.Items.Clear();
            
            root.Name = "Root";
            root.Element = Tree.RootNode;

            BuildChildTree(root, Tree.RootNode);
            tv_STListing.Items.Add(root);
        }

        public void BuildChildTree(STListing item, NodeBase current)
        {
            if (current is STStruct stru)
            {
                foreach (var field in stru.Children)
                    BuildChildTree(item, field);
            }
            else if (current is STArray struArr)
            {
                foreach (var field in struArr.Children)
                    BuildChildTree(item, field);
            }
            else if (current is NodeKey key && key.Child != null)
            {
                var listElement = new STListing() { Name = key.Name, Element = key };
                BuildChildTree(listElement, key.Child);
                item.Items.Add(listElement);
                listElement.Parent = item;
            }

        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            STListing selectedItem = tv_STListing.SelectedItem as STListing;
            if (selectedItem != null)
            {
                var parentKey = selectedItem.Parent.Element as NodeKey;
                var parentStruct = parentKey.Child as STStruct;
                parentStruct.Children.Remove(selectedItem.Element);
                selectedItem.Parent.Items.Remove(selectedItem);
            }
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            STListing selectedItem = tv_STListing.SelectedItem as STListing;
            if (selectedItem != null)
            {
                STStruct parentStruct;
                if (selectedItem.Element == Tree.RootNode)
                    parentStruct = Tree.RootNode as STStruct;
                else
                {
                    var parentKey = selectedItem.Element as NodeKey;
                    parentStruct = parentKey.Child as STStruct;
                }

                if (parentStruct is null)
                    return;

                var window = new FieldAddWindow();
                window.ShowDialog();
                if (window.AddedKey != null)
                {

                    parentStruct.Children.Add(window.AddedKey);
                    selectedItem.Items.Add(new STListing() { Name = window.AddedKey.Name, Element = window.AddedKey });
                }
            }
        }
    }

    public class STListing : INotifyPropertyChanged
    {
        public STListing()
        {
            this.Items = new ObservableCollection<STListing>();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Notify("Name");
            }
        }

        public STListing Parent { get; set; }
        public NodeBase Element { get; set; }

        public ObservableCollection<STListing> Items { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
