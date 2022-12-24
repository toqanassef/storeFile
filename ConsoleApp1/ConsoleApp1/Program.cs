using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ConsoleApp1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string fileName;
            

            do
            {
                Console.Write("press any key to choose csv file");
                Console.ReadLine();

                OpenFileDialog fd = new OpenFileDialog();
                fd.Filter ="csv file (*.csv)|*.csv";
                fd.ShowDialog();
                fileName = fd.FileName;
                if (string.IsNullOrEmpty(fileName))
                {
                    Console.Write(" -- please choose file \n");
                }
                else if (!fileName.Contains(".csv"))
                {
                    Console.Write(" -- please choose csv file \n");
                }
                else
                    break;
            } while (true);


            Console.Write(fileName);
            Console.Write("\n");


            //string csv_file_path = @"C:\Users\Toqa\Downloads\Store Data (2) (1).csv";

            var FolderPath = GetDataFromCSVFile(fileName);

            Console.WriteLine("Files Location:   " + FolderPath);

            Console.ReadLine();
        }

        private static string GetDataFromCSVFile(string csv_file_path)
        {

            try
            {
                var FolderPath = GetFoulderPath();
                if (string.IsNullOrEmpty(FolderPath))
                    return null;


                // create error file
                string Errorpath = FolderPath + @"\ErrorFile.csv" ;
                File.WriteAllText(Errorpath, "ErrorFile" + Environment.NewLine + Environment.NewLine);   


                // read file data
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //string[] colFields = csvReader.ReadFields();

                    // read file data row by row
                    while (!csvReader.EndOfData)
                    {
                        var row = csvReader.ReadFields().FirstOrDefault();
                        //split the row by ; as in file
                        var rowData = row.Split(';');

                        // check for error in file (must be 6 fields no null except comment the last field)
                        // if not valid write it in error file
                        if (rowData.Count() < 6 ||rowData[0..5].Contains("") )
                        {
                            File.AppendAllText(Errorpath, row + Environment.NewLine);
                        }
                        else // valid data
                        {
                            var GoodIdStr = rowData[0];
                            var GoodId = 0;

                            var res = int.TryParse(GoodIdStr,out GoodId);

                            if (res)
                            {
                                /*
                                 * file path : foulderpath + GoodId
                                 * create file if first mention to goodId so file not exist
                                 * if exist write row in it
                                 */
                                var filePath = Path.Combine(FolderPath, GoodIdStr + ".csv");
                                if (!File.Exists(filePath))
                                {
                                    // Create a file to write to.
                                    string createText = "Good " + GoodId + Environment.NewLine + Environment.NewLine;
                                    File.WriteAllText(filePath, createText);

                                    File.AppendAllText(filePath, row + Environment.NewLine);
                                }
                                else
                                {
                                    File.AppendAllText(filePath, row + Environment.NewLine);
                                }
                            }
                            else // goodId not int so not valid
                            {
                                File.AppendAllText(Errorpath, row + Environment.NewLine);
                            }
                        }
                    }
                }
                return FolderPath;
            }
            catch (Exception ex)
            {
                Console.Write(" error happened \n");
                Console.Write(ex.Message);
                Console.Write(" \n");
                return null;
            }
        }

        private static string GetFoulderPath()
        {
            try
            {
                var FolderPathBase = Environment.CurrentDirectory + @"\Files" + DateTime.Today.ToString("yyyyMMdd");
                var i = 1;
                var FolderPath = FolderPathBase;
                do
                {
                    if (!Directory.Exists(FolderPath))
                    {
                        Directory.CreateDirectory(FolderPath);
                        break;
                    }
                    else
                    {
                        FolderPath = FolderPathBase + "_" + i.ToString();
                        i++;
                    }
                } while (true);

                return FolderPath;
            }
            catch(Exception ex)
            {
                Console.Write(" error happened in create foulder \n");
                Console.Write(ex.Message);
                Console.Write(" \n");
                return null;
            }
        }
    }
}
