using DMSConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace PPDF.TotalAgility.Connector
{
    public enum ItemId
    {
        
        TotalAgility = 1,   // "Send" ribbon button
        Configure = 2    // "Configure" ribbon button
    }

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

        internal static MenuItemDefinition[] menuDefinitions =
        {
            // Button 1 — Send
            new MenuItemDefinition(
                ItemId.TotalAgility,
                "MenuSendToTotalAgility",
                "Send document to TotalAgility",
                true,
                CallbackType.CALLBACK_SAVE,
                //"Image_Open", "Image_Open_150", "Image_Open_200",
                //"Image_Open_Small", "Image_Open_Small_150", "Image_Open_Small_200",
                "Image_TA",      "Image_TA_150",       "Image_TA_200",
                "Image_TA_Small","Image_TA_Small_150",  "Image_TA_Small_200",
                true
            ),
            // Button 2 — Configure
            new MenuItemDefinition(
                ItemId.Configure,
                "MenuConfigureTotalAgility",
                "Configure TotalAgility process and variables",
                false,
                CallbackType.CALLBACK_MENUITEM,
                "Image_TA_Configure",      "Image_TA_Configure_150",       "Image_TA_Configure_200",
                "Image_TA_Configure_Small","Image_TA_Configure_Small_150",  "Image_TA_Configure_Small_200",
                true
            )
        };
    }

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
            return string.IsNullOrEmpty(val) ? resName : val;
        }

        private IntPtr GetHBitmapResource(string resName)
        {
            if (String.IsNullOrEmpty(resName))
                return IntPtr.Zero;

            Bitmap bmp = Resources.Resources.ResourceManager.GetObject(resName) as Bitmap;
            if (bmp != null)
                return bmp.GetHbitmap(Color.Transparent);
            else
                return IntPtr.Zero;
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