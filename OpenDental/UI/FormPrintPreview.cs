using System;
using System.Drawing.Printing;
using System.Windows.Forms;
using CodeBase;

namespace OpenDental.UI{
	///<summary></summary>
	public class FormPrintPreview:ODForm {
		private System.ComponentModel.IContainer components;
		///<summary></summary>
		private int TotalPages;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.ImageList imageListMain;
		private OpenDental.UI.ODPrintPreviewControl printPreviewControl2;
		private ODprintout _printout;
		///<summary></summary>
		private PrintDocument Document;
		///<summary></summary>
		private PrintSituation Sit;
		long PatNumCur;
		string AuditDescription;
		
		///<summary>DEPRECATED:  Must supply the printSituation so that when user clicks print, we know where to send it.  Must supply total pages.  PatNum and AuditDescription used to make audit log entry.  PatNum can be 0.  Audit Log Text will show AuditDescription exactly.</summary>
		public FormPrintPreview(PrintSituation sit,PrintDocument document,int totalPages,long patNum,string auditDescription) {
			InitializeComponent();// Required for Windows Form Designer support
			Sit=sit;
			Document=document;
			TotalPages=totalPages;
			PatNumCur=patNum;
			AuditDescription=auditDescription;
		}

		///<summary>Uses the following ODprintout fields: Situation, TotalPages, AuditPatNum, AuditDescription.</summary>
		public FormPrintPreview(ODprintout printout) {
			InitializeComponent();// Required for Windows Form Designer support
			_printout=printout;
			TotalPages=printout.TotalPages;
		}
		
