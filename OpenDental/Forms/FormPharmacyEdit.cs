using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary></summary>
	public class FormPharmacyEdit:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textStoreName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.TextBox textCity;
		private System.Windows.Forms.TextBox textState;
		private System.Windows.Forms.TextBox textZip;
		private System.Windows.Forms.TextBox textAddress2;
		private System.Windows.Forms.TextBox textAddress;
		private ValidPhone textPhone;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label6;
		private ValidPhone textFax;
		private Label label5;
		private Label label7;
		private TextBox textNote;
		private Label label8;
		private UI.ComboBoxClinicPicker comboClinic;

		///<summary>Current pharmacy that is being edited.</summary>
		public Pharmacy PharmCur;

		///<summary></summary>
		public FormPharmacyEdit()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPharmacyEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textStoreName = new System.Windows.Forms.TextBox();
			this.butDelete = new OpenDental.UI.Button();
			this.textCity = new System.Windows.Forms.TextBox();
			this.textState = new System.Windows.Forms.TextBox();
			this.textZip = new System.Windows.Forms.TextBox();
			this.textAddress2 = new System.Windows.Forms.TextBox();
			this.textAddress = new System.Windows.Forms.TextBox();
			this.textPhone = new OpenDental.ValidPhone();
			this.label11 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textFax = new OpenDental.ValidPhone();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.comboClinic = new OpenDental.UI.ComboBoxClinicPicker();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(456, 286);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 10;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(365, 286);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 9;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Store Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textStoreName
			// 
			this.textStoreName.Location = new System.Drawing.Point(160, 20);
			this.textStoreName.Name = "textStoreName";
			this.textStoreName.Size = new System.Drawing.Size(291, 20);
			this.textStoreName.TabIndex = 0;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(27, 286);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(81, 26);
			this.butDelete.TabIndex = 4;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textCity
			// 
			this.textCity.Location = new System.Drawing.Point(160, 125);
			this.textCity.MaxLength = 255;
			this.textCity.Name = "textCity";
			this.textCity.Size = new System.Drawing.Size(155, 20);
			this.textCity.TabIndex = 5;
			// 
			// textState
			// 
			this.textState.Location = new System.Drawing.Point(315, 125);
			this.textState.MaxLength = 255;
			this.textState.Name = "textState";
			this.textState.Size = new System.Drawing.Size(65, 20);
			this.textState.TabIndex = 6;
			// 
			// textZip
			// 
			this.textZip.Location = new System.Drawing.Point(380, 125);
			this.textZip.MaxLength = 255;
			this.textZip.Name = "textZip";
			this.textZip.Size = new System.Drawing.Size(71, 20);
			this.textZip.TabIndex = 7;
			// 
			// textAddress2
			// 
			this.textAddress2.Location = new System.Drawing.Point(160, 104);
			this.textAddress2.MaxLength = 255;
			this.textAddress2.Name = "textAddress2";
			this.textAddress2.Size = new System.Drawing.Size(291, 20);
			this.textAddress2.TabIndex = 4;
			// 
			// textAddress
			// 
			this.textAddress.Location = new System.Drawing.Point(160, 83);
			this.textAddress.MaxLength = 255;
			this.textAddress.Name = "textAddress";
			this.textAddress.Size = new System.Drawing.Size(291, 20);
			this.textAddress.TabIndex = 3;
			// 
			// textPhone
			// 
			this.textPhone.Location = new System.Drawing.Point(160, 41);
			this.textPhone.MaxLength = 255;
			this.textPhone.Name = "textPhone";
			this.textPhone.Size = new System.Drawing.Size(157, 20);
			this.textPhone.TabIndex = 1;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(-5, 129);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(163, 15);
			this.label11.TabIndex = 105;
			this.label11.Text = "City, ST, Zip";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7, 108);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(151, 17);
			this.label4.TabIndex = 103;
			this.label4.Text = "Address2";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 85);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(151, 17);
			this.label3.TabIndex = 101;
			this.label3.Text = "Address";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(151, 17);
			this.label2.TabIndex = 99;
			this.label2.Text = "Phone";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(320, 43);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(144, 18);
			this.label6.TabIndex = 110;
			this.label6.Text = "(###)###-####";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textFax
			// 
			this.textFax.Location = new System.Drawing.Point(160, 62);
			this.textFax.MaxLength = 255;
			this.textFax.Name = "textFax";
			this.textFax.Size = new System.Drawing.Size(157, 20);
			this.textFax.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 65);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(151, 17);
			this.label5.TabIndex = 112;
			this.label5.Text = "Fax";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(320, 64);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(144, 18);
			this.label7.TabIndex = 113;
			this.label7.Text = "(###)###-####";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(160, 168);
			this.textNote.MaxLength = 255;
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(291, 107);
			this.textNote.TabIndex = 8;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(7, 172);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(151, 17);
			this.label8.TabIndex = 115;
			this.label8.Text = "Note";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboClinic
			// 
			this.comboClinic.BackColor = System.Drawing.SystemColors.Control;
			this.comboClinic.IncludeAll = true;
			this.comboClinic.IncludeUnassigned = true;
			this.comboClinic.Location = new System.Drawing.Point(123, 146);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.SelectionModeMulti = true;
			this.comboClinic.Size = new System.Drawing.Size(195, 21);
			this.comboClinic.TabIndex = 116;
			// 
			// FormPharmacyEdit
			// 
			this.ClientSize = new System.Drawing.Size(557, 330);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textFax);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textCity);
			this.Controls.Add(this.textState);
			this.Controls.Add(this.textZip);
			this.Controls.Add(this.textAddress2);
			this.Controls.Add(this.textAddress);
			this.Controls.Add(this.textPhone);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textStoreName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label6);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPharmacyEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Pharmacy";
			this.Load += new System.EventHandler(this.FormPharmacyEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPharmacyEdit_Load(object sender, System.EventArgs e) {
			textStoreName.Text=PharmCur.StoreName;
			textPhone.Text=PharmCur.Phone;
			textFax.Text=PharmCur.Fax;
			textAddress.Text=PharmCur.Address;
			textAddress2.Text=PharmCur.Address2;
			textCity.Text=PharmCur.City;
			textState.Text=PharmCur.State;
			textZip.Text=PharmCur.Zip;
			textNote.Text=PharmCur.Note;
			//Selects all clinics that have a link to this pharmacy. This will only display and select the clinics the user has access to.
			List<PharmClinic> listPharmClinics=PharmClinics.GetPharmClinicsForPharmacy(PharmCur.PharmacyNum);
			comboClinic.ListSelectedClinicNums=listPharmClinics.Select(x => x.ClinicNum).ToList();
			//Save the currently selected clinics for synching down below.
			comboClinic.Tag=listPharmClinics.Where(x => comboClinic.ListSelectedClinicNums.Contains(x.ClinicNum)).ToList();
			//remember, this is NOT all PharmClinics for this Pharmacy, but only the ones that this user has permission to see
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(PharmCur.IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,true,"Delete this Pharmacy?")) {
				return;
			}
			try{
				Pharmacies.DeleteObject(PharmCur.PharmacyNum);
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textStoreName.Text==""){
				MessageBox.Show(Lan.g(this,"Store name cannot be blank."));
				return;
			}
			if(CultureInfo.CurrentCulture.Name=="en-US"){
				if(textPhone.Text!="" && TelephoneNumbers.FormatNumbersExactTen(textPhone.Text)==""){
					MessageBox.Show(Lan.g(this,"Phone number must be in a 10-digit format."));
					return;
				}
				if(textFax.Text!="" && TelephoneNumbers.FormatNumbersExactTen(textFax.Text)==""){
					MessageBox.Show(Lan.g(this,"Fax number must be in a 10-digit format."));
					return;
				}
			}
			PharmCur.StoreName=textStoreName.Text;
			PharmCur.PharmID="";
			PharmCur.Phone=textPhone.Text;
			PharmCur.Fax=textFax.Text;
			PharmCur.Address=textAddress.Text;
			PharmCur.Address2=textAddress2.Text;
			PharmCur.City=textCity.Text;
			PharmCur.State=textState.Text;
			PharmCur.Zip=textZip.Text;
			PharmCur.Note=textNote.Text;
			try{
				if(PharmCur.IsNew){
					Pharmacies.Insert(PharmCur);
				}
				else{
					Pharmacies.Update(PharmCur);
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			//Update PharmClinic links
			List<PharmClinic> listPharmClinicOld=(List<PharmClinic>)comboClinic.Tag;
			List<PharmClinic> listPharmClinicNew=new List<PharmClinic>();
			//comboClinic.All might be selected, and would result in ListSelectedClinicNums containing only the clinics showing in the combobox, which very will not include clinics that user does not have permissions for.  "All" is not separately tested for.  Because the new list is synched against the old list, clinics that aren't showing are not affected one way or the other.
			foreach(long clinicNumNew in comboClinic.ListSelectedClinicNums) {
				if(listPharmClinicOld.Any(x => x.ClinicNum==clinicNumNew)) {//if it existed before, add it to the list
					listPharmClinicNew.Add(listPharmClinicOld.First(x => x.ClinicNum==clinicNumNew));
				}
				else {//otherwise, create a new link.
					listPharmClinicNew.Add(new PharmClinic(PharmCur.PharmacyNum,clinicNumNew));
				}
			}
			PharmClinics.Sync(listPharmClinicNew,listPharmClinicOld);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		

		


	}
}





















