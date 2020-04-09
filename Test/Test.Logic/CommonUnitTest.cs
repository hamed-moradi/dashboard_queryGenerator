using System;
using System.Collections.Generic;
using Asset.Infrastructure._App;
using Domain.Model.Entities;
using Domain.Application.Repository;
using Domain.Application;
using Domain.Application._App;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Linq;
using Domain.Model._App;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Net;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Types;
using System.Text;
using System.Collections;

namespace Tests.Logic
{
    public class Testing { }

    [TestClass]
    public class CommonUnitTest
    {
        private readonly IRepository<Content2Tag> _repository;
        private readonly IBusinessService _businessService;
        private readonly IFacilityService _facilityService;
        private readonly IContentService _contentService;
        private readonly IContentAttachmentService _contentAttachmentService;
        public CommonUnitTest()
        {
            _repository = ServiceLocatorAdapter.Current.GetInstance<IRepository<Content2Tag>>();
            _businessService = ServiceLocatorAdapter.Current.GetInstance<IBusinessService>();
            _facilityService = ServiceLocatorAdapter.Current.GetInstance<IFacilityService>();
            _contentService = ServiceLocatorAdapter.Current.GetInstance<IContentService>();
            _contentAttachmentService = ServiceLocatorAdapter.Current.GetInstance<IContentAttachmentService>();
        }

        [TestMethod]
        public void NewGetbyId()
        {
            var result = _businessService.GetById(1112, false);
            Console.WriteLine(result);
        }

        [TestMethod]
        public void GetById()
        {
            var provider = _businessService.GetById(1114);
            var property = _facilityService.GetById(1);
        }

        [TestMethod]
        public void StringQueryGenerator()
        {
            var selectResult = QueryGenerator<Admin>.Select(new Admin { Username = "asdf", RoleId = 1 });
            Assert.IsNotNull(selectResult);

            var insertResult = QueryGenerator<Admin>.Insert(new Admin { Username = "asdf" });
            Assert.IsNotNull(insertResult);

            var updateResult = QueryGenerator<Admin>.Update(new Admin { FirstName = "asdf" });
            Assert.IsNotNull(updateResult);
        }

