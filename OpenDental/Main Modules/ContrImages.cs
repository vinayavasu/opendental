/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
//#define ISXP
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using Tao.OpenGl;
using CodeBase;
using xImageDeviceManager;
using System.Text.RegularExpressions;
using System.Linq;
using OpenDental.Bridges;
using OpenDental.Thinfinity;

namespace OpenDental {

	///<summary></summary>
	public partial class ContrImages:System.Windows.Forms.UserControl {
		#region Fields - Private
		///<summary>Locker for _listImageDownload</summary>
		//private bool allowTopaz;
		private object _apteryxLocker=new object();
		///<summary>In-memory copies of the images being viewed/edited. No changes are made to these images in memory, they are just kept resident to avoid having to reload the images from disk each time the screen needs to be redrawn.  If no mount, this will just be one image.  A mount will contain a series of images.</summary>
		private Bitmap[] _bitmapArrayImagesCur=new Bitmap[1];
		///<summary>The image currently on the screen.  If a mount, this will be an image representing the entire mount.</summary>
		private Bitmap _bitmapImageRenderingNow=null;
		///<summary>Indicates which documents to update in the image worker thread. This variable must be locked before accessing it and it must also be the same length as DocsInMount at all times.</summary>
		private bool[] _boolArrayMountIdxsFlaggedForUpdate=null;
		///<summary>If this is not zero, then this indicates a different mode special for claimpayment.</summary>
		private long _claimPaymentNum;
		DateTime _dateTimeMouseMoved=new DateTime(1,1,1);
		//private List<Def> DefListExpandedCats=new List<Def>();
		///<summary>List of documents within the currently selected mount (if any).</summary>
		private Document[] _documentArrayDocsInMount=null;
		///<summary>Edited by the main thread to reflect selection changes. Read by worker thread.</summary>
		private Document _documentForSettings=null;
		///<summary>Keeps track of the document settings for the currently selected document or mount.</summary>
		private Document _documentSelected=new Document();
		///<summary>If this is not null, then this indicates a different mode special for amendments.</summary>
		private EhrAmendment _ehrAmendmentCur;
		///<summary>Used as a thread-safe communication device between the main and worker threads.</summary>
		private EventWaitHandle _eventWaitHandleSettings=new EventWaitHandle(false,EventResetMode.AutoReset);
		private Family _familyCur;
		///<summary>Used to flag when ImagesModuleTreeIsCollapsed=2 to disable some of the on expand and collapse logic.</summary>
		private bool _hasTreePrefsEnabled;
		///<summary>The idxSelectedInMount when it is copied.</summary>
		int _idxDocToCopy=-1;
		///<summary>The index of the currently selected item within a mount.</summary>
		private int _idxSelectedInMount=0;
		///<summary>Edited by the main thread to reflect selection changes. Read by worker thread.</summary>
		private EnumImageNodeType _imageNodeTypeForSettings;
		///<summary>Set by the main thread and read by the image worker thread. Specifies which image processing tasks are to be performed by the worker thread.</summary>
		private ImageSettingFlags _imageSettingFlagsForSettings=ImageSettingFlags.NONE;
		private ImageSettingFlags _imageSettingFlagsInvalidated;
		private bool _initializedOnStartup;
		///<summary>Used to prevent concurrent access to the current images by multiple threads.  Each item in array corresponds to an image in a mount.</summary>
		private int[] _intArrayWidthsImagesCur=new int[1];
		///<summary>Used to prevent concurrent access to the current images by multiple threads.  Each item in array corresponds to an image in a mount.</summary>
		private int[] _intArrayHeightsImagesCur=new int[1];
		///<summary>Starts out as false. It's only used when repainting the toolbar, not to test mode.</summary>
		private bool _isCropMode;
		/// <summary>Keep track of if image module is being refreshed so we know when to query the images again and refill the list.</summary>
		private bool _isFillingXVWebFromThread=true;
		private bool _isMouseDown;
		///<summary>Set to true when the image in the picture box is currently being translated.</summary>
		private bool _isDragging;
		/// <summary>Copy of the image information that was recieved. Needed to we can refresh the image module and not have to query again.</summary>
		private List<ApteryxImage>_listImageDownload;
		///<summary>A list of primary keys of the ImageNodeIds that should be expanded when the image module is loaded.</summary>
		private List<long> _listExpandedCats=new List<long>();
		///<summary>If a mount is currently selected, this is the list of the mount items on it.</summary>
		private List<MountItem> _listMountItemsForSelected=null;
		///<summary>Keeps track of which image categories are currently expanded.</summary>
		private List<UserOdPref> _listUserOdPrefImageCats=null;
		//<summary>Keeps track of the mount settings for the currently selected mount document.</summary>
		//private Mount MountForSettings=new Mount();
		///<summary>Keeps track of the currently selected mount object (only when a mount is selected).</summary>
		private Mount _mountSelected=new Mount();
		///<summary>Used to perform mouse selections in the treeDocuments list.</summary>
		private ImageNodeTag _nodeIdentifierDown;
		///<summary>Used to keep track of the old document selection by document number (the only guaranteed unique idenifier). This is to help the code be compatible with both Windows and MONO.</summary>
		private ImageNodeTag _nodeIdentifierOld;
		///<summary></summary>
		private Patient _patCur;
		///<summary>Set with each module refresh, and that's where it's set if it doesn't yet exist.  For now, we are not using ImageStore.GetPatientFolder(), because we haven't tested whether it properly updates the patient object.  We don't want to risk using an outdated patient folder path.  And we don't want to waste time refreshing PatCur after every ImageStore.GetPatientFolder().</summary>
		private string _patFolder;
		///<summary>Gets updated to PatCur.PatNum that the last security log was made with so that we don't make too many security logs for this patient.  When _patNumLast no longer matches PatCur.PatNum (e.g. switched to a different patient within a module), a security log will be entered.  Gets reset (cleared and the set back to PatCur.PatNum) any time a module button is clicked which will cause another security log to be entered.</summary>
		private long _patNumLast;
		private long _patNumPrev=0;
		///<summary>When dragging on Picturebox, this is the starting point in PictureBox coordinates.</summary>
		private Point _pointMouseDown;
		///<summary>Used as a basis for calculating image translations.</summary>
		private PointF _pointImageCur;
		///<summary>The true offset of the document image or mount image.</summary>
		private PointF _pointTranslation;
		private Rectangle _rectCrop=new Rectangle(0,0,-1,-1);
		///<summary>Used to display Topaz signatures on Windows. Is added dynamically to avoid native code references crashing MONO.</summary>
		private Control _sigBoxTopaz;
		///<summary>Used for performing an xRay image capture on an imaging device.</summary>
		private SuniDeviceControl _suniDeviceControl=null;
		///<summary>Used to download images from Apterxy</summary>
		private ODThread _threadImageRequest;
		///<summary>Thread to handle updating the graphical image to the screen when the current document is an image.</summary>
		private Thread _threadImageUpdate=null;
		/// <summary>Tracks the last user to load ContrImages</summary>
		private long _userNumPrev=-1;
		private WebBrowser _webBrowserDocument=null;
		///<summary>The location of the file that <see cref="_webBrowserDocument" /> has navigated to.</summary>
		private string _webBrowserFilePath=null;
		///<summary>The current zoom of the currently loaded image/mount. 1 implies normal size, less than 1 implies the image is shrunk, greater than 1 imples the image/mount is blown-up.</summary>
		private float _zoomImage=1;
		///<summary>The zoom level is 0 after the current image/mount is loaded.  User changes the zoom in integer increments.  ZoomOverall is then (initial image/mount zoom)*(2^ZoomLevel).</summary>
		private int _zoomLevel=0;
		///<summary>Represents the current factor for level of zoom from the initial zoom of the currently loaded image/mount. This is calculated directly as 2^ZoomLevel every time a zoom occurs. Recalculated from ZoomLevel each time, so that ZoomOverall always hits the exact same values for the exact same zoom levels (no loss of data).</summary>
		private float _zoomOverall=1;
		#endregion Fields - Private

		#region Constructor
		///<summary></summary>
		public ContrImages() {
			_zoomImage=1;
			Logger.openlog.Log("Initializing Document Module...",Logger.Severity.INFO);
			InitializeComponent();
			//The context menu causes strange bugs in MONO when performing selections on the tree.
			//Perhaps when MONO is more developed we can remove this check.
			//Also, the SigPlusNet() object cannot be instantiated on 64-bit machines, because
			//the code for instantiation exists in a 32-bit native dll. Therefore, we have put
			//the creation code for the topaz box in TopazWrapper.GetTopaz() so that
			//the native code does not exist or get called anywhere in the program unless we are running on a 
			//32-bit version of Windows.
			//bool is64=CodeBase.ODEnvironment.Is64BitOperatingSystem();
			bool platformUnix=Environment.OSVersion.Platform==PlatformID.Unix;
			//allowTopaz=(!platformUnix && !is64);
			if(platformUnix) {
				treeDocuments.ContextMenu=null;
			}
			//if(allowTopaz){//Windows OS
			try {
				_sigBoxTopaz=TopazWrapper.GetTopaz();
				panelNote.Controls.Add(_sigBoxTopaz);
				_sigBoxTopaz.Location=sigBox.Location;//new System.Drawing.Point(437,15);
				_sigBoxTopaz.Name="sigBoxTopaz";
				_sigBoxTopaz.Size=new System.Drawing.Size(362,79);
				_sigBoxTopaz.TabIndex=93;
				_sigBoxTopaz.Text="sigPlusNET1";
				_sigBoxTopaz.DoubleClick+=new System.EventHandler(this.sigBoxTopaz_DoubleClick);
				TopazWrapper.SetTopazState(_sigBoxTopaz,0);
			}
			catch { }
			//}
			//We always capture with a Suni device for now.
			//TODO: In the future use a device locator in the xImagingDeviceManager
			//project to return the appropriate general device control.
			_suniDeviceControl=new SuniDeviceControl();
			this._suniDeviceControl.OnCaptureReady+=new System.EventHandler(this.CaptureReady);
			this._suniDeviceControl.OnCaptureComplete+=new System.EventHandler(this.CaptureComplete);
			this._suniDeviceControl.OnCaptureFinalize+=new System.EventHandler(this.CaptureFinalize);
			Logger.openlog.Log("Document Module initialization complete.",Logger.Severity.INFO);
		}
		#endregion Constructor

		#region Delegates
		///<summary>Used to safe-guard against multi-threading issues when an image capture is completed.</summary>
		private delegate void CaptureCallback(object sender,EventArgs e);

		///<summary>Used to protect against multi-threading issues when refreshing a mount during an image capture.</summary>
		private delegate void InvalidateSettingsCallback(ImageSettingFlags settings,bool reloadZoomTransCrop);

		///<summary>Used for Invoke() calls in RenderCurrentImage() to safely handle multi-thread access to the picture box.</summary>
		private delegate void RenderImageCallback(Document docCopy,int originalWidth,int originalHeight,float zoom,PointF translation);

		private delegate void ToolBarClick();
		#endregion Delegates

		#region Events - Raise
		///<summary></summary>
		[Category("Action"),Description("Occurs when the close button is clicked in the toolbar.")]
		public event EventHandler CloseClick=null;
		#endregion Events - Raise

		#region Events - TreeDocuments
		private void TreeDocuments_AfterCollapse(object sender,TreeViewEventArgs e) {
			ImageNodeTag nodeCur=(ImageNodeTag)e.Node.Tag;
			_listExpandedCats.RemoveAll(x => x==nodeCur.PriKey);
			UpdateUserOdPrefForImageCat(nodeCur.PriKey,false);
		}

		private void TreeDocuments_AfterExpand(object sender,TreeViewEventArgs e) {
			ImageNodeTag nodeCur=(ImageNodeTag)e.Node.Tag;
			if(!_listExpandedCats.Contains(nodeCur.PriKey)) {
				_listExpandedCats.Add(nodeCur.PriKey);
			}
			UpdateUserOdPrefForImageCat(nodeCur.PriKey,true);
		}

		private void treeDocuments_DragDrop(object sender,DragEventArgs e) {
			TreeNode nodeOver=treeDocuments.GetNodeAt(treeDocuments.PointToClient(Cursor.Position));
			if(nodeOver==null) {
				return;
			}
			ImageNodeTag nodeOverId=(ImageNodeTag)nodeOver.Tag;
			long nodeOverCategoryDefNum=0;
			if(nodeOverId.NodeType==EnumImageNodeType.Category) {
				nodeOverCategoryDefNum=Defs.GetDefsForCategory(DefCat.ImageCats,true)[nodeOver.Index].DefNum;
			}
			else {
				nodeOverCategoryDefNum=Defs.GetDefsForCategory(DefCat.ImageCats,true)[nodeOver.Parent.Index].DefNum;
			}
			Document docSave=new Document();
			ImageNodeTag nodeId=new ImageNodeTag();
			string[] arrayFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
			string errorMessage="";
			for(int i=0;i<arrayFiles.Length;i++) {
				string draggedFilePath=arrayFiles[i];
				string fileName=draggedFilePath.Substring(draggedFilePath.LastIndexOf("\\")+1);
				if(Directory.Exists(draggedFilePath)) {
					errorMessage+="\r\n"+fileName;
					continue;
				}
				docSave=ImageStore.Import(draggedFilePath,nodeOverCategoryDefNum,_patCur);
				FillDocList(false);
				SelectTreeNode(GetNodeById(MakeIdDoc(docSave.DocNum)));
				FormDocInfo FormD=new FormDocInfo(_patCur,docSave,GetCurrentFolderName(treeDocuments.SelectedNode));
				FormD.ShowDialog(this);//some of the fields might get changed, but not the filename
				if(FormD.DialogResult!=DialogResult.OK) {
					DeleteSelection(false,false,docSave);
				}
				else {
					nodeId=MakeIdDoc(docSave.DocNum);
					_documentSelected=docSave.Copy();
				}
			}
			if(docSave!=null && !MakeIdDoc(docSave.DocNum).Equals(nodeId)) {
				SelectTreeNode(GetNodeById(MakeIdDoc(docSave.DocNum)));
			}
			FillDocList(true);
			if(errorMessage!="") {
				MessageBox.Show(Lan.g(this,"The following items are directories and were not copied into the images folder for this patient.")+errorMessage);
			}
		}

		private void treeDocuments_DragEnter(object sender,DragEventArgs e) {
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect=DragDropEffects.Copy;//Fills the DragEventArgs for DragDrop
			}
		}

