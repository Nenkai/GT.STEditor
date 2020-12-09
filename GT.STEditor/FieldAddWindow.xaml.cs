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
using System.Windows.Shapes;

using GTSTEditor.Nodes;
namespace GTSTEditor
{
    /// <summary>
    /// Interaction logic for FieldAddWindow.xaml
    /// </summary>
    public partial class FieldAddWindow : Window
    {
        public NodeKey AddedKey { get; set; }

        public FieldAddWindow()
        {
            InitializeComponent();
            PopulateTypes();

            cb_FieldType.SelectedIndex = 0;
        }

        private void PopulateTypes()
        {
            foreach (var item in Enum.GetNames(typeof(NodeType)))
                cb_FieldType.Items.Add(item);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb_FieldName.Text))
                return;

            var key = new NodeKey();
            key.Name = tb_FieldName.Text;
            try
            {
                switch ((NodeType)cb_FieldType.SelectedIndex)
                {
                    case NodeType.SByte:
                        key.Child = new STSByte(sbyte.Parse(tb_Value.Text));
                        break;
                    case NodeType.Short:
                        key.Child = new STShort(short.Parse(tb_Value.Text));
                        break;
                    case NodeType.Int:
                        key.Child = new STInt(int.Parse(tb_Value.Text));
                        break;
                    case NodeType.Long:
                        key.Child = new STLong(long.Parse(tb_Value.Text));
                        break;
                    case NodeType.Float:
                        key.Child = new STFloat(float.Parse(tb_Value.Text));
                        break;
                    case NodeType.ULong:
                        key.Child = new STULong(ulong.Parse(tb_Value.Text));
                        break;
                    case NodeType.UInt:
                        key.Child = new STUInt(uint.Parse(tb_Value.Text));
                        break;
                    case NodeType.MBlob:
                        key.Child = new MBlob(Array.Empty<byte>());
                        break;

                    case NodeType.Key:
                        key.Child = new NodeKey() { Name = tb_Value.Text };
                        break;

                    case NodeType.StructArray:
                        key.Child = new STArray();
                        break;
                    case NodeType.Struct:
                        key.Child = new STStruct();
                        break;
                    case NodeType.KeyConfig:
                        break;
                    case NodeType.Unk:
                        break;
                    case NodeType.Unk2:
                        break;
                    case NodeType.Unk3:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not parse value as {(NodeType)cb_FieldType.SelectedIndex}.");
                return;
            }

            if (key.Child != null)
                key.Child.Type = (NodeType)cb_FieldType.SelectedIndex;
            key.Type = NodeType.Key;

            AddedKey = key;
            Close();
        }

        private void cb_FieldType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_Value.IsEnabled = IsModifiableNodeType();
        }

        private bool IsModifiableNodeType()
        {
            NodeType selectedType = (NodeType)cb_FieldType.SelectedIndex;
            switch (selectedType)
            {
                case NodeType.SByte:
                case NodeType.Short:
                case NodeType.Int:
                case NodeType.Long:
                case NodeType.Float:
                case NodeType.Key:
                case NodeType.UInt:
                case NodeType.ULong:
                    return true;
                case NodeType.MBlob:
                case NodeType.Null:
                case NodeType.StructArray:
                case NodeType.Struct:
                case NodeType.KeyConfig:
                case NodeType.Unk:
                case NodeType.Unk2:
                case NodeType.Unk3:
                    return false;
                default:
                    return false;
            }
        }
    }
}
