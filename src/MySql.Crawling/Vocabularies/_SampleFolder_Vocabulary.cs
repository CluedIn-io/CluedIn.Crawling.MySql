using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.Crawling.MySql.Vocabularies
{
    public class _SampleFolder_Vocabulary : SimpleVocabulary
    {
        public _SampleFolder_Vocabulary()
        {
            VocabularyName = "MySql [SampleFolder]"; // TODO: Set value
            KeyPrefix = "mysql.[samplefolder]"; // TODO: Set value
            KeySeparator = ".";
            Grouping = EntityType.Unknown; // TODO: Set value

            AddGroup("MySql Details", group =>
            {
                Id = group.Add(new VocabularyKey("Id", VocabularyKeyDataType.Text, VocabularyKeyVisiblity.Visible));
                Name = group.Add(new VocabularyKey("Name", VocabularyKeyDataType.Text, VocabularyKeyVisiblity.Visible));
            });

            // Mappings
            //AddMapping(this.City,          CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCity);
        }

        public VocabularyKey Id { get; private set; }
        public VocabularyKey Name { get; private set; }
    }
}
