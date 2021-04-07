using System;
using System.Drawing;

namespace aphrodite {
    public class Metadata {
        public static bool setMetadata(string file, string artistsMetadata = "", string tagsMetadata = "") {
            if (System.IO.File.Exists(file)) {
                Image openImage = Image.FromFile(file);
                System.Drawing.Imaging.PropertyItem imageProperty = openImage.PropertyItems[0];

                imageProperty = openImage.PropertyItems[0];
                SetProperty(ref imageProperty, 315, artistsMetadata);
                openImage.SetPropertyItem(imageProperty);

                imageProperty = openImage.PropertyItems[0];
                SetProperty(ref imageProperty, 270, tagsMetadata);
                openImage.SetPropertyItem(imageProperty);

                openImage.Save(file);
                return true;
            }

            return false;
        }
        public static void SetProperty(ref System.Drawing.Imaging.PropertyItem prop, int iId, string sTxt) {
            int iLen = sTxt.Length + 1;
            byte[] bTxt = new Byte[iLen];
            for (int i = 0; i < iLen - 1; i++)
                bTxt[i] = (byte)sTxt[i];
            bTxt[iLen - 1] = 0x00;
            prop.Id = iId;
            prop.Type = 2;
            prop.Value = bTxt;
            prop.Len = iLen;
        }
    }
}
