using DMSConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace PPDF.TotalAgility.Connector
{
    /// <summary>
    /// Menu item IDs for the TotalAgility connector ribbon.
    /// </summary>
    public enum ItemId
    {
        TotalAgility = 1   // "Send to TotalAgility" ribbon button
    }

    /// <summary>
    /// Defines the static layout of the connector ribbon button.
    /// Text and tooltip are loaded from string resources.
    /// Icons are loaded from bitmap resources — same placeholder names
    /// as the RAI connector; swap the actual bitmaps in Resources.resx
    /// when a custom TotalAgility icon is ready.
    /// </summary>
    public class MenuItemDefinition
    {
        public ItemId id;
        public string resText;
        public string resTooltip;
        public bool isPartOfToolbar;
        public CallbackType cbType;
        public string resIconBig;
        public string resIconBig_150;
        public string resIconBig_200;
        public string resIconSmall;
        public string resIconSmall_150;
        public string resIconSmall_200;
        public bool enabledWithoutDoc;

        public MenuItemDefinition(
            ItemId id, string resText, string resTooltip,
            bool isPartOfToolbar, CallbackType cbType,
            string resIconBig, string resIconBig_150, string resIconBig_200,
            string resIconSmall, string resIconSmall_150, string resIconSmall_200,
            bool enabledWithoutDoc)
        {
            this.id = id;
            this.resText = resText;
            this.resTooltip = resTooltip;
            this.isPartOfToolbar = isPartOfToolbar;
            this.cbType = cbType;
            this.resIconBig = resIconBig;
            this.resIconBig_150 = resIconBig_150;
            this.resIconBig_200 = resIconBig_200;
            this.resIconSmall = resIconSmall;
            this.resIconSmall_150 = resIconSmall_150;
            this.resIconSmall_200 = resIconSmall_200;
            this.enabledWithoutDoc = enabledWithoutDoc;
        }

        /// <summary>
        /// Static ribbon layout definition.
        /// resText / resTooltip are resource string keys resolved at runtime.
        /// Icon resource names match the existing RAI connector placeholders —
        /// replace the bitmaps in Resources.resx when a TA icon is available.
        /// </summary>
        internal static MenuItemDefinition[] menuDefinitions =
        {
            new MenuItemDefinition(
                ItemId.TotalAgility,
                "MenuSendToTotalAgility",           // Resources string key → "Send to TotalAgility"
                "Send To TotalAgility",             // Tooltip (plain string fallback)
                true,                               // Show in toolbar
                CallbackType.CALLBACK_SAVE,        // Triggers DocAddNew — same pattern as RAI connector
                "Image_Save",     "Image_Save_150",      "Image_Save_200",
                "Image_Save_Small","Image_Save_Small_150","Image_Save_Small_200",
                true                                // Enabled even without a document open
            )
        };
    }

    /// <summary>
    /// Typed list of MenuItem objects built from MenuItemDefinition.
    /// Passed to Power PDF via IDMSConnector.MenuGetMenuItem.
    /// </summary>
    public class MenuItemList : List<MenuItem>
    {
        public static MenuItemList Create()
        {
            return new MenuItemList(MenuItemDefinition.menuDefinitions);
        }

        private MenuItemList(MenuItemDefinition[] definitions)
        {
            foreach (MenuItemDefinition def in definitions)
                Add(new MenuItem(def));
        }
    }

    /// <summary>
    /// Resolves a MenuItemDefinition into string and HBITMAP resources
    /// ready for Power PDF's menu API. Adjusts icon resolution based
    /// on display DPI (100% / 150% / 200%).
    /// </summary>
    public class MenuItem
    {
        public int menuItemId;
        public string text;
        public string tooltip;
        public bool isPartOfToolbar;
        public CallbackType cbType;
        public IntPtr hIconBig;
        public IntPtr hIconSmall;
        public bool enabledWithoutDoc;

        private static readonly int LOGPIXELSX = 88;

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        public MenuItem(MenuItemDefinition definition)
        {
            string resBig = definition.resIconBig;
            string resSmall = definition.resIconSmall;

            // Select the correct DPI-appropriate icon set
            using (System.Drawing.Graphics g =
                System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr hdc = g.GetHdc();
                int lx = GetDeviceCaps(hdc, LOGPIXELSX);

                if (lx >= 192)
                {
                    resBig = definition.resIconBig_200;
                    resSmall = definition.resIconSmall_200;
                }
                else if (lx >= 144)
                {
                    resBig = definition.resIconBig_150;
                    resSmall = definition.resIconSmall_150;
                }

                g.ReleaseHdc();
            }

            menuItemId = (int)definition.id;
            text = GetStringResource(definition.resText);
            tooltip = definition.resTooltip;
            isPartOfToolbar = definition.isPartOfToolbar;
            cbType = definition.cbType;
            hIconBig = GetHBitmapResource(resBig);
            hIconSmall = GetHBitmapResource(resSmall);
            enabledWithoutDoc = definition.enabledWithoutDoc;
        }

        private string GetStringResource(string resName)
        {
            if (string.IsNullOrEmpty(resName)) return string.Empty;
            string val = Resources.Resources.ResourceManager.GetString(resName);
            // Fall back to the resource key itself if string not found,
            // so the button always has readable text during development
            return string.IsNullOrEmpty(val) ? resName : val;
        }

        private IntPtr GetHBitmapResource(string resName)
        {
            if (string.IsNullOrEmpty(resName)) return IntPtr.Zero;
            Bitmap bmp = Resources.Resources.ResourceManager
                             .GetObject(resName) as Bitmap;
            return bmp != null ? bmp.GetHbitmap(Color.Black) : IntPtr.Zero;
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        ~MenuItem()
        {
            if (hIconBig != IntPtr.Zero) { DeleteObject(hIconBig); hIconBig = IntPtr.Zero; }
            if (hIconSmall != IntPtr.Zero) { DeleteObject(hIconSmall); hIconSmall = IntPtr.Zero; }
        }
    }
}