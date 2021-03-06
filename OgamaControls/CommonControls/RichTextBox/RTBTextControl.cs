using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;

namespace OgamaControls
{
  /// <summary>
  /// This <strong>RTBTextControl</strong> is derived from <see cref="UserControl"/>.
  /// It hosts an extended <see cref="RichTextBox"/> an exposes an user interface
  /// with a menu to it to handle the standard layout and formatting actions
  /// used in a <see cref="RichTextBox"/>.
  /// You can subscribe to the <see cref="SelChanged"/> event which is raised,
  /// when the selection of the underlying <see cref="RichTextBox"/> has changed
  /// and the <see cref="RtfChanged"/> event which is raised, whenever
  /// the <see cref="RichTextBox"/>s formatted text changed.
  /// </summary>
  [ToolboxBitmap(typeof(RichTextBox))]
  public partial class RTBTextControl : UserControl
  {
    ///////////////////////////////////////////////////////////////////////////////
    // Defining Constants                                                        //
    ///////////////////////////////////////////////////////////////////////////////
    #region CONSTANTS
    #endregion //CONSTANTS

    ///////////////////////////////////////////////////////////////////////////////
    // Defining Variables, Enumerations, Events                                  //
    ///////////////////////////////////////////////////////////////////////////////
    #region FIELDS

    /// <summary>
    /// Saves a temporarily PrintableRichTextBox which is used
    /// in <see cref="GetFontDetails()"/>.
    /// </summary>
    private PrintableRichTextBox rtbTemp=new PrintableRichTextBox();

    /// <summary>
    /// Saves whether the menu should show the toolbars label.
    /// </summary>
    private bool _showToolBarText;

    /// <summary>
    /// Raised in rtb1_SelectionChanged event so that user can do useful things
    /// </summary>
    [Description("Occurs when the selection is changed"),Category("Behavior")]
    public event EventHandler SelChanged;

    /// <summary>
    /// Raised, when the rich text box controls content has changed.
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler RtfChanged;

    #endregion //FIELDS

    ///////////////////////////////////////////////////////////////////////////////
    // Defining Properties                                                       //
    ///////////////////////////////////////////////////////////////////////////////
    #region PROPERTIES

    /// <summary>
    /// Gets or sets the style of the menu toolbar.
    /// </summary>
    [Category("Appearance"),
    Description("Controls the style of the toolbar.")]
    public ToolStripRenderMode ToolbarRenderMode
    {
      get { return  tosMenu.RenderMode; }
      set { tosMenu.RenderMode = value; }
    }

    /// <summary>
    /// Gets or sets the visibility of the menu.
    /// </summary>
    [Category("Appearance"),
    DefaultValue(true),
    Description("Controls whether the editing toolbar should be displayed")]
    public bool ToolbarVisible
    {
      get { return tosMenu.Visible; }
      set { tosMenu.Visible = value; }
    }

