using System;
using System.Windows.Forms;

namespace Soffish.Objects
{
    public class MapFileDialog
    {
        private OpenFileDialog _dialog = null;
        public String FileName { get; private set; }

        public MapFileDialog(String initialDirectory)
        {
            FileName = "";
            _dialog = new OpenFileDialog();
            _dialog.InitialDirectory = initialDirectory;
            _dialog.Filter = "Halo 4 maps (*.map)|*.map|All files (*.*)|*.*";
            _dialog.FilterIndex = 0;
            _dialog.RestoreDirectory = true;
        }

        public bool OpenMapDialog()
        {
            // Only return true when the user opens a file
            if (_dialog.ShowDialog() == DialogResult.OK)
            {
                FileName = _dialog.FileName;
                return true;
            }

            return false;
        }
    }
}