		/// <summary>Clean up any resources being used.</summary>
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrintPreview));
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.printPreviewControl2 = new OpenDental.UI.ODPrintPreviewControl();
			this.SuspendLayout();
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0,0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(831,25);
			this.ToolBarMain.TabIndex = 5;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0,"");
			this.imageListMain.Images.SetKeyName(1,"");
			this.imageListMain.Images.SetKeyName(2,"");
			// 
			// printPreviewControl2
			// 
			this.printPreviewControl2.AutoZoom = false;
			this.printPreviewControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.printPreviewControl2.Location = new System.Drawing.Point(0,0);
			this.printPreviewControl2.Name = "printPreviewControl2";
			this.printPreviewControl2.Size = new System.Drawing.Size(831,570);
			this.printPreviewControl2.TabIndex = 6;
			// 
			// FormPrintPreview
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(831,570);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.printPreviewControl2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormPrintPreview";
			this.ShowInTaskbar = false;
			this.Text = "Print Preview";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.FormPrintPreview_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FormReport_Layout);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormPrintPreview_Load(object sender, System.EventArgs e) {
			LayoutToolBar();
			PrintDocument printdoc=Document;//Default to the old printing pattern.
			if(_printout==null) {//Old print pattern.
				//Try to find the printable area.  If there are no printers, it will throw an InvalidPrinterException
				if(PrinterSettings.InstalledPrinters.Count==0) {
					MsgBox.Show(this,"Error: No Printers Installed\r\n"+
										"If you do have a printer installed, restarting the workstation may solve the problem."
					);
					DialogResult=DialogResult.Cancel;
					return;
				}
				if(printdoc.DefaultPageSettings.PrintableArea.Height==0) {
					printdoc.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
				}
			}
			else {//New print pattern
				if(_printout.SettingsErrorCode!=PrintoutErrorCode.Success) {
					PrinterL.ShowError(_printout);
					DialogResult=DialogResult.Cancel;
					return;
				}
				printdoc=_printout.PrintDoc;
			}
			LoadPrintDocument(printdoc);
			SetPageNumText();
		}

		private void LoadPrintDocument(PrintDocument printdoc) {
			//if document fits within window, then don't zoom it bigger; leave it at 100%
			if(printdoc.DefaultPageSettings.PaperSize.Height<printPreviewControl2.ClientSize.Height
				&& printdoc.DefaultPageSettings.PaperSize.Width<printPreviewControl2.ClientSize.Width)
			{
				printPreviewControl2.Zoom=1;
			}
			//if document ratio is taller than screen ratio, shrink by height.
			else if(printdoc.DefaultPageSettings.PaperSize.Height
				/printdoc.DefaultPageSettings.PaperSize.Width
				> printPreviewControl2.ClientSize.Height / printPreviewControl2.ClientSize.Width)
			{
				printPreviewControl2.Zoom=((double)printPreviewControl2.ClientSize.Height
					/(double)printdoc.DefaultPageSettings.PaperSize.Height);
			}
			//otherwise, shrink by width
			else{
				printPreviewControl2.Zoom=((double)printPreviewControl2.ClientSize.Width
					/(double)printdoc.DefaultPageSettings.PaperSize.Width);
			}
			printPreviewControl2.Document=printdoc;
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print"),0,"","Print"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",1,"Go Back One Page","Back"));
			ODToolBarButton button=new ODToolBarButton("",-1,"","PageNum");
			button.Style=ODToolBarButtonStyle.Label;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton("",2,"Go Forward One Page","Fwd"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,"Close This Window","Close"));
		}

		private void FormReport_Layout(object sender, System.Windows.Forms.LayoutEventArgs e) {
			printPreviewControl2.Width=ClientSize.Width;	
		}

		private delegate void PrintClick();

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			//MessageBox.Show(e.Button.Tag.ToString());
			switch(e.Button.Tag.ToString()){
				case "Print":
					//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
					//when it comes from a toolbar click.
					//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
					PrintClick printClick=OnPrint_Click;
					this.BeginInvoke(printClick);
					break;
				case "Back":
					OnBack_Click();
					break;
				case "Fwd":
					OnFwd_Click();
					break;
				case "Close":
					OnClose_Click();
					break;
			}
		}

		private void OnPrint_Click() {
			if(_printout==null) {//Old print pattern
				if(!PrinterL.SetPrinter(Document,Sit,PatNumCur,AuditDescription)){
					return;
				}
				if(Document.OriginAtMargins){
					//In the sheets framework,we had to set margins to 20 because of a bug in their preview control.
					//We now need to set it back to 0 for the actual printing.
					//Hopefully, this doesn't break anything else.
					Document.DefaultPageSettings.Margins=new Margins(0,0,0,0);
				}
				try{
					Document.Print();
				}
				catch(Exception e){
					MessageBox.Show(Lan.g(this,"Error: ")+e.Message);
				}
			}
			else {//New print pattern
				if(_printout.PrintDoc.OriginAtMargins) {
					//In the sheets framework,we had to set margins to 20 because of a bug in their preview control.
					//We now need to set it back to 0 for the actual printing.
					//Hopefully, this doesn't break anything else.
					_printout.PrintDoc.DefaultPageSettings.Margins=new Margins(0,0,0,0);
				}
				if(!PrinterL.TryPrint(_printout)) {
					return;
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void OnClose_Click() {
			this.Close();
		}

		private void OnBack_Click(){
			if(printPreviewControl2.StartPage==0) return;
			printPreviewControl2.StartPage--;
			SetPageNumText();
			ToolBarMain.Invalidate();
		}

		private void OnFwd_Click(){
			//if(printPreviewControl2.StartPage==totalPages-1) return;
			printPreviewControl2.StartPage++;
			SetPageNumText();
			ToolBarMain.Invalidate();
		}

		///<summary>Sets the toolbar's pagenum text based on the total pages. If 0 total pages, only shows the current pagenum.</summary>
		private void SetPageNumText() {
			if(TotalPages==0) {
				ToolBarMain.Buttons["PageNum"].Text=(printPreviewControl2.StartPage+1).ToString();
			}
			else {
				ToolBarMain.Buttons["PageNum"].Text=(printPreviewControl2.StartPage+1).ToString()
								+" / "+TotalPages.ToString();
			}
		}
	
	

		

		

		

		


	}
}