        [TestMethod]
        public void BulkInsertQueryGenerator()
        {
            var model = new List<Content2Tag> { new Content2Tag { ContentId = 1, TagId = 1111 }, new Content2Tag { ContentId = 1, TagId = 1111 } };
            var result = QueryGenerator<Content2Tag>.BulkInsert(model);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void BulkInsertTest()
        {
            var model = new List<Content2Tag> { new Content2Tag { ContentId = 1, TagId = 1111 }, new Content2Tag { ContentId = 1, TagId = 1111 } };
            var result = _repository.BulkInsert(model);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void GetTypeTest()
        {
            var model = Type.GetType($"Domain.Application.Entities.Admin, Domain.Application");
            var single = model.GetProperties().SingleOrDefault(prop => prop.Name.Equals("Status"));
        }

        [TestMethod]
        public void XmlDataType()
        {
            //var xmlText = XDocument.Parse("<xsd:schema targetNamespace=\"urn: schemas - microsoft - com:sql: SqlRowSet1\" xmlns:schema=\"urn: schemas - microsoft - com:sql: SqlRowSet1\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:sqltypes=\"http://schemas.microsoft.com/sqlserver/2004/sqltypes\" elementFormDefault=\"qualified\">" +
            //                              "<xsd:import namespace=\"http://schemas.microsoft.com/sqlserver/2004/sqltypes\" schemaLocation=\"http://schemas.microsoft.com/sqlserver/2004/sqltypes/sqltypes.xsd\" />" +
            //                              "<xsd:element name = \"b\" >" +
            //                                "< xsd:complexType>" +
            //                                  "<xsd:attribute name = \"Id\" type=\"sqltypes:int\" use=\"required\" />" +
            //                                  "<xsd:attribute name = \"Avatar\" >" +
            //                                    "< xsd:simpleType>" +
            //                                      "<xsd:restriction base=\"sqltypes:nvarchar\" sqltypes:localeId=\"1065\" sqltypes:sqlCompareOptions=\"IgnoreCase IgnoreNonSpace IgnoreKanaType IgnoreWidth\" sqltypes:sqlCollationVersion=\"2\">" +
            //                                        "<xsd:maxLength value = \"512\" />" +
            //                                      "</ xsd:restriction>" +
            //                                    "</xsd:simpleType>" +
            //                                  "</xsd:attribute>" +
            //                                "</xsd:complexType>" +
            //                              "</xsd:element>" +
            //                            "</xsd:schema>");
            var simple = XDocument.Parse("<b Id=\"1\" Avatar=\"/Content/400x400.png\" />");
            var xmlModel = new XmlSerializer(typeof(XmlBusiness), new XmlRootAttribute { ElementName = "b", IsNullable = true });
            var newModel = (XmlBusiness)xmlModel.Deserialize(simple.CreateReader());
        }

        [TestMethod]
        [TestCategory("Common")]
        public void HtmlString()
        {
            var htmlString1 = "<b>reza</b>";
            var htmlString2 = "<b />";

            var htmlEncode1 = HttpUtility.HtmlEncode(htmlString1);
            var htmlEncode2 = WebUtility.HtmlEncode(htmlString2);

            var htmlDecode1 = HttpUtility.HtmlDecode(htmlEncode1);
            var htmlDecode2 = WebUtility.HtmlDecode(htmlEncode2);
        }

        [TestMethod]
        public void SetPropValue()
        {
            var model = new Admin { };
            var properties = model.GetType().GetProperties();
            var columns = properties.Where(prop => !Attribute.IsDefined(prop, typeof(NotMappedAttribute)));
            foreach (var column in columns)
                if (column.Name == "CreatedAt")
                    column.SetValue(model, DateTime.Now, null);
            Assert.IsNotNull(model.CreatedAt);
        }


        [TestMethod]
        [TestCategory("Common")]
        public void FileRenamer()
        {
            var patern = "[^.a-zA-Z0-9]";
            var path = @"G:\Zibashu\";
            var dirInfo = new DirectoryInfo(path);
            var files = dirInfo.GetFiles("*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FullName);
                var corrected = Regex.Replace(fileName, patern, "_");
                fileName = $@"{Path.GetDirectoryName(file.FullName)}\{corrected}{Path.GetExtension(file.FullName)}";
                File.Move(file.FullName, fileName);
            }
            var folders = dirInfo.GetDirectories("*", SearchOption.AllDirectories);
            foreach (var folder in folders)
            {
                var folderName = folder.Name;
                if (folderName == "H+")
                {
                    folderName = folder.FullName.Replace("H+", "HD");
                }
                else
                {
                    folderName = folder.FullName.Replace(folderName, Regex.Replace(folderName, patern, "_"));
                }
                if (folder.FullName.ToLower() != folderName.ToLower())
                    Directory.Move(folder.FullName, folderName);
            }
        }

        [TestMethod]
        [TestCategory("Common")]
        public void DbRenamer()
        {
            var patern = "[^/.a-zA-Z0-9]";
            var contents = _contentService.GetPaging(new Content(), out long totalCount, take: int.MaxValue);
            foreach (var content in contents)
            {
                content.UpdaterId = 1116;
                if (!string.IsNullOrWhiteSpace(content.Photo))
                {
                    content.Photo = content.Photo.Replace("/H+/", "/HD/");
                    content.Photo = Regex.Replace(content.Photo, patern, "_");
                }
                if (!string.IsNullOrWhiteSpace(content.Thumbnail))
                {
                    content.Thumbnail = content.Thumbnail.Replace("/H+/", "/HD/");
                    content.Thumbnail = Regex.Replace(content.Thumbnail, patern, "_");
                }
                _contentService.Update(content);
            }
            var contentAttachments = _contentAttachmentService.GetPaging(new ContentAttachment(), out long rowsCount, take: int.MaxValue);
            foreach (var attachment in contentAttachments)
            {
                attachment.UpdaterId = 1116;
                if (!string.IsNullOrWhiteSpace(attachment.Path))
                {
                    attachment.Path = attachment.Path.Replace("/H+/", "/HD/");
                    attachment.Path = Regex.Replace(attachment.Path, patern, "_");
                }
                _contentAttachmentService.Update(attachment);
            }
        }
    }
}