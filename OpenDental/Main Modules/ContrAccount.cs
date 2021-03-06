/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {

	///<summary></summary>
	public class ContrAccount:System.Windows.Forms.UserControl {
		#region Designer 
		private System.Windows.Forms.Label labelFamFinancial;
		private System.ComponentModel.IContainer components=null;// Required designer variable.
		private System.Windows.Forms.Label labelUrgFinNote;
		private OpenDental.ODtextBox textUrgFinNote;
		private System.Windows.Forms.ContextMenu contextMenuIns;
		private System.Windows.Forms.MenuItem menuInsOther;
		private System.Windows.Forms.MenuItem menuInsPri;
		private System.Windows.Forms.MenuItem menuInsSec;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.ImageList imageListMain;
		private OpenDental.ODtextBox textFinNote;
		private System.Windows.Forms.ContextMenu contextMenuStatement;
		private System.Windows.Forms.MenuItem menuItemStatementWalkout;
		private System.Windows.Forms.MenuItem menuItemStatementMore;
		private OpenDental.UI.ODGrid gridComm;
		private OpenDental.UI.ODGrid gridAcctPat;
		private OpenDental.UI.ODGrid gridAccount;
		private OpenDental.UI.ODGrid gridRepeat;
		private System.Windows.Forms.MenuItem menuInsMedical;
		private ContextMenu contextMenuRepeat;
		private MenuItem menuItemRepeatStand;
		private MenuItem menuItemRepeatEmail;
		private Panel panelProgNotes;
		private ODGrid gridProg;
		private GroupBox groupBox6;
		private CheckBox checkShowE;
		private CheckBox checkShowR;
		private CheckBox checkShowC;
		private CheckBox checkShowTP;
		private GroupBox groupBox7;
		private CheckBox checkAppt;
		private CheckBox checkLabCase;
		private CheckBox checkRx;
		private CheckBox checkComm;
		private CheckBox checkNotes;
		private OpenDental.UI.Button butShowAll;
		private OpenDental.UI.Button butShowNone;
		private CheckBox checkExtraNotes;
		private CheckBox checkShowTeeth;
		private CheckBox checkAudit;
		private Panel panelAging;
		private Label labelBalance;
		private Label labelInsEst;
		private Label labelTotal;
		private Label label7;
		private TextBox text0_30;
		private Label label6;
		private TextBox text31_60;
		private Label label5;
		private TextBox text61_90;
		private Label label3;
		private TextBox textOver90;
		private Label label2;
		private MenuItem menuItemStatementEmail;
		private Label labelBalanceAmt;
		private TabControl tabControlShow;
		private TabPage tabMain;
		private TabPage tabShow;
		private ODGrid gridPayPlan;
		private ValidDate textDateEnd;
		private ValidDate textDateStart;
		private Label labelEndDate;
		private Label labelStartDate;
		private OpenDental.UI.Button butRefresh;
		private OpenDental.UI.Button but90days;
		private OpenDental.UI.Button but45days;
		private OpenDental.UI.Button butDatesAll;
		private CheckBox checkShowDetail;
		private OpenDental.UI.Button butToday;
		private CheckBox checkShowFamilyComm;
		private Label labelTotalAmt;
		private Label labelInsEstAmt;
		private Panel panelAgeLine;
		private Panel panel2;
		private Panel panel1;
		private ToolTip toolTip1;
		private Label labelPatEstBal;
		private Label labelPatEstBalAmt;
		private Panel panelTotalOwes;
		private Label label21;
		private Label labelTotalPtOwes;
		private Label labelUnearnedAmt;
		private Label labelUnearned;
		private Label labelInsRem;
		private ODToolBarButton _butPayment;
		private ODToolBarButton _butQuickProcs;
		private SplitContainerNoFlicker splitContainerParent;
		private SplitContainerNoFlicker splitContainerRepChargesPP;
		private SplitContainerNoFlicker splitContainerAccountCommLog;
		private UI.Button butServiceDateView;
		#endregion
		#region UserVariables
		///<summary>Gets updated to PatCur.PatNum that the last security log was made with so that we don't make too many security logs for this patient.  When _patNumLast no longer matches PatCur.PatNum (e.g. switched to a different patient within a module), a security log will be entered.  Gets reset (cleared and the set back to PatCur.PatNum) any time a module button is clicked which will cause another security log to be entered.</summary>
		private long _patNumLast;
		///<summary>Partially implemented lock object for an attempted bug fix.</summary>
		private object _lockDataSetMain=new object();
		///<summary>This holds some of the data needed for display.  It is retrieved in one call to the database.</summary>
		private DataSet DataSetMain;
		///<summary>This holds nearly all of the data needed for display.  It is retrieved in one call to the database.</summary>
		private AccountModules.LoadData _loadData;
		private Family FamCur;
		///<summary></summary>
		private Patient PatCur;
		private PatientNote PatientNoteCur;
		private RepeatCharge[] RepeatChargeList;
		///<summary>List of all orthocases for the selected patient.</summary>
		private List<OrthoCase>_listOrthoCases=new List<OrthoCase>();
		///<summary>Public so this can be checked from FormOpenDental and the note can be saved.  Necessary because in some cases the leave event doesn't
		///fire, like when a user switches to a non-modal form, like big phones, and switches patients from that form.</summary>
		public bool FinNoteChanged;
		///<summary>Public so this can be checked from FormOpenDental and the note can be saved.  Necessary because in some cases the leave event doesn't
		///fire, like when a user switches to a non-modal form, like big phones, and switches patients from that form.</summary>
		public bool UrgFinNoteChanged;
		private int Actscrollval;
		///<summary>Set to true if this control is placed in the recall edit window. This affects the control behavior.</summary>
		public bool ViewingInRecall=false;
		private List<DisplayField> fieldsForMainGrid;
		private GroupBox groupBoxIndIns;
		private TextBox textPriDed;
		private TextBox textPriUsed;
		private TextBox textPriDedRem;
		private TextBox textPriPend;
		private TextBox textPriRem;
		private TextBox textPriMax;
		private TextBox textSecRem;
		private Label label10;
		private TextBox textSecPend;
		private Label label11;
		private Label label18;
		private Label label12;
		private Label label13;
		private TextBox textSecDedRem;
		private Label label14;
		private Label label15;
		private TextBox textSecMax;
		private Label label16;
		private TextBox textSecDed;
		private TextBox textSecUsed;
		private GroupBox groupBoxFamilyIns;
		private TextBox textFamPriMax;
		private TextBox textFamPriDed;
		private Label label4;
		private Label label8;
		private TextBox textFamSecMax;
		private Label label9;
		private TextBox textFamSecDed;
		private Label label17;
		private UI.Button butCreditCard;
		private MenuItem menuItemReceipt;
		private MenuItem menuItemRepeatCanada;
		private MenuItem menuItemInvoice;
		private ODGrid gridPatInfo;
		private bool InitializedOnStartup;
		private ContextMenu contextMenuQuickProcs;
		private TextBox textQuickProcs;
		private ContextMenu contextMenuPayment;
		private MenuItem menuItemPay;
		private List<DisplayField> _patInfoDisplayFields;
		private CheckBox checkShowCompletePayPlans;
		private MenuItem menuItemLimited;
		private ContextMenu contextMenuPayPlan;
		private MenuItem menuItemPatPayPlan;
		private MenuItem menuItemInsPayPlan;
		private MenuItem menuItemSalesTax;
		private MenuItem menuItemAddMultAdj;
		private ContextMenu contextMenuAdjust;
		private TabControl tabControlAccount;
		private TabPage tabPagePatAccount;
		private TabPage tabPageAutoOrtho;
		private SplitContainer splitContainerAutoOrtho;
		private ODGrid gridAutoOrtho;
		private GroupBox groupBox1;
		private ValidNum textAutoOrthoMonthsTreat;
		private UI.Button butAutoOrthoDefaultMonthsTreat;
		private UI.Button butAutoOrthoEditMonthsTreat;
		private MenuItem menuItemRepeatSignupPortal;
		private GroupBox groupBox2;
		private ValidDate textDateAutoOrthoPlacement;
		private UI.Button butEditAutoOrthoPlacement;
		private UI.Button butAutoOrthoDefaultPlacement;
		private MenuItem menuItemIncomeTransfer;
		private ContextMenu contextMenuAcctGrid;
		private MenuItem menuItemAddAdj;
		private decimal PPBalanceTotal;
		private PatField[] _patFieldList;
		private MenuItem menuPrepayment;
		private TabPage tabPageHiddenSplits;
		private ODGrid gridTpSplits;
		private Def[] _acctProcQuickAddDefs;
		private MenuItem menuItemDynamicPayPlan;
		private TabPage tabPageOrthoCases;
		private SplitContainer splitContainerOrthoCases;
		private UI.Button butAddOrthoCase;
		private ODGrid gridOrthoCases;
		private CheckBox checkHideInactiveOrthoCases;
		private UI.Button butMakeOrthoCaseActive;
		private List<PaySplit> _listSplitsHidden=new List<PaySplit>();
		private MenuItem menuItemLimitedCustom;
		private FormRpServiceDateView _formRpServiceDateView=null;
		///<summary>True if 'Entire Family' is selected in the Select Patient grid.</summary>
		public bool _isSelectingFamily {
			get {
				if(DataSetMain==null) {
					return false;
				}
				return gridAcctPat.GetSelectedIndex()==gridAcctPat.ListGridRows.Count-1;
			}
		}

		private List<long> _listFamilyPatNums {
			get {
				if(_isSelectingFamily) {
					return FamCur.ListPats.Select(x => x.PatNum).ToList();
				}
				else {
					return new List<long>(){ PatCur.PatNum };
				}
			}
		}
		#endregion UserVariables

		///<summary></summary>
		public ContrAccount() {
			Logger.openlog.Log("Initializing account module...",Logger.Severity.INFO);
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
		}

		///<summary></summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components!= null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrAccount));
			this.labelFamFinancial = new System.Windows.Forms.Label();
			this.labelUrgFinNote = new System.Windows.Forms.Label();
			this.contextMenuIns = new System.Windows.Forms.ContextMenu();
			this.menuInsPri = new System.Windows.Forms.MenuItem();
			this.menuInsSec = new System.Windows.Forms.MenuItem();
			this.menuInsMedical = new System.Windows.Forms.MenuItem();
			this.menuInsOther = new System.Windows.Forms.MenuItem();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.contextMenuStatement = new System.Windows.Forms.ContextMenu();
			this.menuItemStatementWalkout = new System.Windows.Forms.MenuItem();
			this.menuItemStatementEmail = new System.Windows.Forms.MenuItem();
			this.menuItemReceipt = new System.Windows.Forms.MenuItem();
			this.menuItemInvoice = new System.Windows.Forms.MenuItem();
			this.menuItemLimited = new System.Windows.Forms.MenuItem();
			this.menuItemStatementMore = new System.Windows.Forms.MenuItem();
			this.contextMenuRepeat = new System.Windows.Forms.ContextMenu();
			this.menuItemRepeatStand = new System.Windows.Forms.MenuItem();
			this.menuItemRepeatEmail = new System.Windows.Forms.MenuItem();
			this.menuItemRepeatCanada = new System.Windows.Forms.MenuItem();
			this.menuItemRepeatSignupPortal = new System.Windows.Forms.MenuItem();
			this.panelAging = new System.Windows.Forms.Panel();
			this.labelInsRem = new System.Windows.Forms.Label();
			this.labelUnearnedAmt = new System.Windows.Forms.Label();
			this.labelUnearned = new System.Windows.Forms.Label();
			this.labelBalanceAmt = new System.Windows.Forms.Label();
			this.labelTotalAmt = new System.Windows.Forms.Label();
			this.panelTotalOwes = new System.Windows.Forms.Panel();
			this.label21 = new System.Windows.Forms.Label();
			this.labelTotalPtOwes = new System.Windows.Forms.Label();
			this.labelPatEstBalAmt = new System.Windows.Forms.Label();
			this.labelPatEstBal = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panelAgeLine = new System.Windows.Forms.Panel();
			this.labelInsEstAmt = new System.Windows.Forms.Label();
			this.labelBalance = new System.Windows.Forms.Label();
			this.labelInsEst = new System.Windows.Forms.Label();
			this.labelTotal = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.text0_30 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.text31_60 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.text61_90 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textOver90 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tabControlShow = new System.Windows.Forms.TabControl();
			this.tabMain = new System.Windows.Forms.TabPage();
			this.butServiceDateView = new OpenDental.UI.Button();
			this.butCreditCard = new OpenDental.UI.Button();
			this.textUrgFinNote = new OpenDental.ODtextBox();
			this.gridAcctPat = new OpenDental.UI.ODGrid();
			this.textFinNote = new OpenDental.ODtextBox();
			this.tabShow = new System.Windows.Forms.TabPage();
			this.checkShowCompletePayPlans = new System.Windows.Forms.CheckBox();
			this.checkShowFamilyComm = new System.Windows.Forms.CheckBox();
			this.butToday = new OpenDental.UI.Button();
			this.checkShowDetail = new System.Windows.Forms.CheckBox();
			this.butDatesAll = new OpenDental.UI.Button();
			this.but90days = new OpenDental.UI.Button();
			this.but45days = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.labelEndDate = new System.Windows.Forms.Label();
			this.labelStartDate = new System.Windows.Forms.Label();
			this.textDateEnd = new OpenDental.ValidDate();
			this.textDateStart = new OpenDental.ValidDate();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBoxIndIns = new System.Windows.Forms.GroupBox();
			this.textPriDed = new System.Windows.Forms.TextBox();
			this.textPriUsed = new System.Windows.Forms.TextBox();
			this.textPriDedRem = new System.Windows.Forms.TextBox();
			this.textPriPend = new System.Windows.Forms.TextBox();
			this.textPriRem = new System.Windows.Forms.TextBox();
			this.textPriMax = new System.Windows.Forms.TextBox();
			this.textSecRem = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textSecPend = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.textSecDedRem = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.textSecMax = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.textSecDed = new System.Windows.Forms.TextBox();
			this.textSecUsed = new System.Windows.Forms.TextBox();
			this.groupBoxFamilyIns = new System.Windows.Forms.GroupBox();
			this.textFamPriMax = new System.Windows.Forms.TextBox();
			this.textFamPriDed = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textFamSecMax = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.textFamSecDed = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.contextMenuQuickProcs = new System.Windows.Forms.ContextMenu();
			this.textQuickProcs = new System.Windows.Forms.TextBox();
			this.contextMenuPayment = new System.Windows.Forms.ContextMenu();
			this.menuItemPay = new System.Windows.Forms.MenuItem();
			this.menuItemIncomeTransfer = new System.Windows.Forms.MenuItem();
			this.menuPrepayment = new System.Windows.Forms.MenuItem();
			this.contextMenuPayPlan = new System.Windows.Forms.ContextMenu();
			this.menuItemDynamicPayPlan = new System.Windows.Forms.MenuItem();
			this.menuItemPatPayPlan = new System.Windows.Forms.MenuItem();
			this.menuItemInsPayPlan = new System.Windows.Forms.MenuItem();
			this.contextMenuAdjust = new System.Windows.Forms.ContextMenu();
			this.menuItemSalesTax = new System.Windows.Forms.MenuItem();
			this.menuItemAddMultAdj = new System.Windows.Forms.MenuItem();
			this.contextMenuAcctGrid = new System.Windows.Forms.ContextMenu();
			this.menuItemAddAdj = new System.Windows.Forms.MenuItem();
			this.gridPatInfo = new OpenDental.UI.ODGrid();
			this.splitContainerParent = new OpenDental.SplitContainerNoFlicker();
			this.splitContainerRepChargesPP = new OpenDental.SplitContainerNoFlicker();
			this.gridRepeat = new OpenDental.UI.ODGrid();
			this.gridPayPlan = new OpenDental.UI.ODGrid();
			this.splitContainerAccountCommLog = new OpenDental.SplitContainerNoFlicker();
			this.tabControlAccount = new System.Windows.Forms.TabControl();
			this.tabPagePatAccount = new System.Windows.Forms.TabPage();
			this.gridAccount = new OpenDental.UI.ODGrid();
			this.tabPageAutoOrtho = new System.Windows.Forms.TabPage();
			this.splitContainerAutoOrtho = new System.Windows.Forms.SplitContainer();
			this.gridAutoOrtho = new OpenDental.UI.ODGrid();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butAutoOrthoDefaultPlacement = new OpenDental.UI.Button();
			this.textDateAutoOrthoPlacement = new OpenDental.ValidDate();
			this.butEditAutoOrthoPlacement = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textAutoOrthoMonthsTreat = new OpenDental.ValidNum();
			this.butAutoOrthoDefaultMonthsTreat = new OpenDental.UI.Button();
			this.butAutoOrthoEditMonthsTreat = new OpenDental.UI.Button();
			this.tabPageOrthoCases = new System.Windows.Forms.TabPage();
			this.splitContainerOrthoCases = new System.Windows.Forms.SplitContainer();
			this.gridOrthoCases = new OpenDental.UI.ODGrid();
			this.butMakeOrthoCaseActive = new OpenDental.UI.Button();
			this.checkHideInactiveOrthoCases = new System.Windows.Forms.CheckBox();
			this.butAddOrthoCase = new OpenDental.UI.Button();
			this.tabPageHiddenSplits = new System.Windows.Forms.TabPage();
			this.gridTpSplits = new OpenDental.UI.ODGrid();
			this.panelProgNotes = new System.Windows.Forms.Panel();
			this.butShowNone = new OpenDental.UI.Button();
			this.butShowAll = new OpenDental.UI.Button();
			this.checkNotes = new System.Windows.Forms.CheckBox();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.checkShowTeeth = new System.Windows.Forms.CheckBox();
			this.checkAudit = new System.Windows.Forms.CheckBox();
			this.checkExtraNotes = new System.Windows.Forms.CheckBox();
			this.checkAppt = new System.Windows.Forms.CheckBox();
			this.checkLabCase = new System.Windows.Forms.CheckBox();
			this.checkRx = new System.Windows.Forms.CheckBox();
			this.checkComm = new System.Windows.Forms.CheckBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.checkShowE = new System.Windows.Forms.CheckBox();
			this.checkShowR = new System.Windows.Forms.CheckBox();
			this.checkShowC = new System.Windows.Forms.CheckBox();
			this.checkShowTP = new System.Windows.Forms.CheckBox();
			this.gridProg = new OpenDental.UI.ODGrid();
			this.gridComm = new OpenDental.UI.ODGrid();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.menuItemLimitedCustom = new System.Windows.Forms.MenuItem();
			this.panelAging.SuspendLayout();
			this.panelTotalOwes.SuspendLayout();
			this.tabControlShow.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tabShow.SuspendLayout();
			this.groupBoxIndIns.SuspendLayout();
			this.groupBoxFamilyIns.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerParent)).BeginInit();
			this.splitContainerParent.Panel1.SuspendLayout();
			this.splitContainerParent.Panel2.SuspendLayout();
			this.splitContainerParent.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerRepChargesPP)).BeginInit();
			this.splitContainerRepChargesPP.Panel1.SuspendLayout();
			this.splitContainerRepChargesPP.Panel2.SuspendLayout();
			this.splitContainerRepChargesPP.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerAccountCommLog)).BeginInit();
			this.splitContainerAccountCommLog.Panel1.SuspendLayout();
			this.splitContainerAccountCommLog.Panel2.SuspendLayout();
			this.splitContainerAccountCommLog.SuspendLayout();
			this.tabControlAccount.SuspendLayout();
			this.tabPagePatAccount.SuspendLayout();
			this.tabPageAutoOrtho.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerAutoOrtho)).BeginInit();
			this.splitContainerAutoOrtho.Panel1.SuspendLayout();
			this.splitContainerAutoOrtho.Panel2.SuspendLayout();
			this.splitContainerAutoOrtho.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPageOrthoCases.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerOrthoCases)).BeginInit();
			this.splitContainerOrthoCases.Panel1.SuspendLayout();
			this.splitContainerOrthoCases.Panel2.SuspendLayout();
			this.splitContainerOrthoCases.SuspendLayout();
			this.tabPageHiddenSplits.SuspendLayout();
			this.panelProgNotes.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelFamFinancial
			// 
			this.labelFamFinancial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelFamFinancial.Location = new System.Drawing.Point(0, 346);
			this.labelFamFinancial.Name = "labelFamFinancial";
			this.labelFamFinancial.Size = new System.Drawing.Size(154, 16);
			this.labelFamFinancial.TabIndex = 9;
			this.labelFamFinancial.Text = "Family Financial Notes";
			this.labelFamFinancial.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelUrgFinNote
			// 
			this.labelUrgFinNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelUrgFinNote.Location = new System.Drawing.Point(0, 0);
			this.labelUrgFinNote.Name = "labelUrgFinNote";
			this.labelUrgFinNote.Size = new System.Drawing.Size(165, 17);
			this.labelUrgFinNote.TabIndex = 10;
			this.labelUrgFinNote.Text = "Fam Urgent Fin Note";
			this.labelUrgFinNote.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// contextMenuIns
			// 
			this.contextMenuIns.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuInsPri,
            this.menuInsSec,
            this.menuInsMedical,
            this.menuInsOther});
			// 
			// menuInsPri
			// 
			this.menuInsPri.Index = 0;
			this.menuInsPri.Text = "Primary";
			this.menuInsPri.Click += new System.EventHandler(this.menuInsPri_Click);
			// 
			// menuInsSec
			// 
			this.menuInsSec.Index = 1;
			this.menuInsSec.Text = "Secondary";
			this.menuInsSec.Click += new System.EventHandler(this.menuInsSec_Click);
			// 
			// menuInsMedical
			// 
			this.menuInsMedical.Index = 2;
			this.menuInsMedical.Text = "Medical";
			this.menuInsMedical.Click += new System.EventHandler(this.menuInsMedical_Click);
			// 
			// menuInsOther
			// 
			this.menuInsOther.Index = 3;
			this.menuInsOther.Text = "Other";
			this.menuInsOther.Click += new System.EventHandler(this.menuInsOther_Click);
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "Pat.gif");
			this.imageListMain.Images.SetKeyName(1, "");
			this.imageListMain.Images.SetKeyName(2, "");
			this.imageListMain.Images.SetKeyName(3, "Umbrella.gif");
			this.imageListMain.Images.SetKeyName(4, "");
			// 
			// contextMenuStatement
			// 
			this.contextMenuStatement.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemStatementWalkout,
            this.menuItemStatementEmail,
            this.menuItemReceipt,
            this.menuItemInvoice,
            this.menuItemLimited,
            this.menuItemLimitedCustom,
            this.menuItemStatementMore});
			// 
			// menuItemStatementWalkout
			// 
			this.menuItemStatementWalkout.Index = 0;
			this.menuItemStatementWalkout.Text = "Walkout";
			this.menuItemStatementWalkout.Click += new System.EventHandler(this.menuItemStatementWalkout_Click);
			// 
			// menuItemStatementEmail
			// 
			this.menuItemStatementEmail.Index = 1;
			this.menuItemStatementEmail.Text = "Email";
			this.menuItemStatementEmail.Click += new System.EventHandler(this.menuItemStatementEmail_Click);
			// 
			// menuItemReceipt
			// 
			this.menuItemReceipt.Index = 2;
			this.menuItemReceipt.Text = "Receipt";
			this.menuItemReceipt.Click += new System.EventHandler(this.menuItemReceipt_Click);
			// 
			// menuItemInvoice
			// 
			this.menuItemInvoice.Index = 3;
			this.menuItemInvoice.Text = "Invoice";
			this.menuItemInvoice.Click += new System.EventHandler(this.menuItemInvoice_Click);
			// 
			// menuItemLimited
			// 
			this.menuItemLimited.Index = 4;
			this.menuItemLimited.Text = "Limited";
			this.menuItemLimited.Click += new System.EventHandler(this.menuItemLimited_Click);
			// 
			// menuItemLimitedCustom
			// 
			this.menuItemLimitedCustom.Index = 5;
			this.menuItemLimitedCustom.Text = "Limited (Custom)";
			this.menuItemLimitedCustom.Click += new System.EventHandler(this.menuItemLimitedCustom_Click);
			// 
			// menuItemStatementMore
			// 
			this.menuItemStatementMore.Index = 6;
			this.menuItemStatementMore.Text = "More Options";
			this.menuItemStatementMore.Click += new System.EventHandler(this.menuItemStatementMore_Click);
			// 
			// contextMenuRepeat
			// 
			this.contextMenuRepeat.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemRepeatStand,
            this.menuItemRepeatEmail,
            this.menuItemRepeatCanada,
            this.menuItemRepeatSignupPortal});
			// 
			// menuItemRepeatStand
			// 
			this.menuItemRepeatStand.Index = 0;
			this.menuItemRepeatStand.Text = "Standard Monthly";
			this.menuItemRepeatStand.Click += new System.EventHandler(this.MenuItemRepeatStand_Click);
			// 
			// menuItemRepeatEmail
			// 
			this.menuItemRepeatEmail.Index = 1;
			this.menuItemRepeatEmail.Text = "Email Monthly";
			this.menuItemRepeatEmail.Click += new System.EventHandler(this.MenuItemRepeatEmail_Click);
			// 
			// menuItemRepeatCanada
			// 
			this.menuItemRepeatCanada.Index = 2;
			this.menuItemRepeatCanada.Text = "Canada Monthly";
			this.menuItemRepeatCanada.Click += new System.EventHandler(this.menuItemRepeatCanada_Click);
			// 
			// menuItemRepeatSignupPortal
			// 
			this.menuItemRepeatSignupPortal.Index = 3;
			this.menuItemRepeatSignupPortal.Text = "Signup Portal";
			this.menuItemRepeatSignupPortal.Click += new System.EventHandler(this.menuItemRepeatSignupPortal_Click);
			// 
			// panelAging
			// 
			this.panelAging.Controls.Add(this.labelInsRem);
			this.panelAging.Controls.Add(this.labelUnearnedAmt);
			this.panelAging.Controls.Add(this.labelUnearned);
			this.panelAging.Controls.Add(this.labelBalanceAmt);
			this.panelAging.Controls.Add(this.labelTotalAmt);
			this.panelAging.Controls.Add(this.panelTotalOwes);
			this.panelAging.Controls.Add(this.labelPatEstBalAmt);
			this.panelAging.Controls.Add(this.labelPatEstBal);
			this.panelAging.Controls.Add(this.panel2);
			this.panelAging.Controls.Add(this.panel1);
			this.panelAging.Controls.Add(this.panelAgeLine);
			this.panelAging.Controls.Add(this.labelInsEstAmt);
			this.panelAging.Controls.Add(this.labelBalance);
			this.panelAging.Controls.Add(this.labelInsEst);
			this.panelAging.Controls.Add(this.labelTotal);
			this.panelAging.Controls.Add(this.label7);
			this.panelAging.Controls.Add(this.text0_30);
			this.panelAging.Controls.Add(this.label6);
			this.panelAging.Controls.Add(this.text31_60);
			this.panelAging.Controls.Add(this.label5);
			this.panelAging.Controls.Add(this.text61_90);
			this.panelAging.Controls.Add(this.label3);
			this.panelAging.Controls.Add(this.textOver90);
			this.panelAging.Controls.Add(this.label2);
			this.panelAging.Location = new System.Drawing.Point(0, 25);
			this.panelAging.Name = "panelAging";
			this.panelAging.Size = new System.Drawing.Size(749, 37);
			this.panelAging.TabIndex = 213;
			// 
			// labelInsRem
			// 
			this.labelInsRem.BackColor = System.Drawing.Color.LightGray;
			this.labelInsRem.Location = new System.Drawing.Point(700, 4);
			this.labelInsRem.Name = "labelInsRem";
			this.labelInsRem.Size = new System.Drawing.Size(45, 29);
			this.labelInsRem.TabIndex = 0;
			this.labelInsRem.Text = "Ins\r\nRem";
			this.labelInsRem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelInsRem.Click += new System.EventHandler(this.labelInsRem_Click);
			this.labelInsRem.MouseEnter += new System.EventHandler(this.labelInsRem_MouseEnter);
			this.labelInsRem.MouseLeave += new System.EventHandler(this.labelInsRem_MouseLeave);
			// 
			// labelUnearnedAmt
			// 
			this.labelUnearnedAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelUnearnedAmt.ForeColor = System.Drawing.Color.Firebrick;
			this.labelUnearnedAmt.Location = new System.Drawing.Point(636, 18);
			this.labelUnearnedAmt.Name = "labelUnearnedAmt";
			this.labelUnearnedAmt.Size = new System.Drawing.Size(60, 13);
			this.labelUnearnedAmt.TabIndex = 228;
			this.labelUnearnedAmt.Text = "25000.00";
			this.labelUnearnedAmt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelUnearned
			// 
			this.labelUnearned.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelUnearned.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labelUnearned.Location = new System.Drawing.Point(632, 2);
			this.labelUnearned.Name = "labelUnearned";
			this.labelUnearned.Size = new System.Drawing.Size(68, 13);
			this.labelUnearned.TabIndex = 227;
			this.labelUnearned.Text = "Unearned";
			this.labelUnearned.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelBalanceAmt
			// 
			this.labelBalanceAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBalanceAmt.ForeColor = System.Drawing.Color.Firebrick;
			this.labelBalanceAmt.Location = new System.Drawing.Point(456, 16);
			this.labelBalanceAmt.Name = "labelBalanceAmt";
			this.labelBalanceAmt.Size = new System.Drawing.Size(80, 19);
			this.labelBalanceAmt.TabIndex = 60;
			this.labelBalanceAmt.Text = "25000.00";
			this.labelBalanceAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelTotalAmt
			// 
			this.labelTotalAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTotalAmt.ForeColor = System.Drawing.Color.Firebrick;
			this.labelTotalAmt.Location = new System.Drawing.Point(294, 16);
			this.labelTotalAmt.Name = "labelTotalAmt";
			this.labelTotalAmt.Size = new System.Drawing.Size(80, 19);
			this.labelTotalAmt.TabIndex = 61;
			this.labelTotalAmt.Text = "25000.00";
			this.labelTotalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panelTotalOwes
			// 
			this.panelTotalOwes.Controls.Add(this.label21);
			this.panelTotalOwes.Controls.Add(this.labelTotalPtOwes);
			this.panelTotalOwes.Location = new System.Drawing.Point(560, -38);
			this.panelTotalOwes.Name = "panelTotalOwes";
			this.panelTotalOwes.Size = new System.Drawing.Size(126, 37);
			this.panelTotalOwes.TabIndex = 226;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(3, 0);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(123, 12);
			this.label21.TabIndex = 223;
			this.label21.Text = "TOTAL  Owed w/ Plan:";
			this.label21.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.toolTip1.SetToolTip(this.label21, "Total balance owed on all payment plans ");
			// 
			// labelTotalPtOwes
			// 
			this.labelTotalPtOwes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTotalPtOwes.ForeColor = System.Drawing.Color.Firebrick;
			this.labelTotalPtOwes.Location = new System.Drawing.Point(6, 12);
			this.labelTotalPtOwes.Name = "labelTotalPtOwes";
			this.labelTotalPtOwes.Size = new System.Drawing.Size(112, 23);
			this.labelTotalPtOwes.TabIndex = 222;
			this.labelTotalPtOwes.Text = "2500.00";
			this.labelTotalPtOwes.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelPatEstBalAmt
			// 
			this.labelPatEstBalAmt.Location = new System.Drawing.Point(550, 19);
			this.labelPatEstBalAmt.Name = "labelPatEstBalAmt";
			this.labelPatEstBalAmt.Size = new System.Drawing.Size(65, 13);
			this.labelPatEstBalAmt.TabIndex = 89;
			this.labelPatEstBalAmt.Text = "25000.00";
			this.labelPatEstBalAmt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelPatEstBal
			// 
			this.labelPatEstBal.Location = new System.Drawing.Point(550, 3);
			this.labelPatEstBal.Name = "labelPatEstBal";
			this.labelPatEstBal.Size = new System.Drawing.Size(65, 13);
			this.labelPatEstBal.TabIndex = 88;
			this.labelPatEstBal.Text = "Pat Est Bal";
			this.labelPatEstBal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel2.Location = new System.Drawing.Point(624, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(2, 32);
			this.panel2.TabIndex = 87;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.Location = new System.Drawing.Point(541, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(2, 32);
			this.panel1.TabIndex = 86;
			// 
			// panelAgeLine
			// 
			this.panelAgeLine.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panelAgeLine.Location = new System.Drawing.Point(379, 2);
			this.panelAgeLine.Name = "panelAgeLine";
			this.panelAgeLine.Size = new System.Drawing.Size(2, 32);
			this.panelAgeLine.TabIndex = 63;
			// 
			// labelInsEstAmt
			// 
			this.labelInsEstAmt.Location = new System.Drawing.Point(387, 18);
			this.labelInsEstAmt.Name = "labelInsEstAmt";
			this.labelInsEstAmt.Size = new System.Drawing.Size(65, 13);
			this.labelInsEstAmt.TabIndex = 62;
			this.labelInsEstAmt.Text = "2500.00";
			this.labelInsEstAmt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelBalance
			// 
			this.labelBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBalance.Location = new System.Drawing.Point(454, 2);
			this.labelBalance.Name = "labelBalance";
			this.labelBalance.Size = new System.Drawing.Size(80, 13);
			this.labelBalance.TabIndex = 59;
			this.labelBalance.Text = "= Balance";
			this.labelBalance.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelInsEst
			// 
			this.labelInsEst.Location = new System.Drawing.Point(387, 2);
			this.labelInsEst.Name = "labelInsEst";
			this.labelInsEst.Size = new System.Drawing.Size(65, 13);
			this.labelInsEst.TabIndex = 57;
			this.labelInsEst.Text = "- InsEst";
			this.labelInsEst.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelTotal
			// 
			this.labelTotal.Location = new System.Drawing.Point(294, 2);
			this.labelTotal.Name = "labelTotal";
			this.labelTotal.Size = new System.Drawing.Size(80, 13);
			this.labelTotal.TabIndex = 55;
			this.labelTotal.Text = "Total";
			this.labelTotal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(69, 2);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(55, 13);
			this.label7.TabIndex = 53;
			this.label7.Text = "0-30";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// text0_30
			// 
			this.text0_30.Location = new System.Drawing.Point(67, 15);
			this.text0_30.Name = "text0_30";
			this.text0_30.ReadOnly = true;
			this.text0_30.Size = new System.Drawing.Size(55, 20);
			this.text0_30.TabIndex = 52;
			this.text0_30.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(122, 2);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(55, 13);
			this.label6.TabIndex = 51;
			this.label6.Text = "31-60";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// text31_60
			// 
			this.text31_60.Location = new System.Drawing.Point(122, 15);
			this.text31_60.Name = "text31_60";
			this.text31_60.ReadOnly = true;
			this.text31_60.Size = new System.Drawing.Size(55, 20);
			this.text31_60.TabIndex = 50;
			this.text31_60.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(177, 2);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 13);
			this.label5.TabIndex = 49;
			this.label5.Text = "61-90";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// text61_90
			// 
			this.text61_90.Location = new System.Drawing.Point(177, 15);
			this.text61_90.Name = "text61_90";
			this.text61_90.ReadOnly = true;
			this.text61_90.Size = new System.Drawing.Size(55, 20);
			this.text61_90.TabIndex = 48;
			this.text61_90.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(232, 2);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 47;
			this.label3.Text = "over 90";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// textOver90
			// 
			this.textOver90.Location = new System.Drawing.Point(232, 15);
			this.textOver90.Name = "textOver90";
			this.textOver90.ReadOnly = true;
			this.textOver90.Size = new System.Drawing.Size(55, 20);
			this.textOver90.TabIndex = 46;
			this.textOver90.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(14, 2);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(50, 33);
			this.label2.TabIndex = 45;
			this.label2.Text = "Family\r\nAging";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabControlShow
			// 
			this.tabControlShow.Controls.Add(this.tabMain);
			this.tabControlShow.Controls.Add(this.tabShow);
			this.tabControlShow.Location = new System.Drawing.Point(750, 27);
			this.tabControlShow.Name = "tabControlShow";
			this.tabControlShow.SelectedIndex = 0;
			this.tabControlShow.Size = new System.Drawing.Size(186, 530);
			this.tabControlShow.TabIndex = 216;
			// 
			// tabMain
			// 
			this.tabMain.BackColor = System.Drawing.Color.Transparent;
			this.tabMain.Controls.Add(this.butServiceDateView);
			this.tabMain.Controls.Add(this.butCreditCard);
			this.tabMain.Controls.Add(this.labelUrgFinNote);
			this.tabMain.Controls.Add(this.labelFamFinancial);
			this.tabMain.Controls.Add(this.textUrgFinNote);
			this.tabMain.Controls.Add(this.gridAcctPat);
			this.tabMain.Controls.Add(this.textFinNote);
			this.tabMain.Location = new System.Drawing.Point(4, 22);
			this.tabMain.Name = "tabMain";
			this.tabMain.Padding = new System.Windows.Forms.Padding(3);
			this.tabMain.Size = new System.Drawing.Size(178, 504);
			this.tabMain.TabIndex = 0;
			this.tabMain.Text = "Main";
			this.tabMain.UseVisualStyleBackColor = true;
			// 
			// butServiceDateView
			// 
			this.butServiceDateView.Location = new System.Drawing.Point(35, 101);
			this.butServiceDateView.Name = "butServiceDateView";
			this.butServiceDateView.Size = new System.Drawing.Size(111, 24);
			this.butServiceDateView.TabIndex = 217;
			this.butServiceDateView.Text = "Service Date View";
			this.butServiceDateView.UseVisualStyleBackColor = true;
			this.butServiceDateView.Click += new System.EventHandler(this.butServiceDateView_Click);
			// 
			// butCreditCard
			// 
			this.butCreditCard.Location = new System.Drawing.Point(35, 132);
			this.butCreditCard.Name = "butCreditCard";
			this.butCreditCard.Size = new System.Drawing.Size(111, 24);
			this.butCreditCard.TabIndex = 216;
			this.butCreditCard.Text = "Credit Card Manage";
			this.butCreditCard.UseVisualStyleBackColor = true;
			this.butCreditCard.Click += new System.EventHandler(this.butCreditCard_Click);
			// 
			// textUrgFinNote
			// 
			this.textUrgFinNote.AcceptsTab = true;
			this.textUrgFinNote.BackColor = System.Drawing.Color.White;
			this.textUrgFinNote.DetectLinksEnabled = false;
			this.textUrgFinNote.DetectUrls = false;
			this.textUrgFinNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textUrgFinNote.ForeColor = System.Drawing.Color.Red;
			this.textUrgFinNote.Location = new System.Drawing.Point(0, 20);
			this.textUrgFinNote.Name = "textUrgFinNote";
			this.textUrgFinNote.QuickPasteType = OpenDentBusiness.QuickPasteType.FinancialNotes;
			this.textUrgFinNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textUrgFinNote.Size = new System.Drawing.Size(178, 77);
			this.textUrgFinNote.TabIndex = 11;
			this.textUrgFinNote.Text = "";
			this.textUrgFinNote.TextChanged += new System.EventHandler(this.textUrgFinNote_TextChanged);
			this.textUrgFinNote.Leave += new System.EventHandler(this.textUrgFinNote_Leave);
			// 
			// gridAcctPat
			// 
			this.gridAcctPat.ColorSelectedRow = System.Drawing.Color.DarkSalmon;
			this.gridAcctPat.Location = new System.Drawing.Point(0, 163);
			this.gridAcctPat.Name = "gridAcctPat";
			this.gridAcctPat.Size = new System.Drawing.Size(178, 180);
			this.gridAcctPat.TabIndex = 72;
			this.gridAcctPat.Title = "Select Patient";
			this.gridAcctPat.TranslationName = "TableAccountPat";
			this.gridAcctPat.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAcctPat_CellClick);
			// 
			// textFinNote
			// 
			this.textFinNote.AcceptsTab = true;
			this.textFinNote.BackColor = System.Drawing.SystemColors.Window;
			this.textFinNote.DetectLinksEnabled = false;
			this.textFinNote.DetectUrls = false;
			this.textFinNote.Location = new System.Drawing.Point(0, 365);
			this.textFinNote.Name = "textFinNote";
			this.textFinNote.QuickPasteType = OpenDentBusiness.QuickPasteType.FinancialNotes;
			this.textFinNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textFinNote.Size = new System.Drawing.Size(178, 134);
			this.textFinNote.TabIndex = 70;
			this.textFinNote.Text = "";
			this.textFinNote.TextChanged += new System.EventHandler(this.textFinNote_TextChanged);
			this.textFinNote.Leave += new System.EventHandler(this.textFinNote_Leave);
			// 
			// tabShow
			// 
			this.tabShow.BackColor = System.Drawing.Color.Transparent;
			this.tabShow.Controls.Add(this.checkShowCompletePayPlans);
			this.tabShow.Controls.Add(this.checkShowFamilyComm);
			this.tabShow.Controls.Add(this.butToday);
			this.tabShow.Controls.Add(this.checkShowDetail);
			this.tabShow.Controls.Add(this.butDatesAll);
			this.tabShow.Controls.Add(this.but90days);
			this.tabShow.Controls.Add(this.but45days);
			this.tabShow.Controls.Add(this.butRefresh);
			this.tabShow.Controls.Add(this.labelEndDate);
			this.tabShow.Controls.Add(this.labelStartDate);
			this.tabShow.Controls.Add(this.textDateEnd);
			this.tabShow.Controls.Add(this.textDateStart);
			this.tabShow.Location = new System.Drawing.Point(4, 22);
			this.tabShow.Name = "tabShow";
			this.tabShow.Padding = new System.Windows.Forms.Padding(3);
			this.tabShow.Size = new System.Drawing.Size(178, 504);
			this.tabShow.TabIndex = 1;
			this.tabShow.Text = "Show";
			this.tabShow.UseVisualStyleBackColor = true;
			// 
			// checkShowCompletePayPlans
			// 
			this.checkShowCompletePayPlans.Location = new System.Drawing.Point(8, 242);
			this.checkShowCompletePayPlans.Name = "checkShowCompletePayPlans";
			this.checkShowCompletePayPlans.Size = new System.Drawing.Size(164, 18);
			this.checkShowCompletePayPlans.TabIndex = 222;
			this.checkShowCompletePayPlans.Text = "Show Completed Pay Plans";
			this.checkShowCompletePayPlans.UseVisualStyleBackColor = true;
			this.checkShowCompletePayPlans.Click += new System.EventHandler(this.checkShowCompletePayPlans_Click);
			// 
			// checkShowFamilyComm
			// 
			this.checkShowFamilyComm.Location = new System.Drawing.Point(8, 219);
			this.checkShowFamilyComm.Name = "checkShowFamilyComm";
			this.checkShowFamilyComm.Size = new System.Drawing.Size(164, 18);
			this.checkShowFamilyComm.TabIndex = 221;
			this.checkShowFamilyComm.Text = "Show Family Comm Entries";
			this.checkShowFamilyComm.UseVisualStyleBackColor = true;
			this.checkShowFamilyComm.Click += new System.EventHandler(this.checkShowFamilyComm_Click);
			// 
			// butToday
			// 
			this.butToday.Location = new System.Drawing.Point(95, 54);
			this.butToday.Name = "butToday";
			this.butToday.Size = new System.Drawing.Size(77, 24);
			this.butToday.TabIndex = 220;
			this.butToday.Text = "Today";
			this.butToday.Click += new System.EventHandler(this.butToday_Click);
			// 
			// checkShowDetail
			// 
			this.checkShowDetail.Checked = true;
			this.checkShowDetail.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowDetail.Location = new System.Drawing.Point(8, 196);
			this.checkShowDetail.Name = "checkShowDetail";
			this.checkShowDetail.Size = new System.Drawing.Size(164, 18);
			this.checkShowDetail.TabIndex = 219;
			this.checkShowDetail.Text = "Show Proc Breakdowns";
			this.checkShowDetail.UseVisualStyleBackColor = true;
			this.checkShowDetail.Click += new System.EventHandler(this.checkShowDetail_Click);
			// 
			// butDatesAll
			// 
			this.butDatesAll.Location = new System.Drawing.Point(95, 132);
			this.butDatesAll.Name = "butDatesAll";
			this.butDatesAll.Size = new System.Drawing.Size(77, 24);
			this.butDatesAll.TabIndex = 218;
			this.butDatesAll.Text = "All Dates";
			this.butDatesAll.Click += new System.EventHandler(this.butDatesAll_Click);
			// 
			// but90days
			// 
			this.but90days.Location = new System.Drawing.Point(95, 106);
			this.but90days.Name = "but90days";
			this.but90days.Size = new System.Drawing.Size(77, 24);
			this.but90days.TabIndex = 217;
			this.but90days.Text = "Last 90 Days";
			this.but90days.Click += new System.EventHandler(this.but90days_Click);
			// 
			// but45days
			// 
			this.but45days.Location = new System.Drawing.Point(95, 80);
			this.but45days.Name = "but45days";
			this.but45days.Size = new System.Drawing.Size(77, 24);
			this.but45days.TabIndex = 216;
			this.but45days.Text = "Last 45 Days";
			this.but45days.Click += new System.EventHandler(this.but45days_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.Location = new System.Drawing.Point(95, 158);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(77, 24);
			this.butRefresh.TabIndex = 214;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// labelEndDate
			// 
			this.labelEndDate.Location = new System.Drawing.Point(2, 34);
			this.labelEndDate.Name = "labelEndDate";
			this.labelEndDate.Size = new System.Drawing.Size(91, 14);
			this.labelEndDate.TabIndex = 211;
			this.labelEndDate.Text = "End Date";
			this.labelEndDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelStartDate
			// 
			this.labelStartDate.Location = new System.Drawing.Point(8, 11);
			this.labelStartDate.Name = "labelStartDate";
			this.labelStartDate.Size = new System.Drawing.Size(84, 14);
			this.labelStartDate.TabIndex = 210;
			this.labelStartDate.Text = "Start Date";
			this.labelStartDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateEnd
			// 
			this.textDateEnd.Location = new System.Drawing.Point(95, 31);
			this.textDateEnd.Name = "textDateEnd";
			this.textDateEnd.Size = new System.Drawing.Size(77, 20);
			this.textDateEnd.TabIndex = 213;
			// 
			// textDateStart
			// 
			this.textDateStart.BackColor = System.Drawing.SystemColors.Window;
			this.textDateStart.ForeColor = System.Drawing.SystemColors.WindowText;
			this.textDateStart.Location = new System.Drawing.Point(95, 8);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(77, 20);
			this.textDateStart.TabIndex = 212;
			// 
			// groupBoxIndIns
			// 
			this.groupBoxIndIns.Controls.Add(this.textPriDed);
			this.groupBoxIndIns.Controls.Add(this.textPriUsed);
			this.groupBoxIndIns.Controls.Add(this.textPriDedRem);
			this.groupBoxIndIns.Controls.Add(this.textPriPend);
			this.groupBoxIndIns.Controls.Add(this.textPriRem);
			this.groupBoxIndIns.Controls.Add(this.textPriMax);
			this.groupBoxIndIns.Controls.Add(this.textSecRem);
			this.groupBoxIndIns.Controls.Add(this.label10);
			this.groupBoxIndIns.Controls.Add(this.textSecPend);
			this.groupBoxIndIns.Controls.Add(this.label11);
			this.groupBoxIndIns.Controls.Add(this.label18);
			this.groupBoxIndIns.Controls.Add(this.label12);
			this.groupBoxIndIns.Controls.Add(this.label13);
			this.groupBoxIndIns.Controls.Add(this.textSecDedRem);
			this.groupBoxIndIns.Controls.Add(this.label14);
			this.groupBoxIndIns.Controls.Add(this.label15);
			this.groupBoxIndIns.Controls.Add(this.textSecMax);
			this.groupBoxIndIns.Controls.Add(this.label16);
			this.groupBoxIndIns.Controls.Add(this.textSecDed);
			this.groupBoxIndIns.Controls.Add(this.textSecUsed);
			this.groupBoxIndIns.Location = new System.Drawing.Point(556, 144);
			this.groupBoxIndIns.Name = "groupBoxIndIns";
			this.groupBoxIndIns.Size = new System.Drawing.Size(193, 160);
			this.groupBoxIndIns.TabIndex = 219;
			this.groupBoxIndIns.TabStop = false;
			this.groupBoxIndIns.Text = "Individual Insurance";
			this.groupBoxIndIns.Visible = false;
			// 
			// textPriDed
			// 
			this.textPriDed.BackColor = System.Drawing.Color.White;
			this.textPriDed.Location = new System.Drawing.Point(71, 55);
			this.textPriDed.Name = "textPriDed";
			this.textPriDed.ReadOnly = true;
			this.textPriDed.Size = new System.Drawing.Size(60, 20);
			this.textPriDed.TabIndex = 45;
			this.textPriDed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriUsed
			// 
			this.textPriUsed.BackColor = System.Drawing.Color.White;
			this.textPriUsed.Location = new System.Drawing.Point(71, 95);
			this.textPriUsed.Name = "textPriUsed";
			this.textPriUsed.ReadOnly = true;
			this.textPriUsed.Size = new System.Drawing.Size(60, 20);
			this.textPriUsed.TabIndex = 44;
			this.textPriUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriDedRem
			// 
			this.textPriDedRem.BackColor = System.Drawing.Color.White;
			this.textPriDedRem.Location = new System.Drawing.Point(71, 75);
			this.textPriDedRem.Name = "textPriDedRem";
			this.textPriDedRem.ReadOnly = true;
			this.textPriDedRem.Size = new System.Drawing.Size(60, 20);
			this.textPriDedRem.TabIndex = 51;
			this.textPriDedRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriPend
			// 
			this.textPriPend.BackColor = System.Drawing.Color.White;
			this.textPriPend.Location = new System.Drawing.Point(71, 115);
			this.textPriPend.Name = "textPriPend";
			this.textPriPend.ReadOnly = true;
			this.textPriPend.Size = new System.Drawing.Size(60, 20);
			this.textPriPend.TabIndex = 43;
			this.textPriPend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriRem
			// 
			this.textPriRem.BackColor = System.Drawing.Color.White;
			this.textPriRem.Location = new System.Drawing.Point(71, 135);
			this.textPriRem.Name = "textPriRem";
			this.textPriRem.ReadOnly = true;
			this.textPriRem.Size = new System.Drawing.Size(60, 20);
			this.textPriRem.TabIndex = 42;
			this.textPriRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriMax
			// 
			this.textPriMax.BackColor = System.Drawing.Color.White;
			this.textPriMax.Location = new System.Drawing.Point(71, 35);
			this.textPriMax.Name = "textPriMax";
			this.textPriMax.ReadOnly = true;
			this.textPriMax.Size = new System.Drawing.Size(60, 20);
			this.textPriMax.TabIndex = 38;
			this.textPriMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSecRem
			// 
			this.textSecRem.BackColor = System.Drawing.Color.White;
			this.textSecRem.Location = new System.Drawing.Point(130, 135);
			this.textSecRem.Name = "textSecRem";
			this.textSecRem.ReadOnly = true;
			this.textSecRem.Size = new System.Drawing.Size(60, 20);
			this.textSecRem.TabIndex = 46;
			this.textSecRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(73, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(60, 15);
			this.label10.TabIndex = 31;
			this.label10.Text = "Primary";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// textSecPend
			// 
			this.textSecPend.BackColor = System.Drawing.Color.White;
			this.textSecPend.Location = new System.Drawing.Point(130, 115);
			this.textSecPend.Name = "textSecPend";
			this.textSecPend.ReadOnly = true;
			this.textSecPend.Size = new System.Drawing.Size(60, 20);
			this.textSecPend.TabIndex = 47;
			this.textSecPend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(4, 37);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(66, 15);
			this.label11.TabIndex = 32;
			this.label11.Text = "Annual Max";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(2, 77);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(69, 15);
			this.label18.TabIndex = 50;
			this.label18.Text = "Ded Remain";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(4, 57);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(66, 15);
			this.label12.TabIndex = 33;
			this.label12.Text = "Deductible";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(4, 97);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(66, 15);
			this.label13.TabIndex = 34;
			this.label13.Text = "Ins Used";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textSecDedRem
			// 
			this.textSecDedRem.BackColor = System.Drawing.Color.White;
			this.textSecDedRem.Location = new System.Drawing.Point(130, 75);
			this.textSecDedRem.Name = "textSecDedRem";
			this.textSecDedRem.ReadOnly = true;
			this.textSecDedRem.Size = new System.Drawing.Size(60, 20);
			this.textSecDedRem.TabIndex = 52;
			this.textSecDedRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(4, 137);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(66, 15);
			this.label14.TabIndex = 35;
			this.label14.Text = "Remaining";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(4, 117);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(66, 15);
			this.label15.TabIndex = 36;
			this.label15.Text = "Pending";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textSecMax
			// 
			this.textSecMax.BackColor = System.Drawing.Color.White;
			this.textSecMax.Location = new System.Drawing.Point(130, 35);
			this.textSecMax.Name = "textSecMax";
			this.textSecMax.ReadOnly = true;
			this.textSecMax.Size = new System.Drawing.Size(60, 20);
			this.textSecMax.TabIndex = 41;
			this.textSecMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(130, 16);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(60, 14);
			this.label16.TabIndex = 37;
			this.label16.Text = "Secondary";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// textSecDed
			// 
			this.textSecDed.BackColor = System.Drawing.Color.White;
			this.textSecDed.Location = new System.Drawing.Point(130, 55);
			this.textSecDed.Name = "textSecDed";
			this.textSecDed.ReadOnly = true;
			this.textSecDed.Size = new System.Drawing.Size(60, 20);
			this.textSecDed.TabIndex = 40;
			this.textSecDed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSecUsed
			// 
			this.textSecUsed.BackColor = System.Drawing.Color.White;
			this.textSecUsed.Location = new System.Drawing.Point(130, 95);
			this.textSecUsed.Name = "textSecUsed";
			this.textSecUsed.ReadOnly = true;
			this.textSecUsed.Size = new System.Drawing.Size(60, 20);
			this.textSecUsed.TabIndex = 39;
			this.textSecUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// groupBoxFamilyIns
			// 
			this.groupBoxFamilyIns.Controls.Add(this.textFamPriMax);
			this.groupBoxFamilyIns.Controls.Add(this.textFamPriDed);
			this.groupBoxFamilyIns.Controls.Add(this.label4);
			this.groupBoxFamilyIns.Controls.Add(this.label8);
			this.groupBoxFamilyIns.Controls.Add(this.textFamSecMax);
			this.groupBoxFamilyIns.Controls.Add(this.label9);
			this.groupBoxFamilyIns.Controls.Add(this.textFamSecDed);
			this.groupBoxFamilyIns.Controls.Add(this.label17);
			this.groupBoxFamilyIns.Location = new System.Drawing.Point(556, 64);
			this.groupBoxFamilyIns.Name = "groupBoxFamilyIns";
			this.groupBoxFamilyIns.Size = new System.Drawing.Size(193, 80);
			this.groupBoxFamilyIns.TabIndex = 218;
			this.groupBoxFamilyIns.TabStop = false;
			this.groupBoxFamilyIns.Text = "Family Insurance";
			this.groupBoxFamilyIns.Visible = false;
			// 
			// textFamPriMax
			// 
			this.textFamPriMax.BackColor = System.Drawing.Color.White;
			this.textFamPriMax.Location = new System.Drawing.Point(72, 35);
			this.textFamPriMax.Name = "textFamPriMax";
			this.textFamPriMax.ReadOnly = true;
			this.textFamPriMax.Size = new System.Drawing.Size(60, 20);
			this.textFamPriMax.TabIndex = 69;
			this.textFamPriMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textFamPriDed
			// 
			this.textFamPriDed.BackColor = System.Drawing.Color.White;
			this.textFamPriDed.Location = new System.Drawing.Point(72, 55);
			this.textFamPriDed.Name = "textFamPriDed";
			this.textFamPriDed.ReadOnly = true;
			this.textFamPriDed.Size = new System.Drawing.Size(60, 20);
			this.textFamPriDed.TabIndex = 65;
			this.textFamPriDed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(74, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 15);
			this.label4.TabIndex = 66;
			this.label4.Text = "Primary";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(4, 37);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(66, 15);
			this.label8.TabIndex = 67;
			this.label8.Text = "Annual Max";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textFamSecMax
			// 
			this.textFamSecMax.BackColor = System.Drawing.Color.White;
			this.textFamSecMax.Location = new System.Drawing.Point(131, 35);
			this.textFamSecMax.Name = "textFamSecMax";
			this.textFamSecMax.ReadOnly = true;
			this.textFamSecMax.Size = new System.Drawing.Size(60, 20);
			this.textFamSecMax.TabIndex = 70;
			this.textFamSecMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(131, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(60, 14);
			this.label9.TabIndex = 68;
			this.label9.Text = "Secondary";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// textFamSecDed
			// 
			this.textFamSecDed.BackColor = System.Drawing.Color.White;
			this.textFamSecDed.Location = new System.Drawing.Point(131, 55);
			this.textFamSecDed.Name = "textFamSecDed";
			this.textFamSecDed.ReadOnly = true;
			this.textFamSecDed.Size = new System.Drawing.Size(60, 20);
			this.textFamSecDed.TabIndex = 64;
			this.textFamSecDed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(4, 57);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(66, 15);
			this.label17.TabIndex = 63;
			this.label17.Text = "Fam Ded";
			this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textQuickProcs
			// 
			this.textQuickProcs.Location = new System.Drawing.Point(17, 3);
			this.textQuickProcs.Name = "textQuickProcs";
			this.textQuickProcs.Size = new System.Drawing.Size(100, 20);
			this.textQuickProcs.TabIndex = 220;
			this.textQuickProcs.Visible = false;
			// 
			// contextMenuPayment
			// 
			this.contextMenuPayment.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPay,
            this.menuItemIncomeTransfer,
            this.menuPrepayment});
			// 
			// menuItemPay
			// 
			this.menuItemPay.Index = 0;
			this.menuItemPay.Text = "Allocate Unearned";
			this.menuItemPay.Click += new System.EventHandler(this.menuItemPrePay_Click);
			// 
			// menuItemIncomeTransfer
			// 
			this.menuItemIncomeTransfer.Index = 1;
			this.menuItemIncomeTransfer.Text = "Income Transfer";
			this.menuItemIncomeTransfer.Click += new System.EventHandler(this.menuItemIncomeTransfer_Click);
			// 
			// menuPrepayment
			// 
			this.menuPrepayment.Index = 2;
			this.menuPrepayment.Text = "Prepayment Tool";
			this.menuPrepayment.Click += new System.EventHandler(this.menuPrepayment_Click);
			// 
			// contextMenuPayPlan
			// 
			this.contextMenuPayPlan.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDynamicPayPlan,
            this.menuItemPatPayPlan,
            this.menuItemInsPayPlan});
			// 
			// menuItemDynamicPayPlan
			// 
			this.menuItemDynamicPayPlan.Index = 0;
			this.menuItemDynamicPayPlan.Text = "Dynamic Payment Plan";
			this.menuItemDynamicPayPlan.Click += new System.EventHandler(this.MenuItemDynamicPayPlan_Click);
			// 
			// menuItemPatPayPlan
			// 
			this.menuItemPatPayPlan.Index = 1;
			this.menuItemPatPayPlan.Text = "Patient Payment Plan";
			this.menuItemPatPayPlan.Click += new System.EventHandler(this.menuItemPatPayPlan_Click);
			// 
			// menuItemInsPayPlan
			// 
			this.menuItemInsPayPlan.Index = 2;
			this.menuItemInsPayPlan.Text = "Insurance Payment Plan";
			this.menuItemInsPayPlan.Click += new System.EventHandler(this.menuItemInsPayPlan_Click);
			// 
			// contextMenuAdjust
			// 
			this.contextMenuAdjust.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSalesTax,
            this.menuItemAddMultAdj});
			// 
			// menuItemSalesTax
			// 
			this.menuItemSalesTax.Index = 0;
			this.menuItemSalesTax.Text = "Apply Sales Tax";
			this.menuItemSalesTax.Click += new System.EventHandler(this.menuItemSalesTax_Click);
			// 
			// menuItemAddMultAdj
			// 
			this.menuItemAddMultAdj.Index = 1;
			this.menuItemAddMultAdj.Text = "Add Multiple";
			this.menuItemAddMultAdj.Click += new System.EventHandler(this.menuItemAddMultAdj_Click);
			// 
			// contextMenuAcctGrid
			// 
			this.contextMenuAcctGrid.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAddAdj});
			this.contextMenuAcctGrid.Popup += new System.EventHandler(this.contextMenuAcctGrid_Popup);
			// 
			// menuItemAddAdj
			// 
			this.menuItemAddAdj.Index = 0;
			this.menuItemAddAdj.Text = "Add Adjustment";
			this.menuItemAddAdj.Click += new System.EventHandler(this.menuItemAddAdj_Click);
			// 
			// gridPatInfo
			// 
			this.gridPatInfo.Location = new System.Drawing.Point(938, 47);
			this.gridPatInfo.Name = "gridPatInfo";
			this.gridPatInfo.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridPatInfo.Size = new System.Drawing.Size(182, 510);
			this.gridPatInfo.TabIndex = 217;
			this.gridPatInfo.Title = "Patient Information";
			this.gridPatInfo.TranslationName = "TableAccountPat";
			this.gridPatInfo.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPatInfo_CellDoubleClick);
			// 
			// splitContainerParent
			// 
			this.splitContainerParent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.splitContainerParent.Location = new System.Drawing.Point(0, 63);
			this.splitContainerParent.Name = "splitContainerParent";
			this.splitContainerParent.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerParent.Panel1
			// 
			this.splitContainerParent.Panel1.Controls.Add(this.splitContainerRepChargesPP);
			this.splitContainerParent.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainerParent.Panel1MinSize = 45;
			// 
			// splitContainerParent.Panel2
			// 
			this.splitContainerParent.Panel2.Controls.Add(this.splitContainerAccountCommLog);
			this.splitContainerParent.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainerParent.Panel2MinSize = 85;
			this.splitContainerParent.Size = new System.Drawing.Size(749, 669);
			this.splitContainerParent.SplitterDistance = 195;
			this.splitContainerParent.SplitterWidth = 3;
			this.splitContainerParent.TabIndex = 222;
			// 
			// splitContainerRepChargesPP
			// 
			this.splitContainerRepChargesPP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerRepChargesPP.Location = new System.Drawing.Point(0, 0);
			this.splitContainerRepChargesPP.Name = "splitContainerRepChargesPP";
			this.splitContainerRepChargesPP.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerRepChargesPP.Panel1
			// 
			this.splitContainerRepChargesPP.Panel1.Controls.Add(this.gridRepeat);
			this.splitContainerRepChargesPP.Panel1MinSize = 20;
			// 
			// splitContainerRepChargesPP.Panel2
			// 
			this.splitContainerRepChargesPP.Panel2.Controls.Add(this.gridPayPlan);
			this.splitContainerRepChargesPP.Panel2MinSize = 20;
			this.splitContainerRepChargesPP.Size = new System.Drawing.Size(749, 195);
			this.splitContainerRepChargesPP.SplitterDistance = 79;
			this.splitContainerRepChargesPP.TabIndex = 0;
			// 
			// gridRepeat
			// 
			this.gridRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRepeat.Location = new System.Drawing.Point(0, 0);
			this.gridRepeat.Name = "gridRepeat";
			this.gridRepeat.Size = new System.Drawing.Size(749, 79);
			this.gridRepeat.TabIndex = 74;
			this.gridRepeat.Title = "Repeating Charges";
			this.gridRepeat.TranslationName = "TableRepeatCharges";
			this.gridRepeat.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridRepeat_CellDoubleClick);
			// 
			// gridPayPlan
			// 
			this.gridPayPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPayPlan.Location = new System.Drawing.Point(0, 0);
			this.gridPayPlan.Name = "gridPayPlan";
			this.gridPayPlan.Size = new System.Drawing.Size(749, 110);
			this.gridPayPlan.TabIndex = 217;
			this.gridPayPlan.Title = "Payment Plans";
			this.gridPayPlan.TranslationName = "TablePaymentPlans";
			this.gridPayPlan.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPayPlan_CellDoubleClick);
			// 
			// splitContainerAccountCommLog
			// 
			this.splitContainerAccountCommLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerAccountCommLog.Location = new System.Drawing.Point(0, 0);
			this.splitContainerAccountCommLog.Name = "splitContainerAccountCommLog";
			this.splitContainerAccountCommLog.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerAccountCommLog.Panel1
			// 
			this.splitContainerAccountCommLog.Panel1.Controls.Add(this.tabControlAccount);
			this.splitContainerAccountCommLog.Panel1MinSize = 60;
			// 
			// splitContainerAccountCommLog.Panel2
			// 
			this.splitContainerAccountCommLog.Panel2.Controls.Add(this.panelProgNotes);
			this.splitContainerAccountCommLog.Panel2.Controls.Add(this.gridComm);
			this.splitContainerAccountCommLog.Panel2MinSize = 20;
			this.splitContainerAccountCommLog.Size = new System.Drawing.Size(749, 471);
			this.splitContainerAccountCommLog.SplitterDistance = 201;
			this.splitContainerAccountCommLog.TabIndex = 0;
			// 
			// tabControlAccount
			// 
			this.tabControlAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlAccount.Controls.Add(this.tabPagePatAccount);
			this.tabControlAccount.Controls.Add(this.tabPageAutoOrtho);
			this.tabControlAccount.Controls.Add(this.tabPageOrthoCases);
			this.tabControlAccount.Controls.Add(this.tabPageHiddenSplits);
			this.tabControlAccount.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControlAccount.ItemSize = new System.Drawing.Size(370, 18);
			this.tabControlAccount.Location = new System.Drawing.Point(0, 0);
			this.tabControlAccount.Margin = new System.Windows.Forms.Padding(0);
			this.tabControlAccount.Multiline = true;
			this.tabControlAccount.Name = "tabControlAccount";
			this.tabControlAccount.Padding = new System.Drawing.Point(0, 0);
			this.tabControlAccount.SelectedIndex = 0;
			this.tabControlAccount.Size = new System.Drawing.Size(754, 203);
			this.tabControlAccount.TabIndex = 221;
			this.tabControlAccount.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TabControlAccount_DrawItem);
			// 
			// tabPagePatAccount
			// 
			this.tabPagePatAccount.Controls.Add(this.gridAccount);
			this.tabPagePatAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabPagePatAccount.ForeColor = System.Drawing.Color.Black;
			this.tabPagePatAccount.Location = new System.Drawing.Point(4, 22);
			this.tabPagePatAccount.Margin = new System.Windows.Forms.Padding(0);
			this.tabPagePatAccount.Name = "tabPagePatAccount";
			this.tabPagePatAccount.Size = new System.Drawing.Size(746, 177);
			this.tabPagePatAccount.TabIndex = 0;
			this.tabPagePatAccount.Text = "Patient Account";
			this.tabPagePatAccount.UseVisualStyleBackColor = true;
			// 
			// gridAccount
			// 
			this.gridAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAccount.ContextMenu = this.contextMenuAcctGrid;
			this.gridAccount.HScrollVisible = true;
			this.gridAccount.Location = new System.Drawing.Point(0, 0);
			this.gridAccount.Margin = new System.Windows.Forms.Padding(0);
			this.gridAccount.Name = "gridAccount";
			this.gridAccount.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridAccount.Size = new System.Drawing.Size(746, 177);
			this.gridAccount.TabIndex = 73;
			this.gridAccount.Title = "Patient Account";
			this.gridAccount.TranslationName = "TableAccount";
			this.gridAccount.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAccount_CellDoubleClick);
			this.gridAccount.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAccount_CellClick);
			// 
			// tabPageAutoOrtho
			// 
			this.tabPageAutoOrtho.BackColor = System.Drawing.Color.Transparent;
			this.tabPageAutoOrtho.Controls.Add(this.splitContainerAutoOrtho);
			this.tabPageAutoOrtho.Location = new System.Drawing.Point(4, 22);
			this.tabPageAutoOrtho.Name = "tabPageAutoOrtho";
			this.tabPageAutoOrtho.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageAutoOrtho.Size = new System.Drawing.Size(746, 177);
			this.tabPageAutoOrtho.TabIndex = 1;
			this.tabPageAutoOrtho.Text = "Auto Ortho";
			// 
			// splitContainerAutoOrtho
			// 
			this.splitContainerAutoOrtho.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerAutoOrtho.Location = new System.Drawing.Point(3, 3);
			this.splitContainerAutoOrtho.Name = "splitContainerAutoOrtho";
			// 
			// splitContainerAutoOrtho.Panel1
			// 
			this.splitContainerAutoOrtho.Panel1.Controls.Add(this.gridAutoOrtho);
			this.splitContainerAutoOrtho.Panel1MinSize = 200;
			// 
			// splitContainerAutoOrtho.Panel2
			// 
			this.splitContainerAutoOrtho.Panel2.Controls.Add(this.groupBox2);
			this.splitContainerAutoOrtho.Panel2.Controls.Add(this.groupBox1);
			this.splitContainerAutoOrtho.Size = new System.Drawing.Size(740, 171);
			this.splitContainerAutoOrtho.SplitterDistance = 341;
			this.splitContainerAutoOrtho.TabIndex = 1;
			// 
			// gridAutoOrtho
			// 
			this.gridAutoOrtho.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridAutoOrtho.HeadersVisible = false;
			this.gridAutoOrtho.Location = new System.Drawing.Point(0, 0);
			this.gridAutoOrtho.Margin = new System.Windows.Forms.Padding(0);
			this.gridAutoOrtho.Name = "gridAutoOrtho";
			this.gridAutoOrtho.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridAutoOrtho.Size = new System.Drawing.Size(341, 171);
			this.gridAutoOrtho.TabIndex = 1;
			this.gridAutoOrtho.Title = "Ortho Info";
			this.gridAutoOrtho.TranslationName = "TableOrthoInfo";
			this.gridAutoOrtho.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAutoOrtho_CellDoubleClick);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.butAutoOrthoDefaultPlacement);
			this.groupBox2.Controls.Add(this.textDateAutoOrthoPlacement);
			this.groupBox2.Controls.Add(this.butEditAutoOrthoPlacement);
			this.groupBox2.Location = new System.Drawing.Point(3, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 67);
			this.groupBox2.TabIndex = 123;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Ortho Placement Date";
			// 
			// butAutoOrthoDefaultPlacement
			// 
			this.butAutoOrthoDefaultPlacement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAutoOrthoDefaultPlacement.Location = new System.Drawing.Point(90, 41);
			this.butAutoOrthoDefaultPlacement.Name = "butAutoOrthoDefaultPlacement";
			this.butAutoOrthoDefaultPlacement.Size = new System.Drawing.Size(51, 22);
			this.butAutoOrthoDefaultPlacement.TabIndex = 122;
			this.butAutoOrthoDefaultPlacement.Text = "Default";
			this.butAutoOrthoDefaultPlacement.Click += new System.EventHandler(this.butAutoOrthoDefaultPlacement_Click);
			// 
			// textDateAutoOrthoPlacement
			// 
			this.textDateAutoOrthoPlacement.Location = new System.Drawing.Point(6, 16);
			this.textDateAutoOrthoPlacement.Name = "textDateAutoOrthoPlacement";
			this.textDateAutoOrthoPlacement.Size = new System.Drawing.Size(78, 20);
			this.textDateAutoOrthoPlacement.TabIndex = 119;
			// 
			// butEditAutoOrthoPlacement
			// 
			this.butEditAutoOrthoPlacement.Image = global::OpenDental.Properties.Resources.Left;
			this.butEditAutoOrthoPlacement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEditAutoOrthoPlacement.Location = new System.Drawing.Point(6, 41);
			this.butEditAutoOrthoPlacement.Name = "butEditAutoOrthoPlacement";
			this.butEditAutoOrthoPlacement.Size = new System.Drawing.Size(78, 22);
			this.butEditAutoOrthoPlacement.TabIndex = 118;
			this.butEditAutoOrthoPlacement.Text = "Override";
			this.butEditAutoOrthoPlacement.Click += new System.EventHandler(this.butEditAutoOrthoPlacement_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textAutoOrthoMonthsTreat);
			this.groupBox1.Controls.Add(this.butAutoOrthoDefaultMonthsTreat);
			this.groupBox1.Controls.Add(this.butAutoOrthoEditMonthsTreat);
			this.groupBox1.Location = new System.Drawing.Point(3, 75);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 68);
			this.groupBox1.TabIndex = 122;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Patient Ortho Months Treat";
			// 
			// textAutoOrthoMonthsTreat
			// 
			this.textAutoOrthoMonthsTreat.Location = new System.Drawing.Point(6, 19);
			this.textAutoOrthoMonthsTreat.MaxVal = 255;
			this.textAutoOrthoMonthsTreat.MinVal = 0;
			this.textAutoOrthoMonthsTreat.Name = "textAutoOrthoMonthsTreat";
			this.textAutoOrthoMonthsTreat.Size = new System.Drawing.Size(78, 20);
			this.textAutoOrthoMonthsTreat.TabIndex = 119;
			// 
			// butAutoOrthoDefaultMonthsTreat
			// 
			this.butAutoOrthoDefaultMonthsTreat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAutoOrthoDefaultMonthsTreat.Location = new System.Drawing.Point(90, 41);
			this.butAutoOrthoDefaultMonthsTreat.Name = "butAutoOrthoDefaultMonthsTreat";
			this.butAutoOrthoDefaultMonthsTreat.Size = new System.Drawing.Size(51, 22);
			this.butAutoOrthoDefaultMonthsTreat.TabIndex = 121;
			this.butAutoOrthoDefaultMonthsTreat.Text = "Default";
			this.butAutoOrthoDefaultMonthsTreat.Click += new System.EventHandler(this.butAutoOrthoDefaultMonthsTreat_Click);
			// 
			// butAutoOrthoEditMonthsTreat
			// 
			this.butAutoOrthoEditMonthsTreat.Image = global::OpenDental.Properties.Resources.Left;
			this.butAutoOrthoEditMonthsTreat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAutoOrthoEditMonthsTreat.Location = new System.Drawing.Point(6, 41);
			this.butAutoOrthoEditMonthsTreat.Name = "butAutoOrthoEditMonthsTreat";
			this.butAutoOrthoEditMonthsTreat.Size = new System.Drawing.Size(78, 22);
			this.butAutoOrthoEditMonthsTreat.TabIndex = 118;
			this.butAutoOrthoEditMonthsTreat.Text = "Override";
			this.butAutoOrthoEditMonthsTreat.Click += new System.EventHandler(this.butAutoOrthoEditMonthsTreat_Click);
			// 
			// tabPageOrthoCases
			// 
			this.tabPageOrthoCases.BackColor = System.Drawing.Color.Transparent;
			this.tabPageOrthoCases.Controls.Add(this.splitContainerOrthoCases);
			this.tabPageOrthoCases.Location = new System.Drawing.Point(4, 22);
			this.tabPageOrthoCases.Name = "tabPageOrthoCases";
			this.tabPageOrthoCases.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageOrthoCases.Size = new System.Drawing.Size(746, 177);
			this.tabPageOrthoCases.TabIndex = 3;
			this.tabPageOrthoCases.Text = "Ortho Cases";
			// 
			// splitContainerOrthoCases
			// 
			this.splitContainerOrthoCases.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerOrthoCases.Location = new System.Drawing.Point(3, 3);
			this.splitContainerOrthoCases.Name = "splitContainerOrthoCases";
			// 
			// splitContainerOrthoCases.Panel1
			// 
			this.splitContainerOrthoCases.Panel1.Controls.Add(this.gridOrthoCases);
			this.splitContainerOrthoCases.Panel1MinSize = 396;
			// 
			// splitContainerOrthoCases.Panel2
			// 
			this.splitContainerOrthoCases.Panel2.Controls.Add(this.butMakeOrthoCaseActive);
			this.splitContainerOrthoCases.Panel2.Controls.Add(this.checkHideInactiveOrthoCases);
			this.splitContainerOrthoCases.Panel2.Controls.Add(this.butAddOrthoCase);
			this.splitContainerOrthoCases.Panel2MinSize = 216;
			this.splitContainerOrthoCases.Size = new System.Drawing.Size(740, 171);
			this.splitContainerOrthoCases.SplitterDistance = 396;
			this.splitContainerOrthoCases.TabIndex = 0;
			// 
			// gridOrthoCases
			// 
			this.gridOrthoCases.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridOrthoCases.Location = new System.Drawing.Point(0, 0);
			this.gridOrthoCases.Margin = new System.Windows.Forms.Padding(0);
			this.gridOrthoCases.Name = "gridOrthoCases";
			this.gridOrthoCases.Size = new System.Drawing.Size(396, 171);
			this.gridOrthoCases.TabIndex = 2;
			this.gridOrthoCases.Title = "Ortho Cases";
			this.gridOrthoCases.TranslationName = "TableOrthoCases";
			this.gridOrthoCases.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.GridOrthoCases_CellDoubleClick);
			// 
			// butMakeOrthoCaseActive
			// 
			this.butMakeOrthoCaseActive.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butMakeOrthoCaseActive.Location = new System.Drawing.Point(3, 30);
			this.butMakeOrthoCaseActive.Name = "butMakeOrthoCaseActive";
			this.butMakeOrthoCaseActive.Size = new System.Drawing.Size(112, 24);
			this.butMakeOrthoCaseActive.TabIndex = 220;
			this.butMakeOrthoCaseActive.Text = "Make Active";
			this.butMakeOrthoCaseActive.Click += new System.EventHandler(this.ButMakeOrthoCaseActive_Click);
			// 
			// checkHideInactiveOrthoCases
			// 
			this.checkHideInactiveOrthoCases.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHideInactiveOrthoCases.Location = new System.Drawing.Point(4, 55);
			this.checkHideInactiveOrthoCases.Name = "checkHideInactiveOrthoCases";
			this.checkHideInactiveOrthoCases.Size = new System.Drawing.Size(215, 16);
			this.checkHideInactiveOrthoCases.TabIndex = 219;
			this.checkHideInactiveOrthoCases.Text = "Hide Inactive Ortho Cases";
			this.checkHideInactiveOrthoCases.CheckedChanged += new System.EventHandler(this.CheckHideInactiveOrthoCases_CheckedChanged);
			// 
			// butAddOrthoCase
			// 
			this.butAddOrthoCase.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddOrthoCase.Location = new System.Drawing.Point(3, 3);
			this.butAddOrthoCase.Name = "butAddOrthoCase";
			this.butAddOrthoCase.Size = new System.Drawing.Size(112, 24);
			this.butAddOrthoCase.TabIndex = 123;
			this.butAddOrthoCase.Text = "Add Ortho Case";
			this.butAddOrthoCase.Click += new System.EventHandler(this.ButAddOrthoCase_Click);
			// 
			// tabPageHiddenSplits
			// 
			this.tabPageHiddenSplits.BackColor = System.Drawing.Color.Transparent;
			this.tabPageHiddenSplits.Controls.Add(this.gridTpSplits);
			this.tabPageHiddenSplits.ForeColor = System.Drawing.SystemColors.Window;
			this.tabPageHiddenSplits.Location = new System.Drawing.Point(4, 22);
			this.tabPageHiddenSplits.Name = "tabPageHiddenSplits";
			this.tabPageHiddenSplits.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageHiddenSplits.Size = new System.Drawing.Size(746, 177);
			this.tabPageHiddenSplits.TabIndex = 2;
			this.tabPageHiddenSplits.Text = "Hidden Splits";
			// 
			// gridTpSplits
			// 
			this.gridTpSplits.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridTpSplits.Location = new System.Drawing.Point(3, 3);
			this.gridTpSplits.Name = "gridTpSplits";
			this.gridTpSplits.Size = new System.Drawing.Size(740, 171);
			this.gridTpSplits.TabIndex = 218;
			this.gridTpSplits.Title = "Payments Hidden on Account and Reports";
			this.gridTpSplits.TranslationName = "TableTreatmentPlannedUnearned";
			this.gridTpSplits.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.GridTpSplits_CellDoubleClick);
			// 
			// panelProgNotes
			// 
			this.panelProgNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelProgNotes.Controls.Add(this.butShowNone);
			this.panelProgNotes.Controls.Add(this.butShowAll);
			this.panelProgNotes.Controls.Add(this.checkNotes);
			this.panelProgNotes.Controls.Add(this.groupBox7);
			this.panelProgNotes.Controls.Add(this.groupBox6);
			this.panelProgNotes.Controls.Add(this.gridProg);
			this.panelProgNotes.Location = new System.Drawing.Point(0, 21);
			this.panelProgNotes.Name = "panelProgNotes";
			this.panelProgNotes.Size = new System.Drawing.Size(749, 245);
			this.panelProgNotes.TabIndex = 211;
			// 
			// butShowNone
			// 
			this.butShowNone.Location = new System.Drawing.Point(677, 207);
			this.butShowNone.Name = "butShowNone";
			this.butShowNone.Size = new System.Drawing.Size(58, 16);
			this.butShowNone.TabIndex = 216;
			this.butShowNone.Text = "None";
			this.butShowNone.Visible = false;
			this.butShowNone.Click += new System.EventHandler(this.butShowNone_Click);
			// 
			// butShowAll
			// 
			this.butShowAll.Location = new System.Drawing.Point(614, 207);
			this.butShowAll.Name = "butShowAll";
			this.butShowAll.Size = new System.Drawing.Size(53, 16);
			this.butShowAll.TabIndex = 215;
			this.butShowAll.Text = "All";
			this.butShowAll.Visible = false;
			this.butShowAll.Click += new System.EventHandler(this.butShowAll_Click);
			// 
			// checkNotes
			// 
			this.checkNotes.AllowDrop = true;
			this.checkNotes.Checked = true;
			this.checkNotes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNotes.Location = new System.Drawing.Point(624, 191);
			this.checkNotes.Name = "checkNotes";
			this.checkNotes.Size = new System.Drawing.Size(102, 13);
			this.checkNotes.TabIndex = 214;
			this.checkNotes.Text = "Proc Notes";
			this.checkNotes.Visible = false;
			this.checkNotes.Click += new System.EventHandler(this.checkNotes_Click);
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.checkShowTeeth);
			this.groupBox7.Controls.Add(this.checkAudit);
			this.groupBox7.Controls.Add(this.checkExtraNotes);
			this.groupBox7.Controls.Add(this.checkAppt);
			this.groupBox7.Controls.Add(this.checkLabCase);
			this.groupBox7.Controls.Add(this.checkRx);
			this.groupBox7.Controls.Add(this.checkComm);
			this.groupBox7.Location = new System.Drawing.Point(614, 88);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(121, 101);
			this.groupBox7.TabIndex = 213;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Object Types";
			this.groupBox7.Visible = false;
			// 
			// checkShowTeeth
			// 
			this.checkShowTeeth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTeeth.Location = new System.Drawing.Point(44, 63);
			this.checkShowTeeth.Name = "checkShowTeeth";
			this.checkShowTeeth.Size = new System.Drawing.Size(104, 13);
			this.checkShowTeeth.TabIndex = 217;
			this.checkShowTeeth.Text = "Selected Teeth";
			this.checkShowTeeth.Visible = false;
			// 
			// checkAudit
			// 
			this.checkAudit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAudit.Location = new System.Drawing.Point(44, 79);
			this.checkAudit.Name = "checkAudit";
			this.checkAudit.Size = new System.Drawing.Size(73, 13);
			this.checkAudit.TabIndex = 218;
			this.checkAudit.Text = "Audit";
			this.checkAudit.Visible = false;
			// 
			// checkExtraNotes
			// 
			this.checkExtraNotes.AllowDrop = true;
			this.checkExtraNotes.Checked = true;
			this.checkExtraNotes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkExtraNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExtraNotes.Location = new System.Drawing.Point(9, 82);
			this.checkExtraNotes.Name = "checkExtraNotes";
			this.checkExtraNotes.Size = new System.Drawing.Size(102, 13);
			this.checkExtraNotes.TabIndex = 215;
			this.checkExtraNotes.Text = "Extra Notes";
			this.checkExtraNotes.Visible = false;
			this.checkExtraNotes.Click += new System.EventHandler(this.checkExtraNotes_Click);
			// 
			// checkAppt
			// 
			this.checkAppt.Checked = true;
			this.checkAppt.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAppt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAppt.Location = new System.Drawing.Point(10, 17);
			this.checkAppt.Name = "checkAppt";
			this.checkAppt.Size = new System.Drawing.Size(102, 13);
			this.checkAppt.TabIndex = 20;
			this.checkAppt.Text = "Appointments";
			this.checkAppt.Click += new System.EventHandler(this.checkAppt_Click);
			// 
			// checkLabCase
			// 
			this.checkLabCase.Checked = true;
			this.checkLabCase.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkLabCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkLabCase.Location = new System.Drawing.Point(10, 49);
			this.checkLabCase.Name = "checkLabCase";
			this.checkLabCase.Size = new System.Drawing.Size(102, 13);
			this.checkLabCase.TabIndex = 17;
			this.checkLabCase.Text = "Lab Cases";
			this.checkLabCase.Click += new System.EventHandler(this.checkLabCase_Click);
			// 
			// checkRx
			// 
			this.checkRx.Checked = true;
			this.checkRx.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkRx.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRx.Location = new System.Drawing.Point(10, 65);
			this.checkRx.Name = "checkRx";
			this.checkRx.Size = new System.Drawing.Size(102, 13);
			this.checkRx.TabIndex = 8;
			this.checkRx.Text = "Rx";
			this.checkRx.Click += new System.EventHandler(this.checkRx_Click);
			// 
			// checkComm
			// 
			this.checkComm.Checked = true;
			this.checkComm.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkComm.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkComm.Location = new System.Drawing.Point(10, 33);
			this.checkComm.Name = "checkComm";
			this.checkComm.Size = new System.Drawing.Size(102, 13);
			this.checkComm.TabIndex = 16;
			this.checkComm.Text = "Comm Log";
			this.checkComm.Click += new System.EventHandler(this.checkComm_Click);
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.checkShowE);
			this.groupBox6.Controls.Add(this.checkShowR);
			this.groupBox6.Controls.Add(this.checkShowC);
			this.groupBox6.Controls.Add(this.checkShowTP);
			this.groupBox6.Location = new System.Drawing.Point(614, 1);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(121, 85);
			this.groupBox6.TabIndex = 212;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Procedures";
			this.groupBox6.Visible = false;
			// 
			// checkShowE
			// 
			this.checkShowE.Checked = true;
			this.checkShowE.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowE.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowE.Location = new System.Drawing.Point(9, 49);
			this.checkShowE.Name = "checkShowE";
			this.checkShowE.Size = new System.Drawing.Size(101, 13);
			this.checkShowE.TabIndex = 10;
			this.checkShowE.Text = "Existing";
			this.checkShowE.Click += new System.EventHandler(this.checkShowE_Click);
			// 
			// checkShowR
			// 
			this.checkShowR.Checked = true;
			this.checkShowR.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowR.Location = new System.Drawing.Point(9, 65);
			this.checkShowR.Name = "checkShowR";
			this.checkShowR.Size = new System.Drawing.Size(101, 13);
			this.checkShowR.TabIndex = 14;
			this.checkShowR.Text = "Referred";
			this.checkShowR.Click += new System.EventHandler(this.checkShowR_Click);
			// 
			// checkShowC
			// 
			this.checkShowC.Checked = true;
			this.checkShowC.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowC.Location = new System.Drawing.Point(9, 33);
			this.checkShowC.Name = "checkShowC";
			this.checkShowC.Size = new System.Drawing.Size(101, 13);
			this.checkShowC.TabIndex = 9;
			this.checkShowC.Text = "Completed";
			this.checkShowC.Click += new System.EventHandler(this.checkShowC_Click);
			// 
			// checkShowTP
			// 
			this.checkShowTP.Checked = true;
			this.checkShowTP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowTP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTP.Location = new System.Drawing.Point(9, 17);
			this.checkShowTP.Name = "checkShowTP";
			this.checkShowTP.Size = new System.Drawing.Size(101, 13);
			this.checkShowTP.TabIndex = 8;
			this.checkShowTP.Text = "Treat Plan";
			this.checkShowTP.Click += new System.EventHandler(this.checkShowTP_Click);
			// 
			// gridProg
			// 
			this.gridProg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProg.HScrollVisible = true;
			this.gridProg.Location = new System.Drawing.Point(3, 0);
			this.gridProg.Name = "gridProg";
			this.gridProg.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridProg.Size = new System.Drawing.Size(603, 248);
			this.gridProg.TabIndex = 211;
			this.gridProg.Title = "Progress Notes";
			this.gridProg.TranslationName = "TableProg";
			this.gridProg.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProg_CellDoubleClick);
			// 
			// gridComm
			// 
			this.gridComm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridComm.Location = new System.Drawing.Point(0, 0);
			this.gridComm.Name = "gridComm";
			this.gridComm.Size = new System.Drawing.Size(749, 266);
			this.gridComm.TabIndex = 71;
			this.gridComm.Title = "Communications Log";
			this.gridComm.TranslationName = "TableCommLogAccount";
			this.gridComm.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridComm_CellDoubleClick);
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(1123, 25);
			this.ToolBarMain.TabIndex = 47;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// ContrAccount
			// 
			this.Controls.Add(this.groupBoxFamilyIns);
			this.Controls.Add(this.groupBoxIndIns);
			this.Controls.Add(this.splitContainerParent);
			this.Controls.Add(this.gridPatInfo);
			this.Controls.Add(this.tabControlShow);
			this.Controls.Add(this.panelAging);
			this.Controls.Add(this.ToolBarMain);
			this.Name = "ContrAccount";
			this.Size = new System.Drawing.Size(1123, 732);
			this.Load += new System.EventHandler(this.ContrAccount_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ContrAccount_Layout);
			this.Resize += new System.EventHandler(this.ContrAccount_Resize);
			this.panelAging.ResumeLayout(false);
			this.panelAging.PerformLayout();
			this.panelTotalOwes.ResumeLayout(false);
			this.tabControlShow.ResumeLayout(false);
			this.tabMain.ResumeLayout(false);
			this.tabShow.ResumeLayout(false);
			this.tabShow.PerformLayout();
			this.groupBoxIndIns.ResumeLayout(false);
			this.groupBoxIndIns.PerformLayout();
			this.groupBoxFamilyIns.ResumeLayout(false);
			this.groupBoxFamilyIns.PerformLayout();
			this.splitContainerParent.Panel1.ResumeLayout(false);
			this.splitContainerParent.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerParent)).EndInit();
			this.splitContainerParent.ResumeLayout(false);
			this.splitContainerRepChargesPP.Panel1.ResumeLayout(false);
			this.splitContainerRepChargesPP.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerRepChargesPP)).EndInit();
			this.splitContainerRepChargesPP.ResumeLayout(false);
			this.splitContainerAccountCommLog.Panel1.ResumeLayout(false);
			this.splitContainerAccountCommLog.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerAccountCommLog)).EndInit();
			this.splitContainerAccountCommLog.ResumeLayout(false);
			this.tabControlAccount.ResumeLayout(false);
			this.tabPagePatAccount.ResumeLayout(false);
			this.tabPageAutoOrtho.ResumeLayout(false);
			this.splitContainerAutoOrtho.Panel1.ResumeLayout(false);
			this.splitContainerAutoOrtho.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerAutoOrtho)).EndInit();
			this.splitContainerAutoOrtho.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPageOrthoCases.ResumeLayout(false);
			this.splitContainerOrthoCases.Panel1.ResumeLayout(false);
			this.splitContainerOrthoCases.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerOrthoCases)).EndInit();
			this.splitContainerOrthoCases.ResumeLayout(false);
			this.tabPageHiddenSplits.ResumeLayout(false);
			this.panelProgNotes.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		///<summary></summary>
		public void InitializeOnStartup() {
			if(InitializedOnStartup && !ViewingInRecall) {
				return;
			}
			InitializedOnStartup=true;
			//can't use Lan.F(this);
			Lan.C(this,new Control[]
				{
          labelStartDate,
					labelEndDate,
					label2,
					label7,
					label6,
					label5,
					label3,
					labelUrgFinNote,
					labelFamFinancial,
					tabControlAccount,
					gridAccount,
					gridAcctPat,
					gridComm,
					gridPatInfo,
					gridPayPlan,
					gridProg,
					gridRepeat,
					labelInsEst,
					labelBalance,
					labelPatEstBal,
					labelUnearned,
					labelInsRem,
					tabMain,
					tabShow,
					butToday,
					but45days,
					but90days,
					butDatesAll,
					butRefresh,
					butShowAll,
					butShowNone,
					butCreditCard
				});
			Lan.C(this,contextMenuIns,contextMenuStatement);
			LayoutToolBar();
			textQuickProcs.AcceptsTab=true;
			textQuickProcs.KeyDown+=textQuickCharge_KeyDown;
			textQuickProcs.MouseDown+=textQuickCharge_MouseClick;
			textQuickProcs.MouseCaptureChanged+=textQuickCharge_CaptureChange;
			textQuickProcs.LostFocus+=textQuickCharge_FocusLost;
			ToolBarMain.Controls.Add(textQuickProcs);
			splitContainerAccountCommLog.SplitterDistance=splitContainerParent.Panel2.Height * 3/5;//Make Account grid slightly bigger than commlog
			//This just makes the patient information grid show up or not.
			_patInfoDisplayFields=DisplayFields.GetForCategory(DisplayFieldCategory.AccountPatientInformation);
			LayoutPanels();
			checkShowFamilyComm.Checked=PrefC.GetBoolSilent(PrefName.ShowAccountFamilyCommEntries,true);
			checkShowCompletePayPlans.Checked=PrefC.GetBool(PrefName.AccountShowCompletedPaymentPlans);
			Plugins.HookAddCode(this,"ContrAccount.InitializeOnStartup_end");
		}

		private void textQuickCharge_MouseClick(object sender,MouseEventArgs e) {
			if(e.X<0 || e.X>textQuickProcs.Width ||e.Y<0 || e.Y>textQuickProcs.Height) {
				textQuickProcs.Text="";
				textQuickProcs.Visible=false;
				textQuickProcs.Capture=false;
			}
		}

		private void textQuickCharge_CaptureChange(object sender,EventArgs e) {
			if(textQuickProcs.Visible==true) {
				textQuickProcs.Capture=true;
			}
		}

		private void ContrAccount_Load(object sender,System.EventArgs e) {
			this.Parent.MouseWheel+=new MouseEventHandler(Parent_MouseWheel);
			if(!PrefC.IsODHQ) {
				menuPrepayment.Visible=false;
				menuPrepayment.Enabled=false;
			}
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ODToolBarButton button;
			_butPayment=new ODToolBarButton(Lan.g(this,"Payment"),1,"","Payment");
			_butPayment.Style=ODToolBarButtonStyle.DropDownButton;
			_butPayment.DropDownMenu=contextMenuPayment;
			ToolBarMain.Buttons.Add(_butPayment);
			button=new ODToolBarButton(Lan.g(this,"Adjustment"),2,"","Adjustment");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=contextMenuAdjust;
			ToolBarMain.Buttons.Add(button);
			button=new ODToolBarButton(Lan.g(this,"New Claim"),3,"","Insurance");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=contextMenuIns;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.g(this,"Payment Plan"),-1,"","PayPlan");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=contextMenuPayPlan;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Installment Plan"),-1,"","InstallPlan"));
			if(Security.IsAuthorized(Permissions.AccountProcsQuickAdd,true)) {
				//If the user doesn't have permission to use the quick charge button don't add it to the toolbar.
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				_butQuickProcs=new ODToolBarButton(Lan.g(this,"Quick Procs"),-1,"","QuickProcs");
				_butQuickProcs.Style=ODToolBarButtonStyle.DropDownButton;
				_butQuickProcs.DropDownMenu=contextMenuQuickProcs;
				contextMenuQuickProcs.Popup+=new EventHandler(contextMenuQuickProcs_Popup);
				ToolBarMain.Buttons.Add(_butQuickProcs);
			}
			if(!PrefC.GetBool(PrefName.EasyHideRepeatCharges)) {
				button=new ODToolBarButton(Lan.g(this,"Repeating Charge"),-1,"","RepeatCharge");
				button.Style=ODToolBarButtonStyle.PushButton;
				if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {//contextMenuRepeat items only get initialized when at HQ.
					button.Style=ODToolBarButtonStyle.DropDownButton;
					button.DropDownMenu=contextMenuRepeat;
				}
				ToolBarMain.Buttons.Add(button);
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.g(this,"Statement"),4,"","Statement");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=contextMenuStatement;
			ToolBarMain.Buttons.Add(button);
			if(PrefC.GetBool(PrefName.AccountShowQuestionnaire)) {
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Questionnaire"),-1,"","Questionnaire"));
			}
			ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.AccountModule);
			ToolBarMain.Invalidate();
			Plugins.HookAddCode(this,"ContrAccount.LayoutToolBar_end",PatCur);
		}

		///<summary>This gets run just prior to the contextMenuQuickCharge menu displaying to the user.</summary>
		private void contextMenuQuickProcs_Popup(object sender,EventArgs e) {
			//Dynamically fill contextMenuQuickCharge's menu items because the definitions may have changed since last time it was filled.
			_acctProcQuickAddDefs=Defs.GetDefsForCategory(DefCat.AccountQuickCharge,true).ToArray();
			contextMenuQuickProcs.MenuItems.Clear();
			for(int i=0;i<_acctProcQuickAddDefs.Length;i++) {
				contextMenuQuickProcs.MenuItems.Add(new MenuItem(_acctProcQuickAddDefs[i].ItemName,menuItemQuickProcs_Click));
			}
			if(_acctProcQuickAddDefs.Length==0) {
				contextMenuQuickProcs.MenuItems.Add(new MenuItem(Lan.g(this,"No quick charge procedures defined. Go to Setup | Definitions to add."),(x,y) => { }));//"null" event handler.
			}
		}

		private void ContrAccount_Layout(object sender,System.Windows.Forms.LayoutEventArgs e) {
			//see LayoutPanels()
		}

		private void ContrAccount_Resize(object sender,EventArgs e) {
			if(PrefC.HListIsNull()){
				return;//helps on startup.
			}
			LayoutPanels();
		}

		///<summary>This used to be a layout event, but that was making it get called far too frequently.  Now, this must explicitly and intelligently be called.</summary>
		private void LayoutPanels(){
			//Collapse panels according to what is visible at the given time.
			//If both are not visible, collapse the entire parent panel so it does not show extra white space.
			splitContainerParent.Panel1Collapsed=!gridRepeat.Visible && !gridPayPlan.Visible;
			if(!gridRepeat.Visible) {
				splitContainerRepChargesPP.Panel1Collapsed=true;
				splitContainerParent.Panel1MinSize=20;
			}
			if(!gridPayPlan.Visible) {
				splitContainerRepChargesPP.Panel2Collapsed=true;
				splitContainerParent.Panel1MinSize=20;
			}
			//If both visible, make sure the minimum size is set back to orignal value.
			if(gridPayPlan.Visible && gridRepeat.Visible) {
				splitContainerParent.Panel1MinSize=45;
			}
			//60px is the height needed for the tabs, the grid title, and the horizontal scrollbar.
			splitContainerAccountCommLog.Panel1MinSize=60-(gridAccount.HScrollVisible ? 0 : gridAccount.HScrollHeight);
			//85px is the height needed for the account grid and the commlog grid.
			splitContainerParent.Panel2MinSize=85-(gridAccount.HScrollVisible ? 0 : gridAccount.HScrollHeight);
			if(_patInfoDisplayFields!=null && _patInfoDisplayFields.Count==0) {
				gridPatInfo.Visible=false;
			}
			gridProg.Top=0;
			gridProg.Height=panelProgNotes.Height;
			/*
			panelBoldBalance.Left=329;
			panelBoldBalance.Top=29;
			panelInsInfoDetail.Top = panelBoldBalance.Top + panelBoldBalance.Height;
			panelInsInfoDetail.Left = panelBoldBalance.Left + panelBoldBalance.Width - panelInsInfoDetail.Width;*/
			int left=textUrgFinNote.Left;//769;
			labelFamFinancial.Location=new Point(left,gridAcctPat.Bottom);
			textFinNote.Location=new Point(left,labelFamFinancial.Bottom);
			//tabControlShow.Height=panelCommButs.Top-tabControlShow.Top;
			textFinNote.Height=tabMain.Height-textFinNote.Top;
			//only show the auto ortho grid and tab control if they have the show feature enabled.
			//otherwise, hide the tabs and re-size the account grid.
			if(!PrefC.GetBool(PrefName.OrthoEnabled)) {
				tabControlAccount.TabPages.Remove(tabPageAutoOrtho);
			}
			else if(!tabControlAccount.TabPages.Contains(tabPageAutoOrtho)) {
				tabControlAccount.TabPages.Add(tabPageAutoOrtho);
			}
			if(!OrthoCases.HasOrthoCasesEnabled()) {
				tabControlAccount.TabPages.Remove(tabPageOrthoCases);
			}
			else if(!tabControlAccount.TabPages.Contains(tabPageOrthoCases)) {
				tabControlAccount.TabPages.Add(tabPageOrthoCases);
			}
			if(_listSplitsHidden.Count==0) {//might need to get updated more often than from loadData. Not sure how much we care. 
				tabControlAccount.TabPages.Remove(tabPageHiddenSplits);
			}
			else if(!tabControlAccount.TabPages.Contains(tabPageHiddenSplits)) {
				tabControlAccount.TabPages.Add(tabPageHiddenSplits);
			}
			if(!tabControlAccount.TabPages.Contains(tabPageAutoOrtho) && !tabControlAccount.TabPages.Contains(tabPageHiddenSplits)
				&& !tabControlAccount.TabPages.Contains(tabPageOrthoCases)) {
				tabControlAccount.Appearance=TabAppearance.FlatButtons;
				tabControlAccount.SizeMode=TabSizeMode.Fixed;
				tabControlAccount.ItemSize=new Size(0,1);
				tabControlAccount.Bounds = new Rectangle(-4,tabControlAccount.Bounds.Y,gridComm.Width+8,tabControlAccount.Height);
			}
			else {//at least one tab present
				tabControlAccount.Appearance=TabAppearance.Normal;
				tabControlAccount.SizeMode=TabSizeMode.Normal;
				tabControlAccount.ItemSize=new Size(370,18);
				tabControlAccount.Bounds = new Rectangle(0,tabControlAccount.Bounds.Y,gridComm.Width+6,tabControlAccount.Height);
			}
		}

		private void TabControlAccount_DrawItem(object sender,DrawItemEventArgs e) {
			TabPage page=tabControlAccount.TabPages[e.Index];
			Rectangle tabArea=tabControlAccount.GetTabRect(e.Index);
			if(page!=tabPageHiddenSplits) {
				TextRenderer.DrawText(e.Graphics,page.Text,Font,tabArea,page.ForeColor);
				return;
			}
			e.Graphics.FillRectangle(new SolidBrush(Color.Red),tabArea);
			TextRenderer.DrawText(e.Graphics,page.Text,Font,tabArea,page.ForeColor);
		}

		///<summary></summary>
		public void ModuleSelected(long patNum) {
			ModuleSelected(patNum,false);
		}

		///<summary></summary>
		public void ModuleSelected(long patNum,bool isSelectingFamily) {
			UserOdPref userOdPrefProcBreakdown=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.AcctProcBreakdown).FirstOrDefault();
			if(userOdPrefProcBreakdown==null) {
				checkShowDetail.Checked=true;
			}
			else {
				checkShowDetail.Checked=PIn.Bool(userOdPrefProcBreakdown.ValueString);
			}
			Logger.LogAction("RefreshModuleData",LogPath.AccountModule,() => RefreshModuleData(patNum,isSelectingFamily));
			Logger.LogAction("RefreshModuleScreen",LogPath.AccountModule,() => RefreshModuleScreen(isSelectingFamily));
			PatientDashboardDataEvent.Fire(ODEventType.ModuleSelected,_loadData);
			Plugins.HookAddCode(this,"ContrAccount.ModuleSelected_end",patNum,isSelectingFamily);
		}

		///<summary>Used when jumping to this module and directly to a claim.</summary>
		public void ModuleSelected(long patNum,long claimNum) {
			ModuleSelected(patNum);
			DataTable table=DataSetMain.Tables["account"];
			for(int i=0;i<table.Rows.Count;i++){
				if(table.Rows[i]["ClaimPaymentNum"].ToString()!="0") {//claimpayment
					continue;
				}
				if(table.Rows[i]["ClaimNum"].ToString()=="0") {//not a claim or claimpayment
					continue;
				}
				long claimNumRow=PIn.Long(table.Rows[i]["ClaimNum"].ToString());
				if(claimNumRow!=claimNum){
					continue;
				}
				gridAccount.SetSelected(i,true);
			}
		}

		///<summary></summary>
		public void ModuleUnselected() {
			UpdateUrgFinNote();
			UpdateFinNote();
			FamCur=null;
			RepeatChargeList=null;
			_patNumLast=0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
			Plugins.HookAddCode(this,"ContrAccount.ModuleUnselected_end");
		}

		///<summary></summary>
		private void RefreshModuleData(long patNum,bool isSelectingFamily) {
			UpdateUrgFinNote();
			UpdateFinNote();
			if (patNum == 0){
				PatCur=null;
				FamCur=null;
				DataSetMain=null;
				Plugins.HookAddCode(this,"ContrAccount.RefreshModuleData_null");
				return;
			}
			DateTime fromDate=DateTime.MinValue;
			DateTime toDate=DateTime.MaxValue;
			if(textDateStart.errorProvider1.GetError(textDateStart)==""
				&& textDateEnd.errorProvider1.GetError(textDateEnd)=="") {
				if(textDateStart.Text!="") {
					fromDate=PIn.Date(textDateStart.Text);
				}
				if(textDateEnd.Text!="") {
					toDate=PIn.Date(textDateEnd.Text);
				}
			}
			bool viewingInRecall=ViewingInRecall;
			if(PrefC.GetBool(PrefName.FuchsOptionsOn)) {
				panelTotalOwes.Top=-38;
				viewingInRecall=true;
			}
			bool doMakeSecLog=false;
			if(_patNumLast!=patNum) {
				doMakeSecLog=true;
				_patNumLast=patNum;
			}
			bool doGetAutoOrtho=PrefC.GetBool(PrefName.OrthoEnabled);
			try {
				Logger.LogAction("Patients.GetFamily",LogPath.AccountModule,() => _loadData=AccountModules.GetAll(patNum,viewingInRecall,fromDate,toDate,
					isSelectingFamily,checkShowDetail.Checked,true,true,doMakeSecLog,doGetAutoOrtho));
			}
			catch(ApplicationException ex) {
				if(ex.Message=="Missing codenum") {
					MsgBox.Show(this,$"Missing codenum. Please run database maintenance method {nameof(DatabaseMaintenances.ProcedurelogCodeNumInvalid)}.");
					PatCur=null;
					DataSetMain=null;
					return;
				}
				throw;
			}
			lock(_lockDataSetMain) {
				DataSetMain=_loadData.DataSetMain;
			}
			FamCur=_loadData.Fam;
			PatCur=FamCur.GetPatient(patNum);
			PatientNoteCur=_loadData.PatNote;
			_patFieldList=_loadData.ArrPatFields;
			List<long> listDefNumsAcctHidden=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType)
					.FindAll(x => x.ItemValue!="")
					.Select(x => x.DefNum)
					.ToList();
			_listSplitsHidden=_loadData.ListPrePayments.FindAll(x => x.UnearnedType.In(listDefNumsAcctHidden));
			FillSummary();
			Plugins.HookAddCode(this,"ContrAccount.RefreshModuleData_end",FamCur,PatCur,DataSetMain,PPBalanceTotal,isSelectingFamily);
		}

		///<summary>Returns a deep copy of the corresponding table from the main data set.
		///Utilizes a lock object that is partially implemented in an attempt to fix an error when invoking DataTable.Clone()</summary>
		private DataTable GetTableFromDataSet(string tableName) {
			DataTable table;
			lock(_lockDataSetMain) {
				table=DataSetMain.Tables[tableName].Clone();
				foreach(DataRow row in DataSetMain.Tables[tableName].Rows) {
					table.ImportRow(row);
				}
			}
			return table;
		}

		private void RefreshModuleScreen(bool isSelectingFamily) {
			if(PatCur==null) {
				tabControlAccount.Enabled=false;
				ToolBarMain.Buttons["Payment"].Enabled=false;
				ToolBarMain.Buttons["Adjustment"].Enabled=false;
				ToolBarMain.Buttons["Insurance"].Enabled=false;
				ToolBarMain.Buttons["PayPlan"].Enabled=false;
				ToolBarMain.Buttons["InstallPlan"].Enabled=false;
				if(ToolBarMain.Buttons["QuickProcs"]!=null) {
					ToolBarMain.Buttons["QuickProcs"].Enabled=false;
				}
				if(ToolBarMain.Buttons["RepeatCharge"]!=null) {
					ToolBarMain.Buttons["RepeatCharge"].Enabled=false;
				}
				ToolBarMain.Buttons["Statement"].Enabled=false;
				if(ToolBarMain.Buttons["Questionnaire"]!=null && PrefC.GetBool(PrefName.AccountShowQuestionnaire)) {
					ToolBarMain.Buttons["Questionnaire"].Enabled=false;
				}
				ToolBarMain.Invalidate();
				textUrgFinNote.Enabled=false;
				textFinNote.Enabled=false;
				//butComm.Enabled=false;
				tabControlShow.Enabled=false;
				Plugins.HookAddCode(this,"ContrAccount.RefreshModuleScreen_null");
			}
			else{
				tabControlAccount.Enabled=true;
				ToolBarMain.Buttons["Payment"].Enabled=true;
				ToolBarMain.Buttons["Adjustment"].Enabled=true;
				ToolBarMain.Buttons["Insurance"].Enabled=true;
				ToolBarMain.Buttons["PayPlan"].Enabled=true;
				ToolBarMain.Buttons["InstallPlan"].Enabled=true;
				if(ToolBarMain.Buttons["QuickProcs"]!=null) {
					ToolBarMain.Buttons["QuickProcs"].Enabled=true;
				}
				if(ToolBarMain.Buttons["RepeatCharge"]!=null) {
					ToolBarMain.Buttons["RepeatCharge"].Enabled=true;
				} 
				ToolBarMain.Buttons["Statement"].Enabled=true;
				if(ToolBarMain.Buttons["Questionnaire"]!=null && PrefC.GetBool(PrefName.AccountShowQuestionnaire)) {
					ToolBarMain.Buttons["Questionnaire"].Enabled=true;
				}
				ToolBarMain.Invalidate();
				textUrgFinNote.Enabled=true;
				textFinNote.Enabled=true;
				//butComm.Enabled=true;
				tabControlShow.Enabled=true;
			}
			Logger.LogAction("FillPats",LogPath.AccountModule,() => FillPats(isSelectingFamily));
			Logger.LogAction("FillMisc",LogPath.AccountModule,() => FillMisc());
			Logger.LogAction("FillAging",LogPath.AccountModule,() => FillAging(isSelectingFamily));
			//must be in this order.
			Logger.LogAction("FillRepeatCharges",LogPath.AccountModule,() => FillRepeatCharges());//1
			Logger.LogAction("FillPaymentPlans",LogPath.AccountModule,() => FillPaymentPlans());//2
			Logger.LogAction("FillMain",LogPath.AccountModule,() => FillMain());//3
			if(PrefC.GetBool(PrefName.OrthoEnabled)){
				FillAutoOrtho(false);
			}
			if(OrthoCases.HasOrthoCasesEnabled()) {
				FillOrthoCasesGrid();
			}
			Logger.LogAction("FillPatInfo",LogPath.AccountModule,() => FillPatInfo());
			LayoutPanels();
			if(ViewingInRecall || PrefC.GetBoolSilent(PrefName.FuchsOptionsOn,false)) {
				panelProgNotes.Visible = true;
				Logger.LogAction("FillProgNotes",LogPath.AccountModule,() => FillProgNotes());
				if(PrefC.GetBool(PrefName.FuchsOptionsOn) && PatCur!=null){//show prog note options
					groupBox6.Visible = true;
					groupBox7.Visible = true;
					butShowAll.Visible = true;
					butShowNone.Visible = true;
					//FillInsInfo();
				}
			}
			else{
				panelProgNotes.Visible = false;
				FillComm();
			}
			FillTpUnearned();
			Plugins.HookAddCode(this,"ContrAccount.RefreshModuleScreen_end",FamCur,PatCur,DataSetMain,PPBalanceTotal,isSelectingFamily);
		}

		///<summary>Call this before inserting new repeat charge to update patient.BillingCycleDay if no other repeat charges exist.
		///Changes the patient's BillingCycleDay to today if no other active repeat charges are on the patient's account</summary>
		private void UpdatePatientBillingDay(long patNum) {
			if(RepeatCharges.ActiveRepeatChargeExists(patNum)) {
				return;
			}
			Patient patOld=Patients.GetPat(patNum);
			if(patOld.BillingCycleDay==DateTimeOD.Today.Day) {
				return;
			}
			Patient patNew=patOld.Copy();
			patNew.BillingCycleDay=DateTimeOD.Today.Day;
			Patients.Update(patNew,patOld);
		}

		//private void FillPatientButton() {
		//	Patients.AddPatsToMenu(menuPatient,new EventHandler(menuPatient_Click),PatCur,FamCur);
		//}

		private void FillPats(bool isSelectingFamily) {
			if(PatCur==null) {
				gridAcctPat.BeginUpdate();
				gridAcctPat.ListGridRows.Clear();
				gridAcctPat.EndUpdate();
				return;
			}
			gridAcctPat.BeginUpdate();
			gridAcctPat.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableAccountPat","Patient"),105);
			gridAcctPat.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableAccountPat","Bal"),49,HorizontalAlignment.Right);
			gridAcctPat.ListGridColumns.Add(col);
			gridAcctPat.ListGridRows.Clear();
			GridRow row;
			DataTable table=DataSetMain.Tables["patient"];
			decimal bal=0;
			for(int i=0;i<table.Rows.Count;i++) {
				if(i!=table.Rows.Count-1 && PatientLinks.WasPatientMerged(PIn.Long(table.Rows[i]["PatNum"].ToString()),_loadData.ListMergeLinks) 
					&& FamCur.ListPats[i].PatNum!=PatCur.PatNum && ((decimal)table.Rows[i]["balanceDouble"])==0) 
				{
					//Hide merged patients so that new things don't get added to them. If the user really wants to find this patient, they will have to use 
					//the Select Patient window.
					continue;
				}
				bal+=(decimal)table.Rows[i]["balanceDouble"];
				row = new GridRow();
				row.Cells.Add(GetPatNameFromTable(table,i));
				row.Cells.Add(table.Rows[i]["balance"].ToString());
				row.Tag=PIn.Long(table.Rows[i]["PatNum"].ToString());
				if(i==0 || i==table.Rows.Count-1) {
					row.Bold=true;
				}
				gridAcctPat.ListGridRows.Add(row);
			}
			gridAcctPat.EndUpdate();
			if(isSelectingFamily){
				gridAcctPat.SetSelected(gridAcctPat.ListGridRows.Count-1,true);
			}
			else{
				int index=gridAcctPat.ListGridRows.ToList().FindIndex(x => (long)x.Tag==PatCur.PatNum);
				if(index>=0) {
					//If the index is greater than the number of rows, it will return and not select anything.
					gridAcctPat.SetSelected(index,true);
				}
			}
			if(isSelectingFamily){
				ToolBarMain.Buttons["Insurance"].Enabled=false;
			}
			else{
				ToolBarMain.Buttons["Insurance"].Enabled=true;
			}
		}

		private string GetPatNameFromTable(DataTable table,int index) {
			string name=table.Rows[index]["name"].ToString();
			if(PrefC.GetBool(PrefName.TitleBarShowSpecialty) && string.Compare(name,"Entire Family",true)!=0) {
				long patNum=PIn.Long(table.Rows[index]["PatNum"].ToString());
				string specialty=Patients.GetPatientSpecialtyDef(patNum)?.ItemName??"";
				name+=string.IsNullOrWhiteSpace(specialty)?"":"\r\n"+specialty;
			}
			return name;
		}

		private void FillMisc() {
			//textCC.Text="";
			//textCCexp.Text="";
			if(PatCur==null) {
				textUrgFinNote.Text="";
				textFinNote.Text="";
			}
			else{
				textUrgFinNote.Text=FamCur.ListPats[0].FamFinUrgNote;
				textFinNote.Text=PatientNoteCur.FamFinancial;
				if(!textFinNote.Focused) {
					textFinNote.SelectionStart=textFinNote.Text.Length;
					//This will cause a crash if the richTextBox currently has focus. We don't know why.
					//Only happens if you call this during a Leave event, and only when moving between two ODtextBoxes.
					//Tested with two ordinary richTextBoxes, and the problem does not exist.
					//We may pursue fixing the root problem some day, but this workaround will do for now.
					textFinNote.ScrollToCaret();
				}
				if(!textUrgFinNote.Focused) {
					textUrgFinNote.SelectionStart=0;
					textUrgFinNote.ScrollToCaret();
				}
			}
			UrgFinNoteChanged=false;
			FinNoteChanged=false;
			//CCChanged=false;
			if(ViewingInRecall) {
				textUrgFinNote.ReadOnly=true;
				textFinNote.ReadOnly=true;
			}
			else {
				textUrgFinNote.ReadOnly=false;
				textFinNote.ReadOnly=false;
			}
		}

		private void FillAging(bool isSelectingFamily) {
			if(Plugins.HookMethod(this,"ContrAccount.FillAging",FamCur,PatCur,DataSetMain,isSelectingFamily)) {
				return;
			}
			if(PatCur!=null) {
				textOver90.Text=FamCur.ListPats[0].BalOver90.ToString("F");
				text61_90.Text=FamCur.ListPats[0].Bal_61_90.ToString("F");
				text31_60.Text=FamCur.ListPats[0].Bal_31_60.ToString("F");
				text0_30.Text=FamCur.ListPats[0].Bal_0_30.ToString("F");
				decimal total=(decimal)FamCur.ListPats[0].BalTotal;
				List<long> listDefNumsTpUnearned=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType)
					.FindAll(x => x.ItemValue!="")
					.Select(x => x.DefNum)
					.ToList();
				labelTotalAmt.Text=total.ToString("F");
				labelInsEstAmt.Text=FamCur.ListPats[0].InsEst.ToString("F");
				labelBalanceAmt.Text = (total - (decimal)FamCur.ListPats[0].InsEst).ToString("F");
				labelPatEstBalAmt.Text="";
				DataTable tableMisc=DataSetMain.Tables["misc"];
				if(!isSelectingFamily){
					for(int i=0;i<tableMisc.Rows.Count;i++){
						if(tableMisc.Rows[i]["descript"].ToString()=="patInsEst"){
							decimal estBal=(decimal)PatCur.EstBalance-PIn.Decimal(tableMisc.Rows[i]["value"].ToString());
							labelPatEstBalAmt.Text=estBal.ToString("F");
						}
					}
				}
				labelUnearnedAmt.Text="";
				for(int i=0;i<tableMisc.Rows.Count;i++){
					if(tableMisc.Rows[i]["descript"].ToString()=="unearnedIncome") {
						//remove TP splits that do not show on account due to def being checked. 
						List<PaySplit> listUnearnedShownOnAccount=_loadData.ListPrePayments.FindAll(x => !x.UnearnedType.In(listDefNumsTpUnearned) 
							&& x.PatNum.In(FamCur.ListPats.Select(y => y.PatNum)));//We do not want to show unearned balances for paysplits to other families
						labelUnearnedAmt.Text=PaySplits.GetSumUnearnedForFam(FamCur,listUnearnedShownOnAccount).ToString("F");
						if(PIn.Double(labelUnearnedAmt.Text)<=0) {
							labelUnearnedAmt.ForeColor=Color.Black;
							labelUnearnedAmt.Font=new Font(labelUnearnedAmt.Font,FontStyle.Regular);
						}
						else {
							labelUnearnedAmt.ForeColor=Color.Firebrick;
							labelUnearnedAmt.Font=new Font(labelUnearnedAmt.Font,FontStyle.Bold);
						}
					}
				}
				//labelInsLeft.Text=Lan.g(this,"Ins Left");
				//labelInsLeftAmt.Text="";//etc. Will be same for everyone
				Font fontBold=new Font(FontFamily.GenericSansSerif,11,FontStyle.Bold);
				//In the new way of doing it, they are all visible and calculated identically,
				//but the emphasis simply changes by slight renaming of labels
				//and by font size changes.
				if(PrefC.GetBool(PrefName.BalancesDontSubtractIns)){
					labelTotal.Text=Lan.g(this,"Balance");
					labelTotalAmt.Font=fontBold;
					labelTotalAmt.ForeColor=Color.Firebrick;
					panelAgeLine.Visible=true;//verical line
					labelInsEst.Text=Lan.g(this,"Ins Pending");
					labelBalance.Text=Lan.g(this,"After Ins");
					labelBalanceAmt.Font=this.Font;
					labelBalanceAmt.ForeColor=Color.Black;
				}
				else{//this is more common
					labelTotal.Text=Lan.g(this,"Total");
					labelTotalAmt.Font=this.Font;
					labelTotalAmt.ForeColor = Color.Black;
					panelAgeLine.Visible=false;
					labelInsEst.Text=Lan.g(this,"-InsEst");
					labelBalance.Text=Lan.g(this,"=Est Bal");
					labelBalanceAmt.Font=fontBold;
					labelBalanceAmt.ForeColor=Color.Firebrick;
					if(PrefC.GetBool(PrefName.FuchsOptionsOn)){
						labelTotal.Text=Lan.g(this,"Balance");
						labelBalance.Text=Lan.g(this,"=Owed Now");
						labelTotalAmt.Font = fontBold;
					}
				}
			}
			else {
				textOver90.Text="";
				text61_90.Text="";
				text31_60.Text="";
				text0_30.Text="";
				labelTotalAmt.Text="";
				labelInsEstAmt.Text="";
				labelBalanceAmt.Text="";
				labelPatEstBalAmt.Text="";
				labelUnearnedAmt.Text="";
				//labelInsLeftAmt.Text="";
			}
		}

		///<summary></summary>
		private void FillRepeatCharges() {
			//Uncollapse the first panel just in case. If this is left collapsed, setting visible properties on controls within it will have no effect
			splitContainerParent.Panel1Collapsed=false;
			gridRepeat.Visible=false;
			splitContainerRepChargesPP.Panel1Collapsed=true;
			if(PatCur==null) {
				return;
			}
			RepeatChargeList=_loadData.ArrRepeatCharges;
			if(RepeatChargeList.Length==0) {
				return;
			}
			if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				gridRepeat.Title=Lan.g(gridRepeat,"Repeat Charges")+" - Billing Day "+PatCur.BillingCycleDay;
			}
			else {
				gridRepeat.Title=Lan.g(gridRepeat,"Repeat Charges");
			}
			splitContainerRepChargesPP.Panel1Collapsed=false;
			gridRepeat.Visible=true;
			gridRepeat.BeginUpdate();
			gridRepeat.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableRepeatCharges","Description"),150);
			gridRepeat.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableRepeatCharges","Amount"),60,HorizontalAlignment.Right);
			gridRepeat.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableRepeatCharges","Start Date"),70,HorizontalAlignment.Center);
			gridRepeat.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableRepeatCharges","Stop Date"),70,HorizontalAlignment.Center);
			gridRepeat.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableRepeatCharges","Enabled"),55,HorizontalAlignment.Center);
			gridRepeat.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableRepeatCharges","Note"),355);
			gridRepeat.ListGridColumns.Add(col);
			gridRepeat.ListGridRows.Clear();
			UI.GridRow row;
			ProcedureCode procCode;
			for(int i=0;i<RepeatChargeList.Length;i++) {
				row=new GridRow();
				procCode=ProcedureCodes.GetProcCode(RepeatChargeList[i].ProcCode);
				row.Cells.Add(procCode.Descript);
				row.Cells.Add(RepeatChargeList[i].ChargeAmt.ToString("F"));
				if(RepeatChargeList[i].DateStart.Year>1880) {
					row.Cells.Add(RepeatChargeList[i].DateStart.ToShortDateString());
				}
				else {
					row.Cells.Add("");
				}
				if(RepeatChargeList[i].DateStop.Year>1880) {
					row.Cells.Add(RepeatChargeList[i].DateStop.ToShortDateString());
				}
				else {
					row.Cells.Add("");
				}
				if(RepeatChargeList[i].IsEnabled) {
					row.Cells.Add("X");
				}
				else {
					row.Cells.Add("");
				}
				string note="";
				if(!string.IsNullOrEmpty(RepeatChargeList[i].Npi)) {
					note+="NPI="+RepeatChargeList[i].Npi+" ";
				}
				if(!string.IsNullOrEmpty(RepeatChargeList[i].ErxAccountId)) {
					note+="ErxAccountId="+RepeatChargeList[i].ErxAccountId+" ";
				}
				if(!string.IsNullOrEmpty(RepeatChargeList[i].ProviderName)) {
					note+=RepeatChargeList[i].ProviderName+" ";
				}
				note+=RepeatChargeList[i].Note;
				row.Cells.Add(note);
				gridRepeat.ListGridRows.Add(row);
			}
			gridRepeat.EndUpdate();
		}

		///<summary>Show the splits that are flagged as being hidden. </summary>
		private void FillTpUnearned() {
			if(PatCur==null) {
				return;
			}
			if(_listSplitsHidden.Count==0) {
				return;
			}
			List<Procedure> listProceduresForHiddenSplits=Procedures.GetManyProc(_listSplitsHidden.Select(x => x.ProcNum).ToList(),false);
			gridTpSplits.BeginUpdate();
			gridTpSplits.ListGridColumns.Clear();
			GridColumn col=new GridColumn("Date",65);
			gridTpSplits.ListGridColumns.Add(col);
			col=new GridColumn("Patient",150);
			gridTpSplits.ListGridColumns.Add(col);
			col=new GridColumn("Provider",70);
			gridTpSplits.ListGridColumns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new GridColumn("Clinic",60);
				gridTpSplits.ListGridColumns.Add(col);
			}
			col=new GridColumn("Code",80);
			gridTpSplits.ListGridColumns.Add(col);
			col=new GridColumn("Description",180);
			gridTpSplits.ListGridColumns.Add(col);
			col=new GridColumn("Amount",60);
			gridTpSplits.ListGridColumns.Add(col);
			gridTpSplits.ListGridRows.Clear();
			Dictionary<long,Patient> dictPats=new Dictionary<long, Patient>();
			foreach(PaySplit tpSplit in _listSplitsHidden) {
				GridRow row=new GridRow();
				row.Cells.Add(tpSplit.DatePay.ToShortDateString());//Date
				if(!dictPats.ContainsKey(tpSplit.PatNum)) {
					Patient patFromFam=_loadData.Fam.ListPats.FirstOrDefault(x => x.PatNum==tpSplit.PatNum);
					if(patFromFam!=null) {
						dictPats.Add(tpSplit.PatNum,patFromFam);
					}
					else {
						dictPats.Add(tpSplit.PatNum,Patients.GetPat(tpSplit.PatNum));
					}
				}
				Patient patForSplit=dictPats[tpSplit.PatNum];
				row.Cells.Add(patForSplit.LName+", "+patForSplit.FName);//Patient
				row.Cells.Add(Providers.GetAbbr(tpSplit.ProvNum));//Provider
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(tpSplit.ClinicNum));//Clinics
				}
				long codeNum=listProceduresForHiddenSplits.FirstOrDefault(x => x.ProcNum==tpSplit.ProcNum)?.CodeNum??0;
				ProcedureCode procCode=ProcedureCodes.GetFirstOrDefault(x => x.CodeNum==codeNum);				
				if(procCode!=null) {
					row.Cells.Add(procCode.ProcCode);//Code
					row.Cells.Add(procCode.Descript);//Description
				}
				else {
					row.Cells.Add("");//Code
					row.Cells.Add("");//Description
				}
				row.Cells.Add(tpSplit.SplitAmt.ToString("F"));//Amount
				row.Tag=tpSplit;
				Color defColor=Defs.GetDefsForCategory(DefCat.AccountColors)[3].ItemColor;
				row.ColorLborder=defColor;
				row.ColorText=defColor;
				gridTpSplits.ListGridRows.Add(row);
			}
			gridTpSplits.EndUpdate();
		}

		private void FillPaymentPlans(){
			PPBalanceTotal=0;
			//Uncollapse the first panel just in case. If this is left collapsed, setting visible properties on controls within it will have no effect
			splitContainerParent.Panel1Collapsed=false;
			gridPayPlan.Visible=false;
			splitContainerRepChargesPP.Panel2Collapsed=true;
			if(PatCur==null) {
				return;
			}
			DataTable table=DataSetMain.Tables["payplan"];
			if(table.Rows.OfType<DataRow>().Count(x => PIn.Long(x["Guarantor"].ToString())==PatCur.PatNum 
				|| PIn.Long(x["PatNum"].ToString())==PatCur.PatNum)==0 && !_isSelectingFamily) //if we are looking at the entire family, show all the payplans 
			{
				return;
			}
			List<long> listPayPlanNums=table.Select().Select(x => PIn.Long(x["PayPlanNum"].ToString())).ToList();
			List<PayPlan> listOverchargedPayPlans=PayPlans.GetOverchargedPayPlans(listPayPlanNums);
			//do not hide payment plans that still have a balance when not on v2
			if(!checkShowCompletePayPlans.Checked) { //Hide the payment plans grid if there are no payment plans currently visible.
				bool existsOpenPayPlan=false;
				for(int i = 0;i<table.Rows.Count;i++) { //for every payment plan
					if(DoShowPayPlan(checkShowCompletePayPlans.Checked,PIn.Bool(table.Rows[i]["IsClosed"].ToString()),
						PIn.Double(table.Rows[i]["balance"].ToString())))
					{						
						existsOpenPayPlan=true;
						break; //break
					}
				}
				if(!existsOpenPayPlan) {
					return;//no need to do anything else.
				}
			}
			splitContainerRepChargesPP.Panel2Collapsed=false;
			gridPayPlan.Visible=true;
			gridPayPlan.BeginUpdate();
			gridPayPlan.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TablePaymentPlans","Date"),65);
			gridPayPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlans","Guarantor"),100);
			gridPayPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlans","Patient"),100);
			gridPayPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlans","Type"),30,HorizontalAlignment.Center);
			gridPayPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlans","Category"),60,HorizontalAlignment.Center);
			gridPayPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlans","Principal"),60,HorizontalAlignment.Right);
			gridPayPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlans","Total Cost"),60,HorizontalAlignment.Right);
			gridPayPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlans","Paid"),60,HorizontalAlignment.Right);
			gridPayPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlans","PrincPaid"),60,HorizontalAlignment.Right);
			gridPayPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlans","Balance"),60,HorizontalAlignment.Right);
			gridPayPlan.ListGridColumns.Add(col);
			if(PrefC.GetBool(PrefName.PayPlanHideDueNow)) {
				col=new GridColumn("Closed",60,HorizontalAlignment.Center);
			}
			else {
				col=new GridColumn(Lan.g("TablePaymentPlans","Due Now"),60,HorizontalAlignment.Right);
			}
			gridPayPlan.ListGridColumns.Add(col);
			gridPayPlan.ListGridRows.Clear();
			UI.GridRow row;
			UI.GridCell cell;
			for(int i=0;i<table.Rows.Count;i++) {
				if(!DoShowPayPlan(checkShowCompletePayPlans.Checked,PIn.Bool(table.Rows[i]["IsClosed"].ToString()),
					PIn.Double(table.Rows[i]["balance"].ToString())))
				{
					continue;//hide
				}
				row=new GridRow();
				row.Cells.Add(table.Rows[i]["date"].ToString());
				if(table.Rows[i]["InstallmentPlanNum"].ToString()!="0" && table.Rows[i]["PatNum"].ToString()!=PatCur.Guarantor.ToString()) {//Installment plan and not on guar
					cell=new GridCell(((string)"Invalid Guarantor"));
					cell.Bold=YN.Yes;
					cell.ColorText=Color.Red;
				}
				else {
					cell=new GridCell(table.Rows[i]["guarantor"].ToString());
				}
				row.Cells.Add(cell);
				row.Cells.Add(table.Rows[i]["patient"].ToString());
				row.Cells.Add(table.Rows[i]["type"].ToString());
				long planCat=PIn.Long(table.Rows[i]["PlanCategory"].ToString());
				if(planCat==0) {
					row.Cells.Add(Lan.g(this,"None"));
				}
				else {
					row.Cells.Add(Defs.GetDef(DefCat.PayPlanCategories,planCat).ItemName);
				}
				row.Cells.Add(table.Rows[i]["principal"].ToString());
				row.Cells.Add(table.Rows[i]["totalCost"].ToString());
				row.Cells.Add(table.Rows[i]["paid"].ToString());
				row.Cells.Add(table.Rows[i]["princPaid"].ToString());
				row.Cells.Add(table.Rows[i]["balance"].ToString());
				if(table.Rows[i]["IsClosed"].ToString()=="1" && PrefC.GetInt(PrefName.PayPlansVersion)==2) {
					cell=new GridCell(Lan.g(this,"Closed"));
					row.ColorText=Color.Gray;
				}
				else if(PrefC.GetBool(PrefName.PayPlanHideDueNow)) {//pref can only be enabled when PayPlansVersion == 2.
					cell=new GridCell("");
				}
				else { //they aren't hiding the "Due Now" cell text.
					cell=new GridCell(table.Rows[i]["due"].ToString());
					//Only color the due now red and bold in version 1 and 3 of payplans.
					if(PrefC.GetInt(PrefName.PayPlansVersion).In((int)PayPlanVersions.DoNotAge,(int)PayPlanVersions.AgeCreditsOnly,(int)PayPlanVersions.NoCharges)) 
					{
						if(table.Rows[i]["type"].ToString()!="Ins") {
							cell.Bold=YN.Yes;
							cell.ColorText=Color.Red;
						}
					}
				}
				row.Cells.Add(cell);
				row.Tag=table.Rows[i];
				foreach(PayPlan payPlan in listOverchargedPayPlans){
					if(listOverchargedPayPlans.Select(x => x.PayPlanNum).ToList().Contains(PIn.Long(table.Rows[i]["PayPlanNum"].ToString()))) {
						row.ColorBackG=Color.FromArgb(255,255,128);
					}
				}
				gridPayPlan.ListGridRows.Add(row);
				PPBalanceTotal += (Convert.ToDecimal(PIn.Double(table.Rows[i]["balance"].ToString())));
			}
			gridPayPlan.EndUpdate();
			if(PrefC.GetBool(PrefName.FuchsOptionsOn)) {
				panelTotalOwes.Top=1;
				labelTotalPtOwes.Text=(PPBalanceTotal + (decimal)FamCur.ListPats[0].BalTotal - (decimal)FamCur.ListPats[0].InsEst).ToString("F");
			}				
		}

		///<summary>Returns true if the payment plan should be displayed.</summary>
		private bool DoShowPayPlan(bool doShowCompletedPlans,bool isClosed,double balance) {	
			if(doShowCompletedPlans) {
				return true;
			}		
			//do not hide payment plans that still have a balance when not on v2
			bool doShowClosedPlansWithBalance=(PrefC.GetInt(PrefName.PayPlansVersion)!=(int)PayPlanVersions.AgeCreditsAndDebits);
			return !isClosed
						|| (doShowClosedPlansWithBalance && !balance.IsEqual(0)); //Or the payment plan has a balance
		}

		/// <summary>Fills the commlog grid on this form.  It does not refresh the data from the database.</summary>
		private void FillComm() {
			if (DataSetMain == null) {
				gridComm.BeginUpdate();
				gridComm.ListGridRows.Clear();
				gridComm.EndUpdate();
				return;
			}
			gridComm.BeginUpdate();
			gridComm.ListGridColumns.Clear();
			GridColumn col = new GridColumn(Lan.g("TableCommLogAccount", "Date"), 70);
			gridComm.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableCommLogAccount","Time"),42);//,HorizontalAlignment.Right);
			gridComm.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableCommLogAccount","Name"),80);
			gridComm.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableCommLogAccount", "Type"), 80);
			gridComm.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableCommLogAccount", "Mode"), 55);
			gridComm.ListGridColumns.Add(col);
			//col = new ODGridColumn(Lan.g("TableCommLogAccount", "Sent/Recd"), 75);
			//gridComm.Columns.Add(col);
			col = new GridColumn(Lan.g("TableCommLogAccount", "Note"), 455);
			gridComm.ListGridColumns.Add(col);
			gridComm.ListGridRows.Clear();
			OpenDental.UI.GridRow row;
			DataTable table = DataSetMain.Tables["Commlog"];
			for(int i=0;i<table.Rows.Count;i++) {
				//Skip commlog entries which belong to other family members per user option.
				if(!this.checkShowFamilyComm.Checked										//show family not checked
					&& !_isSelectingFamily																	//family not selected
					&& table.Rows[i]["PatNum"].ToString()!=PatCur.PatNum.ToString()	//not this patient
					&& table.Rows[i]["FormPatNum"].ToString()=="0")				//not a questionnaire (FormPat)
				{
					continue;
				}
				else if(table.Rows[i]["EmailMessageNum"].ToString() != "0") {//if this is an Email
					if(((HideInFlags)PIn.Int(table.Rows[i]["EmailMessageHideIn"].ToString())).HasFlag(HideInFlags.AccountCommLog)) {
						continue;
					}
				}
				row=new GridRow();
				int argbColor=PIn.Int(table.Rows[i]["colorText"].ToString());//Convert to int. If blank or 0, will use default color.
				if(argbColor!=Color.Empty.ToArgb()) {//A color was set for this commlog type
					row.ColorText=Color.FromArgb(argbColor);
				}
				row.Cells.Add(table.Rows[i]["commDate"].ToString());
				row.Cells.Add(table.Rows[i]["commTime"].ToString());
				if(_isSelectingFamily) {
					row.Cells.Add(table.Rows[i]["patName"].ToString());
				}
				else {//one patient
					if(table.Rows[i]["PatNum"].ToString()==PatCur.PatNum.ToString()) {//if this patient
						row.Cells.Add("");
					}
					else {//other patient
						row.Cells.Add(table.Rows[i]["patName"].ToString());
					}
				}
				row.Cells.Add(table.Rows[i]["commType"].ToString());
				row.Cells.Add(table.Rows[i]["mode"].ToString());
				//row.Cells.Add(table.Rows[i]["sentOrReceived"].ToString());
				row.Cells.Add(table.Rows[i]["Note"].ToString());
				row.Tag=i;
				gridComm.ListGridRows.Add(row);
			}
			gridComm.EndUpdate();
			gridComm.ScrollToEnd();
		}

		private void FillMain(){
			gridAccount.BeginUpdate();
			gridAccount.ListGridColumns.Clear();
			GridColumn col;
			fieldsForMainGrid=DisplayFields.GetForCategory(DisplayFieldCategory.AccountModule);
			if(!PrefC.HasClinicsEnabled) {
				//remove clinics from displayfields if clinics are disabled
				fieldsForMainGrid.RemoveAll(x => x.InternalName.ToLower().Contains("clinic"));
			}
			HorizontalAlignment align;
			for(int i=0;i<fieldsForMainGrid.Count;i++) {
				align=HorizontalAlignment.Left;
				if(fieldsForMainGrid[i].InternalName=="Charges"
					|| fieldsForMainGrid[i].InternalName=="Credits"
					|| fieldsForMainGrid[i].InternalName=="Balance") 
				{
					align=HorizontalAlignment.Right;
				}
				if(fieldsForMainGrid[i].InternalName=="Signed") {
					align=HorizontalAlignment.Center;
				}
				if(fieldsForMainGrid[i].Description=="") {
					col=new GridColumn(fieldsForMainGrid[i].InternalName,fieldsForMainGrid[i].ColumnWidth,align);
				}
				else {
					col=new GridColumn(fieldsForMainGrid[i].Description,fieldsForMainGrid[i].ColumnWidth,align);
				}
				gridAccount.ListGridColumns.Add(col);
			}
			if(gridAccount.ListGridColumns.Sum(x => x.ColWidth) > gridAccount.Width) {
				gridAccount.HScrollVisible=true;
			}
			else {
			}
			gridAccount.ListGridRows.Clear();
			GridRow row;
			DataTable table=null;
			if(PatCur==null){
				table=new DataTable();
			}
			else{
				table=DataSetMain.Tables["account"];
			}
			for(int i=0;i<table.Rows.Count;i++) {
				row=new GridRow();
				for(int f=0;f<fieldsForMainGrid.Count;f++) {
					switch(fieldsForMainGrid[f].InternalName) {
						case "Date":
							row.Cells.Add(table.Rows[i]["date"].ToString());
							break;
						case "Patient":
							row.Cells.Add(table.Rows[i]["patient"].ToString());
							break;
						case "Prov":
							row.Cells.Add(table.Rows[i]["prov"].ToString());
							break;
						case "Clinic":
							row.Cells.Add(Clinics.GetAbbr(PIn.Long(table.Rows[i]["ClinicNum"].ToString())));
							break;
						case "ClinicDesc":
							row.Cells.Add(Clinics.GetDesc(PIn.Long(table.Rows[i]["ClinicNum"].ToString())));
							break;
						case "Code":
							row.Cells.Add(table.Rows[i]["ProcCode"].ToString());
							break;
						case "Tth":
							row.Cells.Add(table.Rows[i]["tth"].ToString());
							break;
						case "Description":
							row.Cells.Add(table.Rows[i]["description"].ToString());
							break;
						case "Charges":
							row.Cells.Add(table.Rows[i]["charges"].ToString());
							break;
						case "Credits":
							row.Cells.Add(table.Rows[i]["credits"].ToString());
							break;
						case "Balance":
							row.Cells.Add(table.Rows[i]["balance"].ToString());
							break;
						case "Signed":
							row.Cells.Add(table.Rows[i]["signed"].ToString());
							break;
						case "Abbr": //procedure abbreviation
							if(!String.IsNullOrEmpty(table.Rows[i]["AbbrDesc"].ToString())) {
								row.Cells.Add(table.Rows[i]["AbbrDesc"].ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						default:
							row.Cells.Add("");
							break;
					}
				}
				row.ColorText=Color.FromArgb(PIn.Int(table.Rows[i]["colorText"].ToString()));
				if(i==table.Rows.Count-1//last row
					|| (DateTime)table.Rows[i]["DateTime"]!=(DateTime)table.Rows[i+1]["DateTime"])
				{
					row.ColorLborder=Color.Black;
				}
				gridAccount.ListGridRows.Add(row);
			}
			gridAccount.EndUpdate();
			if(Actscrollval==0) {
				gridAccount.ScrollToEnd();
			}
			else {
				gridAccount.ScrollValue=Actscrollval;
				Actscrollval=0;
			}
		}

		private void FillSummary() {
			textFamPriMax.Text="";
			textFamPriDed.Text="";
			textFamSecMax.Text="";
			textFamSecDed.Text="";
			textPriMax.Text="";
			textPriDed.Text="";
			textPriDedRem.Text="";
			textPriUsed.Text="";
			textPriPend.Text="";
			textPriRem.Text="";
			textSecMax.Text="";
			textSecDed.Text="";
			textSecDedRem.Text="";
			textSecUsed.Text="";
			textSecPend.Text="";
			textSecRem.Text="";
			if(PatCur==null) {
				return;
			}
			double maxFam=0;
			double maxInd=0;
			double ded=0;
			double dedFam=0;
			double dedRem=0;
			double remain=0;
			double pend=0;
			double used=0;
			InsPlan PlanCur;
			InsSub SubCur;
			List<InsSub> subList=_loadData.ListInsSubs;
			List<InsPlan> InsPlanList=_loadData.ListInsPlans;
			List<PatPlan> PatPlanList=_loadData.ListPatPlans;
			List<Benefit> BenefitList=_loadData.ListBenefits;
			List<Claim> ClaimList=_loadData.ListClaims;
			List<ClaimProcHist> HistList=_loadData.HistList;
			if(PatPlanList.Count>0) {
				SubCur=InsSubs.GetSub(PatPlanList[0].InsSubNum,subList);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,InsPlanList);
				pend=InsPlans.GetPendingDisplay(HistList,DateTime.Today,PlanCur,PatPlanList[0].PatPlanNum,-1,PatCur.PatNum,PatPlanList[0].InsSubNum,BenefitList);
				used=InsPlans.GetInsUsedDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,-1,InsPlanList,BenefitList,PatCur.PatNum,PatPlanList[0].InsSubNum);
				textPriPend.Text=pend.ToString("F");
				textPriUsed.Text=used.ToString("F");
				maxFam=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,false);
				if(maxFam==-1) {
					textFamPriMax.Text="";
				}
				else {
					textFamPriMax.Text=maxFam.ToString("F");
				}
				if(maxInd==-1) {//if annual max is blank
					textPriMax.Text="";
					textPriRem.Text="";
				}
				else {
					remain=maxInd-used-pend;
					if(remain<0) {
						remain=0;
					}
					//textFamPriMax.Text=max.ToString("F");
					textPriMax.Text=maxInd.ToString("F");
					textPriRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1) {
					textPriDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,-1,InsPlanList,PatCur.PatNum,ded,dedFam);
					textPriDedRem.Text=dedRem.ToString("F");
				}
				if(dedFam!=-1) {
					textFamPriDed.Text=dedFam.ToString("F");
				}
			}
			if(PatPlanList.Count>1) {
				SubCur=InsSubs.GetSub(PatPlanList[1].InsSubNum,subList);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,InsPlanList);
				pend=InsPlans.GetPendingDisplay(HistList,DateTime.Today,PlanCur,PatPlanList[1].PatPlanNum,-1,PatCur.PatNum,PatPlanList[1].InsSubNum,BenefitList);
				textSecPend.Text=pend.ToString("F");
				used=InsPlans.GetInsUsedDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,-1,InsPlanList,BenefitList,PatCur.PatNum,PatPlanList[1].InsSubNum);
				textSecUsed.Text=used.ToString("F");
				//max=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum);
				maxFam=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,false);
				if(maxFam==-1) {
					textFamSecMax.Text="";
				}
				else {
					textFamSecMax.Text=maxFam.ToString("F");
				}
				if(maxInd==-1) {//if annual max is blank
					textSecMax.Text="";
					textSecRem.Text="";
				}
				else {
					remain=maxInd-used-pend;
					if(remain<0) {
						remain=0;
					}
					//textFamSecMax.Text=max.ToString("F");
					textSecMax.Text=maxInd.ToString("F");
					textSecRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1) {
					textSecDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,-1,InsPlanList,PatCur.PatNum,ded,dedFam);
					textSecDedRem.Text=dedRem.ToString("F");
				}
				if(dedFam!=-1) {
					textFamSecDed.Text=dedFam.ToString("F");
				}
			}
		}

		private void FillPatInfo() {
			if(PatCur==null) {
				gridPatInfo.BeginUpdate();
				gridPatInfo.ListGridRows.Clear();
				gridPatInfo.ListGridColumns.Clear();
				gridPatInfo.EndUpdate();
				return;
			}
			gridPatInfo.BeginUpdate();
			gridPatInfo.ListGridColumns.Clear();
			GridColumn col=new GridColumn("",80);
			gridPatInfo.ListGridColumns.Add(col);
			col=new GridColumn("",150);
			gridPatInfo.ListGridColumns.Add(col);
			gridPatInfo.ListGridRows.Clear();
			GridRow row;
			_patInfoDisplayFields=DisplayFields.GetForCategory(DisplayFieldCategory.AccountPatientInformation);
			for(int f=0;f<_patInfoDisplayFields.Count;f++) {
				row=new GridRow();
				if(_patInfoDisplayFields[f].Description=="") {
					if(_patInfoDisplayFields[f].InternalName=="PatFields") {
						//don't add a cell
					}
					else {
						row.Cells.Add(_patInfoDisplayFields[f].InternalName);
					}
				}
				else {
					if(_patInfoDisplayFields[f].InternalName=="PatFields") {
						//don't add a cell
					}
					else {
						row.Cells.Add(_patInfoDisplayFields[f].Description);
					}
				}
				switch(_patInfoDisplayFields[f].InternalName) {
					case "Billing Type":
						row.Cells.Add(Defs.GetName(DefCat.BillingTypes,PatCur.BillingType));
						break;
					case "PatFields":
						PatFieldL.AddPatFieldsToGrid(gridPatInfo,_patFieldList.ToList(),FieldLocations.Account,_loadData.ListFieldDefLinksAcct);
						break;
				}
				if(_patInfoDisplayFields[f].InternalName=="PatFields") {
					//don't add the row here
				}
				else {
					gridPatInfo.ListGridRows.Add(row);
				}
			}
			gridPatInfo.EndUpdate();
		}

		#region AutoOrtho
		private void FillAutoOrtho(bool doCalculateFirstDate=true) {
			if(PatCur==null) {
				return;
			}
			gridAutoOrtho.BeginUpdate();
			gridAutoOrtho.ListGridColumns.Clear();
			gridAutoOrtho.ListGridColumns.Add(new GridColumn("",(gridAutoOrtho.Width/2)-20));//,HorizontalAlignment.Right));
			gridAutoOrtho.ListGridColumns.Add(new GridColumn("",(gridAutoOrtho.Width/2)+20));
			gridAutoOrtho.ListGridRows.Clear();
			GridRow row = new GridRow();
			//Insurance Information
			//PriClaimType
			List<PatPlan> listPatPlans = _loadData.ListPatPlans;
			if(listPatPlans.Count == 0) {
				row = new GridRow();
				row.Cells.Add("");
				row.Cells.Add(Lan.g(this,"Patient has no insurance."));
				gridAutoOrtho.ListGridRows.Add(row);
			}
			else {
				List<Def> listDefs=Defs.GetDefsForCategory(DefCat.MiscColors);
				for(int i = 0;i < listPatPlans.Count;i++) {
					PatPlan patPlanCur = listPatPlans[i];
					InsSub insSub = InsSubs.GetSub(patPlanCur.InsSubNum,_loadData.ListInsSubs);
					InsPlan insPlanCur = InsPlans.GetPlan(insSub.PlanNum,_loadData.ListInsPlans);
					string carrierNameCur = Carriers.GetCarrier(insPlanCur.CarrierNum).CarrierName;
					string subIDCur = insSub.SubscriberID;
					row = new GridRow();
					AutoOrthoPat orthoPatCur=new AutoOrthoPat() {
						InsPlan=insPlanCur,
						PatPlan=patPlanCur,
						CarrierName=carrierNameCur,
						DefaultFee=insPlanCur.OrthoAutoFeeBilled,
						SubID=subIDCur
					};
					if(i==listPatPlans.Count-1) { //last row in the insurance info section
						row.ColorLborder=Color.Black;
					}
					row.ColorBackG=listDefs[0].ItemColor; //same logic as family module insurance colors.
					switch(i) {
						case 0: //primary
							row.Cells.Add(Lan.g(this,"Primary Ins"));
							break;
						case 1: //secondary
							row.Cells.Add(Lan.g(this,"Secondary Ins"));
							break;
						case 2: //tertiary
							row.Cells.Add(Lan.g(this,"Tertiary Ins"));
							break;
						default: //other
							row.Cells.Add(Lan.g(this,"Other Ins"));
							break;
					}
					row.Cells.Add("");
					row.Bold=true;
					row.Tag=orthoPatCur;
					gridAutoOrtho.ListGridRows.Add(row);
					//claimtype
					row=new GridRow();
					row.Cells.Add(Lan.g(this,"ClaimType"));
					if(insPlanCur==null) {
						row.Cells.Add("");
					}
					else {
						row.Cells.Add(insPlanCur.OrthoType.ToString());
					}
					row.Tag=orthoPatCur;
					gridAutoOrtho.ListGridRows.Add(row);
					//Only show for initialPlusPeriodic claimtype.
					if(insPlanCur.OrthoType == OrthoClaimType.InitialPlusPeriodic) {
						//Frequency
						row= new GridRow();
						row.Cells.Add(Lan.g(this,"Frequency"));
						row.Cells.Add(insPlanCur.OrthoAutoProcFreq.ToString());
						row.Tag=orthoPatCur;
						gridAutoOrtho.ListGridRows.Add(row);
						//Fee
						row= new GridRow();
						row.Cells.Add(Lan.g(this,"FeeBilled"));
						row.Cells.Add(patPlanCur.OrthoAutoFeeBilledOverride==-1 ? POut.Double(insPlanCur.OrthoAutoFeeBilled) : POut.Double(patPlanCur.OrthoAutoFeeBilledOverride));
						row.Tag=orthoPatCur;
						gridAutoOrtho.ListGridRows.Add(row);
					}
					//Last Claim Date
					row= new GridRow();
					DateTime dateLast;
					if(!_loadData.DictDateLastOrthoClaims.TryGetValue(patPlanCur.PatPlanNum,out dateLast)) {
						dateLast=Claims.GetDateLastOrthoClaim(patPlanCur,insPlanCur.OrthoType);
					}
					row.Cells.Add(Lan.g(this,"LastClaim"));
					row.Cells.Add(dateLast==null || dateLast.Date == DateTime.MinValue.Date ? Lan.g(this,"None Sent") : dateLast.ToShortDateString());
					row.Tag=orthoPatCur;
					gridAutoOrtho.ListGridRows.Add(row);
					//NextClaimDate - Only show for initialPlusPeriodic claimtype.
					if(insPlanCur.OrthoType == OrthoClaimType.InitialPlusPeriodic) {
						row= new GridRow();
						row.Cells.Add(Lan.g(this,"NextClaim"));
						row.Cells.Add(patPlanCur.OrthoAutoNextClaimDate.Date == DateTime.MinValue.Date ? Lan.g(this,"Stopped") : patPlanCur.OrthoAutoNextClaimDate.ToShortDateString());
						row.Tag=orthoPatCur;
						gridAutoOrtho.ListGridRows.Add(row);
					}
				}
			}
			//Pat Ortho Info Title
			row= new GridRow();
			row.Cells.Add(Lan.g(this,"Pat Ortho Info"));
			row.Cells.Add("");
			row.ColorBackG=Color.LightCyan;
			row.Bold=true;
			row.ColorLborder=Color.Black;
			gridAutoOrtho.ListGridRows.Add(row);
			//OrthoAutoProc Freq
			if(doCalculateFirstDate) {
				_loadData.FirstOrthoProcDate=Procedures.GetFirstOrthoProcDate(PatientNoteCur);
			}
			DateTime firstOrthoProcDate=_loadData.FirstOrthoProcDate;
			if(firstOrthoProcDate!=DateTime.MinValue) {
				row=new GridRow();
				row.Cells.Add(Lan.g(this,"Total Tx Time")); //Number of Years/Months/Days since the first ortho procedure on this account
				DateSpan dateSpan=new DateSpan(firstOrthoProcDate,DateTimeOD.Today);
				string strDateDiff="";
				if(dateSpan.YearsDiff!=0) {
					strDateDiff+=dateSpan.YearsDiff+" "+Lan.g(this,"year"+(dateSpan.YearsDiff==1 ? "" : "s"));
				}
				if(dateSpan.MonthsDiff!=0) {
					if(strDateDiff!="") {
						strDateDiff+=", ";
					}
					strDateDiff+=dateSpan.MonthsDiff+" "+Lan.g(this,"month"+(dateSpan.MonthsDiff==1 ? "" : "s"));
				}
				if(dateSpan.DaysDiff!=0 || strDateDiff=="") {
					if(strDateDiff!="") {
						strDateDiff+=", ";
					}
					strDateDiff+=dateSpan.DaysDiff+" "+Lan.g(this,"day"+(dateSpan.DaysDiff==1 ? "" : "s"));
				}
				row.Cells.Add(strDateDiff);
				gridAutoOrtho.ListGridRows.Add(row);
				//Date Start
				row = new GridRow();
				row.Cells.Add(Lan.g(this,"Date Start")); //Date of the first ortho procedure on this account
				row.Cells.Add(firstOrthoProcDate.ToShortDateString());
				gridAutoOrtho.ListGridRows.Add(row);
				//Tx Months Total
				row = new GridRow();
				row.Cells.Add(Lan.g(this,"Tx Months Total")); //this patient's OrthoClaimMonthsTreatment, or the practice default if 0.
				int txMonthsTotal=(PatientNoteCur.OrthoMonthsTreatOverride==-1?PrefC.GetByte(PrefName.OrthoDefaultMonthsTreat):PatientNoteCur.OrthoMonthsTreatOverride);
				row.Cells.Add(txMonthsTotal.ToString());
				gridAutoOrtho.ListGridRows.Add(row);
				//Months in treatment
				row = new GridRow();
				int txTimeInMonths=(dateSpan.YearsDiff * 12) + dateSpan.MonthsDiff + (dateSpan.DaysDiff < 15? 0: 1);
				row.Cells.Add(Lan.g(this,"Months in Treatment"));
				row.Cells.Add(txTimeInMonths.ToString());
				gridAutoOrtho.ListGridRows.Add(row);
				//Months Rem
				row = new GridRow();
				row.Cells.Add(Lan.g(this,"Months Rem")); //Months Total - Total Tx Time
				row.Cells.Add(Math.Max(0,txMonthsTotal-txTimeInMonths).ToString());
				gridAutoOrtho.ListGridRows.Add(row);
			}
			else { //no ortho procedures charted for this patient.
				row = new GridRow();
				row.Cells.Add(""); 
				row.Cells.Add(Lan.g(this,"No ortho procedures charted."));
				gridAutoOrtho.ListGridRows.Add(row);
			}
			gridAutoOrtho.EndUpdate();
		}

		private void butEditAutoOrthoPlacement_Click(object sender,EventArgs e) {
			DateTime dateOrthoPlacement;
			try {
				dateOrthoPlacement=PIn.Date(textDateAutoOrthoPlacement.Text);
			}
			catch {
				MsgBox.Show(this,"Invalid date.");
				return;
			}
			PatientNoteCur.DateOrthoPlacementOverride=dateOrthoPlacement;
			PatientNotes.Update(PatientNoteCur,PatCur.Guarantor);
			FillAutoOrtho();
		}

		private void butAutoOrthoEditMonthsTreat_Click(object sender,EventArgs e) {
			int txMonths;
			try {
				txMonths=PIn.Byte(textAutoOrthoMonthsTreat.Text);
			}
			catch {
				MsgBox.Show(this,"Please enter a number between 0 and 255.");
				return;
			}
			PatientNoteCur.OrthoMonthsTreatOverride=txMonths;
			PatientNotes.Update(PatientNoteCur,PatCur.Guarantor);
			FillAutoOrtho();
		}

		private void butAutoOrthoDefaultPlacement_Click(object sender,EventArgs e) {
			PatientNoteCur.DateOrthoPlacementOverride=DateTime.MinValue;
			PatientNotes.Update(PatientNoteCur,PatCur.Guarantor);
			FillAutoOrtho();
		}

		private void butAutoOrthoDefaultMonthsTreat_Click(object sender,EventArgs e) {
			//Setting OrthoMonthsTreatOverride locks this value into place just in case it the pref changes down the road.
			PatientNoteCur.OrthoMonthsTreatOverride=PrefC.GetByte(PrefName.OrthoDefaultMonthsTreat);
			PatientNotes.Update(PatientNoteCur,PatCur.Guarantor);
			FillAutoOrtho();
		}

		private void gridAutoOrtho_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridAutoOrtho.ListGridRows[e.Row].Tag == null || gridAutoOrtho.ListGridRows[e.Row].Tag.GetType() != typeof(AutoOrthoPat)) {
				return;
			}
			AutoOrthoPat orthoPatCur=(AutoOrthoPat)gridAutoOrtho.ListGridRows[e.Row].Tag;
			if(orthoPatCur.InsPlan.OrthoType != OrthoClaimType.InitialPlusPeriodic) {
				MsgBox.Show(this,"To view this setup window, the insurance plan must be set to have an Ortho Claim Type of Initial Plus Periodic.");
				return;
			}
			FormOrthoPat FormOP=new FormOrthoPat(orthoPatCur.PatPlan,orthoPatCur.InsPlan,orthoPatCur.CarrierName,orthoPatCur.SubID,orthoPatCur.DefaultFee);
			FormOP.ShowDialog();
			if(FormOP.DialogResult==DialogResult.OK) {
				PatPlans.Update(orthoPatCur.PatPlan);
				FillAutoOrtho();
			}
		}

		private struct AutoOrthoPat {
			public InsPlan InsPlan;
			public PatPlan PatPlan;
			public string CarrierName;
			public string SubID;
			public double DefaultFee;
		}
		#endregion
		private void FillOrthoCasesGrid() {
			gridOrthoCases.BeginUpdate();
			gridOrthoCases.ListGridColumns.Clear();
			GridColumn col;
			col=new GridColumn(Lan.g("TableOrthoCases","Is Active"),70,HorizontalAlignment.Center);
			gridOrthoCases.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableOrthoCases","Is Transfer"),70,HorizontalAlignment.Center);
			gridOrthoCases.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableOrthoCases","Start Date"),130,HorizontalAlignment.Center);
			gridOrthoCases.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableOrthoCases","Completion Date"),0,HorizontalAlignment.Center);
			gridOrthoCases.ListGridColumns.Add(col);
			gridOrthoCases.ListGridRows.Clear();
			gridOrthoCases.EndUpdate();
			RefreshOrthoCasesGridRows();
		}

		private void RefreshOrthoCasesGridRows() {
			gridOrthoCases.BeginUpdate();
			gridOrthoCases.ListGridRows.Clear();
			if(_isSelectingFamily) {
				gridOrthoCases.EndUpdate();
				return;
			}
			GridRow row;
			if(PatCur!=null) {
				_listOrthoCases=OrthoCases.Refresh(PatCur.PatNum);
			}
			List<OrthoProcLink> listProcLinksForPat=OrthoProcLinks.GetManyByOrthoCases(_listOrthoCases.Select(x => x.OrthoCaseNum).ToList());
			Dictionary<long,OrthoProcLink> dictBandingProcLinks=listProcLinksForPat.Where(x => x.ProcLinkType==OrthoProcType.Banding)
				.ToDictionary(x => x.OrthoCaseNum,x => x);
			Dictionary<long,OrthoProcLink> dictDebondProcLinks=listProcLinksForPat.Where(x => x.ProcLinkType==OrthoProcType.Debond)
				.ToDictionary(x => x.OrthoCaseNum,x => x);
			List<Procedure> listLinkedProcsForPat=Procedures.GetManyProc(listProcLinksForPat.Select(x => x.ProcNum).ToList(),false);
			Dictionary<long,Procedure> dictBandingProcs=listLinkedProcsForPat.Where(x => dictBandingProcLinks.Select(y => y.Value.ProcNum)
			.ToList().Contains(x.ProcNum)).ToDictionary(z => z.ProcNum,z => z);
			Dictionary<long,Procedure> dictDebondProcs=listLinkedProcsForPat.Where(x => dictDebondProcLinks.Select(y => y.Value.ProcNum)
			.ToList().Contains(x.ProcNum)).ToDictionary(z => z.ProcNum,z => z);
			butAddOrthoCase.Enabled=true;
			OrthoProcLink bandingProcLink;
			OrthoProcLink debondProcLink;
			Procedure bandingProc;
			Procedure debondProc;
			foreach(OrthoCase orthoCase in _listOrthoCases) {
				//Skip the orthocase if it is inactive and we are not showing inactive orthocases
				if(checkHideInactiveOrthoCases.Checked && !orthoCase.IsActive) {
					continue;
				}
				row=new GridRow();
				if(orthoCase.IsActive) {
					row.Cells.Add("X");
					butAddOrthoCase.Enabled=false;//Can only have one active OrthoCase, se we deactivate the button to add a new active OrthoCase.
				}
				else {
					row.Cells.Add("");
				}
				if(orthoCase.IsTransfer) {
					row.Cells.Add("X");
					row.Cells.Add(orthoCase.BandingDate.ToShortDateString());
				}
				else {
					row.Cells.Add("");
					dictBandingProcLinks.TryGetValue(orthoCase.OrthoCaseNum,out bandingProcLink);
					if(bandingProcLink!=null) {
						dictBandingProcs.TryGetValue(bandingProcLink.ProcNum,out bandingProc);
						//If not null, and complete or TP'd and attached to appointment.
						if(bandingProc!=null && (bandingProc.ProcStatus==ProcStat.C || (bandingProc.ProcStatus==ProcStat.TP && bandingProc.AptNum!=0))) {
							row.Cells.Add(bandingProc.ProcDate.ToShortDateString());
						}
						else {
							row.Cells.Add(Lans.g("TableOrthoCases","Banding Not Scheduled"));
						}
					}
					else {
						row.Cells.Add(Lans.g("TableOrthoCases","Banding Not Scheduled"));
					}
				}
				dictDebondProcLinks.TryGetValue(orthoCase.OrthoCaseNum,out debondProcLink);
				if(debondProcLink!=null) {
					dictDebondProcs.TryGetValue(debondProcLink.ProcNum,out debondProc);
					if(debondProc!=null && debondProc.ProcStatus==ProcStat.C) {
						row.Cells.Add(debondProc.ProcDate.ToShortDateString());
					}
					else {
						row.Cells.Add(Lan.g("TableOrthoCases","Debond Incomplete"));
					}
				}
				else {
					row.Cells.Add(Lan.g("TableOrthoCases","Debond Incomplete"));
				}
				row.Tag=orthoCase;
				gridOrthoCases.ListGridRows.Add(row);
			}
			gridOrthoCases.EndUpdate();
		}

		private void CheckHideInactiveOrthoCases_CheckedChanged(object sender,EventArgs e) {
			RefreshOrthoCasesGridRows();
		}

		private void ButAddOrthoCase_Click(object sender,EventArgs e) {
			FormOrthoCase formOrthoCase=new FormOrthoCase(true,PatCur.PatNum);
			formOrthoCase.ShowDialog();
			if(formOrthoCase.DialogResult==DialogResult.OK) {
				RefreshOrthoCasesGridRows();
			}
		}

		private void ButMakeOrthoCaseActive_Click(object sender,EventArgs e) {
			if(gridOrthoCases.SelectedGridRows.Count<1) {
				return;
			}
			OrthoCase selectedOrthoCase=(OrthoCase)gridOrthoCases.SelectedGridRows[0].Tag;
			_listOrthoCases=OrthoCases.Activate(selectedOrthoCase,PatCur.PatNum);
			RefreshOrthoCasesGridRows();
			OrthoProcLink debondProcLink=OrthoProcLinks.GetByType(selectedOrthoCase.OrthoCaseNum,OrthoProcType.Debond);
			if(debondProcLink!=null) {//If link exists debond proc must be complete
				MsgBox.Show(this,"The activated Ortho Case has a completed debond procedure. This procedure must be detached before others can be added.");
			}
		}

		private void GridOrthoCases_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormOrthoCase formOrthoCase=new FormOrthoCase(false,PatCur.PatNum,(OrthoCase)gridOrthoCases.ListGridRows[e.Row].Tag);
			formOrthoCase.ShowDialog();
			if(formOrthoCase.DialogResult==DialogResult.OK) {
				RefreshOrthoCasesGridRows();
			}
		}

		private void gridAccount_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			DataTable table=DataSetMain.Tables["account"];
			//this seems to fire after a doubleclick, so this prevents error:
			if(e.Row>=table.Rows.Count){
				return;
			}
			gridPayPlan.SetSelected(false);
			foreach(int rowNum in gridAccount.SelectedIndices) {
				if(table.Rows[rowNum]["PayPlanNum"].ToString()!="0") {
					for(int i=0;i < gridPayPlan.ListGridRows.Count;i++) {
						if(((DataRow)(gridPayPlan.ListGridRows[i].Tag))["PayPlanNum"].ToString()==table.Rows[rowNum]["PayPlanNum"].ToString()) {
							gridPayPlan.SetSelected(i,true);
						}
					}
					if(table.Rows[rowNum]["procsOnObj"].ToString()!="0") {
						for(int i=0;i<table.Rows.Count;i++) {//loop through all rows
							if(table.Rows[i]["ProcNum"].ToString()==table.Rows[rowNum]["procsOnObj"].ToString()) {
								gridAccount.SetSelected(i,true);//select the pertinent procedure
								break;
							}
						}
					}
				}
			}
			if(ViewingInRecall) {
				return;
			}
			foreach(int rowNum in gridAccount.SelectedIndices) {
				DataRow rowCur=table.Rows[rowNum];
				if(rowCur["ClaimNum"].ToString()!="0") {//claims and claimpayments
					//Since we removed all selected items above, we need to reselect the claim the user just clicked on at the very least.
					//The "procsOnObj" column is going to be a comma delimited list of ProcNums associated to the corresponding claim.
					List<string> procsOnClaim=rowCur["procsOnObj"].ToString().Split(',').ToList();
					//Loop through the entire table and select any rows that are related to this claim (payments) while keeping track of their related ProcNums.
					for(int i=0;i<table.Rows.Count;i++) {//loop through all rows
						if(table.Rows[i]["ClaimNum"].ToString()==rowCur["ClaimNum"].ToString()) {
							gridAccount.SetSelected(i,true);//for the claim payments
							procsOnClaim.AddRange(table.Rows[i]["procsOnObj"].ToString().Split(','));
						}
					}
					//Other software companies allow claims to be created with no procedures attached.
					//This would cause "procsOnObj" to contain a ProcNum of '0' which the following loop would then select seemingly random rows (any w/ ProcNum=0)
					//Therefore, we need to specifically remove any entries of '0' from our procsOnClaim list before looping through it.
					procsOnClaim.RemoveAll(x => x=="0");
					//Loop through the table again in order to select any related procedures.
					for(int i=0;i<table.Rows.Count;i++) {
						if(procsOnClaim.Contains(table.Rows[i]["ProcNum"].ToString())) {
							gridAccount.SetSelected(i,true);
						}
					}
				}
				else if(rowCur["PayNum"].ToString()!="0") {
					List<string> procsOnPayment=rowCur["procsOnObj"].ToString().Split(',').ToList();
					List<string> paymentsOnObj=rowCur["paymentsOnObj"].ToString().Split(',').ToList();
					List<string> adjustsOnPayment=rowCur["adjustsOnObj"].ToString().Split(',').ToList();
					for(int i = 0;i<table.Rows.Count;i++) {//loop through all rows
						if(table.Rows[i]["PayNum"].ToString()==rowCur["PayNum"].ToString()) {
							gridAccount.SetSelected(i,true);//for other splits in family view
							procsOnPayment.AddRange(table.Rows[i]["procsOnObj"].ToString().Split(','));
							paymentsOnObj.AddRange(table.Rows[i]["paymentsOnObj"].ToString().Split(','));
							adjustsOnPayment.AddRange(table.Rows[i]["adjustsOnObj"].ToString().Split(','));
						}
					}
					for(int i=0;i<table.Rows.Count;i++){
						if(procsOnPayment.Contains(table.Rows[i]["ProcNum"].ToString())) {
							gridAccount.SetSelected(i,true);
						}
						if(paymentsOnObj.Contains(table.Rows[i]["PayNum"].ToString())) {
							gridAccount.SetSelected(i,true);
						}
						if(adjustsOnPayment.Contains(table.Rows[i]["Adjnum"].ToString())) {
							gridAccount.SetSelected(i,true);
						}
					}
				}
				else if(gridAccount.SelectedIndices.Contains(e.Row) && rowCur["AdjNum"].ToString()!="0" && rowCur["procsOnObj"].ToString()!="0") {
					for(int i = 0;i<table.Rows.Count;i++) {
						if(table.Rows[i]["ProcNum"].ToString()==rowCur["procsOnObj"].ToString()) {
							gridAccount.SetSelected(i,true);
							break;
						}
					}
				}
				else if(rowCur["ProcNumLab"].ToString()!="0" && rowCur["ProcNumLab"].ToString()!="") {//Canadian Lab procedure, select parents and other associated labs too.
					for(int i=0;i<table.Rows.Count;i++) {
						if(table.Rows[i]["ProcNum"].ToString()==rowCur["ProcNumLab"].ToString()) {
							gridAccount.SetSelected(i,true);
							continue;
						}
						if(table.Rows[i]["ProcNumLab"].ToString()==rowCur["ProcNumLab"].ToString()) {
							gridAccount.SetSelected(i,true);
							continue;
						}
					}
				}
				else if(rowCur["ProcNum"].ToString()!="0") {//Not a Canadian lab and is a procedure.
					for(int i=0;i<table.Rows.Count;i++) {
						if(table.Rows[i]["ProcNumLab"].ToString()==rowCur["ProcNum"].ToString()) {
							gridAccount.SetSelected(i,true);
							continue;
						}
					}
				}
			}
		}

		private void gridAccount_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(ViewingInRecall) return;
			Actscrollval=gridAccount.ScrollValue;
			DataTable table=DataSetMain.Tables["account"];
			if(table.Rows[e.Row]["ProcNum"].ToString()!="0"){
				Procedure proc=Procedures.GetOneProc(PIn.Long(table.Rows[e.Row]["ProcNum"].ToString()),true);
				Patient pat=FamCur.GetPatient(proc.PatNum);
				FormProcEdit FormPE=new FormProcEdit(proc,pat,FamCur);
				FormPE.ShowDialog();
			}
			else if(table.Rows[e.Row]["AdjNum"].ToString()!="0"){
				Adjustment adj=Adjustments.GetOne(PIn.Long(table.Rows[e.Row]["AdjNum"].ToString()));
				if(adj==null) {
					MsgBox.Show(this,"The adjustment has been deleted.");//Don't return. Fall through to the refresh. 
				}
				else { 
					FormAdjust FormAdj=new FormAdjust(PatCur,adj);
					FormAdj.ShowDialog();
				}
			}
			else if(table.Rows[e.Row]["PayNum"].ToString()!="0"){
				Payment PaymentCur=Payments.GetPayment(PIn.Long(table.Rows[e.Row]["PayNum"].ToString()));
				if(PaymentCur==null) {
					MessageBox.Show(Lans.g(this,"No payment exists.  Please run database maintenance method")+" "+nameof(DatabaseMaintenances.PaySplitWithInvalidPayNum));
					return;
				}
				FormPayment FormPayment2=new FormPayment(PatCur,FamCur,PaymentCur,false);
				FormPayment2.IsNew=false;
				FormPayment2.ShowDialog();
			}
			else if(table.Rows[e.Row]["ClaimNum"].ToString()!="0"){//claims and claimpayments
				if(!Security.IsAuthorized(Permissions.ClaimView)) {
					return;
				}
				Claim claim=Claims.GetClaim(PIn.Long(table.Rows[e.Row]["ClaimNum"].ToString()));
				if(claim==null) {
					MsgBox.Show(this,"The claim has been deleted.");
				}
				else {
					Patient pat=FamCur.GetPatient(claim.PatNum);
					FormClaimEdit FormClaimEdit2=new FormClaimEdit(claim,pat,FamCur);
					FormClaimEdit2.IsNew=false;
					FormClaimEdit2.ShowDialog();
				}
			}
			else if(table.Rows[e.Row]["StatementNum"].ToString()!="0"){
				Statement stmt=Statements.GetStatement(PIn.Long(table.Rows[e.Row]["StatementNum"].ToString()));
				if(stmt==null) {
					MsgBox.Show(this,"The statement has been deleted");//Don't return. Fall through to the refresh. 
				}
				else { 
					FormStatementOptions FormS=new FormStatementOptions();
					FormS.StmtCur=stmt;
					FormS.ShowDialog();
				}
			}
			else if(table.Rows[e.Row]["PayPlanNum"].ToString()!="0"){
				PayPlan payplan=PayPlans.GetOne(PIn.Long(table.Rows[e.Row]["PayPlanNum"].ToString()));
				if(payplan==null) {
					MsgBox.Show(this,"This pay plan has been deleted by another user.");
				}
				else {
					if(payplan.IsDynamic) {
						FormPayPlanDynamic formPayPlanDynamic=new FormPayPlanDynamic(payplan);
						formPayPlanDynamic.ShowDialog();
						if(formPayPlanDynamic.GotoPatNum!=0) {
							FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(formPayPlanDynamic.GotoPatNum),false);
							ModuleSelected(formPayPlanDynamic.GotoPatNum,false);
							return;
						}
					}
					else {//static payplan
						FormPayPlan formPayPlan=new FormPayPlan(payplan);
						formPayPlan.ShowDialog();
						if(formPayPlan.GotoPatNum!=0) {
							FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(formPayPlan.GotoPatNum),false);
							ModuleSelected(formPayPlan.GotoPatNum,false);
							return;
						}
					}
				}
			}
			ModuleSelected(PatCur.PatNum,_isSelectingFamily);
		}

		private void gridPayPlan_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			DataRow selectedRow=((DataRow)(gridPayPlan.ListGridRows[e.Row].Tag));
			if(selectedRow["PayPlanNum"].ToString()!="0") {//Payment plan
				PayPlan payplan=PayPlans.GetOne(PIn.Long(selectedRow["PayPlanNum"].ToString()));
				if(payplan==null) {
					MsgBox.Show(this,"This pay plan has been deleted by another user.");
				}
				else {
					if(payplan.IsDynamic) {
						FormPayPlanDynamic formPayPlanDynamic=new FormPayPlanDynamic(payplan);
						formPayPlanDynamic.ShowDialog();
						if(formPayPlanDynamic.GotoPatNum!=0) {
							FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(formPayPlanDynamic.GotoPatNum),false);
							ModuleSelected(formPayPlanDynamic.GotoPatNum,false);
							return;
						}
					}
					else {
						FormPayPlan formPayPlan=new FormPayPlan(payplan);
						formPayPlan.ShowDialog();
						if(formPayPlan.GotoPatNum!=0) {
							FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(formPayPlan.GotoPatNum),false);
							ModuleSelected(formPayPlan.GotoPatNum,false);
							return;
						}
					}
				}
				ModuleSelected(PatCur.PatNum,_isSelectingFamily);
			}
			else {//Installment Plan
				FormInstallmentPlanEdit FormIPE= new FormInstallmentPlanEdit();
				FormIPE.InstallmentPlanCur = InstallmentPlans.GetOne(PIn.Long(selectedRow["InstallmentPlanNum"].ToString()));
				FormIPE.IsNew=false;
				FormIPE.ShowDialog();
				ModuleSelected(PatCur.PatNum);
			}
		}

		private void GridTpSplits_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PaySplit split=(PaySplit)gridTpSplits.ListGridRows[e.Row].Tag;
			if(split==null) {
				return;
			}
			Payment paymentForSplit=Payments.GetPayment(split.PayNum);
			if(paymentForSplit==null) {
				MsgBox.Show(this,"Payment does not exist.");
				return;
			}
			FormPayment formPayment=new FormPayment(PatCur,FamCur,paymentForSplit,false);
			formPayment.IsNew=false;
			formPayment.ShowDialog();
			ModuleSelected(PatCur.PatNum,_isSelectingFamily);
		}

		private void gridAcctPat_CellClick(object sender,ODGridClickEventArgs e) {
			if(ViewingInRecall){
				return;
			}			
			if(e.Row==gridAcctPat.ListGridRows.Count-1) {//last row
				FormOpenDental.S_Contr_PatientSelected(FamCur.ListPats[0],false);
				ModuleSelected(FamCur.ListPats[0].PatNum,true);
			}
			else{
				long patNum=(long)gridAcctPat.ListGridRows[e.Row].Tag;
				Patient pat=FamCur.ListPats.First(x => x.PatNum==patNum);
				if(pat==null) {
					return;
				}
				FormOpenDental.S_Contr_PatientSelected(pat,false);
				ModuleSelected(patNum);
			}
		}

		private delegate void ToolBarClick();

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)){
				//standard predefined button
				switch(e.Button.Tag.ToString()){
					//case "Patient":
					//	OnPat_Click();
					//	break;
					case "Payment":
						if(Plugins.HookMethod(this,"ContrAccount.ToolBarMain_ButtonClick_Payment")) {
							break;
						}
						bool isTsiPayment=(TsiTransLogs.IsTransworldEnabled(PatCur.ClinicNum)
							&& Patients.IsGuarCollections(PatCur.Guarantor,includeSuspended:false)
							&& !MsgBox.Show(this,MsgBoxButtons.YesNo,"The guarantor of this family has been sent to TSI for a past due balance.  "
								+"Is the payment you are applying directly from the debtor or guarantor?\r\n\r\n"
								+"Yes - this payment is directly from the debtor/guarantor\r\n\r\n"
								+"No - this payment is from TSI"));
						InputBox inputBox=new InputBox(new List<InputBoxParam>() { new InputBoxParam(InputBoxType.ValidDouble,Lan.g(this,"Please enter an amount: ")),
							FamCur.ListPats.Length>1 ? (new InputBoxParam(InputBoxType.CheckBox,"",Lan.g(this," - Prefer this patient"),new Size(120,20))) : null }
							,new Func<string, bool>((text) => {
								if(text=="") {
									MsgBox.Show(this,"Please enter a value.");
									return false;//Should stop user from continuing to payment window.
								}
								return true;//Allow user to the payment window.
							})
						);
						Plugins.HookAddCode(this,"ContrAccount.ToolBarMain_ButtonClick_paymentInputBox",inputBox,PatCur);
						if(inputBox.ShowDialog()!=DialogResult.OK) {
							break;
						}
						toolBarButPay_Click(PIn.Double(inputBox.textResult.Text),preferCurrentPat:(inputBox.checkBoxResult?.Checked??false),isPayPressed:true,isTsiPayment:isTsiPayment);
						break;
					case "Adjustment":
						toolBarButAdj_Click();
						break;
					case "Insurance":
						CreateClaimDataWrapper createClaimDataWrapper=ClaimL.GetCreateClaimDataWrapper(PatCur,FamCur,GetCreateClaimItemsFromUI(),true);
						if(createClaimDataWrapper.HasError) {
							break;
						}
						createClaimDataWrapper=ClaimL.CreateClaimFromWrapper(true,createClaimDataWrapper);
						if(!createClaimDataWrapper.HasError || createClaimDataWrapper.DoRefresh) {
							ModuleSelected(PatCur.PatNum);
						}
						break;
					case "PayPlan":
						contextMenuPayPlan.Show(ToolBarMain,new Point(e.Button.Bounds.Location.X,e.Button.Bounds.Height));
						break;
					case "InstallPlan":
						toolBarButInstallPlan_Click();
						break;
					case "RepeatCharge":
						toolBarButRepeatCharge_Click();
						break;
					case "Statement":
						//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
						//when it comes from a toolbar click.
						//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
						ToolBarClick toolClick=toolBarButStatement_Click;
						this.BeginInvoke(toolClick);
						break;
					case "Questionnaire":
						toolBarButComm_Click();
						break;
					case "QuickProcs":
						toolBarButQuickProcs_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,PatCur);
			}
			Plugins.HookAddCode(this,"ContrAccount.ToolBarMain_ButtonClick_end",PatCur,e);
		}

		private void toolBarButPay_Click(double payAmt,bool preferCurrentPat=false,bool isPrePay=false,bool isIncomeTransfer=false,bool isPayPressed=false,bool isTsiPayment=false) {
			Payment PaymentCur=new Payment();
			PaymentCur.PayDate=DateTimeOD.Today;
			PaymentCur.PatNum=PatCur.PatNum;
			//Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
			//disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
			PaymentCur.ClinicNum=0;
			if(PrefC.HasClinicsEnabled) {//if clinics aren't enabled default to 0
				if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.PatientDefaultClinic) {
					PaymentCur.ClinicNum=PatCur.ClinicNum;
				}
				else if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.SelectedExceptHQ) {
					PaymentCur.ClinicNum=(Clinics.ClinicNum==0)?PatCur.ClinicNum:Clinics.ClinicNum;
				}
				else {
					PaymentCur.ClinicNum=Clinics.ClinicNum;
				}
			}
			PaymentCur.DateEntry=DateTimeOD.Today;//So that it will show properly in the new window.
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			if(listDefs.Count>0) {
				PaymentCur.PayType=listDefs[0].DefNum;
			}
			PaymentCur.PaymentSource=CreditCardSource.None;
			PaymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
			PaymentCur.PayAmt=payAmt;
			FormPayment FormP=new FormPayment(PatCur,FamCur,PaymentCur,preferCurrentPat);
			FormP.IsNew=true;
			FormP.IsIncomeTransfer=isIncomeTransfer;
			List<AccountEntry> listAcctEntries=new List<AccountEntry>();
			if(gridAccount.SelectedIndices.Length>0) {
				DataTable table=DataSetMain.Tables["account"];
				for(int i=0;i<gridAccount.SelectedIndices.Length;i++){
					if(table.Rows[gridAccount.SelectedIndices[i]]["ProcNum"].ToString()!="0") {
						//Add each selected proc to the list
						listAcctEntries.Add(new AccountEntry(Procedures.GetOneProc(PIn.Long(table.Rows[gridAccount.SelectedIndices[i]]["ProcNum"].ToString()),false)));
					}
					if(PIn.Double(table.Rows[gridAccount.SelectedIndices[i]]["chargesDouble"].ToString())>0
						&& table.Rows[gridAccount.SelectedIndices[i]]["PayPlanChargeNum"].ToString()!="0") {//PaymentPlanCharges
						//Add selected positive pay plan debit to the list. Important to check for chargesDouble because there can be negative debits.
						listAcctEntries.Add(new AccountEntry(PayPlanCharges.GetOne(PIn.Long(table.Rows[gridAccount.SelectedIndices[i]]["PayPlanChargeNum"].ToString()))));
					}
					if(table.Rows[gridAccount.SelectedIndices[i]]["AdjNum"].ToString()!="0") {//Adjustments
						Adjustment adjustment=Adjustments.GetOne(PIn.Long(table.Rows[gridAccount.SelectedIndices[i]]["AdjNum"].ToString()));
						if(adjustment.AdjAmt>0 && adjustment.ProcNum==0) {
							listAcctEntries.Add(new AccountEntry(adjustment));//don't include negative adjustments, or adjs attached to procs, since then we pay off the proc
						}
					}
				}
			}	
			if(isPrePay && PIn.Double(labelUnearnedAmt.Text)!=0) {
				if(listAcctEntries.Count<1) {
					FormProcSelect FormPS=new FormProcSelect(PatCur.PatNum,false,true);
					if(FormPS.ShowDialog()!=DialogResult.OK) {
						return;
					}
					listAcctEntries=PaymentEdit.CreateAccountEntries(FormPS.ListSelectedProcs);
				}
				FormP.UnearnedAmt=PIn.Double(labelUnearnedAmt.Text);
			}
			FormP.ListEntriesPayFirst=listAcctEntries;
			if(PaymentCur.PayDate.Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed) && !PrefC.GetBool(PrefName.AccountAllowFutureDebits)) {
				MsgBox.Show(this,"Payments cannot be in the future.");
				return;
			}
			PaymentCur.PayAmt=payAmt;
			Payments.Insert(PaymentCur);
			FormP.ShowDialog();
			//If this is a payment received from Transworld, we don't want to send any new update messages to Transworld for any splits on this payment.
			//To prevent new msgs from being sent, we will insert TsiTransLogs linked to all splits with TsiTransType.None.  The ODService will update the
			//log TransAmt for any edits to this paysplit instead of sending a new msg to Transworld.
			if(isTsiPayment) {
				Payment payCur=Payments.GetPayment(PaymentCur.PayNum);
				if(payCur!=null) {
					List<PaySplit> listSplits=PaySplits.GetForPayment(payCur.PayNum);
					if(listSplits.Count>0) {
						PatAging pAging=Patients.GetAgingListFromGuarNums(new List<long>() { PatCur.Guarantor }).FirstOrDefault();
						List<TsiTransLog> listLogsForInsert=new List<TsiTransLog>();
						foreach(PaySplit splitCur in listSplits) {
							double logAmt=pAging.ListTsiLogs.FindAll(x => x.FKeyType==TsiFKeyType.PaySplit && x.FKey==splitCur.SplitNum).Sum(x => x.TransAmt);
							if(splitCur.SplitAmt.IsEqual(logAmt)) {
								continue;//split already linked to logs that sum to the split amount, nothing to do with this one
							}
							listLogsForInsert.Add(new TsiTransLog() {
								PatNum=pAging.PatNum,//this is the account guarantor, since these are reconciled by guars
								UserNum=Security.CurUser.UserNum,
								TransType=TsiTransType.None,
								//TransDateTime=DateTime.Now,//set on insert, not editable by user
								//DemandType=TsiDemandType.Accelerator,//only valid for placement msgs
								//ServiceCode=TsiServiceCode.Diplomatic,//only valid for placement msgs
								ClientId=pAging.ListTsiLogs.FirstOrDefault()?.ClientId??"",//can be blank, not used since this isn't really sent to Transworld
								TransAmt=-splitCur.SplitAmt-logAmt,//Ex. already logged -10; split changed to -20; -20-(-10)=-10; -10 this split + -10 already logged = -20 split amt
								AccountBalance=pAging.AmountDue-splitCur.SplitAmt-logAmt,
								FKeyType=TsiFKeyType.PaySplit,
								FKey=splitCur.SplitNum,
								RawMsgText="This was not a message sent to Transworld.  This paysplit was entered due to a payment received from Transworld.",
								ClinicNum=(PrefC.HasClinicsEnabled?pAging.ClinicNum:0)
								//,TransJson=""//only valid for placement msgs
							});
						}
						if(listLogsForInsert.Count>0) {
							TsiTransLogs.InsertMany(listLogsForInsert);
						}
					}
				}
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemIncomeTransfer_Click(object sender,EventArgs e) {
			Payment PaymentCur=new Payment();
			PaymentCur.PayDate=DateTimeOD.Today;
			PaymentCur.PatNum=PatCur.PatNum;
			//Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
			//disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
			PaymentCur.ClinicNum=0;
			if(PrefC.HasClinicsEnabled) {//if clinics aren't enabled default to 0
				PaymentCur.ClinicNum=Clinics.ClinicNum;
				if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.PatientDefaultClinic) {
					PaymentCur.ClinicNum=PatCur.ClinicNum;
				}
				else if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.SelectedExceptHQ) {
					PaymentCur.ClinicNum=(Clinics.ClinicNum==0 ? PatCur.ClinicNum : Clinics.ClinicNum);
				}
			}
			PaymentCur.DateEntry=DateTimeOD.Today;//So that it will show properly in the new window.
			PaymentCur.PaymentSource=CreditCardSource.None;
			PaymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
			PaymentCur.PayAmt=0;
			PaymentCur.PayType=0;
			Payments.Insert(PaymentCur);
			FormIncomeTransferManage FormITM=new FormIncomeTransferManage(FamCur,PatCur,PaymentCur);
			if(FormITM.ShowDialog()!=DialogResult.OK) {
				Payments.Delete(PaymentCur);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void toolBarButAdj_Click() {
			AddAdjustmentToSelectedProcsHelper();
		}

		private void menuItemAddAdj_Click(object sender,EventArgs e) {
			AddAdjustmentToSelectedProcsHelper();
		}

		///<summary>If the user selects multiple procedures (validated) then we pass the selected procedures to FormMultiAdj. Otherwise if the user
		///selects one procedure (not validated) we maintain the previous functionality of opening FormAdjust.</summary>
		private void AddAdjustmentToSelectedProcsHelper(bool openMultiAdj=false) {
			Plugins.HookAddCode(this,"ContrAccount.AddAdjustmentToSelectedProcsHelper_beginning",PatCur,gridPayPlan);
			bool isTsiAdj=(TsiTransLogs.IsTransworldEnabled(PatCur.ClinicNum)
				&& Patients.IsGuarCollections(PatCur.Guarantor)
				&& !MsgBox.Show(this,MsgBoxButtons.YesNo,"The guarantor of this family has been sent to TSI for a past due balance.  "
					+"Is this an adjustment applied by the office?\r\n\r\n"
					+"Yes - this is an adjustment applied by the office\r\n\r\n"
					+"No - this adjustment is the result of a payment received from TSI"));
			DataTable tableAcct=DataSetMain.Tables["account"];
			List<Procedure> listSelectedProcs=new List<Procedure>();
			for(int i=0;i<gridAccount.SelectedIndices.Length;i++){
				long procNumCur=PIn.Long(tableAcct.Rows[gridAccount.SelectedIndices[i]]["ProcNum"].ToString());
				if(procNumCur==0){
					MsgBox.Show(this,"You can only select procedures.");
					return;
				}
				listSelectedProcs.Add(Procedures.GetOneProc(procNumCur,false));
			}
			//If the user selects multiple adjustments, open FormMultiAdj with the selected procedures
			if(listSelectedProcs.Count>1 || openMultiAdj) {
				//Open the form with only the selected procedures
				FormAdjMulti form=new FormAdjMulti(PatCur,listSelectedProcs);
				form.ShowDialog();
			}
			else {
				Patient patAdj=PatCur;
				Adjustment adjustmentCur=new Adjustment();
				adjustmentCur.DateEntry=DateTime.Today;//cannot be changed. Handled automatically
				adjustmentCur.AdjDate=DateTime.Today;
				adjustmentCur.ProcDate=DateTime.Today;
				adjustmentCur.ProvNum=PatCur.PriProv;
				adjustmentCur.PatNum=PatCur.PatNum;
				adjustmentCur.ClinicNum=PatCur.ClinicNum;
				if(gridAccount.SelectedGridRows.Count==1) {
					OrthoProcLink orthoProcLink=OrthoProcLinks.GetByProcNum(PIn.Long(tableAcct.Rows[gridAccount.SelectedIndices[0]]["ProcNum"].ToString()));
					if(orthoProcLink!=null) {
						MsgBox.Show(this,"Procedures linked to ortho cases cannot be adjusted.");
						return;
					}
					adjustmentCur.ProcNum=PIn.Long(tableAcct.Rows[gridAccount.SelectedIndices[0]]["ProcNum"].ToString());
					Procedure proc=Procedures.GetOneProc(adjustmentCur.ProcNum,false);
					if(proc!=null) {
						adjustmentCur.ProvNum=proc.ProvNum;
						adjustmentCur.ClinicNum=proc.ClinicNum;
						adjustmentCur.PatNum=proc.PatNum;
						if(adjustmentCur.PatNum!=PatCur.PatNum) {
							patAdj=FamCur.GetPatient(adjustmentCur.PatNum)??Patients.GetPat(adjustmentCur.PatNum);
						}
					}
				}
				FormAdjust FormAdjust2=new FormAdjust(patAdj,adjustmentCur,isTsiAdj);
				FormAdjust2.IsNew=true;
				FormAdjust2.ShowDialog();
				//Shared.ComputeBalances();
			}
			ModuleSelected(PatCur.PatNum);
		}

		///<summary>Returns a list of CreateClaimItems comprised from the selected items within gridAccount.
		///If no rows are currently selected then the list returned will be comprised of all items within the "account" table in the DataSet.</summary>
		private List<CreateClaimItem> GetCreateClaimItemsFromUI() {
			//There have been reports of concurrency issues so make a deep copy of the selected indices and the table first to help alleviate the problem.
			//See task #830623 and task #1266253 for more details.
			int[] arraySelectedIndices=(int[])gridAccount.SelectedIndices.Clone();
			DataTable table=GetTableFromDataSet("account");
			List<CreateClaimItem> listCreateClaimItems=ClaimL.GetCreateClaimItems(table,arraySelectedIndices);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				//We do not want to consider Canadian lab procs to be selected.  If we do, these lab procs will later cause the corresponding lab ClaimProcs to 
				//be included in the Claim's list of ClaimProcs, which will then cause the ClaimProcs for the labs to get a LineNumber, which will in turn cause
				//the EOB Importer to fail because the LineNumbers in the database's list of ClaimProcs no longer match the EOB LineNumbers.
				listCreateClaimItems.RemoveAll(x => x.ProcNumLab!=0);
			}
			return listCreateClaimItems;
		}

		private void menuItemSalesTax_Click(object sender,EventArgs e) {
			if(gridAccount.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one procedure.");
				return;
			}
			DataTable table=DataSetMain.Tables["account"];
			List<long> listSelectedProcNums=new List<long>();
			foreach(int idx in gridAccount.SelectedIndices) {
				if(table.Rows[idx]["ProcNum"].ToString()=="0") {
					continue;
				}
				listSelectedProcNums.Add(PIn.Long(table.Rows[idx]["ProcNum"].ToString()));
			}
			List<OrthoProcLink> listOrthoProcLinks=OrthoProcLinks.GetManyForProcs(listSelectedProcNums);
			if(listOrthoProcLinks.Count>0) {
				MsgBox.Show(this,"One or more of the selected procedures cannot be adjusted because it is attached to an ortho case." +
					" Please deselect these items and try again.");
				return;
			}
			double taxPercent=PrefC.GetDouble(PrefName.SalesTaxPercentage);
			long adjType=PrefC.GetLong(PrefName.SalesTaxAdjustmentType);
			foreach(int idx in gridAccount.SelectedIndices) {
				if(table.Rows[idx]["ProcNum"].ToString()=="0") {
					continue;//They selected a whole bunch, if it's not a proc don't make a sales tax adjustment
				}
				Procedure proc=Procedures.GetOneProc(PIn.Long(table.Rows[idx]["ProcNum"].ToString()),false);
				List<ClaimProc> listClaimProcs=ClaimProcs.GetForProcs(new List<long>() { proc.ProcNum });
				double writeOff=0;
				foreach(ClaimProc claimProc in listClaimProcs) {
					if(claimProc.Status==ClaimProcStatus.Estimate) {
						if(claimProc.WriteOffEstOverride!=-1) {
							writeOff+=claimProc.WriteOffEstOverride;
						}
						else if(claimProc.WriteOffEst!=-1) {
							writeOff+=claimProc.WriteOffEst;
						}
					}
					else if((claimProc.Status==ClaimProcStatus.Received || claimProc.Status==ClaimProcStatus.NotReceived) && claimProc.WriteOff!=-1) {
						writeOff+=claimProc.WriteOff;
					}
				}
				Adjustment adjustment=new Adjustment();
				adjustment.AdjDate=DateTime.Today;
				adjustment.ProcDate=proc.ProcDate;
				adjustment.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
				Clinic procClinic=Clinics.GetClinic(proc.ClinicNum);
				if(proc.ClinicNum!=0 && procClinic.DefaultProv!=0) {
					adjustment.ProvNum=procClinic.DefaultProv;
				}
				adjustment.PatNum=PatCur.PatNum;
				adjustment.ClinicNum=proc.ClinicNum;
				adjustment.AdjAmt=Math.Round((proc.ProcFee-writeOff)*(taxPercent/100),2);//Round to two places
				adjustment.AdjType=adjType;
				adjustment.ProcNum=proc.ProcNum;
				//adjustment.AdjNote=Lan.g(this,"Sales Tax");
				Adjustments.Insert(adjustment);
				TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(adjustment);
			}
			ModuleSelected(PatCur.PatNum);
		}
		
		private void menuItemAddMultAdj_Click(object sender,EventArgs e) {
			AddAdjustmentToSelectedProcsHelper(true);
		}

		private void menuInsPri_Click(object sender, System.EventArgs e) {
			CreateClaimDataWrapper createClaimDataWrapper=ClaimL.GetCreateClaimDataWrapper(PatCur,FamCur,GetCreateClaimItemsFromUI(),true,true);
			if(createClaimDataWrapper.HasError) {
				return;
			}
			if(PatPlans.GetOrdinal(PriSecMed.Primary,createClaimDataWrapper.ClaimData.ListPatPlans,createClaimDataWrapper.ClaimData.ListInsPlans
				,createClaimDataWrapper.ClaimData.ListInsSubs)==0)
			{
				MsgBox.Show(this,"The patient does not have any dental insurance plans.");
				return;
			}
			Claim claimCur=new Claim();
			claimCur.ClaimStatus="W";
			claimCur.DateSent=DateTime.Today;
			claimCur.DateSentOrig=DateTime.MinValue;
			//Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
			claimCur=ClaimL.CreateClaim(claimCur,"P",true,createClaimDataWrapper);
			if(claimCur.ClaimNum==0){
				ModuleSelected(PatCur.PatNum);
				return;
			}
			//still have not saved some changes to the claim at this point
			FormClaimEdit FormCE=new FormClaimEdit(claimCur,PatCur,FamCur);
			FormCE.IsNew=true;//this causes it to delete the claim if cancelling.
			//If there's unallocated amounts, we want to redistribute the money to other procedures.
			if(FormCE.ShowDialog()==DialogResult.OK && PIn.Double(labelUnearnedAmt.Text)>0) {
				ClaimL.AllocateUnearnedPayment(PatCur,FamCur,PIn.Double(labelUnearnedAmt.Text),claimCur);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuInsSec_Click(object sender, System.EventArgs e) {
			CreateClaimDataWrapper createClaimDataWrapper=ClaimL.GetCreateClaimDataWrapper(PatCur,FamCur,GetCreateClaimItemsFromUI(),true,true);
			if(createClaimDataWrapper.HasError) {
				return;
			}
			if(createClaimDataWrapper.ClaimData.ListPatPlans.Count<2) {
				MessageBox.Show(Lan.g(this,"Patient does not have secondary insurance."));
				return;
			}
			if(PatPlans.GetOrdinal(PriSecMed.Secondary,createClaimDataWrapper.ClaimData.ListPatPlans,createClaimDataWrapper.ClaimData.ListInsPlans
				,createClaimDataWrapper.ClaimData.ListInsSubs)==0)
			{
				MsgBox.Show(this,"Patient does not have secondary insurance.");
				return;
			}
			Claim claimCur=new Claim();
			claimCur.ClaimStatus="W";
			claimCur.DateSent=DateTimeOD.Today;
			claimCur.DateSentOrig=DateTime.MinValue;
			//Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
			claimCur=ClaimL.CreateClaim(claimCur,"S",true,createClaimDataWrapper);
			if(claimCur.ClaimNum==0) {
				ModuleSelected(PatCur.PatNum);
				return;
			}
			FormClaimEdit FormCE=new FormClaimEdit(claimCur,PatCur,FamCur);
			FormCE.IsNew=true;//this causes it to delete the claim if cancelling.
			//If there's unallocated amounts, we want to redistribute the money to other procedures.
			if(FormCE.ShowDialog()==DialogResult.OK && PIn.Double(labelUnearnedAmt.Text)>0) {
				ClaimL.AllocateUnearnedPayment(PatCur,FamCur,PIn.Double(labelUnearnedAmt.Text),claimCur);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuInsMedical_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			if(!ClaimL.CheckClearinghouseDefaults()) {
				return;
			}
			AccountModules.CreateClaimData data=AccountModules.GetCreateClaimData(PatCur,FamCur);
			long medSubNum=0;
			for(int i=0;i<data.ListPatPlans.Count;i++){
				InsSub sub=InsSubs.GetSub(data.ListPatPlans[i].InsSubNum,data.ListInsSubs);
				if(InsPlans.GetPlan(sub.PlanNum,data.ListInsPlans).IsMedical){
					medSubNum=sub.InsSubNum;
					break;
				}
			}
			if(medSubNum==0){
				MsgBox.Show(this,"Patient does not have medical insurance.");
				return;
			}
			DataTable table=DataSetMain.Tables["account"];
			Procedure proc;
			if(gridAccount.SelectedIndices.Length==0){
				//autoselect procedures
				for(int i=0;i<table.Rows.Count;i++){//loop through every line showing on screen
					if(table.Rows[i]["ProcNum"].ToString()=="0"){
						continue;//ignore non-procedures
					}
					proc=Procedures.GetProcFromList(data.ListProcs,PIn.Long(table.Rows[i]["ProcNum"].ToString()));
					if(proc.ProcFee==0){
						continue;//ignore zero fee procedures, but user can explicitly select them
					}
					if(proc.MedicalCode==""){
						continue;//ignore non-medical procedures
					}
					if(Procedures.NeedsSent(proc.ProcNum,medSubNum,data.ListClaimProcs)) {
						gridAccount.SetSelected(i,true);
					}
				}
				if(gridAccount.SelectedIndices.Length==0){//if still none selected
					MsgBox.Show(this,"Please select procedures first.");
					return;
				}
			}
			bool allAreProcedures=true;
			for(int i=0;i<gridAccount.SelectedIndices.Length;i++){
				if(table.Rows[gridAccount.SelectedIndices[i]]["ProcNum"].ToString()=="0"){
					allAreProcedures=false;
				}
			}
			if(!allAreProcedures){
				MsgBox.Show(this,"You can only select procedures.");
				return;
			}
			//Medical claims are slightly different so we'll just manually create the CreateClaimDataWrapper needed for creating the claim.
			CreateClaimDataWrapper createClaimDataWrapper=new CreateClaimDataWrapper() {
				Pat=PatCur,
				Fam=FamCur,
				ListCreateClaimItems=GetCreateClaimItemsFromUI(),
				ClaimData=data,
			};
			Claim claimCur=new Claim();
			claimCur.ClaimStatus="W";
			claimCur.DateSent=DateTimeOD.Today;
			claimCur.DateSentOrig=DateTime.MinValue;
			//Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
			claimCur=ClaimL.CreateClaim(claimCur,"Med",true,createClaimDataWrapper);
			if(claimCur.ClaimNum==0){
				ModuleSelected(PatCur.PatNum);
				return;
			}
			//still have not saved some changes to the claim at this point
			FormClaimEdit FormCE=new FormClaimEdit(claimCur,PatCur,FamCur);
			FormCE.IsNew=true;//this causes it to delete the claim if cancelling.
			//If there's unallocated amounts, we want to redistribute the money to other procedures.
			if(FormCE.ShowDialog()==DialogResult.OK && PIn.Double(labelUnearnedAmt.Text)>0) {
				ClaimL.AllocateUnearnedPayment(PatCur,FamCur,PIn.Double(labelUnearnedAmt.Text),claimCur);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuInsOther_Click(object sender, System.EventArgs e) {
			CreateClaimDataWrapper createClaimDataWrapper=ClaimL.GetCreateClaimDataWrapper(PatCur,FamCur,GetCreateClaimItemsFromUI(),true,true);
			if(createClaimDataWrapper.HasError) {
				return;
			}
			Claim claimCur=new Claim();
			claimCur.ClaimStatus="U";
			//Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
			claimCur=ClaimL.CreateClaim(claimCur,"Other",true,createClaimDataWrapper);
			if(claimCur.ClaimNum==0) {
				ModuleSelected(PatCur.PatNum);
				return;
			}
			//still have not saved some changes to the claim at this point
			FormClaimEdit FormCE=new FormClaimEdit(claimCur,PatCur,FamCur);
			FormCE.IsNew=true;//this causes it to delete the claim if cancelling.
			if(FormCE.ShowDialog()==DialogResult.OK && PIn.Double(labelUnearnedAmt.Text)>0) {
				ClaimL.AllocateUnearnedPayment(PatCur,FamCur,PIn.Double(labelUnearnedAmt.Text),claimCur);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void MenuItemDynamicPayPlan_Click(object sender,EventArgs e) {
			PayPlanHelper(PayPlanModes.Dynamic);//when payment plan is dynamic, insurance vs. pat does not matter.
		}

		private void menuItemInsPayPlan_Click(object sender,EventArgs e) {
			PayPlanHelper(PayPlanModes.Insurance);
		}

		private void menuItemPatPayPlan_Click(object sender,EventArgs e) {
			PayPlanHelper(PayPlanModes.Patient);
		}

		private void PayPlanHelper(PayPlanModes payPlanMode) {
			if(!Security.IsAuthorized(Permissions.PayPlanEdit)) {
				return;
			}
			bool isTsiPayplan=TsiTransLogs.IsTransworldEnabled(FamCur.Guarantor.ClinicNum) && Patients.IsGuarCollections(PatCur.Guarantor,false);
			string msg="";
			if(isTsiPayplan) {
				if(!Security.IsAuthorized(Permissions.Billing,true)) {
					msg=Lan.g(this,"The guarantor of this family has been sent to TSI for a past due balance.")+"\r\n"
						+Lan.g(this,"Creating a payment plan for this guarantor would cause the account to be suspended in the TSI system but you are not "
							+"authorized for")+"\r\n"
						+GroupPermissions.GetDesc(Permissions.Billing);
					MessageBox.Show(this,msg);
					return;
				}
				string billingType=Defs.GetName(DefCat.BillingTypes,PrefC.GetLong(PrefName.TransworldPaidInFullBillingType));
				msg=Lan.g(this,"The guarantor of this family has been sent to TSI for a past due balance.")+"\r\n"
					+Lan.g(this,"Creating this payment plan will suspend the TSI account for a maximum of 50 days if the account is in the Accelerator or "
						+"Profit Recovery stage.")+"\r\n"
					+Lan.g(this,"Continue creating the payment plan?")+"\r\n\r\n"
					+Lan.g(this,"Yes - Create the payment plan, send a suspend message to TSI, and change the guarantor's billing type to")+" "
						+billingType+".\r\n\r\n"
					+Lan.g(this,"No - Do not create the payment plan and allow TSI to continue managing the account.");
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,msg)) {
					return;
				}
			}
			PayPlan payPlan=new PayPlan();
			payPlan.IsNew=true;
			payPlan.PatNum=PatCur.PatNum;
			payPlan.Guarantor=PatCur.Guarantor;
			payPlan.PayPlanDate=DateTimeOD.Today;
			payPlan.CompletedAmt=0;
			long goToPatNum=0;
			if(payPlanMode.HasFlag(PayPlanModes.Dynamic)) {
				payPlan.IsDynamic=true;
				payPlan.ChargeFrequency=PayPlanFrequency.Monthly;
				payPlan.PayPlanNum=PayPlans.Insert(payPlan);
				FormPayPlanDynamic formPayPlanDynamic=new FormPayPlanDynamic(payPlan);
				formPayPlanDynamic.ShowDialog();
				goToPatNum=formPayPlanDynamic.GotoPatNum;
			}
			else {
				payPlan.PayPlanNum=PayPlans.Insert(payPlan);
				FormPayPlan formPayPlan=new FormPayPlan(payPlan);
				formPayPlan.TotalAmt=PatCur.EstBalance;
				formPayPlan.IsNew=true;
				formPayPlan.IsInsPayPlan=payPlanMode.HasFlag(PayPlanModes.Insurance);
				formPayPlan.ShowDialog();
				goToPatNum=formPayPlan.GotoPatNum;
			}
			if(goToPatNum!=0) {
				FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(goToPatNum),false);
				ModuleSelected(goToPatNum);//switches to other patient.
			}
			else{
				ModuleSelected(PatCur.PatNum);
			}
			if(isTsiPayplan && PayPlans.GetOne(payPlan.PayPlanNum)!=null) {
				msg=TsiTransLogs.SuspendGuar(FamCur.Guarantor);
				if(!string.IsNullOrEmpty(msg)) {
					MessageBox.Show(this,msg+"\r\n"+Lan.g(this,"The account will have to be suspended manually using the A/R Manager or the TSI web portal."));
				}
			}
		}
		
		private void toolBarButInstallPlan_Click() {
			if(InstallmentPlans.GetOneForFam(PatCur.Guarantor)!=null) {
				MsgBox.Show(this,"Family already has an installment plan.");
				return;
			}
			InstallmentPlan installPlan=new InstallmentPlan();
			installPlan.PatNum=PatCur.Guarantor;
			installPlan.DateAgreement=DateTime.Today;
			installPlan.DateFirstPayment=DateTime.Today;
			//InstallmentPlans.Insert(installPlan);
			FormInstallmentPlanEdit FormIPE=new FormInstallmentPlanEdit();
			FormIPE.InstallmentPlanCur=installPlan;
			FormIPE.IsNew=true;
			FormIPE.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void toolBarButRepeatCharge_Click(){
			RepeatCharge repeat=new RepeatCharge();
			repeat.PatNum=PatCur.PatNum;
			repeat.DateStart=DateTime.Today;
			FormRepeatChargeEdit FormR=new FormRepeatChargeEdit(repeat);
			FormR.IsNew=true;
			FormR.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void MenuItemRepeatStand_Click(object sender,System.EventArgs e) {
			if(!ProcedureCodes.GetContainsKey("001")) {
				return;
			}
			UpdatePatientBillingDay(PatCur.PatNum);
			RepeatCharge repeat=new RepeatCharge();
			repeat.PatNum=PatCur.PatNum;
			repeat.ProcCode="001";
			repeat.ChargeAmt=169;
			repeat.DateStart=DateTimeOD.Today;
			repeat.DateStop=DateTimeOD.Today.AddMonths(11);
			repeat.IsEnabled=true;
			RepeatCharges.Insert(repeat);
			repeat=new RepeatCharge();
			repeat.PatNum=PatCur.PatNum;
			repeat.ProcCode="001";
			repeat.ChargeAmt=119;
			repeat.DateStart=DateTimeOD.Today.AddYears(1);
			repeat.IsEnabled=true;
			RepeatCharges.Insert(repeat);
			ModuleSelected(PatCur.PatNum);
		}

		private void MenuItemRepeatEmail_Click(object sender,System.EventArgs e) {
			if(!ProcedureCodes.GetContainsKey("008")) {
				return;
			}
			UpdatePatientBillingDay(PatCur.PatNum);
			RepeatCharge repeat=new RepeatCharge();
			repeat.PatNum=PatCur.PatNum;
			repeat.ProcCode="008";
			repeat.ChargeAmt=89;
			repeat.DateStart=DateTimeOD.Today;
			repeat.IsEnabled=true;
			RepeatCharges.Insert(repeat);
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemRepeatCanada_Click(object sender,EventArgs e) {
			if(!ProcedureCodes.GetContainsKey("001")) {
				return;
			}
			UpdatePatientBillingDay(PatCur.PatNum);
			RepeatCharge repeat=new RepeatCharge();
			repeat.PatNum=PatCur.PatNum;
			repeat.ProcCode="001";
			repeat.ChargeAmt=135;
			repeat.DateStart=DateTimeOD.Today;
			repeat.DateStop=DateTimeOD.Today.AddMonths(11);
			repeat.IsEnabled=true;
			RepeatCharges.Insert(repeat);
			repeat=new RepeatCharge();
			repeat.PatNum=PatCur.PatNum;
			repeat.ProcCode="001";
			repeat.ChargeAmt=109;
			repeat.DateStart=DateTimeOD.Today.AddYears(1);
			repeat.IsEnabled=true;
			RepeatCharges.Insert(repeat);
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemRepeatSignupPortal_Click(object sender,EventArgs e) {
			if(PatCur==null) {
				MsgBox.Show(this,"A customer must be selected first.");
				return;
			}
			List<RegistrationKey> listRegKeys=RegistrationKeys.GetForPatient(PatCur.PatNum)
				//.Where(x => RegistrationKeys.KeyIsEnabled(x)) //We no longer want to only show enabled keys, sometimes we need to manage disabled.
				.OrderBy(x => x.RegKey)
				.ToList();
			if(listRegKeys.Count < 1) {
				MsgBox.Show(this,"No registration keys found for this customer's family.");
				return;
			}
			RegistrationKey regKey;
			if(listRegKeys.Count==1) {
				regKey=listRegKeys[0];
			}
			else {
				InputBox inputBox=new InputBox("Select a registration key to load into the Signup Portal"
					,listRegKeys.Select(x => "PatNum: "+x.PatNum+"   RegKey: "+x.RegKey).ToList());
				if(inputBox.ShowDialog()!=DialogResult.OK) {
					return;
				}
				regKey=listRegKeys[inputBox.SelectedIndex];
			}
			try {
				//Get the URL for the selected registration key.
				WebServiceMainHQProxy.EServiceSetup.SignupOut signupOut=WebServiceMainHQProxy.GetEServiceSetupLite(SignupPortalPermission.FromHQ
					,regKey.RegKey,"","","");
				FormWebBrowser FormWB=new FormWebBrowser(signupOut.SignupPortalUrl);
				FormWB.ShowDialog();
				ModuleSelected(PatCur.PatNum);//Refresh the module.
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void toolBarButStatement_Click() {
			Statement stmt=new Statement();
			stmt.PatNum=PatCur.Guarantor;
			stmt.DateSent=DateTimeOD.Today;
			stmt.IsSent=true;
			stmt.Mode_=StatementMode.InPerson;
			stmt.HidePayment=false;
			stmt.SinglePatient=false;
			stmt.Intermingled=PrefC.GetBool(PrefName.IntermingleFamilyDefault);
			stmt.StatementType=StmtType.NotSet;
			stmt.DateRangeFrom=DateTime.MinValue;
			if(PrefC.GetBool(PrefName.FuchsOptionsOn)){
				stmt.DateRangeFrom = PIn.Date(DateTime.Today.AddDays(-45).ToShortDateString());
				stmt.DateRangeTo = PIn.Date(DateTime.Today.ToShortDateString());
			} 
			else {
				if (textDateStart.errorProvider1.GetError(textDateStart) == "") {
					if (textDateStart.Text != "") {
						stmt.DateRangeFrom = PIn.Date(textDateStart.Text);
					}
				}
			}
			stmt.DateRangeTo = DateTimeOD.Today;//This is needed for payment plan accuracy.//new DateTime(2200,1,1);
			if (textDateEnd.errorProvider1.GetError(textDateEnd) == "") {
				if (textDateEnd.Text != "") {
					stmt.DateRangeTo = PIn.Date(textDateEnd.Text);
				}
			}
			stmt.Note = "";
			stmt.NoteBold = "";
			Patient guarantor = null;
			if(PatCur!=null) {
				guarantor = Patients.GetPat(PatCur.Guarantor);
			}
			if(guarantor!=null) {
				stmt.IsBalValid=true;
				stmt.BalTotal=guarantor.BalTotal;
				stmt.InsEst=guarantor.InsEst;
			}
			PrintStatement(stmt);
			ModuleSelected(PatCur.PatNum);
		}
		
		private void menuItemStatementWalkout_Click(object sender, System.EventArgs e) {
			Statement stmt=new Statement();
			stmt.PatNum=PatCur.PatNum;
			stmt.DateSent=DateTimeOD.Today;
			stmt.IsSent=true;
			stmt.Mode_=StatementMode.InPerson;
			stmt.HidePayment=true;
			stmt.Intermingled=PrefC.GetBool(PrefName.IntermingleFamilyDefault);
			stmt.SinglePatient=!stmt.Intermingled;
			stmt.IsReceipt=false;
			stmt.StatementType=StmtType.NotSet;
			stmt.DateRangeFrom=DateTimeOD.Today;
			stmt.DateRangeTo=DateTimeOD.Today;
			stmt.Note="";
			stmt.NoteBold="";
			Patient guarantor = null;
			if(PatCur!=null) {
				guarantor = Patients.GetPat(PatCur.Guarantor);
			}
			if(guarantor!=null) {
				stmt.IsBalValid=true;
				stmt.BalTotal=guarantor.BalTotal;
				stmt.InsEst=guarantor.InsEst;
			}
			PrintStatement(stmt);
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemStatementEmail_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.EmailSend)) {
				Cursor=Cursors.Default;
				return;
			}
			Statement stmt=new Statement();
			stmt.PatNum=PatCur.Guarantor;
			stmt.DateSent=DateTimeOD.Today;
			stmt.IsSent=true;
			stmt.Mode_=StatementMode.Email;
			stmt.HidePayment=false;
			stmt.SinglePatient=false;
			stmt.Intermingled=PrefC.GetBool(PrefName.IntermingleFamilyDefault);
			stmt.IsReceipt=false;
			stmt.StatementType=StmtType.NotSet;
			stmt.DateRangeFrom=DateTime.MinValue;
			if(textDateStart.errorProvider1.GetError(textDateStart)==""){
				if(textDateStart.Text!=""){
					stmt.DateRangeFrom=PIn.Date(textDateStart.Text);
				}
			}
			stmt.DateRangeTo=DateTimeOD.Today;//Needed for payplan accuracy.  Used to be setting to new DateTime(2200,1,1);
			if(textDateEnd.errorProvider1.GetError(textDateEnd)==""){
				if(textDateEnd.Text!=""){
					stmt.DateRangeTo=PIn.Date(textDateEnd.Text);
				}
			}
			stmt.Note="";
			stmt.NoteBold="";
			Patient guarantor = null;
			if(PatCur!=null) {
				guarantor = Patients.GetPat(PatCur.Guarantor);
			}
			if(guarantor!=null) {
				stmt.IsBalValid=true;
				stmt.BalTotal=guarantor.BalTotal;
				stmt.InsEst=guarantor.InsEst;
			}
			//It's pointless to give the user the window to select statement options, because they could just as easily have hit the More Options dropdown, then Email from there.
			PrintStatement(stmt);
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemReceipt_Click(object sender,EventArgs e) {
			Statement stmt=new Statement();
			stmt.PatNum=PatCur.PatNum;
			stmt.DateSent=DateTimeOD.Today;
			stmt.IsSent=true;
			stmt.Mode_=StatementMode.InPerson;
			stmt.HidePayment=true;
			stmt.Intermingled=PrefC.GetBool(PrefName.IntermingleFamilyDefault);
			stmt.SinglePatient=!stmt.Intermingled;
			stmt.IsReceipt=true;
			stmt.StatementType=StmtType.NotSet;
			stmt.DateRangeFrom=DateTimeOD.Today;
			stmt.DateRangeTo=DateTimeOD.Today;
			stmt.Note="";
			stmt.NoteBold="";
			Patient guarantor = null;
			if(PatCur!=null) {
				guarantor = Patients.GetPat(PatCur.Guarantor);
			}
			if(guarantor!=null) {
				stmt.IsBalValid=true;
				stmt.BalTotal=guarantor.BalTotal;
				stmt.InsEst=guarantor.InsEst;
			}
			PrintStatement(stmt);
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemInvoice_Click(object sender,EventArgs e) {
			DataTable table=DataSetMain.Tables["account"];
			Dictionary<string,List<long>> dictSuperFamItems=new Dictionary<string,List<long>>();
			Patient guarantor=Patients.GetPat(PatCur.Guarantor);
			Patient superHead=Patients.GetPat(PatCur.SuperFamily);
			if(gridAccount.SelectedIndices.Length==0 
				&& (!PrefC.GetBool(PrefName.ShowFeatureSuperfamilies) || !guarantor.HasSuperBilling || !superHead.HasSuperBilling)) 
			{
				//autoselect procedures, adjustments, and some pay plan charges
				for(int i=0;i<table.Rows.Count;i++) {//loop through every line showing on screen
					if(table.Rows[i]["ProcNum"].ToString()=="0" 
						&& table.Rows[i]["AdjNum"].ToString()=="0"
						&& table.Rows[i]["PayPlanChargeNum"].ToString()=="0") 
					{
						continue;//ignore items that aren't procs, adjustments, or pay plan charges
					}
					if(PIn.Date(table.Rows[i]["date"].ToString())!=DateTime.Today) {
						continue;
					}
					if(table.Rows[i]["ProcNum"].ToString()!="0") {//if selected item is a procedure
						Procedure proc=Procedures.GetOneProc(PIn.Long(table.Rows[i]["ProcNum"].ToString()),false);
						if(proc.StatementNum!=0) {//already attached so don't autoselect
							continue;
						}
						if(proc.PatNum!=PatCur.PatNum) {
							continue;
						}
					}
					else if(table.Rows[i]["PayPlanChargeNum"].ToString()!="0") {//selected item is pay plan charge
						PayPlanCharge payPlanCharges=PayPlanCharges.GetOne(PIn.Long(table.Rows[i]["PayPlanChargeNum"].ToString()));
						if(payPlanCharges.PatNum!=PatCur.PatNum){
							continue;
						}
						if(payPlanCharges.ChargeType!=PayPlanChargeType.Debit) {
							continue;
						}
						if(payPlanCharges.StatementNum!=0) {
							continue;
						}					
					}
					else {//item must be adjustment
						Adjustment adj=Adjustments.GetOne(PIn.Long(table.Rows[i]["AdjNum"].ToString()));
						if(adj.StatementNum!=0) {//already attached so don't autoselect
							continue;
						}
						if(adj.PatNum!=PatCur.PatNum) {
							continue;
						}
					}
					gridAccount.SetSelected(i,true);
				}
				if(gridAccount.SelectedIndices.Length==0) {//if still none selected
					MsgBox.Show(this,"Please select procedures, adjustments or payment plan charges first.");
					return;
				}
			}
			else if(gridAccount.SelectedIndices.Length==0 
				&& (PrefC.GetBool(PrefName.ShowFeatureSuperfamilies) && guarantor.HasSuperBilling && superHead.HasSuperBilling)) 
			{
				//No selections and superbilling is enabled for this family.  Show a window to select and attach procs to this statement for the superfamily.
				FormInvoiceItemSelect FormIIS=new FormInvoiceItemSelect(PatCur.SuperFamily);
				if(FormIIS.ShowDialog()==DialogResult.Cancel) {
					return;
				}
				dictSuperFamItems=FormIIS.DictSelectedItems;
			}
			for(int i=0;i<gridAccount.SelectedIndices.Length;i++) {
				DataRow row=table.Rows[gridAccount.SelectedIndices[i]];
				if(row["ProcNum"].ToString()=="0" 
					&& row["AdjNum"].ToString()=="0"
					&& row["PayPlanChargeNum"].ToString()=="0") //the selected item is neither a procedure nor an adjustment
				{
					MsgBox.Show(this,"You can only select procedures, payment plan charges or adjustments.");
					gridAccount.SetSelected(false);
					return;
				}
				if(row["ProcNum"].ToString()!="0") {//the selected item is a proc
					Procedure proc=Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),false);
					if(proc.PatNum!=PatCur.PatNum) {
						MsgBox.Show(this,"You can only select procedures, payment plan charges or adjustments for the current patient on an invoice.");
						gridAccount.SetSelected(false);
						return;
					}
					if(proc.StatementNum!=0) {
						MsgBox.Show(this,"Selected procedure(s) are already attached to an invoice.");
						gridAccount.SetSelected(false);
						return;
					}
				}
				else if(row["PayPlanChargeNum"].ToString()!="0") {
					PayPlanCharge ppCharge=PayPlanCharges.GetOne(PIn.Long(row["PayPlanChargeNum"].ToString()));
					if(ppCharge.PatNum!=PatCur.PatNum){
						MsgBox.Show(this,"You can only select procedures, payment plan charges or adjustments for a single patient on an invoice.");
						gridAccount.SetSelected(false);
						return;
					}
					if(ppCharge.ChargeType!=PayPlanChargeType.Debit) {
						MsgBox.Show(this,"You can only select payment plans charges that are debits.");
						gridAccount.SetSelected(false);
						return;
					}
					if(ppCharge.StatementNum!=0) {
						MsgBox.Show(this,"Selected payment plan charges(s) are already attached to an invoice.");
						gridAccount.SetSelected(false);
						return;
					}							
				}
				else{//the selected item must be an adjustment
					Adjustment adj=Adjustments.GetOne(PIn.Long(row["AdjNum"].ToString()));
					if(adj.AdjDate.Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
						MsgBox.Show(this,"Adjustments cannot be made for future dates");
						return;
					}
					if(adj.PatNum!=PatCur.PatNum) {
						MsgBox.Show(this,"You can only select procedures, payment plan charges or adjustments for a single patient on an invoice.");
						gridAccount.SetSelected(false);
						return;
					}
					if(adj.StatementNum!=0) {
						MsgBox.Show(this,"Selected adjustment(s) are already attached to an invoice.");
						gridAccount.SetSelected(false);
						return;
					}
				}
			}
			//At this point, all selected items are procedures or adjustments, and are not already attached, and are for a single patient.
			Statement stmt=new Statement();
			stmt.PatNum=PatCur.PatNum;
			stmt.DateSent=DateTimeOD.Today;
			stmt.IsSent=false;
			stmt.Mode_=StatementMode.InPerson;
			stmt.HidePayment=true;
			stmt.SinglePatient=true;
			stmt.Intermingled=false;
			stmt.IsReceipt=false;
			stmt.IsInvoice=true;
			stmt.StatementType=StmtType.NotSet;
			stmt.DateRangeFrom=DateTime.MinValue;
			stmt.DateRangeTo=DateTimeOD.Today;
			stmt.Note=PrefC.GetString(PrefName.BillingDefaultsInvoiceNote);
			stmt.NoteBold="";
			stmt.IsBalValid=true;
			stmt.BalTotal=guarantor.BalTotal;
			stmt.InsEst=guarantor.InsEst;
			if(dictSuperFamItems.Count > 0) {
				stmt.SuperFamily=PatCur.SuperFamily;
			}
			Statements.Insert(stmt);
			stmt.IsNew=true;
			List<Procedure> procsForPat=Procedures.Refresh(PatCur.PatNum);
			for(int i=0;i<gridAccount.SelectedIndices.Length;i++) {
				DataRow row=table.Rows[gridAccount.SelectedIndices[i]];
				if(row["ProcNum"].ToString()!="0") {//if selected item is a procedure
					Procedure proc=Procedures.GetProcFromList(procsForPat,PIn.Long(row["ProcNum"].ToString()));
					Procedure oldProc=proc.Copy();
					proc.StatementNum=stmt.StatementNum;
					if(proc.ProcStatus==ProcStat.C && proc.ProcDate.Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
						MsgBox.Show(this,"Completed procedures cannot be set for future dates.");
						return;
					}
					Procedures.Update(proc,oldProc);
				}
				else if(row["PayPlanChargeNum"].ToString()!="0") {
					PayPlanCharge ppCharge=PayPlanCharges.GetOne(PIn.Long(row["PayPlanChargeNum"].ToString()));
					ppCharge.StatementNum=stmt.StatementNum;
					PayPlanCharges.Update(ppCharge);
				}
				else {//selected item must be adjustment
					Adjustment adj=Adjustments.GetOne(PIn.Long(row["AdjNum"].ToString()));
					adj.StatementNum=stmt.StatementNum;
					Adjustments.Update(adj);
				}
			}
			foreach(KeyValuePair<string,List<long>> entry in dictSuperFamItems) {//Should really only have three keys, Proc, Pay Plan, and Adj
				if(entry.Key=="Proc") {//Procedure key, loop through all procedures
					foreach(long priKey in entry.Value) {
						Procedure newProc=Procedures.GetOneProc(priKey,false);
						Procedure oldProc=newProc.Copy();
						newProc.StatementNum=stmt.StatementNum;
						if(newProc.ProcStatus==ProcStat.C && newProc.ProcDate.Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
							MsgBox.Show(this,"Procedures cannot be set for future dates.");
							return;
						}
						Procedures.Update(newProc,oldProc);
					}
				}
				else if(entry.Key=="Pay Plan") {
					foreach(long priKey in entry.Value) {
						PayPlanCharge newCharge=PayPlanCharges.GetOne(priKey);
						newCharge.StatementNum=stmt.StatementNum;
						PayPlanCharges.Update(newCharge);
					}
				}
				else {//Adjustment key, loop through all adjustments
					foreach(long priKey in entry.Value) {
						Adjustment adj=Adjustments.GetOne(priKey);
						adj.StatementNum=stmt.StatementNum;
						Adjustments.Update(adj);
					}
				}
			}
			//All printing and emailing will be done from within the form:
			FormStatementOptions FormSO=new FormStatementOptions();
			FormSO.StmtCur=stmt;
			FormSO.ShowDialog();
			if(FormSO.DialogResult!=DialogResult.OK) {
				Statements.DeleteStatements(new List<Statement> { stmt });//detached from adjustments, procedurelogs, and paysplits as well
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemLimited_Click(object sender,EventArgs e) {
			DataTable table=DataSetMain.Tables["account"];
			DataRow row;
			#region Autoselect Today's Procedures
			if(gridAccount.SelectedIndices.Length==0) {//autoselect procedures
				for(int i = 0;i<table.Rows.Count;i++) {//loop through every line showing on screen
					row=table.Rows[i];
					if(row["ProcNum"].ToString()=="0" //ignore items that aren't procs
						|| PIn.Date(row["date"].ToString())!=DateTime.Today //autoselecting todays procs only
						|| PIn.Long(row["PatNum"].ToString())!=PatCur.PatNum) //only procs for the current patient
					{
						continue;
					}
					gridAccount.SetSelected(i,true);
				}
				if(gridAccount.SelectedIndices.Length==0) {//if still none selected
					MsgBox.Show(this,"Please select procedures, adjustments, payments, or claims first.");
					return;
				}
			}
			#endregion Autoselect Today's Procedures
			//guaranteed to have rows selected from here down, verify they are allowed transactions
			if(gridAccount.SelectedIndices.Any(x => table.Rows[x]["StatementNum"].ToString()!="0" || table.Rows[x]["PayPlanNum"].ToString()!="0")) {
				MsgBox.Show(this,"You can only select procedures, adjustments, payments, and claims.");
				gridAccount.SetSelected(false);
				return;
			}
			//At this point, all selected items are procedures, adjustments, payments, or claims.
			//get all ClaimNums from claimprocs for the selected procs
			List<long> listProcClaimNums=ClaimProcs.GetForProcs(gridAccount.SelectedIndices.Where(x => table.Rows[x]["ProcNum"].ToString()!="0")
				.Select(x => PIn.Long(table.Rows[x]["ProcNum"].ToString())).ToList()).FindAll(x => x.ClaimNum!=0).Select(x => x.ClaimNum).ToList();
			//get all ClaimNums for any selected claimpayments
			List<long> listPayClaimNums=gridAccount.SelectedIndices
				.Where(x => table.Rows[x]["ClaimNum"].ToString()!="0" && table.Rows[x]["ClaimPaymentNum"].ToString()=="1")
				.Select(x => PIn.Long(table.Rows[x]["ClaimNum"].ToString())).ToList();
			//prevent user from selecting a claimpayment that is not associated with any of the selected procs
			if(listPayClaimNums.Any(x => !listProcClaimNums.Contains(x))) {
				MsgBox.Show(this,"You can only select claim payments for the selected procedures.");
				gridAccount.SetSelected(false);
				return;
			}
			List<long> listPatNums=gridAccount.SelectedIndices
				.Select(x => table.Rows[x]["PatNum"].ToString()).Distinct().Select(x => PIn.Long(x)).ToList();
			List<long> listAdjNums=gridAccount.SelectedIndices
				.Where(x => table.Rows[x]["AdjNum"].ToString()!="0")
				.Select(x => PIn.Long(table.Rows[x]["AdjNum"].ToString())).ToList();
			List<long> listPayNums=gridAccount.SelectedIndices
				.Where(x => table.Rows[x]["PayNum"].ToString()!="0")
				.Select(x => PIn.Long(table.Rows[x]["PayNum"].ToString())).ToList();
			List<long> listProcNums=gridAccount.SelectedIndices
				.Where(x => table.Rows[x]["ProcNum"].ToString()!="0")
				.Select(x => PIn.Long(table.Rows[x]["ProcNum"].ToString())).ToList();
			Statement stmt=Statements.CreateLimitedStatement(listPatNums,PatCur.PatNum,listPayClaimNums,listAdjNums,listPayNums,listProcNums);
			//All printing and emailing will be done from within the form:
			FormStatementOptions FormSO=new FormStatementOptions();
			FormSO.StmtCur=stmt;
			FormSO.ShowDialog();
			if(FormSO.DialogResult!=DialogResult.OK) {
				Statements.DeleteStatements(new List<Statement> { stmt });//detached from adjustments, procedurelogs, and paysplits as well
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemLimitedCustom_Click(object sender,EventArgs e) {
			DataTable table=DataSetMain.Tables["account"];
			DataRow row;
			#region Autoselect Today's Procedures
			if(gridAccount.SelectedIndices.Length==0) {//autoselect procedures
				for(int i = 0;i<table.Rows.Count;i++) {//loop through every line showing on screen
					row=table.Rows[i];
					if(row["ProcNum"].ToString()=="0" //ignore items that aren't procs
						|| PIn.Date(row["date"].ToString())!=DateTime.Today //autoselecting todays procs only
						|| PIn.Long(row["PatNum"].ToString())!=PatCur.PatNum) //only procs for the current patient
					{
						continue;
					}
					gridAccount.SetSelected(i,true);
				}
			}
			#endregion Autoselect Today's Procedures
			List<long> listPatNums=null;
			List<long> listProcClaimNums=null;
			List<long> listPayClaimNums=null;
			List<long> listProcNums=null;
			List<long> listAdjNums=null;
			List<long> listPayNums=null;
			if(gridAccount.SelectedIndices.Length>0) {
				//guaranteed to have rows selected from here down, verify they are allowed transactions
				if(gridAccount.SelectedIndices.Any(x => table.Rows[x]["StatementNum"].ToString()!="0" || table.Rows[x]["PayPlanNum"].ToString()!="0")) {
					MsgBox.Show(this,"You can only select procedures, adjustments, payments, and claims.");
					gridAccount.SetSelected(false);
					return;
				}
				//get all ClaimNums from claimprocs for the selected procs
				listProcClaimNums=ClaimProcs.GetForProcs(gridAccount.SelectedIndices.Where(x => table.Rows[x]["ProcNum"].ToString()!="0")
					.Select(x => PIn.Long(table.Rows[x]["ProcNum"].ToString())).ToList()).FindAll(x => x.ClaimNum!=0).Select(x => x.ClaimNum).ToList();
				//get all ClaimNums for any selected claimpayments
				listPayClaimNums=gridAccount.SelectedIndices
					.Where(x => table.Rows[x]["ClaimNum"].ToString()!="0" && table.Rows[x]["ClaimPaymentNum"].ToString()=="1")
					.Select(x => PIn.Long(table.Rows[x]["ClaimNum"].ToString())).ToList();
				//prevent user from selecting a claimpayment that is not associatede with any of the selected procs
				if(listPayClaimNums.Any(x => !listProcClaimNums.Contains(x))) {
					MsgBox.Show(this,"You can only select claim payments for the selected procedures.");
					gridAccount.SetSelected(false);
					return;
				}
				listPatNums=gridAccount.SelectedIndices.Select(x => table.Rows[x]["PatNum"].ToString()).Distinct().Select(x => PIn.Long(x)).ToList();
				listAdjNums=gridAccount.SelectedIndices
					.Where(x => table.Rows[x]["AdjNum"].ToString()!="0")
					.Select(x => PIn.Long(table.Rows[x]["AdjNum"].ToString())).ToList();
				listPayNums=gridAccount.SelectedIndices
					.Where(x => table.Rows[x]["PayNum"].ToString()!="0")
					.Select(x => PIn.Long(table.Rows[x]["PayNum"].ToString())).ToList();
				listProcNums=gridAccount.SelectedIndices
					.Where(x => table.Rows[x]["ProcNum"].ToString()!="0")
					.Select(x => PIn.Long(table.Rows[x]["ProcNum"].ToString())).ToList();
			}
			FormLimitedStatementSelect formL=new FormLimitedStatementSelect(table,listPayClaimNums,listAdjNums,listPayNums,listProcNums,listPatNums);
			if(formL.ShowDialog()!=DialogResult.OK) {
				return;
			}
			listPatNums=formL.ListSelectedPatNums;
			listPayClaimNums=formL.ListSelectedPayClaimNums;
			listProcNums=formL.ListSelectedProcNums;
			listAdjNums=formL.ListSelectedAdjNums;
			listPayNums=formL.ListSelectedPayNums;
			//At this point, all selected items are procedures, adjustments, payments, or claims.
			Statement stmt=Statements.CreateLimitedStatement(listPatNums,PatCur.PatNum,listPayClaimNums,listAdjNums,listPayNums,listProcNums);
			//All printing and emailing will be done from within the form:
			FormStatementOptions FormSO=new FormStatementOptions();
			FormSO.StmtCur=stmt;
			FormSO.ShowDialog();
			if(FormSO.DialogResult!=DialogResult.OK) {
				Statements.DeleteStatements(new List<Statement> { stmt });//detached from adjustments, procedurelogs, and paysplits as well
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemStatementMore_Click(object sender, System.EventArgs e) {
			Statement stmt=new Statement();
			stmt.PatNum=PatCur.PatNum;
			stmt.DateSent=DateTime.Today;
			stmt.IsSent=false;
			stmt.Mode_=StatementMode.InPerson;
			stmt.HidePayment=false;
			stmt.SinglePatient=false;
			stmt.Intermingled=PrefC.GetBool(PrefName.IntermingleFamilyDefault);
			stmt.IsReceipt=false;
			stmt.StatementType=StmtType.NotSet;
			stmt.DateRangeFrom=DateTime.MinValue;
			stmt.DateRangeFrom=DateTime.MinValue;
			if(textDateStart.errorProvider1.GetError(textDateStart)==""){
				if(textDateStart.Text!=""){
					stmt.DateRangeFrom=PIn.Date(textDateStart.Text);
				}
			}
			if(PrefC.GetBool(PrefName.FuchsOptionsOn)) {
				stmt.DateRangeFrom=DateTime.Today.AddDays(-90);
			}
			stmt.DateRangeTo=DateTime.Today;//Needed for payplan accuracy.//new DateTime(2200,1,1);
			if(textDateEnd.errorProvider1.GetError(textDateEnd)==""){
				if(textDateEnd.Text!=""){
					stmt.DateRangeTo=PIn.Date(textDateEnd.Text);
				}
			}
			stmt.Note="";
			stmt.NoteBold="";
			Patient guarantor = null;
			if(PatCur!=null) {
				guarantor = Patients.GetPat(PatCur.Guarantor);
			}
			if(guarantor!=null) {
				stmt.IsBalValid=true;
				stmt.BalTotal=guarantor.BalTotal;
				stmt.InsEst=guarantor.InsEst;
			}
			//All printing and emailing will be done from within the form:
			FormStatementOptions FormSO=new FormStatementOptions();
			stmt.IsNew=true;
			FormSO.StmtCur=stmt;
			FormSO.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		/// <summary>Saves the statement.  Attaches a pdf to it by creating a doc object.  Prints it or emails it.  </summary>
		private void PrintStatement(Statement stmt) {
			Cursor=Cursors.WaitCursor;
			Statements.Insert(stmt);
			SheetDef sheetDef=SheetUtil.GetStatementSheetDef();
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,stmt.PatNum,stmt.HidePayment);
			DataSet dataSet=AccountModules.GetAccount(stmt.PatNum,stmt,doShowHiddenPaySplits:stmt.IsReceipt);
			sheet.Parameters.Add(new SheetParameter(true,"Statement") { ParamValue=stmt });
			SheetFiller.FillFields(sheet,dataSet,stmt);
			SheetUtil.CalculateHeights(sheet,dataSet,stmt);
			string tempPath=CodeBase.ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),stmt.PatNum.ToString()+".pdf");
			SheetPrinting.CreatePdf(sheet,tempPath,stmt,dataSet,null);
			long category=0;
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			for(int i=0;i<listDefs.Count;i++) {
				if(Regex.IsMatch(listDefs[i].ItemValue,@"S")) {
					category=listDefs[i].DefNum;
					break;
				}
			}
			if(category==0) {
				category=listDefs[0].DefNum;//put it in the first category.
			}
			//create doc--------------------------------------------------------------------------------------
			OpenDentBusiness.Document docc=null;
			try {
				docc=ImageStore.Import(tempPath,category,Patients.GetPat(stmt.PatNum));
			}
			catch {
				MsgBox.Show(this,"Error saving document.");
				//this.Cursor=Cursors.Default;
				return;
			}
			docc.ImgType=ImageType.Document;
			docc.DateCreated=stmt.DateSent;
			stmt.DocNum=docc.DocNum;//this signals the calling class that the pdf was created successfully.
			Statements.AttachDoc(stmt.StatementNum,docc);
			//if(ImageStore.UpdatePatient == null){
			//	ImageStore.UpdatePatient = new FileStore.UpdatePatientDelegate(Patients.Update);
			//}
			Patient guar=Patients.GetPat(stmt.PatNum);
			string guarFolder=ImageStore.GetPatientFolder(guar,ImageStore.GetPreferredAtoZpath());
			//OpenDental.Imaging.ImageStoreBase imageStore = OpenDental.Imaging.ImageStore.GetImageStore(guar);
			if(stmt.Mode_==StatementMode.Email) {
				if(!Security.IsAuthorized(Permissions.EmailSend)) {
					Cursor=Cursors.Default;
					return;
				}
				string attachPath=EmailAttaches.GetAttachPath();
				Random rnd=new Random();
				string fileName=DateTime.Now.ToString("yyyyMMdd")+DateTime.Now.TimeOfDay.Ticks.ToString()+rnd.Next(1000).ToString()+".pdf";
				string filePathAndName=FileAtoZ.CombinePaths(attachPath,fileName);
				FileAtoZ.Copy(ImageStore.GetFilePath(Documents.GetByNum(stmt.DocNum),guarFolder),filePathAndName,FileAtoZSourceDestination.AtoZToAtoZ);
				//Process.Start(filePathAndName);
				EmailMessage message=Statements.GetEmailMessageForStatement(stmt,guar);
				EmailAttach attach=new EmailAttach();
				attach.DisplayedFileName="Statement.pdf";
				attach.ActualFileName=fileName;
				message.Attachments.Add(attach);
				FormEmailMessageEdit FormE=new FormEmailMessageEdit(message,EmailAddresses.GetByClinic(guar.ClinicNum));
				FormE.IsNew=true;
				FormE.ShowDialog();
				//If user clicked delete or cancel, delete pdf and statement
				if(FormE.DialogResult==DialogResult.Cancel) {
					Patient pat;
					string patFolder;
					if(stmt.DocNum!=0) {
						//delete the pdf
						pat=Patients.GetPat(stmt.PatNum);
						patFolder=ImageStore.GetPatientFolder(pat,ImageStore.GetPreferredAtoZpath());
						List<Document> listdocs=new List<Document>();
						listdocs.Add(Documents.GetByNum(stmt.DocNum));
						try {
							ImageStore.DeleteDocuments(listdocs,patFolder);
						}
						catch {  //Image could not be deleted, in use.
							//This should never get hit because the file was created by this user within this method.  
							//If the doc cannot be deleted, then we will not stop them, they will have to manually delete it from the images module.
						}
					}
					//delete statement
					Statements.Delete(stmt);
				}
			}
			else {//not email
#if DEBUG
				//don't bother to check valid path because it's just debug.
				Document doc=Documents.GetByNum(stmt.DocNum);
				string imgPath=ImageStore.GetFilePath(doc,guarFolder);
				DateTime now=DateTime.Now;
				while(DateTime.Now<now.AddSeconds(5) && !FileAtoZ.Exists(imgPath)) {//wait up to 5 seconds.
					Application.DoEvents();
				}
				try {
					FileAtoZ.StartProcess(imgPath);
				}
				catch(Exception ex) {
					FriendlyException.Show($"Unable to open the following file: {doc.FileName}",ex);
				}
#else
				//Thread thread=new Thread(new ParameterizedThreadStart(SheetPrinting.PrintStatement));
				//thread.Start(new List<object> { sheetDef,stmt,tempPath });
				//NOTE: This is printing a "fresh" GDI+ version of the statment which is ever so slightly different than the PDFSharp statment that was saved to disk.
				sheet=SheetUtil.CreateSheet(sheetDef,stmt.PatNum,stmt.HidePayment);
				SheetFiller.FillFields(sheet,dataSet,stmt);
				SheetUtil.CalculateHeights(sheet,dataSet,stmt);
				SheetPrinting.Print(sheet,1,false,stmt);//use GDI+ printing, which is slightly different than the pdf.
#endif
			}
			Cursor=Cursors.Default;

		}

		private void textUrgFinNote_TextChanged(object sender, System.EventArgs e) {
			UrgFinNoteChanged=true;
		}

		private void textFinNote_TextChanged(object sender, System.EventArgs e) {
			FinNoteChanged=true;
		}

		//private void textCC_TextChanged(object sender,EventArgs e) {
		//  CCChanged=true;
		//  if(Regex.IsMatch(textCC.Text,@"^\d{4}$")
		//    || Regex.IsMatch(textCC.Text,@"^\d{4}-\d{4}$")
		//    || Regex.IsMatch(textCC.Text,@"^\d{4}-\d{4}-\d{4}$")) 
		//  {
		//    textCC.Text=textCC.Text+"-";
		//    textCC.Select(textCC.Text.Length,0);
		//  }
		//}

		//private void textCCexp_TextChanged(object sender,EventArgs e) {
		//  CCChanged=true;
		//}

		private void textUrgFinNote_Leave(object sender, System.EventArgs e) {
			//need to skip this if selecting another module. Handled in ModuleUnselected due to click event
			UpdateUrgFinNote();
		}

		public void UpdateUrgFinNote() {
			if(FamCur==null)
				return;
			if(UrgFinNoteChanged){
				Patient PatOld=FamCur.ListPats[0].Copy();
				FamCur.ListPats[0].FamFinUrgNote=textUrgFinNote.Text;
				Patients.Update(FamCur.ListPats[0],PatOld);
				UrgFinNoteChanged=false;
			}
		}

		private void textFinNote_Leave(object sender, System.EventArgs e) {
			UpdateFinNote();
		}

		public void UpdateFinNote() {
			if(FamCur==null)
				return;
			if(FinNoteChanged){
				PatientNoteCur.FamFinancial=textFinNote.Text;
				PatientNotes.Update(PatientNoteCur,PatCur.Guarantor);
				FinNoteChanged=false;
			}
		}

		//private void textCC_Leave(object sender,EventArgs e) {
		//  if(FamCur==null)
		//    return;
		//  if(CCChanged) {
		//    CCSave();
		//    CCChanged=false;
		//    ModuleSelected(PatCur.PatNum);
		//  }
		//}

		//private void textCCexp_Leave(object sender,EventArgs e) {
		//  if(FamCur==null)
		//    return;
		//  if(CCChanged){
		//    CCSave();
		//    CCChanged=false;
		//    ModuleSelected(PatCur.PatNum);
		//  }
		//}

		//private void CCSave(){
		//  string cc=textCC.Text;
		//  if(Regex.IsMatch(cc,@"^\d{4}-\d{4}-\d{4}-\d{4}$")){
		//    PatientNoteCur.CCNumber=cc.Substring(0,4)+cc.Substring(5,4)+cc.Substring(10,4)+cc.Substring(15,4);
		//  }
		//  else{
		//    PatientNoteCur.CCNumber=cc;
		//  }
		//  string exp=textCCexp.Text;
		//  if(Regex.IsMatch(exp,@"^\d\d[/\- ]\d\d$")){//08/07 or 08-07 or 08 07
		//    PatientNoteCur.CCExpiration=new DateTime(Convert.ToInt32("20"+exp.Substring(3,2)),Convert.ToInt32(exp.Substring(0,2)),1);
		//  }
		//  else if(Regex.IsMatch(exp,@"^\d{4}$")){//0807
		//    PatientNoteCur.CCExpiration=new DateTime(Convert.ToInt32("20"+exp.Substring(2,2)),Convert.ToInt32(exp.Substring(0,2)),1);
		//  } 
		//  else if(exp=="") {
		//    PatientNoteCur.CCExpiration=new DateTime();//Allow the experation date to be deleted.
		//  } 
		//  else {
		//    MsgBox.Show(this,"Expiration format invalid.");
		//  }
		//  PatientNotes.Update(PatientNoteCur,PatCur.Guarantor);
		//}

		private void butToday_Click(object sender,EventArgs e) {
			textDateStart.Text=DateTime.Today.ToShortDateString();
			textDateEnd.Text=DateTime.Today.ToShortDateString();
			ModuleSelected(PatCur.PatNum);
		}

		private void but45days_Click(object sender,EventArgs e) {
			textDateStart.Text=DateTime.Today.AddDays(-45).ToShortDateString();
			textDateEnd.Text="";
			ModuleSelected(PatCur.PatNum);
		}

		private void but90days_Click(object sender,EventArgs e) {
			textDateStart.Text=DateTime.Today.AddDays(-90).ToShortDateString();
			textDateEnd.Text="";
			ModuleSelected(PatCur.PatNum);
		}

		private void butDatesAll_Click(object sender,EventArgs e) {
			textDateStart.Text="";
			textDateEnd.Text="";
			ModuleSelected(PatCur.PatNum);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(PatCur==null){
				return;
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void checkShowDetail_Click(object sender,EventArgs e) {
			UserOdPref userOdPrefProcBreakdown = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.AcctProcBreakdown).FirstOrDefault();
			if(userOdPrefProcBreakdown==null) {
				userOdPrefProcBreakdown=new UserOdPref();
				userOdPrefProcBreakdown.UserNum=Security.CurUser.UserNum;
				userOdPrefProcBreakdown.FkeyType=UserOdFkeyType.AcctProcBreakdown;
				userOdPrefProcBreakdown.Fkey=0;
			}
			userOdPrefProcBreakdown.ValueString=POut.Bool(checkShowDetail.Checked);
			UserOdPrefs.Upsert(userOdPrefProcBreakdown);
			if(PatCur==null){
				return;
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void toolBarButComm_Click() {
			FormPat form=new FormPat();
			form.PatNum=PatCur.PatNum;
			form.FormDateTime=DateTime.Now;
			FormFormPatEdit FormP=new FormFormPatEdit();
			FormP.FormPatCur=form;
			FormP.IsNew=true;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.OK) {
				ModuleSelected(PatCur.PatNum);
			}
		}

		/*private void butTask_Click(object sender, System.EventArgs e) {
			//FormTaskListSelect FormT=new FormTaskListSelect(TaskObjectType.Patient,PatCur.PatNum);
			//FormT.ShowDialog();
		}*/

		private void butCreditCard_Click(object sender,EventArgs e) {
			FormCreditCardManage FormCCM=new FormCreditCardManage(PatCur);
			FormCCM.ShowDialog();
		}

		private void butServiceDateView_Click(object sender,EventArgs e) {
			//If the window is already open and it's for the same patient, bring the window to front. Otherwise close and/or open it.
			long patNum=_isSelectingFamily ? FamCur.Guarantor.PatNum : PatCur.PatNum;
			if(_formRpServiceDateView!=null && (_formRpServiceDateView.PatNum!=patNum || _formRpServiceDateView.IsFamily!=_isSelectingFamily)) {
				_formRpServiceDateView.Close();
				_formRpServiceDateView=null;
			}
			if(_formRpServiceDateView==null || _formRpServiceDateView.IsDisposed) {
				_formRpServiceDateView=new FormRpServiceDateView(patNum,_isSelectingFamily);
				_formRpServiceDateView.FormClosed+=new FormClosedEventHandler((o,e1) => { _formRpServiceDateView=null; });
				_formRpServiceDateView.Show();
			}
			if(_formRpServiceDateView.WindowState==FormWindowState.Minimized) {
				_formRpServiceDateView.WindowState=FormWindowState.Normal;
			}
			_formRpServiceDateView.BringToFront();
		}

		private void toolBarButQuickProcs_Click() {
			if(!Security.IsAuthorized(Permissions.AccountProcsQuickAdd,true)) {
				//only happens if permissions are changed after the program is opened. (Very Rare)
				MsgBox.Show(this,"Not authorized for Quick Procs.");
				return;
			}
			//Main QuickCharge button was clicked.  Create a textbox that can be entered so users can insert manually entered proc codes.
			if(!Security.IsAuthorized(Permissions.ProcComplCreate,true)) {//Button doesn't show up unless they have AccountQuickCharge permission. 
				//user can still use dropdown, just not type in codes.
				contextMenuQuickProcs.Show(this,new Point(_butQuickProcs.Bounds.X,_butQuickProcs.Bounds.Y+_butQuickProcs.Bounds.Height));
				return; 
			}
			textQuickProcs.SetBounds(_butQuickProcs.Bounds.X+1,_butQuickProcs.Bounds.Y+2,_butQuickProcs.Bounds.Width-17,_butQuickProcs.Bounds.Height-2);
			textQuickProcs.Visible=true;
			textQuickProcs.BringToFront();
			textQuickProcs.Focus();
			textQuickProcs.Capture=true;
		}

		private void textQuickCharge_FocusLost(object sender,EventArgs e) {
			textQuickProcs.Text="";
			textQuickProcs.Visible=false;
			textQuickProcs.Capture=false;
		}

		private void textQuickCharge_KeyDown(object sender,KeyEventArgs e) {
			//This is only the KeyDown event, user can still type if we return here.
			if(e.KeyCode!=Keys.Enter) {
				return;
			}
			textQuickProcs.Visible=false;
			textQuickProcs.Capture=false;
			e.Handled=true;//Suppress the "ding" in windows when pressing enter.
			e.SuppressKeyPress=true;//Suppress the "ding" in windows when pressing enter.
			if(textQuickProcs.Text=="") {
				return;
			}
			string quickProcText=textQuickProcs.Text;//because the text seems to disappear from textbox in menu bar when MsgBox comes up.
			if(PrefC.IsODHQ){
				if (PatCur.State=="") {
					MessageBox.Show("A valid state is required to process sales tax on procedures. "
						+"Please delete the procedure, enter a valid state, then reenter the procedure.");
				}
				//if this patient is in a taxable state
				if(AvaTax.ListTaxableStates!=null && AvaTax.ListTaxableStates.Any(x => x==PatCur.State)){
					if(!Patients.HasValidUSZipCode(PatCur)) {
						MessageBox.Show("A valid zip code is required to process sales tax on procedures in this patient's state. "
						+"Please delete the procedure, enter a valid zip, then reenter the procedure.");
					}
				}
			}
			Provider patProv=Providers.GetProv(PatCur.PriProv);
			if(AddProcAndValidate(quickProcText,patProv)) {
				SecurityLogs.MakeLogEntry(Permissions.AccountProcsQuickAdd,PatCur.PatNum
					,Lan.g(this,"The following procedures were added via the Quick Charge button from the Account module")
						+": "+string.Join(",",quickProcText));
				ModuleSelected(PatCur.PatNum);
			}
			textQuickProcs.Text="";
		}

		private void menuItemPrePay_Click(object sender,EventArgs e) {
			toolBarButPay_Click(0,isPrePay:true,isIncomeTransfer:true);
		}

		private void menuItemQuickProcs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AccountProcsQuickAdd,true)) {
				//only happens if permissions are changed after the program is opened or a different user logs in
				MsgBox.Show(this,"Not authorized for Quick Procs.");
				return;
			}
			//One of the QuickCharge menu items was clicked.
			if(sender.GetType()!=typeof(MenuItem)) {
				return;
			}
			Def quickChargeDef=_acctProcQuickAddDefs[contextMenuQuickProcs.MenuItems.IndexOf((MenuItem)sender)];
			string[] procCodes=quickChargeDef.ItemValue.Split(',');
			if(procCodes.Length==0) {
				//No items entered into the definition category.  Notify the user.
				MsgBox.Show(this,"There are no Quick Charge items in Setup | Definitions.  There must be at least one in order to use the Quick Charge drop down menu.");
			}
			List<string> procCodesAdded=new List<string>();
			Provider patProv=Providers.GetProv(PatCur.PriProv);
			for(int i=0;i<procCodes.Length;i++) {
				if(AddProcAndValidate(procCodes[i],patProv)) {
					procCodesAdded.Add(procCodes[i]);
				}
			}
			if(procCodesAdded.Count > 0) {
				SecurityLogs.MakeLogEntry(Permissions.AccountProcsQuickAdd,PatCur.PatNum
					,Lan.g(this,"The following procedures were added via the Quick Charge button from the Account module")
						+": "+string.Join(",",procCodesAdded));
				ModuleSelected(PatCur.PatNum);
			}
		}

		///<summary>Validated the procedure code using FormProcEdit and prompts user for input if required.</summary>
		private bool AddProcAndValidate(string procString,Provider patProv) {
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procString);
			if(procCode.CodeNum==0) {
				MsgBox.Show(this,"Invalid Procedure Code: "+procString);
				return false; //Invalid ProcCode string manually entered.
			}
			Procedure proc=new Procedure();
			proc.ProcStatus=ProcStat.C;
			proc.ClinicNum=PatCur.ClinicNum;
			proc.CodeNum=procCode.CodeNum;
			proc.DateEntryC=DateTime.Now;
			proc.DateTP=DateTime.Now;
			proc.PatNum=PatCur.PatNum;
			proc.ProcDate=DateTime.Now;
			proc.ToothRange="";
			proc.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default Proc Place of Service for the Practice is used. 
			if(!PrefC.GetBool(PrefName.EasyHidePublicHealth)) {
				proc.SiteNum=PatCur.SiteNum;
			}
			proc.ProvNum=procCode.ProvNumDefault;//use proc default prov if set
			if(proc.ProvNum==0) { //if none set, use primary provider.
				proc.ProvNum=patProv.ProvNum;
			}
			List<InsSub> insSubList=InsSubs.RefreshForFam(FamCur);
			List<InsPlan> insPlanList=InsPlans.RefreshForSubList(insSubList);
			List<PatPlan> patPlanList=PatPlans.Refresh(PatCur.PatNum);
			InsPlan insPlanPrimary=null;
			InsSub insSubPrimary=null;
			if(patPlanList.Count>0) {
				insSubPrimary=InsSubs.GetSub(patPlanList[0].InsSubNum,insSubList);
				insPlanPrimary=InsPlans.GetPlan(insSubPrimary.PlanNum,insPlanList);
			}
			proc.MedicalCode=procCode.MedicalCode;
			proc.ProcFee=Procedures.GetProcFee(PatCur,patPlanList,insSubList,insPlanList,proc.CodeNum,proc.ProvNum,proc.ClinicNum,proc.MedicalCode);
			proc.UnitQty=1;
			//Find out if we are going to link the procedure to an ortho case.
			OrthoCase activeOrthoCase=null;
			List<OrthoProcLink> listOrthoProcLinksForCase=null;
			bool willProcLinkToOrthoCase=OrthoProcLinks.WillProcLinkToOrthoCase(PatCur.PatNum,procCode.ProcCode,ref activeOrthoCase,ref listOrthoProcLinksForCase);
			Procedures.Insert(proc,skipDiscountPlanAdjustment:willProcLinkToOrthoCase);
			if(willProcLinkToOrthoCase) {
				Procedure procOld=proc.Copy();
				OrthoProcLinks.LinkProcForActiveOrthoCase(proc,activeOrthoCase,listOrthoProcLinksForCase);
				Procedures.Update(proc,procOld);
			}
			//launch form silently to validate code. If entry errors occur the form will be shown to user, otherwise it will close immediately.
			FormProcEdit FormPE=new FormProcEdit(proc,PatCur,FamCur,true);
			FormPE.IsNew=true;
			FormPE.ShowDialog();
			if(FormPE.DialogResult!=DialogResult.OK) {
				Procedures.Delete(proc.ProcNum);
				return false;
			}
			if(proc.ProcStatus==ProcStat.C) {
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,new List<string>() { ProcedureCodes.GetStringProcCode(proc.CodeNum) },PatCur.PatNum);
			}
			return true;
		}

		private void gridComm_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			int row=(int)gridComm.ListGridRows[e.Row].Tag;
			if(DataSetMain.Tables["Commlog"].Rows[row]["CommlogNum"].ToString()!="0") {
				Commlog commlogCur=
					Commlogs.GetOne(PIn.Long(DataSetMain.Tables["Commlog"].Rows[row]["CommlogNum"].ToString()));
				if(commlogCur==null) {
					MsgBox.Show(this,"This commlog has been deleted by another user.");
					ModuleSelected(PatCur.PatNum);
				}
				else {
					FormCommItem FormCI=new FormCommItem(commlogCur);
					if(FormCI.ShowDialog()==DialogResult.OK) {
						ModuleSelected(PatCur.PatNum);
					}
				}
			}
			else if(DataSetMain.Tables["Commlog"].Rows[row]["WebChatSessionNum"].ToString()!="0") {
				long webChatSessionNum=PIn.Long(DataSetMain.Tables["Commlog"].Rows[row]["WebChatSessionNum"].ToString());
				WebChatSession webChatSession=WebChatSessions.GetOne(webChatSessionNum);
				FormWebChatSession formWebChatSession=new FormWebChatSession(webChatSession,() => { ModuleSelected(PatCur.PatNum); });
				formWebChatSession.ShowDialog();
			}
			else if(DataSetMain.Tables["Commlog"].Rows[row]["EmailMessageNum"].ToString()!="0") {
				EmailMessage email=
					EmailMessages.GetOne(PIn.Long(DataSetMain.Tables["Commlog"].Rows[row]["EmailMessageNum"].ToString()));
				if(email.SentOrReceived==EmailSentOrReceived.WebMailReceived
					|| email.SentOrReceived==EmailSentOrReceived.WebMailRecdRead
					|| email.SentOrReceived==EmailSentOrReceived.WebMailSent
					|| email.SentOrReceived==EmailSentOrReceived.WebMailSentRead) 
				{
					//web mail uses special secure messaging portal
					FormWebMailMessageEdit FormWMME=new FormWebMailMessageEdit(PatCur.PatNum,email);
					if(FormWMME.ShowDialog()==DialogResult.OK) {
						ModuleSelected(PatCur.PatNum);
					}
				}
				else {
					FormEmailMessageEdit FormE=new FormEmailMessageEdit(email);
					FormE.ShowDialog();
					if(FormE.DialogResult==DialogResult.OK) {
						ModuleSelected(PatCur.PatNum);
					}
				}
			}
			else if(DataSetMain.Tables["Commlog"].Rows[row]["FormPatNum"].ToString()!="0") {
				FormPat form=FormPats.GetOne(PIn.Long(DataSetMain.Tables["Commlog"].Rows[row]["FormPatNum"].ToString()));
				FormFormPatEdit FormP=new FormFormPatEdit();
				FormP.FormPatCur=form;
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.OK) {
					ModuleSelected(PatCur.PatNum);
				}
			}
			else if(DataSetMain.Tables["Commlog"].Rows[row]["SheetNum"].ToString()!="0") {
				Sheet sheet=Sheets.GetSheet(PIn.Long(DataSetMain.Tables["Commlog"].Rows[row]["SheetNum"].ToString()));
				SheetUtil.ShowSheet(sheet,PatCur,FormSheetFillEdit_FormClosing);
			}
		}

		/// <summary>Event handler for closing FormSheetFillEdit when it is non-modal.</summary>
		private void FormSheetFillEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(((FormSheetFillEdit)sender).DialogResult==DialogResult.OK || ((FormSheetFillEdit)sender).DidChangeSheet) {
				ModuleSelected(PatCur.PatNum);
			}
		}

		private void Parent_MouseWheel(Object sender,MouseEventArgs e){
			if(Visible){
				this.OnMouseWheel(e);
			}
		}

		private void gridRepeat_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			FormRepeatChargeEdit FormR=new FormRepeatChargeEdit(RepeatChargeList[e.Row]);
			FormR.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void gridPatInfo_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(TerminalActives.PatIsInUse(PatCur.PatNum)) {
				MsgBox.Show(this,"Patient is currently entering info at a reception terminal.  Please try again later.");
				return;
			}
			if(gridPatInfo.ListGridRows[e.Row].Tag is PatFieldDef) {//patfield for an existing PatFieldDef
				PatFieldDef patFieldDef=(PatFieldDef)gridPatInfo.ListGridRows[e.Row].Tag;
				PatField field=PatFields.GetByName(patFieldDef.FieldName,_patFieldList);
				PatFieldL.OpenPatField(field,patFieldDef,PatCur.PatNum);
			}
			else if(gridPatInfo.ListGridRows[e.Row].Tag is PatField) {//PatField for a PatFieldDef that no longer exists
				PatField field=(PatField)gridPatInfo.ListGridRows[e.Row].Tag;
				FormPatFieldEdit FormPF=new FormPatFieldEdit(field);
				FormPF.ShowDialog();
			}
			else {
				FormPatientEdit FormP=new FormPatientEdit(PatCur,FamCur);
				FormP.IsNew=false;
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.OK) {
					FormOpenDental.S_Contr_PatientSelected(PatCur,false);
				}
			}
			ModuleSelected(PatCur.PatNum);
		}

		#region ProgressNotes
		///<summary>The supplied procedure row must include these columns: ProcDate,ProcStatus,ProcCode,Surf,ToothNum, and ToothRange, all in raw database format.</summary>
		private bool ShouldDisplayProc(DataRow row) {
			switch ((ProcStat)PIn.Long(row["ProcStatus"].ToString())) {
				case ProcStat.TP:
					if (checkShowTP.Checked) {
						return true;
					}
					break;
				case ProcStat.C:
					if (checkShowC.Checked) {
						return true;
					}
					break;
				case ProcStat.EC:
					if (checkShowE.Checked) {
						return true;
					}
					break;
				case ProcStat.EO:
					if (checkShowE.Checked) {
						return true;
					}
					break;
				case ProcStat.R:
					if (checkShowR.Checked) {
						return true;
					}
					break;
				case ProcStat.D:
					if (checkAudit.Checked) {
						return true;
					}
					break;
			}
			return false;
		}

		private void FillProgNotes() {
			ArrayList selectedTeeth = new ArrayList();//integers 1-32
			for(int i = 0;i < 32;i++) {
				selectedTeeth.Add(i);
			}
			gridProg.BeginUpdate();
			gridProg.ListGridColumns.Clear();
			GridColumn col = new GridColumn(Lan.g("TableProg", "Date"), 67);
			gridProg.ListGridColumns.Add(col);
			if(!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				col = new GridColumn(Lan.g("TableProg","Th"),27);
				gridProg.ListGridColumns.Add(col);
				col = new GridColumn(Lan.g("TableProg","Surf"),40);
				gridProg.ListGridColumns.Add(col);
			}
			col = new GridColumn(Lan.g("TableProg", "Dx"), 28);
			gridProg.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableProg", "Description"), 218);
			gridProg.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableProg", "Stat"), 25);
			gridProg.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableProg", "Prov"), 42);
			gridProg.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableProg", "Amount"), 48, HorizontalAlignment.Right);
			gridProg.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableProg", "ADA Code"), 62, HorizontalAlignment.Center);
			gridProg.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableProg", "User"), 62, HorizontalAlignment.Center);
			gridProg.ListGridColumns.Add(col);
			col = new GridColumn(Lan.g("TableProg", "Signed"), 55, HorizontalAlignment.Center);
			gridProg.ListGridColumns.Add(col);
			gridProg.NoteSpanStart = 2;
			gridProg.NoteSpanStop = 7;
			gridProg.ListGridRows.Clear();
			GridRow row;
			//Type type;
			if (DataSetMain == null) {
				gridProg.EndUpdate();
				return;
			}
			DataTable table = DataSetMain.Tables["ProgNotes"];
			//ProcList = new List<DataRow>();
			for (int i = 0; i < table.Rows.Count; i++) {
				if (table.Rows[i]["ProcNum"].ToString() != "0") {//if this is a procedure
					if (ShouldDisplayProc(table.Rows[i])) {
						//ProcList.Add(table.Rows[i]);//show it in the graphical tooth chart
						//and add it to the grid below.
					}
					else {
						continue;
					}
				}
				else if (table.Rows[i]["CommlogNum"].ToString() != "0") {//if this is a commlog
					if (!checkComm.Checked) {
						continue;
					}
				}
				else if (table.Rows[i]["RxNum"].ToString() != "0") {//if this is an Rx
					if (!checkRx.Checked) {
						continue;
					}
				}
				else if (table.Rows[i]["LabCaseNum"].ToString() != "0") {//if this is a LabCase
					if (!checkLabCase.Checked) {
						continue;
					}
				}
				else if (table.Rows[i]["AptNum"].ToString() != "0") {//if this is an Appointment
					if (!checkAppt.Checked) {
						continue;
					}
				}
				else if (table.Rows[i]["EmailMessageNum"].ToString() != "0") {//if this is an Email
					if(((HideInFlags)PIn.Int(table.Rows[i]["EmailMessageHideIn"].ToString())).HasFlag(HideInFlags.AccountProgNotes)) {
						continue;
					}
				}
				row = new GridRow();
				row.ColorLborder = Color.Black;
				//remember that columns that start with lowercase are already altered for display rather than being raw data.
				row.Cells.Add(table.Rows[i]["procDate"].ToString());
				if(!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
					row.Cells.Add(table.Rows[i]["toothNum"].ToString());
					row.Cells.Add(table.Rows[i]["Surf"].ToString());
				}
				row.Cells.Add(table.Rows[i]["dx"].ToString());
				row.Cells.Add(table.Rows[i]["description"].ToString());
				row.Cells.Add(table.Rows[i]["procStatus"].ToString());
				row.Cells.Add(table.Rows[i]["prov"].ToString());
				row.Cells.Add(table.Rows[i]["procFee"].ToString());
				row.Cells.Add(table.Rows[i]["ProcCode"].ToString());
				row.Cells.Add(table.Rows[i]["user"].ToString());
				row.Cells.Add(table.Rows[i]["signature"].ToString());
				if (checkNotes.Checked) {
					row.Note = table.Rows[i]["note"].ToString();
				}
				row.ColorText = Color.FromArgb(PIn.Int(table.Rows[i]["colorText"].ToString()));
				row.ColorBackG = Color.FromArgb(PIn.Int(table.Rows[i]["colorBackG"].ToString()));
				row.Tag = table.Rows[i];
				gridProg.ListGridRows.Add(row);
			
			}
			gridProg.EndUpdate();
			gridProg.ScrollToEnd();
		}

		private void gridProg_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//Chartscrollval = gridProg.ScrollValue;
			DataRow row = (DataRow)gridProg.ListGridRows[e.Row].Tag;
			if(row["ProcNum"].ToString() != "0") {
				if(checkAudit.Checked) {
					MsgBox.Show(this,"Not allowed to edit procedures when in audit mode.");
					return;
				}
				Procedure proc = Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),true);
				FormProcEdit FormP = new FormProcEdit(proc,PatCur,FamCur);
				FormP.ShowDialog();
				if(FormP.DialogResult != DialogResult.OK) {
					return;
				}
			}
			else if(row["CommlogNum"].ToString() != "0") {
				Commlog comm = Commlogs.GetOne(PIn.Long(row["CommlogNum"].ToString()));
				if(comm==null) {
					MsgBox.Show(this,"This commlog has been deleted by another user.");
				}
				else { 
					FormCommItem FormCI=new FormCommItem(comm);
					if(FormCI.ShowDialog()!=DialogResult.OK) {
						return;
					}
				}
			}
			else if(row["RxNum"].ToString() != "0") {
				RxPat rx = RxPats.GetRx(PIn.Long(row["RxNum"].ToString()));
				if(rx==null) {
					MsgBox.Show(this,"This prescription has been deleted by another user.");
				}
				else {
					FormRxEdit FormRxE = new FormRxEdit(PatCur,rx);
					FormRxE.ShowDialog();
					if(FormRxE.DialogResult != DialogResult.OK) {
						return;
					}
				}
			}
			else if(row["LabCaseNum"].ToString() != "0") {
				LabCase lab=LabCases.GetOne(PIn.Long(row["LabCaseNum"].ToString()));
				if(lab==null) {
					MsgBox.Show(this,"This LabCase has been deleted by another user.");
				}
				else {
					FormLabCaseEdit FormL=new FormLabCaseEdit();
					FormL.CaseCur=lab;
					FormL.ShowDialog();
				}
			}
			else if(row["TaskNum"].ToString() != "0") {
				Task task = Tasks.GetOne(PIn.Long(row["TaskNum"].ToString()));
				FormTaskEdit FormT = new FormTaskEdit(task);
				FormT.Closing+=new CancelEventHandler(TaskGoToEvent);
				FormT.Show();//non-modal
			}
			else if(row["AptNum"].ToString() != "0") {
				//Appointment apt=Appointments.GetOneApt(
				FormApptEdit FormA = new FormApptEdit(PIn.Long(row["AptNum"].ToString()));
				//PinIsVisible=false
				FormA.ShowDialog();
				if(FormA.DialogResult != DialogResult.OK) {
					return;
				}
			}
			else if(row["EmailMessageNum"].ToString() != "0") {
				EmailMessage msg = EmailMessages.GetOne(PIn.Long(row["EmailMessageNum"].ToString()));
				FormEmailMessageEdit FormE = new FormEmailMessageEdit(msg);
				FormE.ShowDialog();
				if(FormE.DialogResult != DialogResult.OK) {
					return;
				}
			}
			ModuleSelected(PatCur.PatNum);
		}

		public void TaskGoToEvent(object sender,CancelEventArgs e) {
			FormTaskEdit FormT=(FormTaskEdit)sender;
			TaskObjectType GotoType=FormT.GotoType;
			long keyNum=FormT.GotoKeyNum;
			if(GotoType==TaskObjectType.None) {
				return;
			}
			if(GotoType == TaskObjectType.Patient) {
				if(keyNum != 0) {
					Patient pat = Patients.GetPat(keyNum);
					FormOpenDental.S_Contr_PatientSelected(pat,false);
					ModuleSelected(pat.PatNum);
					return;
				}
			}
			if(GotoType == TaskObjectType.Appointment) {
				//There's nothing to do here, since we're not in the appt module.
				return;
			}
		}

		private void checkShowTP_Click(object sender,EventArgs e) {
			FillProgNotes();
		}

		private void checkShowC_Click(object sender,EventArgs e) {
			FillProgNotes();

		}

		private void checkShowE_Click(object sender,EventArgs e) {
			FillProgNotes();

		}

		private void checkShowR_Click(object sender,EventArgs e) {
			FillProgNotes();

		}

		private void checkAppt_Click(object sender,EventArgs e) {
			FillProgNotes();

		}

		private void checkComm_Click(object sender,EventArgs e) {
			FillProgNotes();

		}

		private void checkLabCase_Click(object sender,EventArgs e) {
			FillProgNotes();

		}

		private void checkRx_Click(object sender,EventArgs e) {
		if (checkRx.Checked)//since there is no double click event...this allows almost the same thing
            {
                checkShowTP.Checked=false;
                checkShowC.Checked=false;
                checkShowE.Checked=false;
                checkShowR.Checked=false;
                checkNotes.Checked=true;
                checkRx.Checked=true;
                checkComm.Checked=false;
                checkAppt.Checked=false;
				checkLabCase.Checked=false;
                checkExtraNotes.Checked=false;

            }

			FillProgNotes();

		}

		private void checkExtraNotes_Click(object sender,EventArgs e) {
			FillProgNotes();

		}

		private void checkNotes_Click(object sender,EventArgs e) {
			FillProgNotes();

		}

		private void butShowNone_Click(object sender,EventArgs e) {
			checkShowTP.Checked=false;
			checkShowC.Checked=false;
			checkShowE.Checked=false;
			checkShowR.Checked=false;
			checkAppt.Checked=false;
			checkComm.Checked=false;
			checkLabCase.Checked=false;
			checkRx.Checked=false;
			checkShowTeeth.Checked=false;

			FillProgNotes();

		}

		private void butShowAll_Click(object sender,EventArgs e) {
			checkShowTP.Checked=true;
			checkShowC.Checked=true;
			checkShowE.Checked=true;
			checkShowR.Checked=true;
			checkAppt.Checked=true;
			checkComm.Checked=true;
			checkLabCase.Checked=true;
			checkRx.Checked=true;
			checkShowTeeth.Checked=false;
			FillProgNotes();

		}

		private void gridProg_MouseUp(object sender,MouseEventArgs e) {

		}
		#endregion ProgressNotes

		private void checkShowFamilyComm_Click(object sender,EventArgs e) {
			FillComm();
		}
		private void checkShowCompletePayPlans_Click(object sender,EventArgs e) {
			Prefs.UpdateBool(PrefName.AccountShowCompletedPaymentPlans,checkShowCompletePayPlans.Checked);
			FillPaymentPlans();
			RefreshModuleScreen(false); //so the grids get redrawn if the payment plans grid hides/shows itself.
		}

		private void labelInsRem_MouseEnter(object sender,EventArgs e) {
			groupBoxFamilyIns.Visible=true;
			groupBoxIndIns.Visible=true;
		}

		private void labelInsRem_MouseLeave(object sender,EventArgs e) {
			groupBoxFamilyIns.Visible=false;
			groupBoxIndIns.Visible=false;
		}

		private void labelInsRem_Click(object sender,EventArgs e) {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				//Since the bonus information in FormInsRemain is currently only helpful in Canada,
				//we have decided not to show the form for other countries at this time.
				return;
			}
			if(PatCur==null) {
				return;
			}
			FormInsRemain FormIR=new FormInsRemain(PatCur.PatNum);
			FormIR.ShowDialog();
		}

		private void menuPrepayment_Click(object sender,EventArgs e) {
			if(PatCur==null) {
				return;
			}
			FormPrepaymentTool FormPT=new FormPrepaymentTool(PatCur);
			if(FormPT.ShowDialog()==DialogResult.OK) {
				Family famCur=Patients.GetFamily(PatCur.PatNum);
				FormPayment FormP=new FormPayment(PatCur,famCur,FormPT.ReturnPayment,false);
				FormP.IsNew=true;
				Payments.Insert(FormPT.ReturnPayment);
				RefreshModuleData(PatCur.PatNum,false);
				RefreshModuleScreen(false);
				FormP.ShowDialog();
			}
			RefreshModuleData(PatCur.PatNum,false);
			RefreshModuleScreen(false);
		}

		///<summary>Hides the 'Add Adjustment' context menu if anything other than a procedure is selected.</summary>
		private void contextMenuAcctGrid_Popup(object sender,EventArgs e) {
			DataTable table=DataSetMain.Tables["account"];
			List<int> listSelectedRows=gridAccount.SelectedIndices.ToList();
			foreach(int row in listSelectedRows) {
				if(table.Rows[row]["ProcNum"].ToString()=="0") {
					menuItemAddAdj.Enabled=false;
					return;
				}
			}
			//If all selected rows are adjustments enable the 'Add Adjustment' button.
			menuItemAddAdj.Enabled=true;
		}
	}
}











