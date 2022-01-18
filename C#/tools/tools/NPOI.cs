using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI;
using NPOI.HPSF.Extractor;
using tools;
//* HSSF使用於2007之前的xls版本，XSSF適用於2007及其之後的xlsx版本。 *//
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;

namespace tools
{
    class Page
    {
        public string Name { get; set; }
        public int Order { get; set; }
    }

    class NPOI
    {
        public NPOI() {
            NPOIprintf();
            //NPOIRead();
        }

        public void NPOIprintf()
        {
            //自訂 Excel 工作分頁
            List<Page> pages = new()
            {
                new Page { Name = "Sheet1", Order = 1 },
                new Page { Name = "Sheet2", Order = 2 },
                new Page { Name = "Sheet3", Order = 3 },
            };

            //建立分頁
            //HSSFWorkbook workbook2003 = HSSFCreateSheet(pages);
            XSSFWorkbook workbook2007 = XSSFCreateSheet(pages);
            SetXSSF_xlsx(workbook2007);

            //寫入檔案
            //Write(workbook2003, @"F:\NPOI_Excel\Excel2003.xls");
            Write(workbook2007, @"F:\NPOI_Excel\Excel2007.xlsx");
        }

        public void NPOIRead()
        {
            ISheet sheet = ReadSheetAt(@"F:\NPOI_Excel\Excel2007.xlsx", 0);  //獲取第一個工作表
            for (int i = 0; i <= sheet.LastRowNum; i++)  //對工作表每一行
            {
                if (sheet.GetRow(i) == null) { continue; }
                foreach (ICell cell in sheet.GetRow(i))
                {
                    Console.WriteLine(cell);
                }
            }
        }

        private static ISheet ReadSheetAt(string fileName, int index)
        {
            FileStream fileStream = new(fileName, FileMode.Open, FileAccess.Read);

            IWorkbook workbook = fileName.IndexOf(".xls") > 0 
                ? new HSSFWorkbook(fileStream)// 2003版本 xls資料讀入workbook
                : new XSSFWorkbook(fileStream);// 2007版本xlsx資料讀入workbook

            fileStream.Close();
            return workbook.GetSheetAt(index);
        }

        private static void SetXSSF_xlsx(XSSFWorkbook Workbook)
        {
            int SIZE = 8;
            int index = 0;
            XSSFCell[] SheetCell = new XSSFCell[SIZE];
            while (index < Workbook.NumberOfSheets)
            {
                string sheet = Workbook.GetSheetName(index);
                XSSFSheet _ROW = (XSSFSheet)Workbook.GetSheet(sheet);
                //對工作表先新增行，下標從0開始
                for (int i = 0; i < SIZE; i++)
                {
                    _ROW.CreateRow(i);//建立8行
                }

                //對每一行建立10個單元格
                XSSFRow SheetRow = (XSSFRow)_ROW.GetRow(0);  //獲取Sheet1工作表的首行
                for (int i = 0; i < SIZE; i++)
                {
                    SheetCell[i] = (XSSFCell)SheetRow.CreateCell(i);  //為第一行建立10個單元格
                }

                //建立值
                for (int i = 1; i < SIZE; i++)
                {
                    SheetCell[i].SetCellValue(i);    //迴圈賦值為整形
                }

                index++;
            }

            //隱藏最後一個工作表
            Workbook.SetSheetHidden(Workbook.NumberOfSheets -1 , true);
        }

        public static void SetCellDropdownList(ISheet sheet, int firstcol, int lastcol, string[] vals)
        {
            //設定生成下拉框的行和列
            var cellRegions = new CellRangeAddressList(1, 65535, firstcol, lastcol);

            //設定 下拉框內容
            DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(vals);

            //繫結下拉框和作用區域，並設定錯誤提示資訊
            HSSFDataValidation dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("輸入不合法", "請輸入或選擇下拉列表中的值。");
            dataValidate.ShowPromptBox = true;

            sheet.AddValidationData(dataValidate);
        }

        /// <summary>
        /// 創建xlsx excel 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        private static XSSFWorkbook XSSFCreateSheet(List<Page> datas)
        {
            //如果是空值，直接剔除執行
            if (datas == null) { return null; }
            //設定 HSSF
            XSSFWorkbook _XSSF = new();
            //LINQ 排序
            IEnumerable<Page> pages = datas.OrderBy(row => row.Order);
            //設定分頁
            foreach (Page result in pages)
            {
                _XSSF.CreateSheet(result.Name);
            }
            //回傳 HSSFWorkbook
            return _XSSF;
        }

        private static HSSFWorkbook HSSFCreateSheet(List<Page> datas)
        {
            //如果是空值，直接剔除執行
            if (datas == null) { return null; }
            //設定 HSSF
            HSSFWorkbook _HSSF = new();
            //LINQ 排序
            IEnumerable<Page> pages = datas.OrderBy(row => row.Order);
            //設定分頁
            foreach (Page result in pages)
            {
                _HSSF.CreateSheet(result.Name);
            }
            //回傳 HSSFWorkbook
            return _HSSF;
        }

        /// <summary>
        /// 新增excel
        /// </summary>
        /// <param name="HSSF"></param>
        /// <param name="FilePath"></param>
        /// <param name="Data"></param>
        private static void Write(HSSFWorkbook HSSF, string FilePath, string Data = "")
        {
            FileStream file = new(FilePath, FileMode.Create);
            HSSF.Write(file);
            file.Close();  //關閉檔案流
            HSSF.Close();
        }

        private static void Write(XSSFWorkbook XSSF, string FilePath, string Data = "")
        {
            FileStream file = new(FilePath, FileMode.Create);
            XSSF.Write(file);
            file.Close();  //關閉檔案流
            XSSF.Close();
        }
    }
}
