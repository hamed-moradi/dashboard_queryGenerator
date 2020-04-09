using Asset.Infrastructure._App;
using Domain.Application;
using Domain.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.OleDb;
using System.Linq;

namespace Tests.Domain
{
    [TestClass]
    public class ProviderUnitTest
    {
        private readonly IBusinessService _businessService;
        public ProviderUnitTest()
        {
            _businessService = ServiceLocatorAdapter.Current.GetInstance<IBusinessService>();
        }

        [TestMethod]
        public void ReadExcelFile()
        {
            var rand = new Random();
            for (int i = 1; i <= 6; i++)
            {
                string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\m.hosseini\Desktop\smartcut excel files\district " + i + ".xls;Extended Properties='Excel 8.0;HDR=Yes;'";
                using (OleDbConnection connection = new OleDbConnection(con))
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                    using (OleDbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var phone = dr["phone"].ToString();
                            var statusCol = dr["status"].ToString();
                            var genderCol = dr["gender"].ToString();
                            var status = ((statusCol != null && statusCol.Trim() == "درست است") ? (byte)1 : (byte)0);
                            var genderTypeId = (byte)3;
                            if (genderCol != null)
                            {
                                genderCol = genderCol.Trim().ToLower();
                                if (genderCol == "z")
                                {
                                    genderTypeId = 2;
                                }
                                if (genderCol == "m")
                                {
                                    genderTypeId = 1;
                                }
                            }

                            var imageCounter = rand.Next(1, 21);
                            long total;
                            var businesses = _businessService.GetPaging(new Business { PhoneNo = phone }, out total).ToList();
                            foreach (var item in businesses)
                            {
                                item.Status = status;
                                item.GenderTypeId = genderTypeId;
                                item.Avatar = "/Content/Business/Zibasho/" + imageCounter + ".jpg";
                                //_businessService.Update(item);
                            }
                        }
                    }
                }
            }

        }
    }
}
