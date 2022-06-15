        public async Task<List<CampaignCode>> AddCodeFromExcel(IFormFile file)
        {
            var list = new List<CampaignCode>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;

                    for (int row = 1; row < rowcount; row+=2 )
                    {
                       
                        list.Add(new CampaignCode
                        {
                            Code = worksheet.Cells[row, 1].Value == null ? string.Empty : worksheet.Cells[row, 1].Value.ToString(),
                            Discount = worksheet.Cells[row, 2].Value == null ? string.Empty : worksheet.Cells[row, 2].Value.ToString(),
                           
                        });
                    }

                    foreach (var item in list)

                    {
                        CampaignCode code = new CampaignCode();
                        code.Code = item.Code;
                        code.IsUsed = false;
                        code.Discount = item.Discount + " TL";
                        //code.Date = DateTime.Now;

                        try
                        {
                            if(_db.CampaignCode.FirstOrDefault(p=>p.Code == code.Code) == null)
                            {

                                _db.CampaignCode.Add(code);
                                _db.SaveChanges();
                            }


                        }
                        catch
                        {

                        }

                    }
                }

                return list;
            }

        }
