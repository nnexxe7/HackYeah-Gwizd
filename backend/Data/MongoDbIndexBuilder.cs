using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GWIZD.Core.Data;

public class MongoDbIndexBuilder : IMongoDbIndexBuilder
{
	private readonly ILog<MongoDbIndexBuilder> _logger;
	private readonly HashSet<string> _processedCollectionNames = new HashSet<string>();


	public MongoDbIndexBuilder(ILog<MongoDbIndexBuilder> logger)
	{
		_logger = logger;
	}

	public void ProcessIndexes<T>(IMongoCollection<T> collection)
	{
		lock (_processedCollectionNames)
		{
			if (_processedCollectionNames.Contains(collection.CollectionNamespace.CollectionName)) return;

			try
			{
				Type baseType = typeof(T);
				ProcessIndexes(collection, baseType, string.Empty, new HashSet<Type>());
				_processedCollectionNames.Add(collection.CollectionNamespace.CollectionName);
			}
			catch (Exception e)
			{
				_logger.Error(
					$"Couldn't build all indexes for collection {collection.CollectionNamespace.CollectionName} in database {collection.Database.DatabaseNamespace.DatabaseName}",
					e);
			}
		}
	}

	private void ProcessIndexes<T>(IMongoCollection<T> collection, Type docType, string docPrefix, HashSet<Type> processedTypes)
	{
		if (docPrefix == null) throw new ArgumentNullException(nameof(docPrefix));

		if (processedTypes.Contains(docType)) return;

		processedTypes.Add(docType);

		foreach (PropertyInfo property in docType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
		{
			if (property.Name == "Id") continue;

			if (IsTypeIndexable(property.PropertyType))
			{
				var fieldIndex = property.GetCustomAttributes<FieldIndexAttribute>().SingleOrDefault();
				if (fieldIndex != null)
				{
					IndexKeysDefinitionBuilder<T> ikb = Builders<T>.IndexKeys;
					IndexKeysDefinition<T> keyDefinition = ikb.Ascending(property.Name);

					var indexOptions = new CreateIndexOptions<T>
					{
						Background = true,
						Name = docPrefix + property.Name
					};
					if (fieldIndex.CaseInsensitive)
					{
						indexOptions.Collation = new Collation("en", strength: new Optional<CollationStrength?>(CollationStrength.Secondary));
					}

					if (fieldIndex.Unique) indexOptions.Unique = true;

					collection.Indexes.CreateOne(new CreateIndexModel<T>(keyDefinition, indexOptions));
				}
			}
			else if (IsTypeSubdocument(property.PropertyType))
			{
				ProcessIndexes(collection, property.PropertyType, docPrefix + property.Name + ".", processedTypes);
			}
		}
	}

	private bool IsTypeIndexable(Type t)
	{
		return t.IsPrimitive || t.IsEnum
		                     || t == typeof(string)
		                     || t == typeof(DateTime)
		                     || t == typeof(Guid)
		                     || t == typeof(ObjectId);
	}

	private bool IsTypeSubdocument(Type t)
	{
		return t.IsClass && t.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length > 0
		                 && !t.Name.StartsWith("System.");
	}
}