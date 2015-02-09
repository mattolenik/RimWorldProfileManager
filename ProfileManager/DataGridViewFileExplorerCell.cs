using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ProfileManager
{
    internal enum ExplorerType
    {
        Directory,
        File
    }

    internal class DataGridViewFileExplorerCell : DataGridViewTextBoxCell
    {
        private readonly DataGridViewFileExplorerColumn _column;
        private bool _focused;
        private PushButtonState _state = PushButtonState.Normal;
        private Rectangle? buttonBounds;

        public DataGridViewFileExplorerCell()
        {
            _column = OwningColumn as DataGridViewFileExplorerColumn;
            if (_column == null)
                throw new Exception(
                    "DataGridViewFileExplorerCell must be added as a CellTemplate to a DataGridViewFileExplorerColumn");
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
                cellStyle, advancedBorderStyle, paintParts);

            var bounds = cellBounds;
            var newBounds = new Rectangle(bounds.X + bounds.Width - 30 - 1, bounds.Y + 1, 30, bounds.Height - 2);
            // buttonBounds is without the bounds.X and bounds.Y, because the MouseEventArgs are based on x=0 and y=0 being the cell.
            // in cellBounds, x = datagridview.x, y = datagridview.y.
            buttonBounds = new Rectangle(bounds.Width - 30 - 1, 1, 30, bounds.Height - 2);
            ButtonRenderer.DrawButton(graphics, newBounds, "...", new Font("Comic Sans MS", 9.0f, FontStyle.Bold),
                _focused, _state);
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            if (!buttonBounds.HasValue) return;
            if (!buttonBounds.Value.Contains(e.Location) && e.Button == MouseButtons.Left &&
                _state == PushButtonState.Pressed)
            {
                _state = PushButtonState.Normal;
                DataGridView.InvalidateCell(this);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (!buttonBounds.HasValue) return;
            if (buttonBounds.Value.Contains(e.Location) && e.Button == MouseButtons.Left)
            {
                if (e.RowIndex == -1) return;
                _focused = true;
                _column.ClearButtonStates();
                var data = ShowFileExplorer(DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string);
                if (data != null)
                {
                    // This triggers the database change event which will save the data.
                    DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = data;
                }
            }
            base.OnMouseClick(e);
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (!buttonBounds.HasValue) return;
            if (buttonBounds.Value.Contains(e.Location) && e.Button == MouseButtons.Left)
            {
                _state = PushButtonState.Pressed;
                DataGridView.InvalidateCell(this);
                return;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if (!buttonBounds.HasValue) return;
            if (_state == PushButtonState.Hot) _state = PushButtonState.Normal;
            if (buttonBounds.Value.Contains(e.Location) && _state == PushButtonState.Pressed)
            {
                _state = PushButtonState.Hot;
            }
            if (_state == PushButtonState.Pressed) _state = PushButtonState.Normal;
            base.OnMouseUp(e);
        }

        private string ShowFileExplorer(string current)
        {
            var result = _column.ShowDialog(current);
            return result == DialogResult.OK ? _column.DialogResult : null;
        }

        protected override void OnLeave(int rowIndex, bool throughMouseClick)
        {
            _focused = false;
            _state = PushButtonState.Normal;
            DataGridView.InvalidateCell(this);
            base.OnLeave(rowIndex, throughMouseClick);
        }

        public void ClearState()
        {
            _state = PushButtonState.Normal;
        }
    }
}