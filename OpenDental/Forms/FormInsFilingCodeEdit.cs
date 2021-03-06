using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormInsFilingCodeEdit:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textDescription;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components=null;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.TextBox textEclaimCode;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.ODGrid gridInsFilingCodeSubtypes;
		public InsFilingCode InsFilingCodeCur;
		private OpenDental.UI.Button butAdd;
		private Label label3;
		private UI.ComboBoxPlus comboGroup;
		private List <InsFilingCodeSubtype> insFilingCodeSubtypes;

		///<summary></summary>
		public FormInsFilingCodeEdit()
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
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(FormInsFilingCodeEdit));
			this.label1=new System.Windows.Forms.Label();
			this.textDescription=new System.Windows.Forms.TextBox();
			this.textEclaimCode=new System.Windows.Forms.TextBox();
			this.label2=new System.Windows.Forms.Label();
			this.gridInsFilingCodeSubtypes=new OpenDental.UI.ODGrid();
			this.butDelete=new OpenDental.UI.Button();
			this.butOK=new OpenDental.UI.Button();
			this.butCancel=new OpenDental.UI.Button();
			this.butAdd=new OpenDental.UI.Button();
			this.label3=new System.Windows.Forms.Label();
			this.comboGroup=new UI.ComboBoxPlus();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location=new System.Drawing.Point(9, 21);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(148, 17);
			this.label1.TabIndex=2;
			this.label1.Text="Description";
			this.label1.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Location=new System.Drawing.Point(160, 20);
			this.textDescription.Name="textDescription";
			this.textDescription.Size=new System.Drawing.Size(291, 20);
			this.textDescription.TabIndex=0;
			this.textDescription.TextChanged+=new System.EventHandler(this.textDescription_TextChanged);
			// 
			// textEclaimCode
			// 
			this.textEclaimCode.Location=new System.Drawing.Point(160, 45);
			this.textEclaimCode.MaxLength=255;
			this.textEclaimCode.Name="textEclaimCode";
			this.textEclaimCode.Size=new System.Drawing.Size(157, 20);
			this.textEclaimCode.TabIndex=1;
			this.textEclaimCode.TextChanged+=new System.EventHandler(this.textEclaimCode_TextChanged);
			// 
			// label2
			// 
			this.label2.Location=new System.Drawing.Point(8, 48);
			this.label2.Name="label2";
			this.label2.Size=new System.Drawing.Size(151, 17);
			this.label2.TabIndex=99;
			this.label2.Text="Eclaim Code";
			this.label2.TextAlign=System.Drawing.ContentAlignment.TopRight;
			// 
			// gridInsFilingCodeSubtypes
			// 
			this.gridInsFilingCodeSubtypes.Location=new System.Drawing.Point(160, 97);
			this.gridInsFilingCodeSubtypes.Name="gridInsFilingCodeSubtypes";
			this.gridInsFilingCodeSubtypes.Size=new System.Drawing.Size(291, 160);
			this.gridInsFilingCodeSubtypes.TabIndex=100;
			this.gridInsFilingCodeSubtypes.Title="Insurance Filing Code Subtypes";
			this.gridInsFilingCodeSubtypes.TranslationName="TableInsFilingCodeSubtypes";
			this.gridInsFilingCodeSubtypes.CellDoubleClick+=new OpenDental.UI.ODGridClickEventHandler(this.gridInsFilingCodeSubtypes_CellDoubleClick);
			// 
			// butDelete
			// 
			this.butDelete.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image=global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign=System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location=new System.Drawing.Point(27, 302);
			this.butDelete.Name="butDelete";
			this.butDelete.Size=new System.Drawing.Size(81, 26);
			this.butDelete.TabIndex=4;
			this.butDelete.Text="Delete";
			this.butDelete.Click+=new System.EventHandler(this.butDelete_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location=new System.Drawing.Point(412, 302);
			this.butOK.Name="butOK";
			this.butOK.Size=new System.Drawing.Size(75, 26);
			this.butOK.TabIndex=9;
			this.butOK.Text="&OK";
			this.butOK.Click+=new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location=new System.Drawing.Point(503, 302);
			this.butCancel.Name="butCancel";
			this.butCancel.Size=new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex=10;
			this.butCancel.Text="&Cancel";
			this.butCancel.Click+=new System.EventHandler(this.butCancel_Click);
			// 
			// butAdd
			// 
			this.butAdd.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Enabled=false;
			this.butAdd.Image=global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign=System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location=new System.Drawing.Point(160, 263);
			this.butAdd.Name="butAdd";
			this.butAdd.Size=new System.Drawing.Size(80, 24);
			this.butAdd.TabIndex=101;
			this.butAdd.Text="&Add";
			this.butAdd.Click+=new System.EventHandler(this.butAdd_Click);
			// 
			// label3
			// 
			this.label3.Location=new System.Drawing.Point(8, 73);
			this.label3.Name="label3";
			this.label3.Size=new System.Drawing.Size(151, 17);
			this.label3.TabIndex=103;
			this.label3.Text="Group";
			this.label3.TextAlign=System.Drawing.ContentAlignment.TopRight;
			// 
			// comboGroup
			// 
			this.comboGroup.Location=new System.Drawing.Point(160, 70);
			this.comboGroup.Name="comboGroup";
			this.comboGroup.Size=new System.Drawing.Size(157, 21);
			this.comboGroup.TabIndex=104;
			// 
			// FormInsFilingCodeEdit
			// 
			this.ClientSize=new System.Drawing.Size(604, 346);
			this.Controls.Add(this.comboGroup);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.gridInsFilingCodeSubtypes);
			this.Controls.Add(this.textEclaimCode);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Icon=((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox=false;
			this.MinimizeBox=false;
			this.Name="FormInsFilingCodeEdit";
			this.ShowInTaskbar=false;
			this.Text="Edit Claim Filing Code";
			this.Load+=new System.EventHandler(this.FormInsFilingCodeEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormInsFilingCodeEdit_Load(object sender, System.EventArgs e) {
			textDescription.Text=InsFilingCodeCur.Descript;
			textEclaimCode.Text=InsFilingCodeCur.EclaimCode;
			comboGroup.Items.AddDefNone();
			comboGroup.Items.AddDefs(Defs.GetDefsForCategory(DefCat.InsuranceFilingCodeGroup,true));
			comboGroup.SetSelectedDefNum(InsFilingCodeCur.GroupType); 
			FillGrid();
		}

		private void FillGrid() {
			InsFilingCodeSubtypes.RefreshCache();
			insFilingCodeSubtypes=InsFilingCodeSubtypes.GetForInsFilingCode(InsFilingCodeCur.InsFilingCodeNum);
			gridInsFilingCodeSubtypes.BeginUpdate();
			gridInsFilingCodeSubtypes.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableInsFilingCodes","Description"),100);
			gridInsFilingCodeSubtypes.ListGridColumns.Add(col);
			gridInsFilingCodeSubtypes.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<insFilingCodeSubtypes.Count;i++) {
				row=new GridRow();
				row.Cells.Add(insFilingCodeSubtypes[i].Descript);
				gridInsFilingCodeSubtypes.ListGridRows.Add(row);
			}
			gridInsFilingCodeSubtypes.EndUpdate();
		}

		private void gridInsFilingCodeSubtypes_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormInsFilingCodeSubtypeEdit FormI=new FormInsFilingCodeSubtypeEdit();
			FormI.InsFilingCodeSubtypeCur=insFilingCodeSubtypes[e.Row].Clone();
			FormI.ShowDialog();
			if(FormI.DialogResult==DialogResult.OK){
				try {
					InsFilingCodeSubtypes.Update(FormI.InsFilingCodeSubtypeCur);
				} 
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormInsFilingCodeSubtypeEdit FormI=new FormInsFilingCodeSubtypeEdit();
			FormI.InsFilingCodeSubtypeCur=new InsFilingCodeSubtype();
			FormI.InsFilingCodeSubtypeCur.IsNew=true;
			FormI.ShowDialog();
			if(FormI.DialogResult==DialogResult.OK) {
				if(InsFilingCodeCur.IsNew){
					//If we are adding a subtype to a new filing code, then we need to
					//save the filing code to the database to generate the InsFilingCodeNum,
					//so that we can then save teh InsFilingCodeSubtype record with the correct
					//foreign key.
					SaveFilingCode();
					InsFilingCodeCur.IsNew=false;
				}
				FormI.InsFilingCodeSubtypeCur.InsFilingCodeNum=InsFilingCodeCur.InsFilingCodeNum;
				try {
					InsFilingCodeSubtypes.Insert(FormI.InsFilingCodeSubtypeCur);
				} 
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
				FillGrid();
			}
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(InsFilingCodeCur.IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete this code?")) {
				return;
			}
			try{
				InsFilingCodes.Delete(InsFilingCodeCur.InsFilingCodeNum);
				InsFilingCodeSubtypes.DeleteForInsFilingCode(InsFilingCodeCur.InsFilingCodeNum);
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(this.textDescription.Text==""){
				MessageBox.Show(Lan.g(this,"Please enter a description."));
				return;
			}
			if(this.textEclaimCode.Text==""){
				MessageBox.Show(Lan.g(this,"Please enter an electronic claim code."));
				return;
			}
			if(comboGroup.GetSelectedDefNum()==0) {
				MsgBox.Show(this,"Please select a group.");
				return;
			}
			SaveFilingCode();
			DialogResult=DialogResult.OK;
		}

		private void SaveFilingCode(){
			InsFilingCodeCur.Descript=textDescription.Text;
			InsFilingCodeCur.EclaimCode=textEclaimCode.Text;
			InsFilingCodeCur.GroupType=comboGroup.GetSelectedDefNum();
			try {
				if(InsFilingCodeCur.IsNew) {
					InsFilingCodes.Insert(InsFilingCodeCur);
				}
				else {
					InsFilingCodes.Update(InsFilingCodeCur);
				}
			} 
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
		}

		private void CheckSubtypeButtonEnabled(){
			this.butAdd.Enabled=(this.textDescription.Text!="" && this.textEclaimCode.Text!="");
		}

		private void textDescription_TextChanged(object sender,EventArgs e) {
			CheckSubtypeButtonEnabled();
		}

		private void textEclaimCode_TextChanged(object sender,EventArgs e) {
			CheckSubtypeButtonEnabled();
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















