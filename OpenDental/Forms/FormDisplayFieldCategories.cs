using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	/// <summary></summary>
	public class FormDisplayFieldCategories:ODForm {
		private Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		//private bool changed;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private ListBox listCategory;
		private bool _isCemtMode;
		//private List<DisplayField> ListShowing;
		//private List<DisplayField> ListAvailable;

		///<summary></summary>
		public FormDisplayFieldCategories(bool isCemtMode=false)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_isCemtMode=isCemtMode;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDisplayFieldCategories));
			this.listCategory = new System.Windows.Forms.ListBox();
			this.butOK = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// listCategory
			// 
			this.listCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listCategory.FormattingEnabled = true;
			this.listCategory.Location = new System.Drawing.Point(23, 34);
			this.listCategory.Name = "listCategory";
			this.listCategory.Size = new System.Drawing.Size(225, 238);
			this.listCategory.TabIndex = 57;
			this.listCategory.DoubleClick += new System.EventHandler(this.listCategory_DoubleClick);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(92, 284);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 56;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(194, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Select a category";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(173, 284);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "Cancel";
			// 
			// FormDisplayFieldCategories
			// 
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(271, 323);
			this.Controls.Add(this.listCategory);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(217, 148);
			this.Name = "FormDisplayFieldCategories";
			this.ShowInTaskbar = false;
			this.Text = "Setup Display Fields";
			this.Load += new System.EventHandler(this.FormDisplayFields_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormDisplayFields_Load(object sender,EventArgs e) {
			//Alphabetical order.  When new display fields are added this will need to be changed.
			foreach(DisplayFieldCategory cat in Enum.GetValues(typeof(DisplayFieldCategory))
				.OfType<DisplayFieldCategory>().OrderBy(x => x.GetDescription())) 
			{
				if(cat == DisplayFieldCategory.None) {//skip None because user not allowed to select that
					continue;
				}
				bool displayIsCemtOnly=cat.GetAttributeOrDefault<PermissionAttribute>().IsCEMT;
				if(_isCemtMode != displayIsCemtOnly) {
					continue;
				}
				if(cat == DisplayFieldCategory.OrthoChart) { //orthochart tabs can have their own name.
					listCategory.Items.Add(new ODBoxItem<DisplayFieldCategory>(OrthoChartTabs.GetFirst(true).TabName,cat));
					continue;
				}
				listCategory.Items.Add(new ODBoxItem<DisplayFieldCategory>(Lan.g("enumDisplayFieldCategory",cat.GetDescription()),cat));
			}
			listCategory.SelectedIndex=0;
		}

		private void listCategory_DoubleClick(object sender,EventArgs e) {
			ShowCategoryEdit();
			Close();
		}

		private void ShowCategoryEdit() {
			DisplayFieldCategory selectedCategory = listCategory.GetSelected<DisplayFieldCategory>();
			if(selectedCategory==DisplayFieldCategory.None) {//should never happen.
				return;
			}
			//The ortho chart is a more complicated display field so it has its own window.
			if(selectedCategory==DisplayFieldCategory.OrthoChart) {
				FormDisplayFieldsOrthoChart FormDFOC=new FormDisplayFieldsOrthoChart();
				FormDFOC.ShowDialog();
			}
			else {//All other display fields use the base display fields window.
				FormDisplayFields FormF=new FormDisplayFields();
				FormF.Category=selectedCategory;
				FormF.ShowDialog();
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			ShowCategoryEdit();
			Close();
		}

		

		

		

		

		

		

		


	}
}





















