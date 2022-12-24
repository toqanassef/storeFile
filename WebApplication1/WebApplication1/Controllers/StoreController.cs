using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Model model)
        {
            
            try
            {
                // Upload file
                var path = await UploadFile(model.File);

                // get data from file
                var data = GetObjectsFromExcel(path);

                // filter data by data and good Id
                var Gooddata = data.Where(a => a.GoodId == model.GoodId && a.TransactionDate >= model.FromDate && a.TransactionDate <= model.ToDate).ToList();

                // return data (List of trans & total and remain amount)
                // total amount = all transaction in and out
                // remain amount = out trans - In trans (if <0 return 0 -- no remain)
                var RetDataDto = new RetData()
                {
                    TransactionNum = Gooddata.Count(),
                    AmountTotal = Gooddata.Sum(a => a.Amount),
                    AmountRemain = Gooddata.Where(a => a.Direction == "Out").Sum(a => a.Amount) - Gooddata.Where(a => a.Direction == "In").Sum(a => a.Amount),
                    GoodData = Gooddata
                };
                RetDataDto.AmountRemain = RetDataDto.AmountRemain < 0 ? 0 : RetDataDto.AmountRemain;

                return Ok(RetDataDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        private async Task<string> UploadFile(IFormFile File)
        {
            try
            {
                //Get file name to create path
                string SafeFileName = File.FileName;
                SafeFileName = SafeFileName.Replace(" ", "_");

                var path = Environment.CurrentDirectory + @"\Files\Files" + DateTime.Today.ToString("yyyyMMdd") + SafeFileName;

                // delete file if exist with same name
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                // create file
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    await File.CopyToAsync(fs);
                }
                return path;
            }
            catch
            {
                throw;
            }
        }

        private List<GoodDataDto> GetObjectsFromExcel(string FilePath)
        {
            List<GoodDataDto> listofData = new List<GoodDataDto>();

            
            using (TextFieldParser csvReader = new TextFieldParser(FilePath))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = true;

                // read data from file by row
                while (!csvReader.EndOfData)
                {
                    var row = csvReader.ReadFields().FirstOrDefault();
                    var rowData = row.Split(';');  // as in file ( all in one col split by ;)
                    var Data = new GoodDataDto();

                    //check Validation
                    // 6 row and no null except comment (the last field)
                    if(rowData.Count() == 6 && !rowData[0..5].Contains(""))
                    {
                        try
                        {
                            Data.GoodId = int.Parse(rowData[0]);
                            Data.TransactionId = int.Parse(rowData[1]);
                            Data.TransactionDate = DateTime.Parse(rowData[2]);
                            Data.Amount = int.Parse(rowData[3]);
                            Data.Direction = rowData[4];
                            Data.Comment = rowData[5];

                            // add if valid
                            listofData.Add(Data);
                        }
                        catch // if can't parse GoodId or Date 
                        {
                            continue;
                        }
                        
                    }                   
                    
                }
            }
            return listofData;
        }
    }


}
