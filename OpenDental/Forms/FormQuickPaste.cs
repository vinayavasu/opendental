using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using OpenDental.UI;
using System.Linq;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormQuickPaste : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox listCat;
		private OpenDental.UI.Button butDownCat;
		private OpenDental.UI.Button butUpCat;
		private OpenDental.UI.Button butAddCat;
		private OpenDental.UI.Button butDeleteCat;
		private OpenDental.UI.Button butAddNote;
		private OpenDental.UI.Button butDownNote;
		private OpenDental.UI.Button butUpNote;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private List<QuickPasteNote> _listNotes;
		private List<QuickPasteNote> _listNotesOld;
		private List<QuickPasteCat> _listCats;
		private List<QuickPasteCat> _listCatsOld;
		private OpenDental.UI.Button butEditNote;
		private OpenDental.UI.Button butDate;
		//<summary>This is the note that gets passed back to the calling function.</summary>
		//public string SelectedNote;
		///<summary>Set this property before calling this form. It will insert the value into this textbox.</summary>
		public System.Windows.Forms.RichTextBox TextToFill;
		private UI.ODGrid gridMain;
		private bool _hasChanges;
		private UI.Button butAlphabetize;
		private bool _isSetupMode;
		private GroupBox groupBox1;
		private RadioButton radioSortByNote;
		private RadioButton radioSortByAbbrev;

		///<summary></summary>
		public QuickPasteType QuickType;

		///<summary></summary>
		public FormQuickPaste(bool isSetupMode){
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_isSetupMode=isSetupMode;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuickPaste));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.listCat = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butDownCat = new OpenDental.UI.Button();
			this.butUpCat = new OpenDental.UI.Button();
			this.butAddCat = new OpenDental.UI.Button();
			this.butDeleteCat = new OpenDental.UI.Button();
			this.butAddNote = new OpenDental.UI.Button();
			this.butDownNote = new OpenDental.UI.Button();
			this.butUpNote = new OpenDental.UI.Button();
			this.butEditNote = new OpenDental.UI.Button();
			this.butDate = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAlphabetize = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioSortByNote = new System.Windows.Forms.RadioButton();
			this.radioSortByAbbrev = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(899, 641);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(818, 641);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// listCat
			// 
			this.listCat.Location = new System.Drawing.Point(8, 25);
			this.listCat.Name = "listCat";
			this.listCat.Size = new System.Drawing.Size(169, 316);
			this.listCat.TabIndex = 2;
			this.listCat.DoubleClick += new System.EventHandler(this.listCat_DoubleClick);
			this.listCat.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listCat_MouseDown);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 19);
			this.label1.TabIndex = 3;
			this.label1.Text = "Category";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(181, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 19);
			this.label2.TabIndex = 5;
			this.label2.Text = "Note";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butDownCat
			// 
			this.butDownCat.Image = global::OpenDental.Properties.Resources.down;
			this.butDownCat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDownCat.Location = new System.Drawing.Point(98, 383);
			this.butDownCat.Name = "butDownCat";
			this.butDownCat.Size = new System.Drawing.Size(79, 26);
			this.butDownCat.TabIndex = 11;
			this.butDownCat.Text = "Down";
			this.butDownCat.Click += new System.EventHandler(this.butDownCat_Click);
			// 
			// butUpCat
			// 
			this.butUpCat.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUpCat.Image = global::OpenDental.Properties.Resources.up;
			this.butUpCat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUpCat.Location = new System.Drawing.Point(98, 348);
			this.butUpCat.Name = "butUpCat";
			this.butUpCat.Size = new System.Drawing.Size(79, 26);
			this.butUpCat.TabIndex = 10;
			this.butUpCat.Text = "Up";
			this.butUpCat.Click += new System.EventHandler(this.butUpCat_Click);
			// 
			// butAddCat
			// 
			this.butAddCat.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddCat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddCat.Location = new System.Drawing.Point(8, 348);
			this.butAddCat.Name = "butAddCat";
			this.butAddCat.Size = new System.Drawing.Size(79, 26);
			this.butAddCat.TabIndex = 12;
			this.butAddCat.Text = "Add";
			this.butAddCat.Click += new System.EventHandler(this.butAddCat_Click);
			// 
			// butDeleteCat
			// 
			this.butDeleteCat.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDeleteCat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeleteCat.Location = new System.Drawing.Point(8, 383);
			this.butDeleteCat.Name = "butDeleteCat";
			this.butDeleteCat.Size = new System.Drawing.Size(79, 26);
			this.butDeleteCat.TabIndex = 13;
			this.butDeleteCat.Text = "Delete";
			this.butDeleteCat.Click += new System.EventHandler(this.butDeleteCat_Click);
			// 
			// butAddNote
			// 
			this.butAddNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddNote.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddNote.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddNote.Location = new System.Drawing.Point(184, 641);
			this.butAddNote.Name = "butAddNote";
			this.butAddNote.Size = new System.Drawing.Size(79, 26);
			this.butAddNote.TabIndex = 16;
			this.butAddNote.Text = "Add";
			this.butAddNote.Click += new System.EventHandler(this.butAddNote_Click);
			// 
			// butDownNote
			// 
			this.butDownNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDownNote.Image = global::OpenDental.Properties.Resources.down;
			this.butDownNote.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDownNote.Location = new System.Drawing.Point(439, 641);
			this.butDownNote.Name = "butDownNote";
			this.butDownNote.Size = new System.Drawing.Size(79, 26);
			this.butDownNote.TabIndex = 15;
			this.butDownNote.Text = "Down";
			this.butDownNote.Click += new System.EventHandler(this.butDownNote_Click);
			// 
			// butUpNote
			// 
			this.butUpNote.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUpNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUpNote.Image = global::OpenDental.Properties.Resources.up;
			this.butUpNote.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUpNote.Location = new System.Drawing.Point(354, 641);
			this.butUpNote.Name = "butUpNote";
			this.butUpNote.Size = new System.Drawing.Size(79, 26);
			this.butUpNote.TabIndex = 14;
			this.butUpNote.Text = "Up";
			this.butUpNote.Click += new System.EventHandler(this.butUpNote_Click);
			// 
			// butEditNote
			// 
			this.butEditNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditNote.Image = global::OpenDental.Properties.Resources.editPencil;
			this.butEditNote.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEditNote.Location = new System.Drawing.Point(269, 641);
			this.butEditNote.Name = "butEditNote";
			this.butEditNote.Size = new System.Drawing.Size(79, 26);
			this.butEditNote.TabIndex = 17;
			this.butEditNote.Text = "Edit";
			this.butEditNote.Click += new System.EventHandler(this.butEditNote_Click);
			// 
			// butDate
			// 
			this.butDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDate.Location = new System.Drawing.Point(737, 641);
			this.butDate.Name = "butDate";
			this.butDate.Size = new System.Drawing.Size(75, 26);
			this.butDate.TabIndex = 18;
			this.butDate.Text = "Date";
			this.butDate.Click += new System.EventHandler(this.butDate_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(184, 28);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(790, 601);
			this.gridMain.TabIndex = 19;
			this.gridMain.Title = "Notes";
			this.gridMain.TranslationName = "TableNotes";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAlphabetize
			// 
			this.butAlphabetize.Location = new System.Drawing.Point(6, 14);
			this.butAlphabetize.Name = "butAlphabetize";
			this.butAlphabetize.Size = new System.Drawing.Size(75, 26);
			this.butAlphabetize.TabIndex = 20;
			this.butAlphabetize.Text = "Alphabetize";
			this.butAlphabetize.Click += new System.EventHandler(this.butAlphabetize_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.radioSortByNote);
			this.groupBox1.Controls.Add(this.radioSortByAbbrev);
			this.groupBox1.Controls.Add(this.butAlphabetize);
			this.groupBox1.Location = new System.Drawing.Point(524, 627);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(207, 46);
			this.groupBox1.TabIndex = 21;
			this.groupBox1.TabStop = false;
			// 
			// radioSortByNote
			// 
			this.radioSortByNote.Location = new System.Drawing.Point(88, 26);
			this.radioSortByNote.Name = "radioSortByNote";
			this.radioSortByNote.Size = new System.Drawing.Size(85, 17);
			this.radioSortByNote.TabIndex = 22;
			this.radioSortByNote.Text = "Note";
			this.radioSortByNote.UseVisualStyleBackColor = true;
			// 
			// radioSortByAbbrev
			// 
			this.radioSortByAbbrev.Checked = true;
			this.radioSortByAbbrev.Location = new System.Drawing.Point(88, 9);
			this.radioSortByAbbrev.Name = "radioSortByAbbrev";
			this.radioSortByAbbrev.Size = new System.Drawing.Size(113, 17);
			this.radioSortByAbbrev.TabIndex = 21;
			this.radioSortByAbbrev.TabStop = true;
			this.radioSortByAbbrev.Text = "Abbreviation";
			this.radioSortByAbbrev.UseVisualStyleBackColor = true;
			// 
			// FormQuickPaste
			// 
			this.ClientSize = new System.Drawing.Size(986, 677);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butDate);
			this.Controls.Add(this.butEditNote);
			this.Controls.Add(this.butAddNote);
			this.Controls.Add(this.butDownNote);
			this.Controls.Add(this.butUpNote);
			this.Controls.Add(this.butDeleteCat);
			this.Controls.Add(this.butAddCat);
			this.Controls.Add(this.butDownCat);
			this.Controls.Add(this.butUpCat);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listCat);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.groupBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(1002, 458);
			this.Name = "FormQuickPaste";
			this.ShowInTaskbar = false;
			this.Text = "Quick Paste Notes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQuickPaste_FormClosing);
			this.Load += new System.EventHandler(this.FormQuickPaste_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormQuickPaste_Load(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit,true)) {
				butAddCat.Enabled=false;
				butDeleteCat.Enabled=false;
				butAddNote.Enabled=false;
				butUpCat.Enabled=false;
				butUpNote.Enabled=false;
				butDownCat.Enabled=false;
				butDownNote.Enabled=false;
				butAlphabetize.Enabled=false;
				butEditNote.Text="View";
			}
			if(TextToFill==null) {
				butDate.Visible=false;
			}
			_listCats=QuickPasteCats.GetDeepCopy();
			_listCatsOld=_listCats.Select(x=>x.Copy()).ToList();
			FillCats();
			listCat.SelectedIndex=QuickPasteCats.GetDefaultType(QuickType);
			_listNotes=QuickPasteNotes.GetDeepCopy();
			_listNotesOld=_listNotes.Select(x=>x.Copy()).ToList();
			FillMain();
		}

		private void FillCats() {
			int selected=listCat.SelectedIndex;
			listCat.Items.Clear();
			foreach(QuickPasteCat category in _listCats) {
				listCat.Items.Add(category.Description);
			}
			if(selected<listCat.Items.Count) {
				listCat.SelectedIndex=selected;
			}
			if(listCat.SelectedIndex==-1) {
				listCat.SelectedIndex=listCat.Items.Count-1;
			}
		}

		private void FillMain() {
			int selectedIdx=gridMain.GetSelectedIndex();
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridColumns.Add(new GridColumn("Abbr",75));
			gridMain.ListGridColumns.Add(new GridColumn("Note",600));
			gridMain.ListGridRows.Clear();
			if(listCat.SelectedIndex==-1) {
				gridMain.EndUpdate();
				return;
			}
			GridRow row;
			_listNotes=_listNotes.OrderBy(x => x.ItemOrder).ToList();
			foreach(QuickPasteNote note in _listNotes) {
				if(note.QuickPasteCatNum!=_listCats[listCat.SelectedIndex].QuickPasteCatNum) {
					continue;
				}
				row=new GridRow();
				row.Cells.Add(string.IsNullOrWhiteSpace(note.Abbreviation)?"":"?"+note.Abbreviation);
				row.Cells.Add(note.Note.Replace("\r","").Replace("\n","").Left(120,true));
				row.Tag=note;
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
			if(selectedIdx==-1) {//Select the last option.
				gridMain.SetSelected(gridMain.ListGridRows.Count-1,true);
				gridMain.ScrollToEnd();
			}
			else if(selectedIdx<gridMain.ListGridRows.Count) {//Select the previously selected position.
				gridMain.SetSelected(selectedIdx,true);
			}
		}

		private void butAddCat_Click(object sender, System.EventArgs e) {
			QuickPasteCat quickCat=new QuickPasteCat();
      FormQuickPasteCat FormQ=new FormQuickPasteCat(quickCat);
			FormQ.ShowDialog();
			if(FormQ.DialogResult!=DialogResult.OK) {
				return;
			}
			quickCat=FormQ.QuickCat;
			QuickPasteCats.Insert(quickCat);
			_hasChanges=true;
			//We are doing this so tha when the sync is called in FormQuickPaste_FormClosing(...) we do not re-insert.
			//For now the sync will still detect a change due to the item orders.
			_listCatsOld.Add(quickCat.Copy());
			if(listCat.SelectedIndex!=-1) {
				_listCats.Insert(listCat.SelectedIndex,quickCat);//insert at selectedindex AND selects new category when we refill grid below.
			}
			else {//Will only happen if they do not have any categories.
				_listCats.Add(quickCat);//add to bottom of list, will be selected when we fill grid below.
			}			
			FillCats();
			FillMain();
		}

		private void butDeleteCat_Click(object sender,System.EventArgs e) {
			if(listCat.SelectedIndex==-1) {
				MessageBox.Show(Lan.g(this,"Please select a category first."));
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Are you sure you want to delete the entire category and all notes in it?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			QuickPasteCat category=_listCats[listCat.SelectedIndex];
			_listCats.Remove(category);
			_listNotes.RemoveAll(x=>x.QuickPasteCatNum==category.QuickPasteCatNum);
			FillCats();
			FillMain();
		}

		private void butUpCat_Click(object sender,System.EventArgs e) {
			if(listCat.SelectedIndex==-1) {
				MessageBox.Show(Lan.g(this,"Please select a category first."));
				return;
			}
			if(listCat.SelectedIndex==0) {
				return;//can't go up any more
			}
			_listCats.Reverse(listCat.SelectedIndex-1,2);
			listCat.SelectedIndex--;
			FillCats();
			FillMain();
		}

		private void butDownCat_Click(object sender,System.EventArgs e) {
			if(listCat.SelectedIndex==-1) {
				MessageBox.Show(Lan.g(this,"Please select a category first."));
				return;
			}
			if(listCat.SelectedIndex==_listCats.Count-1) {
				return;//can't go down any more
			}
			_listCats.Reverse(listCat.SelectedIndex,2);
			listCat.SelectedIndex++;
			FillCats();
			FillMain();
		}

		private void listCat_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			FillMain();
		}

		private void listCat_DoubleClick(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit)) {
				return;
			}
			if(listCat.SelectedIndex==-1) {
				return;
			}
			FormQuickPasteCat FormQ=new FormQuickPasteCat(_listCats[listCat.SelectedIndex]);
			FormQ.ShowDialog();
			if(FormQ.DialogResult!=DialogResult.OK) {
				return;
			}
			_listCats[listCat.SelectedIndex]=FormQ.QuickCat;
			FillCats();
			FillMain();
		}

		private void butAddNote_Click(object sender,System.EventArgs e) {
			if(listCat.SelectedIndex==-1) {
				MessageBox.Show(Lan.g(this,"Please select a category first."));
				return;
			}
			QuickPasteNote quickNote=new QuickPasteNote();
			quickNote.QuickPasteCatNum=_listCats[listCat.SelectedIndex].QuickPasteCatNum;
			FormQuickPasteNoteEdit FormQ=new FormQuickPasteNoteEdit(quickNote);
			FormQ.ShowDialog();
			if(FormQ.DialogResult!=DialogResult.OK || FormQ.QuickNote==null) {//Deleted
				return;
			}
			if(gridMain.GetSelectedIndex()==-1) {
				_listNotes.Add(FormQ.QuickNote);
			}
			else { 
				int selectedIdx=_listNotes.IndexOf((QuickPasteNote)gridMain.ListGridRows[gridMain.GetSelectedIndex()].Tag);
				_listNotes.Insert(selectedIdx,FormQ.QuickNote);//Insert the new note at the selected index.
			}			
			FillMain();
		}

		private void butEditNote_Click(object sender,System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MessageBox.Show(Lan.g(this,"Please select a note first."));
				return;
			}
			QuickPasteNote quickNote=(QuickPasteNote)gridMain.ListGridRows[gridMain.GetSelectedIndex()].Tag;
			ShowEditWindow(quickNote);
		}

		private void ShowEditWindow(QuickPasteNote quickNote) {
			FormQuickPasteNoteEdit FormQ=new FormQuickPasteNoteEdit((QuickPasteNote)gridMain.ListGridRows[gridMain.GetSelectedIndex()].Tag);
			FormQ.ShowDialog();
			if(FormQ.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormQ.QuickNote==null) {//deleted
				_listNotes.Remove(quickNote);
			}
			else { 
				_listNotes[_listNotes.IndexOf(quickNote)]=FormQ.QuickNote;
			}
			FillMain();
		}

		private void butUpNote_Click(object sender, System.EventArgs e) {
			MoveNote(false);
		}

		private void butDownNote_Click(object sender, System.EventArgs e) {
			MoveNote(true);
		}

		/// <summary>Moves the selected note either down or up in the grid and sets the itemOrder for each.</summary>
		private void MoveNote(bool isDown) {
			int selectedIdx=gridMain.GetSelectedIndex();
			int destinationIdx=(selectedIdx+(isDown?1:-1));
			if(selectedIdx==-1) {
				MessageBox.Show(Lan.g(this,"Please select a note first."));
				return;
			}
			if(!destinationIdx.Between(0,gridMain.ListGridRows.Count-1)) {
				return;//can't go up or down any more
			}
			QuickPasteNote sourceNote=(QuickPasteNote)gridMain.ListGridRows[selectedIdx].Tag;
			QuickPasteNote destNote=(QuickPasteNote)gridMain.ListGridRows[destinationIdx].Tag;
			sourceNote.ItemOrder=_listNotes.IndexOf(destNote);
			destNote.ItemOrder=_listNotes.IndexOf(sourceNote);
			gridMain.SetSelected(false);
			FillMain();
			gridMain.SetSelected(destinationIdx,true);
		}

		private void butAlphabetize_Click(object sender,EventArgs e) {
			//Since this string is hardcoded we can pass them into the MsgBox for translation even with the variable
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Quick Paste Notes will be ordered alphabetically by "
				+(radioSortByAbbrev.Checked?"abbreviation":"note")
				+".  This cannot be undone.  Continue?")) 
			{
				return;
			}
			List<QuickPasteNote> listNotesForCat=_listNotes.FindAll(x => x.QuickPasteCatNum==_listCats[listCat.SelectedIndex].QuickPasteCatNum);
			if(radioSortByAbbrev.Checked) {
				listNotesForCat=listNotesForCat.OrderBy(x => x.Abbreviation).ToList();
			}
			else {
				listNotesForCat=listNotesForCat.OrderBy(x => x.Note).ToList();
			}
			List<int> listIndices=listNotesForCat.Select(x => x.ItemOrder).OrderBy(x => x).ToList();
			for(int i=0;i<listNotesForCat.Count;i++) {
				listNotesForCat[i].ItemOrder=listIndices[i];//lists are 1:1
			}
			_hasChanges=true;
			FillMain();
		}

		private void gridMain_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			if(_isSetupMode) {
				QuickPasteNote quickNote=(QuickPasteNote)gridMain.ListGridRows[gridMain.GetSelectedIndex()].Tag;
				ShowEditWindow(quickNote);
			}
			else {
				InsertValue(((QuickPasteNote)gridMain.ListGridRows[gridMain.GetSelectedIndex()].Tag).Note);
				DialogResult=DialogResult.OK;
			}
		}

		private void butDate_Click(object sender, System.EventArgs e) {
			InsertValue(DateTime.Today.ToShortDateString());
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(TextToFill!=null) {
				if(gridMain.GetSelectedIndex()==-1) {
					MessageBox.Show(Lan.g(this,"Please select a note first."));
					return;
				}
				InsertValue(((QuickPasteNote)gridMain.ListGridRows[gridMain.GetSelectedIndex()].Tag).Note);
			}
			DialogResult=DialogResult.OK;
		}

		private void InsertValue(string strPaste) {
			if(TextToFill==null) {
				return;
			}
			try {
				//When trying to paste plain text into the Rtf text, an exception will throw.
				TextToFill.SelectedRtf=strPaste;
			}
			catch(Exception) {
				//If we couldn't paste into the Rtf text, try to paste into the plain text section.
				try {
					TextToFill.SelectedText=strPaste;
				}
				catch(Exception) {
					//If pasting into the Rtf AND the plain text fails, notify the user.
					MsgBox.Show(this,"There was a problem pasting clipboard contents.");
				}
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormQuickPaste_FormClosing(object sender,FormClosingEventArgs e) {
			for(int i=0;i<_listNotes.Count;i++) {
				_listNotes[i].ItemOrder=i;//Fix item orders.
			}
			for(int i=0;i<_listCats.Count;i++) {
				_listCats[i].ItemOrder=i;//Fix item orders.
			}
			_hasChanges|=QuickPasteCats.Sync(_listCats,_listCatsOld);
			if(QuickPasteNotes.Sync(_listNotes,_listNotesOld) || _hasChanges) {
				SecurityLogs.MakeLogEntry(Permissions.AutoNoteQuickNoteEdit,0,"Quick Paste Notes/Cats Changed");
				DataValid.SetInvalid(InvalidType.QuickPaste);
			}
		}

	}
}





















