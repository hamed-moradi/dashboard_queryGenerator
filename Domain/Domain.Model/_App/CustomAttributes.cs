using System;

namespace Domain.Model._App
{
    public class InsertField : Attribute {}

    public class InsertMandatoryField : Attribute {}

    public class UpdateField : Attribute {}

    public class UpdateMandatoryField : Attribute {}

    public class HelperField : Attribute {}

    public class RelatedField : Attribute
    {
        public string EntityTitle { get; set; }
        public string PrimaryTitle { get; set; }
        public string ForignKey { get; set; }
        public bool Mandatory { get; set; }

        public RelatedField(string entityTitle, string primaryTitle, string forignKey, bool mandatory = false)
        {
            EntityTitle = entityTitle;
            PrimaryTitle = primaryTitle;
            ForignKey = forignKey;
            Mandatory = mandatory;
        }
    }
}