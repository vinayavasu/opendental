using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using OpenDental.UI;

namespace OpenDental{
///<summary></summary>
	public class FormDefinitions : ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textGuide;
		private System.Windows.Forms.GroupBox groupEdit;
		private System.Windows.Forms.ListBox listCategory;
		private System.Windows.Forms.Label label13;
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butHide;
		private UI.ODGrid gridDefs;
		private DefCat _initialCat;
		private bool _isDefChanged;
		private UI.Button butAlphabetize;
		private List<Def> _listDefsAll;

		///<summary>Gets the currently selected DefCat along with its options.</summary>
		private DefCatOptions _selectedDefCatOpt {
			get { return ((ODBoxItem<DefCatOptions>)listCategory.SelectedItem).Tag; }
		}

		///<summary>All definitions for the current category, hidden and non-hidden.</summary>
		private List<Def> _listDefsCur {
			get { return _listDefsAll.Where(x => x.Category == _selectedDefCatOpt.DefCat).OrderBy(x => x.ItemOrder).ToList(); }
		}

		///<summary>Must check security before allowing this window to open.</summary>
		public FormDefinitions(DefCat initialCat){
			InitializeComponent();// Required for Windows Form Designer support
			_initialCat=initialCat;
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDefinitions));
			this.butClose = new OpenDental.UI.Button();
			this.label14 = new System.Windows.Forms.Label();
			this.textGuide = new System.Windows.Forms.TextBox();
			this.groupEdit = new System.Windows.Forms.GroupBox();
			this.butAlphabetize = new OpenDental.UI.Button();
			this.butHide = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.listCategory = new System.Windows.Forms.ListBox();
			this.label13 = new System.Windows.Forms.Label();
			this.gridDefs = new OpenDental.UI.ODGrid();
			this.groupEdit.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(700, 670);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(78, 631);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(100, 18);
			this.label14.TabIndex = 22;
			this.label14.Text = "Guidelines";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textGuide
			// 
			this.textGuide.Location = new System.Drawing.Point(184, 631);
			this.textGuide.Multiline = true;
			this.textGuide.Name = "textGuide";
			this.textGuide.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textGuide.Size = new System.Drawing.Size(488, 63);
			this.textGuide.TabIndex = 2;
			// 
			// groupEdit
			// 
			this.groupEdit.Controls.Add(this.butAlphabetize);
			this.groupEdit.Controls.Add(this.butHide);
			this.groupEdit.Controls.Add(this.butDown);
			this.groupEdit.Controls.Add(this.butUp);
			this.groupEdit.Controls.Add(this.butAdd);
			this.groupEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupEdit.Location = new System.Drawing.Point(184, 576);
			this.groupEdit.Name = "groupEdit";
			this.groupEdit.Size = new System.Drawing.Size(488, 51);
			this.groupEdit.TabIndex = 1;
			this.groupEdit.TabStop = false;
			this.groupEdit.Text = "Edit Items";
			// 
			// butAlphabetize
			// 
			this.butAlphabetize.Location = new System.Drawing.Point(395, 20);
			this.butAlphabetize.Name = "butAlphabetize";
			this.butAlphabetize.Size = new System.Drawing.Size(75, 24);
			this.butAlphabetize.TabIndex = 21;
			this.butAlphabetize.Text = "Alphabetize";
			this.butAlphabetize.Click += new System.EventHandler(this.butAlphabetize_Click);
			// 
			// butHide
			// 
			this.butHide.Location = new System.Drawing.Point(107, 20);
			this.butHide.Name = "butHide";
			this.butHide.Size = new System.Drawing.Size(79, 24);
			this.butHide.TabIndex = 10;
			this.butHide.Text = "&Hide";
			this.butHide.Click += new System.EventHandler(this.butHide_Click);
			// 
			// butDown
			// 
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(299, 20);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(79, 24);
			this.butDown.TabIndex = 9;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(203, 20);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(79, 24);
			this.butUp.TabIndex = 8;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butAdd
			// 
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(11, 20);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 24);
			this.butAdd.TabIndex = 6;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// listCategory
			// 
			this.listCategory.Location = new System.Drawing.Point(11, 30);
			this.listCategory.Name = "listCategory";
			this.listCategory.Size = new System.Drawing.Size(167, 550);
			this.listCategory.TabIndex = 0;
			this.listCategory.SelectedIndexChanged += new System.EventHandler(this.listCategory_SelectedIndexChanged);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(11, 12);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(162, 17);
			this.label13.TabIndex = 17;
			this.label13.Text = "Select Category:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// gridDefs
			// 
			this.gridDefs.Location = new System.Drawing.Point(184, 30);
			this.gridDefs.Name = "gridDefs";
			this.gridDefs.Size = new System.Drawing.Size(488, 537);
			this.gridDefs.TabIndex = 23;
			this.gridDefs.Title = "Definitions";
			this.gridDefs.TranslationName = "TableDefinitions";
			this.gridDefs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridDefs_CellDoubleClick);
			// 
			// FormDefinitions
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(789, 707);
			this.Controls.Add(this.gridDefs);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.textGuide);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.groupEdit);
			this.Controls.Add(this.listCategory);
			this.Controls.Add(this.label13);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDefinitions";
			this.ShowInTaskbar = false;
			this.Text = "Definitions";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormDefinitions_Closing);
			this.Load += new System.EventHandler(this.FormDefinitions_Load);
			this.groupEdit.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDefinitions_Load(object sender, System.EventArgs e) {
			LoadListDefCats();
		}

		private void LoadListDefCats() {
			List<DefCatOptions> listDefCatsOrdered = new List<DefCatOptions>();
			listDefCatsOrdered=DefL.GetOptionsForDefCats(Enum.GetValues(typeof(DefCat)));
			listDefCatsOrdered = listDefCatsOrdered.OrderBy(x => x.DefCat.GetDescription()).ToList(); //orders alphabetically.
			ODBoxItem<DefCatOptions> defCatItem;
			foreach(DefCatOptions defCOpt in listDefCatsOrdered) {
				defCatItem=new ODBoxItem<DefCatOptions>(Lan.g(this,defCOpt.DefCat.GetDescription()),defCOpt);
				listCategory.Items.Add(defCatItem);
				if(_initialCat == defCOpt.DefCat) {
					listCategory.SelectedItem=defCatItem;
				}
			}
		}

		private void listCategory_SelectedIndexChanged(object sender,System.EventArgs e) {
			FillGridDefs();
		}

		private void RefreshDefs() {
			Defs.RefreshCache();
			_listDefsAll=Defs.GetDeepCopy().SelectMany(x => x.Value).ToList();
		}

		private void FillGridDefs(){
			if(_listDefsAll == null || _listDefsAll.Count == 0) {
				RefreshDefs();
			}
			DefL.FillGridDefs(gridDefs,_selectedDefCatOpt,_listDefsCur);
			//the following do not require a refresh of the table:
			if(_selectedDefCatOpt.CanHide) {
				butHide.Visible=true;
			}
			else {
				butHide.Visible=false;
			}
			if(_selectedDefCatOpt.CanEditName) {
				groupEdit.Enabled=true;
				groupEdit.Text=Lans.g(this,"Edit Items");
			}
			else {
				groupEdit.Enabled=false;
				groupEdit.Text=Lans.g(this,"Not allowed");
			}
			textGuide.Text=_selectedDefCatOpt.HelpText;
		}

		private void gridDefs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Def selectedDef = (Def)gridDefs.ListGridRows[e.Row].Tag;
			_isDefChanged=DefL.GridDefsDoubleClick(selectedDef,gridDefs,_selectedDefCatOpt,_listDefsCur,_listDefsAll,_isDefChanged);
			if(_isDefChanged) {
				RefreshDefs();
				FillGridDefs();
			}
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			if(DefL.AddDef(gridDefs,_selectedDefCatOpt)) {
				RefreshDefs();
				FillGridDefs();
				_isDefChanged=true;
			}
		}

		private void butHide_Click(object sender, System.EventArgs e) {
			if(DefL.HideDef(gridDefs,_selectedDefCatOpt)) {
				RefreshDefs();
				FillGridDefs();
				_isDefChanged=true;
			}
		}

		private void butUp_Click(object sender, System.EventArgs e) {
			if(DefL.UpClick(gridDefs)) {
				_isDefChanged=true;
				FillGridDefs();
			}
		}

		private void butDown_Click(object sender, System.EventArgs e) {
			if(DefL.DownClick(gridDefs)){
				_isDefChanged=true;
				FillGridDefs();
			}
		}

		private void butAlphabetize_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Alphabetizing does not have an 'undo' button.  Continue?")) {
				return;
			}
			List<Def> listDefsSorting=_listDefsCur.OrderBy(x => x.ItemName).ToList(); 
			for(int i=0;i < listDefsSorting.Count;i++) {
				listDefsSorting[i].ItemOrder=i;
				Defs.Update(listDefsSorting[i]);
			}
			_isDefChanged=true;
			RefreshDefs();
			FillGridDefs();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormDefinitions_Closing(object sender,System.ComponentModel.CancelEventArgs e) {
			//Correct the item orders of all definition categories.
			List<Def> listDefUpdates=new List<Def>();
			foreach(KeyValuePair<DefCat,List<Def>> kvp in Defs.GetDeepCopy()) {
				for(int i=0;i<kvp.Value.Count;i++) {
					if(kvp.Value[i].ItemOrder!=i) {
						kvp.Value[i].ItemOrder=i;
						listDefUpdates.Add(kvp.Value[i]);
					}
				}
			}
			listDefUpdates.ForEach(x => Defs.Update(x));
			if(_isDefChanged || listDefUpdates.Count>0) {
				//A specialty could have been renamed, invalidate the specialty associated to the currently selected patient just in case.
				PatientL.InvalidateSelectedPatSpecialty();
				DataValid.SetInvalid(InvalidType.Defs);
			}
		}

	}
}