		private void TreeDocuments_MouseDoubleClick(object sender,MouseEventArgs e) {
			if(_webBrowserDocument!=null) {
				//This prevents users from previewing the PDF in OD at the same time they have it open in an external PDF viewer.
				//There was a strange graphical bug that occurred when the PDF was previewed at the same time the PDF was open in the Adobe Acrobat Reader DC
				//if the Adobe "Enable Protected Mode" option was disabled.  The graphical bug caused many ODButtons and ODGrids to disappear even though their
				//Visible flags were set to true.  Somehow the WndProc() for the form which owned these controls was not calling the OnPaint() method.
				//Thus the bug affected many custom drawn controls.
				_webBrowserDocument.Dispose();
				_webBrowserDocument=null;
			}
			TreeNode clickedNode=treeDocuments.GetNodeAt(e.Location);
			if(clickedNode==null) {
				return;
			}
			ImageNodeTag nodeId=(ImageNodeTag)clickedNode.Tag;
			if(nodeId.NodeType==EnumImageNodeType.None) {
				return;
			}
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
				MsgBox.Show(this,"Images stored directly in database. Export file in order to open with external program.");
				return;//Documents must be stored in the A to Z Folder to open them outside of Open Dental.  Users can use the export button for now.
			}
			if(nodeId.NodeType==EnumImageNodeType.Mount) {
				FormMountEdit fme=new FormMountEdit(_mountSelected);
				fme.ShowDialog();//Edits the MountSelected object directly and updates and changes to the database as well.
				FillDocList(true);//Refresh tree in case description for the mount changed.
				return;
			}
			else if(nodeId.NodeType==EnumImageNodeType.Doc) {
				Document nodeDoc=Documents.GetByNum(nodeId.PriKey);
				string ext=ImageStore.GetExtension(nodeDoc);
				if(ext==".jpg" || ext==".jpeg" || ext==".gif") {
					return;
				}
				//We allow anything which ends with a different extention to be viewed in the windows fax viewer.
				//Specifically, multi-page faxes can be viewed more easily by one of our customers using the fax
				//viewer. On Unix systems, it is imagined that an equivalent viewer will launch to allow the image
				//to be viewed.
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
					if(ODBuild.IsWeb()) {
						string tempFile=ImageStore.GetFilePath(nodeDoc,_patFolder);
						ThinfinityUtils.HandleFile(tempFile);
					}
					else {
						try {
							Process.Start(ImageStore.GetFilePath(nodeDoc,_patFolder));
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
						}
					}
				}
				else {//Cloud
					//Download document into temp directory for displaying.
					FormProgress FormP=new FormProgress();
					FormP.DisplayText="Downloading Document...";
					FormP.NumberFormat="F";
					FormP.NumberMultiplication=1;
					FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
					FormP.TickMS=1000;
					OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.DownloadAsync(_patFolder.Replace("\\","/")
						,nodeDoc.FileName
						,new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
					FormP.ShowDialog();
					if(FormP.DialogResult==DialogResult.Cancel) {
						state.DoCancel=true;
					}
					else {
						string tempFile=PrefC.GetRandomTempFile(Path.GetExtension(nodeDoc.FileName));
						File.WriteAllBytes(tempFile,state.FileContent);
						if(ODBuild.IsWeb()) {
							ThinfinityUtils.HandleFile(tempFile);
						}
						else {
							Process.Start(tempFile);
						}
					}
				}
			}
		}

		///<summary></summary>
		private void TreeDocuments_MouseDown(object sender,MouseEventArgs e) {
			_nodeIdentifierDown=new ImageNodeTag();
			TreeNode nodeOver=treeDocuments.GetNodeAt(e.Location);
			if(nodeOver==null) {
				return;
			}
			ImageNodeTag nodeIdDown=(ImageNodeTag)nodeOver.Tag;
			if(nodeIdDown.NodeType==EnumImageNodeType.Doc
				|| nodeIdDown.NodeType==EnumImageNodeType.Mount) {
				//These are the only types that can be dragged.
				_nodeIdentifierDown=nodeIdDown;
				_dateTimeMouseMoved=new DateTime(1,1,1);//For time delay. This will be set the moment the mouse actually starts moving
			}
			if(nodeIdDown.NodeType==EnumImageNodeType.Category) {
				nodeOver.SelectedImageIndex=nodeOver.ImageIndex;
			}
			//Always select the node on a mouse-down press for either right or left buttons.
			//If the left button is pressed, then the document is either being selected or dragged, so
			//setting the image at the beginning of the drag will either display the image as expected, or
			//automatically display the image while the document is being dragged (since it is in a different thread).
			//If the right button is pressed, then the user wants to view the properties of the image they are
			//clicking on, so displaying the image (in a different thread) will give the user a chance to view
			//the image corresponding to a delete, info display, etc...
			SelectTreeNode(nodeOver);
		}

		private void TreeDocuments_MouseLeave(object sender,EventArgs e) {
			treeDocuments.Cursor=Cursors.Default;
			_nodeIdentifierDown=new ImageNodeTag();
		}

		private void TreeDocuments_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(_nodeIdentifierDown.NodeType==EnumImageNodeType.None) {
				treeDocuments.Cursor=Cursors.Default;
				return;
			}
			TreeNode nodeOver=treeDocuments.GetNodeAt(e.Location);
			if(nodeOver==null) {//unknown malfunction
				treeDocuments.Cursor=Cursors.Default;
				return;
			}
			if(_nodeIdentifierDown.Equals((ImageNodeTag)nodeOver.Tag)) {//Over the original node
				treeDocuments.Cursor=Cursors.Default;
				return;
			}
			//Show drag
			//Cursor cursorDrag=new System.Windows.Forms.Cursor();
			treeDocuments.Cursor=Cursors.Hand;//need a better cursor than this
			if(_dateTimeMouseMoved.Year==1) {
				_dateTimeMouseMoved=DateTime.Now;
			}
		}

		///<summary></summary>
		private void TreeDocuments_MouseUp(object sender,System.Windows.Forms.MouseEventArgs e) {
			treeDocuments.Cursor=Cursors.Default;
			if(_nodeIdentifierDown.NodeType==EnumImageNodeType.None) {
				return;
			}
			if(e.Button!=MouseButtons.Left) {
				return;//Dragging can only happen with the left mouse button.
			}
			if(_dateTimeMouseMoved.Year==1) {//No valid mouse movements occurred after the mouse down event and before the mouse up event.
				//If the user moused down, then immediately moused up or moved slightly then moused up on the original source node before moving elsewhere.
				return;//Do not move the document.
				//This fixed a bug where users were able to accidentally move images while they were loading.  The user would mouse down, then mouse up
				//(to select the image), then start moving the mouse to do something else and the image would unexpectedly move to another image category.
				//NOTE: For some reason, if the user tries to move an image while it is loading, then the move action will be ignored, because the 
				//mouse events fire out of order (mouse down, then mouse up, then mouse move).  However, this seems like a minor issue, because
				//users typically intuitively know that certain commands are ignored while loading is in progress.  If this situation occurs to a user,
				//they will probably know that they need to try again.  The second time they try to move the image it will move, because once the image
				//is loaded, clicking on the image will not cause it to reload.  Also, the move will work the first time if the mouse up event happens
				//after loading is completed.  Therefore, this minor issue can only happen on very slow computers or very large images.
				//To fix the minor issue in the future, we might consider somehow forcing the mouse events to fire in order, even when images are loading.
			}
			//TimeSpan timeSpanDrag=(TimeSpan)(DateTime.Now-TimeMouseMoved);
			//if(timeSpanDrag.Milliseconds < 200) { //js 3/31/2012. Was 250
			//	return;//Too short of a drag and drop.  Probably human error
			//}
			TreeNode nodeOver=treeDocuments.GetNodeAt(e.Location);
			if(nodeOver==null) {
				return;
			}
			ImageNodeTag nodeOverId=(ImageNodeTag)nodeOver.Tag;
			long nodeOverCategoryDefNum=0;
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			if(nodeOverId.NodeType==EnumImageNodeType.Category) {
				nodeOverCategoryDefNum=listDefs[nodeOver.Index].DefNum;
			}
			else {
				nodeOverCategoryDefNum=listDefs[nodeOver.Parent.Index].DefNum;
			}
			TreeNode nodeOriginal=GetNodeById(_nodeIdentifierDown);
			long nodeOriginalCategoryDefNum=0;
			if(_nodeIdentifierDown.NodeType==EnumImageNodeType.Category) {
				nodeOriginalCategoryDefNum=listDefs[nodeOriginal.Index].DefNum;
			}
			else {
				nodeOriginalCategoryDefNum=listDefs[nodeOriginal.Parent.Index].DefNum;
			}
			if(nodeOverCategoryDefNum==nodeOriginalCategoryDefNum) {
				return;//category hasn't changed
			}
			if(_nodeIdentifierDown.NodeType==EnumImageNodeType.Mount) {
				Mount mount=Mounts.GetByNum(_nodeIdentifierDown.PriKey);
				string mountSourceCat=Defs.GetDef(DefCat.ImageCats,mount.DocCategory).ItemName;
				string mountDestCat=Defs.GetDef(DefCat.ImageCats,nodeOverCategoryDefNum).ItemName;
				mount.DocCategory=nodeOverCategoryDefNum;
				SecurityLogs.MakeLogEntry(Permissions.ImageEdit,mount.PatNum,Lan.g(this,"Mount moved from")+" "+mountSourceCat+" "
					+Lan.g(this,"to")+" "+mountDestCat);
				Mounts.Update(mount);
			}
			else if(_nodeIdentifierDown.NodeType==EnumImageNodeType.Doc) {
				Document doc=Documents.GetByNum(_nodeIdentifierDown.PriKey);
				string docSourceCat=Defs.GetDef(DefCat.ImageCats,doc.DocCategory).ItemName;
				string docDestCat=Defs.GetDef(DefCat.ImageCats,nodeOverCategoryDefNum).ItemName;
				doc.DocCategory=nodeOverCategoryDefNum;
				string logText=Lan.g(this,"Document moved")+": "+doc.FileName;
				if(doc.Description!="") {
					string docDescript=doc.Description;
					if(docDescript.Length>50) {
						docDescript=docDescript.Substring(0,50);
					}
					logText+=" "+Lan.g(this,"with description")+" "+docDescript;
				}
				logText+=" "+Lan.g(this,"from category")+" "+docSourceCat+" "+Lan.g(this,"to category")+" "+docDestCat;
				SecurityLogs.MakeLogEntry(Permissions.ImageEdit,doc.PatNum,logText,doc.DocNum,doc.DateTStamp);
				Documents.Update(doc);
			}
			FillDocList(true);
			_nodeIdentifierDown=new ImageNodeTag();
		}
		#endregion Events - TreeDocuments

		#region Events
		private void brightnessContrastSlider_Scroll(object sender,EventArgs e) {
			if(_documentSelected==null) {
				return;
			}
			_documentSelected.WindowingMin=sliderBrightnessContrast.MinVal;
			_documentSelected.WindowingMax=sliderBrightnessContrast.MaxVal;
			InvalidateSettings(ImageSettingFlags.COLORFUNCTION,false);
		}

		private void brightnessContrastSlider_ScrollComplete(object sender,EventArgs e) {
			if(_documentSelected==null) {
				return;
			}
			Documents.Update(_documentSelected);
			DeleteThumbnailImage(_documentSelected);
			InvalidateSettings(ImageSettingFlags.COLORFUNCTION,false);
		}

		///<summary>Called on successful capture of image.</summary>
		private void CaptureComplete(object sender,EventArgs e) {
			if(this.InvokeRequired) {
				CaptureCallback c=new CaptureCallback(CaptureComplete);
				Invoke(c,new object[] { sender,e });
				return;
			}
			if(_idxSelectedInMount<0 || _documentArrayDocsInMount[_idxSelectedInMount]!=null) {//Mount is full.
				_suniDeviceControl.KillXRayThread();
				return;
			}
			//Depending on the device being captured from, we need to rotate the images returned from the device by a certain
			//angle, and we need to place the returned images in a specific order within the mount slots. Later, we will allow
			//the user to define the rotations and slot orders, but for now, they will be hard-coded.
			short rotationAngle=0;
			switch(_idxSelectedInMount) {
				case (0):
					rotationAngle=90;
					break;
				case (1):
					rotationAngle=90;
					break;
				case (2):
					rotationAngle=270;
					break;
				default://3
					rotationAngle=270;
					break;
			}
			//Create the document object in the database for this mount image.
			Bitmap capturedImage=_suniDeviceControl.capturedImage;
			Document doc=ImageStore.ImportImageToMount(capturedImage,rotationAngle,_listMountItemsForSelected[_idxSelectedInMount].MountItemNum,GetCurrentCategory(),_patCur);
			_bitmapArrayImagesCur[_idxSelectedInMount]=capturedImage;
			_intArrayWidthsImagesCur[_idxSelectedInMount]=capturedImage.Width;
			_intArrayHeightsImagesCur[_idxSelectedInMount]=capturedImage.Height;
			_documentArrayDocsInMount[_idxSelectedInMount]=doc;
			_documentSelected=doc;
			SetBrightnessAndContrast();
			//Refresh image in in picture box.
			InvalidateSettings(ImageSettingFlags.ALL,false);
			//This capture was successful. Keep capturing more images until the capture is manually aborted.
			//This will cause calls to OnCaptureBegin(), then OnCaptureFinalize().
			_suniDeviceControl.CaptureXRay();
		}

		///<summary>Called when the entire sequence of image captures is complete (possibly because of failure, or a full mount among other things).</summary>
		private void CaptureFinalize(object sender,EventArgs e) {
			if(this.InvokeRequired) {
				CaptureCallback c=new CaptureCallback(CaptureFinalize);
				Invoke(c,new object[] { sender,e });
				return;
			}
			ToolBarMain.Buttons["Capture"].Pushed=false;
			ToolBarMain.Invalidate();
			EnableAllTools(true);
			if(_idxSelectedInMount>0 && _documentArrayDocsInMount[_idxSelectedInMount]!=null) {//The capture finished in a state where a mount item is selected.
				EnableTreeItemTools(true,true,false,true,false,true,false,true,true,true,true,true,true,true);
			}
			else {//The capture finished without a mount item selected (so the mount itself is considered to be selected).
				EnableTreeItemTools(true,true,true,true,false,false,false,true,true,true,false,false,false,true);
			}
		}

		///<summary>Called when the image capture device is ready for exposure.</summary>
		private void CaptureReady(object sender,EventArgs e) {
			GetNextUnusedMountItem();
			InvalidateSettings(ImageSettingFlags.NONE,false);//Refresh the selection box change (does not do any image processing here).
		}

		private void ContrImages_Resize(object sender,EventArgs e) {
			ResizeAll();
		}

		private void label1_DoubleClick(object sender,EventArgs e) {
			ToolBarSign_Click();
		}

		private void label15_DoubleClick(object sender,EventArgs e) {
			ToolBarSign_Click();
		}

		private void labelInvalidSig_DoubleClick(object sender,EventArgs e) {
			ToolBarSign_Click();
		}

		private void menuTree_Click(object sender,System.EventArgs e) {
			if(treeDocuments.SelectedNode==null) {
				return;//Probably the user has no patient selected
			}
			switch(((MenuItem)sender).Index) {
				case 0://print
					ToolBarPrint_Click();
					break;
				case 1://delete
					ToolBarDelete_Click();
					break;
				case 2://info
					ToolBarInfo_Click();
					break;
			}
		}

		private void menuForms_Click(object sender,System.EventArgs e) {
			string formName = ((MenuItem)sender).Text;
			bool copied = true;
			Document doc = null;
			try {
				doc=ImageStore.ImportForm(formName,GetCurrentCategory(),_patCur);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				copied=false;
			}
			if(copied) {
				FillDocList(false);
				SelectTreeNode(GetNodeById(MakeIdDoc(doc.DocNum)));
				FormDocInfo FormD=new FormDocInfo(_patCur,doc,GetCurrentFolderName(treeDocuments.SelectedNode));
				FormD.ShowDialog(this);//some of the fields might get changed, but not the filename
				if(FormD.DialogResult!=DialogResult.OK) {
					DeleteSelection(false,false,doc);
				}
				else {
					FillDocList(true);//Refresh possible changes in the document due to FormD.
				}
			}
		}

		private void paintTools_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)) {
				switch(e.Button.Tag.ToString()) {
					case "Crop":
						ToolBarCrop_Click();
						break;
					case "Hand":
						ToolBarHand_Click();
						break;
					case "ZoomIn":
						ToolBarZoomIn_Click();
						break;
					case "ZoomOut":
						ToolBarZoomOut_Click();
						break;
					case "Zoom100":
						ToolBarZoom100_Click();
						break;
					case "Flip":
						ToolBarFlip_Click();
						break;
					case "RotateL":
						ToolBarRotateL_Click();
						break;
					case "RotateR":
						ToolBarRotateR_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,_patCur);
			}
		}

		private void panelNote_DoubleClick(object sender,EventArgs e) {
			ToolBarSign_Click();
		}

		///<summary></summary>
		private void PictureBox1_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			_pointMouseDown=new Point(e.X,e.Y);
			_isMouseDown=true;
			_pointImageCur=new PointF(_pointTranslation.X,_pointTranslation.Y);
		}

		private void PictureBox1_MouseHover(object sender,EventArgs e) {
			if(ToolBarPaint.Buttons["Hand"]==null) {
				pictureBoxMain.Cursor=Cursors.Hand;
				return;
			}
			if(ToolBarPaint.Buttons["Hand"].Pushed) {//Hand mode.
				pictureBoxMain.Cursor=Cursors.Hand;
			}
			else {
				pictureBoxMain.Cursor=Cursors.Default;
			}
		}

		private void PictureBox1_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(!_isMouseDown) {
				return;
			}
			_isDragging=true;
			if(treeDocuments.SelectedNode==null) {
				return;
			}
			if(((ImageNodeTag)treeDocuments.SelectedNode.Tag).NodeType==EnumImageNodeType.None) {
				return;
			}
			if(ToolBarPaint.Buttons["Hand"]==null//when hand button is not visible, it's always hand mode
				|| ToolBarPaint.Buttons["Hand"].Pushed)//Hand mode.
			{
				_pointTranslation=new PointF(_pointImageCur.X+(e.Location.X-_pointMouseDown.X),_pointImageCur.Y+(e.Location.Y-_pointMouseDown.Y));
			}
			else if(ToolBarPaint.Buttons["Crop"]!=null && ToolBarPaint.Buttons["Crop"].Pushed) {
				float[] intersect=ODMathLib.IntersectRectangles(Math.Min(e.Location.X,_pointMouseDown.X),
					Math.Min(e.Location.Y,_pointMouseDown.Y),Math.Abs(e.Location.X-_pointMouseDown.X),
					Math.Abs(e.Location.Y-_pointMouseDown.Y),pictureBoxMain.ClientRectangle.X,pictureBoxMain.ClientRectangle.Y,
					pictureBoxMain.ClientRectangle.Width-1,pictureBoxMain.ClientRectangle.Height-1);
				if(intersect.Length<0) {
					_rectCrop=new Rectangle(0,0,-1,-1);
				}
				else {
					_rectCrop=new Rectangle((int)intersect[0],(int)intersect[1],(int)intersect[2],(int)intersect[3]);
				}
			}
			InvalidateSettings(ImageSettingFlags.NONE,false);//Refresh display.
		}

		private void PictureBox1_MouseUp(object sender,System.Windows.Forms.MouseEventArgs e) {
			bool wasDragging=_isDragging;
			_isMouseDown=false;
			_isDragging=false;
			if(treeDocuments.SelectedNode==null) {
				return;
			}
			ImageNodeTag nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
			if(nodeId.NodeType==EnumImageNodeType.None) {
				return;
			}
			if(ToolBarPaint.Buttons["Hand"]==null//if button is not visible, it's always hand mode.
				|| ToolBarPaint.Buttons["Hand"].Pushed) {
				if(e.Button!=MouseButtons.Left || wasDragging) {
					return;
				}
				if(nodeId.NodeType==EnumImageNodeType.Mount) {//The user may be trying to select an individual image within the current mount.
					_idxSelectedInMount=GetIdxAtMountLocation(_pointMouseDown);
					//Assume no item will be selected and enable tools again if an item was actually selected.
					EnableTreeItemTools(true,true,true,true,false,false,false,true,true,true,false,false,false,true);
					for(int j=0;j<_listMountItemsForSelected.Count;j++) {
						if(_listMountItemsForSelected[j].OrdinalPos==_idxSelectedInMount) {
							if(_documentArrayDocsInMount[j]!=null) {
								_documentSelected=_documentArrayDocsInMount[j];
								SetBrightnessAndContrast();
								EnableTreeItemTools(true,true,false,true,false,true,false,true,true,true,true,true,true,true);
							}
						}
					}
					ToolBarPaint.Invalidate();
					if(_idxSelectedInMount<0) {//The current selection was unselected.
						_suniDeviceControl.KillXRayThread();//Stop xray capture, because it relies on the current selection to place images.
					}
					InvalidateSettings(ImageSettingFlags.ALL,false);
				}
			}
			else {//crop mode
				if(_rectCrop.Width<=0 || _rectCrop.Height<=0) {
					return;
				}
				if(!MsgBox.Show(this,true,"Crop to Rectangle?")) {
					_rectCrop=new Rectangle(0,0,-1,-1);
					InvalidateSettings(ImageSettingFlags.NONE,false);//Refresh display (since message box was covering).
					return;
				}
				float cropZoom=_zoomImage*_zoomOverall;
				PointF cropTrans=_pointTranslation;
				PointF cropPoint1=ScreenPointToUnalteredDocumentPoint(_rectCrop.Location,_documentSelected,
					_intArrayWidthsImagesCur[_idxSelectedInMount],_intArrayHeightsImagesCur[_idxSelectedInMount],cropZoom,cropTrans);
				PointF cropPoint2=ScreenPointToUnalteredDocumentPoint(new Point(_rectCrop.Location.X+_rectCrop.Width,
					_rectCrop.Location.Y+_rectCrop.Height),_documentSelected,_intArrayWidthsImagesCur[_idxSelectedInMount],_intArrayHeightsImagesCur[_idxSelectedInMount],
					cropZoom,cropTrans);
				//cropPoint1 and cropPoint2 together define an axis-aligned bounding area, or our crop area. 
				//However, the two points have no guaranteed order, thus we must sort them using Math.Min.
				Rectangle rawCropRect=new Rectangle(
					(int)Math.Round((decimal)Math.Min(cropPoint1.X,cropPoint2.X)),
					(int)Math.Round((decimal)Math.Min(cropPoint1.Y,cropPoint2.Y)),
					(int)Math.Ceiling((decimal)Math.Abs(cropPoint1.X-cropPoint2.X)),
					(int)Math.Ceiling((decimal)Math.Abs(cropPoint1.Y-cropPoint2.Y)));
				//We must also intersect the old cropping rectangle with the new cropping rectangle, so that part of
				//the image does not come back as a result of multiple crops.
				Rectangle oldCropRect=DocCropRect(_documentSelected,_intArrayWidthsImagesCur[_idxSelectedInMount],_intArrayHeightsImagesCur[_idxSelectedInMount]);
				float[] finalCropRect=ODMathLib.IntersectRectangles(rawCropRect.X,rawCropRect.Y,rawCropRect.Width,
					rawCropRect.Height,oldCropRect.X,oldCropRect.Y,oldCropRect.Width,oldCropRect.Height);
				//Will return a null intersection when the user chooses a crop rectangle which is
				//entirely outside the current visible portion of the image. Can also return a zero-area rect,
				//if the entire image is cropped away.
				if(finalCropRect.Length!=4 || finalCropRect[2]<=0 || finalCropRect[3]<=0) {
					_rectCrop=new Rectangle(0,0,-1,-1);
					InvalidateSettings(ImageSettingFlags.NONE,false);//Refresh display (since message box was covering).
					return;
				}
				Rectangle prevCropRect=DocCropRect(_documentSelected,_intArrayWidthsImagesCur[_idxSelectedInMount],_intArrayHeightsImagesCur[_idxSelectedInMount]);
				_documentSelected.CropX=(int)finalCropRect[0];
				_documentSelected.CropY=(int)finalCropRect[1];
				_documentSelected.CropW=(int)Math.Ceiling(finalCropRect[2]);
				_documentSelected.CropH=(int)Math.Ceiling(finalCropRect[3]);
				Documents.Update(_documentSelected);
				if(nodeId.NodeType==EnumImageNodeType.Doc) {
					DeleteThumbnailImage(_documentSelected);
					Rectangle newCropRect=DocCropRect(_documentSelected,_intArrayWidthsImagesCur[_idxSelectedInMount],_intArrayHeightsImagesCur[_idxSelectedInMount]);
					//Update the location of the image so that the cropped portion of the image does not move in screen space.
					PointF prevCropCenter=new PointF(prevCropRect.X+prevCropRect.Width/2.0f,prevCropRect.Y+prevCropRect.Height/2.0f);
					PointF newCropCenter=new PointF(newCropRect.X+newCropRect.Width/2.0f,newCropRect.Y+newCropRect.Height/2.0f);
					PointF[] imageCropCenters=new PointF[] {
						prevCropCenter,
						newCropCenter
					};
					Matrix docMat=GetDocumentFlippedRotatedMatrix(_documentSelected);
					docMat.Scale(cropZoom,cropZoom);
					docMat.TransformPoints(imageCropCenters);
					_pointTranslation=new PointF(_pointTranslation.X+(imageCropCenters[1].X-imageCropCenters[0].X),
																			_pointTranslation.Y+(imageCropCenters[1].Y-imageCropCenters[0].Y));
				}
				_rectCrop=new Rectangle(0,0,-1,-1);
				InvalidateSettings(ImageSettingFlags.CROP,false);
			}
		}

		///<summary>Keeps the back buffer for the picture box to be the same in dimensions as the picture box itself.</summary>
		private void PictureBox1_SizeChanged(object sender,EventArgs e) {
			if(this.DesignMode) {
				return;
			}
			InvalidateSettings(ImageSettingFlags.NONE,false);//Refresh display.
		}

		private void printDocument_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			//Keep a local pointer to the ImageRenderingNow so that the print results cannot be messed up by the current rendering thread (by changing the ImageRenderingNow).
			if(_bitmapImageRenderingNow==null) {
				e.HasMorePages=false;
				return;
			}
			Bitmap bitmapCloned=(Bitmap)_bitmapImageRenderingNow.Clone();
			if(bitmapCloned.Width<1 || bitmapCloned.Height<1 || treeDocuments.SelectedNode==null || treeDocuments.SelectedNode.Tag==null) {
				bitmapCloned.Dispose();
				bitmapCloned=null;
				e.HasMorePages=false;
				return;
			}
			Bitmap bitmapPrint=null;
			ImageNodeTag nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
			if(nodeId.NodeType==EnumImageNodeType.Category) {
				bitmapCloned.Dispose();
				bitmapCloned=null;
				e.HasMorePages=false;
				return;
			}
			if(nodeId.NodeType==EnumImageNodeType.Mount) {
				if(_idxSelectedInMount>=0 && _documentArrayDocsInMount[_idxSelectedInMount]!=null) {//mount item only
					bitmapPrint=ImageHelper.ApplyDocumentSettingsToImage(_documentArrayDocsInMount[_idxSelectedInMount],_bitmapArrayImagesCur[_idxSelectedInMount],ImageSettingFlags.ALL);
				}
				else {//Entire mount. Individual images are already rendered onto mount with correct settings.
					bitmapPrint=bitmapCloned;
				}
			}
			else if(nodeId.NodeType==EnumImageNodeType.Doc) {
				//Crop and color function have already been applied to the render image, now do the rest.
				bitmapPrint=ImageHelper.ApplyDocumentSettingsToImage(Documents.GetByNum(nodeId.PriKey),bitmapCloned,ImageSettingFlags.FLIP | ImageSettingFlags.ROTATE);
			}
			else if(nodeId.NodeType==EnumImageNodeType.Eob) {
				bitmapPrint=(Bitmap)bitmapCloned.Clone();
			}
			else if(nodeId.NodeType==EnumImageNodeType.Amd) {
				bitmapPrint=(Bitmap)bitmapCloned.Clone();
			}
			RectangleF rectf=e.MarginBounds;
			float ratio=Math.Min(rectf.Width/(float)bitmapPrint.Width,rectf.Height/(float)bitmapPrint.Height);
			Graphics g=e.Graphics;
			g.InterpolationMode=InterpolationMode.HighQualityBicubic;
			g.CompositingQuality=CompositingQuality.HighQuality;
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.PixelOffsetMode=PixelOffsetMode.HighQuality;
			g.DrawImage(bitmapPrint,0,0,(int)(bitmapPrint.Width*ratio),(int)(bitmapPrint.Height*ratio));
			bitmapCloned.Dispose();
			bitmapCloned=null;
			bitmapPrint.Dispose();
			bitmapPrint=null;
			e.HasMorePages=false;
		}

		private void MountMenu_Opening(object sender,CancelEventArgs e) {
			if(treeDocuments.SelectedNode==null) {
				e.Cancel=true;
				return;
			}
			ImageNodeTag nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
			if(nodeId.NodeType!=EnumImageNodeType.Mount) {
				e.Cancel=true;
				return;//No mount is currently selected so cancel the menu.
			}
			_idxSelectedInMount=GetIdxAtMountLocation(_pointMouseDown);
			if(_idxSelectedInMount<0) {
				e.Cancel=true;
				return;//No mount item was clicked on, so cancel the menu.
			}
			IDataObject clipboard=null;
			try {
				clipboard=Clipboard.GetDataObject();
			}
			catch(Exception ex) {
				clipboard=null;
				ex.DoNothing();
			}
			MountMenu.Items.Clear();
			//Only show the copy option in the mount menu if the item in the mount selected contains an image.
			if(_documentArrayDocsInMount[_idxSelectedInMount]!=null) {
				MountMenu.Items.Add("Copy",null,new System.EventHandler(MountMenuCopy_Click));
			}
			//Only show the paste option in the menu if an item is currently on the clipboard.
			if(clipboard != null && clipboard.GetDataPresent(DataFormats.Bitmap)) {
				MountMenu.Items.Add("Paste",null,new System.EventHandler(MountMenuPaste_Click));
			}
			//Only show the swap item in the menu if the item on the clipboard exists in the current mount.
			if(_idxDocToCopy>=0 && _documentArrayDocsInMount[_idxSelectedInMount]!=null && _idxSelectedInMount!=_idxDocToCopy) {
				MountMenu.Items.Add("Swap",null,new System.EventHandler(MountMenuSwap_Click));
			}
			//Cancel the menu if no items have been added into it.
			if(MountMenu.Items.Count<1) {
				e.Cancel=true;
				return;
			}
			//Refresh the mount image, since the IdxSelectedInMount may have changed.
			InvalidateSettings(ImageSettingFlags.ALL,false);
		}

		private void MountMenuCopy_Click(object sender,EventArgs e) {
			ToolBarCopy_Click();
			_idxDocToCopy=_idxSelectedInMount;
		}

		private void MountMenuPaste_Click(object sender,EventArgs e) {
			ToolBarPaste_Click();
		}

		private void MountMenuSwap_Click(object sender,EventArgs e) {
			long mountItemNum=_documentArrayDocsInMount[_idxSelectedInMount].MountItemNum;
			_documentArrayDocsInMount[_idxSelectedInMount].MountItemNum=_documentArrayDocsInMount[_idxDocToCopy].MountItemNum;
			_documentArrayDocsInMount[_idxDocToCopy].MountItemNum=mountItemNum;
			Document doc=_documentArrayDocsInMount[_idxSelectedInMount];
			_documentArrayDocsInMount[_idxSelectedInMount]=_documentArrayDocsInMount[_idxDocToCopy];
			_documentArrayDocsInMount[_idxDocToCopy]=doc;
			MountItem mount=_listMountItemsForSelected[_idxSelectedInMount];
			_listMountItemsForSelected[_idxSelectedInMount]=_listMountItemsForSelected[_idxDocToCopy];
			_listMountItemsForSelected[_idxDocToCopy]=mount;
			Documents.Update(_documentArrayDocsInMount[_idxSelectedInMount]);
			Documents.Update(_documentArrayDocsInMount[_idxDocToCopy]);
			bool[] idxDocsToUpdate=new bool[_documentArrayDocsInMount.Length];
			idxDocsToUpdate[_idxSelectedInMount]=true;
			idxDocsToUpdate[_idxDocToCopy]=true;
			//Make it so that another swap cannot be done without first copying.
			_idxDocToCopy=-1;
			//Update the mount image to reflect the swapped images.
			InvalidateSettings(ImageSettingFlags.ALL,false,idxDocsToUpdate);
		}		

		private void sigBox_DoubleClick(object sender,EventArgs e) {
			ToolBarSign_Click();
		}

		private void sigBoxTopaz_DoubleClick(object sender,EventArgs e) {
			ToolBarSign_Click();
		}

		private void textNote_DoubleClick(object sender,EventArgs e) {
			ToolBarSign_Click();
		}

		private void textNote_MouseHover(object sender,EventArgs e) {
			textNote.Cursor=Cursors.IBeam;
		}

		private void ToolBarMain_ButtonClick(object sender,OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)) {
				switch(e.Button.Tag.ToString()) {
					case "Print":
						//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
						//when it comes from a toolbar click.
						//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
						ToolBarClick toolClick=ToolBarPrint_Click;
						this.BeginInvoke(toolClick);
						break;
					case "Delete":
						ToolBarDelete_Click();
						break;
					case "Info":
						ToolBarInfo_Click();
						break;
					case "Sign":
						ToolBarSign_Click();
						break;
					case "ScanDoc":
						ToolBarScan_Click("doc");
						break;
					case "ScanMultiDoc":
						ToolBarScanMulti_Click();
						break;
					case "ScanXRay":
						ToolBarScan_Click("xray");
						break;
					case "ScanPhoto":
						ToolBarScan_Click("photo");
						break;
					case "Import":
						ToolBarImport_Click();
						break;
					case "Export":
						ToolBarExport_Click();
						break;
					case "Copy":
						ToolBarCopy_Click();
						break;
					case "Paste":
						ToolBarPaste_Click();
						break;
					case "Forms":
						MsgBox.Show(this,"Use the dropdown list.  Add forms to the list by copying image files into your A-Z folder, Forms.  Restart the program to see newly added forms.");
						break;
					case "Capture":
						ToolBarCapture_Click();
						break;
					case "Close":
						ToolBarClose_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,_patCur);
			}
		}
		#endregion Events

		#region Methods - Public
		///<summary>doc may be null if eob.</summary>
		public static Rectangle DocCropRect(Document doc,int originalImageWidth,int originalImageHeight) {
			if(doc==null) {//no cropping
				return new Rectangle(0,0,originalImageWidth,originalImageHeight);
			}
			if(doc.CropW==0 && doc.CropH==0) {//Crop rectangles of 0 area are considered non-existant (i.e. no cropping).
				return new Rectangle(0,0,originalImageWidth,originalImageHeight);
			}
			return new Rectangle(doc.CropX,doc.CropY,doc.CropW,doc.CropH);
		}

		///<summary>Refreshes list from db, then fills the treeview.  Set keepSelection to true in order to keep the current selection active.</summary>
		public void FillDocList(bool keepSelection) {
			ImageNodeTag nodeIdSelection=new ImageNodeTag();
			if(keepSelection && treeDocuments.SelectedNode!=null) {
				nodeIdSelection=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
			}
			//(keepSelection?GetNodeIdentifier(treeDocuments.SelectedNode):"");
			//Clear current tree contents.
			treeDocuments.SelectedNode=null;
			treeDocuments.Nodes.Clear();
			if(_claimPaymentNum!=0) {
				List<EobAttach> listEobs=EobAttaches.Refresh(_claimPaymentNum);
				for(int i=0;i<listEobs.Count;i++) {
					TreeNode node=new TreeNode(listEobs[i].FileName);
					node.Tag=MakeIdEob(listEobs[i].EobAttachNum);
					node.ImageIndex=2;
					node.SelectedImageIndex=node.ImageIndex;//redundant?
					treeDocuments.Nodes.Add(node);
					if(((ImageNodeTag)node.Tag).Equals(nodeIdSelection)) {
						SelectTreeNode(node);
					}
				}
				return;
			}
			else if(_ehrAmendmentCur!=null) {
				if(_ehrAmendmentCur.FileName!=null && _ehrAmendmentCur.FileName!="") {
					TreeNode node=new TreeNode(_ehrAmendmentCur.FileName);
					node.Tag=MakeIdAmd(_ehrAmendmentCur.EhrAmendmentNum);
					node.ImageIndex=2;
					node.SelectedImageIndex=node.ImageIndex;//redundant?
					treeDocuments.Nodes.Add(node);
					if(((ImageNodeTag)node.Tag).Equals(nodeIdSelection)) {
						SelectTreeNode(node);
					}
				}
				return;
			}
			//the rest of this is for normal images module-------------------------------------------------------------------------------------------------
			if(_patCur==null) {
				return;
			}
			List<Def> listImageCatDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			//Add all predefined folder names to the tree.
			for(int i=0;i<listImageCatDefs.Count;i++) {
				treeDocuments.Nodes.Add(new TreeNode(listImageCatDefs[i].ItemName));
				treeDocuments.Nodes[i].Tag=MakeIdDef(listImageCatDefs[i].DefNum);
				if(listImageCatDefs[i].ItemValue.Contains("L")) { //Patient Portal Folder
					treeDocuments.Nodes[i].SelectedImageIndex=7;
					treeDocuments.Nodes[i].ImageIndex=7;
				}
				else {
					treeDocuments.Nodes[i].SelectedImageIndex=1;
					treeDocuments.Nodes[i].ImageIndex=1;
				}
			}
			//Add all relevant documents and mounts as stored in the database to the tree for the current patient.
			DataSet ds=Documents.RefreshForPatient(new string[] { _patCur.PatNum.ToString() });
			DataRowCollection rows=ds.Tables["DocumentList"].Rows;
			for(int i=0;i<rows.Count;i++) {
				TreeNode node=new TreeNode(rows[i]["description"].ToString());
				int parentFolder=PIn.Int(rows[i]["docFolder"].ToString());
				treeDocuments.Nodes[parentFolder].Nodes.Add(node);
				if(rows[i]["DocNum"].ToString()=="0") {//must be a mount
					node.Tag=MakeIdMount(PIn.Long(rows[i]["MountNum"].ToString()));
				}
				else {//doc
					node.Tag=MakeIdDoc(PIn.Long(rows[i]["DocNum"].ToString()));
				}
				node.ImageIndex=2+Convert.ToInt32(rows[i]["ImgType"].ToString());
				node.SelectedImageIndex=node.ImageIndex;
				if(((ImageNodeTag)node.Tag).Equals(nodeIdSelection)) {
					SelectTreeNode(node);
				}
			}
			if(PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)==0) {//Expand the document tree each time the Images module is visited
					treeDocuments.ExpandAll();//Invalidates tree too.
			}
			else if(PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)==1) {//Document tree collapses when patient changes
				TreeNode selectedNode=treeDocuments.SelectedNode;//Save the selection so we can reselect after collapsing.
				treeDocuments.CollapseAll();//Invalidates tree and clears selection too.
				treeDocuments.SelectedNode=selectedNode;//This will expand any category/folder nodes necessary to show the selection.
				if(_patNumPrev==_patCur.PatNum) {//Maintain previously expanded nodes when patient not changed.
					for(int i=0;i<_listExpandedCats.Count;i++) {
						for(int j=0;j<treeDocuments.Nodes.Count;j++) {//Enumerate the image categories.
							if(_listExpandedCats[i]==((ImageNodeTag)treeDocuments.Nodes[j].Tag).PriKey) {
								treeDocuments.Nodes[j].Expand();
								break;
							}
						}
					}
				}
				else {//Patient changed.
					_listExpandedCats.Clear();
				}
				_patNumPrev=_patCur.PatNum;
			}
			else {//Document tree folders persistent expand/collapse per user
				_hasTreePrefsEnabled=true;//Initialize flag so that we don't run into duplication of the UserOdPref overrides rows.
				if(_userNumPrev==Security.CurUser.UserNum) {//User has not changed.  Maintain expanded nodes.
					TreeNode selectedNode=treeDocuments.SelectedNode;//Save the selection so we can reselect after collapsing.
					treeDocuments.CollapseAll();//Invalidates tree and clears selection too.
					treeDocuments.SelectedNode=selectedNode;//This will expand any category/folder nodes necessary to show the selection.
					for(int i=0;i<_listExpandedCats.Count;i++) {
						for(int j=0;j<treeDocuments.Nodes.Count;j++) {//Enumerate the image categories.
							ImageNodeTag nodeIdCategory=(ImageNodeTag)treeDocuments.Nodes[j].Tag;//Get current tree document node.
							if(nodeIdCategory.PriKey==_listExpandedCats[i]){
								treeDocuments.Nodes[j].Expand();
								break;
							}
						}
					}
				}
				else {//User has changed.  Expand image categories based on user preference.
					_listExpandedCats.Clear();
					_listUserOdPrefImageCats=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.Definition);//Update override list.
					foreach(Def curDef in listImageCatDefs) {
						//Should only be one value with associated Fkey.
						UserOdPref userOdPrefTemp=_listUserOdPrefImageCats.FirstOrDefault(x => x.Fkey==curDef.DefNum);
						if(userOdPrefTemp!=null) {//User has a preference for this image category.
							if(!userOdPrefTemp.ValueString.Contains("E")) {//The user's preference is to collapse this category.
								continue;
							}
							for(int j=0;j<treeDocuments.Nodes.Count;j++) {//Enumerate the image categories.
								ImageNodeTag nodeIdCategory=(ImageNodeTag)treeDocuments.Nodes[j].Tag;//Get current tree document node.
								if(nodeIdCategory.PriKey==userOdPrefTemp.Fkey) {
									treeDocuments.Nodes[j].Expand();//Expand folder.
									break;
								}
							}
						}
						else {//User doesn't have a preference for this image category.
							if(!curDef.ItemValue.Contains("E")) {//The default preference is to collapse this category.
								continue;
							}
							for(int j=0;j<treeDocuments.Nodes.Count;j++) {//Enumerate the image categories.
								ImageNodeTag nodeIdCategory=(ImageNodeTag)treeDocuments.Nodes[j].Tag;//Get current tree document node.
								if(nodeIdCategory.PriKey==curDef.DefNum) {
									treeDocuments.Nodes[j].Expand();
									break;
								}
							}
						}
					}
				}
				_userNumPrev=Security.CurUser.UserNum;//Update the Previous user num.
				_hasTreePrefsEnabled=false;//Disable flag
			}
			if(XVWeb.IsDisplayingImagesInProgram && !_isFillingXVWebFromThread) {//list was already added if this is from module refresh
				FillTreeXVWebItems(_patCur.PatNum);
			}
		}

		///<summary>The screen matrix of the image is relative to the upper left of the image, but our calculations are from the center of the image (since the calculations are easier everywhere else if taken from the center). This function converts our calculation matrix into an equivalent screen matrix for display. Assumes document rotations are in 90 degree multiples.</summary>
		public static Matrix GetScreenMatrix(Document doc,int docOriginalImageWidth,int docOriginalImageHeight,float imageScale,PointF imageTranslation) {
			Matrix matrixDoc=GetDocumentFlippedRotatedMatrix(doc);
			matrixDoc.Scale(imageScale,imageScale);
			Rectangle rectCrop=DocCropRect(doc,docOriginalImageWidth,docOriginalImageHeight);
			//The screen matrix of a GDI image is always relative to the upper left hand corner of the image.
			PointF pointPreOrigin=new PointF(-rectCrop.Width/2.0f,-rectCrop.Height/2.0f);
			PointF[] pointsMatrixScreen=new PointF[]{
				pointPreOrigin,
				new PointF(pointPreOrigin.X+1 ,pointPreOrigin.Y  ),
				new PointF(pointPreOrigin.X		,pointPreOrigin.Y+1),
			};
			matrixDoc.TransformPoints(pointsMatrixScreen);
			Matrix matrixScreen=new Matrix(
				pointsMatrixScreen[1].X-pointsMatrixScreen[0].X,//X.X
				pointsMatrixScreen[1].Y-pointsMatrixScreen[0].Y,//X.Y
				pointsMatrixScreen[2].X-pointsMatrixScreen[0].X,//Y.X
				pointsMatrixScreen[2].Y-pointsMatrixScreen[0].Y,//Y.Y
				pointsMatrixScreen[0].X+imageTranslation.X,	//Dx
				pointsMatrixScreen[0].Y+imageTranslation.Y);	//Dy
			return matrixScreen;
		}

		///<summary>Also does LayoutToolBar.</summary>
		public void InitializeOnStartup() {
			if(_initializedOnStartup) {
				return;
			}
			_initializedOnStartup=true;
			_pointMouseDown=new Point();
			Lan.C(this,new System.Windows.Forms.Control[] {
				//this.button1,
			});
			LayoutToolBar();
			contextTree.MenuItems.Clear();
			contextTree.MenuItems.Add("Print",new System.EventHandler(menuTree_Click));
			contextTree.MenuItems.Add("Delete",new System.EventHandler(menuTree_Click));
			if(_claimPaymentNum==0 && _ehrAmendmentCur==null) {//not an eob and not an amendment
				contextTree.MenuItems.Add("Info",new System.EventHandler(menuTree_Click));
			}
		}

		///<summary>Toolbar Layout for Amendments</summary>
		public void LayoutAmendmentToolBar() {
			ToolBarMain.Buttons.Clear();
			ToolBarPaint.Buttons.Clear();
			ODToolBarButton button;
			ToolBarMain.Buttons.Add(new ODToolBarButton("",1,Lan.g(this,"Print"),"Print"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",2,Lan.g(this,"Delete"),"Delete"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.g(this,"Scan:"),-1,"","");
			button.Style=ODToolBarButtonStyle.Label;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton("",14,Lan.g(this,"Scan Document"),"ScanDoc"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",18,Lan.g(this,"Scan Multi-Page Document"),"ScanMultiDoc"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Import"),5,Lan.g(this,"Import From File"),"Import"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Export"),19,Lan.g(this,"Export To File"),"Export"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Copy"),17,Lan.g(this,"Copy displayed image to clipboard"),"Copy"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Paste"),6,Lan.g(this,"Paste From Clipboard"),"Paste"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,Lan.g(this,"Close window"),"Close"));
			ToolBarPaint.Buttons.Add(new ODToolBarButton("",8,Lan.g(this,"Zoom In"),"ZoomIn"));
			ToolBarPaint.Buttons.Add(new ODToolBarButton("",9,Lan.g(this,"Zoom Out"),"ZoomOut"));
			ToolBarPaint.Buttons.Add(new ODToolBarButton("100",-1,Lan.g(this,"Zoom 100"),"Zoom100"));
			ToolBarMain.Invalidate();
			ToolBarPaint.Invalidate();
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ToolBarPaint.Buttons.Clear();
			ODToolBarButton button;
			ToolBarMain.Buttons.Add(new ODToolBarButton("",1,Lan.g(this,"Print"),"Print"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",2,Lan.g(this,"Delete"),"Delete"));
			if(_claimPaymentNum==0) {
				ToolBarMain.Buttons.Add(new ODToolBarButton("",3,Lan.g(this,"Item Info"),"Info"));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Sign"),-1,Lan.g(this,"Sign this document"),"Sign"));
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.g(this,"Scan:"),-1,"","");
			button.Style=ODToolBarButtonStyle.Label;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton("",14,Lan.g(this,"Scan Document"),"ScanDoc"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",18,Lan.g(this,"Scan Multi-Page Document"),"ScanMultiDoc"));
			if(_claimPaymentNum==0) {
				ToolBarMain.Buttons.Add(new ODToolBarButton("",16,Lan.g(this,"Scan Radiograph"),"ScanXRay"));
				ToolBarMain.Buttons.Add(new ODToolBarButton("",15,Lan.g(this,"Scan Photo"),"ScanPhoto"));
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Import"),5,Lan.g(this,"Import From File"),"Import"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Export"),19,Lan.g(this,"Export To File"),"Export"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Copy"),17,Lan.g(this,"Copy displayed image to clipboard"),"Copy"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Paste"),6,Lan.g(this,"Paste From Clipboard"),"Paste"));
			if(_claimPaymentNum==0) {
				button=new ODToolBarButton(Lan.g(this,"Templates"),-1,"","Forms");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				menuForms=new ContextMenu();
				string formDir=FileAtoZ.CombinePaths(ImageStore.GetPreferredAtoZpath(),"Forms");
				if(CloudStorage.IsCloudStorage) {
					//Running this asynchronously to not slowdown start up time.
					ODThread odThreadTemplate=new ODThread((o) => {
						OpenDentalCloud.Core.TaskStateListFolders state=CloudStorage.ListFolderContents(formDir);
						foreach(string fileName in state.ListFolderPathsDisplay) {
							if(InvokeRequired) {
								Invoke((Action)delegate () {
									menuForms.MenuItems.Add(Path.GetFileName(fileName),new EventHandler(menuForms_Click));
								});
							}
						}
					});
					//Swallow all exceptions and allow thread to exit gracefully.
					odThreadTemplate.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception ex) => { }));
					odThreadTemplate.Start(true);
				}
				else {//Not cloud
					if(Directory.Exists(formDir)) {
						DirectoryInfo dirInfo=new DirectoryInfo(formDir);
						FileInfo[] fileInfos=dirInfo.GetFiles();
						for(int i=0;i<fileInfos.Length;i++) {
							if(Documents.IsAcceptableFileName(fileInfos[i].FullName)) {
								menuForms.MenuItems.Add(fileInfos[i].Name,new System.EventHandler(menuForms_Click));
							}
						}
					}
				}
				button.DropDownMenu=menuForms;
				ToolBarMain.Buttons.Add(button);
				button=new ODToolBarButton(Lan.g(this,"Capture"),-1,"Capture Image From Device","Capture");
				button.Style=ODToolBarButtonStyle.ToggleButton;
				ToolBarMain.Buttons.Add(button);
				//Program links:
				ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.ImagesModule);
			}
			else {//claimpayment
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,Lan.g(this,"Close window"),"Close"));
			}
			if(_claimPaymentNum==0) {
				button=new ODToolBarButton("",7,Lan.g(this,"Crop Tool"),"Crop");
				button.Style=ODToolBarButtonStyle.ToggleButton;
				button.Pushed=_isCropMode;
				ToolBarPaint.Buttons.Add(button);
				button=new ODToolBarButton("",10,Lan.g(this,"Hand Tool"),"Hand");
				button.Style=ODToolBarButtonStyle.ToggleButton;
				button.Pushed=!_isCropMode;
				ToolBarPaint.Buttons.Add(button);
				ToolBarPaint.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			}
			ToolBarPaint.Buttons.Add(new ODToolBarButton("",8,Lan.g(this,"Zoom In"),"ZoomIn"));
			ToolBarPaint.Buttons.Add(new ODToolBarButton("",9,Lan.g(this,"Zoom Out"),"ZoomOut"));
			ToolBarPaint.Buttons.Add(new ODToolBarButton("100",-1,Lan.g(this,"Zoom 100"),"Zoom100"));
			if(_claimPaymentNum==0) {
				ToolBarPaint.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				button=new ODToolBarButton(Lan.g(this,"Rotate:"),-1,"","");
				button.Style=ODToolBarButtonStyle.Label;
				ToolBarPaint.Buttons.Add(button);
				ToolBarPaint.Buttons.Add(new ODToolBarButton("",11,Lan.g(this,"Flip"),"Flip"));
				ToolBarPaint.Buttons.Add(new ODToolBarButton("",12,Lan.g(this,"Rotate Left"),"RotateL"));
				ToolBarPaint.Buttons.Add(new ODToolBarButton("",13,Lan.g(this,"Rotate Right"),"RotateR"));
			}
			ToolBarMain.Invalidate();
			ToolBarPaint.Invalidate();
			Plugins.HookAddCode(this,"ContrDocs.LayoutToolBar_end",_patCur);
		}

		///<summary>One of two overloads.</summary>
		public void ModuleSelected(long patNum) {
			ModuleSelected(patNum,0);
		}

		///<summary>This overload is needed when jumping to a specific image from FormPatientForms.</summary>
		public void ModuleSelected(long patNum,long docNum) {
			try {
				RefreshModuleData(patNum);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"Error accessing images."),ex);
			}
			RefreshModuleScreen();
			if(panelNote.Visible) {//Notes and sig box may have been visible previously, with info from another image/patient
				panelNote.Visible=false;
				ResizeAll();//Resize pictureboxmain to fit the whole screen
			}
			if(docNum!=0) {
				SelectTreeNode(GetNodeById(MakeIdDoc(docNum)));
			}
			Plugins.HookAddCode(this,"ContrImages.ModuleSelected_end",patNum,docNum);
		}

		///<summary>This overload is for amendment images.  Loads the one image for this amendment.</summary>
		public void ModuleSelectedAmendment(EhrAmendment amendment) {
			_ehrAmendmentCur=amendment;
			//Just in case this control has not been initialized yet.  Does not hurt to call multiple times.  Simply returns if already initilized.
			InitializeOnStartup();
			LayoutAmendmentToolBar();
			sliderBrightnessContrast.Visible=false;
			//ToolBarPaint.Location=new Point(pictureBoxMain.Left,ToolBarPaint.Top);//happens in ResizeAll().
			ResizeAll();
			//RefreshModuleData-----------------------------------------------------------------------
			SelectTreeNode(null);//Clear selection and image and reset visibilities.
			//PatFolder=ImageStore.GetPatientFolder(PatCur);//This is where the pat folder gets created if it does not yet exist.
			//RefreshModuleScreen---------------------------------------------------------------------
			EnableAllTools(true);
			EnableAllTreeItemTools(false);
			ToolBarMain.Invalidate();
			ToolBarPaint.Invalidate();
			FillDocList(false);
			if(treeDocuments.Nodes.Count>0) {
				SelectTreeNode(treeDocuments.Nodes[0]);
			}
		}

		///<summary>This overload is for batch claim payment (EOB) images.</summary>
		public void ModuleSelectedClaimPayment(long claimPaymentNum) {
			_claimPaymentNum=claimPaymentNum;
			//Just in case this control has not been initialized yet.  Does not hurt to call multiple times.  Simply returns if already initilized.
			InitializeOnStartup();
			LayoutToolBar();//again
			sliderBrightnessContrast.Visible=false;
			//ToolBarPaint.Location=new Point(pictureBoxMain.Left,ToolBarPaint.Top);//happens in ResizeAll().
			ResizeAll();
			//RefreshModuleData-----------------------------------------------------------------------
			SelectTreeNode(null);//Clear selection and image and reset visibilities.
			//PatFolder=ImageStore.GetPatientFolder(PatCur);//This is where the pat folder gets created if it does not yet exist.
			//RefreshModuleScreen---------------------------------------------------------------------
			EnableAllTools(true);
			EnableAllTreeItemTools(false);
			ToolBarMain.Invalidate();
			ToolBarPaint.Invalidate();
			FillDocList(false);
			if(treeDocuments.Nodes.Count>0) {
				SelectTreeNode(treeDocuments.Nodes[0]);
			}
			//SelectTreeNode(GetNodeById(MakeIdentifier(docNum.ToString(),"0")));
		}

		///<summary></summary>
		public void ModuleUnselected() {
			_familyCur=null;
			foreach(Control c in this.Controls) {
				if(c.GetType()==typeof(WebBrowser)) {//_webBrowserDocument
					Controls.Remove(c);
					c.Dispose();
				}
			}
			//Cancel current image capture.
			_suniDeviceControl.KillXRayThread();
			_patNumLast=0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
			Plugins.HookAddCode(this,"ContrImages.ModuleUnselected_end");
		}

		///<summary>Selection doesn't only happen by the tree and mouse clicks, but can also happen by automatic processes, such as image import, image paste, etc...
		///localPath will be set only if using Cloud storage and an image was imported.  We want to use the local version instead of re-downloading what was just uploaded.</summary>
		public void SelectTreeNode(TreeNode node,string localPath="") {
			//Select the node always, but perform additional tasks when necessary (i.e. load an image, or mount).
			treeDocuments.SelectedNode=node;
			treeDocuments.Invalidate();
			//Clear the copy document number for mount item swapping whenever a new mount is potentially selected.
			_idxDocToCopy=-1;
			//We only perform a load if the new selection is different than the old selection.
			ImageNodeTag nodeId=new ImageNodeTag();
			if(node!=null) {
				nodeId=(ImageNodeTag)node.Tag;
			}
			if(nodeId.Equals(_nodeIdentifierOld)) {
				return;
			}
			pictureBoxMain.Visible=true;
			if(_webBrowserDocument!=null) {
				_webBrowserDocument.Dispose();//Clear any previously loaded Acrobat .pdf file.
				_webBrowserDocument=null;
			}
			_documentSelected=new Document();
			bool isNodeOldDoc=(_nodeIdentifierOld.NodeType==EnumImageNodeType.Doc);
			long docNumOld=_nodeIdentifierOld.PriKey;
			_nodeIdentifierOld=nodeId;
			//Disable all item tools until the currently selected node is loaded properly in the picture box.
			EnableAllTreeItemTools(false);
			if(ToolBarPaint.Buttons["Hand"]!=null) {
				ToolBarPaint.Buttons["Hand"].Pushed=true;
			}
			if(ToolBarPaint.Buttons["Crop"]!=null) {
				ToolBarPaint.Buttons["Crop"].Pushed=false;
			}
			//Stop any current image processing. This will avoid having the ImageRenderingNow set to a valid image after
			//the current image has been erased. This will also avoid concurrent access to the the currently loaded images by
			//the main and worker threads.
			EraseCurrentImages();
			if(isNodeOldDoc) {//We are no longer using the previously saved image, try to delete it if it is in the temp directory.
				DeleteTempPdf(docNumOld);//Clean up the temp storage copy of Pdf.
			}
			if(nodeId.NodeType==EnumImageNodeType.ApteryxImage) {
				ShowApteryxImage(node); //Display image in our own special way. 
			}
			if(nodeId.NodeType==EnumImageNodeType.Category) {
				//A folder was selected (or unselection, but I am not sure unselection would be possible here).
				//The panel note control is made invisible to start and then made visible for the appropriate documents. This
				//line prevents the possibility of showing a signature box after selecting a folder node.
				panelNote.Visible=false;
				//Make sure the controls are sized properly in the image module since the visibility of the panel note might
				//have just changed.
				ResizeAll();
			}
			else if(nodeId.NodeType==EnumImageNodeType.Eob) {
				EobAttach eob=EobAttaches.GetOne(nodeId.PriKey);
				Action actionCloseDownloadProgress=null;
				if(CloudStorage.IsCloudStorage) {
					actionCloseDownloadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.g("ContrImages","Downloading..."));
				}
				try {
					_bitmapArrayImagesCur=ImageStore.OpenImagesEob(eob,localPath);
					actionCloseDownloadProgress?.Invoke();
				}
				catch(ApplicationException ex) {
					actionCloseDownloadProgress?.Invoke();
					FriendlyException.Show(ex.Message,(ex.InnerException==null ? ex : ex.InnerException));
				}
				bool isExportable=pictureBoxMain.Visible;
				if(_bitmapArrayImagesCur[0]==null) {
					if(Path.GetExtension(eob.FileName).ToLower()==".pdf") {//Adobe acrobat file.
						try {
							LoadPdf(ImageStore.GetEobFolder(),eob.FileName,localPath,ref isExportable,"Downloading EOB...");
						}
						catch(ApplicationException ex) {
							actionCloseDownloadProgress?.Invoke();
							FriendlyException.Show(ex.Message,ex);
						}
					}
				}
				EnableTreeItemTools(pictureBoxMain.Visible,true,true,pictureBoxMain.Visible,true,pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,
					pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,isExportable);
			}
			else if(nodeId.NodeType==EnumImageNodeType.Doc) {
				//Reload the doc from the db. We don't just keep reusing the tree data, because it will become more and 
				//more stale with age if the program is left open in the image module for long periods of time.
				_documentSelected=Documents.GetByNum(nodeId.PriKey,doReturnNullIfNotFound:true);
				if(_documentSelected==null) {
					MsgBox.Show(this,"Document has been deleted.");
					FillDocList(false);
					return;
				}
				_idxSelectedInMount=0;
				Action actionCloseDownloadProgress=null;
				if(CloudStorage.IsCloudStorage) {
					actionCloseDownloadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.g("ContrImages","Downloading..."));
				}
				//ImagesCur contains BitMaps of selected images if they are found.  ImagesCur is used to display images in the main window in a later method.
				//PDF files will always return null.
				List<Document> listDocs=new List<Document>() { _documentSelected };
				_bitmapArrayImagesCur=ImageStore.OpenImages(listDocs.ToArray(),_patFolder,localPath);
				//Diverges slightly from the normal use of this event, in that it is fired from SelectTreeNode() rather than ModuleSelected.  Appropriate
				//here because this is the only data in ContrImages that might affect the PatientDashboard, and there is no "LoadData" in this Module.
				PatientDashboardDataEvent.Fire(ODEventType.ModuleSelected
					,new PatientDashboardDataEventArgs() {
						Pat=_patCur,
						ListDocuments=listDocs,
						BitmapImagesModule=_bitmapArrayImagesCur[0],
					}
				);
				actionCloseDownloadProgress?.Invoke();
				bool isExportable=pictureBoxMain.Visible;
				if(_bitmapArrayImagesCur[0]==null) {
					if(ImageHelper.HasImageExtension(_documentSelected.FileName)) {
						string srcFileName = ODFileUtils.CombinePaths(_patFolder,_documentSelected.FileName);
						if(File.Exists(srcFileName)) {
							MessageBox.Show(Lan.g(this,"File found but cannot be opened")+": " + _documentSelected.FileName);
						}
						else {
							MessageBox.Show(Lan.g(this,"File not found")+": " + _documentSelected.FileName);
						}
					}
					else if(Path.GetExtension(_documentSelected.FileName).ToLower()==".pdf") {//Adobe acrobat file.
						LoadPdf(_patFolder,_documentSelected.FileName,localPath,ref isExportable,"Downloading Document...");
					}
				}
				SetBrightnessAndContrast();
				//Previously _webBrowserDocument would occassionaly be null. This is an attempt to fix that.
				bool doShowPrint=pictureBoxMain.Visible;
				if(_webBrowserDocument!=null) {
					doShowPrint=(doShowPrint || _webBrowserDocument.Visible);
				}
				EnableTreeItemTools(doShowPrint,true,true,pictureBoxMain.Visible,true,pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,
					pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,isExportable);
			}
			else if(nodeId.NodeType==EnumImageNodeType.Mount) {
				//Creates a complete initial mount image. No need to call invalidate until changes are made to the mount later.
				_listMountItemsForSelected=MountItems.GetItemsForMount(nodeId.PriKey);
				_documentArrayDocsInMount=Documents.GetDocumentsForMountItems(_listMountItemsForSelected);
				_idxSelectedInMount=-1;//No selection to start.
				Action actionCloseDownloadProgress=null;
				if(CloudStorage.IsCloudStorage) {
					actionCloseDownloadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.g("ContrImages","Downloading..."));
				}
				_bitmapArrayImagesCur=ImageStore.OpenImages(_documentArrayDocsInMount,_patFolder,localPath);
				actionCloseDownloadProgress?.Invoke();
				_mountSelected=Mounts.GetByNum(nodeId.PriKey);
				_bitmapImageRenderingNow=new Bitmap(_mountSelected.Width,_mountSelected.Height);
				ImageHelper.RenderMountImage(_bitmapImageRenderingNow,_bitmapArrayImagesCur,_listMountItemsForSelected,_documentArrayDocsInMount,_idxSelectedInMount);
				EnableTreeItemTools(true,true,true,true,false,false,false,true,true,true,false,false,false,true);
			}
			else if(nodeId.NodeType==EnumImageNodeType.Amd) {
				EhrAmendment amd=EhrAmendments.GetOne(nodeId.PriKey);
				try {
					_bitmapArrayImagesCur=ImageStore.OpenImagesAmd(amd);
				}
				catch(ApplicationException ex) {
					FriendlyException.Show(ex.Message,(ex.InnerException==null ? ex : ex.InnerException));
				}
				bool isExportable=pictureBoxMain.Visible;
				if(_bitmapArrayImagesCur[0]==null) {
					if(Path.GetExtension(amd.FileName).ToLower()==".pdf") {//Adobe acrobat file.
						LoadPdf(ImageStore.GetAmdFolder(),amd.FileName,localPath,ref isExportable,"Downloading Amendment...");
					}
				}
				EnableTreeItemTools(pictureBoxMain.Visible,true,true,pictureBoxMain.Visible,true,pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,
					pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,pictureBoxMain.Visible,isExportable);
			}
			if(nodeId.NodeType.In(EnumImageNodeType.Doc,EnumImageNodeType.Mount,EnumImageNodeType.Eob,EnumImageNodeType.Amd,EnumImageNodeType.ApteryxImage)) {
				_intArrayWidthsImagesCur=new int[_bitmapArrayImagesCur.Length];
				_intArrayHeightsImagesCur=new int[_bitmapArrayImagesCur.Length];
				for(int i=0;i<_bitmapArrayImagesCur.Length;i++) {
					if(_bitmapArrayImagesCur[i]!=null) {
						_intArrayWidthsImagesCur[i]=_bitmapArrayImagesCur[i].Width;
						_intArrayHeightsImagesCur[i]=_bitmapArrayImagesCur[i].Height;
					}
				}
				//Adjust visibility of panel note control based on if the new document has a signature.
				SetPanelNoteVisibility(_documentSelected);
				//Resize controls in our window to adjust for a possible change in the visibility of the panel note control.
				ResizeAll();
				//Refresh the signature and note in case the last document also had a signature.
				FillSignature();
			}
			if(nodeId.NodeType==EnumImageNodeType.Mount) {
				ReloadZoomTransCrop(_bitmapImageRenderingNow.Width,_bitmapImageRenderingNow.Height,new Document(),
					new Rectangle(0,0,pictureBoxMain.Width,pictureBoxMain.Height),out _zoomImage,
					out _zoomLevel,out _zoomOverall,out _pointTranslation);
				RenderCurrentImage(new Document(),_bitmapImageRenderingNow.Width,_bitmapImageRenderingNow.Height,_zoomImage,_pointTranslation);
			}
			if(nodeId.NodeType.In(EnumImageNodeType.Doc,EnumImageNodeType.Eob,EnumImageNodeType.Amd,EnumImageNodeType.ApteryxImage)) {
				//Render the initial image within the current bounds of the picturebox (if the document is an image).
				InvalidateSettings(ImageSettingFlags.ALL,true);
			}
		}
		#endregion Methods - Public

		#region Methods - ToolBar
		///<summary>Handles a change in selection of the xRay capture button.</summary>
		private void ToolBarCapture_Click() {
			if(treeDocuments.SelectedNode==null) {
				return;
			}
			if(ToolBarMain.Buttons["Capture"].Pushed) {
				ImageNodeTag nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
				if(nodeId.NodeType==EnumImageNodeType.ApteryxImage) {
					MsgBox.Show(this,"Cannot capture a read-only image. Please copy/paste or export/import the image you are trying to capture.");
					return;
				}
				//ComputerPref computerPrefs=ComputerPrefs.GetForLocalComputer();
				_suniDeviceControl.SensorType=ComputerPrefs.LocalComputer.SensorType;
				_suniDeviceControl.PortNumber=ComputerPrefs.LocalComputer.SensorPort;
				_suniDeviceControl.Binned=ComputerPrefs.LocalComputer.SensorBinned;
				_suniDeviceControl.ExposureLevel=ComputerPrefs.LocalComputer.SensorExposure;
				if(nodeId.NodeType!=EnumImageNodeType.Mount) {//No mount is currently selected.
					//Show the user that they are performing an image capture by generating a new mount.
					Mount mount=new Mount();
					mount.DateCreated=DateTimeOD.Today;
					mount.Description="unnamed capture";
					mount.DocCategory=GetCurrentCategory();
					mount.ImgType=ImageType.Mount;
					mount.PatNum=_patCur.PatNum;
					int border=Math.Max(_suniDeviceControl.SensorSize.Width,_suniDeviceControl.SensorSize.Height)/24;
					mount.Width=4*_suniDeviceControl.SensorSize.Width+5*border;
					mount.Height=_suniDeviceControl.SensorSize.Height+2*border;
					mount.MountNum=Mounts.Insert(mount);
					MountItem mountItem=new MountItem();
					mountItem.MountNum=mount.MountNum;
					mountItem.Width=_suniDeviceControl.SensorSize.Width;
					mountItem.Height=_suniDeviceControl.SensorSize.Height;
					mountItem.Ypos=border;
					mountItem.OrdinalPos=1;
					mountItem.Xpos=border;
					MountItems.Insert(mountItem);
					mountItem.OrdinalPos=0;
					mountItem.Xpos=mountItem.Width+2*border;
					MountItems.Insert(mountItem);
					mountItem.OrdinalPos=2;
					mountItem.Xpos=2*mountItem.Width+3*border;
					MountItems.Insert(mountItem);
					mountItem.OrdinalPos=3;
					mountItem.Xpos=3*mountItem.Width+4*border;
					MountItems.Insert(mountItem);
					FillDocList(false);
					SelectTreeNode(GetNodeById(MakeIdMount(mount.MountNum)));
					sliderBrightnessContrast.MinVal=PrefC.GetInt(PrefName.ImageWindowingMin);
					sliderBrightnessContrast.MaxVal=PrefC.GetInt(PrefName.ImageWindowingMax);
				}
				else if(nodeId.NodeType==EnumImageNodeType.Mount) {//A mount is currently selected. We must allow the user to insert new images into partially complete mounts.
					//Clear the visible selection so that the user will know when the device is ready for xray exposure.
					ImageHelper.RenderMountFrames(_bitmapImageRenderingNow,_listMountItemsForSelected,-1);
					RenderCurrentImage(new Document(),_bitmapImageRenderingNow.Width,_bitmapImageRenderingNow.Height,_zoomImage*_zoomOverall,_pointTranslation);
				}
				//Here we can only allow access to the capture button during a capture, because it is too complicated and hard for a 
				//user to follow what is going on if they use the other tools when a capture is taking place.
				EnableAllTools(false);
				ToolBarMain.Buttons["Capture"].Enabled=true;
				ToolBarMain.Invalidate();
				_suniDeviceControl.CaptureXRay();
			}
			else {//The user unselected the image capture button, so cancel the current image capture.
				_suniDeviceControl.KillXRayThread();//Stop current xRay capture and call OnCaptureFinalize() when done.
			}
		}

		///<summary>Handles a change in selection of the xRay capture button.</summary>
		private void ToolBarClose_Click() {
			EventArgs args=new EventArgs();
			SelectTreeNode(null);
			this.Dispose();
			if(CloseClick!=null) {
				CloseClick(this,args);
			}
		}

		private void ToolBarCopy_Click() {
			if(treeDocuments.SelectedNode==null || treeDocuments.SelectedNode.Tag==null) {
				MsgBox.Show(this,"Please select a document before copying");
				return;
			}
			Bitmap bitmapCopy=null;
			Cursor=Cursors.WaitCursor;
			ImageNodeTag nodeId=new ImageNodeTag();
			nodeId = (ImageNodeTag)treeDocuments.SelectedNode.Tag;
			if(nodeId.NodeType==EnumImageNodeType.Mount) {
				if(_idxSelectedInMount>=0 && _documentArrayDocsInMount[_idxSelectedInMount]!=null) {//A mount item is currently selected.
					bitmapCopy=ImageHelper.ApplyDocumentSettingsToImage(_documentArrayDocsInMount[_idxSelectedInMount],_bitmapArrayImagesCur[_idxSelectedInMount],ImageSettingFlags.ALL);
				}
				else {//Assume the copy is for the entire mount.
					bitmapCopy=(Bitmap)_bitmapImageRenderingNow.Clone();
				}
			}
			else if(nodeId.NodeType==EnumImageNodeType.Doc) {
				//Crop and color function has already been applied to the render image.
				bitmapCopy=ImageHelper.ApplyDocumentSettingsToImage(Documents.GetByNum(nodeId.PriKey),_bitmapImageRenderingNow,
					ImageSettingFlags.FLIP | ImageSettingFlags.ROTATE);
			}
			else if(nodeId.NodeType.In(EnumImageNodeType.Eob,EnumImageNodeType.Amd,EnumImageNodeType.ApteryxImage)) {
				bitmapCopy=(Bitmap)_bitmapImageRenderingNow.Clone();
			}
			if(bitmapCopy!=null) {
				try {
					Clipboard.SetDataObject(bitmapCopy);
				}
				catch(Exception ex) {
					MsgBox.Show(this,"Could not copy contents to the clipboard.  Please try again.");
					ex.DoNothing();
					return;
				}
				//Can't do this, or the clipboard object goes away.
				//bitmapCopy.Dispose();
				//bitmapCopy=null;
			}
			long patNum=0;
			if(_patCur!=null) {
				patNum=_patCur.PatNum;
			}
			if(nodeId.NodeType==EnumImageNodeType.ApteryxImage) {
				SecurityLogs.MakeLogEntry(Permissions.Copy,patNum,"Patient image "+nodeId.ImgDownload.AcquisitionDate.ToShortDateString()+" "
					+nodeId.ImgDownload.AdultTeeth.ToString()+nodeId.ImgDownload.DeciduousTeeth.ToString()+" copied to clipboard");
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.Copy,patNum,"Patient image "+Documents.GetByNum(nodeId.PriKey).FileName+" copied to clipboard");
			}
			Cursor=Cursors.Default;
		}

		private void ToolBarCrop_Click() {
			//remember it's testing after the push has been completed
			if(ToolBarPaint.Buttons["Crop"].Pushed) { //Crop Mode
				ToolBarPaint.Buttons["Hand"].Pushed=false;
				pictureBoxMain.Cursor=Cursors.Default;
			}
			else {
				ToolBarPaint.Buttons["Crop"].Pushed=true;
			}
			_isCropMode=true;
			ToolBarPaint.Invalidate();
		}

		///<summary>If the node does not correspond to a valid document or mount, nothing happens. Otherwise the document/mount record and its corresponding file(s) are deleted.</summary>
		private void ToolBarDelete_Click() {
			DeleteSelection(true,true);
		}

		private void ToolBarExport_Click() {
			if(treeDocuments.SelectedNode==null) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			Document apteryxDoc=null;
			ImageNodeTag nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
			if(nodeId.NodeType==EnumImageNodeType.ApteryxImage) {
				string imageCat=ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.XVWeb),XVWeb.ProgramProps.ImageCategory);
				//save copy to db for temp storage
				apteryxDoc=ImageStore.Import(_bitmapImageRenderingNow,(Defs.GetDef(DefCat.ImageCats,PIn.Long(imageCat)).DefNum),ImageType.Photo,_patCur); 
			}
			if(nodeId.NodeType==EnumImageNodeType.Category || nodeId.NodeType==EnumImageNodeType.Mount || nodeId.NodeType==EnumImageNodeType.None) {
				MsgBox.Show(this,"Not allowed.");
				return;
			}
			string fileName="";
			if(ODBuild.IsWeb()) {
				ToolBarWebExport(nodeId,apteryxDoc);
				return;
			}
			SaveFileDialog dlg=new SaveFileDialog();
			dlg.Title="Export a Document";
			if(nodeId.NodeType.In(EnumImageNodeType.Doc,EnumImageNodeType.ApteryxImage)) {
				Document doc;
				if(nodeId.NodeType==EnumImageNodeType.Doc) {
					doc=Documents.GetByNum(nodeId.PriKey);
				}
				else {
					doc=apteryxDoc;
				}
				dlg.FileName=doc.FileName;
				dlg.DefaultExt=Path.GetExtension(doc.FileName);
				if(dlg.ShowDialog()!=DialogResult.OK) {
					return;
				}
				fileName=dlg.FileName;
				if(fileName.Length<1) {
					MsgBox.Show(this,"You must enter a file name.");
					return;
				}
				try {
					ImageStore.Export(fileName,doc,_patCur);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Unable to export file, May be in use")+": " + ex.Message + ": " + fileName);
					return;
				}
			}
			if(nodeId.NodeType==EnumImageNodeType.Eob) {
				EobAttach eob=EobAttaches.GetOne(nodeId.PriKey);
				dlg.FileName=eob.FileName;
				dlg.DefaultExt=Path.GetExtension(eob.FileName);
				if(dlg.ShowDialog()!=DialogResult.OK) {
					return;
				}
				fileName=dlg.FileName;
				if(fileName.Length<1) {
					MsgBox.Show(this,"You must enter a file name.");
					return;
				}
				try {
					ImageStore.ExportEobAttach(fileName,eob);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Unable to export file, May be in use")+": " + ex.Message + ": " + fileName);
					return;
				}
			}
			else if(nodeId.NodeType==EnumImageNodeType.Amd) {
				EhrAmendment amd=EhrAmendments.GetOne(nodeId.PriKey);
				dlg.FileName=amd.FileName;
				dlg.DefaultExt=Path.GetExtension(amd.FileName);
				if(dlg.ShowDialog()!=DialogResult.OK) {
					return;
				}
				fileName=dlg.FileName;
				if(fileName.Length<1) {
					MsgBox.Show(this,"You must enter a file name.");
					return;
				}
				try {
					ImageStore.ExportAmdAttach(fileName,amd);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Unable to export file, May be in use")+": " + ex.Message + ": " + fileName);
					return;
				}
			}
			MessageBox.Show(Lan.g(this,"Successfully exported to ")+fileName);
			if(nodeId.NodeType==EnumImageNodeType.ApteryxImage && apteryxDoc!=null) {
				try {
					ImageStore.DeleteDocuments(new List<Document> { apteryxDoc },_patFolder);
				}
				catch(Exception ex) {  //Image could not be deleted, in use.
					ex.DoNothing();//The user doesn't even know this document exists, so there's not any point in telling them we couldn't delete it.
				}
			}
		}

		private void ToolBarFlip_Click() {
			if(((ImageNodeTag)treeDocuments.SelectedNode.Tag).NodeType==EnumImageNodeType.None || _documentSelected==null) {
				return;
			}
			_documentSelected.IsFlipped=!_documentSelected.IsFlipped;
			//Since the document is always flipped and then rotated in the mathematical functions below, and since we
			//always want the selected image to rotate left to right no matter what orientation the image is in,
			//we must modify the document settings so that the document will always be flipped left to right, but
			//in such a way that the flipping always happens before the rotation.
			if(_documentSelected.DegreesRotated==90||_documentSelected.DegreesRotated==270) {
				_documentSelected.DegreesRotated*=-1;
				while(_documentSelected.DegreesRotated<0) {
					_documentSelected.DegreesRotated+=360;
				}
				_documentSelected.DegreesRotated=(short)(_documentSelected.DegreesRotated%360);
			}
			Documents.Update(_documentSelected);
			DeleteThumbnailImage(_documentSelected);
			InvalidateSettings(ImageSettingFlags.FLIP,false);//Refresh display.
		}

		private void ToolBarHand_Click() {
			if(ToolBarPaint.Buttons["Hand"].Pushed) {//Hand Mode
				ToolBarPaint.Buttons["Crop"].Pushed=false;
				pictureBoxMain.Cursor=Cursors.Hand;
			}
			else {
				ToolBarPaint.Buttons["Hand"].Pushed=true;
			}
			_isCropMode=false;
			ToolBarPaint.Invalidate();
		}

		private void ToolBarImport_Click() {
			if(Plugins.HookMethod(this,"ContrImages.ToolBarImport_Click_Start",_patCur)) {
				FillDocList(true);
				return;
			}
			if(_ehrAmendmentCur!=null) {
				if(_ehrAmendmentCur.FileName!=null && _ehrAmendmentCur.FileName!="") {
					if(!MsgBox.Show(this,true,"This will delete your old file. Proceed?")) {
						return;
					}
				}
			}
			OpenFileDialog openFileDialog=new OpenFileDialog();
			openFileDialog.Multiselect=true;
			if(Prefs.GetContainsKey(nameof(PrefName.UseAlternateOpenFileDialogWindow)) && PrefC.GetBool(PrefName.UseAlternateOpenFileDialogWindow)){//Hidden pref, almost always false.
				//We don't know why this makes any difference but people have mentioned this will stop some hanging issues.
				//https://stackoverflow.com/questions/6718148/windows-forms-gui-hangs-when-calling-openfiledialog-showdialog
				openFileDialog.ShowHelp=true;
			}
			if(_ehrAmendmentCur!=null) {
				openFileDialog.Multiselect=false;//this image module control is reused in formEHR for amendments. If so, EhrAmendmentCur!=null and we should only allow single select.
			}
			if(openFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			string[] fileNames=openFileDialog.FileNames;
			if(fileNames.Length<1) {
				return;
			}
			ImageNodeTag nodeId=new ImageNodeTag();
			bool copied=true;
			if(_claimPaymentNum!=0) {//eob
				EobAttach eob=null;
				Action actionCloseUploadProgress=null;
				if(CloudStorage.IsCloudStorage) {
					actionCloseUploadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.g("ContrImages","Uploading..."));
				}
				for(int i=0;i<fileNames.Length;i++) {
					try {
						eob=ImageStore.ImportEobAttach(fileNames[i],_claimPaymentNum);
					}
					catch(Exception ex) {
						actionCloseUploadProgress?.Invoke();
						MessageBox.Show(Lan.g(this,"Unable to copy file, May be in use: ")+ex.Message+": "+openFileDialog.FileName);
						copied = false;
					}
				}
				actionCloseUploadProgress?.Invoke();
				if(copied) {
					FillDocList(false);
				}
				if(eob!=null) {
					SelectTreeNode(GetNodeById(MakeIdEob(eob.EobAttachNum)),fileNames[fileNames.Length-1]);
				}
			}
			else if(_ehrAmendmentCur!=null) {
				string amdFilename=_ehrAmendmentCur.FileName;
				Action actionCloseUploadProgress=null;
				if(CloudStorage.IsCloudStorage) {
					actionCloseUploadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.g("ContrImages","Uploading..."));
				}
				for(int i=0;i<fileNames.Length;i++) {
					try {
						_ehrAmendmentCur=ImageStore.ImportAmdAttach(fileNames[i],_ehrAmendmentCur);
						SelectTreeNode(null);
						ImageStore.CleanAmdAttach(amdFilename);
					}
					catch(Exception ex) {
						actionCloseUploadProgress?.Invoke();
						MessageBox.Show(Lan.g(this,"Unable to copy file, May be in use: ")+ex.Message+": "+openFileDialog.FileName);
						copied = false;
					}
				}
				actionCloseUploadProgress?.Invoke();
				if(copied) {
					FillDocList(false);
				}
				if(_ehrAmendmentCur!=null) {
					SelectTreeNode(GetNodeById(MakeIdAmd(_ehrAmendmentCur.EhrAmendmentNum)),fileNames[fileNames.Length-1]);
				}
			}
			else {//regular Images module
				Document doc=null;
				Action actionCloseUploadProgress=null;
				if(CloudStorage.IsCloudStorage) {
					actionCloseUploadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.g("ContrImages","Uploading..."));
				}
				for(int i=0;i<fileNames.Length;i++) {
					try {
						doc=ImageStore.Import(fileNames[i],GetCurrentCategory(),_patCur);//Makes log
					}
					catch(Exception ex) {
						actionCloseUploadProgress?.Invoke();
						MessageBox.Show(Lan.g(this,"Unable to copy file, May be in use: ")+ex.Message+": "+openFileDialog.FileName);
						copied = false;
					}
					if(copied) {
						FillDocList(false);
						SelectTreeNode(GetNodeById(MakeIdDoc(doc.DocNum)),fileNames[i]);
						FormDocInfo FormD=new FormDocInfo(_patCur,doc,GetCurrentFolderName(treeDocuments.SelectedNode));
						FormD.TopMost=true;
						FormD.ShowDialog(this);//some of the fields might get changed, but not the filename
						if(FormD.DialogResult!=DialogResult.OK) {
							DeleteSelection(false,false,doc);
						}
						else {
							if(doc.ImgType==ImageType.Photo) {
								PatientEvent.Fire(ODEventType.Patient,_patCur);//Possibly updated the patient picture.
							}
							nodeId=MakeIdDoc(doc.DocNum);
							_documentSelected=doc.Copy();
						}
					}
				}
				actionCloseUploadProgress?.Invoke();
				//Reselect the last successfully added node when necessary.
				if(doc!=null && !MakeIdDoc(doc.DocNum).Equals(nodeId)) {
					SelectTreeNode(GetNodeById(MakeIdDoc(doc.DocNum)),fileNames[fileNames.Length-1]);
				}
				FillDocList(true);
			}
		}

		private void ToolBarInfo_Click() {
			ImageNodeTag nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
			if(nodeId.NodeType==EnumImageNodeType.None) {
				return;
			}
			if(nodeId.NodeType==EnumImageNodeType.Mount) {
				FormMountEdit form=new FormMountEdit(_mountSelected);
				form.ShowDialog();//Edits the MountSelected object directly and updates and changes to the database as well.
				FillDocList(true);//Refresh tree in case description for the mount changed.}
			}
			if(nodeId.NodeType==EnumImageNodeType.ApteryxImage) {
				Document doc=new Document();
				doc.DateCreated=nodeId.ImgDownload.AcquisitionDate;
				doc.ToothNumbers=string.Join(",",nodeId.ImgDownload.AdultTeeth,nodeId.ImgDownload.DeciduousTeeth);
				FormDocInfo fdi=new FormDocInfo(_patCur,doc,GetCurrentFolderName(treeDocuments.SelectedNode),true);//disable ok button, they can't save anything. Image is just temp.
				fdi.ShowDialog(this);
				if(fdi.DialogResult!=DialogResult.OK) {
					return;
				}
				FillDocList(false);
			}
			else if(nodeId.NodeType==EnumImageNodeType.Doc) {
				//The FormDocInfo object updates the DocSelected and stores the changes in the database as well.
				FormDocInfo formDocInfo2=new FormDocInfo(_patCur,_documentSelected,GetCurrentFolderName(treeDocuments.SelectedNode));
				formDocInfo2.ShowDialog(this);
				if(formDocInfo2.DialogResult!=DialogResult.OK) {
					return;
				}
				FillDocList(true);
			}
		}

		private void ToolBarPaste_Click() {
			IDataObject clipboard;
			try {
				clipboard=Clipboard.GetDataObject();
			}
			catch(Exception ex) {
				MsgBox.Show(this,"Could not paste contents from the clipboard.  Please try again.");
				ex.DoNothing();
				return;
			}
			if(!clipboard.GetDataPresent(DataFormats.Bitmap)) {
				MessageBox.Show(Lan.g(this,"No bitmap present on clipboard"));
				return;
			}
			Bitmap bitmapPaste=(Bitmap)clipboard.GetData(DataFormats.Bitmap);
			Document doc;
			ImageNodeTag nodeId=new ImageNodeTag();
			if(treeDocuments.SelectedNode!=null && treeDocuments.SelectedNode.Tag!=null) {
				nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
			}
			Cursor=Cursors.WaitCursor;
			if(_claimPaymentNum!=0) {
				EobAttach eob=null;
				try {
					eob=ImageStore.ImportEobAttach(bitmapPaste,_claimPaymentNum);
				}
				catch {
					MessageBox.Show(Lan.g(this,"Error saving eob."));
					Cursor=Cursors.Default;
					return;
				}
				FillDocList(false);
				SelectTreeNode(GetNodeById(MakeIdEob(eob.EobAttachNum)));
			}
			else if(_ehrAmendmentCur!=null) {
				EhrAmendment amd=null;
				try {
					amd=ImageStore.ImportAmdAttach(bitmapPaste,_ehrAmendmentCur);
				}
				catch {
					MessageBox.Show(Lan.g(this,"Error saving amendment."));
					Cursor=Cursors.Default;
					return;
				}
				FillDocList(false);
				SelectTreeNode(GetNodeById(MakeIdAmd(amd.EhrAmendmentNum)));
			}
			else if(nodeId.NodeType==EnumImageNodeType.Mount && _idxSelectedInMount>=0) {//Pasting into the mount item of the currently selected mount.
				if(_documentArrayDocsInMount[_idxSelectedInMount]!=null) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Do you want to replace the existing item in this mount location?")) {
						this.Cursor=Cursors.Default;
						return;
					}
					DeleteSelection(false,true);
				}
				try {
					doc=ImageStore.ImportImageToMount(bitmapPaste,0,_listMountItemsForSelected[_idxSelectedInMount].MountItemNum,GetCurrentCategory(),_patCur);//Makes log entry				
					doc.WindowingMax=255;
					doc.WindowingMin=0;
					Documents.Update(doc);
				}
				catch {
					MessageBox.Show(Lan.g(this,"Error saving document."));
					Cursor=Cursors.Default;
					return;
				}
				_documentArrayDocsInMount[_idxSelectedInMount]=doc;
				_bitmapArrayImagesCur[_idxSelectedInMount]=bitmapPaste;
			}
			else {//Paste the image as its own unique document.
				try {
					doc=ImageStore.Import(bitmapPaste,GetCurrentCategory(),ImageType.Photo,_patCur);//Makes log entry
				}
				catch {
					MessageBox.Show(Lan.g(this,"Error saving document."));
					Cursor=Cursors.Default;
					return;
				}
				FillDocList(false);
				SelectTreeNode(GetNodeById(MakeIdDoc(doc.DocNum)));
				FormDocInfo formD=new FormDocInfo(_patCur,doc,GetCurrentFolderName(treeDocuments.SelectedNode));
				formD.ShowDialog(this);
				if(formD.DialogResult!=DialogResult.OK) {
					DeleteSelection(false,false,doc);
				}
				else {
					_documentSelected=doc.Copy();
					FillDocList(true);
				}
			}
			InvalidateSettings(ImageSettingFlags.ALL,true);
			Cursor=Cursors.Default;
		}

		private void ToolBarPrint_Click() {
			if(treeDocuments.SelectedNode==null || treeDocuments.SelectedNode.Tag==null) {
				MsgBox.Show(this,"Cannot print. No document currently selected.");
				return;
			}
			try {
				string fileName=null;
				string description=null;
				ImageNodeTag nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
				if(nodeId.NodeType==EnumImageNodeType.ApteryxImage) {
					MsgBox.Show(this,"Cannot print a read only file. Copy/paste or export/import the file for printing.");
					return;
				}
				if(nodeId.NodeType==EnumImageNodeType.Eob) {
					fileName=EobAttaches.GetOne(nodeId.PriKey).FileName;
					description="";
				}
				else if(nodeId.NodeType==EnumImageNodeType.Amd) {
					EhrAmendment ehrAmendment=EhrAmendments.GetOne(nodeId.PriKey);
					fileName=ehrAmendment.FileName;
					description=ehrAmendment.Description;
				}
				else {
					fileName=_documentSelected.FileName;
					description=_documentSelected.Description;
				}
				if(Path.GetExtension(fileName).ToLower()==".pdf") {//Selected document is PDF, we handle differently than documents that aren't pdf.
					if(ODBuild.IsWeb()) {
						ThinfinityUtils.HandleFile(_webBrowserFilePath);//This will do a PDF preview. _webBrowserDocument.ShowPrintPreviewDialog() doesn't work.
					}
					else {
						_webBrowserDocument.ShowPrintPreviewDialog();
					}
				}
				else {
					PrintDocument pd=new PrintDocument();//TODO: Implement ODprintout pattern
					pd.PrintPage+=new PrintPageEventHandler(printDocument_PrintPage);
					PrintDialog dlg=new PrintDialog();
					dlg.AllowCurrentPage=false;
					dlg.AllowPrintToFile=true;
					dlg.AllowSelection=false;
					dlg.AllowSomePages=false;
					dlg.Document=pd;
					dlg.PrintToFile=false;
					dlg.ShowHelp=true;
					dlg.ShowNetwork=true;
					dlg.UseEXDialog=true; //needed because PrintDialog was not showing on 64 bit Vista systems
					if(dlg.ShowDialog()==DialogResult.OK) {
						if(pd.DefaultPageSettings.PrintableArea.Width==0||
							pd.DefaultPageSettings.PrintableArea.Height==0) {
							pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
						}
						pd.OriginAtMargins=true;
						pd.DefaultPageSettings.Margins=new Margins(50,50,50,50);//Half-inch all around
						pd.Print();
						if(((ImageNodeTag)treeDocuments.SelectedNode.Tag).NodeType==EnumImageNodeType.Eob) { //This happens when printing an EOB from the Batch Ins Claim
							SecurityLogs.MakeLogEntry(Permissions.Printing,0,"EOB printed");
						}
						else if(description=="") {
							SecurityLogs.MakeLogEntry(Permissions.Printing,_patCur.PatNum,"Patient image "+fileName+" printed");
						}
						else {
							SecurityLogs.MakeLogEntry(Permissions.Printing,_patCur.PatNum,"Patient image "+description+" printed");
						}
					}
				}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"An error occurred while printing")+"\r\n"+ex.ToString());
			}
		}

		private void ToolBarRotateL_Click() {
			if(((ImageNodeTag)treeDocuments.SelectedNode.Tag).NodeType==EnumImageNodeType.None || _documentSelected==null) {
				return;
			}
			_documentSelected.DegreesRotated-=90;
			while(_documentSelected.DegreesRotated<0) {
				_documentSelected.DegreesRotated+=360;
			}
			Documents.Update(_documentSelected);
			DeleteThumbnailImage(_documentSelected);
			InvalidateSettings(ImageSettingFlags.ROTATE,false);//Refresh display.
		}

		private void ToolBarRotateR_Click() {
			if(((ImageNodeTag)treeDocuments.SelectedNode.Tag).NodeType==EnumImageNodeType.None || _documentSelected==null) {
				return;
			}
			_documentSelected.DegreesRotated+=90;
			_documentSelected.DegreesRotated%=360;
			Documents.Update(_documentSelected);
			DeleteThumbnailImage(_documentSelected);
			InvalidateSettings(ImageSettingFlags.ROTATE,false);//Refresh display.
		}

		///<summary>Valid values for scanType are "doc","xray",and "photo"</summary>
		private void ToolBarScan_Click(string scanType) {
			if(_ehrAmendmentCur!=null) {
				if(_ehrAmendmentCur.FileName!=null && _ehrAmendmentCur.FileName!="") {
					if(!MsgBox.Show(this,true,"This will delete your old file. Proceed?")) {
						return;
					}
				}
			}
			Cursor=Cursors.WaitCursor;
			Bitmap bitmapScanned=null;
			IntPtr hdib=IntPtr.Zero;
			try {
				xImageDeviceManager.Obfuscator.ActivateEZTwain();
			}
			catch {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"EzTwain4.dll not found.  Please run the setup file in your images folder.");
				return;
			}
			if(IsDisposed) {
				return;//Possibly the form was being closed as the user clicked the scan button. 
			}
			if(ComputerPrefs.LocalComputer.ScanDocSelectSource) {
				if(!EZTwain.SelectImageSource(this.Handle)) {//dialog to select source
					Cursor=Cursors.Default;
					return;//User clicked cancel.
				}
			}
			EZTwain.SetHideUI(!ComputerPrefs.LocalComputer.ScanDocShowOptions);
			if(!EZTwain.OpenDefaultSource()) {//if it can't open the scanner successfully
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Default scanner could not be opened.  Check that the default scanner works from Windows Control Panel and from Windows Fax and Scan.");
				return;
			}
			EZTwain.SetResolution(ComputerPrefs.LocalComputer.ScanDocResolution);
			if(ComputerPrefs.LocalComputer.ScanDocGrayscale) {
				EZTwain.SetPixelType(1);//8-bit grayscale - only set if scanner dialog will not show
			}
			else {
				EZTwain.SetPixelType(2);//24-bit color
			}
			EZTwain.SetJpegQuality(ComputerPrefs.LocalComputer.ScanDocQuality);
			EZTwain.SetXferMech(EZTwain.XFERMECH_MEMORY);
			Cursor=Cursors.Default;
			hdib=EZTwain.Acquire(this.Handle);//This is where the options dialog would come up. The settings above will not populate this window.
			int errorCode=EZTwain.LastErrorCode();
			if(errorCode!=0) {
				string message="";
				if(errorCode==(int)EZTwainErrorCode.EZTEC_USER_CANCEL) {//19
					//message="\r\nScanning cancelled.";//do nothing
					return;
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_JPEG_DLL) {//22
					message="Missing dll\r\n\r\nRequired file EZJpeg.dll is missing.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_0_PAGES) {//38
					//message="\r\nScanning cancelled.";//do nothing
					return;
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_NO_PDF) {//43
					message="Missing dll\r\n\r\nRequired file EZPdf.dll is missing.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_DEVICE_PAPERJAM) {//76
					message="Paper jam\r\n\r\nPlease check the scanner document feeder and ensure there path is clear of any paper jams.";
				}
				else {
					message=errorCode+" "+((EZTwainErrorCode)errorCode).ToString();
				}
				MessageBox.Show(Lan.g(this,"Unable to scan. Please make sure you can scan using other software. Error: "+message));
				return;
			}
			if(hdib==(IntPtr)0) {//This is down here because there might also be an informative error code that we would like to use above.
				return;//User cancelled
			}
			double xdpi=EZTwain.DIB_XResolution(hdib);
			double ydpi=EZTwain.DIB_XResolution(hdib);
			IntPtr hbitmap=EZTwain.DIB_ToDibSection(hdib);
			try {
				bitmapScanned=Bitmap.FromHbitmap(hbitmap);//Sometimes throws 'A generic error occurred in GDI+.'
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"Error importing eob")+": "+ex.Message,ex);
				return;
			}			
			bitmapScanned.SetResolution((float)xdpi,(float)ydpi);
			try {
				Clipboard.SetImage(bitmapScanned);//We do this because a customer requested it, and some customers probably use it.
			}
			catch {
				//Rarely, setting the clipboard image fails, in which case we should ignore the failure because most people do not use this feature.
			}
			ImageType imgType;
			if(scanType=="xray") {
				imgType=ImageType.Radiograph;
			}
			else if(scanType=="photo") {
				imgType=ImageType.Photo;
			}
			else {//Assume document
				imgType=ImageType.Document;
			}
			bool saved=true;
			if(_claimPaymentNum!=0) {//eob
				EobAttach eob=null;
				try {
					eob=ImageStore.ImportEobAttach(bitmapScanned,_claimPaymentNum);
				}
				catch(Exception ex) {
					saved=false;
					Cursor=Cursors.Default;
					MessageBox.Show(Lan.g(this,"Error saving eob")+": "+ex.Message);
				}
				if(bitmapScanned!=null) {
					bitmapScanned.Dispose();
					bitmapScanned=null;
				}
				if(hdib!=IntPtr.Zero) {
					EZTwain.DIB_Free(hdib);
				}
				Cursor=Cursors.Default;
				if(saved) {
					FillDocList(false);
					SelectTreeNode(GetNodeById(MakeIdEob(eob.EobAttachNum)));
				}
			}
			else if(_ehrAmendmentCur!=null) {
				//We only allow users to scan in one amendment at a time.  Keep track of the old file name.
				string fileNameOld=_ehrAmendmentCur.FileName;
				try {
					ImageStore.ImportAmdAttach(bitmapScanned,_ehrAmendmentCur);
					SelectTreeNode(null);
					ImageStore.CleanAmdAttach(fileNameOld);//Delete the old scanned document.
				}
				catch(Exception ex) {
					saved=false;
					Cursor=Cursors.Default;
					MessageBox.Show(Lan.g(this,"Error saving amendment")+": "+ex.Message);
				}
				if(bitmapScanned!=null) {
					bitmapScanned.Dispose();
					bitmapScanned=null;
				}
				if(hdib!=IntPtr.Zero) {
					EZTwain.DIB_Free(hdib);
				}
				Cursor=Cursors.Default;
				if(saved) {
					FillDocList(false);
					SelectTreeNode(GetNodeById(MakeIdAmd(_ehrAmendmentCur.EhrAmendmentNum)));
				}
			}
			else {//regular Images module
				Document doc = null;
				try {//Create corresponding image file.
					doc=ImageStore.Import(bitmapScanned,GetCurrentCategory(),imgType,_patCur);
				}
				catch(Exception ex) {
					saved=false;
					Cursor=Cursors.Default;
					MessageBox.Show(Lan.g(this,"Unable to save document")+": "+ex.Message);
				}
				if(bitmapScanned!=null) {
					bitmapScanned.Dispose();
					bitmapScanned=null;
				}
				if(hdib!=IntPtr.Zero) {
					EZTwain.DIB_Free(hdib);
				}
				Cursor=Cursors.Default;
				if(saved) {
					FillDocList(false);//Reload and keep new document selected.
					SelectTreeNode(GetNodeById(MakeIdDoc(doc.DocNum)));
					FormDocInfo formDocInfo=new FormDocInfo(_patCur,_documentSelected,GetCurrentFolderName(treeDocuments.SelectedNode));
					formDocInfo.ShowDialog(this);
					if(formDocInfo.DialogResult!=DialogResult.OK) {
						DeleteSelection(false,false,doc);
					}
					else {
						FillDocList(true);//Update tree, in case the new document's icon or category were modified in formDocInfo.
					}
				}
			}
		}

		private void ToolBarScanMulti_Click() {
			if(_ehrAmendmentCur!=null) {
				if(_ehrAmendmentCur.FileName!=null && _ehrAmendmentCur.FileName!="") {
					if(!MsgBox.Show(this,true,"This will delete your old file. Proceed?")) {
						return;
					}
				}
			}
			string tempFile=PrefC.GetRandomTempFile(".pdf");
			try {
				xImageDeviceManager.Obfuscator.ActivateEZTwain();
			}
			catch {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"EzTwain4.dll not found.  Please run the setup file in your images folder.");
				return;
			}
			if(IsDisposed) {
				return;//Possibly the form was being closed as the user clicked the scan button. 
			}
			if(ComputerPrefs.LocalComputer.ScanDocSelectSource) {
				if(!EZTwain.SelectImageSource(this.Handle)) {
					return;//User clicked cancel.
				}
			}
			//EZTwain.LogFile(7);//Writes at level 7 (very detailed) in the C:\eztwain.log text file. Useful for getting help from EZTwain support on their forum.
			EZTwain.SetHideUI(!ComputerPrefs.LocalComputer.ScanDocShowOptions);
			EZTwain.PDF_SetCompression((int)this.Handle,(int)ComputerPrefs.LocalComputer.ScanDocQuality);
			if(!EZTwain.OpenDefaultSource()) {//if it can't open the scanner successfully
				MsgBox.Show(this,"Default scanner could not be opened.  Check that the default scanner works from Windows Control Panel and from Windows Fax and Scan.");
				Cursor=Cursors.Default;
				return;
			}
			bool duplexEnabled=EZTwain.EnableDuplex(ComputerPrefs.LocalComputer.ScanDocDuplex);//This line seems to cause problems.
			if(ComputerPrefs.LocalComputer.ScanDocGrayscale) {
				EZTwain.SetPixelType(1);//8-bit grayscale
			}
			else {
				EZTwain.SetPixelType(2);//24-bit color
			}
			EZTwain.SetResolution(ComputerPrefs.LocalComputer.ScanDocResolution);
			EZTwain.AcquireMultipageFile(this.Handle,tempFile);//This is where the options dialog will come up if enabled. This will ignore and override the settings above.
			int errorCode=EZTwain.LastErrorCode();
			if(errorCode!=0) {
				string message="";
				if(errorCode==(int)EZTwainErrorCode.EZTEC_USER_CANCEL) {//19
					//message="\r\nScanning cancelled.";//do nothing
					return;
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_JPEG_DLL) {//22
					message="Missing dll\r\n\r\nRequired file EZJpeg.dll is missing.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_0_PAGES) {//38
					//message="\r\nScanning cancelled.";//do nothing
					return;
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_NO_PDF) {//43
					message="Missing dll\r\n\r\nRequired file EZPdf.dll is missing.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_DEVICE_PAPERJAM) {//76
					message="Paper jam\r\n\r\nPlease check the scanner document feeder and ensure there path is clear of any paper jams.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_DS_FAILURE) {//5
					message="Duplex failure\r\n\r\nDuplex mode without scanner options window failed. Try enabling the scanner options window or disabling duplex mode.";
				}
				else {
					message=errorCode+" "+((EZTwainErrorCode)errorCode).ToString();
				}
				MessageBox.Show(Lan.g(this,"Unable to scan. Please make sure you can scan using other software. Error: "+message));
				return;
			}
			ImageNodeTag nodeId=new ImageNodeTag();
			bool copied=true;
			if(_claimPaymentNum!=0) {//eob
				EobAttach eob=null;
				try {
					eob=ImageStore.ImportEobAttach(tempFile,_claimPaymentNum);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Unable to copy file, May be in use: ") + ex.Message + ": " + tempFile);
					copied = false;
				}
				if(copied) {
					FillDocList(false);
					SelectTreeNode(GetNodeById(MakeIdEob(eob.EobAttachNum)));
				}
				ImageStore.TryDeleteFile(tempFile
					,actInUseException:(msg) => MsgBox.Show(msg)//Informs user when a 'file is in use' exception occurs.
				);
			}
			else if(_ehrAmendmentCur!=null) {//amendment
				string fileNameOld=_ehrAmendmentCur.FileName;
				try {
					ImageStore.ImportAmdAttach(tempFile,_ehrAmendmentCur);
					SelectTreeNode(null);
					ImageStore.CleanAmdAttach(fileNameOld);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Unable to copy file, May be in use: ") + ex.Message + ": " + tempFile);
					copied = false;
				}
				if(copied) {
					FillDocList(false);
					SelectTreeNode(GetNodeById(MakeIdAmd(_ehrAmendmentCur.EhrAmendmentNum)));
				}
				ImageStore.TryDeleteFile(tempFile
					,actInUseException:(msg) => MsgBox.Show(msg)//Informs user when a 'file is in use' exception occurs.
				);
			}
			else {//regular Images module
				Document doc=null;
				try {
					doc=ImageStore.Import(tempFile,GetCurrentCategory(),_patCur);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Unable to copy file, May be in use: ") + ex.Message + ": " + tempFile);
					copied = false;
				}
				if(copied) {
					FillDocList(false);
					SelectTreeNode(GetNodeById(MakeIdDoc(doc.DocNum)));
					FormDocInfo FormD=new FormDocInfo(_patCur,doc,GetCurrentFolderName(treeDocuments.SelectedNode));
					FormD.ShowDialog(this);//some of the fields might get changed, but not the filename 
					//Customer complained this window was showing up behind OD.  We changed above line to add a parent form as an attempted fix.
					//If this doesn't solve it we can also try adding FormD.BringToFront to see if it does anything.
					if(FormD.DialogResult!=DialogResult.OK) {
						DeleteSelection(false,false,doc);
					}
					else {
						nodeId=MakeIdDoc(doc.DocNum);
						_documentSelected=doc.Copy();
					}
				}
				ImageStore.TryDeleteFile(tempFile
					,actInUseException:(msg) => MsgBox.Show(msg)//Informs user when a 'file is in use' exception occurs.
				);
				//Reselect the last successfully added node when necessary. js This code seems to be copied from import multi.  Simplify it.
				if(doc!=null && !MakeIdDoc(doc.DocNum).Equals(nodeId)) {
					SelectTreeNode(GetNodeById(MakeIdDoc(doc.DocNum)));
				}
				FillDocList(true);
			}
		}

		private void ToolBarSign_Click() {
			if(treeDocuments.SelectedNode==null ||				//No selection
				treeDocuments.SelectedNode.Tag==null ||			//Internal error
				treeDocuments.SelectedNode.Parent==null) {		//This is a folder node.
				return;
			}
			//Show the underlying panel note box while the signature is being filled.
			panelNote.Visible=true;
			ResizeAll();
			//Display the document signature form.
			FormDocSign docSignForm=new FormDocSign(_documentSelected,_patCur);//Updates our local document and saves changes to db also.
			int signLeft=treeDocuments.Left;
			docSignForm.Location=PointToScreen(new Point(signLeft,this.ClientRectangle.Bottom-docSignForm.Height));
			docSignForm.Width=Math.Max(0,Math.Min(docSignForm.Width,pictureBoxMain.Right-signLeft));
			docSignForm.ShowDialog();
			FillDocList(true);
			//Adjust visibility of panel note based on changes made to the signature above.
			SetPanelNoteVisibility(_documentSelected);
			//Resize controls in our window to adjust for a possible change in the visibility of the panel note control.
			ResizeAll();
			//Update the signature and note with the new data.
			FillSignature();
		}

		///<summary>This button is disabled for mounts, in which case this code is never called.</summary>
		private void ToolBarZoom100_Click() {
			_zoomLevel=0;
			_zoomOverall=1;
			_zoomImage=1;
			InvalidateSettings(ImageSettingFlags.NONE,false);//Refresh display.
		}

		///<summary>This button is disabled for mounts, in which case this code is never called.</summary>
		private void ToolBarZoomIn_Click() {
			_zoomLevel++;
			PointF c=new PointF(pictureBoxMain.ClientRectangle.Width/2.0f,pictureBoxMain.ClientRectangle.Height/2.0f);
			PointF p=new PointF(c.X-_pointTranslation.X,c.Y-_pointTranslation.Y);
			_pointTranslation=new PointF(_pointTranslation.X-p.X,_pointTranslation.Y-p.Y);
			_zoomOverall=(float)Math.Pow(2,_zoomLevel);
			InvalidateSettings(ImageSettingFlags.NONE,false);//Refresh display.
		}

		///<summary>This button is disabled for mounts, in which case this code is never called.</summary>
		private void ToolBarZoomOut_Click() {
			_zoomLevel--;
			PointF c=new PointF(pictureBoxMain.ClientRectangle.Width/2.0f,pictureBoxMain.ClientRectangle.Height/2.0f);
			PointF p=new PointF(c.X-_pointTranslation.X,c.Y-_pointTranslation.Y);
			_pointTranslation=new PointF(_pointTranslation.X+p.X/2.0f,_pointTranslation.Y+p.Y/2.0f);
			_zoomOverall=(float)Math.Pow(2,_zoomLevel);
			InvalidateSettings(ImageSettingFlags.NONE,false);//Refresh display.
		}
		#endregion Methods - ToolBarMain

		#region Methods - Private
		///<summary>Resizes all controls in the image module to fit inside the current window, including controls which have varying visibility.</summary>
		private void ResizeAll() {
			treeDocuments.Height=Height-treeDocuments.Top-2;
			pictureBoxMain.Width=Width-pictureBoxMain.Left-4;
			panelNote.Width=pictureBoxMain.Width;
			panelNote.Height=(int)Math.Min(114,Height-pictureBoxMain.Location.Y);
			int panelNoteHeight=(panelNote.Visible?panelNote.Height:0);
			pictureBoxMain.Height=Height-panelNoteHeight-pictureBoxMain.Top;
			if(_webBrowserDocument!=null) {
				_webBrowserDocument.Location=pictureBoxMain.Location;
				_webBrowserDocument.Width=pictureBoxMain.Width;
				_webBrowserDocument.Height=pictureBoxMain.Height;
			}
			panelNote.Location=new Point(pictureBoxMain.Left,Height-panelNoteHeight-1);
			if(_claimPaymentNum!=0 || _ehrAmendmentCur!=null) {//eob or amendment
				ToolBarPaint.Location=new Point(pictureBoxMain.Left,ToolBarPaint.Top);
			}
			else {//ordinary images module
				ToolBarPaint.Location=new Point(sliderBrightnessContrast.Location.X+sliderBrightnessContrast.Width+4,ToolBarPaint.Location.Y);
			}
			ToolBarPaint.Size=new Size(pictureBoxMain.Width-sliderBrightnessContrast.Width-4,ToolBarPaint.Height);
			panelUnderline.Location=new Point(pictureBoxMain.Location.X,panelUnderline.Location.Y);
			panelUnderline.Width=Width-panelUnderline.Location.X;
		}

		///<summary>Sets the panelnote visibility based on the given document's signature data and the current operating system.</summary>
		private void SetPanelNoteVisibility(Document doc) {
			panelNote.Visible=(doc!=null) && (((doc.Note!=null && doc.Note!="") || (doc.Signature!=null && doc.Signature!="")) && 
				(Environment.OSVersion.Platform!=PlatformID.Unix || !doc.SigIsTopaz));
		}

		///<summary></summary>
		private void RefreshModuleData(long patNum) {
			SelectTreeNode(null);//Clear selection and image and reset visibilities.
			if(patNum==0) {
				_familyCur=null;
				_patCur=null;
				return;
			}
			_familyCur=Patients.GetFamily(patNum);
			_patCur=_familyCur.GetPatient(patNum);
			_patFolder=ImageStore.GetPatientFolder(_patCur,ImageStore.GetPreferredAtoZpath());//This is where the pat folder gets created if it does not yet exist.
			if(_patNumLast!=patNum) {
				SecurityLogs.MakeLogEntry(Permissions.ImagesModule,patNum,"");
				_patNumLast=patNum;
			}
			Action actionClosing=null;
			if(CloudStorage.IsCloudStorage) {
				actionClosing=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.g(this,"Loading..."));
			}
			ImageStore.AddMissingFilesToDatabase(_patCur);
			actionClosing?.Invoke();
		}

		private void RefreshModuleScreen() {
			if(this.Enabled && _patCur!=null) {
				//Enable tools which must always be accessible when a valid patient is selected.
				EnableAllTools(true);
				//Item specific tools disabled until item chosen.
				EnableAllTreeItemTools(false);
			}
			else {
				EnableAllTools(false);//Disable entire menu (besides select patient).
			}
			//get the program properties for XVWeb from the cache.
			if(XVWeb.IsDisplayingImagesInProgram && _patCur!=null)
			{
				//start thread to load all apteryx images into OD. 
				_threadImageRequest?.QuitAsync();//If an old thread is still running, we want to make it stop so the new one can run.
				_threadImageRequest=new ODThread(ImagesOnThreadStart,_patCur.Copy());
				_threadImageRequest.AddExceptionHandler(new ODThread.ExceptionDelegate((ex) => {
					if(InvokeRequired) {
						Invoke((Action)(() => FriendlyException.Show(Lan.g(this,"Unable to display Apteryx Images"),ex)));
					}
					else {
						FriendlyException.Show(Lan.g(this,"Unable to display Apteryx Images"),ex);
					}
				}));
				_threadImageRequest.Name="ImageRequestThread";
				_threadImageRequest.GroupName="ImageRequestThread";
				_threadImageRequest.Start(true); //run it once
			}
			ToolBarMain.Invalidate();
			ToolBarPaint.Invalidate();
			FillDocList(false);
		}

		private void ImagesOnThreadStart(ODThread workerThread) {
			Patient patient=(Patient)workerThread.Parameters[0];
			_isFillingXVWebFromThread=true;
			List<ApteryxImage> listAI=new List<ApteryxImage>();
			listAI=XVWeb.GetImagesList(_patCur);
			lock(_apteryxLocker) {
				_listImageDownload=listAI;
			}
			//put images into desired image category folder from property value
			FillTreeXVWebItems(patient.PatNum);
			_isFillingXVWebFromThread=false;
		}	

		///<summary>Applies to all tools.</summary>
		private void EnableAllTools(bool enable) {
			for(int i=0;i<ToolBarMain.Buttons.Count;i++) {
				ToolBarMain.Buttons[i].Enabled=enable;
			}
			if(ToolBarMain.Buttons["Capture"]!=null) {
				ToolBarMain.Buttons["Capture"].Enabled=(ToolBarMain.Buttons["Capture"].Enabled && Environment.OSVersion.Platform!=PlatformID.Unix);
			}
			ToolBarMain.Invalidate();
			for(int i=0;i<ToolBarPaint.Buttons.Count;i++) {
				ToolBarPaint.Buttons[i].Enabled=enable;
			}
			ToolBarPaint.Enabled=enable;
			ToolBarPaint.Invalidate();
			sliderBrightnessContrast.Enabled=enable;
			sliderBrightnessContrast.Invalidate();
		}

		///<summary>Defined this way to force future programming to consider which tools are enabled and disabled for every possible tool in the menu.</summary>
		private void EnableTreeItemTools(bool print,bool delete,bool info,bool copy,bool sign,bool brightAndContrast,bool crop,bool hand,bool zoomIn,bool zoomOut,bool flip,bool rotateL,bool rotateR,bool export) {
			ToolBarMain.Buttons["Print"].Enabled=print;
			ToolBarMain.Buttons["Delete"].Enabled=delete;
			if(ToolBarMain.Buttons["Info"]!=null) {
				ToolBarMain.Buttons["Info"].Enabled=info;
			}
			ToolBarMain.Buttons["Copy"].Enabled=copy;
			if(ToolBarMain.Buttons["Sign"]!=null) {
				ToolBarMain.Buttons["Sign"].Enabled=sign;
			}
			ToolBarMain.Buttons["Export"].Enabled=export;
			ToolBarMain.Invalidate();
			if(ToolBarPaint.Buttons[""]!=null) {
				ToolBarPaint.Buttons["Crop"].Enabled=crop;
			}
			if(ToolBarPaint.Buttons["Hand"]!=null) {
				ToolBarPaint.Buttons["Hand"].Enabled=hand;
			}
			ToolBarPaint.Buttons["ZoomIn"].Enabled=zoomIn;
			ToolBarPaint.Buttons["ZoomOut"].Enabled=zoomOut;
			ToolBarPaint.Buttons["Zoom100"].Enabled=zoomOut;
			if(ToolBarPaint.Buttons["Flip"]!=null) {
				ToolBarPaint.Buttons["Flip"].Enabled=flip;
			}
			if(ToolBarPaint.Buttons["RotateR"]!=null) {
				ToolBarPaint.Buttons["RotateR"].Enabled=rotateR;
			}
			if(ToolBarPaint.Buttons["RotateL"]!=null) {
				ToolBarPaint.Buttons["RotateL"].Enabled=rotateL;
			}
			//Enabled if one tool inside is enabled.
			ToolBarPaint.Enabled=(brightAndContrast||crop||hand||zoomIn||zoomOut||flip||rotateL||rotateR);
			ToolBarPaint.Invalidate();
			sliderBrightnessContrast.Enabled=brightAndContrast;
			sliderBrightnessContrast.Invalidate();
		}

		private void EnableAllTreeItemTools(bool enable) {
			EnableTreeItemTools(enable,enable,enable,enable,enable,enable,enable,enable,enable,enable,enable,enable,enable,enable);
		}

		///<summary>Displays the PDF in a web browser. Downloads the PDF file from the cloud if necessary.</summary>
		private void LoadPdf(string atoZFolder,string atoZFileName,string localPath,ref bool isExportable,string downloadMessage) {
			try {
				_webBrowserDocument=new WebBrowser();
				this.Controls.Add(_webBrowserDocument);
				_webBrowserDocument.Visible=true;
				_webBrowserDocument.Size=pictureBoxMain.Size;
				_webBrowserDocument.Location=pictureBoxMain.Location;
				string pdfFilePath="";
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
					pdfFilePath=ODFileUtils.CombinePaths(atoZFolder,atoZFileName);
				}
				else if(CloudStorage.IsCloudStorage) {
					if(localPath!="") {
						pdfFilePath=localPath;
					}
					else {
						//Download PDF into temp directory for displaying.
						pdfFilePath=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),_documentSelected.DocNum+(_patCur!=null ? _patCur.PatNum.ToString() : "")+".pdf");
						FileAtoZ.Download(FileAtoZ.CombinePaths(atoZFolder,atoZFileName),pdfFilePath,downloadMessage);
					}
				}
				else {
					pdfFilePath=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),_documentSelected.DocNum+(_patCur!=null ? _patCur.PatNum.ToString() : "")+".pdf");
					File.WriteAllBytes(pdfFilePath,Convert.FromBase64String(_documentSelected.RawBase64));
				}
				if(!File.Exists(pdfFilePath)) {
					MessageBox.Show(Lan.g(this,"File not found")+": " + atoZFileName);
				}
				else {
					Application.DoEvents();//Show the browser control before loading, in case loading a large PDF, so the user can see the preview has started without waiting.
					_webBrowserDocument.Navigate(pdfFilePath);//The return status of this function doesn't seem to be helpful.
					_webBrowserFilePath=pdfFilePath;
					pictureBoxMain.Visible=false;
					isExportable=true;
					//We used to delete the pdf as it was no longer needed.  
					//The web browser can take time to load and requires the file to be present when it finishes loading, 
					//so it will get deleted later (either when switching preview images or closing Open Dental
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
				//An exception can happen if they do not have Adobe Acrobat Reader version 8.0 or later installed.
				//Simply ignore this exception and do nothing. We never used to display .pdf files anyway, so we
				//essentially revert back to the old behavior in this case.
			}
		}

		///<summary>When storing PDFs directly in DB/Cloud, we download a temp file to display. This could cause local temp storage bloat if not cleaned 
		///up when tree selection changes. Need to delete the temp file associated to NodeIdentifierOld, which persists even across module changes, so 
		///while changing module will not cause the temp file to delete, returning to the image module or closing OpenDental cleans it up.</summary>
		private void DeleteTempPdf(long docNum) {
			Document doc=Documents.GetByNum(docNum,doReturnNullIfNotFound:true);//Get old document.
			if(doc!=null && Path.GetExtension(doc.FileName).ToLower()==".pdf") {//Adobe acrobat file.
				string pdfFilePath=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),doc.DocNum+(_patCur!=null ? _patCur.PatNum.ToString() : "")+".pdf");
				if(!_suniDeviceControl.IsDisposed){
					_suniDeviceControl.Dispose();
				}
				if(File.Exists(pdfFilePath)){
					try {
						File.Delete(pdfFilePath);//Delete temp file
					}
					catch (Exception ex) {
						ex.DoNothing();
						//Can happen if user is clicking around very quickly and EraseCurrentImages() hasn't quite freed up the file.
						//Do nothing, worst case we orphan a temp pdf that will clean up next time it's previewed.
					}
				}
			}
		}

		/// <summary>Special way of selecting and displaying XVWeb downloaded images</summary>
		private void ShowApteryxImage(TreeNode nodeOver) {
			ApteryxImage img=((ImageNodeTag)nodeOver.Tag).ImgDownload; //cast back to an image to access id,width,height
			Bitmap apiImage=null;
			double fileSizeMB=(double)img.FileSize / 1024 / 1024;
			FormProgress FormP=new FormProgress(maxVal:fileSizeMB);
			FormP.DisplayText="?currentVal MB of ?maxVal MB copied";
			//start the thread that will perform the download
			ODThread threadGetBitmap=new ODThread(new ODThread.WorkerDelegate((o) => {
				apiImage=XVWeb.GetBitmap(img,FormP);
			}));
			threadGetBitmap.AddExceptionHandler(new ODThread.ExceptionDelegate((ex) => {
				if(InvokeRequired) {
						Invoke((Action)(() => FriendlyException.Show(Lan.g(this,"Unable to download image."),ex)));
					}
					else {
						FriendlyException.Show(Lan.g(this,"Unable to download image."),ex);
					}
			}));
			//display the progress dialog to the user:
			threadGetBitmap.Name="DownloadApteryxImage"+img.Id;
			threadGetBitmap.Start(true);
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel) {
				threadGetBitmap.QuitAsync();
				return;
			}
			threadGetBitmap.Join(2000);//give thread some time to finish before trying to display the image. 
			Document newDoc=XVWeb.SaveApteryxImageToDoc(img,apiImage,_patCur);
			if(newDoc!=null) {
				nodeOver.Tag=MakeIdDoc(newDoc.DocNum);
				nodeOver.ImageIndex=2+(int)newDoc.ImgType;
				nodeOver.SelectedImageIndex=nodeOver.ImageIndex;
				SelectTreeNode(nodeOver);
			}
			else {
				treeDocuments.SelectedNode=nodeOver;
				treeDocuments.Invalidate();
				pictureBoxMain.Visible=true;
				EnableAllTreeItemTools(false);
				panelNote.Visible=false;
				ResizeAll();
				_bitmapArrayImagesCur=new Bitmap[] { apiImage };
				EnableTreeItemTools(true,false,true,true,false,false,false,true,true,true,false,false,false,true);
			}
		}

		///<summary>Gets the category folder name for the given document node.</summary>
		private string GetCurrentFolderName(TreeNode node) {
			if(node!=null) {
				while(node.Parent!=null) {//Find the corresponding root level node.
					node=node.Parent;
				}
				return node.Text;
			}
			//We must always return a category if one is available, so that new documents can be properly added.
			if(treeDocuments.Nodes.Count>0) {
				return treeDocuments.Nodes[0].Text;
			}
			return "";
		}

		///<summary>Gets the document category of the current selection. The current selection can be a folder itself, or a document within a folder.</summary>
		private long GetCurrentCategory() {
			//If it's a document category, return the def's primary key so we can differentiate between categories of same name.
			if(treeDocuments.SelectedNode!=null && ((ImageNodeTag)treeDocuments.SelectedNode.Tag).NodeType==EnumImageNodeType.Doc) {
				TreeNode node=treeDocuments.SelectedNode;
				while(node.Parent!=null) {//Find the corresponding root level node.
					node=node.Parent;
				}
				return ((ImageNodeTag)node.Tag).PriKey;
			}
			else { 
				return Defs.GetByExactName(DefCat.ImageCats,GetCurrentFolderName(treeDocuments.SelectedNode));
			}
		}

		///<summary>Returns the current tree node with the given node id.</summary>
		private TreeNode GetNodeById(ImageNodeTag nodeId) {
			return GetNodeById(nodeId,treeDocuments.Nodes);//This defines the root node.
		}

		///<summary>Searches the current object tree for a row which has the given unique document number. This will work for a tree with any number of nested folders, as long as tags are defined only for items which correspond to data rows.</summary>
		private TreeNode GetNodeById(ImageNodeTag nodeId,TreeNodeCollection rootNodes) {
			if(rootNodes==null) {
				return null;
			}
			foreach(TreeNode node in rootNodes) {
				if(node==null) {
					continue;
				}
				if(((ImageNodeTag)node.Tag).Equals(nodeId)) {
					return node;
				}
				//Check the child nodes.
				TreeNode child=GetNodeById(nodeId,node.Nodes);
				if(child!=null) {
					return child;
				}
			}
			return null;
		}

		///<summary></summary>
		private ImageNodeTag MakeIdDoc(long docNum) {
			ImageNodeTag nodeId=new ImageNodeTag();
			nodeId.NodeType=EnumImageNodeType.Doc;
			nodeId.PriKey=docNum;
			return nodeId;
			//return docNum+"*"+mountNum;
		}

		///<summary></summary>
		private ImageNodeTag MakeIdMount(long mountNum) {
			ImageNodeTag nodeId=new ImageNodeTag();
			nodeId.NodeType=EnumImageNodeType.Mount;
			nodeId.PriKey=mountNum;
			return nodeId;
		}

		///<summary></summary>
		private ImageNodeTag MakeIdDef(long defNum) {
			ImageNodeTag nodeId=new ImageNodeTag();
			nodeId.NodeType=EnumImageNodeType.Category;
			nodeId.PriKey=defNum;
			return nodeId;
		}

		///<summary></summary>
		private ImageNodeTag MakeIdEob(long eobAttachNum) {
			ImageNodeTag nodeId=new ImageNodeTag();
			nodeId.NodeType=EnumImageNodeType.Eob;
			nodeId.PriKey=eobAttachNum;
			return nodeId;
		}

		///<summary></summary>
		private ImageNodeTag MakeIdAmd(long ehrAmendmentNum) {
			ImageNodeTag nodeId=new ImageNodeTag();
			nodeId.NodeType=EnumImageNodeType.Amd;
			nodeId.PriKey=ehrAmendmentNum;
			return nodeId;
		}

		///<summary>DO NOT CALL UNLESS THE CURRENTLY SELECTED NODE IS A DOCUMENT NODE. Fills the panelnote control with the current document signature when the panelnote is visible and when a valid document is currently selected.</summary>
		private void FillSignature() {
			textNote.Text="";
			sigBox.ClearTablet();
			if(!panelNote.Visible) {
				return;
			}
			textNote.Text=_documentSelected.Note;
			labelInvalidSig.Visible=false;
			sigBox.Visible=true;
			sigBox.SetTabletState(0);//never accepts input here
			//Topaz box is not supported in Unix, since the required dll is Windows native.
			if(_documentSelected.SigIsTopaz) {
				if(_documentSelected.Signature!=null && _documentSelected.Signature!="") {
					//if(allowTopaz) {	
					sigBox.Visible=false;
					_sigBoxTopaz.Visible=true;
					TopazWrapper.ClearTopaz(_sigBoxTopaz);
					TopazWrapper.SetTopazCompressionMode(_sigBoxTopaz,0);
					TopazWrapper.SetTopazEncryptionMode(_sigBoxTopaz,0);
					TopazWrapper.SetTopazKeyString(_sigBoxTopaz,"0000000000000000");//Clear out the key string
					string keystring=GetHashString(_documentSelected);
					TopazWrapper.SetTopazAutoKeyData(_sigBoxTopaz,keystring);
					TopazWrapper.SetTopazEncryptionMode(_sigBoxTopaz,2);//high encryption
					TopazWrapper.SetTopazCompressionMode(_sigBoxTopaz,2);//high compression
					TopazWrapper.SetTopazSigString(_sigBoxTopaz,_documentSelected.Signature);
					_sigBoxTopaz.Refresh();
					//If sig is not showing, then setting the Key String to the hashed data. This is the way we used to handle signatures.
					if(TopazWrapper.GetTopazNumberOfTabletPoints(_sigBoxTopaz)==0) {
						TopazWrapper.SetTopazKeyString(_sigBoxTopaz,"0000000000000000");//Clear out the key string
						TopazWrapper.SetTopazKeyString(_sigBoxTopaz,keystring);
						TopazWrapper.SetTopazSigString(_sigBoxTopaz,_documentSelected.Signature);
					}
					//If sig is not showing, then try encryption mode 3 for signatures signed with old SigPlusNet.dll.
					if(TopazWrapper.GetTopazNumberOfTabletPoints(_sigBoxTopaz)==0) {
						TopazWrapper.SetTopazEncryptionMode(_sigBoxTopaz,3);//Unknown mode (told to use via TopazSystems)
						TopazWrapper.SetTopazSigString(_sigBoxTopaz,_documentSelected.Signature);
					}
					if(TopazWrapper.GetTopazNumberOfTabletPoints(_sigBoxTopaz)==0) {
						labelInvalidSig.Visible=true;
					}
					//}
				}
			}
			else {//not topaz
				if(_documentSelected.Signature!=null && _documentSelected.Signature!="") {
					sigBox.Visible=true;
					//if(allowTopaz) {	
					_sigBoxTopaz.Visible=false;
					//}
					sigBox.ClearTablet();
					//sigBox.SetSigCompressionMode(0);
					//sigBox.SetEncryptionMode(0);
					sigBox.SetKeyString(GetHashString(_documentSelected));
					//"0000000000000000");
					//sigBox.SetAutoKeyData(ProcCur.Note+ProcCur.UserNum.ToString());
					//sigBox.SetEncryptionMode(2);//high encryption
					//sigBox.SetSigCompressionMode(2);//high compression
					sigBox.SetSigString(_documentSelected.Signature);
					if(sigBox.NumberOfTabletPoints()==0) {
						labelInvalidSig.Visible=true;
					}
					sigBox.SetTabletState(0);//not accepting input.
				}
			}
		}

		private string GetHashString(Document doc) {
			return ImageStore.GetHashString(doc,_patFolder);
		}

		private void UpdateUserOdPrefForImageCat(long defNum,bool isExpand) {
			if(PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)!=2) {//Document tree folders persistent expand/collapse per user.
				return;
			}
			//Calls to Expand() and Collapse() in code cause the TreeDocuments_AfterExpand() and TreeDocuments_AfterCollapse() events to fire.
			//This flag helps us ignore these two events when initializing the tree.
			if(_hasTreePrefsEnabled) {
				return;
			}
			Def defImageCatCur=Defs.GetDefsForCategory(DefCat.ImageCats,true).FirstOrDefault(x => x.DefNum==defNum);
			if(defImageCatCur==null) {
				return;//Should never happen, but if it does, there was something wrong with the treeDocument list, and thus nothing should be changed.
			}
			string defaultValue=defImageCatCur.ItemValue;//Stores the default ItemValue of the definition from the catList.
			string curValue=defaultValue;//Stores the current edited ImageCats to compare to the default.
			if(isExpand && !curValue.Contains("E")) {//Since we are expanding we would like to see if the expand flag is present.
				curValue+="E";//If it is not, add expanded flag.
			}
			else if(!isExpand && curValue.Contains("E")) {//Since we are collapsing we want to see if the expand flag is present.
				curValue=curValue.Replace("E","");//If it is, remove expanded flag.
			}
			//Always delete to remove previous value (prevents duplicates).
			UserOdPrefs.DeleteForFkey(Security.CurUser.UserNum,UserOdFkeyType.Definition,defImageCatCur.DefNum);
			if(defaultValue!=curValue) {//Insert an override in the UserOdPref table, only if the chosen value is different than the default.
				UserOdPref userPrefCur=new UserOdPref();//Preference to be inserted to override.
				userPrefCur.UserNum=Security.CurUser.UserNum;
				userPrefCur.Fkey=defImageCatCur.DefNum;
				userPrefCur.FkeyType=UserOdFkeyType.Definition;
				userPrefCur.ValueString=curValue;
				UserOdPrefs.Insert(userPrefCur);
			}
		}

		///<summary>Invalidates some or all of the image settings.  This will cause those settings to be recalculated, either immediately, or when the current ApplySettings thread is finished.  If supplied settings is ApplySettings.NONE, then that part will be skipped.</summary>
		private void InvalidateSettings(ImageSettingFlags settings,bool reloadZoomTransCrop) {
			bool[] mountIdxsToUpdate=new bool[this._bitmapArrayImagesCur.Length];
			if(_bitmapArrayImagesCur.Length==1) {//An image is currently showing.
				mountIdxsToUpdate[0]=true;//Mark the document to be updated.
			}
			else if(_bitmapArrayImagesCur.Length==4) {//4 bite-wing mount is currently selected.
				if(_idxSelectedInMount>=0) {
					//The current active document will be updated.
					mountIdxsToUpdate[_idxSelectedInMount]=true;
				}
			}
			InvalidateSettings(settings,reloadZoomTransCrop,mountIdxsToUpdate);
		}

		///<summary>Invalidates some or all of the image settings.  This will cause those settings to be recalculated, either immediately, or when the current ApplySettings thread is finished.  If supplied settings is ApplySettings.NONE, then that part will be skipped.</summary>
		private void InvalidateSettings(ImageSettingFlags settings,bool reloadZoomTransCrop,bool[] mountIdxsToUpdate) {
			if(this.InvokeRequired) {
				InvalidateSettingsCallback c=new InvalidateSettingsCallback(InvalidateSettings);
				Invoke(c,new object[] { settings,reloadZoomTransCrop });
				return;
			}
			//Do not allow image rendering when the paint tools are disabled. This will disable the display image when a folder or non-image document is selected, or when no document is currently selected. The ToolBarPaint.Enabled boolean is controlled in SelectTreeNode() and is set to true only if a valid image is currently being displayed.
			if(treeDocuments.SelectedNode==null || treeDocuments.SelectedNode.Tag==null) {
				EraseCurrentImages();
				return;
			}
			ImageNodeTag nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
			if(nodeId.NodeType==EnumImageNodeType.None || nodeId.NodeType==EnumImageNodeType.Category) {
				EraseCurrentImages();
				return;
			}
			if(nodeId.NodeType==EnumImageNodeType.Doc) {
				if(!ToolBarPaint.Enabled) {
					EraseCurrentImages();
					return;
				}
			}
			if(nodeId.NodeType.In(EnumImageNodeType.Doc,EnumImageNodeType.Eob,EnumImageNodeType.Amd,EnumImageNodeType.ApteryxImage)) {
				if(reloadZoomTransCrop) {
					//Reloading the image settings only happens when a new image is selected, pasted, scanned, etc...
					//Therefore, the is no need for any current image processing anymore (it would be on a stale image).
					KillThreadImageUpdate();
					ReloadZoomTransCrop(_intArrayWidthsImagesCur[0],_intArrayHeightsImagesCur[0],_documentSelected,
						new Rectangle(0,0,pictureBoxMain.Width,pictureBoxMain.Height),
						out _zoomImage,out _zoomLevel,out _zoomOverall,out _pointTranslation);
					_rectCrop=new Rectangle(0,0,-1,-1);
				}
			}
			_imageSettingFlagsInvalidated |= settings;
			//DocSelected is an individual document instance. Assigning a new document to DocForSettings here does not 
			//negatively effect our image application thread, because the thread either will keep its current 
			//reference to the old document, or will apply the settings with this newly assigned document. In either
			//case, the output is either what we expected originally, or is a more accurate image for more recent 
			//settings. We lock here so that we are sure that the resulting document and setting tuple represent
			//a single point in time.
			lock(_eventWaitHandleSettings) {//Does not actually lock the EventWaitHandleSettings object, but rather locks the variables in the block.
				_boolArrayMountIdxsFlaggedForUpdate=(bool[])mountIdxsToUpdate.Clone();
				_imageSettingFlagsForSettings=_imageSettingFlagsInvalidated;
				_imageNodeTypeForSettings=((ImageNodeTag)treeDocuments.SelectedNode.Tag).NodeType;
				if(_imageNodeTypeForSettings==EnumImageNodeType.Doc
					|| _imageNodeTypeForSettings==EnumImageNodeType.Mount) 
				{
					_documentForSettings=_documentSelected.Copy();
				}
			}
			//Tell the thread to start processing (as soon as the thread is created, or as soon as otherwise 
			//possible). Set() has no effect if the handle is already signaled.
			_eventWaitHandleSettings.Set();
			if(_threadImageUpdate==null) {//Create the thread if it has not been created, or if it was killed for some reason.
				_threadImageUpdate=new Thread((ThreadStart)(delegate { Worker(); }));
				_threadImageUpdate.IsBackground=true;
				_threadImageUpdate.Start();
			}
			_imageSettingFlagsInvalidated=ImageSettingFlags.NONE;
		}

		///<summary>Handles rendering to the PictureBox of the image in its current state. The image calculations are not performed here, only rendering of the image is performed here, so that we can guarantee a fast display.</summary>
		private void RenderCurrentImage(Document docCopy,int originalWidth,int originalHeight,float zoom,PointF translation) {
			if(!this.Visible) {
				return;
			}
			//Helps protect against simultaneous access to the picturebox in both the main and render worker threads.
			if(pictureBoxMain.InvokeRequired) {
				RenderImageCallback c=new RenderImageCallback(RenderCurrentImage);
				Invoke(c,new object[] { docCopy,originalWidth,originalHeight,zoom,translation });
				return;
			}
			int width=pictureBoxMain.Bounds.Width;
			int height=pictureBoxMain.Bounds.Height;
			if(width<=0 || height<=0) {
				return;
			}
			Bitmap backBuffer=new Bitmap(width,height);
			Graphics g=Graphics.FromImage(backBuffer);
			try {
				g.Clear(pictureBoxMain.BackColor);
				g.Transform=GetScreenMatrix(docCopy,originalWidth,originalHeight,zoom,translation);
				g.DrawImage(_bitmapImageRenderingNow,0,0);
				if(_rectCrop.Width>0 && _rectCrop.Height>0) {//Must be drawn last so it is on top.
					g.ResetTransform();
					g.DrawRectangle(Pens.Blue,_rectCrop);
				}
				g.Dispose();
				//Cleanup old back-buffer.
				if(pictureBoxMain.Image!=null) {
					pictureBoxMain.Image.Dispose();	//Make sure that the calling thread performs the memory cleanup, instead of relying
					//on the memory-manager in the main thread (otherwise the graphics get spotty sometimes).
				}
				pictureBoxMain.Image=backBuffer;
				pictureBoxMain.Refresh();
			}
			catch(Exception) {
				g.Dispose();
			}
			//Tried this.  Program crashes when any small window is dragged across the picturebox.
			//backBuffer.Dispose();
			//backBuffer=null;
		}

		private void DeleteThumbnailImage(Document doc) {
			ImageStore.DeleteThumbnailImage(doc,_patFolder);
		}

		private void SetBrightnessAndContrast() {
			if(_documentSelected.WindowingMax==0) {
				//The document brightness/contrast settings have never been set. By default, we use settings
				//which do not alter the original image.
				sliderBrightnessContrast.MinVal=0;
				sliderBrightnessContrast.MaxVal=255;
			}
			else {
				sliderBrightnessContrast.MinVal=_documentSelected.WindowingMin;
				sliderBrightnessContrast.MaxVal=_documentSelected.WindowingMax;
			}
		}

		private void GetNextUnusedMountItem() {
			//Advance selection box to the location where the next image will capture to.
			if(_idxSelectedInMount<0) {
				_idxSelectedInMount=0;
			}
			int hotStart=_idxSelectedInMount;
			int d=_idxSelectedInMount;
			do {
				if(_documentArrayDocsInMount[_idxSelectedInMount]==null) {
					return;//Found an open frame in the mount.
				}
				_idxSelectedInMount=(_idxSelectedInMount+1)%_documentArrayDocsInMount.Length;
			}
			while(_idxSelectedInMount!=hotStart);
			_idxSelectedInMount=-1;
		}

		///<summary>Kills ImageApplicationThread.  Disposes of both currentImages and ImageRenderingNow.  Does not actually trigger a refresh of the Picturebox, though.</summary>
		private void EraseCurrentImages() {
			KillThreadImageUpdate();//Stop any current access to the current image and render image so we can dispose them.
			pictureBoxMain.Image=null;
			_imageSettingFlagsInvalidated=ImageSettingFlags.NONE;
			if(_bitmapArrayImagesCur!=null) {
				for(int i=0;i<_bitmapArrayImagesCur.Length;i++) {
					if(_bitmapArrayImagesCur[i]!=null) {
						_bitmapArrayImagesCur[i].Dispose();
						_bitmapArrayImagesCur[i]=null;
					}
				}
			}
			if(_bitmapImageRenderingNow!=null) {
				_bitmapImageRenderingNow.Dispose();
				_bitmapImageRenderingNow=null;
			}
			System.GC.Collect();
		}

		///<summary>Takes in a mount object and finds all the images pertaining to the mount, then concatonates them together into one large, unscaled image and returns that image. For use in other modules.</summary>
		public Bitmap CreateMountImage(Mount mount) {
			List<MountItem> mountItems=MountItems.GetItemsForMount(mount.MountNum);
			Document[] documents=Documents.GetDocumentsForMountItems(mountItems);
			Bitmap[] originalImages=ImageStore.OpenImages(documents,_patFolder);
			Bitmap mountImage=new Bitmap(mount.Width,mount.Height);
			ImageHelper.RenderMountImage(mountImage,originalImages,mountItems,documents,-1);
			return mountImage;
		}

		/// <summary>Used with XVWeb bridge to dispaly images in images module</summary>
		private void FillTreeXVWebItems(long patNum) {
			if(InvokeRequired) {
				Invoke((Action)(() => { FillTreeXVWebItems(patNum); }));
				return;
			}
			if(patNum!=_patCur.PatNum) {
				return;//The patient was changed while the thread was getting the images.
			}
			string imageCat=ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.XVWeb),XVWeb.ProgramProps.ImageCategory);
			if(_listImageDownload==null || string.IsNullOrEmpty(imageCat)) {
				return;
			}
			TreeNode apteryxFolderNode=treeDocuments.Nodes[Defs.GetOrder(DefCat.ImageCats,Defs.GetDef(DefCat.ImageCats,PIn.Long(imageCat)).DefNum)];
			List<TreeNode> listApteryxNodes=new List<TreeNode>();
			foreach(TreeNode node in apteryxFolderNode.Nodes) {
				ImageNodeTag nodeTag=((ImageNodeTag)node.Tag);
				if(nodeTag.NodeType==EnumImageNodeType.ApteryxImage) {
					listApteryxNodes.Add(node);
				}
			}
			listApteryxNodes.ForEach(x => apteryxFolderNode.Nodes.Remove(x));
			List<ApteryxImage> listAI=new List<ApteryxImage>();
			lock(_apteryxLocker) {
				listAI=_listImageDownload.DeepCopyList<ApteryxImage,ApteryxImage>(true);
			}
			foreach(ApteryxImage image in listAI) {
				if(Documents.DocExternalExists(image.Id.ToString(),ExternalSourceType.XVWeb)) {
					continue;//don't add the image if it was already saved to the database
				}
				//manually add api image nodes
				TreeNode apiNode = new TreeNode(image.AcquisitionDate.ToShortDateString()+": "+image.FormattedTeeth);
				ImageNodeTag nodeTag=new ImageNodeTag();
				nodeTag.NodeType=EnumImageNodeType.ApteryxImage;
				nodeTag.ImgDownload=image;
				apiNode.Tag=nodeTag;
				apiNode.Name="xvweb"+image.Id;
				apteryxFolderNode.Nodes.Add(apiNode);
			}
		}

		///<summary>Deletes the current selection from the database and refreshes the tree view. Set securityCheck false when creating a new document 
		///that might get cancelled.</summary>
		///<param name="docToDelete">If available, pass in the document to be deleted. It is sometimes necessary to use this if the document is no longer
		///selected, e.g. the image folder it belongs to is now hidden.</param>
		private void DeleteSelection(bool verbose,bool securityCheck,Document docToDelete=null) {
			ImageNodeTag nodeId;
			if(docToDelete==null) {
				if(treeDocuments.SelectedNode==null) {
					MsgBox.Show(this,"No item is currently selected");
					return;//No current selection, or some kind of internal error somehow.
				}
				nodeId=(ImageNodeTag)treeDocuments.SelectedNode.Tag;
				if(nodeId.NodeType==EnumImageNodeType.None) {
					MsgBox.Show(this,"No item is currently selected");
					return;//No current selection, or some kind of internal error somehow.
				}
				if(nodeId.NodeType==EnumImageNodeType.Category) {
					MsgBox.Show(this,"Cannot delete folders");
					return;
				}
			}
			else {
				nodeId=new ImageNodeTag {
					NodeType=EnumImageNodeType.Doc,
					PriKey=docToDelete.DocNum,
				};
			}
			Document doc=null;
			if(nodeId.NodeType==EnumImageNodeType.Doc) {
				doc=docToDelete??Documents.GetByNum(nodeId.PriKey);
				if(securityCheck) {
					if(!Security.IsAuthorized(Permissions.ImageDelete,doc.DateCreated)) {
						return;
					}
				}
				EhrLab lab=EhrLabImages.GetFirstLabForDocNum(doc.DocNum);
				if(lab!=null) {
					string dateSt=lab.ObservationDateTimeStart.PadRight(8,'0').Substring(0,8);//stored in DB as yyyyMMddhhmmss-zzzz
					DateTime dateT=PIn.Date(dateSt.Substring(4,2)+"/"+dateSt.Substring(6,2)+"/"+dateSt.Substring(0,4));
					MessageBox.Show(Lan.g(this,"This image is attached to a lab order for this patient on "+dateT.ToShortDateString()+". "+Lan.g(this,"Detach image from this lab order before deleting the image.")));
					return;
				}
			}
			else if(nodeId.NodeType==EnumImageNodeType.Mount) {
				//no security yet for mounts.
			}
			EnableAllTreeItemTools(false);
			Document[] docs=null;
			bool refreshTree=true;
			if(nodeId.NodeType==EnumImageNodeType.Mount) {
				//Delete the mount object.
				long mountNum=nodeId.PriKey;
				Mount mount=Mounts.GetByNum(mountNum);
				//Delete the mount items attached to the mount object.
				List<MountItem> mountItems=MountItems.GetItemsForMount(mountNum);
				if(_idxSelectedInMount>=0 && _documentArrayDocsInMount[_idxSelectedInMount]!=null) {
					if(verbose) {
						if(!MsgBox.Show(this,true,"Delete mount xray image?")) {
							return;
						}
					}
					_documentSelected=new Document();
					docs=new Document[1] { _documentArrayDocsInMount[_idxSelectedInMount] };
					_documentArrayDocsInMount[_idxSelectedInMount]=null;
					//Release access to current image so it may be properly deleted.
					if(_bitmapArrayImagesCur[_idxSelectedInMount]!=null) {
						_bitmapArrayImagesCur[_idxSelectedInMount].Dispose();
						_bitmapArrayImagesCur[_idxSelectedInMount]=null;
					}
					InvalidateSettings(ImageSettingFlags.ALL,false);
					refreshTree=false;
				}
				else {
					if(verbose) {
						if(!MsgBox.Show(this,true,"Delete entire mount?")) {
							return;
						}
					}
					docs=_documentArrayDocsInMount;
					Mounts.Delete(mount);
					for(int i=0;i<mountItems.Count;i++) {
						MountItems.Delete(mountItems[i]);
					}
					SelectTreeNode(null);//Release access to current image so it may be properly deleted.
				}
			}
			else if(nodeId.NodeType==EnumImageNodeType.Doc) {
				if(verbose) {
					if(!MsgBox.Show(this,true,"Delete document?")) {
						return;
					}
				}
				docs=new Document[1] { doc };
				//Since documents can be associated to a statement we need to detach this document from any statements linked to it to avoid orphaned FKs.
				//If this doucment is not linked to a statement, this method will not alter any statements.
				Statements.DetachDocFromStatements(docs[0].DocNum);
				SelectTreeNode(null);//Release access to current image so it may be properly deleted.
			}
			else if(nodeId.NodeType==EnumImageNodeType.Eob) {
				if(verbose) {
					if(!MsgBox.Show(this,true,"Delete EOB?")) {
						return;
					}
				}
				EobAttach eob=EobAttaches.GetOne(nodeId.PriKey);
				SelectTreeNode(null);//release access
				ImageStore.DeleteEobAttach(eob);
			}
			else if(nodeId.NodeType==EnumImageNodeType.Amd) {
				if(verbose) {
					if(!MsgBox.Show(this,true,"Delete amendment?")) {
						return;
					}
				}
				if(_ehrAmendmentCur==null) {
					return;
				}
				SelectTreeNode(null);//release access
				ImageStore.DeleteAmdAttach(_ehrAmendmentCur);
			}
			if(nodeId.NodeType==EnumImageNodeType.Doc || nodeId.NodeType==EnumImageNodeType.Mount) {
				//Delete all documents involved in deleting this object.
				//ImageStoreBase.verbose=verbose;				
				try {
					ImageStore.DeleteDocuments(docs,_patFolder);
				}
				catch(Exception ex) {  //Image could not be deleted, in use.
					MessageBox.Show(this,ex.Message);
				}
			}
			if(refreshTree) {
				FillDocList(false);
			}
		}

		private void ToolBarWebExport(ImageNodeTag nodeId,Document apteryxDoc) {
			string tempFilePath="";
			string docPath="";
			if(nodeId.NodeType.In(EnumImageNodeType.Doc,EnumImageNodeType.ApteryxImage)) {
				Document doc;
				if(nodeId.NodeType==EnumImageNodeType.Doc) {
					doc=Documents.GetByNum(nodeId.PriKey);
				}
				else {
					doc=apteryxDoc;
				}
				tempFilePath=ODFileUtils.CombinePaths(Path.GetTempPath(),doc.FileName);
				docPath=FileAtoZ.CombinePaths(ImageStore.GetPatientFolder(_patCur,ImageStore.GetPreferredAtoZpath()),doc.FileName);
			}
			else if(nodeId.NodeType==EnumImageNodeType.Eob) {
				EobAttach eob=EobAttaches.GetOne(nodeId.PriKey);
				tempFilePath=ODFileUtils.CombinePaths(Path.GetTempPath(),eob.FileName);
				docPath=ODFileUtils.CombinePaths(ImageStore.GetEobFolder(),eob.FileName);
			}
			else if(nodeId.NodeType==EnumImageNodeType.Amd) {
				EhrAmendment amd=EhrAmendments.GetOne(nodeId.PriKey);
				tempFilePath=ODFileUtils.CombinePaths(Path.GetTempPath(),amd.FileName);
				docPath=ODFileUtils.CombinePaths(ImageStore.GetAmdFolder(),amd.FileName);
			}
			if(!string.IsNullOrEmpty(docPath)) {
				FileAtoZ.Copy(docPath,tempFilePath,FileAtoZSourceDestination.AtoZToLocal,"Exporting file...");
				ThinfinityUtils.ExportForDownload(tempFilePath);
			}
			else {
				MessageBox.Show("Unable to export file");
			}
		}

		///<summary>Returns the index in the DocsInMount array of the given location (relative to the upper left-hand corner of the pictureBoxMain control) or -1 if the location is outside all documents in the current mount. A mount must be currently selected to call this function.</summary>
		private int GetIdxAtMountLocation(Point location) {
			PointF relativeLocation=new PointF(
				(location.X-_pointTranslation.X)/(_zoomImage*_zoomOverall)+_mountSelected.Width/2,
				(location.Y-_pointTranslation.Y)/(_zoomImage*_zoomOverall)+_mountSelected.Height/2);
			//Enumerate the image locations.
			for(int i=0;i<_listMountItemsForSelected.Count;i++) {
				RectangleF itemLocation=new RectangleF(_listMountItemsForSelected[i].Xpos,_listMountItemsForSelected[i].Ypos,
					_listMountItemsForSelected[i].Width,_listMountItemsForSelected[i].Height);
				if(itemLocation.Contains(relativeLocation)) {
					return i;
				}
			}
			return -1;//No document selected in the current mount.
		}

		private PointF MountSpaceToScreenSpace(PointF p) {
			PointF relvec=new PointF(p.X/_mountSelected.Width-0.5f,p.Y/_mountSelected.Height-0.5f);
			return new PointF(_pointTranslation.X+relvec.X*_mountSelected.Width*_zoomImage*_zoomOverall,
				_pointTranslation.Y+relvec.Y*_mountSelected.Height*_zoomImage*_zoomOverall);
		}

		///<summary>Applies crop and colors. Then, paints ImageRenderingNow onto pictureBoxMain.</summary>
		private void Worker() {
			while(true) {
				try {
					//Wait indefinitely for a signal to start processing again. Since the OS handles this function,
					//this thread will not run even a single process cycle until a signal is received. This is ideal,
					//since it means that we do not waste any CPU cycles when image processing is not currently needed.
					//At the same time, this function allows us to keep a single thread for as long as possible, so
					//that we do not need to destroy and recreate this thread (except in rare circumstances, such as
					//the deletion of the current image).
					_eventWaitHandleSettings.WaitOne(-1,false);
					//The DocForSettings may have been reset several times at this point by calls to InvalidateSettings(), but that cannot hurt
					//us here because it simply means that we are getting even more current information than we had when this thread was
					//signaled to start. We lock here so that we are sure that the resulting document and setting tuple represent
					//a single point in time.
					Document docForSettings;
					ImageSettingFlags imageSettingFlagsForSettings;
					bool[] idxDocsFlaggedForUpdate;
					lock(_eventWaitHandleSettings) {//Does not actually lock the EventWaitHandleSettings object.
						docForSettings=_documentForSettings;
						imageSettingFlagsForSettings=_imageSettingFlagsForSettings;
						idxDocsFlaggedForUpdate=_boolArrayMountIdxsFlaggedForUpdate;
					}
					if(_imageNodeTypeForSettings==EnumImageNodeType.Doc) {
						//Perform cropping and colorfunction here if one of the two flags is set. Rotation, flip, zoom and translation are
						//taken care of in RenderCurrentImage().
						if((imageSettingFlagsForSettings & ImageSettingFlags.COLORFUNCTION)!=ImageSettingFlags.NONE || 
								(imageSettingFlagsForSettings & ImageSettingFlags.CROP)!=ImageSettingFlags.NONE) {
							//Ensure that memory management for the ImageRenderingNow is performed in the worker thread, otherwise the main thread
							//will be slowed when it has to cleanup dozens of old renderImages, which causes a temporary pause in operation.
							if(_bitmapImageRenderingNow!=null) {
								//Done like this so that the ImageRenderingNow is cleared in a single atomic line of code (in case the thread is
								//killed during this step), so that we don't end up with a pointer to a disposed image in the ImageRenderingNow.
								Bitmap oldRenderImage=_bitmapImageRenderingNow;
								_bitmapImageRenderingNow=null;
								oldRenderImage.Dispose();
								oldRenderImage=null;
							}
							//currentImages[] is guaranteed to exist and be the current. If currentImages gets updated, this thread 
							//gets aborted with a call to KillMyThread(). The only place currentImages[] is invalid is in a call to 
							//EraseCurrentImage(), but at that point, this thread has been terminated.
							_bitmapImageRenderingNow=ImageHelper.ApplyDocumentSettingsToImage(docForSettings,_bitmapArrayImagesCur[_idxSelectedInMount],
								ImageSettingFlags.CROP | ImageSettingFlags.COLORFUNCTION);
						}
						//Make the current ImageRenderingNow visible in the picture box, and perform rotation, flip, zoom, and translation on
						//the ImageRenderingNow.
						RenderCurrentImage(docForSettings,_intArrayWidthsImagesCur[_idxSelectedInMount],_intArrayHeightsImagesCur[_idxSelectedInMount],_zoomImage*_zoomOverall,_pointTranslation);
					}
					else if(_imageNodeTypeForSettings==EnumImageNodeType.Mount) {
						ImageHelper.RenderMountFrames(_bitmapImageRenderingNow,_listMountItemsForSelected,_idxSelectedInMount);
						//Render only the modified image over the old mount image.
						//A null document can happen when a new image frame is selected, but there is no image in that frame.
						if(docForSettings!=null && imageSettingFlagsForSettings!=ImageSettingFlags.NONE) {
							for(int i=0;i<idxDocsFlaggedForUpdate.Length;i++) {
								if(idxDocsFlaggedForUpdate[i]) {
									//js 11/12/11 I don't understand why we keep reusing the docForSettings for multiple docs.  Revisit this when mounts are enhanced.
									ImageHelper.RenderImageIntoMount(_bitmapImageRenderingNow,_listMountItemsForSelected[i],_bitmapArrayImagesCur[i],docForSettings);
								}
							}
						}
						RenderCurrentImage(new Document(),_bitmapImageRenderingNow.Width,_bitmapImageRenderingNow.Height,_zoomImage*_zoomOverall,_pointTranslation);
					}
					else if(_imageNodeTypeForSettings==EnumImageNodeType.Eob || _imageNodeTypeForSettings==EnumImageNodeType.Amd) {
						if((imageSettingFlagsForSettings & ImageSettingFlags.COLORFUNCTION)!=ImageSettingFlags.NONE || 
							(imageSettingFlagsForSettings & ImageSettingFlags.CROP)!=ImageSettingFlags.NONE) {
							if(_bitmapImageRenderingNow!=null) {
								Bitmap oldRenderImage=_bitmapImageRenderingNow;
								_bitmapImageRenderingNow=null;
								oldRenderImage.Dispose();
								oldRenderImage=null;
							}
							_bitmapImageRenderingNow=ImageHelper.ApplyDocumentSettingsToImage(docForSettings,_bitmapArrayImagesCur[_idxSelectedInMount],
								ImageSettingFlags.CROP | ImageSettingFlags.COLORFUNCTION);
							//ImageRenderingNow=ImagesCur[IdxSelectedInMount];//no crop or color settings in an eob
						}
						RenderCurrentImage(null,_intArrayWidthsImagesCur[_idxSelectedInMount],_intArrayHeightsImagesCur[_idxSelectedInMount],_zoomImage*_zoomOverall,_pointTranslation);
					}
					else if(_imageNodeTypeForSettings==EnumImageNodeType.ApteryxImage) {
						_bitmapImageRenderingNow=_bitmapArrayImagesCur[_idxSelectedInMount];//no crop or color settings in an Apteryx image
						RenderCurrentImage(new Document(),_bitmapImageRenderingNow.Width,_bitmapImageRenderingNow.Height,_zoomImage*_zoomOverall,_pointTranslation);
					}
				}
				catch(ThreadAbortException) {
					return;	//Exit as requested. This can happen when the current document is being deleted, 
					//or during shutdown of the program.
				}
				catch(Exception) {
					//We don't draw anyting on error (because most of the time it will be due to the current selection state).
				}
			}
		}

		///<summary>Kills the image processing thread if it is currently running.</summary>
		private void KillThreadImageUpdate() {
			_suniDeviceControl.KillXRayThread();//Stop the current xRay image thread if it is running.
			if(_threadImageUpdate!=null) {//Clear any previous image processing.
				if(_threadImageUpdate.IsAlive) {
					_threadImageUpdate.Abort();//this is not recommended because it results in an exception.  But it seems to work.
					_threadImageUpdate.Join();//Wait for thread to stop execution.
				}
				_threadImageUpdate=null;
			}
		}

		//Static Functions------------------------------------------------------------------------------------------------------------------------------------------------------
		///<summary>Sets global variables: Zoom, translation, and crop to initial starting values where the image fits perfectly within the box.</summary>
		private static void ReloadZoomTransCrop(int docImageWidth,int docImageHeight,Document doc,Rectangle viewport,
			out float zoom,out int zoomLevel,out float zoomFactor,out PointF translation) {
			//Choose an initial zoom so that the image is scaled to fit the destination box size.
			//Keep in mind that bitmaps are not allowed to have either a width or height of 0,
			//so the following equations will always work. The following subtracts from the 
			//destination box width and height to force a little extra white space.
			RectangleF imageRect=CalcImageDims(docImageWidth,docImageHeight,doc);
			float matchWidth=(int)(viewport.Width*0.975f);
			matchWidth=(matchWidth<=0?1:matchWidth);
			float matchHeight=(int)(viewport.Height*0.975f);
			matchHeight=(matchHeight<=0?1:matchHeight);
			zoom=(float)Math.Min(matchWidth/imageRect.Width,matchHeight/imageRect.Height);
			zoomLevel=0;
			zoomFactor=1;
			translation=new PointF(viewport.Left+viewport.Width/2.0f,viewport.Top+viewport.Height/2.0f);
		}

		///<summary>Calculates the image dimensions after factoring flip and rotation of the given document.</summary>
		private static RectangleF CalcImageDims(int imageWidth,int imageHeight,Document doc) {
			Matrix orientation=GetScreenMatrix(doc,imageWidth,imageHeight,1,new PointF(0,0));
			PointF[] corners=new PointF[] {
				new PointF(-imageWidth/2,-imageHeight/2),
				new PointF(imageWidth/2,-imageHeight/2),
				new PointF(-imageWidth/2,imageHeight/2),
				new PointF(imageWidth/2,imageHeight/2),
			};
			orientation.TransformPoints(corners);
			float minx=corners[0].X;
			float maxx=minx;
			float miny=corners[0].Y;
			float maxy=miny;
			for(int i=1;i<corners.Length;i++) {
				if(corners[i].X<minx) {
					minx=corners[i].X;
				}
				else if(corners[i].X>maxx) {
					maxx=corners[i].X;
				}
				if(corners[i].Y<miny) {
					miny=corners[i].Y;
				}
				else if(corners[i].Y>maxy) {
					maxy=corners[i].Y;
				}
			}
			return new RectangleF(0,0,maxx-minx,maxy-miny);
		}

		///<summary>Converts a screen-space location into a location which is relative to the given document in its unrotated/unflipped/unscaled/untranslated state.</summary>
		private static PointF ScreenPointToUnalteredDocumentPoint(PointF screenLocation,Document doc,
			int docOriginalImageWidth,int docOriginalImageHeight,float imageScale,PointF imageTranslation) {
			Matrix docMat=GetDocumentFlippedRotatedMatrix(doc);
			docMat.Scale(imageScale,imageScale);
			//Now we have a matrix representing the image in its current state-space.
			float[] docMatAxes=docMat.Elements;
			float px=screenLocation.X-imageTranslation.X;
			float py=screenLocation.Y-imageTranslation.Y;
			//The origin of our internal image axis is always relative to the center of the crop rectangle.
			Rectangle docCropRect=DocCropRect(doc,docOriginalImageWidth,docOriginalImageHeight);
			PointF cropRectCenter=new PointF(docCropRect.X+docCropRect.Width/2.0f,
				docCropRect.Y+docCropRect.Height/2.0f);
			return new PointF(
				(cropRectCenter.X+(px*docMatAxes[0]+py*docMatAxes[1])/(imageScale*imageScale)),
				(cropRectCenter.Y+(px*docMatAxes[2]+py*docMatAxes[3])/(imageScale*imageScale)));
		}

		///<summary>Returns a matrix for the given document which represents flipping over the Y-axis before rotating. Of course, if doc.IsFlipped is false, then no flipping is performed, and if doc.DegreesRotated is a multiple of 360 then no rotation is performed.  doc may be null if eob.</summary>
		private static Matrix GetDocumentFlippedRotatedMatrix(Document doc) {
			if(doc==null) {
				return new Matrix(1,0,0,1,0,0);
			}
			Matrix result=new Matrix(
				doc.IsFlipped?-1:1,0,//X-axis
				0,1,//Y-axis
				0,0);//Offset/Translation(dx,dy)
			result.Rotate(doc.IsFlipped?-doc.DegreesRotated:doc.DegreesRotated);
			return result;
		}
		#endregion Methods - Private
	}

	#region Outside Class
	///<summary>Because this is a struct, equivalency is based on values, not references.</summary>
	public struct ImageNodeTag {
		public EnumImageNodeType NodeType;
		///<summary>The table to which the primary key refers will differ based on the node type.</summary>
		public long PriKey;
		public ApteryxImage ImgDownload;
		//could use an == overload here, but don't know syntax right now.
	}

	public enum EnumImageNodeType {
		///<summary>This is the initial empty id.  Used instead of a null ImageNodeId</summary>
		None,
		///<summary>PriKey is DefNum</summary>
		Category,
		///<summary>PriKey is DocNum</summary>
		Doc,
		///<summary>PriKey is MountNum</summary>
		Mount,
		///<summary>PriKey is EobAttachNum</summary>
		Eob,
		///<summary>PriKey is EhrAmendmentNum</summary>
		Amd,
		///<summary>PriKey is 0. The ImgDownload field will have store any information needed.</summary>
		ApteryxImage,
	}
	#endregion Outside Class
}
