using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormInsFilingCodes:ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butNone;
		private OpenDental.UI.Button butOK;
		private bool changed;
		public bool IsSelectionMode;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.Button butDown;
		///<summary>Only used if IsSelectionMode.  On OK, contains selected InsFilingCodeNum.  Can be 0.  Can also be set ahead of time externally.</summary>
		public long SelectedInsFilingCodeNum;
		private List<InsFilingCode> _listInsFilingCodes;

		///<summary></summary>
		public FormInsFilingCodes()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsFilingCodes));
			this.butNone = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butNone
			// 
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNone.Location = new System.Drawing.Point(512, 349);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(80, 24);
			this.butNone.TabIndex = 16;
			this.butNone.Text = "None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(517, 570);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 15;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(17, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(467, 612);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "Insurance Filing Codes";
			this.gridMain.TranslationName = "TableInsFilingCodes";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(512, 12);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(80, 24);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(517, 600);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(512, 185);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(75, 24);
			this.butUp.TabIndex = 17;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butDown
			// 
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(512, 215);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(75, 24);
			this.butDown.TabIndex = 18;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// FormInsFilingCodes
			// 
			this.ClientSize = new System.Drawing.Size(620, 640);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormInsFilingCodes";
			this.ShowInTaskbar = false;
			this.Text = "Insurance Filing Codes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInsFilingCodes_FormClosing);
			this.Load += new System.EventHandler(this.FormInsFilingCodes_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormInsFilingCodes_Load(object sender,EventArgs e) {
			if(IsSelectionMode) {
				butClose.Text=Lan.g(this,"Cancel");
			} 
			else {
				butOK.Visible=false;
				butNone.Visible=false;
			}
			_listInsFilingCodes=InsFilingCodes.GetDeepCopy();
			//synch the itemorders just in case
			for(int i=0;i<_listInsFilingCodes.Count;i++) {
			  if(_listInsFilingCodes[i].ItemOrder!=i) {
			    _listInsFilingCodes[i].ItemOrder=i;
			    InsFilingCodes.Update(_listInsFilingCodes[i]);
			    changed=true;
			  }
			}
			FillGrid();
			if(SelectedInsFilingCodeNum!=0) {
				for(int i=0;i<_listInsFilingCodes.Count;i++) {
					if(_listInsFilingCodes[i].InsFilingCodeNum==SelectedInsFilingCodeNum) {
						gridMain.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void FillGrid(){
			InsFilingCodes.RefreshCache();
			_listInsFilingCodes=InsFilingCodes.GetDeepCopy();
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableInsFilingCodes","Description"),250);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableInsFilingCodes","Group"),100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableInsFilingCodes","EclaimCode"),100);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<_listInsFilingCodes.Count;i++){
				row=new GridRow();
				row.Cells.Add(_listInsFilingCodes[i].Descript);
				string group="";
				if(_listInsFilingCodes[i].GroupType > 0) {
					group=Defs.GetDef(DefCat.InsuranceFilingCodeGroup,_listInsFilingCodes[i].GroupType)?.ItemName??"";
				}
				row.Cells.Add(group);
				row.Cells.Add(_listInsFilingCodes[i].EclaimCode);
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormInsFilingCodeEdit FormIE=new FormInsFilingCodeEdit();
			FormIE.InsFilingCodeCur=new InsFilingCode();
			FormIE.InsFilingCodeCur.ItemOrder=_listInsFilingCodes.Count;
			FormIE.InsFilingCodeCur.IsNew=true;
			FormIE.ShowDialog();
			FillGrid();
			changed=true;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(IsSelectionMode){
				SelectedInsFilingCodeNum=_listInsFilingCodes[e.Row].InsFilingCodeNum;
				DialogResult=DialogResult.OK;
				return;
			}
			else{
				FormInsFilingCodeEdit FormI=new FormInsFilingCodeEdit();
				FormI.InsFilingCodeCur=_listInsFilingCodes[e.Row];
				FormI.ShowDialog();
				FillGrid();
				changed=true;
			}
		}

		private void butNone_Click(object sender,EventArgs e) {
			//not even visible unless is selection mode
			SelectedInsFilingCodeNum=0;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//not even visible unless is selection mode
			if(gridMain.GetSelectedIndex()==-1){
				SelectedInsFilingCodeNum=0;
			}
			else{
				SelectedInsFilingCodeNum=_listInsFilingCodes[gridMain.GetSelectedIndex()].InsFilingCodeNum;
			}
			DialogResult=DialogResult.OK;
		}

		private void butUp_Click(object sender,EventArgs e) {
			int idx=gridMain.GetSelectedIndex();
			if(idx==-1) {
				MsgBox.Show(this,"Please select an insurance filing code first.");
				return;
			}
			if(idx==0) {
				return;
			}
			//swap the orders.
			int order1=_listInsFilingCodes[idx-1].ItemOrder;
			int order2=_listInsFilingCodes[idx].ItemOrder;
			_listInsFilingCodes[idx-1].ItemOrder=order2;
			InsFilingCodes.Update(_listInsFilingCodes[idx-1]);
			_listInsFilingCodes[idx].ItemOrder=order1;
			InsFilingCodes.Update(_listInsFilingCodes[idx]);
			changed=true;
			FillGrid();
			gridMain.SetSelected(idx-1,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			int idx=gridMain.GetSelectedIndex();
			if(idx==-1) {
				MsgBox.Show(this,"Please select an insurance filing code first.");
				return;
			}
			if(idx==_listInsFilingCodes.Count-1) {
				return;
			}
			int order1=_listInsFilingCodes[idx].ItemOrder;
			int order2=_listInsFilingCodes[idx+1].ItemOrder;
			_listInsFilingCodes[idx].ItemOrder=order2;
			InsFilingCodes.Update(_listInsFilingCodes[idx]);
			_listInsFilingCodes[idx+1].ItemOrder=order1;
			InsFilingCodes.Update(_listInsFilingCodes[idx+1]);
			changed=true;
			FillGrid();
			gridMain.SetSelected(idx+1,true);
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			Close();
		}

		private void FormInsFilingCodes_FormClosing(object sender,FormClosingEventArgs e) {
			if(changed) {
				DataValid.SetInvalid(InvalidType.InsFilingCodes);
			}
		}
		
	}
}





















