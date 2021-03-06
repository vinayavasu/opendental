using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary></summary>
	public class FormCustomerManagement : ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.ODGrid gridMain;
		private ContextMenu contextMain;
		private MenuItem menuItemGoTo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<Summary>This will only contain a value if the user clicked GoTo.</Summary>
		public long SelectedPatNum;
		private Label label1;
		private DataTable TableRegKeys;

		///<summary></summary>
		public FormCustomerManagement()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			gridMain.ContextMenu=contextMain;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCustomerManagement));
			this.contextMain = new System.Windows.Forms.ContextMenu();
			this.menuItemGoTo = new System.Windows.Forms.MenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// contextMain
			// 
			this.contextMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGoTo});
			// 
			// menuItemGoTo
			// 
			this.menuItemGoTo.Index = 0;
			this.menuItemGoTo.Text = "GoTo";
			this.menuItemGoTo.Click += new System.EventHandler(this.menuItemGoTo_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(666, 19);
			this.label1.TabIndex = 18;
			this.label1.Text = "Below is a list of customers who have active registration keys where no family me" +
    "mbers have an active repeating charge. \r\n";
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(12, 31);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(666, 623);
			this.gridMain.TabIndex = 2;
			this.gridMain.Title = "Customers";
			this.gridMain.TranslationName = "TableCustomers";
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(714, 628);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormCustomerManagement
			// 
			this.ClientSize = new System.Drawing.Size(801, 666);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormCustomerManagement";
			this.ShowInTaskbar = false;
			this.Text = "Customer Management";
			this.Load += new System.EventHandler(this.FormCustomerManagement_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormCustomerManagement_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			Cursor.Current=Cursors.WaitCursor;
			TableRegKeys=RegistrationKeys.GetAllWithoutCharges();
			Cursor.Current=Cursors.Default;
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn("PatNum",60);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("RegKey",140);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Family",200);
			gridMain.ListGridColumns.Add(col);
			//col=new ODGridColumn("Repeating Charge",150);
			//gridMain.Columns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<TableRegKeys.Rows.Count;i++){
				row=new GridRow();
				row.Cells.Add(TableRegKeys.Rows[i]["PatNum"].ToString());
				row.Cells.Add(TableRegKeys.Rows[i]["RegKey"].ToString());
				row.Cells.Add(TableRegKeys.Rows[i]["LName"].ToString()+", "+TableRegKeys.Rows[i]["FName"].ToString());
				//row.Cells.Add(table.Rows[i]["dateStop"].ToString());
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void menuItemGoTo_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				return;
			}
			SelectedPatNum=PIn.Long(TableRegKeys.Rows[gridMain.GetSelectedIndex()]["PatNum"].ToString());
			Close();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		

		


	}
}





















