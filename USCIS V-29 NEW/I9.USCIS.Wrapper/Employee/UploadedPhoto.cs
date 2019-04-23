using System;
using System.Text;
using System.IO;
using System.Drawing;

namespace I9.USCIS.Wrapper.Employee
{
    public class UploadedPhoto
    {

        #region Members

        private string _UID = null;
        private byte[] _GIFToSend = null;
        private string _FName = null;
        private byte[] _DocGIF = null;

        #endregion

        #region Constructor

        internal UploadedPhoto(Sql.spUSCISPhoto_Get sp)
        {
            if (sp == null) throw new ArgumentNullException("spUSCISPhoto_Get is null");

            
            _FName = (Convert.ToString(sp.GetDataReaderValue(0, "")));
            if (Convert.ToString(sp.GetDataReaderValue(1, "")).Length > 0) { _DocGIF = (byte[])sp.GetDataReaderValue(1, ""); }
            _UID = (Convert.ToString(sp.GetDataReaderValue(2, "")));

            if (_FName.ToUpper().IndexOf("PDF") > 0)
            {
                String pdfconvoutput = ConvertPDFToImage(_DocGIF, _UID);
                Bitmap myImage = new Bitmap(pdfconvoutput);

                MemoryStream mems = new System.IO.MemoryStream();
                myImage.Save(mems, System.Drawing.Imaging.ImageFormat.Gif);

                _GIFToSend = mems.GetBuffer();   //send this to E-Verify

                myImage.Dispose();
                mems.Dispose();
                System.IO.File.Delete(pdfconvoutput);
            }
            // convert jpg to gif
            else if (_FName.ToUpper().IndexOf("JPG") > 0)
            {

                MemoryStream memOrig = new MemoryStream((byte[])_DocGIF);
                Bitmap OriginalImage = new Bitmap(memOrig);

                MemoryStream mems = new System.IO.MemoryStream();
                OriginalImage.Save(mems, System.Drawing.Imaging.ImageFormat.Gif);

                _GIFToSend = mems.GetBuffer();   //send this to E-Verify

                OriginalImage.Dispose();
                mems.Dispose();
                //System.IO.File.Delete(pdfconvoutput);
            }
            else if (_FName.ToUpper().IndexOf("BMP") > 0)
            {

                MemoryStream memOrig = new MemoryStream((byte[])_DocGIF);
                Bitmap OriginalImage = new Bitmap(memOrig);

                MemoryStream mems = new System.IO.MemoryStream();
                OriginalImage.Save(mems, System.Drawing.Imaging.ImageFormat.Gif);

                _GIFToSend = mems.GetBuffer();   //send this to E-Verify

                OriginalImage.Dispose();
                mems.Dispose();
                //System.IO.File.Delete(pdfconvoutput);
            }
            else if (_FName.ToUpper().IndexOf("TIF") > 0)
            {

                MemoryStream memOrig = new MemoryStream((byte[])_DocGIF);
                Bitmap OriginalImage = new Bitmap(memOrig);

                MemoryStream mems = new System.IO.MemoryStream();
                OriginalImage.Save(mems, System.Drawing.Imaging.ImageFormat.Gif);

                _GIFToSend = mems.GetBuffer();   //send this to E-Verify

                OriginalImage.Dispose();
                mems.Dispose();
                //System.IO.File.Delete(pdfconvoutput);
            }
            else if (_FName.ToUpper().IndexOf("TIFF") > 0)
            {

                MemoryStream memOrig = new MemoryStream((byte[])_DocGIF);
                Bitmap OriginalImage = new Bitmap(memOrig);

                MemoryStream mems = new System.IO.MemoryStream();
                OriginalImage.Save(mems, System.Drawing.Imaging.ImageFormat.Gif);

                _GIFToSend = mems.GetBuffer();   //send this to E-Verify

                OriginalImage.Dispose();
                mems.Dispose();
                //System.IO.File.Delete(pdfconvoutput);
            }
            else
            {
                _GIFToSend = _DocGIF;

            }


        }//end internal uploadedphoto constructor

        #endregion

        #region Methods - Private

        private string ConvertPDFToImage(byte[] data, string UID)
        {

            String filename = "EverifyPhoto\\" + UID + ".pdf";
            //String filename = "c:\\Dev\\" + UID + ".pdf";

            FileStream fs = new System.IO.FileStream(filename, FileMode.Create, FileAccess.Write);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            PdfToImage.PDFConvert converter = new PdfToImage.PDFConvert();
            converter.OutputToMultipleFile = false;

            converter.FirstPageToConvert = -1;
            converter.LastPageToConvert = -1;
            converter.FitPage = false;

            converter.JPEGQuality = 10;
            converter.OutputFormat = "tifflzw";
            System.IO.FileInfo input = new FileInfo(filename);
            string output = string.Format("{0}\\{1}{2}", input.Directory, input.Name, ".tif");
            //If the output file exist alrady be sure to add a random name at the end until is unique!
            while (System.IO.File.Exists(output))
            {
                output = output.Replace(".tif", string.Format("{1}{0}", ".tif", DateTime.Now.Ticks));
            }


            converter.Convert(input.FullName, output);

            System.IO.File.Delete(filename);

            return output;

        }

        #endregion

        #region Properties - Public

        public byte[] GIFToSend { get { return _GIFToSend; } }
        public string FName { get { return _FName; } }

        #endregion

    }//end class
}//end namespace
