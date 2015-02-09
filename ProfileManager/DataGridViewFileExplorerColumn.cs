using System;
using System.Windows.Forms;

namespace ProfileManager
{
    internal class DataGridViewFileExplorerColumn : DataGridViewColumn
    {
        private readonly CommonDialog _dialog;

        public DataGridViewFileExplorerColumn(ExplorerType type)
        {
            ExplorerType = type;
            if (type == ExplorerType.Directory)
                _dialog = new FolderBrowserDialog();
            if (type == ExplorerType.File)
                _dialog = new OpenFileDialog();
        }

        public DataGridViewFileExplorerColumn(OpenFileDialog dialog)
        {
            _dialog = dialog;
            ExplorerType = ExplorerType.File;
        }

        public ExplorerType ExplorerType { get; set; }

        public string DialogResult
        {
            get
            {
                switch (ExplorerType)
                {
                    case ExplorerType.Directory:
                        return ((FolderBrowserDialog) _dialog).SelectedPath;
                    case ExplorerType.File:
                        return ((OpenFileDialog) _dialog).FileName;
                    default:
                        return null;
                }
            }
        }

        public void ClearButtonStates()
        {
            foreach (DataGridViewRow row in DataGridView.Rows)
            {
                var filecell = row.Cells[Index] as DataGridViewFileExplorerCell;
                if (filecell != null)
                {
                    filecell.ClearState();
                }
                DataGridView.InvalidateCell(row.Cells[Index]);
            }
        }

        public DialogResult ShowDialog(string current)
        {
            var startDir = OnGetStartDirectory() ?? current;
            if (startDir != null)
                switch (ExplorerType)
                {
                    case ExplorerType.Directory:
                        ((FolderBrowserDialog) _dialog).SelectedPath = startDir;
                        break;
                    case ExplorerType.File:
                        ((OpenFileDialog) _dialog).InitialDirectory = startDir;
                        break;
                }
            return _dialog.ShowDialog();
        }

        public event Func<string> GetStartDirectory;

        protected virtual string OnGetStartDirectory()
        {
            return GetStartDirectory?.Invoke();
        }
    }
}