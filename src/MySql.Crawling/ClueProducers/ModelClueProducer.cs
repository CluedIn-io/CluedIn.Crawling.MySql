using System;
using System.Linq;
using CluedIn.Core.Data;
using CluedIn.Crawling.MySql.Core.Models;
using CluedIn.Crawling.Factories;
using CluedIn.Core;
using CluedIn.Crawling.MySql.Vocabularies;

namespace CluedIn.Crawling.MySql.ClueProducers
{
    public class ModelClueProducer : BaseClueProducer<Model>
    {
        private readonly IClueFactory _factory;

        public ModelClueProducer([NotNull] IClueFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _factory = factory;
        }

        protected override Clue MakeClueImpl([NotNull] Model input, Guid accountId)
        {

            if (input == null) throw new ArgumentNullException(nameof(input));

            // TODO: Create clue specifying the type of entity it is and ID            
            var clue = _factory.Create(input.TableMapping.EntityType, input.Columns.ElementAt(input.TableMapping.Columns.FindIndex(c => c.IsId)).ToString(), accountId);

            // TODO: Populate clue data
            var data = clue.Data.EntityData;

            var vocab = new ModelVocabulary();

            if (input.TableMapping.Columns.Any(c => c.CluedInFieldMapping == "Name"))
            {
                data.Name = input.Columns.ElementAt(input.TableMapping.Columns.FindIndex(c => c.CluedInFieldMapping == "Name")).ToString();
            }
            if (input.TableMapping.Columns.Any(c => c.CluedInFieldMapping == "DisplayName"))
            {
                data.DisplayName = input.Columns.ElementAt(input.TableMapping.Columns.FindIndex(c => c.CluedInFieldMapping == "DisplayName")).ToString();
            }
            if (input.TableMapping.Columns.Any(c => c.CluedInFieldMapping == "Url"))
            {
                Uri url;

                if (Uri.TryCreate(input.Columns.ElementAt(input.TableMapping.Columns.FindIndex(c => c.CluedInFieldMapping == "Url")).ToString(), UriKind.Absolute, out url))
                {
                    data.Uri = url;
                }
            }
            if (input.TableMapping.Columns.Any(c => c.CluedInFieldMapping == "CreatedDate"))
            {
                DateTimeOffset createdDate;

                if (DateTimeOffset.TryParse(input.Columns.ElementAt(input.TableMapping.Columns.FindIndex(c => c.CluedInFieldMapping == "CreatedDate")).ToString(), out createdDate))
                {
                    data.CreatedDate = createdDate;
                }
            }
            if (input.TableMapping.Columns.Any(c => c.CluedInFieldMapping == "ModifiedDate"))
            {
                DateTimeOffset modifiedDate;

                if (DateTimeOffset.TryParse(input.Columns.ElementAt(input.TableMapping.Columns.FindIndex(c => c.CluedInFieldMapping == "ModifiedDate")).ToString(), out modifiedDate))
                {
                    data.ModifiedDate = modifiedDate;
                }
            }

            for (int i = 0; i <= input.Columns.Count; i++)
            {
                data.Properties[string.Format("sqlServer.{1}.custom-{0}", input.TableMapping.Columns[i], input.TableMapping.EntityType.ToString().ToLowerInvariant().Replace("//", string.Empty))] = input.Columns[i].ToString();
            }

            foreach (var reference in input.TableMapping.Keys)
            {
                _factory.CreateOutgoingEntityReference(clue, reference.Type, reference.EdgeType, reference, input.Columns.ElementAt(input.TableMapping.Columns.FindIndex(c => c.CluedInFieldMapping == reference.FieldSource)).ToString());
            }

            return clue;
        }
    }

}
