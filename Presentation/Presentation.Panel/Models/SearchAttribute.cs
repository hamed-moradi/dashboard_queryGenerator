using Asset.Infrastructure.Common;
using System;
using System.Web.ModelBinding;


namespace Presentation.Panel.Models
{
    public class SearchAttribute : Attribute, IMetadataAware
    {
        public SearchAttribute(GeneralEnums.SearchFieldType type = GeneralEnums.SearchFieldType.String)
        {
            Type = type;
        }

        public GeneralEnums.SearchFieldType Type { get; }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["Type"] = Type;
        }
    }
}