    /// <summary>
    /// Gets or sets the Default RichTextBoxs font size.
    /// </summary>
    /// <value>A <see cref="Single"/> with the default em font size.</value>
    [Description("The internal richtextboxs font size."),
    Category("Appearance"),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Single DefaultFontSize
    {
      get { return Convert.ToSingle(cbbFontSize.Text); }
      set { cbbFontSize.Text = value.ToString(); }
    }

    /// <summary>
    /// Gets the RichTextBox that is 
    /// contained with-in the RichTextBoxExtended control
    /// </summary>
    /// <value>A <see cref="PrintableRichTextBox"/> that contains the formatted text.</value>
    [Description("The internal richtextbox control"),
    Category("Internal Controls"),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public PrintableRichTextBox RichTextBox
    {
      get { return rtb1; }
    }

    #region MenuVisibility

    /// <summary>
    /// Gets or sets the visibility of the save button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button save should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the save button or not"),
   DefaultValue(false),
   Category("Appearance")]
    public bool ShowSave
    {
      get { return btnSave.Visible; }
      set { btnSave.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the open button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the open save should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the open button or not"),
   DefaultValue(false),
   Category("Appearance")]
    public bool ShowOpen
    {
      get { return btnOpen.Visible; }
      set { btnOpen.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the color dropdown.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the color dropdown should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the color drop down or not"),
   DefaultValue(false),
   Category("Appearance")]
    public bool ShowColors
    {
      get { return cbbColor.Visible; }
      set { cbbColor.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the undo button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button undo should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the undo button or not"),
   DefaultValue(true),
   Category("Appearance")]
    public bool ShowUndo
    {
      get { return btnUndo.Visible; }
      set { btnUndo.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the redo button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button redo should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the redo button or not"),
    DefaultValue(true),
  Category("Appearance")]
    public bool ShowRedo
    {
      get { return btnRedo.Visible; }
      set { btnRedo.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the bold button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button bold should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the bold button or not"),
   DefaultValue(true),
   Category("Appearance")]
    public bool ShowBold
    {
      get { return btnBold.Visible; }
      set { btnBold.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the italic button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button italic should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the italic button or not"),
   DefaultValue(true),
   Category("Appearance")]
    public bool ShowItalic
    {
      get { return btnItalic.Visible; }
      set { btnItalic.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the underline button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button underline should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the underline button or not"),
    DefaultValue(true),
  Category("Appearance")]
    public bool ShowUnderline
    {
      get { return btnUnderline.Visible; }
      set { btnUnderline.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the strikeout button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button strikeout should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the strikeout button or not"),
    DefaultValue(false),
  Category("Appearance")]
    public bool ShowStrikeout
    {
      get { return btnStrikeout.Visible; }
      set { btnStrikeout.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the align left button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button align left should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the left justify button or not"),
   DefaultValue(true),
   Category("Appearance")]
    public bool ShowLeftJustify
    {
      get { return btnLeftAlign.Visible; }
      set { btnLeftAlign.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the align right button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button align right should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the right justify button or not"),
   DefaultValue(true),
   Category("Appearance")]
    public bool ShowRightJustify
    {
      get { return btnRightAlign.Visible; }
      set { btnRightAlign.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the align center button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button align center should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the center justify button or not"),
   DefaultValue(true),
   Category("Appearance")]
    public bool ShowCenterJustify
    {
      get { return btnCenterAlign.Visible; }
      set { btnCenterAlign.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the font families drop down.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the font families drop down should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the font families drop down or not"),
    DefaultValue(true),
  Category("Appearance")]
    public bool ShowFont
    {
      get { return cbbFontFamilies.Visible; }
      set { cbbFontFamilies.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the font size drop down.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the font size drop down should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the font size drop down or not"),
   DefaultValue(true),
   Category("Appearance")]
    public bool ShowFontSize
    {
      get { return cbbFontSize.Visible; }
      set { cbbFontSize.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the cut button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button cut should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the cut button or not"),
   DefaultValue(true),
   Category("Appearance")]
    public bool ShowCut
    {
      get { return btnCut.Visible; }
      set { btnCut.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the copy button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button copy should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the copy button or not"),
    DefaultValue(true),
  Category("Appearance")]
    public bool ShowCopy
    {
      get { return btnCopy.Visible; }
      set { btnCopy.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the paste button.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the button paste should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the paste button or not"),
    DefaultValue(true),
  Category("Appearance")]
    public bool ShowPaste
    {
      get { return btnPaste.Visible; }
      set { btnPaste.Visible = value; UpdateToolbarSeperators(); }
    }

    /// <summary>
    /// Gets or sets the visibility of the label.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the label should be shown, otherwise <strong>false</strong>.</value>
    [Description("Show the label or not"),
    DefaultValue(true),
  Category("Appearance")]
    public bool ShowLabel
    {
      get { return lblLabel.Visible; }
      set { lblLabel.Visible = value; UpdateToolbarSeperators(); }
    }

    #endregion //MenuVisibility

    #region ContextMenuVisibility

 //   /// <summary>
 //   /// Gets or sets the visibility of the save button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button save should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the save button or not"),
 //  DefaultValue(false),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuSave
 //   {
 //     get { return cmuSave.Visible; }
 //     set { cmuSave.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the open button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the open save should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the open button or not"),
 //  DefaultValue(false),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuOpen
 //   {
 //     get { return cmuOpen.Visible; }
 //     set { cmuOpen.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the color dropdown in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the color dropdown should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the color drop down or not"),
 //  DefaultValue(false),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuColors
 //   {
 //     get { return cmuFontColor.Visible; }
 //     set { cmuFontColor.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the undo button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button undo should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the undo button or not"),
 //  DefaultValue(true),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuUndo
 //   {
 //     get { return cmuUndo.Visible; }
 //     set { cmuUndo.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the redo button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button redo should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the redo button or not"),
 //   DefaultValue(true),
 //Category("ContextMenu")]
 //   public bool ShowContextMenuRedo
 //   {
 //     get { return cmuRedo.Visible; }
 //     set { cmuRedo.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the bold button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button bold should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the bold button or not"),
 //  DefaultValue(true),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuBold
 //   {
 //     get { return cmuBold.Visible; }
 //     set { cmuBold.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the italic button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button italic should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the italic button or not"),
 //  DefaultValue(true),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuItalic
 //   {
 //     get { return cmuItalic.Visible; }
 //     set { cmuItalic.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the underline button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button underline should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the underline button or not"),
 //   DefaultValue(true),
 //Category("ContextMenu")]
 //   public bool ShowContextMenuUnderline
 //   {
 //     get { return cmuUnderline.Visible; }
 //     set { cmuUnderline.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the strikeout button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button strikeout should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the strikeout button or not"),
 //   DefaultValue(false),
 //Category("ContextMenu")]
 //   public bool ShowContextMenuStrikeout
 //   {
 //     get { return cmuStrikeout.Visible; }
 //     set { cmuStrikeout.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the align left button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button align left should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the left justify button or not"),
 //  DefaultValue(true),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuLeftJustify
 //   {
 //     get { return cmuAlignleft.Visible; }
 //     set { cmuAlignleft.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the align right button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button align right should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the right justify button or not"),
 //  DefaultValue(true),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuRightJustify
 //   {
 //     get { return cmuAlignRight.Visible; }
 //     set { cmuAlignRight.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the align center button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button align center should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the center justify button or not"),
 //  DefaultValue(true),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuCenterJustify
 //   {
 //     get { return cmuAlignCenter.Visible; }
 //     set { cmuAlignCenter.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the font families drop down in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the font families drop down should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the font families drop down or not"),
 //   DefaultValue(true),
 //Category("ContextMenu")]
 //   public bool ShowContextMenuFont
 //   {
 //     get { return cbbFontFamilies.Visible; }
 //     set { cbbFontFamilies.Visible = value; UpdateContextMenuSeperators();  }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the font size drop down in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the font size drop down should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the font size drop down or not"),
 //  DefaultValue(true),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuFontSize
 //   {
 //     get { return cbbFontSize.Visible; }
 //     set { cbbFontSize.Visible = value; UpdateContextMenuSeperators();  }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the cut button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button cut should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the cut button or not"),
 //  DefaultValue(true),
 // Category("ContextMenu")]
 //   public bool ShowContextMenuCut
 //   {
 //     get { return cmuCut.Visible; }
 //     set { cmuCut.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the copy button in the context menu.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button copy should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the copy button or not"),
 //   DefaultValue(true),
 //Category("ContextMenu")]
 //   public bool ShowContextMenuCopy
 //   {
 //     get { return cmuCopy.Visible; }
 //     set { cmuCopy.Visible = value; UpdateContextMenuSeperators(); }
 //   }

 //   /// <summary>
 //   /// Gets or sets the visibility of the paste button.
 //   /// </summary>
 //   /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
 //   /// if the button paste should be shown, otherwise <strong>false</strong>.</value>
 //   [Description("Show the paste button or not"),
 //   DefaultValue(true),
 //Category("ContextMenu")]
 //   public bool ShowContextMenuPaste
 //   {
 //     get { return cmuPaste.Visible; }
 //     set { cmuPaste.Visible = value; UpdateContextMenuSeperators(); }
 //   }

        #endregion //ContextMenuVisibility

    /// <summary>
    /// Gets or sets whether the rich text box will detect URLS.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the rich text box detects URLS, otherwise <strong>false</strong>.</value>
    [Description("Detect URLs with-in the richtextbox"),
    Category("Behavior")]
    public bool DetectURLs
    {
      get { return rtb1.DetectUrls; }
      set { rtb1.DetectUrls = value; }
    }

    /// <summary>
    /// Gets or sets if the TAB key moves to the next control or enters a TAB character in the richtextbox.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the if the TAB key enters a TAB character in the richtextbox, otherwise <strong>false</strong>.</value>
    [Description("Determines if the TAB key moves to the next control or enters a TAB character in the richtextbox"),
    DefaultValue(true),
    Category("Behavior")]
    public bool AcceptsTab
    {
      get { return rtb1.AcceptsTab; }
      set { rtb1.AcceptsTab = value; }
    }

    /// <summary>
    /// Gets or sets the auto word selection value.
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the auto word selection is enabled, otherwise <strong>false</strong>.</value>
    [Description("Determines if auto word selection is enabled"),
    Category("Behavior")]
    public bool AutoWordSelection
    {
      get { return rtb1.AutoWordSelection; }
      set { rtb1.AutoWordSelection = value; }
    }

    /// <summary>
    /// Gets or sets the label of the rich text box.
    /// </summary>
    /// <value>A <see cref="string"/> with the controls label.</value>
    [Description("Label of the rich text box to display in the toolbar."),
    Category("Behavior")]
    public string Label
    {
      get { return lblLabel.Text; }
      set { lblLabel.Text = value; }
    }

    /// <summary>
    /// Gets or sets if this control can be edited
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the control can be edited, otherwise <strong>false</strong>.</value>
    [Description("Determines if this control can be edited"),
    Category("Behavior")]
    public bool ReadOnly
    {
      get { return rtb1.ReadOnly; }
      set
      {
        rtb1.Visible = !value;
        rtb1.ReadOnly = value;
      }
    }

    /// <summary>
    /// Determines if the buttons on the toolbar will show there text or not
    /// </summary>
    /// <value>A <see cref="Boolean"/> that is <strong>true</strong>,
    /// if the toolbar buttons will show there text, otherwise <strong>false</strong>.</value>
    [Description("Determines if the buttons on the toolbar will show there text or not"),
    Category("Behavior")]
    public bool ShowToolBarText
    {
      get { return _showToolBarText; }
      set
      {
        _showToolBarText = value;

        if (_showToolBarText)
        {
          btnSave.Text = "Save";
          btnOpen.Text = "Open";
          btnBold.Text = "Bold";
          cbbFontFamilies.Text = "Font";
          cbbColor.Text = "Font Color";
          btnItalic.Text = "Italic";
          btnStrikeout.Text = "Strikeout";
          btnUnderline.Text = "Underline";
          btnLeftAlign.Text = "Left";
          btnCenterAlign.Text = "Center";
          btnRightAlign.Text = "Right";
          btnUndo.Text = "Undo";
          btnRedo.Text = "Redo";
          btnCut.Text = "Cut";
          btnCopy.Text = "Copy";
          btnPaste.Text = "Paste";
        }
        else
        {
          btnSave.Text = "";
          btnOpen.Text = "";
          btnBold.Text = "";
          cbbFontFamilies.Text = "";
          cbbColor.Text = "";
          btnItalic.Text = "";
          btnStrikeout.Text = "";
          btnUnderline.Text = "";
          btnLeftAlign.Text = "";
          btnCenterAlign.Text = "";
          btnRightAlign.Text = "";
          btnUndo.Text = "";
          btnRedo.Text = "";
          btnCut.Text = "";
          btnCopy.Text = "";
          btnPaste.Text = "";
        }

        this.Invalidate();
        this.Update();
      }
    }

    #endregion //PROPERTIES

    ///////////////////////////////////////////////////////////////////////////////
    // Construction and Initializing methods                                     //
    ///////////////////////////////////////////////////////////////////////////////
    #region CONSTRUCTION

    /// <summary>
    /// Constructor.
    /// </summary>
    public RTBTextControl()
    {
      InitializeComponent();
    }

    /// <summary>
    /// <see cref="Form.Load"/> event handler, Initializes UI.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">An empty <see cref="EventArgs"/></param>
    private void RTBTextControl_Load(object sender, EventArgs e)
    {
      //Fill font families.
      InstalledFontCollection installedFontCollection = new InstalledFontCollection();
      foreach (FontFamily family in installedFontCollection.Families)
      {
        cbbFontFamilies.Items.Add(family.Name);
        cmuFont.Items.Add(family.Name);
      }

      cbbFontSize.Text = this.Font.SizeInPoints.ToString();
      cmuFontsize.Text = this.Font.SizeInPoints.ToString();
    }

    /// <summary>
    /// <see cref="Control.Leave"/> event handler 
    /// for the <see cref="Form"/> <see cref="RTBTextControl"/>
    /// Raises the <see cref="RtfChanged"/> event.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void RTBTextControl_Leave(object sender, System.EventArgs e)
    {
      if (this.rtb1.Modified && RtfChanged != null)
        RtfChanged(this, EventArgs.Empty);
    }

    #endregion //CONSTRUCTION

    ///////////////////////////////////////////////////////////////////////////////
    // Eventhandler                                                              //
    ///////////////////////////////////////////////////////////////////////////////
    #region EVENTS

    ///////////////////////////////////////////////////////////////////////////////
    // Eventhandler for UI, Menu, Buttons, Toolbars etc.                         //
    ///////////////////////////////////////////////////////////////////////////////
    #region WINDOWSEVENTHANDLER

    #region Buttons

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnSave"/>
    /// Saves the current RichTextBox content into a file.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnSave_Click(object sender, EventArgs e)
    {
      if (sfd1.ShowDialog() == DialogResult.OK && sfd1.FileName.Length > 0)
        if (System.IO.Path.GetExtension(sfd1.FileName).ToLower().Equals(".rtf"))
          rtb1.SaveFile(sfd1.FileName);
        else
          rtb1.SaveFile(sfd1.FileName, RichTextBoxStreamType.PlainText);
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnOpen"/>
    /// Opens a textual content into the RichTextBox.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnOpen_Click(object sender, EventArgs e)
    {
      try
      {
        if (ofd1.ShowDialog() == DialogResult.OK && ofd1.FileName.Length > 0)
          if (System.IO.Path.GetExtension(ofd1.FileName).ToLower().Equals(".rtf"))
            rtb1.LoadFile(ofd1.FileName, RichTextBoxStreamType.RichText);
          else
            rtb1.LoadFile(ofd1.FileName, RichTextBoxStreamType.PlainText);
      }
      catch (ArgumentException ae)
      {
        if (ae.Message == "Invalid file format.")
          MessageBox.Show("There was an error loading the file: " + ofd1.FileName);
      }
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnBold"/>
    /// Changes the <see cref="FontStyle.Bold"/> style 
    /// of the selected Text in the RichTextBox, referring to the buttons state.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnBold_Click(object sender, EventArgs e)
    {
      if (sender is ToolStripDropDownItem)
      {
        rtb1.ChangeFontStyle(FontStyle.Bold, cmuBold.Checked);
        btnBold.Checked = cmuBold.Checked;
      }
      else
      {
        rtb1.ChangeFontStyle(FontStyle.Bold, btnBold.Checked);
        cmuBold.Checked = btnBold.Checked;
      }
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnItalic"/>
    /// Changes the <see cref="FontStyle.Italic"/> style 
    /// of the selected Text in the RichTextBox, referring to the buttons state.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnItalic_Click(object sender, EventArgs e)
    {
      if (sender is ToolStripDropDownItem)
      {
        rtb1.ChangeFontStyle(FontStyle.Italic, cmuItalic.Checked);
        btnItalic.Checked = cmuItalic.Checked;
      }
      else
      {
        rtb1.ChangeFontStyle(FontStyle.Italic, btnItalic.Checked);
        cmuItalic.Checked = btnItalic.Checked;
      }
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnUnderline"/>
    /// Changes the <see cref="FontStyle.Underline"/> style 
    /// of the selected Text in the RichTextBox, referring to the buttons state.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnUnderline_Click(object sender, EventArgs e)
    {
      if (sender is ToolStripDropDownItem)
      {
        rtb1.ChangeFontStyle(FontStyle.Underline, cmuUnderline.Checked);
        btnUnderline.Checked = cmuUnderline.Checked;
      }
      else
      {
        rtb1.ChangeFontStyle(FontStyle.Underline, btnUnderline.Checked);
        cmuUnderline.Checked = btnUnderline.Checked;
      }
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnStrikeout"/>
    /// Changes the <see cref="FontStyle.Strikeout"/> style 
    /// of the selected Text in the RichTextBox, referring to the buttons state.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnStrikeout_Click(object sender, EventArgs e)
    {
      if (sender is ToolStripDropDownItem)
      {
        rtb1.ChangeFontStyle(FontStyle.Strikeout, cmuStrikeout.Checked);
        btnStrikeout.Checked = cmuStrikeout.Checked;
      }
      else
      {
        rtb1.ChangeFontStyle(FontStyle.Strikeout, btnStrikeout.Checked);
        cmuStrikeout.Checked = btnStrikeout.Checked;
      }
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnLeftAlign"/>
    /// Changes the <see cref="HorizontalAlignment"/> style 
    /// of the selected Text paragraph in the RichTextBox
    /// to <see cref="HorizontalAlignment.Left"/>.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnLeftAlign_Click(object sender, EventArgs e)
    {
      //change horizontal alignment to left
      rtb1.SelectionAlignment = HorizontalAlignment.Left;

      btnCenterAlign.Checked = false;
      btnRightAlign.Checked = false;
      cmuAlignCenter.Checked = false;
      cmuAlignRight.Checked = false;
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnLeftAlign"/>
    /// Changes the <see cref="HorizontalAlignment"/> style 
    /// of the selected Text paragraph in the RichTextBox
    /// to <see cref="HorizontalAlignment.Center"/>.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnCenter_Click(object sender, EventArgs e)
    {
      //change horizontal alignment to center
      btnLeftAlign.Checked = false;
      cmuAlignleft.Checked = false;
      rtb1.SelectionAlignment = HorizontalAlignment.Center;
      btnRightAlign.Checked = false;
      cmuAlignRight.Checked = false;
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnLeftAlign"/>
    /// Changes the <see cref="HorizontalAlignment"/> style 
    /// of the selected Text paragraph in the RichTextBox
    /// to <see cref="HorizontalAlignment.Right"/>.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnRightAlign_Click(object sender, EventArgs e)
    {
      //change horizontal alignment to right
      btnLeftAlign.Checked = false;
      btnCenterAlign.Checked = false;
      rtb1.SelectionAlignment = HorizontalAlignment.Right;
      cmuAlignleft.Checked = false;
      cmuAlignCenter.Checked = false;
    }


    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnUndo"/>
    /// Performs an Undo for the RichTextBox.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnUndo_Click(object sender, EventArgs e)
    {
      rtb1.Undo();
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnRedo"/>
    /// Performs an Redo for the RichTextBox.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnRedo_Click(object sender, EventArgs e)
    {
      rtb1.Redo();
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnCut"/>
    /// Performs an edit->cut operation on the
    /// rich text box control.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnCut_Click(object sender, EventArgs e)
    {
      Cut();
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnCopy"/>
    /// Performs an edit->copy operation on the
    /// rich text box control.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnCopy_Click(object sender, EventArgs e)
    {
      Copy();
    }

    /// <summary>
    /// <see cref="Control.Click"/> event handler 
    /// for the <see cref="Button"/> <see cref="btnPaste"/>
    /// Performs an edit->paste operation on the
    /// rich text box control.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void btnPaste_Click(object sender, EventArgs e)
    {
      Paste();
    }

    #endregion //Buttons

    #region ComboBoxes

    /// <summary>
    /// <see cref="ToolStripColorDropdown.ColorChanged"/> event handler 
    /// for the <see cref="ToolStripColorDropdown"/> <see cref="cbbColor"/>
    /// set the richtextbox color based on the name of the menu item
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void cbbColor_ColorChanged(object sender, EventArgs e)
    {
      ToolStripItem menuControl = (ToolStripItem)sender;
      if (menuControl.Owner is ContextMenuStrip)
      {
        rtb1.ChangeFontColor(cmuFontColor.CurrentColor);
        cbbColor.CurrentColor = cmuFontColor.CurrentColor;
      }
      else
      {
        rtb1.ChangeFontColor(cbbColor.CurrentColor);
        cmuFontColor.CurrentColor = cbbColor.CurrentColor;
      }
    }

    /// <summary>
    /// <see cref="ToolStripComboBox.SelectedIndexChanged"/> event handler 
    /// for the <see cref="ToolStripComboBox"/> <see cref="cbbFontFamilies"/>
    /// Set the font for the entire selection.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void cbbFontFamilies_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        ToolStripItem menuControl = (ToolStripItem)sender;
        if (menuControl.Owner is ContextMenuStrip)
        {
          cbbFontFamilies.SelectedItem = cmuFont.SelectedItem;
        }
        else
        {
          rtb1.ChangeFont(cbbFontFamilies.SelectedItem.ToString());
          cmuFont.SelectedItem = cbbFontFamilies.SelectedItem;
        }
      }
      catch (ArgumentException ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    /// <summary>
    /// <see cref="ToolStripComboBox.SelectedIndexChanged"/> event handler 
    /// for the <see cref="ToolStripComboBox"/> <see cref="cbbFontSize"/>
    /// Change the richtextbox font size.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void cbbFontSize_SelectedIndexChanged(object sender, EventArgs e)
    {
      ParseFontSize(sender);
    }

    /// <summary>
    /// <see cref="ToolStripItem.TextChanged"/> event handler 
    /// for the <see cref="ToolStripComboBox"/> <see cref="cbbFontSize"/>
    /// Change the richtextbox font size.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void cbbFontSize_TextChanged(object sender, EventArgs e)
    {
      ParseFontSize(sender);
    }

    #endregion //ComboBoxes

    #region ForwardedRTB

    /// <summary>
    /// <see cref="ToolStripItem.TextChanged"/> event handler 
    /// for the <see cref="RichTextBox"/> <see cref="rtb1"/>
    /// Raises the <see cref="RtfChanged"/> event.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void rtb1_TextChanged(object sender, EventArgs e)
    {
      if (RtfChanged != null)
        RtfChanged(this, EventArgs.Empty);
    }


    /// <summary>
    /// <see cref="System.Windows.Forms.RichTextBox.LinkClicked"/> event handler 
    /// for the <see cref="RichTextBox"/> <see cref="rtb1"/>.
    /// Starts the default browser if a link is clicked
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">The <see cref="LinkClickedEventArgs"/> with the event data.</param>
    private void rtb1_LinkClicked(object sender, LinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(e.LinkText);
    }


    /// <summary>
    /// <see cref="Control.KeyDown"/> event handler 
    /// for the <see cref="RichTextBox"/> <see cref="rtb1"/>
    /// Checks for style shortcuts like Control-B for bold.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A <see cref="KeyEventArgs"/> with the event data.</param>
    private void rtb1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Modifiers == Keys.Control)
      {
        switch (e.KeyCode)
        {
          case Keys.B:
            btnBold.Checked = !btnBold.Checked;
            break;
          case Keys.I:
            btnItalic.Checked = !btnItalic.Checked;
            break;
          case Keys.U:
            btnUnderline.Checked = !btnUnderline.Checked;
            break;
          case Keys.OemMinus:
            btnStrikeout.Checked = !btnStrikeout.Checked;
            break;
        }
      }

      if (e.KeyCode == Keys.Tab)
        rtb1.SelectedText = "\t";

      rtb1.PrintableRichTextBox_KeyDown(sender, e);
    }

    /// <summary>
    /// <see cref="Control.KeyPress"/> event handler 
    /// for the <see cref="RichTextBox"/> <see cref="rtb1"/>
    /// Stops Ctrl+I from inserting a tab (char HT) into the richtextbox
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A <see cref="KeyPressEventArgs"/> with the event data.</param>
    private void rtb1_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int)e.KeyChar == 9)
        e.Handled = true;
    }

    #endregion //ForwardedRTB

    #endregion //WINDOWSEVENTHANDLER

    ///////////////////////////////////////////////////////////////////////////////
    // Eventhandler for Custom Defined Events                                    //
    ///////////////////////////////////////////////////////////////////////////////
    #region CUSTOMEVENTHANDLER

    /// <summary>
    ///	Change the toolbar buttons when new text is selected
    ///	and raise event SelChanged
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">A empty <see cref="EventArgs"/></param>
    private void rtb1_SelectionChanged(object sender, EventArgs e)
    {
      //Update the toolbar buttons
      UpdateToolbar();

      //Send the SelChangedEvent
      if (SelChanged != null)
        SelChanged(this, e);
    }



    #endregion //CUSTOMEVENTHANDLER

    #endregion //EVENTS

    ///////////////////////////////////////////////////////////////////////////////
    // Methods and Eventhandling for Background tasks                            //
    ///////////////////////////////////////////////////////////////////////////////
    #region BACKGROUNDWORKER
    #endregion //BACKGROUNDWORKER

    ///////////////////////////////////////////////////////////////////////////////
    // Inherited methods                                                         //
    ///////////////////////////////////////////////////////////////////////////////
    #region OVERRIDES
    #endregion //OVERRIDES

    ///////////////////////////////////////////////////////////////////////////////
    // Methods for doing main class job                                          //
    ///////////////////////////////////////////////////////////////////////////////
    #region METHODS

    /// <summary>
    /// Update the toolbar button statuses
    /// </summary>
    public void UpdateToolbar()
    {
      // Get the font, fontsize and style to apply to the toolbar buttons
      Font fnt = GetFontDetails();
      // Set font style buttons to the styles applying to the entire selection
      FontStyle style = fnt.Style;

      //Set all the style buttons using the gathered style
      btnBold.Checked = fnt.Bold;
      btnItalic.Checked = fnt.Italic;
      btnUnderline.Checked = fnt.Underline;
      btnStrikeout.Checked = fnt.Strikeout; //strikeout button
      btnLeftAlign.Checked = (rtb1.SelectionAlignment == HorizontalAlignment.Left); //justify left
      btnCenterAlign.Checked = (rtb1.SelectionAlignment == HorizontalAlignment.Center); //justify center
      btnRightAlign.Checked = (rtb1.SelectionAlignment == HorizontalAlignment.Right); //justify right


      //Check the correct color
      cbbColor.CurrentColor = rtb1.SelectionColor;

      cbbFontFamilies.SelectedItem = fnt.FontFamily.Name;

      //Check the correct font size
      // This is temporarily disabled, because when the
      // cbbFontSize is active and we are selecting the text, it will shrink down to
      // a size of 8.25 pts.
      //cbbFontSize.Text = fnt.SizeInPoints.ToString();
    }

    /// <summary>
    /// Updates the separators of the menu items
    /// according to visibility of buttons.
    /// </summary>
    private void UpdateToolbarSeperators()
    {

      //Font & Font Size
      if (!cbbFontFamilies.Visible && !cbbFontSize.Visible && !cbbColor.Visible)
        toolStripSeparator2.Visible = false;
      else
        toolStripSeparator2.Visible = true;

      //Bold, Italic, Underline
      if (!btnBold.Visible && !btnItalic.Visible && !btnUnderline.Visible && !btnStrikeout.Visible)
        toolStripSeparator3.Visible = false;
      else
        toolStripSeparator3.Visible = true;

      //Left, Center, & Right
      if (!btnLeftAlign.Visible && !btnCenterAlign.Visible && !btnRightAlign.Visible)
        toolStripSeparator4.Visible = false;
      else
        toolStripSeparator4.Visible = true;

      //Undo & Redo
      if (!btnUndo.Visible && !btnRedo.Visible)
        toolStripSeparator5.Visible = false;
      else
        toolStripSeparator5.Visible = true;

      //Cut Copy & Paste
      if (!btnCut.Visible && !btnCopy.Visible && !btnPaste.Visible)
        toolStripSeparator1.Visible = false;
      else
        toolStripSeparator1.Visible = true;

    }

    /// <summary>
    /// Updates the separators of the context menu items
    /// according to visibility of buttons.
    /// </summary>
    private void UpdateContextMenuSeperators()
    {
      //Font & Font Size
      if (!cmuFont.Visible && !cmuFontsize.Visible && !cmuFontColor.Visible)
        cmuSepAfterColor.Visible = false;
      else
        cmuSepAfterColor.Visible = true;

      //Bold, Italic, Underline
      if (!cmuBold.Visible && !cmuItalic.Visible && !cmuUnderline.Visible && !cmuStrikeout.Visible)
        cmuSepAfterStrikeout.Visible = false;
      else
        cmuSepAfterStrikeout.Visible = true;

      //Left, Center, & Right
      if (!cmuAlignleft.Visible && !cmuAlignCenter.Visible && !cmuAlignRight.Visible)
        cmuSepAfterAlignRight.Visible = false;
      else
        cmuSepAfterAlignRight.Visible = true;

      //Undo & Redo
      if (!cmuUndo.Visible && !cmuRedo.Visible)
        cmuSepAfterRedo.Visible = false;
      else
        cmuSepAfterRedo.Visible = true;

      //Cut Copy & Paste
      if (!btnCut.Visible && !btnCopy.Visible && !btnPaste.Visible)
        cmuSepAfterPaste.Visible = false;
      else
        cmuSepAfterPaste.Visible = true;
    }


    /// <summary>
    /// Pastes the content of the clipboard into the rich text box control.
    /// </summary>
    public void Paste()
    {
      try
      {
        rtb1.Paste();
      }
      catch
      {
        MessageBox.Show("Paste Failed");
      }
    }

    /// <summary>
    /// Copys the selected content of the rich text box
    /// into the clipboard.
    /// </summary>
    public void Copy()
    {
      if (rtb1.SelectedText.Length > 0)
        rtb1.Copy();
    }

    /// <summary>
    /// Cuts the selected content of the rich text box
    /// into the clipboard.
    /// </summary>
    public void Cut()
    {
      if (rtb1.SelectedText.Length > 0)
        rtb1.Cut();
    }

    #endregion //METHODS

    ///////////////////////////////////////////////////////////////////////////////
    // Small helping Methods                                                     //
    ///////////////////////////////////////////////////////////////////////////////
    #region HELPER

    /// <summary>
    /// Returns a Font with:
    ///   1) The font applying to the entire selection, if none is the default font. 
    ///   2) The font size applying to the entire selection, if none is the size of the default font.
    ///   3) A style containing the attributes that are common to the entire selection, default regular.
    /// </summary>		
    /// <returns>The <see cref="Font"/> that is currently selected.</returns>
    public Font GetFontDetails()
    {
      //This method should handle cases that occur when multiple fonts/styles are selected

      int rtb1start = rtb1.SelectionStart;
      int len = rtb1.SelectionLength;
      int rtbTempStart = 0;

      if (len <= 1)
      {
        // Return the selection or default font
        if (rtb1.SelectionFont != null)
          return rtb1.SelectionFont;
        else
          return rtb1.Font;
      }

      // Step through the selected text one char at a time	
      // after setting defaults from first char
      rtbTemp.Rtf = rtb1.SelectedRtf;

      //Turn everything on so we can turn it off one by one
      FontStyle replystyle =
        FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout | FontStyle.Underline;

      // Set reply font, size and style to that of first char in selection.
      rtbTemp.Select(rtbTempStart, 1);
      string replyfont = rtbTemp.SelectionFont.Name;
      float replyfontsize = rtbTemp.SelectionFont.Size;
      replystyle = replystyle & rtbTemp.SelectionFont.Style;

      // Search the rest of the selection
      for (int i = 1; i < len; ++i)
      {
        rtbTemp.Select(rtbTempStart + i, 1);

        // Check reply for different style
        replystyle = replystyle & rtbTemp.SelectionFont.Style;

        // Check font
        if (replyfont != rtbTemp.SelectionFont.FontFamily.Name)
          replyfont = "";

        // Check font size
        if (replyfontsize != rtbTemp.SelectionFont.Size)
          replyfontsize = (float)0.0;
      }

      // Now set font and size if more than one font or font size was selected
      if (replyfont == "")
        replyfont = rtbTemp.Font.FontFamily.Name;

      if (replyfontsize == 0.0)
        replyfontsize = rtbTemp.Font.Size;

      // generate reply font
      Font reply
        = new Font(replyfont, replyfontsize, replystyle);

      return reply;
    }

    /// <summary>
    /// This method parses the drop down value
    /// for the fontsize and updates the selection accordingly.
    /// </summary>
    /// <param name="sender">The sender of the new size, could 
    /// be the context menu or the menu.</param>
    private void ParseFontSize(object sender)
    {
      try
      {
        ToolStripItem menuControl = (ToolStripItem)sender;
        if (menuControl.Owner is ContextMenuStrip)
        {
          cbbFontSize.Text = cmuFontsize.Text;
        }
        else
        {
          rtb1.ChangeFontSize(float.Parse(cbbFontSize.Text));
          cmuFontsize.Text = cbbFontSize.Text;
        }
      }
      catch (Exception)
      {
      }
    }

    #endregion //HELPER
  }
}
