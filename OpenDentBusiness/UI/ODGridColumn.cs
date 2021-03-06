using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenDental.UI{ 
	//Jordan is the only one allowed to edit this file.

	///<summary></summary>
	//[DesignTimeVisible(false)]
	//[TypeConverter(typeof(GridColumnTypeConverter))]
	public class GridColumn{
		///<summary>Set this to an event method and it will be used when the column header is clicked.</summary>
		public EventHandler CustomClickEvent;

		#region Constructors
		///<summary>Creates a new ODGridcolumn.</summary>
		public GridColumn(){
		
		}

			///<summary>Creates a new ODGridcolumn with the given heading and width. Alignment left</summary>
		public GridColumn(string heading,int colWidth){
			Heading=heading;
			ColWidth=colWidth;
		}

		///<summary>Creates a new ODGridcolumn with the given heading and width.</summary>
		public GridColumn(string heading,int colWidth,HorizontalAlignment textAlign){
			Heading=heading;
			ColWidth=colWidth;
			TextAlign=textAlign;
		}

		///<summary>Creates a new ODGridcolumn with the given heading and width. Alignment left</summary>
		public GridColumn(string heading,int colWidth,HorizontalAlignment textAlign,bool isEditable) {
			Heading=heading;
			ColWidth=colWidth;
			TextAlign=textAlign;
			IsEditable=isEditable;
		}

		///<summary>Creates a new ODGridcolumn with the given heading and width. Alignment left</summary>
		public GridColumn(string heading,int colWidth,bool isEditable) {
			Heading=heading;
			ColWidth=colWidth;
			IsEditable=isEditable;
		}

		///<summary>Creates a new ODGridcolumn with the given heading, width, and sorting strategy.</summary>
		public GridColumn(string heading,int colWidth,GridSortingStrategy sortingStrategy) {
			Heading=heading;
			ColWidth=colWidth;
			SortingStrategy=sortingStrategy;
		}

		///<summary>Creates a new ODGridcolumn with the given heading, width, and sorting strategy.</summary>
		public GridColumn(string heading,int colWidth,HorizontalAlignment textAlign,GridSortingStrategy sortingStrategy) {
			Heading=heading;
			ColWidth=colWidth;
			TextAlign=textAlign;
			SortingStrategy=sortingStrategy;
		}		
		#endregion Constructors

		#region Properties
		///<summary>Column width, default 80</summary>
		[DefaultValue(80)]
		public int ColWidth { get; set; } = 80;

		///<summary>When combo boxes are used in column cells, this can be set to force a width of dropdown instead of using the column width.</summary>
		[DefaultValue(0)]
		public int DropDownWidth { get; set;} = 0;

		///<summary>String that shows in the top heading cell.</summary>
		[DefaultValue("")]
		public string Heading { get; set; } = "";

		///<summary></summary>
		[DefaultValue(null)]
		public ImageList ImageList { get; set; } = null;

		///<summary>Default false</summary>
		[DefaultValue(false)]
		public bool IsEditable { get; set; } = false;

		///<summary>Can be used when grid.SelectionMode=OneCell. Setting this list of strings causes combo boxes to be used in column cells instead of textboxes.  This is the list of strings to show in the combo boxes.</summary>
		[DefaultValue(null)]
		public List<string> ListDisplayStrings { get; set; } = null;

		///<summary>Default StringCompare</summary>
		[DefaultValue(GridSortingStrategy.StringCompare)]
		public GridSortingStrategy SortingStrategy { get; set; } = GridSortingStrategy.StringCompare;

		///<summary>Attach any object to a column for a variety of reference purposes.</summary>
		[DefaultValue(null)]
		public object Tag { get; set; } = null;

		///<summary>Default Left</summary>
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment TextAlign { get; set; } = HorizontalAlignment.Left;
		#endregion Properties

		public GridColumn Copy() {
			GridColumn retVal=(GridColumn)this.MemberwiseClone();
			if(this.ListDisplayStrings!=null) {
				retVal.ListDisplayStrings=this.ListDisplayStrings.Select(x => new string(x.ToArray())).ToList();
			}
			return retVal;
		}

	}


	public enum GridSortingStrategy {
		///<summary>0- Default</summary>
		StringCompare,
		DateParse,
		ToothNumberParse,
		AmountParse,
		TimeParse,
		VersionNumber,
	}
}






