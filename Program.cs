using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet
{
    internal static class Program
    {        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Constants.Reflection.Init();           

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

/*using System;
using System.Drawing;
using System.Windows.Forms;

public class CalendarCell : DataGridViewTextBoxCell
{

    public CalendarCell()
        : base()
    {
        // Use the short date format.
        this.Style.Format = "d";
    }

    public override void InitializeEditingControl(int rowIndex, object
        initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
        // Set the value of the editing control to the current cell value.
        base.InitializeEditingControl(rowIndex, initialFormattedValue,
            dataGridViewCellStyle);
        CalendarEditingControl ctl =
            DataGridView.EditingControl as CalendarEditingControl;
        // Use the default row value when Value property is null.
        if (this.Value == null)
        {
            ctl.Value = (DateTime)this.DefaultNewRowValue;
        }
        else
        {
            ctl.Value = (DateTime)this.Value;
        }
    }

    public override Type EditType
    {
        get
        {
            // Return the type of the editing control that CalendarCell uses.
            return typeof(CalendarEditingControl);
        }
    }

    public override Type ValueType
    {
        get
        {
            // Return the type of the value that CalendarCell contains.

            return typeof(DateTime);
        }
    }

    public override object DefaultNewRowValue
    {
        get
        {
            // Use the current date and time as the default value.
            return DateTime.Now;
        }
    }
}

class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
{
    DataGridView dataGridView;
    private bool valueChanged = false;
    int rowIndex;

    public CalendarEditingControl()
    {
        this.Format = DateTimePickerFormat.Short;
    }

    // Implements the IDataGridViewEditingControl.EditingControlFormattedValue
    // property.
    public object EditingControlFormattedValue
    {
        get
        {
            return this.Value.ToShortDateString();
        }
        set
        {
            if (value is String)
            {
                try
                {
                    // This will throw an exception of the string is
                    // null, empty, or not in the format of a date.
                    this.Value = DateTime.Parse((String)value);
                }
                catch
                {
                    // In the case of an exception, just use the
                    // default value so we're not left with a null
                    // value.
                    this.Value = DateTime.Now;
                }
            }
        }
    }

    // Implements the
    // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
    public object GetEditingControlFormattedValue(
        DataGridViewDataErrorContexts context)
    {
        return EditingControlFormattedValue;
    }

    // Implements the
    // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
    public void ApplyCellStyleToEditingControl(
        DataGridViewCellStyle dataGridViewCellStyle)
    {
        this.Font = dataGridViewCellStyle.Font;
        this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
        this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
    }

    // Implements the IDataGridViewEditingControl.EditingControlRowIndex
    // property.
    public int EditingControlRowIndex
    {
        get
        {
            return rowIndex;
        }
        set
        {
            rowIndex = value;
        }
    }

    // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey
    // method.
    public bool EditingControlWantsInputKey(
        Keys key, bool dataGridViewWantsInputKey)
    {
        // Let the DateTimePicker handle the keys listed.
        switch (key & Keys.KeyCode)
        {
            case Keys.Left:
            case Keys.Up:
            case Keys.Down:
            case Keys.Right:
            case Keys.Home:
            case Keys.End:
            case Keys.PageDown:
            case Keys.PageUp:
                return true;
            default:
                return !dataGridViewWantsInputKey;
        }
    }

    // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit
    // method.
    public void PrepareEditingControlForEdit(bool selectAll)
    {
        // No preparation needs to be done.
    }

    // Implements the IDataGridViewEditingControl
    // .RepositionEditingControlOnValueChange property.
    public bool RepositionEditingControlOnValueChange
    {
        get
        {
            return false;
        }
    }

    // Implements the IDataGridViewEditingControl
    // .EditingControlDataGridView property.
    public DataGridView EditingControlDataGridView
    {
        get
        {
            return dataGridView;
        }
        set
        {
            dataGridView = value;
        }
    }

    // Implements the IDataGridViewEditingControl
    // .EditingControlValueChanged property.
    public bool EditingControlValueChanged
    {
        get
        {
            return valueChanged;
        }
        set
        {
            valueChanged = value;
        }
    }

    // Implements the IDataGridViewEditingControl
    // .EditingPanelCursor property.
    public Cursor EditingPanelCursor
    {
        get
        {
            return base.Cursor;
        }
    }

    protected override void OnValueChanged(EventArgs eventargs)
    {
        // Notify the DataGridView that the contents of the cell
        // have changed.
        valueChanged = true;
        this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        base.OnValueChanged(eventargs);
    }
}

public class Form1 : Form
{
    private DataGridView dataGridView1 = new DataGridView();

    [STAThreadAttribute()]
    public static void Main()
    {
        Application.Run(new Form1());
    }

    public Form1()
    {
        this.dataGridView1.Dock = DockStyle.Fill;
        this.Controls.Add(this.dataGridView1);
        this.Load += new EventHandler(Form1_Load);
        this.Text = "DataGridView calendar column demo";
        dataGridView1.CellPainting += DataGridView1_CellPainting;
        dataGridView1.CellClick += DataGridView1_CellClick;
    }

    private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        dataGridView1.BeginEdit(false);        
    }

    private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
        e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
        var cal = new CalendarEditingControl
        {
            Size = e.CellBounds.Size,            
        };
        var bmp = new Bitmap(e.CellBounds.Width, e.CellBounds.Height);
        cal.DrawToBitmap(bmp, new Rectangle(Point.Empty, e.CellBounds.Size));
        e.Graphics.DrawImageUnscaled(bmp, e.CellBounds.Location);
        e.Handled = true;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        //CalendarColumn col = new CalendarColumn();
        //this.dataGridView1.Columns.Add(col);
        dataGridView1.ColumnCount = 2;
        this.dataGridView1.RowCount = 5;
        foreach (DataGridViewRow row in this.dataGridView1.Rows)
        {
            row.Cells[0].Value = DateTime.Now;
            row.Cells[1] = new CalendarCell();
        }
    }
}
*/