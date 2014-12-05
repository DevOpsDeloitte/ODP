#define INCLUDE_WEB_FUNCTIONS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.IO;

namespace ODPTaxonomyUtility_TT
{
    public class CreateExcelFile
    {
        
        public static void CreateExcelDocument<T>(List<T> list, string xlsxFilePath)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ListToDataTable(list));

            CreateExcelDocument(ds, xlsxFilePath);
        }
        #region HELPER_FUNCTIONS
        //  This function is adapated from: http://www.codeguru.com/forum/showthread.php?t=450171
        
        public static DataTable ListToDataTable<T>(List<T> list, string tableName = "")
        {
            DataTable dt = new DataTable();
            dt.TableName = tableName;
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }
        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }

        public static void CreateExcelDocument(DataTable dt, string xlsxFilePath)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            CreateExcelDocument(ds, xlsxFilePath);
            ds.Tables.Remove(dt);
            
        }
        #endregion

#if INCLUDE_WEB_FUNCTIONS
        /// <summary>
        /// Create an Excel file, and write it out to a MemoryStream (rather than directly to a file)
        /// </summary>
        /// <param name="dt">DataTable containing the data to be written to the Excel.</param>
        /// <param name="filename">The filename (without a path) to call the new Excel file.</param>
        /// <param name="Response">HttpResponse of the current page.</param>
        /// <returns>True if it was created succesfully, otherwise false.</returns>
        public static void CreateExcelDocument(DataTable dt, string filename, System.Web.HttpResponse Response)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                CreateExcelDocumentAsStream(ds, filename, Response);
                ds.Tables.Remove(dt);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateExcelDocument<T>(List<T> list, string filename, System.Web.HttpResponse Response)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(ListToDataTable(list));
                CreateExcelDocumentAsStream(ds, filename, Response);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet CreateExcelDocument<T>(List<T> list, System.Web.HttpResponse Response, string tableName, DataSet ds)
        {
            try
            {

                ds.Tables.Add(ListToDataTable(list, tableName));
                
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Create an Excel file, and write it out to a MemoryStream (rather than directly to a file)
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="filename">The filename (without a path) to call the new Excel file.</param>
        /// <param name="Response">HttpResponse of the current page.</param>
        /// <returns>Either a MemoryStream, or NULL if something goes wrong.</returns>
        public static void CreateExcelDocumentAsStream(DataSet ds, string filename, System.Web.HttpResponse Response)
        {
            try
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
                {
                    WriteExcelFile(ds, document);
                }
                /* Code for POST Back button option: START */
                //stream.Flush();
                //stream.Position = 0;

                //Response.ClearContent();
                //Response.Clear();
                //Response.Buffer = true;
                //Response.Charset = "";

                ////  NOTE: If you get an "HttpCacheability does not exist" error on the following line, make sure you have
                ////  manually added System.Web to this project's References.

                //Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                //Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //byte[] data1 = new byte[stream.Length];
                //stream.Read(data1, 0, data1.Length);
                //stream.Close();
                //Response.BinaryWrite(data1);
                //Response.Flush();
                //Response.End();
                /* Code for POST Back button option:END */

                /* Code for client-side option: START */
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                stream.WriteTo(Response.OutputStream);
                //Response.End();
                /* Use 3 lines below instead of Response.End() which always throws an exception */
                Response.Flush(); // Sends all currently buffered output to the client.
                Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all 
                /* Code for client-side option: START */
                
            }
            catch (Exception ex)
            {
                throw ex;
                
            }
        }
#endif      //  End of "INCLUDE_WEB_FUNCTIONS" section

        /// <summary>
        /// Create an Excel file, and write it to a file.
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="excelFilename">Name of file to be written.</param>
        /// <returns>True if successful, false if something went wrong.</returns>
        public static void CreateExcelDocument(DataSet ds, string excelFilename)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
                {
                    //WriteExcelFile(ds, document);
                    WriteExcelFileSAX(ds, document);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void WriteExcelFileSAX(DataSet ds, SpreadsheetDocument myDoc)
        {

            List<OpenXmlAttribute> xmlAttributes;
            OpenXmlWriter writer;
            uint worksheetNumber = 1;

            //add a workbookpart manually
            myDoc.AddWorkbookPart();
            WorksheetPart worksheetpart = myDoc.WorkbookPart.AddNewPart<WorksheetPart>();

            //create an XML writer for the worksheetpart
            writer = OpenXmlWriter.Create(worksheetpart);
            writer.WriteStartElement(new Worksheet());
            writer.WriteStartElement(new SheetData());
            

            
            foreach (DataTable dt in ds.Tables)
            {

            


            //DataTable dt = ds.Tables[0];            

            int rowNumber = 1;
            int numberOfColumns = dt.Columns.Count;

            foreach (DataRow dr in dt.Rows)
            {
                

                xmlAttributes = new List<OpenXmlAttribute>();
                // this is the row index
                xmlAttributes.Add(new OpenXmlAttribute("r", null, rowNumber.ToString()));

                //write the row start element with the attributes added above
                writer.WriteStartElement(new Row(), xmlAttributes);

                for (int columnNumber = 0; columnNumber < numberOfColumns; columnNumber++)
                {
                    string cellValue = dr.ItemArray[columnNumber].ToString();

                    //reset the attributes
                    xmlAttributes = new List<OpenXmlAttribute>();
                    // this is the data type ("t"), with CellValues.String ("str")
                    // you might need to change this depending on your source data
                    // you might also consider using the Shared Strings table instead
                    xmlAttributes.Add(new OpenXmlAttribute("t", null, "str"));

                    //add the cell reference (A1, B1, A2... etc)
                    xmlAttributes.Add(new OpenXmlAttribute("r", null, "column"));

                    //write the start of the cell element with the type and cell reference attributes
                    writer.WriteStartElement(new Cell(), xmlAttributes);

                    //write the cell value
                    writer.WriteElement(new CellValue(cellValue));

                    //write the cell end element
                    writer.WriteEndElement();
                }

                //write the row end element
                writer.WriteEndElement();

                rowNumber++;
            }

            //write the sheetdata end element
            writer.WriteEndElement();
            //write the worksheet end element
            writer.WriteEndElement(); 
            writer.Close();

               



            //create a writer for the workbookpart
            writer = OpenXmlWriter.Create(myDoc.WorkbookPart);
            //write the start element of a workbook to the workbook part
            writer.WriteStartElement(new Workbook());
            //write the start element of a sheets item to the workbook part
            writer.WriteStartElement(new Sheets());
            //write the whole element of a sheet to the workbook part
            //note we link it to the id of the worksheetpart populated above
            writer.WriteElement(new Sheet()
            {
                Name = dt.TableName,
                SheetId = worksheetNumber,
                Id = myDoc.WorkbookPart.GetIdOfPart(worksheetpart)
            });


            }//foreach


            //write the sheets end element
            writer.WriteEndElement();
            //write the workbook end element
            writer.WriteEndElement();
            writer.Close();


        }


        private static void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet)
        {
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //  The following line of code prevents crashes in Excel 2010
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            //Stylesheet stylesheet = new Stylesheet();
            workbookStylesPart.Stylesheet = CreateStylesheet();
            workbookStylesPart.Stylesheet.Save();            

            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                string workSheetID = "rId" + worksheetNumber.ToString();
                string worksheetName = dt.TableName;

                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();

                // create sheet data
                newWorksheetPart.Worksheet.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.SheetData());

                // save worksheet
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                //newWorksheetPart.Worksheet.Save(); 

                // create the worksheet to workbook relation
                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                spreadsheet.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>().AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = (uint)worksheetNumber,
                    Name = dt.TableName
                });

                worksheetNumber++;
                dt.Clear();
            }

            spreadsheet.WorkbookPart.Workbook.Save();
        }


        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            string cellValue = "";

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;

            var headerRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(headerRow);

            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];
                AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, headerRow, true);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");
            }

            //
            //  Now, step through each row of data in our DataTable...
            //
            double cellNumericValue = 0;
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();

                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                        }
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, false);
                    }
                }
            }
        }

        private static void AppendTextCell(string cellReference, string cellStringValue, Row excelRow, bool isHeaderRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = null;
            if (isHeaderRow)
            {
                cell = new Cell() { CellReference = cellReference, DataType = CellValues.String, StyleIndex = 4 };
            }
            else
            {
                cell = new Cell() { CellReference = cellReference, DataType = CellValues.String };
            }
            
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static void AppendNumericCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }

        private static Stylesheet CreateStylesheet()
        {             
            Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = 
            new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            Fonts fonts1 = new Fonts() { Count = (UInt32Value)1U, KnownFonts
             = true };
            //Normal Font
            DocumentFormat.OpenXml.Spreadsheet.Font font1 = 
            new DocumentFormat.OpenXml.Spreadsheet.Font();
            DocumentFormat.OpenXml.Spreadsheet.FontSize fontSize1 = 
            new DocumentFormat.OpenXml.Spreadsheet.FontSize(){ Val = 11D };
            DocumentFormat.OpenXml.Spreadsheet.Color color1 = 
            new DocumentFormat.OpenXml.Spreadsheet.Color() 
            { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = 
            new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme1 = new FontScheme() 
            { Val = FontSchemeValues.Minor };

            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);
            fonts1.Append(font1);

            //Bold Font
            DocumentFormat.OpenXml.Spreadsheet.Font bFont = 
            new DocumentFormat.OpenXml.Spreadsheet.Font();                     
            DocumentFormat.OpenXml.Spreadsheet.FontSize bfontSize = 
            new DocumentFormat.OpenXml.Spreadsheet.FontSize(){ Val = 11D };
            DocumentFormat.OpenXml.Spreadsheet.Color bcolor = 
            new DocumentFormat.OpenXml.Spreadsheet.Color()
            { Theme = (UInt32Value)1U };
            FontName bfontName = new FontName() { Val = "Calibri" };
            FontFamilyNumbering bfontFamilyNumbering = 
            new FontFamilyNumbering() { Val = 2 };
            FontScheme bfontScheme = new FontScheme() 
            { Val = FontSchemeValues.Minor };
            Bold bFontBold = new Bold();

            bFont.Append(bfontSize);
            bFont.Append(bcolor);
            bFont.Append(bfontName);    
            bFont.Append(bfontFamilyNumbering);
            bFont.Append(bfontScheme);
            bFont.Append(bFontBold);

            fonts1.Append(bFont);

            Fills fills1 = new Fills() { Count = (UInt32Value)5U };

            // FillId = 0
            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() 
            { PatternType = PatternValues.None };
            fill1.Append(patternFill1);

            // FillId = 1
            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() 
            { PatternType = PatternValues.Gray125 };             
            fill2.Append(patternFill2);

            // FillId = 2,RED             
            Fill fill3 = new Fill();             
            PatternFill patternFill3 = new PatternFill() 
            { PatternType = PatternValues.Solid };             
            ForegroundColor foregroundColor1 = new ForegroundColor() 
            { Rgb = "FFB6C1" };             
            BackgroundColor backgroundColor1 = new BackgroundColor() 
            { Indexed = (UInt32Value)64U };             
            patternFill3.Append(foregroundColor1);             
            patternFill3.Append(backgroundColor1);             
            fill3.Append(patternFill3);

            // FillId = 3,GREEN             
            Fill fill4 = new Fill();             
            PatternFill patternFill4 = new PatternFill() 
            { PatternType = PatternValues.Solid };             
            ForegroundColor foregroundColor2 = new ForegroundColor() 
            { Rgb = "90EE90" };             
            BackgroundColor backgroundColor2 = new BackgroundColor() 
            { Indexed = (UInt32Value)64U };             
            patternFill4.Append(foregroundColor2);             
            patternFill4.Append(backgroundColor2);             
            fill4.Append(patternFill4);

            // FillId = 4,YELLO             
            Fill fill5 = new Fill();             
            PatternFill patternFill5 = new PatternFill() 
            { PatternType = PatternValues.Solid };             
            ForegroundColor foregroundColor3 = new ForegroundColor() 
            { Rgb = "FFFF00" };             
            BackgroundColor backgroundColor3 = new BackgroundColor() 
            { Indexed = (UInt32Value)64U };             
            patternFill5.Append(foregroundColor3);             
            patternFill5.Append(backgroundColor3);             
            fill5.Append(patternFill5);

            // FillId = 5,RED and BOLD Text             
            Fill fill6 = new Fill();             
            PatternFill patternFill6 = new PatternFill() 
            { PatternType = PatternValues.Solid };             
            ForegroundColor foregroundColor4 = new ForegroundColor() 
            { Rgb = "FFB6C1" };             
            BackgroundColor backgroundColor4 = new BackgroundColor() 
            { Indexed = (UInt32Value)64U };             
            Bold bold1 = new Bold();             
            patternFill6.Append(foregroundColor4);             
            patternFill6.Append(backgroundColor4);             
            fill6.Append(patternFill6);

            fills1.Append(fill1);             
            fills1.Append(fill2);             
            fills1.Append(fill3);             
            fills1.Append(fill4);             
            fills1.Append(fill5);             
            fills1.Append(fill6);

            Borders borders1 = new Borders() { Count = (UInt32Value)1U };

            Border border1 = new Border();             
            LeftBorder leftBorder1 = new LeftBorder();             
            RightBorder rightBorder1 = new RightBorder();            
            TopBorder topBorder1 = new TopBorder();             
            BottomBorder bottomBorder1 = new BottomBorder();             
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();

            border1.Append(leftBorder1);             
            border1.Append(rightBorder1);             
            border1.Append(topBorder1);             
            border1.Append(bottomBorder1);             
            border1.Append(diagonalBorder1);

            borders1.Append(border1);

            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() 
            { Count = (UInt32Value)1U };             
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, 
                FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

            cellStyleFormats1.Append(cellFormat1);

            CellFormats cellFormats1 = new CellFormats() { Count = 
            (UInt32Value)4U };             
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = 
            (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = 
            (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = 
            (UInt32Value)0U };             
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = 
            (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = 
            (UInt32Value)2U, BorderId = (UInt32Value)0U, FormatId = 
            (UInt32Value)0U, ApplyFill = true };             
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = 
            (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = 
            (UInt32Value)3U, BorderId = (UInt32Value)0U, FormatId = 
            (UInt32Value)0U, ApplyFill = true };             
            CellFormat cellFormat5 = new CellFormat() { NumberFormatId = 
            (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = 
            (UInt32Value)4U, BorderId = (UInt32Value)0U, FormatId = 
            (UInt32Value)0U, ApplyFill = true };             
            CellFormat cellFormat6 = new CellFormat() { NumberFormatId = 
            (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = 
            (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = 
            (UInt32Value)0U, ApplyFill = true };             
            CellFormat cellFormat7 = new CellFormat() { NumberFormatId = 
            (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = 
            (UInt32Value)3U, BorderId = (UInt32Value)0U, FormatId = 
            (UInt32Value)0U, ApplyFill = true };             
            CellFormat cellFormat8 = new CellFormat() { NumberFormatId = 
            (UInt32Value)0U, FontId = (UInt32Value)1U, FillId =     
            (UInt32Value)4U, BorderId = (UInt32Value)0U, FormatId = 
            (UInt32Value)0U, ApplyFill = true };

            cellFormats1.Append(cellFormat2);             
            cellFormats1.Append(cellFormat3);             
            cellFormats1.Append(cellFormat4);             
            cellFormats1.Append(cellFormat5);             
            cellFormats1.Append(cellFormat6);             
            cellFormats1.Append(cellFormat7);             
            cellFormats1.Append(cellFormat8);

            CellStyles cellStyles1 = new CellStyles() 
            { Count = (UInt32Value)1U };             
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", 
            FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles1.Append(cellStyle1);             
            DifferentialFormats differentialFormats1 = 
            new DifferentialFormats() { Count = (UInt32Value)0U };             
            TableStyles tableStyles1 = new TableStyles() 
            { Count = (UInt32Value)0U, DefaultTableStyle = 
            "TableStyleMedium2", DefaultPivotStyle = "PivotStyleMedium9" };

            stylesheet1.Append(fonts1);             
            stylesheet1.Append(fills1);             
            stylesheet1.Append(borders1);             
            stylesheet1.Append(cellStyleFormats1);             
            stylesheet1.Append(cellFormats1);             
            stylesheet1.Append(cellStyles1);             
            stylesheet1.Append(differentialFormats1);             
            stylesheet1.Append(tableStyles1);

            return stylesheet1;         
        }

    }
}
