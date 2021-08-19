using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Baraka.Data.Quran
{
    // TODO
    // Instead of using the kind fahd complex fonts as direct TTF files in quran/mushaf/fonts,
    // store them in a mushaf.ser file and load the dynamically at runtime using PrivateFontCollection.
    
    /*
    public class MushafDataContainer
    {
        private Dictionary<string, FontFamily> _familiesDict;
        
        public MushafDataContainer(Dictionary<string, byte[]> kingFonts)
        {
            _familiesDict = new Dictionary<string, FontFamily>();

            foreach (KeyValuePair<string, byte[]> entry in kingFonts)
            {
                string fontName = entry.Key;
                FontFamily fam = ByteArrToFontFamily(entry.Value);
                _familiesDict.Add(fontName, fam);
            }
        }

        public FontFamily GetFamily(string name)
        {
            return _familiesDict[name];
        }

        #region Utils
        // SOF user4136548
        private System.Windows.Media.FontFamily ByteArrToFontFamily(byte[] arr)
        {
            IntPtr data = Marshal.AllocCoTaskMem(arr.Length);
            Marshal.Copy(arr, 0, data, arr.Length);

            var pfc = new PrivateFontCollection();
            pfc.AddMemoryFont(data, arr.Length);

            Marshal.FreeCoTaskMem(data);

            return new System.Windows.Media.FontFamily(pfc.Families[0].Name);
        }
        #endregion
    }
    */
}
