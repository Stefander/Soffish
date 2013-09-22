using System;
using Common.Framework;
using Soffish.Objects;
using MapReader;
using MapReader.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Soffish.ViewModel
{
    public class SoffishViewModel : BaseViewModel
    {
        private HaloMap _map;
        private SoffishView _view;
        private MapFileDialog _openDialog;
        private Visibility _isVisible;

        public String MetaDataOffset { get; private set; }
        public uint XDKVersion { get; private set; }
        public uint ObjectAmount { get; private set; }
        public uint TagAmount { get; private set; }
        public String Type { get; private set; }
        public Visibility IsVisible { get { return _isVisible; } private set { _isVisible = value; OnPropertyChanged("ViewVisible"); } }
        public String IndexOffset { get; private set; }
        public String IndexOffsetMagic { get; private set; }
        public String ScenarioName { get; private set; }
        public String MapMagic { get; private set; }
        public String MapName { get; private set; }
        public String BuildVersion { get; private set; }
        public DelegateCommand<object> LoadCommand { get; private set; }
        public DelegateCommand<object> QuitCommand { get; private set; }

        public SoffishViewModel(SoffishView inView)
        {
            _view = inView;
            _map = new HaloMap();
            _openDialog = new MapFileDialog(System.AppDomain.CurrentDomain.BaseDirectory);
            LoadCommand = new DelegateCommand<object>(LoadMap, null, "Load map");
            QuitCommand = new DelegateCommand<object>(Quit, null, "Quit");
            IsVisible = Visibility.Hidden;
        }

        private void UpdateObjectList()
        {
            _view.tagView.Items.Clear();

            foreach (TagListEntry entry in _map.Tables.Tags.TagList)
            {
                TreeViewItem item = new TreeViewItem();

                foreach (ObjectListEntry objectEntry in _map.Tables.Objects.ObjectList)
                {
                    if (objectEntry.ObjectTag == entry)
                    {
                        TreeViewItem objectItem = new TreeViewItem();
                        objectItem.Header = "0x" + HaloMap.ToHex(objectEntry.Offset);
                        item.Items.Add(objectItem);
                    }
                }

                item.Header = entry.Tags[0].TagName;

                if (item.Items.Count > 0)
                {
                    item.Header += " (" + item.Items.Count + ")";
                }
                item.IsExpanded = false;
                _view.tagView.Items.Add(item);
            }
        }

        private void UpdateData()
        {
            _view.Title = "Soffish - " + _map.Header.MapName;
            IsVisible = Visibility.Visible;
            MapName = _map.Header.MapName;
            BuildVersion = _map.Header.BuildVersion;
            TagAmount = _map.Tables.TagList.Count;
            ObjectAmount = _map.Tables.ObjectList.Count;
            ScenarioName = _map.Header.ScenarioName;
            Type = _map.Header.Type.ToString();
            XDKVersion = _map.Header.XDKVersion;
            MapMagic = "0x" + HaloMap.ToHex(_map.Header.MapMagic);
            IndexOffset = "0x" + HaloMap.ToHex(_map.Header.IndexOffset);
            MetaDataOffset = "0x" + HaloMap.ToHex(_map.Header.MetaDataOffset);
            IndexOffsetMagic = "0x" + HaloMap.ToHex(_map.Header.IndexOffsetMagic);

            // Update all binding properties
            OnPropertyChanged(null);

            UpdateObjectList();
        }

        void LoadMap(object arg)
        {
            if (!_openDialog.OpenMapDialog())
            {
                return;
            }

            IsVisible = Visibility.Hidden;

            if (_map.LoadMap(_openDialog.FileName))
            {
                UpdateData();
            }
        }

        void Quit(object arg)
        {
            _view.Close();
        }
    }
}
